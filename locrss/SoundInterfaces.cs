//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		SOundInterfaces.cs
//
// FACILITY:	RSS to Morse tool
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
//
using System;
using System.Collections.Generic;
using System.Text;

namespace com.dc3
{
	interface ITone
	{
		float Frequency { get; set; }
		float Amplitude { get; set; }
		int StartLatency { get; set; }
		int DitMilliseconds { get; set; }
		void Dit();
		void Dah();
		void Space();
		void PlayFor(int ms);
		void Stop();
	}

	interface IAudioWav
	{
		
		int SoundIndex { get; set; }
		int StartLatency { get; set; }
		int DitMilliseconds { get; set; }
		void Dit();
		void Dah();
		void Space();
		void PlayFor(int ms);
		void Stop();
	}
}
