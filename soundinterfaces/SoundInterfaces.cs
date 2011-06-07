//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		SoundInterfaces.cs
//
// FACILITY:	Morse Code News Reader
//
// ABSTRACT:	Intervaces that allow simple switching between DirectX and the
//				Media.SoundPlayer for Morse code sounds.
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
// 03-May-10	rbd		1.3.2 - This was created a few days ago, and modified
//						several times due to refactoring.
// 07-May-10	rbd		1.5.0 - Refactoring into separate assy. Make interfaces
//						public. Add Down() and Up().
// 11-May-10	rbd		1.5.0 - Add Voume to both, remove Amplitude froom ITone.
//
using System;
using System.Collections.Generic;
using System.Text;

namespace com.dc3.morse
{
	public interface ITone
	{
		float Frequency { get; set; }
		float Volume { get; set; }
		int StartLatency { get; set; }
		int DitMilliseconds { get; set; }
		void Dit();
		void Dah();
		void Space();
		void PlayFor(int ms);
		void Stop();
		void Down();
		void Up();
	}

	public interface IAudioWav
	{
		
		int SoundIndex { get; set; }
		float Volume { get; set; }
		int StartLatency { get; set; }
		int DitMilliseconds { get; set; }
		void Dit();
		void Dah();
		void Space();
		void PlayFor(int ms);
		void Stop();
		void Down();
		void Up();
	}
}
