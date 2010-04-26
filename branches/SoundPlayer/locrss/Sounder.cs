using System;
using System.Media;
using System.Threading;
using System.IO;

namespace com.dc3.morse
{
	class Sounder
	{
		private SoundPlayer _clickPlayer;
		private SoundPlayer _clackPlayer;
		private int _ditMs;

		public Sounder(int SounderNumber)
		{
			if (SounderNumber < 1 || SounderNumber > 7)
				throw new ApplicationException("Sounder number out of range");
			switch (SounderNumber)
			{
				case 1:
					_clickPlayer = new System.Media.SoundPlayer(Properties.Resources.Click_1);
					_clackPlayer = new System.Media.SoundPlayer(Properties.Resources.Clack_1);
					break;
				case 2:
					_clickPlayer = new System.Media.SoundPlayer(Properties.Resources.Click_2);
					_clackPlayer = new System.Media.SoundPlayer(Properties.Resources.Clack_2);
					break;
				case 3:
					_clickPlayer = new System.Media.SoundPlayer(Properties.Resources.Click_3);
					_clackPlayer = new System.Media.SoundPlayer(Properties.Resources.Clack_3);
					break;
				case 4:
					_clickPlayer = new System.Media.SoundPlayer(Properties.Resources.Click_4);
					_clackPlayer = new System.Media.SoundPlayer(Properties.Resources.Clack_4);
					break;
				case 5:
					_clickPlayer = new System.Media.SoundPlayer(Properties.Resources.Click_5);
					_clackPlayer = new System.Media.SoundPlayer(Properties.Resources.Clack_5);
					break;
				case 6:
					_clickPlayer = new System.Media.SoundPlayer(Properties.Resources.Click_6);
					_clackPlayer = new System.Media.SoundPlayer(Properties.Resources.Clack_6);
					break;
				case 7:
					_clickPlayer = new System.Media.SoundPlayer(Properties.Resources.Click_7);
					_clackPlayer = new System.Media.SoundPlayer(Properties.Resources.Clack_7);
					break;
			}
		}

		public void Stop()
		{
			_clickPlayer.Stop();
			_clackPlayer.Stop();
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
			_clickPlayer.Stop();
			_clackPlayer.Stop();
			_clickPlayer.Play();
			Thread.Sleep(ms);
			_clickPlayer.Stop();
			_clackPlayer.Play();
		}
	}
}
