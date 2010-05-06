//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		DxSpark.cs
//
// FACILITY:	RSS to Morse tool
//
// ABSTRACT:	Generates spark gap sounds via Managed DirectX. 
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
// 30-Apr-10	rbd		From DxSounder, for spark gap audio
// 02-May-10	rbd		Interface and ctor changes for loadable directx classes
// 03-May-10	rbd		1.3.2 - No loadables. Shipping DX assys. Refactor to new
//						common IAudioWav interface. New PreciseDelay.
// 05-May-10	rbd		1.4.2 - Seek to within ms of end of spark sound, play to
//						and. Eliminates expensive Stop() call.
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.DirectX.DirectSound;

namespace com.dc3.morse
{
    class DxSpark : IAudioWav
    {
        private Device _deviceSound;
		private int _sparkNum;
		private int _ditMs;
		private int _startLatency;

		private BufferDescription _bufDesc;
		private Microsoft.DirectX.DirectSound.Buffer _buf;

		public DxSpark(System.Windows.Forms.Control Handle)
		{
			_ditMs = 80;
			_startLatency = 0;

			_deviceSound = new Microsoft.DirectX.DirectSound.Device();
			_deviceSound.SetCooperativeLevel(Handle, CooperativeLevel.Priority);	// Up priority for quick response

			_bufDesc = new BufferDescription();
			_bufDesc.ControlEffects = false;									// Necessary because .wav file is so short (typ.)
			_bufDesc.GlobalFocus = true;										// Enable audio when program is in background (typ.)

			this.SoundIndex = 1;												// Default to spark number #1
		}
		
		//
		// Publics
		//
		public int SoundIndex
		{
			get { return _sparkNum; }
			set
			{
				if (value < 1 || value > 4)
					throw new ApplicationException("Spark number out of range");
				_sparkNum = value;
				_buf = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.ResourceManager.GetStream("Spark_" + value), 
							_bufDesc, _deviceSound);
			}
		}

		public int StartLatency
		{
			get { return _startLatency; }
			set { _startLatency = value; }
		}

		public int DitMilliseconds
		{
			get { return _ditMs; }
			set { _ditMs = value; }
		}

		public void Dit()
		{
			PlayFor(_ditMs);
		}

		public void Dah()
		{
			PlayFor(_ditMs * 3);
		}

		public void Space()
		{
			PreciseDelay.Wait(_ditMs - _startLatency);
		}

		public void PlayFor(int ms)
		{
			_buf.SetCurrentPosition(_bufDesc.BufferBytes - ((_bufDesc.Format.AverageBytesPerSecond * ms) / 1000));
			_buf.Play(0, BufferPlayFlags.Default);
			PreciseDelay.Wait(ms);
		}

		public void Stop()
		{
			_buf.Stop();
		}
    }
}
