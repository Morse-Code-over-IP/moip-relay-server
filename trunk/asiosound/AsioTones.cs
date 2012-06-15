//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		AsioTones.cs
//
// FACILITY:	ASIO Tone Generator
//
// ABSTRACT:	Generates radio tone sounds via the low-latency Steinberg ASIO
//				audio driver
//
// ENVIRONMENT:	Microsoft.NET 2.0/3.5
//				Developed under Visual Studio.NET 2008
//
// AUTHOR:		Bob Denny, <rdenny@dc3.com>
//
// Edit Log:
//
// When			Who		What
//----------	---		-------------------------------------------------------
// 18-May-12	rbd		SF #3522625 Initial edit
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using BlueWave.Interop.Asio;

namespace com.dc3.morse
{
	public class AsioTones : ITone, IDisposable
	{
		private AsioDriver _drvr = null;
		private int _sampleRate;
		private int _maxLen;														// Max length tone
		private float _freq;
		private double _filtCoeff;
		private float _volume;
		private int _ditMs;
		private int _riseFallTime;

		private float[] _toneBuf;
		private int _toneIndex;
		private object _toneLock;

		private double _cycleSamples;
		private int _tailStart;
		private bool _playTail;


		public AsioTones(AsioDriver Driver, int MaxLenMs)
		{
			_drvr = Driver;
			_sampleRate = (int)_drvr.SampleRate;
			_maxLen = MaxLenMs;
			this.Frequency = 880;
			_volume = 1.0f;
			_ditMs = 80;
			_riseFallTime = 10;
			_toneIndex = int.MaxValue;
			_toneLock = new object();
			_playTail = false;

			//
			// We just start this thing up right now and let it run.
			//
			_drvr.BufferUpdate += new EventHandler(AsioDriver_BufferUpdate);
			_drvr.CreateBuffers(false);
		}

		//
		// helper function for creating sound
		//
		private float[] GenTone(double frequency, double amp, int duration)
		{
			int length = (int)(_sampleRate * ((double)duration + (_riseFallTime / 2.0)) / 1000.0);
			float[] wavedata = new float[length];
			double timeScale = frequency * 2 * Math.PI / (double)_sampleRate;
			int envelopeSamples = (int)((double)_sampleRate * _riseFallTime / 1000.0);

			_cycleSamples = _sampleRate / _freq;						// Used to jump to the end envelope in Up()
			_tailStart = length - envelopeSamples;
			double xo = 0;
			double yo = 0;
			lock (_toneLock)
			{
				for (int i = 0; i < length; i++)
				{
					double a0 = amp;
					if (i < envelopeSamples)
						a0 *= Math.Min(0.5 - (0.5 * Math.Cos(Math.PI * i / envelopeSamples)), 1.0);
					else if (i >= (length - envelopeSamples))
						a0 *= Math.Min(0.5 - (0.5 * Math.Cos(Math.PI * (length - i) / envelopeSamples)), 1.0);

					double xn = Math.Sin(i * timeScale);

					double yn = xn - (Math.Exp((-Math.PI * frequency / (50.0 * _sampleRate))) * yo); // Low pass filter
					xo = xn;
					yo = yn;
					wavedata[i] = (float)(yn * a0 * 1.95);
				}
			}
			return wavedata;
		}

		//
		// Buffer event handler
		//
		private void AsioDriver_BufferUpdate(object sender, EventArgs e)
		{
			AsioDriver driver = sender as AsioDriver;
			Channel L = driver.OutputChannels[0];
			Channel R = driver.OutputChannels[1];
			lock (_toneLock)
			{
				if (_playTail && _toneIndex < _tailStart)
				{
					double jumpCycles = Math.Truncate((double)(_tailStart - _toneIndex) / (double)_cycleSamples);
					float t1 = _toneBuf[_toneIndex];
					_toneIndex += (int)(jumpCycles * _cycleSamples);
					float t2 = _toneBuf[_toneIndex];
				}
				_playTail = false;
				for (int i = 0; i < L.BufferSize; i++)
				{
					if (_toneBuf == null || _toneIndex >= _toneBuf.Length)
					{
						L[i] = R[i] = 0.0f;
					}
					else
					{
						L[i] = R[i] = _toneBuf[_toneIndex];
						_toneIndex += 1;
					}
				}
			}
		}

		//
		// After making all of the settings call this to start the process.
		//
		public void Start()
		{
			int z = _drvr.Start();
		}

		#region ITone Members

		public float Frequency
		{
			get { return _freq; }
			set
			{
				_freq = value;
				_filtCoeff = Math.Exp((-Math.PI * _freq / (50.0 * _sampleRate)));	// Rolloff at freq / 50
			}
		}

		public float Volume
		{
			get { return _volume; }
			set { _volume = value; }
		}

		public int RiseFallTime
		{
			get { return _riseFallTime; }
			set { _riseFallTime = value; }
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
			PreciseDelay.Wait(_ditMs);
		}

		public void PlayFor(int ms)
		{
			_toneBuf = GenTone(_freq, _volume, ms);
			lock (_toneLock) { _toneIndex = 0; }
			PreciseDelay.Wait(ms);
		}

		public void Stop()
		{
			lock (_toneLock) { _toneIndex = int.MaxValue; }
		}

		public void Down()
		{
			_toneBuf = GenTone(_freq, _volume, _maxLen);
			lock (_toneLock) { _toneIndex = 0; }
		}

		public void Up()
		{
			lock (_toneLock) { _playTail = true; }
			PreciseDelay.Wait(100);
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if (_drvr != null)
			{
				_drvr.Stop();
				_drvr.Release();
			}
		}

		#endregion

	}
}
