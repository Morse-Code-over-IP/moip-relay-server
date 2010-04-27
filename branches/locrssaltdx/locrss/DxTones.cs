//
// 22-Apr-10	rbd		Play tones of arbitrary length
//
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using NAudio.Wave;

namespace com.dc3.morse
{
	//public class SineWaveProvider32 : WaveProvider32
	//{
	//    int sample;

	//    public SineWaveProvider32()
	//    {
	//        Frequency = 1000;
	//        Amplitude = 0.25f; // let's not hurt our ears            
	//    }

	//    public float Frequency { get; set; }
	//    public float Amplitude { get; set; }

	//    public override int Read(float[] buffer, int offset, int sampleCount)
	//    {
	//        int sampleRate = WaveFormat.SampleRate;
	//        for (int n = 0; n < sampleCount; n++)
	//        {
	//            buffer[n + offset] = (float)(Amplitude * Math.Sin((2 * Math.PI * sample * Frequency) / sampleRate));
	//            sample++;
	//            if (sample >= sampleRate) sample = 0;
	//        }
	//        return sampleCount;
	//    }
	//}

	public class WaveMemoryStream : WaveStream
	{
		private MemoryStream _stream;
		private WaveFormat _wavFmt;

		public WaveMemoryStream(MemoryStream stream, WaveFormat waveFormat)
		{
			_stream = stream;
			_wavFmt = waveFormat;
		}

		public override WaveFormat WaveFormat
		{
			get { return _wavFmt; }
		}

		public override long Length
		{
			get { return _stream.Length; }
		}

		public override long Position
		{
			get
			{
				return _stream.Position;
			}
			set
			{
				_stream.Seek(value, SeekOrigin.Begin);
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return _stream.Read(buffer, offset, count);
		}
	}

	public class TonePlayer : IDisposable
	{
		private DirectSoundOut _wPlayer = null;
		private WaveMemoryStream _wavMemStrm;

		public TonePlayer(int SampleRate, double Frequency, double Amplitude, int Duration)
		{
			int length = (int)(SampleRate * Duration / 1000.0);
			byte[] data = new byte[length * 2];
			double timeScale = Frequency * 2 * Math.PI / (double)SampleRate;

			int waveformPeriod = (int)(SampleRate / Frequency);						// Generate tone
			for (int i = 0; i < length; i++)
			{
				if (i <= waveformPeriod)
				{
					double dbl = Math.Sin(i * timeScale);
					short sh = (short)(dbl * Amplitude * short.MaxValue);

					data[i * 2] = (byte)(sh & 0x00FF);								// Low byte
					data[i * 2 + 1] = (byte)(sh >> 8);								// High byte
				}
				else  // we have already computed the wave, it is periodic. Good optimization!
				{
					int prevspot = i % waveformPeriod;
					data[i * 2] = data[prevspot * 2];
					data[i * 2 + 1] = data[prevspot * 2 + 1];
				}
			}
			MemoryStream wavStrm = new MemoryStream();								// Stream for SoundPlayer
			BinaryWriter bWriter = new BinaryWriter(wavStrm, System.Text.Encoding.ASCII);	// Using a binary writer
			bWriter.Write(data);													// Tone data
			wavStrm.Seek(0, SeekOrigin.Begin);

			WaveFormat wave = new WaveFormat(SampleRate, 16, 1);
			_wavMemStrm = new WaveMemoryStream(wavStrm, wave);
			_wPlayer = new DirectSoundOut();
			_wPlayer.Init(_wavMemStrm);
		}

		public void Play()
		{
			_wavMemStrm.Seek(0, SeekOrigin.Begin);
			_wPlayer.Play();
		}

		public void Play(int ms)
		{
			_wavMemStrm.Seek(((1000 - ms) * 88200) / 1000, SeekOrigin.Begin);
			_wPlayer.Play();
		}

		public void Stop()
		{
			_wPlayer.Stop();
		}

		#region IDisposable Members

		public void Dispose()
		{
			_wPlayer.Stop();
			_wPlayer.Dispose();
		}

		#endregion
	}
	
	class DxTones
    {
		private const int _sampleRate = 44100;
		private const short _bitsPerSample = 16;
		private const short _bytesPerSample = 2;

		private int _maxLen;														// Max length tone
		private float _freq;
		private float _ampl;
		private int _ditMs;

		private TonePlayer _tonePlayer = null;
		//private SineWaveProvider32 _sineWave;
		//private WaveOut _waveOut;


		public DxTones(System.Windows.Forms.Control Handle, int MaxLenMs)
        {
			//_waveOut = new WaveOut();

			_maxLen = MaxLenMs;
			_freq = 880;															// Defaults (typ.)
			_ampl = 0.3F;
			_ditMs = 80;

			_tonePlayer = new TonePlayer(_sampleRate, _freq, _ampl, _maxLen);
			//setupTone();
		}

		//
		// Generate the tone data
		//
		//private void setupTone()
		//{
		//    _sineWave = new SineWaveProvider32();
		//    _sineWave.SetWaveFormat(_sampleRate, 1); // 16kHz mono
		//    _sineWave.Frequency = _freq;
		//    _sineWave.Amplitude = 0.25f;
		//    _waveOut.Init(_sineWave);
		//}

		//
		// Publics
		//
		public int MaxLenMs
		{
			get { return _maxLen; }
			set
			{
				_maxLen = value;
				//setupTone();
				if (_tonePlayer != null) _tonePlayer.Dispose();
				_tonePlayer = new TonePlayer(_sampleRate, _freq, _ampl, _maxLen);

			}
		}

		public float Frequency
		{
			get { return _freq; }
			set 
			{ 
				_freq = value;
				//setupTone();
				if (_tonePlayer != null) _tonePlayer.Dispose();
				_tonePlayer = new TonePlayer(_sampleRate, _freq, _ampl, _maxLen);
			}
		}

		public float Amplitude
		{
			get { return _ampl; }
			set
			{ 
				_ampl = value;
				//setupTone();
				if (_tonePlayer != null) _tonePlayer.Dispose();
				_tonePlayer = new TonePlayer(_sampleRate, _freq, _ampl, _maxLen);
			}
		}

		public void Stop()
		{
			//_waveOut.Stop();
			_tonePlayer.Stop();
		}

		public int DitMilliseconds
		{
			get { return _ditMs; }
			set { _ditMs = value; }
		}

		public void Dit()
		{
			Tone(_ditMs);
		}

		public void Dah()
		{
			Tone(_ditMs * 3);
		}

		public void Space()
		{
			Thread.Sleep(_ditMs);
		}

		public void Tone(int ms)
		{
			//_waveOut.Play();
			_tonePlayer.Play(ms);
			Thread.Sleep(ms);
			//_waveOut.Stop();
		}
    }
}
