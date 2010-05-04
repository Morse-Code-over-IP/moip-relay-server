//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		SpTones.cs
//
// FACILITY:	RSS to Morse tool
//
// ABSTRACT:	Generates radio tone sounds via Media.SoundPlayer. This is
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
// 27-Apr-10	rbd		Initial edit
// 30-Apr-10	rbd		ITone intervace
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
    class SpTones : ITone
    {
		private const int _sampleRate = 44100;
		private const short _bitsPerSample = 16;
		private const short _bytesPerSample = 2;
		private const int _duration = 1000;

		private float _frequency;
		private float _amplitude;
		private int _ditMs;
		private int _startLatency;
		private byte[] data;
		private MemoryStream _wavStrm;
		private BinaryWriter _bWriter;
		private int _totalLength;
		private long _totalLengthPos;
		private int _dataLength;
		private long _dataLengthPos;
		private SoundPlayer _player;

		public SpTones()
        {
			_frequency = 640.0F;													// Defaults (typ.)
			_amplitude = 0.3F;
			_startLatency = 0;
			_ditMs = 80;

			data = GenTone(_frequency, _amplitude, _duration);

			_dataLength = data.Length;

			_wavStrm = new MemoryStream();											// Stream for SoundPlayer
			_bWriter = new BinaryWriter(_wavStrm, System.Text.Encoding.ASCII);		// Using a binary writer
			_bWriter.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));			// Main RIFF header
			_totalLengthPos = _wavStrm.Position;									// Remember where this went
			_bWriter.Write((int)0);													// Overall length (placeholder for now)
			_bWriter.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt "));		// Wave type followed by format chunk header
			_bWriter.Write((int)(18));												// PCM wave format length
			_bWriter.Write((short)1);												// PCM type
			_bWriter.Write((short)1);												// Mono, 1 channel
			_bWriter.Write((int)_sampleRate);										// Sample rate Hz
			_bWriter.Write((int)2 * _sampleRate);									// Avg bps is BlockALign (2) * sample rate
			_bWriter.Write((short)2);												// 2-byte alignment, (1 chan, 16 bits -> 2 bytes)
			_bWriter.Write((short)16);												// 16-bit samples
			_bWriter.Write((short)0);												// No extra data
			// No fact chunk for PCM
			_bWriter.Write(System.Text.Encoding.ASCII.GetBytes("data"));			// Data chunk header
			_dataLengthPos = _wavStrm.Position;										// Remember where this went
			_bWriter.Write(data.Length);											// Data chunk length, bytes
			_bWriter.Write(data);													// Tone data
			_bWriter.Flush();														// Flush to stream
			_totalLength = (int)(_wavStrm.Length - 8);
			_bWriter.Seek((int)_totalLengthPos, SeekOrigin.Begin);					// Rewind
			_bWriter.Write(_totalLength);											// Write overall length to placeholder
			_bWriter.Flush();														// Flush to stream
			_wavStrm.Seek(0, SeekOrigin.Begin);										// Rewind the underlying stream
			_player = new SoundPlayer(_wavStrm);									// Create a player for the stream
			_player.Load();															// Load it, ready to play.
		}

		//
        // helper function for creating sound
        //
		private byte[] GenTone(double frequency, double amp, int duration)
        {
            int length = (int)(_sampleRate * duration / 1000.0);
            byte[] wavedata = new byte[length * 2];
			double timeScale = frequency * 2 * Math.PI / (double)_sampleRate;

            int waveformPeriod = (int)(_sampleRate / frequency);
            for (int i = 0; i < length; i++)
            {
                if (i <= waveformPeriod)
                {
					double dbl = Math.Sin(i * timeScale);
                    short sh = (short)(dbl * amp * short.MaxValue);

                    wavedata[i * 2] = (byte)(sh & 0x00FF); // low byte
                    wavedata[i * 2 + 1] = (byte)(sh >> 8); // high byte
                }
                else  // we have already computed the wave, it is periodic. Good optimization!
                {
                    int prevspot = i % waveformPeriod;
                    wavedata[i * 2] = wavedata[prevspot * 2];
                    wavedata[i * 2 + 1] = wavedata[prevspot * 2 + 1];
                }
            }
            return wavedata;
        }

		//
		// Publics
		//
		public float Frequency
		{
			get { return _frequency; }
			set 
			{ 
				_frequency = value;
				data = GenTone(_frequency, _amplitude, _duration);					// Make new data
				_bWriter.Seek((int)_dataLengthPos + 4, SeekOrigin.Begin);			// Overwrite data (length will be same)
				_bWriter.Write(data);												// Tone data
				_bWriter.Flush();													// Flush to stream
			}
		}

		public float Amplitude
		{
			get { return _amplitude; }
			set
			{ 
				_amplitude = value;
				data = GenTone(_frequency, _amplitude, _duration);					// Make new data
				_bWriter.Seek((int)_dataLengthPos + 4, SeekOrigin.Begin);			// Overwrite data (length will be same)
				_bWriter.Write(data);												// Tone data
				_bWriter.Flush();													// Flush to stream
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
		// Synchronous for the duration of ms
		//
		public void PlayFor(int ms)
		{
			int l = 2 * ((_sampleRate * ms) / 1000);								// New data length - must be even number!
			int m = _totalLength - _dataLength + l;
			_bWriter.Seek((int)_totalLengthPos, SeekOrigin.Begin);
			_bWriter.Write(m);
			_bWriter.Seek((int)_dataLengthPos, SeekOrigin.Begin);
			_bWriter.Write(l);
			//_wavStrm.Flush();
			_wavStrm.Seek(0, SeekOrigin.Begin);
			_player.SoundLocation = "x";											// Trick! Force the stream to be reloaded next
			_player.Stream = _wavStrm;
			//_player.Load();
			_player.Play();															// TODO - PlaySync() and no Thread.Sleep?
			Thread.Sleep(ms);
		}

		public void Stop()
		{
			_player.Stop();
		}
    }
}
