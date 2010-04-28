//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		DxSounder.cs
//
// FACILITY:	RSS to Morse tool
//
// ABSTRACT:	Generates telegraph sounder sounds via Managed DirectX. This is
//				no longer used, see SpSounder.cs.
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
// 22-Apr-10	rbd		From DxTones, for sounder audio
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Microsoft.DirectX.DirectSound;

namespace com.dc3.morse
{
    class DxSounder
    {
        private Device _deviceSound;
		private int _sounder;
		private int _ditMs;

		private BufferDescription _bufDescClick;
		private BufferDescription _bufDescClack;
		private Microsoft.DirectX.DirectSound.Buffer _bufClick;
		private Microsoft.DirectX.DirectSound.Buffer _bufClack;

		public DxSounder(System.Windows.Forms.Control Handle)
		{
			_ditMs = 80;

			_deviceSound = new Microsoft.DirectX.DirectSound.Device();
			_deviceSound.SetCooperativeLevel(Handle, CooperativeLevel.Priority);	// Up priority for quick response

			_bufDescClick = new BufferDescription();
			_bufDescClick.ControlEffects = false;								// Necessary because .wav file is so short (typ.)
			_bufDescClick.GlobalFocus = true;									// Enable audio when program is in background (typ.)

			_bufDescClack = new BufferDescription();
			_bufDescClack.ControlEffects = false;
			_bufDescClack.GlobalFocus = true;

			this.Sounder = 1;													// Default to sounder #1
		}
		
		//
		// Publics
		//
		public int Sounder
		{
			get { return _sounder; }
			set
			{
				if (value < 1 || value > 7)
					throw new ApplicationException("Sounder number out of range");
				_sounder = value;
				switch (value)
				{
					case 1:
						_bufClick = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Click_1, _bufDescClick, _deviceSound);
						_bufClack = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Clack_1, _bufDescClack, _deviceSound);
						break;
					case 2:
						_bufClick = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Click_2, _bufDescClick, _deviceSound);
						_bufClack = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Clack_2, _bufDescClack, _deviceSound);
						break;
					case 3:
						_bufClick = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Click_3, _bufDescClick, _deviceSound);
						_bufClack = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Clack_3, _bufDescClack, _deviceSound);
						break;
					case 4:
						_bufClick = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Click_4, _bufDescClick, _deviceSound);
						_bufClack = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Clack_4, _bufDescClack, _deviceSound);
						break;
					case 5:
						_bufClick = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Click_5, _bufDescClick, _deviceSound);
						_bufClack = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Clack_5, _bufDescClack, _deviceSound);
						break;
					case 6:
						_bufClick = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Click_6, _bufDescClick, _deviceSound);
						_bufClack = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Clack_6, _bufDescClack, _deviceSound);
						break;
					case 7:
						_bufClick = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Click_7, _bufDescClick, _deviceSound);
						_bufClack = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Clack_7, _bufDescClack, _deviceSound);
						break;
				}
			}
		}

		public int DitMilliseconds
		{
			get { return _ditMs; }
			set { _ditMs = value; }
		}

		public void Dit()
		{
			ClickClack(_ditMs);
		}

		public void Dah()
		{
			ClickClack(_ditMs * 3);
		}

		public void Space()
		{
			Thread.Sleep(_ditMs);
		}

		public void ClickClack(int ms)
		{
			_bufClick.SetCurrentPosition(0);
			_bufClick.Play(0, BufferPlayFlags.Default);
			Thread.Sleep(ms);
			_bufClick.Stop();
			_bufClack.SetCurrentPosition(0);
			_bufClack.Play(0, BufferPlayFlags.Default);
		}
    }
}
