//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		SpSounder.cs
//
// FACILITY:	Morse Code News Reader
//
// ABSTRACT:	Generates telegraph sounder sounds via Media.SoundPlayer. 
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
// 27-Apr-10	rbd		From SpTones, for sounder audio
// 28-Apr-10	rbd		1.1.0 - Simplify sound resource loading
// 30-Apr-10	rbd		1.2.0 - ISounder, Stop()
// 02-May-10	rbd		Interface and ctor changes for loadable directx classes
// 03-May-10	rbd		1.3.2 - No loadables. Shipping DX assys. Refactor to new
//						common IAudioWav interface. New PreciseDelay.
// 05-May-10	rbd		1.3.3 - Oops, forgot the delay in Space();
// 05-May-10	rbd		1.4.2 - Remove Stop() calls, sounds are now 20ms. Much 
//						better, smoother.
// 07-May-10	rbd		1.5.0 - Refactor into separate assy, make class public.
//						Add Down() and Up().
// 11-May-10	rbd		1.5.0 - Stubbed out Volume property
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Text;
using System.Threading;

namespace com.dc3.morse
{
    public class SpSounder : IAudioWav
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

		public float Volume
		{
			get { return 1.0f; }
			set { }
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
			PreciseDelay.Wait(_ditMs - StartLatency);
		}

		//
		// Synchronous for the duration of ms, the 'clack', which comes
		// at the end of the 'mark' symbol, is allowed to play async.
		//
		public void PlayFor(int ms)
		{
			//_spClack.Stop();														// In case previous mark's clack still playing
			_spClick.Play();														// Start the click playing then...
			PreciseDelay.Wait(ms);
			//_spClick.Stop();														// ... stop the click in case the sound is too long
			_spClack.Play();														// Start the clack and return while playing
		}

		public void Stop()
		{
			_spClick.Stop();
			_spClack.Stop();
		}

		public void Down()
		{
			_spClick.Play();
		}

		public void Up()
		{
			_spClick.Stop();
			_spClack.Play();
		}
    }
}
