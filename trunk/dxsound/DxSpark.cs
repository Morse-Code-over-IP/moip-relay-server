//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		DxSpark.cs
//
// FACILITY:	Morse Code News Reader
//
// ABSTRACT:	Generates spark gap sounds via Managed DirectX. 
//
// ENVIRONMENT:	Microsoft.NET 2.0/3.5
//				Developed under Visual Studio.NET 2008
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
// 07-May-10	rbd		1.5.0 Refactoring into separate assy, make class public.
//						Add Down() and Up().
// 11-May-10	rbd		1.5.0 - Volume Control!
// 18-May-10	rbd		1.5.0 - Volume 0 means absolutely silent.
// 19-May-10	rbd		1.5.0 - Stop before Down, prevent stuttering
// 02-Jun-11	rbd		1.8.0 - Disposals to prevent memory leaks
// 28-Nov-11	rbd		1.9.0 - (*SF #3432844) Add parameter for sound device 
//						selection to constructor.
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.DirectX.DirectSound;

namespace com.dc3.morse
{
    public class DxSpark : IAudioWav, IDisposable
    {
        private Device _deviceSound = null;										// [sentinel]
		private float _volume;
		private int _rawVol;
		private int _sparkNum;
		private int _ditMs;
		private int _startLatency;

		private BufferDescription _bufDesc = null;								// [sentinel]
		private Microsoft.DirectX.DirectSound.Buffer _buf = null;				// [sentinel]

		public DxSpark(System.Windows.Forms.Control Handle, Guid DeviceGuid)
		{
			_ditMs = 80;
			_startLatency = 0;
			this.Volume = 1.0F;

			_deviceSound = new Microsoft.DirectX.DirectSound.Device(DeviceGuid);
			_deviceSound.SetCooperativeLevel(Handle, CooperativeLevel.Priority);	// Up priority for quick response

			_bufDesc = new BufferDescription();
			_bufDesc.ControlEffects = false;									// Necessary because .wav file is so short (typ.)
			_bufDesc.GlobalFocus = true;										// Enable audio when program is in background (typ.)
			_bufDesc.ControlVolume = true;

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
				if (_buf != null)
					_buf.Dispose();
				_buf = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.ResourceManager.GetStream("Spark_" + value), 
							_bufDesc, _deviceSound);
			}
		}

		public int StartLatency
		{
			get { return _startLatency; }
			set { _startLatency = value; }
		}

		public float Volume
		{
			get { return _volume; }
			set
			{
				_volume = value;
				if (value == 0.0F)
					_rawVol = -9000;
				else
					_rawVol = -(int)Math.Pow((60 * (value - 1.0F)), 2);
			}
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
			_buf.Volume = _rawVol;
			_buf.SetCurrentPosition(_bufDesc.BufferBytes - ((_bufDesc.Format.AverageBytesPerSecond * ms) / 1000));
			_buf.Play(0, BufferPlayFlags.Default);
			PreciseDelay.Wait(ms);
		}

		public void Stop()
		{
			_buf.Stop();
		}

		public void Down()
		{
			_buf.Stop();
			_buf.Volume = _rawVol;
			_buf.SetCurrentPosition(0);
			_buf.Play(0, BufferPlayFlags.Default);
		}

		public void Up()
		{
			this.Stop();
		}

		#region IDisposable Members

		public void Dispose()
		{
			if (_deviceSound != null)
				_deviceSound.Dispose();
			_deviceSound = null;
			if (_bufDesc != null)
				_bufDesc.Dispose();
			_bufDesc = null;
			if (_buf != null)
				_buf.Dispose();
			_buf = null;
		}

		#endregion
	}
}
