using System;
using System.Collections.Generic;
using System.Text;

namespace com.dc3
{
	interface ITone
	{
		float Frequency { get; set; }
		float Amplitude { get; set; }
		int DitMilliseconds { get; set; }
		void Dit();
		void Dah();
		void Space();
		void Tone(int ms);
		void Stop();
	}

	interface ISpark
	{
		int SparkNumber { get; set; }
		int StartLatency { get; set; }
		int DitMilliseconds { get; set; }
		void Dit();
		void Dah();
		void Space();
		void Spark(int ms);
		void Stop();
	}

	interface ISounder
	{
		int Sounder { get; set; }
		int StartLatency { get; set; }
		int DitMilliseconds { get; set; }
		void Dit();
		void Dah();
		void Space();
		void ClickClack(int ms);
		void Stop();
	}
}
