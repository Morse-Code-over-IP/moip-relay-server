//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		SpSpark.cs
//
// FACILITY:	RSS to Morse tool
//
// ABSTRACT:	Generates spark gap sounds via Media.SoundPlayer. This is
//				used in preference to Managed DirectX (see DxTones.cs).
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
// 29-Apr-10	rbd		Initial edit - from SpTones
// 30-Apr-10	rbd		1.2.0 - ISpark, 4 sounds
// 02-May-10	rbd		Interface and ctor changes for loadable directx classes
// 03-May-10	rbd		1.3.0 - No loadables. Shipping DX assys. Refactor to new
//						common IAudioWav interface.
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Text;
using System.Threading;

namespace com.dc3.morse
{
    class SpSpark : IAudioWav
    {
		//private const int _sampleRate = 44100;										// Parameters of spark WAV file resources
		//private const short _bitsPerSample = 16;
		//private const short _bytesPerSample = 2;
		//private const int _duration = 1000;
		//private const long _totalLengthPos = 4;										// These come from the WAV resource
		//private const long _dataLengthPos = 42;
		//private const int _headerSize = 38;

		private int _sparkNumber;
		private int _ditMs;
		private int _startLatency;
		//private MemoryStream _wavStrm;
		//private BinaryWriter _bWriter;
		private SoundPlayer _player;

		public SpSpark()
        {
			_startLatency = 0;
			_ditMs = 80;
			this.SoundIndex = 1;
		}

		public int SoundIndex
		{
			get { return _sparkNumber; }
			set
			{
				if (value < 1 || value > 4)
					throw new ApplicationException("Spark number out of range");
				_sparkNumber = value;

				//UnmanagedMemoryStream rStream = Properties.Resources.ResourceManager.GetStream("Spark_" + value);
				//byte[] buf = new byte[rStream.Length];
				//rStream.Read(buf, 0, buf.Length);
				//_wavStrm = new MemoryStream(buf);										// WAV is now in a MemoryStream
				//_bWriter = new BinaryWriter(_wavStrm, System.Text.Encoding.ASCII);		// Using a binary writer
				//_player = new SoundPlayer(_wavStrm);									// Create a player for the stream
				//_player.Load();															// Load it, ready to play.

				_player = new SoundPlayer(Properties.Resources.ResourceManager.GetStream("Spark_" + value));

			}
		}

		//
		// Publics
		//

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
//			Thread.Sleep(_ditMs - _startLatency);
			PreciseDelay.Wait(_ditMs - _startLatency);
		}

		//
		// Synchronous for the duration of ms
		//
		public void PlayFor(int ms)
		{
			//int l = 2 * ((_sampleRate * ms) / 1000);								// New data length - must be even number!
			//int m = _headerSize + l;
			//_bWriter.Seek((int)_totalLengthPos, SeekOrigin.Begin);
			//_bWriter.Write(m);
			//_bWriter.Seek((int)_dataLengthPos, SeekOrigin.Begin);
			//_bWriter.Write(l);
			////_wavStrm.Flush();
			//_wavStrm.Seek(0, SeekOrigin.Begin);
			//_player.SoundLocation = "x";											// Trick! Force the stream to be reloaded next
			//_player.Stream = _wavStrm;
			////_player.Load();
			//_player.Play();															// TODO - PlaySync() and no Thread.Sleep?
			//Thread.Sleep(ms);
			_player.Play();
//			Thread.Sleep(ms);
			PreciseDelay.Wait(ms);
			_player.Stop();
		}

		public void Stop()
		{
			_player.Stop();
		}
    }
}
