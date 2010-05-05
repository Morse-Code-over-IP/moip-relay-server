//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		ComPortCtrl.cs
//
// FACILITY:	RSS to Morse tool
//
// ABSTRACT:	The .NET SerialPort class is extremely sluggish in controlling
//				the modem-control lines on the serial port. This class uses 
//				P/Invoke to talk directly to the kernel serial port driver to
//				accomplish the same thing with far more accuracy and less 
//				latency.
//
// ENVIRONMENT:	Microsoft.NET 2.0/3.5
//				Developed under Visual Studio.NET 2008
//				Also may be built under MonoDevelop 2.2.1/Mono 2.4+
//
// AUTHOR:		Bob Denny, <rdenny@dc3.com>
//
// Edit Log:
//
// When			Who		What
//----------	---		-------------------------------------------------------
// 05-May-10	rbd		1.3.3 - Initial edit.
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO.Ports;

namespace com.dc3
{
	class ComPortCtrl : IDisposable
	{
#if !MONO_BUILD

#region P/Invoke Declarationa
		private const int SETRTS = 3;
		private const int CLRRTS = 4;
		private const int SETDTR = 5;
		private const int CLRDTR = 6;
		private const uint GENERIC_READ = 0x80000000;
		private const uint GENERIC_WRITE = 0x40000000;
		private const uint OPEN_EXISTING = 3;
		private const uint FILE_ATTRIBUTE_NORMAL = 0x80;

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr CreateFile(
			string lpFileName,
			uint dwDesiredAccess,
			uint dwShareMode,
			IntPtr lpSecurityAttributes,
			uint dwCreationDisposition,
			uint dwFlagsAndAttributes,
			IntPtr hTemplateFile);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr handle);

		[DllImport("kernel32.dll", SetLastError = true)]
		static extern bool EscapeCommFunction(IntPtr hFile, int dwFunc);

		[DllImport("kernel32.dll")]
		static extern int GetLastError();
#endregion

		private string _portName;
		private IntPtr _portHandle;

		public ComPortCtrl()
		{
			_portName = "";
			_portHandle = IntPtr.Zero;
		}

		public void Open(string portName)
		{
			Close();
			_portHandle = CreateFile(portName, (GENERIC_READ | GENERIC_WRITE), 0, IntPtr.Zero, 
							OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, IntPtr.Zero);
			if (_portHandle == IntPtr.Zero)
				throw new ApplicationException("Failed to open " + portName + ". Doesn't exist or may be in use.");
			if (!EscapeCommFunction(_portHandle, SETDTR))
				throw new ApplicationException("Failed to control DTR.");
			_portName = portName;
		}

		public void Close()
		{
			if (_portHandle != IntPtr.Zero)
			{
				EscapeCommFunction(_portHandle, CLRDTR);
				if (!CloseHandle(_portHandle))
					throw new ApplicationException("Failed to close serial port. You shouldn't see this.");
			}
			_portName = "";
			_portHandle = IntPtr.Zero;
		}

		public bool RTS
		{
			set 
			{
				if (_portHandle == IntPtr.Zero)
					throw new ApplicationException("The serial control port is closed.");
				if (!EscapeCommFunction(_portHandle, (value ? SETRTS : CLRRTS)))
					throw new ApplicationException("Failed to control RTS.");
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

		public bool RTS
		{
			set
			{

			}
		}

#endif

		#region IDisposable Members

		public void Dispose()
		{
			this.Close();
		}

		#endregion
	}
}
