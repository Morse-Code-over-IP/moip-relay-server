//
// 22-Apr-10	rbd		From DxTones, for sounder audio
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

using NAudio.Wave;
using NAudio.CoreAudioApi;

namespace com.dc3.morse
{
    class DxSounder
    {
		private int _sounder;
		private int _ditMs;

		private WaveOut _waveOut;

		public DxSounder(System.Windows.Forms.Control Handle)
		{
			_ditMs = 80;
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
				//switch (value)
				//{
				//    case 1:
				//        _bufClick = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Click_1, _bufDescClick, _deviceSound);
				//        _bufClack = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Clack_1, _bufDescClack, _deviceSound);
				//        break;
				//    case 2:
				//        _bufClick = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Click_2, _bufDescClick, _deviceSound);
				//        _bufClack = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Clack_2, _bufDescClack, _deviceSound);
				//        break;
				//    case 3:
				//        _bufClick = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Click_3, _bufDescClick, _deviceSound);
				//        _bufClack = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Clack_3, _bufDescClack, _deviceSound);
				//        break;
				//    case 4:
				//        _bufClick = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Click_4, _bufDescClick, _deviceSound);
				//        _bufClack = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Clack_4, _bufDescClack, _deviceSound);
				//        break;
				//    case 5:
				//        _bufClick = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Click_5, _bufDescClick, _deviceSound);
				//        _bufClack = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Clack_5, _bufDescClack, _deviceSound);
				//        break;
				//    case 6:
				//        _bufClick = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Click_6, _bufDescClick, _deviceSound);
				//        _bufClack = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Clack_6, _bufDescClack, _deviceSound);
				//        break;
				//    case 7:
				//        _bufClick = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Click_7, _bufDescClick, _deviceSound);
				//        _bufClack = new Microsoft.DirectX.DirectSound.Buffer(Properties.Resources.Clack_7, _bufDescClack, _deviceSound);
				//        break;
				//}
			}
		}

		public int DitMilliseconds
		{
			get { return _ditMs; }
			set { _ditMs = value; }
		}

		public void Stop()
		{
			return;
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
			//_bufClick.SetCurrentPosition(0);
			//_bufClick.Play(0, BufferPlayFlags.Default);
			//Thread.Sleep(ms);
			//_bufClick.Stop();
			//_bufClack.SetCurrentPosition(0);
			//_bufClack.Play(0, BufferPlayFlags.Default);
		}
    }
}
