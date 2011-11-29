//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		DxSounder.cs
//
// FACILITY:	Morse Code News Reader
//
// ABSTRACT:	Generates telegraph sounder sounds via Managed DirectX. 
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
// 22-Apr-10	rbd		From DxTones, for sounder audio
// 30-Apr-10	rbd		1.2.0 - Resurrect, simplify sound resource loading
//						ISounder.
// 02-May-10	rbd		Interface and ctor changes for loadable directx classes
// 03-May-10	rbd		1.3.2 - No loadables. Shipping DX assys. Refactor to new
//						common IAudioWav interface. New PreciseDelay.
// 07-May-10	rbd		1.5.0 - Refactor into separate assy, make class public.
//						Add Down() and Up().
// 11-May-10	rbd		1.5.0 - Volume Control!
// 18-May-10	rbd		1.5.0 - Volume 0 means absolutely silent.
// 19-May-10	rbd		1.5.0 - Stop clack before Down, prevent stuttering
// 02-Jun-11	rbd		1.8.0 - Disposals to prevent memory leaks
// 28-Nov-11	rbd		1.9.0 - (*SF #3432844) Add parameter for sound device 
//						selection to constructor.
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.DirectX.DirectSound;

namespace com.dc3.morse
{
    public class DxSounder : IAudioWav, IDisposable
    {
        private Device _deviceSound= null;										// [sentinel]
		private int _sounder;
		private int _ditMs;
		private int _startLatency;
		private float _volume;
		private int _rawVol;

		private BufferDescription _bufDescClick = null;							// [sentinel]
		private BufferDescription _bufDescClack = null;							// [sentinel]
		private SecondaryBuffer _bufClick = null;								// [sentinel]
		private SecondaryBuffer _bufClack = null;								// [sentinel]
		private int _clickLenMs;
		private int _clackLenMs;

		public DxSounder(System.Windows.Forms.Control Handle, Guid DeviceGuid)
		{
			_ditMs = 80;
			this.Volume = 1.0F;

			_deviceSound = new Microsoft.DirectX.DirectSound.Device(DeviceGuid);
			_deviceSound.SetCooperativeLevel(Handle, CooperativeLevel.Priority);	// Up priority for quick response

			_bufDescClick = new BufferDescription();
			_bufDescClick.ControlEffects = false;								// Necessary because .wav file is so short (typ.)
			_bufDescClick.GlobalFocus = true;									// Enable audio when program is in background (typ.)
			_bufDescClick.ControlVolume = true;

			_bufDescClack = new BufferDescription();
			_bufDescClack.ControlEffects = false;
			_bufDescClack.GlobalFocus = true;
			_bufDescClack.ControlVolume = true;

			this.SoundIndex = 1;												// Default to sounder #1
		}
		
		//
		// Publics
		//
		public int SoundIndex
		{
			get { return _sounder; }
			set
			{
				if (value < 1 || value > 7)
					throw new ApplicationException("Sounder number out of range");
				_sounder = value;
				if (_bufClick != null)
					_bufClick.Dispose();
				_bufClick = new SecondaryBuffer(Properties.Resources.ResourceManager.GetStream("Click_" + value), 
							_bufDescClick, _deviceSound);
				_clickLenMs = (_bufDescClick.BufferBytes * 1000 /_bufDescClick.Format.AverageBytesPerSecond);
				if (_bufClack != null)
					_bufClack.Dispose();
				_bufClack = new SecondaryBuffer(Properties.Resources.ResourceManager.GetStream("Clack_" + value), 
							_bufDescClack, _deviceSound);
				_clackLenMs = (_bufDescClack.BufferBytes * 1000 /_bufDescClack.Format.AverageBytesPerSecond);
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
			//
			// Much magic and pain here. Do NOT make any calls that you don't need.
			// Every one of them is expensive in milliseconds.
			//
			//Debug.Print("A " + _bufClack.PlayPosition.ToString());			// Ideally this should be zero during operation
			_bufClick.Volume = _rawVol;
			_bufClack.Volume = _rawVol;
			if (_sounder == 2)													// Special case, tone mix (long sound)
			{
				_bufClack.Stop();
				_bufClick.SetCurrentPosition(0);								// Costs 5-10ms on a fast system! (typ.)
			}
			_bufClick.Play(0, BufferPlayFlags.Default);
			PreciseDelay.Wait(ms);
			//Debug.Print("I " + _bufClick.PlayPosition.ToString());			// Ideally this should be zero during operation
			if (_sounder == 2)													// Special case again
			{
				_bufClick.Stop();
				_bufClack.SetCurrentPosition(0);
			}
			_bufClack.Play(0, BufferPlayFlags.Default);
		}

		public void Stop()
		{
			_bufClick.Stop();
			_bufClack.Stop();
		}

		public void Down()
		{
			_bufClack.Stop();
			_bufClick.Volume = _rawVol;
			_bufClick.SetCurrentPosition(0);
			_bufClick.Play(0, BufferPlayFlags.Default);
		}

		public void Up()
		{
			_bufClick.Stop();
			_bufClack.Volume = _rawVol;
			_bufClack.SetCurrentPosition(0);
			_bufClack.Play(0, BufferPlayFlags.Default);
		}


		#region IDisposable Members

		public void Dispose()
		{
			if (_deviceSound != null)
				_deviceSound.Dispose();
			_deviceSound = null;
			if (_bufDescClick != null)
				_bufDescClick.Dispose();
			_bufDescClick = null;
			if (_bufDescClack != null)
				_bufDescClack.Dispose();
			_bufDescClack = null;
			if (_bufClick != null)
				_bufClick.Dispose();
			_bufClick = null;
			if (_bufClack != null)
				_bufClack.Dispose();
			_bufClack = null;
		}

		#endregion
	}
}
