//
// 22-Apr-10	rbd		Play tones of arbitrary length
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NAudio.Wave;

namespace com.dc3.morse
{
	public class SineWaveProvider32 : WaveProvider32
	{
		int sample;

		public SineWaveProvider32()
		{
			Frequency = 1000;
			Amplitude = 0.25f; // let's not hurt our ears            
		}

		public float Frequency { get; set; }
		public float Amplitude { get; set; }

		public override int Read(float[] buffer, int offset, int sampleCount)
		{
			int sampleRate = WaveFormat.SampleRate;
			for (int n = 0; n < sampleCount; n++)
			{
				buffer[n + offset] = (float)(Amplitude * Math.Sin((2 * Math.PI * sample * Frequency) / sampleRate));
				sample++;
				if (sample >= sampleRate) sample = 0;
			}
			return sampleCount;
		}
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

		private SineWaveProvider32 _sineWave;
		private WaveOut _waveOut;


		public DxTones(System.Windows.Forms.Control Handle, int MaxLenMs)
        {
			_waveOut = new WaveOut();

			_maxLen = MaxLenMs;
			_freq = 880;															// Defaults (typ.)
			_ampl = 0.3F;
			_ditMs = 80;

			setupTone();
		}

		//
		// Generate the tone data
		//
		private void setupTone()
		{
			_sineWave = new SineWaveProvider32();
			_sineWave.SetWaveFormat(_sampleRate, 1); // 16kHz mono
			_sineWave.Frequency = _freq;
			_sineWave.Amplitude = 0.25f;
			_waveOut.Init(_sineWave);
		}

		//
		// Publics
		//
		public int MaxLenMs
		{
			get { return _maxLen; }
			set
			{
				_maxLen = value;
			}
		}

		public float Frequency
		{
			get { return _freq; }
			set 
			{ 
				_freq = value;
				setupTone();
			}
		}

		public float Amplitude
		{
			get { return _ampl; }
			set
			{ 
				_ampl = value;
				setupTone();
			}
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
			_waveOut.Play();
			Thread.Sleep(ms);
			_waveOut.Stop();
		}
    }
}
