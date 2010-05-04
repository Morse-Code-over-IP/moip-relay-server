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
