//
// 27-Apr-10	rbd		From SpTones, for sounder audio
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Text;
using System.Threading;

namespace com.dc3.morse
{
    class SpSounder
    {
		private int _sounder;
		private SoundPlayer _spClick;
		private SoundPlayer _spClack;
		private int _startLatency;
		private int _ditMs;

		public SpSounder()
		{
			_startLatency = 0;
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
				switch (value)
				{
					case 1:
						_spClick = new SoundPlayer(Properties.Resources.Click_1);
						_spClack = new SoundPlayer(Properties.Resources.Clack_1);
						break;
					case 2:
						_spClick = new SoundPlayer(Properties.Resources.Click_2);
						_spClack = new SoundPlayer(Properties.Resources.Clack_2);
						break;
					case 3:
						_spClick = new SoundPlayer(Properties.Resources.Click_3);
						_spClack = new SoundPlayer(Properties.Resources.Clack_3);
						break;
					case 4:
						_spClick = new SoundPlayer(Properties.Resources.Click_4);
						_spClack = new SoundPlayer(Properties.Resources.Clack_4);
						break;
					case 5:
						_spClick = new SoundPlayer(Properties.Resources.Click_5);
						_spClack = new SoundPlayer(Properties.Resources.Clack_5);
						break;
					case 6:
						_spClick = new SoundPlayer(Properties.Resources.Click_6);
						_spClack = new SoundPlayer(Properties.Resources.Clack_6);
						break;
					case 7:
						_spClick = new SoundPlayer(Properties.Resources.Click_7);
						_spClack = new SoundPlayer(Properties.Resources.Clack_7);
						break;
				}
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
			_spClick.Play();
			Thread.Sleep(ms);
			_spClick.Stop();
			_spClack.Play();
		}
    }
}
