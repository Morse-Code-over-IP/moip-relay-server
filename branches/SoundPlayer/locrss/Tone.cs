using System;
using System.Media;
using System.Threading;
using System.IO;

namespace com.dc3.morse
{
	class Tone
	{
		private SoundPlayer _player;
		private int _ditMs;

		public Tone(int SampleRate, double Frequency, double Amplitude, int Duration)
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
			bWriter.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));				// Main RIFF header
			bWriter.Write((int)0);													// Overall length (placeholder for now)
			bWriter.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt "));			// Wave type followed by format chunk header
			bWriter.Write((int)(18));												// PCM wave format length
			bWriter.Write((short)1);												// PCM type
			bWriter.Write((short)1);												// Mono, 1 channel
			bWriter.Write((int)SampleRate);											// Sample rate Hz
			bWriter.Write((int)2 * SampleRate);										// Avg bps is BlockALign (2) * sample rate
			bWriter.Write((short)2);												// 2-byte alignment, (1 chan, 16 bits -> 2 bytes)
			bWriter.Write((short)16);												// 16-bit samples
			bWriter.Write((short)0);												// No extra data
			// No fact chunk for PCM
			bWriter.Write(System.Text.Encoding.ASCII.GetBytes("data"));				// Data chunk header
			bWriter.Write(data.Length);												// Data chunk length, bytes
			bWriter.Write(data);													// Tone data
			bWriter.Flush();														// Flush to stream
			bWriter.Seek(4, SeekOrigin.Begin);										// Rewind
			bWriter.Write((int)(wavStrm.Length - 8));								// Write overall length to placeholder
			bWriter.Flush();
			wavStrm.Seek(0, SeekOrigin.Begin);
			_player = new SoundPlayer(wavStrm);
			_player.Load();
			bWriter.Close();

			_ditMs = 80;
		}

		public void Play()
		{
			_player.Play();
		}

		public void Stop()
		{
			_player.Stop();
		}

		public int DitMilliseconds
		{
			get { return _ditMs; }
			set { _ditMs = value; }
		}

		public void Dit()
		{
			Mark(_ditMs);
		}

		public void Dah()
		{
			Mark(_ditMs * 3);
		}

		public void Space()
		{
			Thread.Sleep(_ditMs);
		}

		public void Mark(int ms)
		{
			_player.Play();
			Thread.Sleep(ms);
			_player.Stop();
		}

	}
}
