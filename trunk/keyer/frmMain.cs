//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		frmMain.cs
//
// FACILITY:	Morse Code Practice Keyer
//
// ABSTRACT:	Program that will read either the lerf/right mouse buttons or 
//				a key attached to a serial port's CTS/DSR pins and output CW 
//				tones or telegraph sounnder sounds.
//
// IMPORTANT:	The Sender delegate must be synchronous, that is, it must not
//				return until the symbol is actually sent (tone played, etc.).
//
// NOTE:		The .NET SerialPort class is too sluggish for high speed keyer
//				operation. THe NEW_COM #define uses code to call the ComPortCtrl
//				class, which uses low-level P/Invoke code to talk to the kernel
//				serial I/O driver.
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
// ??-Apr-10	rbd		Initial development
// 07-May-10	rbd		New ComPortCtrl class for low-level serial I/O
// 07-May-10	rbd		Refactored IambicKeyer into a separate assembly. Other
//						tweaks.
//

#define NEW_COM								// Define to use P/Invoke serial port I/O

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace com.dc3.morse
{
	public partial class frmMain : Form
	{
		//
		// Constants
		//
		private const int _tbase = 1200;	// Time base for millisecond timings. A standard 1WPM dit = 1200 ms mark, 1200 ms space

		//
		// Config
		//
		private int _keyerMode;				// Keyer mode, 0=straight 1=iambic
		private int _soundMode;				// Sound output, 0=tone 1=sounder
		private int _toneFreq;
		private int _sounderNum;
		private int _codeSpeed;				// Character WPM
		private int _serialPortNum;
		private bool _useSerial;

		//
		// State Variables
		//
#if NEW_COM
		private ComPortCtrl _serialPort;
#else
		private SerialPort _serialPort;		// Used for paddle via serial and optional real sounder output
#endif
		private DxTones _dxTones;
		private DxSounder _dxSounder;
		private IambicKeyer _iambicKeyer;
		private int _ctime;					// Character dit time (ms)
		private int _stime;					// Symbol space time (ms)
		private int _cstime;				// Character space time (ms)
		private int _wstime;				// Word space time (ms)

		//
		// Form construction, load, closing
		//
		public frmMain()
		{
			InitializeComponent();
		}

		private void _calcSpaceTime()					// Calculate space times for Farnsworth (word rate < char rate)
		{
			//
			// There are 50 units in "PARIS " - 36 are in the characters, 14 are in the spaces
			//
			int t_total = (_tbase / _codeSpeed) * 50;
			int t_chars = (_tbase / _codeSpeed) * 36;
			_stime = (t_total - t_chars) / 14;			// Time for 1 space (ms)
			_cstime = _stime * 2;						// Character and word spacing
			_wstime = _stime * 4;
		}

		private void frmMain_Load(object sender, EventArgs e)
		{
			_codeSpeed = (int)Properties.Settings.Default.CodeSpeed;
			_ctime = _tbase / _codeSpeed;
			_calcSpaceTime();

			_toneFreq = (int)Properties.Settings.Default.ToneFreq;
			_dxTones = new DxTones(this, 1000);
			_dxTones.Frequency = _toneFreq;
			_dxTones.DitMilliseconds = _ctime;

			_sounderNum = (int)Properties.Settings.Default.SounderNumber;
			_dxSounder = new DxSounder(this);
			_dxSounder.SoundIndex = _sounderNum;
			_dxSounder.DitMilliseconds = _ctime;

			_keyerMode = Properties.Settings.Default.KeyerMode;
			if (_keyerMode == 0)
				rbStraightKey.Checked = true;
			else
				rbIambicB.Checked = true;

			_soundMode = Properties.Settings.Default.SoundMode;
			if (_soundMode == 0)
				rbTone.Checked = true;
			else if (_soundMode == 1)
				rbSounder.Checked = true;
			else
				rbExtSounder.Checked = true;

			_sounderNum = (int)Properties.Settings.Default.SounderNumber;
			_serialPortNum = (int)Properties.Settings.Default.SerialPort;
			_useSerial = Properties.Settings.Default.UseSerial;
			if (_useSerial)
			{
				if (!OpenSerialPort())
					chkUseSerial.Checked = false;
			}

			_iambicKeyer = new IambicKeyer(SendCallback);
		}

		private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			Properties.Settings.Default.Save();
			if (_serialPort != null) _serialPort.Close();
			_iambicKeyer.Dispose();
		}

		//
		// Control events
		//

		private void nudSpeed_ValueChanged(object sender, EventArgs e)
		{
			_codeSpeed = (int)nudCodeSpeed.Value;
			_ctime = _tbase / _codeSpeed;
			_dxTones.DitMilliseconds = _ctime;
			_dxSounder.DitMilliseconds = _ctime;
			_calcSpaceTime();
		}

		private void nudToneFreq_ValueChanged(object sender, EventArgs e)
		{
			_toneFreq = (int)nudToneFreq.Value;
			_dxTones.Frequency = _toneFreq;
			_dxTones.PlayFor(100);
		}

		private void nudSounder_ValueChanged(object sender, EventArgs e)
		{
			_sounderNum = (int)nudSounder.Value;
			_dxSounder.SoundIndex = _sounderNum;
			_dxSounder.PlayFor(100);
		}

		private void rbTone_CheckedChanged(object sender, EventArgs e)
		{
			if (rbTone.Checked)
			{
				_soundMode = 0;
				Properties.Settings.Default.SoundMode = 0;
			}
			UpdateUI();
		}

		private void rbSounder_CheckedChanged(object sender, EventArgs e)
		{
			if (rbSounder.Checked)
			{
				_soundMode = 1;
				Properties.Settings.Default.SoundMode = 1;
			}
			UpdateUI();
		}

		private void rbExtSounder_CheckedChanged(object sender, EventArgs e)
		{
			if (rbExtSounder.Checked)
			{
				_soundMode = 2;
				Properties.Settings.Default.SoundMode = 2;
			}
			UpdateUI();
		}

		private void rbStraightKey_CheckedChanged(object sender, EventArgs e)
		{
			if (rbStraightKey.Checked)
			{
				_keyerMode = 0;
				Properties.Settings.Default.KeyerMode = 0;
			}
			UpdateUI();
		}

		private void rbIambicB_CheckedChanged(object sender, EventArgs e)
		{
			if (rbIambicB.Checked)
			{
				_keyerMode = 1;
				Properties.Settings.Default.KeyerMode = 1;
			}
			UpdateUI();
		}

		private void nudSerialPort_ValueChanged(object sender, EventArgs e)
		{
			_serialPortNum = (int)nudSerialPort.Value;
		}

		private void chkUseSerial_CheckedChanged(object sender, EventArgs e)
		{
			_useSerial = chkUseSerial.Checked;
			if (_serialPort != null)
			{
				_serialPort.Close();
				_serialPort = null;
			}
			if (_useSerial)
			{
				OpenSerialPort();
			}
			else
			{
				if (rbExtSounder.Checked)
					rbTone.Checked = true;
			}
			UpdateUI();
		}

		private void btnTestSerial_Click(object sender, EventArgs e)
		{
			btnTestSerial.Enabled = false;
			if (OpenSerialPort())
			{
				for (int i = 0; i < 4; i++)
				{
					_serialPort.RtsEnable = true;
					PreciseDelay.Wait(100);
					_serialPort.RtsEnable = false;
					PreciseDelay.Wait(100);
				}
				_serialPort.Close();
				_serialPort = null;
				MessageBox.Show(null, "Test complete, 4 dits sent.", "Sounder Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			btnTestSerial.Enabled = true;
		}

		private void pnlHotSpot_MouseDown(object sender, MouseEventArgs e)
		{
			if (_keyerMode == 0)
			{
				if (_soundMode == 0)
					_dxTones.Down();
				else
					_dxSounder.Down();
			}
			else
			{
				switch (e.Button)
				{
					case MouseButtons.Left:
						_iambicKeyer.KeyEvent(IambicKeyer.KeyEventType.DitPress);
						break;

					case MouseButtons.Right:
						_iambicKeyer.KeyEvent(IambicKeyer.KeyEventType.DahPress);
						break;
				}
			}
		}

		private void pnlHotSpot_MouseUp(object sender, MouseEventArgs e)
		{
			if (_keyerMode == 0)
			{
				if (_soundMode == 0)
					_dxTones.Up();
				else
					_dxSounder.Up();
			}
			else
			{
				switch (e.Button)
				{
					case MouseButtons.Left:
						_iambicKeyer.KeyEvent(IambicKeyer.KeyEventType.DitRelease);
						break;

					case MouseButtons.Right:
						_iambicKeyer.KeyEvent(IambicKeyer.KeyEventType.DahRelease);
						break;
				}
			}
		}

		private bool OpenSerialPort()
		{
			try
			{
#if NEW_COM
				_serialPort = new ComPortCtrl();
				_serialPort.ComPortPinChanged += new ComPortEventHandler(comPort_PinChanged);
				_serialPort.Open("COM" + _serialPortNum.ToString());
				_serialPort.DtrEnable = true;					// TODO is this needed? It was with the .NET SerialPort class

#else
				_serialPort = new SerialPort("COM" + _serialPortNum.ToString());
				_serialPort.PinChanged += new SerialPinChangedEventHandler(comPort_PinChanged);
				_serialPort.Open();
				_serialPort.DtrEnable = true;					// Not sure why, but these are needed to get pin events
#endif
				return true;
			}
			catch (Exception ex)
			{
				_serialPort.Close();
				_serialPort = null;
				MessageBox.Show(this, ex.Message, "Serial Port", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				chkUseSerial.Checked = false;
				return false;
			}
		}


		private void UpdateUI()
		{
			nudCodeSpeed.Enabled = rbIambicB.Checked;
			nudToneFreq.Enabled = rbTone.Checked;
			nudSounder.Enabled = rbSounder.Checked;
			btnTestSerial.Enabled = nudSerialPort.Enabled = !chkUseSerial.Checked;
			rbExtSounder.Enabled = chkUseSerial.Checked;
		}

		static bool _prevDSR = false;
		static bool _prevCTS = false;

		//
		// TODO need real debouncing! Also this logic needs reduction...
		//
#if NEW_COM
		private void comPort_PinChanged(Object sender, ComPortEventArgs e)
		{
			ComPortCtrl com = (ComPortCtrl)sender;
			bool curState;
			object lockObj = new object();
#else
		private void comPort_PinChanged(Object sender, SerialPinChangedEventArgs e)
		{
			SerialPort com = (SerialPort)sender;
#endif
			switch (e.EventType)
			{
#if NEW_COM
				case ComPortEventArgs.PinChange.DsrChanged:
#else
				case SerialPinChange.DsrChanged:
#endif
					lock (lockObj) { curState = com.DsrHolding; }
					Debug.Print("DSR -> " + curState.ToString() + " (prev = " + _prevDSR.ToString() + ")");
					if (curState == _prevDSR) return;							// Simple debouncing (typ.)
					if (curState)// && !_prevDSR)
					{
							if (_keyerMode == 0)
							{
								if (_soundMode == 0)
									_dxTones.Down();
								else
									_dxSounder.Down();
							}
							else
								_iambicKeyer.KeyEvent(IambicKeyer.KeyEventType.DitPress);
					}
					else //if (!curState && _prevDSR)
					{
						if (_keyerMode == 0)
						{
							if (_soundMode == 0)
								_dxTones.Up();
							else
								_dxSounder.Up();
						}
						else
							_iambicKeyer.KeyEvent(IambicKeyer.KeyEventType.DitRelease);
					}
					_prevDSR = curState; // com.DsrHolding;
					break;

#if NEW_COM
				case ComPortEventArgs.PinChange.CtsChanged:
#else
				case SerialPinChange.CtsChanged:
#endif
					lock (lockObj) { curState = com.CtsHolding; }
					Debug.Print("CTS -> " + curState.ToString() + " (prev = " + _prevCTS.ToString() + ")");
					if (curState == _prevCTS) return;
					if (curState)// && !_prevCTS)
					{
						if (_keyerMode == 0)
						{
							if (_soundMode == 0)
								_dxTones.Down();
							else
								_dxSounder.Down();
						}
						else
							_iambicKeyer.KeyEvent(IambicKeyer.KeyEventType.DahPress);
					}
					else //if (!curState & _prevCTS)
					{
						if (_keyerMode == 0)
						{
							if (_soundMode == 0)
								_dxTones.Up();
							else
								_dxSounder.Up();
						}
						else
							_iambicKeyer.KeyEvent(IambicKeyer.KeyEventType.DahRelease);
					}
					_prevCTS = curState; // com.CtsHolding;
					break;
			}
			//com.BreakState = true;
		}

		private void SendCallback(IambicKeyer.MorseSymbol S)
		{
			//Debug.Print(DateTime.Now.Ticks.ToString() + " " + S.ToString());
			if (S == IambicKeyer.MorseSymbol.Dit)
			{
				switch (_soundMode)
				{
					case 0:
						_dxTones.Dit();
						_dxTones.Space();
						break;
					case 1:
						_dxSounder.Dit();
						_dxSounder.Space();
						break;
					case 2:
						_serialPort.RtsEnable = true;
						PreciseDelay.Wait(_ctime);
						_serialPort.RtsEnable = false;
						PreciseDelay.Wait(_ctime);
						break;
				}
			}
			else
			{
				switch (_soundMode)
				{
					case 0:
						_dxTones.Dah();
						_dxTones.Space();
						break;
					case 1:
						_dxSounder.Dah();
						_dxSounder.Space();
						break;
					case 2:
						_serialPort.RtsEnable = true;
						PreciseDelay.Wait(_ctime * 3);
						_serialPort.RtsEnable = false;
						PreciseDelay.Wait(_ctime);
						break;
				}
			}
		}
	}
}
