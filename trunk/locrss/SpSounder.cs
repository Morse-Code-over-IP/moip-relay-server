//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		SpSounder.cs
//
// FACILITY:	RSS to Morse tool
//
// ABSTRACT:	Generates telegraph sounder sounds via Media.SoundPlayer. This is
//				used in preference to Managed DirectX (see DxSounder.cs).
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
// 27-Apr-10	rbd		From SpTones, for sounder audio
// 28-Apr-10	rbd		1.1.0 - Simplify sound resource loading
// 30-Apr-10	rbd		1.2.0 - ISounder, Stop()
// 02-May-10	rbd		Interface and ctor changes for loadable directx classes
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Text;
using System.Threading;

namespace com.dc3.morse
{
    class SpSounder : ISounder
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
			this.SoundIndex = 1;													// Default to sounder #1
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
				_spClick = new SoundPlayer(Properties.Resources.ResourceManager.GetStream("Click_" + value));
				_spClack = new SoundPlayer(Properties.Resources.ResourceManager.GetStream("Clack_" + value));
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
			Thread.Sleep(_ditMs - _startLatency);
		}

		//
		// Synchronous for the duration of ms, the 'clack', which comes
		// at the end of the 'mark' symbol, is allowed to play async.
		//
		public void PlayFor(int ms)
		{
			_spClack.Stop();														// In case previous mark's clack still playing
			_spClick.Play();														// Start the click playing then...
			Thread.Sleep(ms);														// ... wait for just the mark time, then ...
			_spClick.Stop();														// ... stop the click in case the sound is too long
			_spClack.Play();														// Start the clack and return while playing
		}

		public void Stop()
		{
			_spClick.Stop();
			_spClack.Stop();
		}
    }
}
