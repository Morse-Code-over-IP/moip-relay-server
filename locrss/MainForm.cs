//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		MainForm.cs
//
// FACILITY:	Morse Code News Reader
//
// ABSTRACT:	Reads news from RSS and Twitter feeds and generates Morse code tones
//				or telegraph sounds. This is the main form for the app.
//
// ENVIRONMENT:	Microsoft.NET 2.0/3.5
//				Developed under Visual Studio.NET 2008
//
//
// NOTE ON CODE AND CHAR SPEED SPINBOXES:  If the CharSpeed spinbox is auto-bound
//				to the Application Setting, the interaction between it and the
//				CodeSPeed spinbox is strange. When the CodeSpeed ValueChanged
//				event fires, and needs to increase the code speed to match
//				char speed cannot be less then code speed!), a complex series
//				of events takes plaace, including re-binding all of the settings
//				which resets the CodeSpeed box back to its pre-spin value. 
//				Making a long story short, manually binding the CharSpeed spinbox
//				avoids this problem. Weird.
//
// AUTHOR:		Bob Denny, <rdenny@dc3.com>
//
// Edit Log:
//
// When			Who		What
//----------	---		-------------------------------------------------------
// ??-Apr-10	rbd		Initial editing and development
// 28-Apr-10	rbd		1.1.0 Merge SoundPlayer support and make it the default.
//						For Mono, do not include null as first param to 
//						MessageBox.Show(). Mono has no sound so doesn't work!
//						Add Timing Comp control for tone start latency. Fix
//						tab ordering. Fix range of code speed control.
// 30-Apr-10	rbd		1.2.0 - Reorganize, add spark gap, resurrect DirectX,
//						move DX and TimingComp to separate form.  Add message
//						to cache only is really sent completely. New class
//						MorseMessage. Increase URL list capacity to 16. Handle
//						authenticated feeds!
// 01-May-10	rbd		1.3.0 - Make this runnable on systems which don't have
//						Microsoft.DirectX via second project which omits DX
//						sound classes and ref to DirectX.DirectSound
// 03-May-10	rbd		1.3.0 - Much better solution: Include the Managed DirectX
//						assemblies in the setup. This allows Fusion to find them
//						if the DirectX End User Runtime is not installed. Refactor
//						audio interfaces, now just two (tone and wav). Fix DX/Sound
//						switching (on close SoundCfg dialog).
// 03-May-10	rbd		1.3.2 - Switched to new PreciseDelay.Wait for timing, uses
//						multimedia timer. Make sounder test dits at 20WPM.
// 04-May-10	rbd		1.3.3 - Low level serial control for physical sounder, 
//						change test dits to 20WPM. Misc cleanups.
// 11-May-10	rbd		1.5.1 - Volume control!
// 13-May-10	rbd		1.5.1 - Feedburner, CNN, others don't send Content-Length
//						so no longer require it. Also, Feedburner sends EDT (daylight)
//						pubDates during DST, handle that.
// 17-May-10	rbd		1.5.1 - Fox sends EST on EDT times, detect and subtract an
//						hour. Add Unknown Char Handler so prevent error code being
//						send for unknown chars (usually unicode stuff) that isn't 
//						caught by the fixup code.
// 21-May-10	rbd		1.5.0 - Change doc root	from index.html to keyer.html so 
//						can share same doc folder with keyer in end-user install.
// 02-Jun-11	rbd		1.8.0 - TimingComp is now dual-purpose, used for rise/fall
//						time when running in DirectX sound mode.
// 03-Jun-11	rbd		2.0.0 - Start addition of Twitter feed sourcing. 
// 06-Jun-11	rbd		2.0.0 - Much more logic changes. Too numerous for description here.
// 08-Jun-11	rbd		2.0.0 - Finishing touches on this. 
// 10-Jun-11	rbd		2.1.1 - Search ResultType now an enum (TwitterVB 3.1)
// 13-Jun-11	rbd		2.1.2 - SF Issues 3315998 and 3316001 relating to message numbering.
// 14-Jun-11	rbd		2.1.2 - Story age now means "don't send stories older than this" - period.
//						Add setting to ignore retweets (default true). No UI for this.
//						Add setting for break time between stories (def 5 sec). No UI for this.
//						Add setting for tracing to DebugView, and a bunch of tracing.
//						Capitalize Twitter screen names, put them into the same place (after
//						the header) as they are in RSS posts. 
// 24-Jun-11	rbd		2.1.2 - SF 3329000 Increase max speed to 60 (ridiculous but ...)
// 14-Jul-11	rbd		2.1.4 - Add noise for CW tones and spark gap.
// 28-Nov-11	rbd		2.2.0 - SF #3232844 - DXSounds take audio device GUID (always 
//						'primary' for this app). SF #3444355 Hardwire to DirectX sounds in
//						anticipation of removing the programmed sound stuff.
// 11-Apr-12	rbd		2.4.0 - SF #3516969 - Add Atom feed capability.
//
//
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Media;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Windows.Forms;
using System.Xml;

using TwitterVB2;

using com.dc3.morse;

namespace com.dc3
{
	public partial class MainForm : Form
	{
		private const string _traceCatMorseNews = "MorseNews";

		public enum CodeMode { International, American }
		public enum SoundMode { Tone, Spark, Sounder }
		//
		// Represents an RSS or Twitter item with a correct pubDate
		//
		private class DatedItem
		{
			public DatedItem()
			{
				rssItem = null;													// [sentinel] (typ.)
				atomItem = null;
				atomNsMgr = null;
				twType = null;
				twScreenName = null;
				twRawText = null;
				pubDate = DateTime.MinValue;
			}
			public DateTime pubDate { get; set; }
			public XmlNode rssItem { get; set; }
			public XmlNode atomItem { get; set; }
			public XmlNamespaceManager atomNsMgr { get; set; }
			public string twType { get; set; }
			public string twScreenName { get; set; }
			public string twRawText { get; set; }
		}

		private class MorseMessage
		{
			public string Title { get; set; }
			public string Contents { get; set; }
		}
		
		//
		// Settings
		//
		private CodeMode _codeMode;
		private SoundMode _soundMode;
		private int _pollInterval;
		private int _codeSpeed;
		private int _charSpeed;
		private bool _directX;
		private string _soundDevGUID;
		private int _toneFreq;
		private int _noiseLevel;
		private int _timingComp;
		private int _sounderNum;
		private int _sparkNum;
		private int _storyAge;
		private string _feedUrl;
		private int _serialPortNum;
		private bool _useSerial;
		private bool _ignoreRetweets;											// Settings only from here on
		private bool _debugTracing;
		private int _breakTimeMs;

		//
		// State variables
		//
		private Thread _runThread = null;
		private bool _run;
		private Dictionary<string, DateTime> _titleCache = new Dictionary<string, DateTime>();
		private Thread _titleExpireThread = null;
		private int _msgNr = 0;
		private ITone _tones;
		private IAudioWav _sounder;
		private IAudioWav _spark;
		private SoundPlayer _noise;
		private DateTime _lastPollTime;
		private DateTime _lastMsgSendDate = DateTime.MinValue;					// [sentinel]
		private ComPortCtrl _serialPort;

		//
		// Form ctor and event methods
		//
		public MainForm()
		{
			InitializeComponent();
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			Debug.Print("Load");
			_pollInterval = (int)nudPollInterval.Value;
			_codeSpeed = (int)nudCodeSpeed.Value;
			// -- MANUALLY BOUND - SEE NOTE ABOVE --
			nudCharSpeed.Value = Properties.Settings.Default.CharSpeed;
			nudCharSpeed.Minimum = (decimal)_codeSpeed;
			_charSpeed = (int)nudCharSpeed.Value;
			// ------ FROM SOUND CONFIG FORM -------
			// SF #3444355 _directX = Properties.Settings.Default.DirectX;
			_directX = true;
			_soundDevGUID = Properties.Settings.Default.SoundDevGUID;
			if (_directX)
				_timingComp = Properties.Settings.Default.RiseFall;
			else
				_timingComp = (int)Properties.Settings.Default.TimingComp;
			_noiseLevel = Properties.Settings.Default.NoiseLevel;
			// -------------------------------------
			_toneFreq = (int)nudToneFreq.Value;
			_sounderNum = (int)nudSounder.Value;
			_sparkNum = (int)nudSpark.Value;
			_storyAge = (int)nudStoryAge.Value;
			_feedUrl = cbFeedUrl.Text;
			_serialPortNum = (int)nudSerialPort.Value;
			_useSerial = chkUseSerial.Checked;
			_serialPort = null;
			_noise = null;
			_run = false;

			if (Properties.Settings.Default.CodeMode == 0)
				rbInternational.Checked = true;									// Triggers CheckedChanged (typ.)
			else
				rbAmerican.Checked = true;
			switch (Properties.Settings.Default.SoundMode)
			{
				case 0:
					rbTone.Checked = true;
					break;
				case 1:
					rbSounder.Checked = true;
					break;
				case 2:
					rbSpark.Checked = true;
					break;
			}

			foreach (string uri in Properties.Settings.Default.LRU)
				cbFeedUrl.Items.Add(uri);
			if (cbFeedUrl.Text == "")											// Force something into feed URL
				cbFeedUrl.Text = Properties.Settings.Default.LRU[0];

			_titleExpireThread = new Thread(new ThreadStart(TitleExpireThread));
			_titleExpireThread.Name = "Title Expiry";
			_titleExpireThread.Start();

			SetupSound();
			PreciseDelay.Initialize();

			statBarLabel.Text = "Ready";
			statBarCrawl.Text = "";
			UpdateUI();

			_ignoreRetweets = Properties.Settings.Default.IgnoreRetweets;
			_debugTracing = Properties.Settings.Default.DebugTracing;
			_breakTimeMs = Properties.Settings.Default.BreakTime * 1000;

			this.Left = Properties.Settings.Default.SavedWinX;					// TODO safety these
			this.Top = Properties.Settings.Default.SavedWinY;
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Properties.Settings.Default.SavedWinX = this.Left;
			Properties.Settings.Default.SavedWinY = this.Top;

			if (_titleExpireThread != null)
			{
				_titleExpireThread.Interrupt();
				_titleExpireThread.Join(1000);
			}
			if (_runThread != null)
			{
				_runThread.Interrupt();
				_runThread.Join(1000);
			}
			if (_serialPort != null)
				_serialPort.Close();
			_serialPort = null;
			PreciseDelay.Cleanup();
			Properties.Settings.Default.LRU.Clear();
			foreach (string uri in cbFeedUrl.Items)
				Properties.Settings.Default.LRU.Add(uri);
			Properties.Settings.Default.Save();
		}

		private void btnFeedList_Click(object sender, EventArgs e)
		{
			ofnDlg.InitialDirectory = Properties.Settings.Default.LastListDir;
			if (ofnDlg.ShowDialog() == DialogResult.OK)
			{
				Uri furi = new Uri(ofnDlg.FileName);
				int index = cbFeedUrl.Items.Add(furi.AbsoluteUri);
				cbFeedUrl.SelectedIndex = index;
				Properties.Settings.Default.LastListDir = Path.GetDirectoryName(ofnDlg.FileName);
			}

		}

		private void cbFeedUrl_TextChanged(object sender, EventArgs e)
		{
			_feedUrl = cbFeedUrl.Text;
			UpdateUI();															// For disabling Start if empty
		}

		private void nudPollInterval_ValueChanged(object sender, EventArgs e)
		{
			_pollInterval = (int)nudPollInterval.Value;
		}

		private void nudStoryAge_ValueChanged(object sender, EventArgs e)
		{
			_storyAge = (int)nudStoryAge.Value;
		}

		private void nudCodeSpeed_ValueChanged(object sender, EventArgs e)
		{
			_codeSpeed = (int)nudCodeSpeed.Value;
			if (_codeSpeed > _charSpeed)
				nudCharSpeed.Value = (decimal)_codeSpeed;
			nudCharSpeed.Minimum = (decimal)_codeSpeed;

		}

		private void nudCharSpeed_ValueChanged(object sender, EventArgs e)
		{
			// -- MANUALLY BOUND - SEE NOTE ABOVE --
			_charSpeed = (int)nudCharSpeed.Value;
			Properties.Settings.Default.CharSpeed = (decimal)_charSpeed;
			// -------------------------------------
		}

		private void nudToneFreq_ValueChanged(object sender, EventArgs e)
		{
			_toneFreq = (int)nudToneFreq.Value;
			_tones.Frequency = _toneFreq;
			_tones.PlayFor(100);
		}

		private void nudSpark_ValueChanged(object sender, EventArgs e)
		{
			_sparkNum = (int)nudSpark.Value;
			_spark.SoundIndex = _sparkNum;
			_spark.PlayFor(100);
		}

		private void nudSounder_ValueChanged(object sender, EventArgs e)
		{
			_sounderNum = (int)nudSounder.Value;
			_sounder.SoundIndex = _sounderNum;
			_sounder.PlayFor(100);
		}

		private void nudSerialPort_ValueChanged(object sender, EventArgs e)
		{
			_serialPortNum = (int)nudSerialPort.Value;
		}

		private void rbInternational_CheckedChanged(object sender, EventArgs e)
		{
			if (rbInternational.Checked)
			{
				_codeMode = CodeMode.International;
				Properties.Settings.Default.CodeMode = 0;
			}
			UpdateUI();
		}

		private void rbAmerican_CheckedChanged(object sender, EventArgs e)
		{
			if (rbAmerican.Checked)
			{
				_codeMode = CodeMode.American;
				Properties.Settings.Default.CodeMode = 1;
			}
			UpdateUI();
		}

		private void rbTone_CheckedChanged(object sender, EventArgs e)
		{
			if (rbTone.Checked)
			{
				_soundMode = SoundMode.Tone;
				Properties.Settings.Default.SoundMode = 0;
			}
			UpdateUI();
		}

		private void rbSpark_CheckedChanged(object sender, EventArgs e)
		{
			if (rbSpark.Checked)
			{
				_soundMode = SoundMode.Spark;
				Properties.Settings.Default.SoundMode = 2;
			}
			UpdateUI();
		}

		private void rbSounder_CheckedChanged(object sender, EventArgs e)
		{
			if (rbSounder.Checked)
			{
				_soundMode = SoundMode.Sounder;
				Properties.Settings.Default.SoundMode = 1;
			}
			UpdateUI();
		}

		private void tbVolume_Scroll(object sender, EventArgs e)
		{
			_tones.Volume = _sounder.Volume = _spark.Volume = tbVolume.Value / 10.0F;
			if (_run) return;
			switch (_soundMode)
			{
				case SoundMode.Tone:
					_tones.PlayFor(60);
					break;
				case SoundMode.Spark:
					_spark.PlayFor(60);
					break;
				case SoundMode.Sounder:
					_sounder.PlayFor(60);
					break;
			}
		}

		private void chkUseSerial_CheckedChanged(object sender, EventArgs e)
		{
			_useSerial = chkUseSerial.Checked;
			UpdateUI();
		}

		private void picTestSerial_Click(object sender, EventArgs e)
		{
			try
			{
				picTestSerial.Enabled = false;
				ComPortCtrl S = new ComPortCtrl();
#if MONO_BUILD
				S.Open("/dev/tty.serial" + _serialPortNum.ToString());
#else
				S.Open("COM" + _serialPortNum.ToString());
#endif
				for (int i = 0; i < 4; i++)										// 4 dits @ 20 WPM
				{
					S.RtsEnable = true;
					PreciseDelay.Wait(60);
					S.RtsEnable = false;
					PreciseDelay.Wait(60);
				}
				S.Close();
				S.Dispose();
				MessageBox.Show("Test complete, 4 dits sent.", "Sounder Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Sounder Test", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			}
			picTestSerial.Enabled = true;
		}


		private void btnClearCache_Click(object sender, EventArgs e)
		{
			lock (_titleCache)
			{
				_titleCache.Clear();
			}
			MessageBox.Show("Seen stories have been forgotten", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void mnuRemoveFromList_Click(object sender, EventArgs e)
		{
			cbFeedUrl.Items.RemoveAt(cbFeedUrl.SelectedIndex);
		}

		private void mnuUrlResetToDefault_Click(object sender, EventArgs e)
		{
			//
			// This is really obscure. I found an article on the almighty StackOverflow
			// http://stackoverflow.com/questions/49269/reading-default-application-settings-in-c
			// which showed how to get the original/default value of an application
			// setting. But doing this does not return a collection - it returns raw XML.
			// So we have to parse that and get the LRU list that way. Oh well...
			//
			string lru = (string)Properties.Settings.Default.Properties["LRU"].DefaultValue;
			XmlDocument listXml = new XmlDocument();
			listXml.LoadXml(lru);
			XmlNodeList items = listXml.SelectNodes("/ArrayOfString/string");
			cbFeedUrl.Items.Clear();
			foreach (XmlNode item in items)
				cbFeedUrl.Items.Add(item.InnerText);
			cbFeedUrl.Text = items.Item(0).InnerText;
		}

		private void llSoundCfg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			picSoundCfg_Click(sender, new EventArgs());
		}

		private void picSoundCfg_Click(object sender, EventArgs e)
		{
			SoundCfgForm sf = new SoundCfgForm(_soundDevGUID);
			sf.UseDirectX = Properties.Settings.Default.DirectX;
			sf.NoiseLevel = Properties.Settings.Default.NoiseLevel;
			if (sf.UseDirectX)
				sf.TimingComp = Properties.Settings.Default.RiseFall;
			else
				sf.TimingComp = (int)Properties.Settings.Default.TimingComp;
			if (sf.ShowDialog(this) == DialogResult.OK)
			{
				_soundDevGUID = sf.SoundDevGUID;
				_timingComp = sf.TimingComp;
				_tones.StartLatency = _timingComp;
				_sounder.StartLatency = _timingComp;
				_spark.StartLatency = _timingComp;
				//if (_directX != sf.UseDirectX)									// Switching sound technology
				//{
				//	_directX = sf.UseDirectX;
					SetupSound();
				//}
				//else
				//	_directX = sf.UseDirectX;
				//if (_directX)
					Properties.Settings.Default.RiseFall = _timingComp;
				//else
				//	Properties.Settings.Default.TimingComp = (decimal)_timingComp;
				Properties.Settings.Default.DirectX = _directX;
				_noiseLevel = sf.NoiseLevel;
				Properties.Settings.Default.NoiseLevel = _noiseLevel;
				Properties.Settings.Default.SoundDevGUID = _soundDevGUID;
			}
		}

		private void llHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			picHelp_Click(sender, new EventArgs());
		}

		private void picHelp_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start(Path.GetDirectoryName(Application.ExecutablePath) + "\\doc\\news.html");
		}

		private void llRSSFeeds_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			picRSS_Click(sender, new EventArgs());
		}

		private void picRSS_Click(object sender, EventArgs e)
		{
			System.Diagnostics.Process.Start("http://news.yahoo.com/page/rss");
		}

		private void SetupSound()
		{
#if !MONO_BUILD
			if (_directX)
			{
				Guid guid = new Guid(_soundDevGUID);
				_tones = new DxTones(this, guid, 1000);
				_sounder = new DxSounder(this, guid);
				_spark = new DxSpark(this, guid);
			}
			else
			{
				_tones = new SpTones();
				_sounder = new SpSounder();
				_spark = new SpSpark();
			}
#else
			_tones = new SpTones();
			_sounder = new SpSounder();
			_spark = new SpSpark();
#endif
			_tones.Frequency = _toneFreq;
			_tones.Volume = tbVolume.Value / 10.0F;
			_tones.StartLatency = _timingComp;
			_sounder.SoundIndex = _sounderNum;
			_sounder.Volume = tbVolume.Value / 10.0F;
			_sounder.StartLatency = _timingComp;
			_spark.SoundIndex = _sparkNum;
			_spark.Volume = tbVolume.Value / 10.0F;
			_spark.StartLatency = _timingComp;
		}

		private void UpdateUI()
		{
			bool enable = !_run;
			btnStartStop.Enabled = (cbFeedUrl.Text != "");
			btnFeedList.Enabled = enable;
			cbFeedUrl.Enabled = enable;
			nudCodeSpeed.Enabled = enable;
			nudCharSpeed.Enabled = enable;
			nudPollInterval.Enabled = enable;
			nudStoryAge.Enabled = enable;
			nudSerialPort.Enabled = enable;
			chkUseSerial.Enabled = enable;
			rbAmerican.Enabled = enable;
			rbInternational.Enabled = enable;
			picTestSerial.Enabled = enable & !chkUseSerial.Checked;

			enable = enable & !_useSerial;
			nudSpark.Enabled = enable && rbSpark.Checked;
			nudSounder.Enabled = enable & rbSounder.Checked;
			nudToneFreq.Enabled = enable & rbTone.Checked;
			rbTone.Enabled = enable;
			rbSounder.Enabled = enable;
			rbSpark.Enabled = enable;
			llSoundCfg.Enabled = enable;
			picSoundCfg.Enabled = enable;
		}

		private void btnStartStop_Click(object sender, EventArgs e)
		{
			if (!_run)
			{
				if (!cbFeedUrl.Items.Contains(cbFeedUrl.Text))					// Add new URIs to combo box when actually USED!
				{
					while (cbFeedUrl.Items.Count > 16)							// Safety catch only
						cbFeedUrl.Items.RemoveAt(0);
					if (cbFeedUrl.Items.Count == 16)								// If full, remove last item
						cbFeedUrl.Items.RemoveAt(15);
					cbFeedUrl.Items.Insert(0, cbFeedUrl.Text);					// Insert new item at top
				}
				else
				{
					int idx = cbFeedUrl.FindStringExact(cbFeedUrl.Text);
					if (idx > 0)												// If not already at top, move to top
					{
						string uri = cbFeedUrl.Text;							// Save this, next stmt clears text!
						cbFeedUrl.Items.RemoveAt(idx);
						cbFeedUrl.Items.Insert(0, uri);
						cbFeedUrl.Text = uri;
					}
				}

				_runThread = new Thread(new ThreadStart(Run));
				_runThread.Name = "RSS engine";
				_runThread.Start();
				btnStartStop.Text = "Stop";
				_run = true;
			}
			else
			{
				switch (_soundMode)
				{
					case SoundMode.Tone:
						_tones.Stop();
						break;
					case SoundMode.Spark:
						_spark.Stop();
						break;
					case SoundMode.Sounder:
						_sounder.Stop();
						break;
				}
				if (_runThread != null)
				{
					_runThread.Interrupt();
					_runThread.Join(1000);
				}
				if (_serialPort != null)
					_serialPort.Close();
				_serialPort = null;
				btnStartStop.Text = "Start";
				_run = false;
			}
			statBarCrawl.Text = "";
			statBarLabel.Text = "Ready";
			UpdateUI();
		}

		//
		// Cross-thread methods for the worker thread and the status bar
		//
		delegate void SetTextCallback(string text, bool trace);

		private void SetStatus(string text, bool trace)
		{
			if (this.statusStrip1.InvokeRequired)
			{
				SetTextCallback d = new SetTextCallback(SetStatus);
				this.Invoke(d, new object[] { text, trace });
			}
			else
			{
				if (trace && _debugTracing)
					Trace.WriteLine(text, _traceCatMorseNews);
				statBarLabel.Text = text;
			}
		}

		delegate void CrawlerCallback(string text);

		private void SetCrawler(string text)
		{
			if (this.statusStrip1.InvokeRequired)
			{
				CrawlerCallback d = new CrawlerCallback(SetCrawler);
				this.Invoke(d, new object[] { text });
			}
			else
			{
				statBarCrawl.Text = text;
			}
		}

		private void AddToCrawler(string text)
		{
			if (this.statusStrip1.InvokeRequired)
			{
				CrawlerCallback d = new CrawlerCallback(AddToCrawler);
				this.Invoke(d, new object[] { text });
			}
			else
			{
				statBarCrawl.Text += text;
			}
		}

		delegate void StopOnErrorCallback();

		private void StopOnError()
		{
			if (this.btnStartStop.InvokeRequired)
			{
				StopOnErrorCallback d = new StopOnErrorCallback(StopOnError);
				this.Invoke(d, new object[] { });
			}
			else
			{
				if (_run)
					btnStartStop_Click(new object(), new EventArgs());
			}
		}

		// ===========
		// Other Logic
		// ===========

		//
		// Strip stuff from text that can/should not be sent by Morse Code
		// HTML-decodes, then removes URLs, HTML tags, and non-Morse characters,
		// and finally removes runs of more than one whitespace character,
		// replacing with a single space and UPPERcases it. 
		//
		private string GetMorseInputText(string stuff)
		{
			string buf = HttpUtility.HtmlDecode(stuff);							// Decode HTML entities, etc.
			buf = Regex.Replace(buf, "http://[\\S]*", "", RegexOptions.IgnoreCase); // Remove URLs
			buf = Regex.Replace(buf, "<[^>]*>", " ");							// Remove HTML tags completely
			buf = Regex.Replace(buf, "[\\~\\^\\%\\|\\#\\<\\>\\*\\u00A0]", " ");	// Some characters we don't have translations for => space
			buf = Regex.Replace(buf, "[\\‘\\’\\`]", "'");						// Unicode left/right single quote, backtick -> ASCII single quote
			buf = Regex.Replace(buf, "[\\{\\[]", "(");							// Left brace/bracket -> left paren
			buf = Regex.Replace(buf, "[\\}\\]]", ")");							// Right brace/bracket -> Right paren
			buf = Regex.Replace(buf, "[—–]", "-");								// Unicode emdash/endash -> hyphen
			buf = buf.Replace("(AP)", "AP");									// Special case for AP Atom feeds
			buf = Regex.Replace(buf, "\\s\\s+", " ").Trim().ToUpper();			// Compress running whitespace, fold all to upper case

			return buf;
		}

		//
		// Tries to get a correct pubDate (local time) from the RSS or Atom
		// <item> node. Easy for Atom ISO 8601 date format.
		//
		// RSS would be much easier if the @#$%^& date in pubDate wasn't the
		// old lazy-man's unix strftime()/RFC822 format, with its "whatever"
		// time zone abbrevations!!!! FeedBurner uses "EST" egad.
		//
		private DateTime GetPubDateUtc(XmlNode item, XmlNamespaceManager nsmgr)
		{
			DateTime corrDate;

			XmlNode n = item.SelectSingleNode("pubDate");						// Try RSS dfirst
			if (n == null && nsmgr != null)
			{
				n = item.SelectSingleNode("at:updated", nsmgr);					// Then try Atom:updated, then Atom:published
				if (n == null)
				{
					n = item.SelectSingleNode("at:published", nsmgr);
					if (n == null)
						return DateTime.Now;									// Can't find the date, use 'Now'
				}
			}
			else
				return DateTime.Now;

			string dateStr = n.InnerText;
			if (!DateTime.TryParse(dateStr, out corrDate))						// This works on Atom dates!
			{
				//
				// Probably an RFC 822 time with text time zone other than 'GMT"
				// Try FeedBurner's EST/EDT. FOx sends EST with EDT time so can
				// get times in future :-( Try subtracting an hour as a guess.
				//
				string buf;
				if (dateStr.Contains("EST"))									// FeedBurner sends EST/EDT times :-(
					buf = dateStr.Replace(" EST", "-0500");
				else if (dateStr.Contains("EDT"))
					buf = dateStr.Replace(" EDT", "-0400");
				else
					return DateTime.MinValue;									// [sentinel] No luck
				if (DateTime.TryParse(buf, out corrDate))
				{
					if (corrDate > DateTime.Now)								// If in future (e.g. Fox/Science)
					{
						corrDate = corrDate.AddHours(-1);
						if (corrDate > DateTime.Now)
							return DateTime.MinValue;
						else
							return corrDate;
					}
					else
						return corrDate;
				}
				else
				{
					return DateTime.MinValue;									// [sentinel] No luck
				}
			}
			else																// Converted successfully!
				return corrDate;
		}

		//
		// Get the XML from a feed supporting a URL of the form
		//   http://user:pass@domain/...
		// so can get authenticated feeds (e.g. Twitter). Cannot
		// do this with XmlDocument.Load();
		//
		private string GetAuthFeedXml(string authUrl)
		{
			string xml, buf;
			HttpWebResponse rsp = null;											// [sentinel]
			NetworkCredential cred = null;										// [sentinel]

			buf = authUrl;
			buf = Regex.Replace(buf, "http://([^\\:]*\\:[^\\@]*)\\@.*", "$1");
			if (buf != authUrl)													// If found basic auth
			{
				string[] bits = buf.Split(new char[] { ':' });
				if (bits.Length != 2)
					throw new ApplicationException("Basic auth format is incorrect, see help");
				cred = new NetworkCredential(bits[0], bits[1]);
			}

			try
			{
				HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(authUrl);
				if (cred != null)
					req.Credentials = cred;
				rsp = (HttpWebResponse)req.GetResponse();
				if (rsp.StatusCode != HttpStatusCode.OK)
					throw new ApplicationException("RSS server returned " + rsp.StatusDescription + ", check the URL");
				// Feedburner and some other feeds have 0 or -1 Content-Length. Sad but true.
				//if (rsp.ContentLength <= 0)
				//    throw new ApplicationException("RSS server can't return feed data, check the URL");
				using (Stream rspStrm = rsp.GetResponseStream())
				{
					using (StreamReader rdr = new StreamReader(rspStrm))
						xml = rdr.ReadToEnd();
				}
				if (xml.Length == 0)
					throw new ApplicationException("RSS server can't return feed data, check the URL");
				return xml;
			}
			finally
			{
				if (rsp != null) rsp.Close();
			}
		}
		//
		// Remove titles older than 'titleAge' from the cache
		// Runs as separate thread.
		//
		private void TitleExpireThread()
		{
			try
			{
				while (true)
				{
					TitleExpire();
					Thread.Sleep(300000);										// Every 5 min
				}
			}
			catch (ThreadInterruptedException)
			{
				return;
			}
		}

		//
		// Worker logic for title cache expiry
		//
		private void TitleExpire()
		{
			DateTime expiryTime = DateTime.Now.AddMinutes(-_storyAge - 10);		// Leave in cache longer, avoid edge condition
			lock (_titleCache)
			{
				List<string> oldTitles = new List<string>();
				foreach (string title in _titleCache.Keys)
				{
					if (_titleCache[title] < expiryTime)
						oldTitles.Add(title);
				}
				foreach (string title in oldTitles)
					_titleCache.Remove(title);
			}
		}
		//
		// Sender delegate for the CwCom mode of Morse. This gets timing arrays, and calls
		// the tone generator or twiddles the RTS line on the serial port
		// (it's not really CwCom :-)).
		//
		private void Send(Int32[] code, string text)
		{
			for (int i = 0; i < code.Length; i++)
			{
				if (code[i] > 0)
				{
					if (_useSerial)
					{
						_serialPort.RtsEnable = true;
						PreciseDelay.Wait(code[i]);
						_serialPort.RtsEnable = false;
					}
					else
					{
						switch (_soundMode)
						{
							case SoundMode.Tone:
								_tones.PlayFor(code[i]);
								break;
							case SoundMode.Spark:
								_spark.PlayFor(code[i]);
								break;
							case SoundMode.Sounder:
								_sounder.PlayFor(code[i]);
								break;
						}
					}
				}
				else
					PreciseDelay.Wait(_useSerial ? -code[i] : -code[i] - _timingComp);
			}
			string ct = Regex.Replace(text, "\\s+", " ");						// Remove running spaces from crawler
			AddToCrawler(ct);
		}
		//
		// Unknown character handler delegate. When this is active, a
		// space will be sent instead of the error code. Here we just
		// put the character into the crawler in [] so it will show.
		// for debugging, send extra stuff that will help us ID the
		// character for possible inclusion into the cleanup code.
		//
		private void HandleUnkChar(char ch)
		{
			string msg = "";

			if (_debugTracing)
			{
				msg = " ['" + ch + "'";
				msg += " U+" + ((int)ch).ToString("X4") + "]";
			}
			else
			{
				msg = " [" + ch + "] ";
			}
			AddToCrawler(msg);
		}

		//
		// Log into Twitter, with OAuth. Saves the Twitter username and OAuth keypair in Settings. 
		//
		private TwitterAPI LoginTwitter()
		{
			TwitterAPI twConn = new TwitterAPI();

			if (Properties.Settings.Default.oAuthToken == "" ||
				Properties.Settings.Default.oAuthTokenSecret == "")
			{
				string sUrl = twConn.GetAuthenticationLink("TP4s16loEhiwF6H4xmLPg",
										"w8TRJ0lGGUXJ88rCopfpLRiTuh91r6vYtsK4Fr5kAIA");
				Process.Start(sUrl);

				string sPin = "";
				TwitterPin pinForm = new TwitterPin();
				do
				{
					DialogResult ans = pinForm.ShowDialog();
					if (ans == DialogResult.Cancel) return null;
					sPin = pinForm.txtPin.Text;
				} while (!twConn.ValidatePIN(sPin));
				//
				// Got a good Pin, so the oAuth keypair is good!
				//
				Properties.Settings.Default.oAuthToken = twConn.OAuth_Token;
				Properties.Settings.Default.oAuthTokenSecret = twConn.OAuth_TokenSecret;
			}

			try
			{
				twConn.AuthenticateWith("TP4s16loEhiwF6H4xmLPg",
										"w8TRJ0lGGUXJ88rCopfpLRiTuh91r6vYtsK4Fr5kAIA",
										Properties.Settings.Default.oAuthToken,
										Properties.Settings.Default.oAuthTokenSecret);
			}
			catch (Exception ex)
			{
				throw new ApplicationException("Twitter authentication failed:\r\n" + ex.Message);
			}

			return twConn;

		}


		// =================
		// MAIN SENDING LOOP
		// =================

		private void Run()
		{
			try
			{
				Morse M = new Morse();
				M.Mode = (_codeMode == CodeMode.International ? Morse.CodeMode.International : Morse.CodeMode.American);
				M.CharacterWpm = _charSpeed;
				M.WordWpm = _codeSpeed;
				M.UnknownCharacter = HandleUnkChar;

				_msgNr = 0;														// Start with message #1

				if (_useSerial)
				{
					_serialPort = new ComPortCtrl();
#if MONO_BUILD
					_serialPort.Open("/dev/tty.serial" + _serialPortNum.ToString());
#else
					_serialPort.Open("COM" + _serialPortNum.ToString());
#endif
					_noise = null;
				}
				else
				{
					if (_noiseLevel > 0 && _soundMode != SoundMode.Sounder)		// No noise for telegraph sounder
					{
						_noise = new SoundPlayer(Properties.Resources.ResourceManager.GetStream("Noise" + _noiseLevel));
						_noise.PlayLooping();
					}
					else
						_noise = null;
				}

				// Remember the state of the title cache, we have a clear button!
				//lock (_titleCache) { _titleCache.Clear(); }					// Clear title cache on start

				//
				// Look for a file:// URI with a .txt extension (as opposed to .xml)
				// If so, then it should be a list of feed URIs. Read it into a list.
				//
				List<string> feedUrls = new List<String>();
				Uri uri = new Uri(_feedUrl);
				if (uri.IsFile)
				{
					string p = uri.LocalPath;
					if (Path.GetExtension(p) == ".txt")							// Assume this is a feed URI list
					{
						string[] lines = File.ReadAllLines(p);
						foreach (string l in lines)
						{
							string line = Regex.Replace(l, ";.*", "").Trim();	// Remove comment
							if (line == "") continue;
							feedUrls.Add(line);									// Live line, add the feed URI
						}
					}
					else if (Path.GetExtension(p) == "xml")						// Local RSS file
					{
						feedUrls.Add(_feedUrl);									// File path to RSS or 
					}
					else
					{
						throw new ApplicationException("file:// can only be .txt (feed list) or .xml (local RSS file)");
					}
				}
				else
				{
					feedUrls.Add(_feedUrl);										// Just this one feed
				}

				// =======================================
				// MAIN LOOP FOR READING AND SENDING MORSE
				// =======================================

				List<DatedItem> stories = new List<DatedItem>();
				while (true)
				{
					stories.Clear();											// Start fresh

					//
					// Feed list support
					//
					foreach (string feedUri in feedUrls)
					{
						_lastPollTime = DateTime.Now;
						uri = new Uri(feedUri);
						if (uri.Scheme == "twitter")
						{
							//
							// =======
							// TWITTER
							// =======
							//
							//
							// Using the TwitterVB library, which puts an API on Twitter's "xml" 
							// format. Yes, we could just use Twitter's RSS, but this gives us
							// more flexibility.
							//
							SetStatus("Getting Twitter feed data...", true);
							if (_debugTracing)
								Trace.WriteLine("  " + feedUri, _traceCatMorseNews);
							TwitterAPI twConn = LoginTwitter();
							List<TwitterStatus> sts = null;
							string screen_name = null;
							NameValueCollection opts = HttpUtility.ParseQueryString(uri.Query);
							if (uri.Host == "search")
							{
								string srch = opts.Get("query");							// Either query= or q=
								if (srch == null)
									srch = opts.Get("q");
								//
								// THis strange logic handles the case where the search is for a hashtag
								// In this case the search string will be in the URI "Fragment" and the 
								// query will be "query=", resulting in "" for srch below.
								//
								if (srch == null || (srch == "" && uri.Fragment == ""))
									throw new ApplicationException("twitter://search requires ?query=xxx to qualify the search");
								if (srch == "")
								{
									srch = Regex.Replace(uri.Fragment, "&.*", "");			// Remove following query elements
									string rq = Regex.Replace(uri.Fragment, "#[^&]*&", "");	//Get remaining query elements
									NameValueCollection opts2 = HttpUtility.ParseQueryString(rq);
									opts.Add(opts2);
								}
								string rtstr = opts.Get("result_type");						// Will be null if not in URI
								TwitterVB2.Globals.ResultType rtype = Globals.ResultType.Mixed;	// Value is placeholder for compiler

								if (rtstr != null)
								{
									rtstr = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(rtstr);	// Tricky!
									try {
										rtype = (TwitterVB2.Globals.ResultType)Enum.Parse(typeof(TwitterVB2.Globals.ResultType), rtstr);
									} catch (Exception) {
										throw new ApplicationException("twitter://search?result_type= must be mixed, popular, or recent");
									}
								}
								TwitterSearchParameters tsp = new TwitterSearchParameters();
								tsp.Add(TwitterSearchParameterNames.SearchTerm, srch);
								if (rtstr != null)
									tsp.Add(TwitterSearchParameterNames.ResultType, rtype);
								tsp.Add(TwitterSearchParameterNames.Rpp, 20);
								List<TwitterSearchResult> resList = twConn.Search(tsp);
								if (_debugTracing)
									Trace.WriteLine("Search API returned " + resList.Count + " tweets", _traceCatMorseNews);
								foreach (TwitterSearchResult res in resList)
								{
									DatedItem ni = new DatedItem();
									ni.pubDate = res.CreatedAtLocalTime;
									ni.twType = "SCH ";
									ni.twScreenName = res.AuthorName;
									ni.twRawText = res.Title;
									stories.Add(ni);
								}
							}
							else if (uri.Host == "messages")
							{
								List<TwitterDirectMessage> msgList = twConn.DirectMessages();
								if (_debugTracing)
									Trace.WriteLine("Found  " + msgList.Count + " direct message tweets", _traceCatMorseNews);
								foreach (TwitterDirectMessage msg in msgList)
								{
									DatedItem ni = new DatedItem();
									ni.pubDate = msg.CreatedAtLocalTime;
									ni.twType = "PVT ";
									ni.twScreenName = msg.SenderScreenName;
									ni.twRawText = msg.Text;
									stories.Add(ni);
								}
							}
							else
							{
								// TODO add parameter decoding here
								switch (uri.Host)
								{
									case "timeline":										// Auth user's home timeline
										sts = twConn.HomeTimeline();
										break;

									case "user":
										screen_name = opts.Get("screen_name");
										if (screen_name == null)
											sts = twConn.UserTimeline();					// Tweets by auth user
										else
											sts = twConn.UserTimeline(screen_name);			// Tweets by given user
										break;

									case "public":											// Tweets mentioning auth user
										sts = twConn.PublicTimeline();
										break;

									case "mentions":										// Tweets mentioning auth user
										sts = twConn.Mentions();
										break;

									case "list":											// Tweets from give user's list (or auth user if not given)
										screen_name = opts.Get("screen_name");
										if (screen_name == null)
											screen_name = twConn.AccountInformation().ScreenName;
										string listName = opts.Get("list_name");
										if (listName == null)
											throw new ApplicationException("twitter://list requires ?list_name=xxx to identify the list");
										sts = twConn.ListStatuses(screen_name, listName, 20);
										break;

									default:
										throw new ApplicationException("Unknown twitter:// uri '" + uri.Host + "'");
								}
								if (_debugTracing)
									Trace.WriteLine("Found  " + sts.Count + " " + uri.Host + " tweets", _traceCatMorseNews);
								foreach (TwitterStatus tw in sts)
								{
									DatedItem ni = new DatedItem();
									ni.pubDate = tw.CreatedAtLocalTime;
									ni.twType = ""; // "PUB ";
									ni.twScreenName = tw.User.ScreenName;
									ni.twRawText = tw.Text;
									stories.Add(ni);
								}
							}
							//if (stories.Count == 0)
							//    throw new ApplicationException(feedUri + " resulted in no tweets!");
						}
						else
						{
							//
							// ===
							// RSS
							// ===
							//
							SetStatus("Getting RSS feed data...", true);
							if (_debugTracing)
								Trace.WriteLine("  " + feedUri, _traceCatMorseNews);
							XmlDocument feedXml = new XmlDocument();
							//feedXml.Load(feedUri);
							feedXml.LoadXml(GetAuthFeedXml(feedUri));

							//
							// First try for RSS
							//
							XmlNodeList items = feedXml.SelectNodes("/rss/channel/item");
							if (items.Count > 0)
							{
								if (_debugTracing)
									Trace.WriteLine("Found  " + items.Count + " RSS articles", _traceCatMorseNews);
								foreach (XmlNode item in items)
								{
									DateTime pubUtc = GetPubDateUtc(item, null);			// See comments above, sick hack
									if (pubUtc == DateTime.MinValue)
									{
										if (_debugTracing)
										{
											Trace.WriteLine("RSS article has bad pubDate " + item.SelectSingleNode("pubDate").InnerText, _traceCatMorseNews);
											Trace.WriteLine("  " + item.SelectSingleNode("title").InnerText, _traceCatMorseNews);
										}
										continue;											// Bad pubDate, skip this story
									}
									//
									// OK we have a story we can use, as we were able to parse the date.
									//
									DatedItem ni = new DatedItem();
									ni.pubDate = pubUtc;
									ni.rssItem = item;
									stories.Add(ni);
								}
							}
							else
							{
								//
								// ====
								// ATOM
								// ====
								//
								XmlNamespaceManager nsmgr = new XmlNamespaceManager(feedXml.NameTable);
								nsmgr.AddNamespace("at", "http://www.w3.org/2005/Atom");
								items = feedXml.SelectNodes("/at:feed/at:entry", nsmgr);
								if (items.Count == 0)
									throw new ApplicationException("This does not look like an RSS or Atom feed");
								if (_debugTracing)
									Trace.WriteLine("Found  " + items.Count + " Atom articles", _traceCatMorseNews);
								foreach (XmlNode item in items)
								{
									DateTime pubUtc = GetPubDateUtc(item, nsmgr);			// See comments above, sick hack
									if (pubUtc == DateTime.MinValue)
									{
										if (_debugTracing)
										{
											Trace.WriteLine("RSS article has bad pubDate " + item.SelectSingleNode("pubDate").InnerText, _traceCatMorseNews);
											Trace.WriteLine("  " + item.SelectSingleNode("title").InnerText, _traceCatMorseNews);
										}
										continue;											// Bad pubDate, skip this story
									}
									//
									// OK we have a story we can use, as we were able to parse the date.
									//
									DatedItem ni = new DatedItem();
									ni.pubDate = pubUtc;
									ni.atomItem = item;
									ni.atomNsMgr = nsmgr;
									stories.Add(ni);
								}
							}
							//if (stories.Count == 0)
							//    throw new ApplicationException("This RSS feed has strange or missing pub dates, thus it can't be used, sorry.");
						}
					}

					//
					// Sort the stories newest to oldest. 
					//
					// This uses the overload of the List.Sort method that takes a Comparison<T> 
					// delegate (and thus lambda expression). Love this language!!
					//
					if (stories.Count > 1)
						stories.Sort((x, y) => DateTime.Compare(y.pubDate, x.pubDate));	// x <-> y for sort decreasing

					//
					// Create a list of strings which are the final messages to be send in Morse
					//
					List<MorseMessage> messages = new List<MorseMessage>();
					foreach (DatedItem story in stories)
					{
						string title;
						string time;
						string detail;
						string msg;
						string typ;

						if (story.pubDate < DateTime.Now.AddMinutes(-_storyAge))	// If too old
							continue;											// Skip it

						if (story.twType != null)								// This one is from Twitter
						{
							// Entire story is the title. Add screen name before to look similar to RSS newsfeeds
							title = GetMorseInputText(story.twScreenName + " - " + story.twRawText);
							if (_ignoreRetweets && title.Contains(" RT @"))		// Skip retweets (already capitalized)
								continue;
						}
						else if (story.rssItem != null)
						{
							title = GetMorseInputText(story.rssItem.SelectSingleNode("title").InnerText);
						}
						else
						{
							title = GetMorseInputText(story.atomItem.SelectSingleNode("at:title", story.atomNsMgr).InnerText);
						}

						lock (_titleCache)
						{
							if (_titleCache.ContainsKey(title))
							{
								if (_debugTracing)
								{
									Trace.WriteLine("Found in cache and skipped:", _traceCatMorseNews);
									Trace.WriteLine("  " + title, _traceCatMorseNews);
								}
								continue;										// Recently sent, skip
							}
						}


						time = story.pubDate.ToUniversalTime().ToString("HHmm") + "Z";
						if (story.twType != null)								// This one is from Twitter
						{
							detail = "";										// Entire tweet is title
							typ = "TWT " + story.twType;	// From includes screen name
						}
						else if (story.rssItem != null)
						{
							// ?? SHOULD I DO THIS ??
							//if (story.pubDate < DateTime.Now.AddMinutes(-_storyAge))
							//    continue;
							//
							// May be headline-only article, or a weird feed where the detail is much
							// shorter than the title (Quote of the day, title is quote, detail is author)
							//
							XmlNode detNode = story.rssItem.SelectSingleNode("description");	// Try for description
							if (detNode != null)
								detail = GetMorseInputText(detNode.InnerText);
							else
								detail = "";
							typ = "RSS";
						}
						else
						{
							XmlNode detNode = story.atomItem.SelectSingleNode("at:summary", story.atomNsMgr);
							if (detNode != null)
								detail = GetMorseInputText(detNode.InnerText);
							else
								detail = "";
							typ = "ATM";
						}

						if (M.Mode == Morse.CodeMode.International)				// Radiotelegraphy
						{
							msg = "NR _##_ DE " + typ + " " + time + " \\BT\\";
							if (detail.Length < title.Length)
								msg += title + " " + detail;
							else
								msg += detail;
							msg += " \\AR\\";
						}
						else													// American telegraph
						{
							// TODO - Make time zone name adapt to station TZ and DST
							string date = story.pubDate.ToUniversalTime().ToString("MMM d h mm tt") + " GMT";
							msg = "NR _##_ " + typ + " FILED " + date + " = ";
							if (detail.Length < title.Length)
								msg += title + " " + detail;
							else
								msg += detail;
							msg += " END";
						}

						MorseMessage mMsg = new MorseMessage();
						mMsg.Title = title;
						mMsg.Contents = msg;
						messages.Add(mMsg);
					}

					//
					// If we're still sending and the poll interval expires, force
					// a re-poll for new stories.
					//
					if (messages.Count > 0)
					{
						//
						// Have message(s), generate Morse Code sound for each
						// Number them here, counting only those actually sent.
						//
						int n = 1;
						foreach (MorseMessage msg in messages)
						{
							bool repeated = false;
							//
							// Catch the same message appearing multiple times in the current set
							// (before being filtered above). 
							//
							lock (_titleCache)
							{
								if (_titleCache.ContainsKey(msg.Title))
									repeated = true;
							}
							if (repeated)
							{
								if (_debugTracing)
								{
									Trace.WriteLine("**Same story repeated in current set, skipped:", _traceCatMorseNews);
									Trace.WriteLine("  " + msg.Title, _traceCatMorseNews);
								}
								continue;
							}

							SetStatus("Break...", true);
							Thread.Sleep(_breakTimeMs);
							if (DateTime.Now.Date == _lastMsgSendDate)
							{
								_msgNr += 1;
							}
							else
							{
								if (_debugTracing)
									Trace.WriteLine("New day " + DateTime.Now.Date.ToString("d") +
										" - resetting message number to zero", _traceCatMorseNews);
								_msgNr = 1;											// Reset msg number at date change
							}
							SetStatus("Sending message " + n++ + " of " + messages.Count, true);
							SetCrawler("");
							msg.Contents = msg.Contents.Replace("_##_", _msgNr.ToString());
							if (_debugTracing)
								Trace.WriteLine(DateTime.Now.ToString("HH:mm:ss") + " " + msg.Contents, _traceCatMorseNews);
							_lastMsgSendDate = DateTime.Now.Date;
							M.CwCom(msg.Contents, Send);
							lock (_titleCache)
							{
								if (_titleCache.ContainsKey(msg.Title))
								{
									// SHOULD NEVER HAPPEN!
									Trace.WriteLine("** Tried to add existing title to cache:", _traceCatMorseNews);
									Trace.WriteLine("  " + msg.Title, _traceCatMorseNews);
								}
								else
									_titleCache.Add(msg.Title, DateTime.Now);
							}
							if (DateTime.Now > _lastPollTime.AddMinutes(_pollInterval))
								break;
							GC.Collect(GC.MaxGeneration);
						}
					}
					else
					{
						//
						// No messages to send this time, wait until next time to poll
						//
						TimeSpan tWait = _lastPollTime.AddMinutes(_pollInterval) - DateTime.Now;
						if (_debugTracing)
							Trace.WriteLine("No messages, wait for " + tWait.ToString(), _traceCatMorseNews);
						if (tWait > TimeSpan.Zero)
						{
							for (int i = 0; i < tWait.TotalSeconds; i++)
							{
								string buf = TimeSpan.FromSeconds(tWait.TotalSeconds - i).ToString().Substring(3, 5);
								SetStatus("Check feed in " + buf + "...", false);
								Thread.Sleep(1000);
							}
						}
					}
					SetStatus("", false);
				}
			}
			catch (ThreadInterruptedException)
			{
				if (_noise != null) _noise.Stop();
				return;
			}
			catch (Exception ex)
			{
				if (_noise != null) _noise.Stop();
				if (_debugTracing)
				{
					Trace.WriteLine(ex.ToString(), _traceCatMorseNews);
					MessageBox.Show(ex.ToString(), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}
				else
				{
					MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				}	
				StopOnError();
				return;
			}
		}


	}
}
