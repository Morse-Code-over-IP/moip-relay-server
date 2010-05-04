//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		DxTones.cs
//
// FACILITY:	RSS to Morse tool
//
// ABSTRACT:	Generates radio tone sounds via Managed DirectX.
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
// 22-Apr-10	rbd		Play tones of arbitrary length
// 28-Apr-10	rbd		Remove sync parameter from Tone()
// 30-Apr-10	rbd		ITone intervace
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.DirectX.DirectSound;

namespace com.dc3.morse
{
    class DxTones : ITone
    {
		private const int _sampleRate = 44100;
		private const short _bitsPerSample = 16;
		private const short _bytesPerSample = 2;

        private Device _deviceSound;
		private int _maxLen;														// Max length tone
		private float _freq;
		private float _ampl;
		private int _ditMs;
		private int _startLatency;

		private byte[] _waveBuf;
		private WaveFormat _waveFmt;
		private BufferDescription _bufDesc;
		private SecondaryBuffer _secBuf;

		public DxTones(System.Windows.Forms.Control Handle, int MaxLenMs)
        {
			_maxLen = MaxLenMs;
			_freq = 880;															// Defaults (typ.)
			_ampl = 0.3F;
			_ditMs = 80;
			_startLatency = 0;

			_deviceSound = new Microsoft.DirectX.DirectSound.Device();
			_deviceSound.SetCooperativeLevel(Handle, CooperativeLevel.Priority);	// Up priority for quick response

			genWaveBuf();
		}

		//
		// Generate the tone data
		//
		private void genWaveBuf()
		{
			_waveBuf = GenTone(_freq, _ampl, _maxLen);

			_waveFmt = new WaveFormat();
			_waveFmt.BitsPerSample = (short)_bitsPerSample;
			_waveFmt.Channels = 1;
			_waveFmt.BlockAlign = _bytesPerSample;

			_waveFmt.FormatTag = WaveFormatTag.Pcm;
			_waveFmt.SamplesPerSecond = _sampleRate; 
			_waveFmt.AverageBytesPerSecond = _sampleRate * _bytesPerSample;

			_bufDesc = new BufferDescription(_waveFmt);
			_bufDesc.DeferLocation = true;
			_bufDesc.BufferBytes = _waveBuf.Length;
			_bufDesc.ControlEffects = false;										// Necessary for short tones
			_bufDesc.GlobalFocus = true;											// Enable audio when program is in background

			_secBuf = new SecondaryBuffer(_bufDesc, _deviceSound);
			_secBuf.Write(0, _waveBuf, LockFlag.EntireBuffer);

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
		public int MaxLenMs
		{
			get { return _maxLen; }
			set
			{
				_maxLen = value;
				genWaveBuf();
			}
		}

		public float Frequency
		{
			get { return _freq; }
			set 
			{ 
				_freq = value;
				genWaveBuf();
			}
		}

		public float Amplitude
		{
			get { return _ampl; }
			set
			{ 
				_ampl = value;
				genWaveBuf();
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
			Thread.Sleep(_ditMs);
		}

		public void PlayFor(int ms)
		{
			_secBuf.SetCurrentPosition((_sampleRate * (_maxLen - ms)) * 2 / 1000);
			_secBuf.Play(0, BufferPlayFlags.Default);
			Thread.Sleep(ms);
		}

		public void Stop()
		{
			_secBuf.Stop();
		}
    }
}
