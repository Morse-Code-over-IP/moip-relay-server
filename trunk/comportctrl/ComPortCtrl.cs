//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		ComPortCtrl.cs
//
// FACILITY:	Morse Code News Reader
//
// ABSTRACT:	The .NET SerialPort class is extremely sluggish in handling
//				the modem-control lines on the serial port. This class uses 
//				P/Invoke to talk directly to the kernel serial port driver to
//				accomplish the same thing with far more accuracy and less 
//				latency.
//
// ENVIRONMENT:	Microsoft.NET 2.0/3.5
//				Developed under Visual Studio.NET 2008
//
//
// AUTHOR:		Bob Denny, <rdenny@dc3.com>
//
// Edit Log:
//
// When			Who		What
//----------	---		-------------------------------------------------------
// 05-May-10	rbd		1.3.3 - Initial edit.
// 07-May-10	rbd		1.5.0 - Refactor into separate assy, make class public.
//						Major upgrade, add members for reading physical key
//						input.
// 18-May-10	rbd		1.6.0 - CreateFile on port may return -1 on error. Check
//						this as well as IntPtr.Zero and make it IntPtr.Zero which
//						is the sentinel used elsewhere.
// 19-May-10	rbd		1.6.0 - Lame attempt to detect changes on pins while 
//						handling previous change. Really lame!
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO.Ports;
using System.Threading;

namespace com.dc3
{
#if !MONO_BUILD
	public delegate void ComPortEventHandler(object sender, ComPortEventArgs e);

	public class ComPortEventArgs : EventArgs
	{
		public enum PinChange { DsrChanged, CtsChanged, NoPinsChanged }

		internal const UInt32 EV_CTS = 0x0008;
		internal const UInt32 EV_DSR = 0x0010;

		uint _pinBits = 0;

		public ComPortEventArgs(uint pinBits)
		{
			_pinBits = pinBits;
		}

		public PinChange EventType
		{
			get
			{
				if ((_pinBits & EV_CTS) != 0) return PinChange.CtsChanged;
				if ((_pinBits & EV_DSR) != 0) return PinChange.DsrChanged;
				return PinChange.NoPinsChanged;
			}
		}

	}

	public class ComPortCtrl : IDisposable
	{

#region P/Invoke Declarations
		// for EsacapeCommFunction()
		internal const UInt32 CF_SETRTS = 3;
		internal const UInt32 CF_CLRRTS = 4;
		internal const UInt32 CF_SETDTR = 5;
		internal const UInt32 CF_CLRDTR = 6;
		// For GetCommModemStatus()
		internal const UInt32 MS_CTSON = 0x0010;
		internal const UInt32 MS_DSRON = 0x0020;
		// For SetCommMask()
		internal const UInt32 EV_CTS = 0x0008;
		internal const UInt32 EV_DSR = 0x0010;
		// For CreateFile();
		internal const UInt32 GENERIC_READ = 0x80000000;
		internal const UInt32 GENERIC_WRITE = 0x40000000;
		internal const UInt32 OPEN_EXISTING = 3;
		// for CreateFile()dwFlagsAndAttributes
		internal const UInt32 FILE_ATTRIBUTE_NORMAL = 0x80;
		internal const UInt32 FILE_FLAG_OVERLAPPED = 0x40000000;
		// For WaitCommEvent
		internal const UInt32 ERROR_IO_PENDING = 997;

		[StructLayout(LayoutKind.Sequential)]
		internal struct OVERLAPPED
		{
			internal UIntPtr Internal;
			internal UIntPtr InternalHigh;
			internal UInt32 Offset;
			internal UInt32 OffsetHigh;
			internal IntPtr hEvent;
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern IntPtr CreateFile(
			string lpFileName,
			UInt32 dwDesiredAccess,
			UInt32 dwShareMode,
			IntPtr lpSecurityAttributes,
			UInt32 dwCreationDisposition,
			UInt32 dwFlagsAndAttributes,
			IntPtr hTemplateFile);

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool CloseHandle(IntPtr handle);

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool EscapeCommFunction(IntPtr hFile, UInt32 dwFunc);

		[DllImport("kernel32.dll")]
		internal static extern Boolean SetCommMask(IntPtr hFile, UInt32 dwEvtMask);

		[DllImport("kernel32.dll")]
		internal static extern Boolean GetCommModemStatus(IntPtr hFile, out UInt32 lpModemStat);

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern Boolean WaitCommEvent(IntPtr hFile, IntPtr lpEvtMask, IntPtr lpOverlapped);

		[DllImport("kernel32.dll")]
		internal static extern Boolean CancelIo(IntPtr hFile);

		[DllImport("kernel32.dll")]
		internal static extern int GetLastError();
#endregion

		private string _portName;
		private IntPtr _portHandle;
		private object _lockObj = new Object();
		private bool _CTS;
		private bool _DSR;
		private Thread _monitorThread = null;									// [sentinel]

		public event ComPortEventHandler ComPortPinChanged;

		public ComPortCtrl()
		{
			_portName = "";
			_portHandle = IntPtr.Zero;
			_CTS = _DSR = false;
		}

		private void MonitorThread()
		{
			AutoResetEvent sg = new AutoResetEvent(false);

			OVERLAPPED ov = new OVERLAPPED();
			ov.Offset = 0; ov.OffsetHigh = 0;
			ov.hEvent = sg.SafeWaitHandle.DangerousGetHandle();
			IntPtr unmanagedOv = Marshal.AllocHGlobal(Marshal.SizeOf(ov));
			Marshal.StructureToPtr(ov, unmanagedOv, true);

			uint eventMask = 0;
			IntPtr unmanagedEvMask = Marshal.AllocHGlobal(Marshal.SizeOf(eventMask));

			GetPinStatus();
			bool prevCTS = _CTS;
			bool prevDSR = _DSR;
			try
			{
				while (true)
				{
					//
					// Lameness here. Try to detect changes that happened during
					// the OnComPortPinChanged() call.
					//
					GetPinStatus();
					if (_CTS != prevCTS)
					{
					    eventMask = EV_CTS;
					    prevCTS = _CTS;
					}
					else if (_DSR != prevDSR)
					{
					    eventMask = EV_DSR;
					    prevDSR = _DSR;
					}
					else
					{
						if (!SetCommMask(_portHandle, (EV_CTS | EV_DSR)))
							throw new ApplicationException("Failed to set comm event mask");
						Marshal.WriteInt32(unmanagedEvMask, 0);
						if (!WaitCommEvent(_portHandle, unmanagedEvMask, unmanagedOv))
						{
							if (Marshal.GetLastWin32Error() != ERROR_IO_PENDING)
								throw new ApplicationException("WaitCommEvent() failed");
							sg.WaitOne();
						}
						eventMask = (uint)Marshal.ReadInt32(unmanagedEvMask);
						GetPinStatus();
						prevCTS = _CTS;
						prevDSR = _DSR;
					}
					ComPortEventArgs e = new ComPortEventArgs(eventMask);
					OnComPortPinChanged(e);
				}
			}
			catch (ThreadInterruptedException) { }
		}

		private void GetPinStatus()
		{
			uint f;
			if (!GetCommModemStatus(_portHandle, out f))
				throw new ApplicationException("Failed to get modem status");
			_CTS = ((f & MS_CTSON) != 0);
			_DSR = ((f & MS_DSRON) != 0);
		}

		protected virtual void OnComPortPinChanged(ComPortEventArgs e)
		{
			if (ComPortPinChanged != null)
				ComPortPinChanged(this, e);
		}


		public void Open(string portName)
		{
			Close();
			_portHandle = CreateFile(portName, (GENERIC_READ | GENERIC_WRITE), 0, IntPtr.Zero, 
							OPEN_EXISTING, (FILE_ATTRIBUTE_NORMAL | FILE_FLAG_OVERLAPPED), 
							IntPtr.Zero);
			if (_portHandle == IntPtr.Zero || (int)_portHandle == -1)
			{
				_portHandle = IntPtr.Zero;
				throw new ApplicationException("Failed to open " + portName + ". Doesn't exist or may be in use.");
			}
			this.DtrEnable = true;
			GetPinStatus();
			_monitorThread = new Thread(new ThreadStart(MonitorThread));
			_monitorThread.Priority = ThreadPriority.AboveNormal;				// ==== HIGHER PRIORITY ====
			_monitorThread.Name = "Monitor Thread";
			_monitorThread.Start();
			_portName = portName;
		}

		public void Close()
		{
			if (_monitorThread != null)
			{
				_monitorThread.Interrupt();
				_monitorThread.Join(1000);
			}
			_monitorThread = null;
			if (_portHandle != IntPtr.Zero)
			{
				this.DtrEnable = false;
				this.RtsEnable = false;
				if (!CloseHandle(_portHandle))
					throw new ApplicationException("Failed to close serial port. You shouldn't see this.");
			}
			_portName = "";
			_portHandle = IntPtr.Zero;
		}

		public bool RtsEnable
		{
			set 
			{
				if (_portHandle == IntPtr.Zero)
					throw new ApplicationException("The serial control port is closed.");
				if (!EscapeCommFunction(_portHandle, (value ? CF_SETRTS : CF_CLRRTS)))
					throw new ApplicationException("Failed to control RTS.");
			}
		}

		public bool DtrEnable
		{
			set
			{
				if (_portHandle == IntPtr.Zero)
					throw new ApplicationException("The serial control port is closed.");
				if (!EscapeCommFunction(_portHandle, (value ? CF_SETDTR : CF_CLRDTR)))
					throw new ApplicationException("Failed to control DTR.");
			}
		}

		public bool CtsHolding
		{ 
			get 
			{
				GetPinStatus();
				return _CTS; 
			} 
		}

		public bool DsrHolding
		{ 
			get 
			{
				GetPinStatus();
				return _DSR; 
			} 
		}

#else
		private SerialPort _serialPort;

		public ComPortCtrl()
		{
			_serialPort = null;
		}

		public void Open(string portName)
		{
			this.Close();
			_serialPort.Open();
			_serialPort.DtrEnable = true;
		}

		public void Close()
		{
			if (_serialPort != null)
			{
				_serialPort.DtrEnable = false;
				_serialPort.Close();
				_serialPort = null;
		}

#error Not yet implemented

#endif

		#region IDisposable Members

		public void Dispose()
		{
			this.Close();
		}

		#endregion
	}
}
