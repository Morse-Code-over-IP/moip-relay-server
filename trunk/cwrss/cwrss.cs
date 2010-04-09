//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		cwrss.cs
//
// FACILITY:	RSS-Fed Morse Code News Robot 
//
// ABSTRACT:	Reads news from Yahoo! RSS feeds and sends the stories out over 
//				the CwCom Morse Code Service via the Ionosphere server.
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
// xx-Jan-10	rbd		Initial edits
// 04-Feb-10	rbd		0.5.1 - Add Windows Service capability
// 04-Feb-10	rbd		0.5.2 - Complete windows service checkout. Cleanly 
//						shut down threads on exit. Finish tracing via 
//						LogMessage().
// 05-Feb-10	rbd		0.5.3 - Fix LogMessage() for global search & destroy 
//						error. Was calling itself!
// 07-Feb-10	rbd		0.5.4 - Fix shutdown from SCM and ^C so does it cleanly.
//						Fix AS sequencing so always sends AS between stories and
//						Ident messages.
// 09-Feb-10	rbd		0.6.1 - Change "identify" to "periodic" message, the 
//						non-news messages we send periodically. Now these 
//						messages come from a file, and are a collection through
//						which we loop. Change the list of RSS newsfeeds to be
//						read from a file as well. To re-read these files, 
//						restart the server. Move WPM to const configs. Redo
//						Connect logic, force (re)connect every 30 sec even when 
//						sending news, avoiding the bot's disappearing from the
//						CwCom station list. 
// 10-Feb-10	rbd		0.6.2 - Multiple bots, read bot list, periodic messages,
//						and RSS feed list from files for each bot by name.
// 17-Feb-10	rbd		0.6.3 - Fix shutdown logic. No longer filter stories 
//						with pubDate < lastNews, just keep reaching back and 
//						sending stories not seen in the last _titleAge minutes.
//						This will keep the flow of news pretty constant. 
//						Refactor news message construction, add pub time to the 
//						header. Cache and skip only titles we SEND not those 
//						we've seen. Reorganize "no news" condition, it now only
//						gets hit when there is nothing in RSS that we haven't
//						already sent within the last 2 hours (increased cache
//						time to 2 hours). Fix message ordering so they are sent
//						oldest to newest. Was reversing the wrong list!
// 03-Mar-10	rbd		0.6.5 - Mono compatibility. OS-independent path seps and 
//						hide Windows Service stuff from Mono. Using MonoDevelop,
//						get rid of some now-unused variables (fossils from new
//						news selection logic). Re-enable periodic messages,
//						make interval a manifest constant.
// 22-Mar-10	rbd		0.6.6 - Don't reconnect while sending news story. This
//						could cause a long pause if the UDP "ack" packet is 
//						lost, causing a 5 sec receive timeout, another connect
//						attempt etc. Reconnect (if interval elapsed) after every
//						story, even the last one in a block. Lengthen "Sending
//						Story..." log message to 60. Re-enable periodic robot
//						message.
// 24-Mar-10	rbd		0.6.7 - Replace unicode apostrophe '’' with ascii 
//						apostrophe/single-quote (which is in Morse table).
// 25-Mar-10	rbd		0.6.8 - More Unicode replacements, now convert braces
//						and brackets to parens, backtick to apostrophe. This 
//						should make for fewer '........' outputs and help
//						readability. See GetMorseInputText().
// 05-Apr-10	rbd		0.7.2 - Add support for new American mode of Morse
//						class, and additional MorseKOB support (open key at
//						initial connect). Using new "blind" mode for cw.Connect()
//						re-enable periodic connect while sending. This avoids
//						the possible delay mentioned in the 22-Mar-10 notes.
// 05-Apr-10	rbd		0.7.2 - Add listener so can avoid sending unless someone
//						else is connected to the channel/line. Remove Q-signals
//						from ident. Don't send both the leader and the body,
//						just one or the other.
// 08-Apr-10	rbd		0.7.2 - Oops, DeadStation harvester, catch empty list.
//						Add logging. Add last chance exception handling. Clean
//						up some logged messages. Increase dead stn rate to 0.1 Hz.
//						Catch exceptions from malformed received packets - log
//						and ignore them. Write them to a file for debugging.
// 09-Apr-10	rbd		0.7.2 - Add per-feed title aging. Do not send title on
//						International feeds any more, unless title-only. Prevent
//						crash on feeds with no <detail> elements. Comment out
//						verbose logging of story retrieval and aging. Reduce
//						periodic message interval to 30 min.
//-----------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Web;
#if !MONO_BUILD
using System.ServiceProcess;
#endif

using com.dc3.morse;

namespace com.dc3.cwcom
{
	class Station
	{
		public string ID { get; set; }											// Callsign
		public DateTime LastRecvTime { get; set; }								// Last time station heard from
	}

	class CwNewsBot
	{
		private const int _reconnSeconds = 30;
		private const int _titleAge = 120;										// Keep titles in cache for two hours
		private const int _wordsPerStory = 40;
		private const int _cycleMinutes = 10;									// Send 10 min of stories in each cycle
		private const int _perMsgIntMinutes = 30;

		//
		// Config items from constructor
		//
		private string _botName = "";
		private string _serverAddr = "";
		private int _serverPort = 7890;
		private short _botChannel = 1111;
		private Morse.CodeMode _codeMode = Morse.CodeMode.International;
		private int _characterWpm = 20;
		private int _wordWpm = 20;

		//
		// State variables
		//
		private CwCom _cw;
		private Morse _morse;
		private DateTime _nextPerMsgTime = DateTime.MinValue;
		private Dictionary<string, DateTime> _titleCache = new Dictionary<string, DateTime>();
		private Thread _botThread = null;
		private Thread _titleExpireThread = null;
		private DateTime _nextConnect = DateTime.MinValue;
		private List<string> _periodicMessages = new List<string>();
		private int _nextPeriodicMessage = -1;
		private List<Feed> _rssFeeds = new List<Feed>();
		private int _storiesPerCycle = 5;										// _wordWpm * _cycleMinutes / _wordsPerStory;
		private int _msgNr;														// For American/telegraph daily message number
		private DateTime _msgNrDay;

		private List<Station> _stationList = new List<Station>();
		private Thread _deadStationThread = null;

		//
		// Represents an RSS feed for this bot
		//
		class Feed
		{
			public string Name { get; set; }
			public string URL { get; set; }
			public int TitleAge { get; set; }									// Seconds
		}

		//
		// Represents an RSS item with a correct pubDate
		//
		private class datedRssItem
		{
			public string feedName { get; set; }
			public DateTime pubDate { get; set; }
			public int titleAge { get; set; }
			public XmlNode rssItem { get; set; }
		}

		//
		// Constructor
		//
		public CwNewsBot(string BotName, string ServerAddr, int ServerPort, short BotChannel, int CharWpm, int WordWpm, Morse.CodeMode Mode)
		{
			_botName = BotName;
			_serverAddr = ServerAddr;
			_serverPort = ServerPort;
			_botChannel = BotChannel;
			_codeMode = Mode;
			_characterWpm = CharWpm;
			_wordWpm = WordWpm;
			_storiesPerCycle = _wordWpm * _cycleMinutes / _wordsPerStory;
			_msgNr = 1;
			_msgNrDay = DateTime.Now.Date;
		}

		//
		// Clean shutdown - shared by Control-C handler and the Service Stop
		// Called on a separate thread...
		//
		private void CleanShutdown()
		{
			LogMessage("Shutting down bot...");
			if (_titleExpireThread != null)										// Could happen on shutdown during initial connect attempt
			{
				_titleExpireThread.Interrupt();
				_titleExpireThread.Join(1000);
			}
			if (_deadStationThread != null)
			{
				_deadStationThread.Interrupt();
				_deadStationThread.Join(1000);
			}
			_cw.Disconnect();
			_botThread.Interrupt();
			_botThread.Join(1000);
			LogMessage("Bot shutdown complete...");
		}

		//
		// Next two provide hex dump for logging bad packets. This is "least
		// common denominator" code, for both .NET and Mono.
		//
		// http://www.codeproject.com/KB/cs/HexDump.aspx plus corrections in
		// first couple of comments.
		//
		private char HexChar(int value)
		{
			value &= 0xF;
			if (value >= 0 && value <= 9) return (char)('0' + value);
			else return (char)('A' + (value - 10));
		}

		public string HexDump(byte[] bytes)
		{
			if (bytes == null) return "<null>";
			int len = bytes.Length;
			StringBuilder result = new StringBuilder(((len + 15) / 16) * 78);
			char[] chars = new char[78];
			// fill all with blanks
			for (int i = 0; i < 75; i++) chars[i] = ' ';
			chars[76] = '\r';
			chars[77] = '\n';

			for (int i1 = 0; i1 < len; i1 += 16)
			{
				chars[0] = HexChar(i1 >> 28);
				chars[1] = HexChar(i1 >> 24);
				chars[2] = HexChar(i1 >> 20);
				chars[3] = HexChar(i1 >> 16);
				chars[4] = HexChar(i1 >> 12);
				chars[5] = HexChar(i1 >> 8);
				chars[6] = HexChar(i1 >> 4);
				chars[7] = HexChar(i1 >> 0);

				int offset1 = 11;
				int offset2 = 60;

				for (int i2 = 0; i2 < 16; i2++)
				{
					if (i1 + i2 >= len)
					{
						chars[offset1] = ' ';
						chars[offset1 + 1] = ' ';
						chars[offset2] = ' ';
					}
					else
					{
						byte b = bytes[i1 + i2];
						chars[offset1] = HexChar(b >> 4);
						chars[offset1 + 1] = HexChar(b);
						chars[offset2] = (b < 32 ? '·' : (char)b);
					}
					offset1 += (i2 == 7 ? 4 : 3);
					offset2++;
				}
				result.Append(chars);
			}
			return result.ToString();
		}

		private void ReceiveMessage(byte[] rcvdMsg)
		{
			ReceivedMessage msg;
			if (rcvdMsg.Length != ReceivedMessage.Length)
				return;															// Ignore control messages
			try
			{
				msg = new ReceivedMessage(rcvdMsg); 							// Safe from malformed received packets
			}
			catch (Exception ex)
			{
				LogMessage("Malformed large message: " + ex.Message + ", ignoring");
				LogMessage("\r\n" + HexDump(rcvdMsg));							// Start dump on new line (no timestamp)
				return;
			}
			if (msg.Type != LargeMessage.MessageTypes.ID)						// Ignore all but ID messages
				return;

			//
			// ID message, look for station and create one if not found
			//
			Station thisStation = null;
			lock (_stationList)
			{
				foreach (Station S in _stationList)
				{
					if (S.ID == msg.ID)
					{
						thisStation = S;
						break;
					}
				}
				if (thisStation == null)
				{
					thisStation = new Station();
					thisStation.ID = msg.ID;
					_stationList.Add(thisStation);
					LogMessage("New station " + thisStation.ID + " joined (" + _stationList.Count + " stns total)");
				}
				thisStation.LastRecvTime = DateTime.Now;
			}
		}


		//
		// Writes to shell if running interactive, else traces so can be seen in 
		// (e.g.) the awesome DebugView tool if running as a service.
		//
		private void LogMessage(string msg)
		{
			if (s_serviceMode)
				Trace.WriteLine("[" + _botName + "] " + msg);					// For DebugView when running as service
			else
				Console.WriteLine("[" + _botName + "] " + msg);

			string prefix = "[" + DateTime.Now.ToUniversalTime().ToString("s") + " " + _botName + "] ";

			lock (s_logLock)
			{
				if (DateTime.Now.Date != s_curLogDate)								// Time to rotate log
				{
					if (File.Exists(s_prevPath))
						File.Delete(s_prevPath);
					File.AppendAllText(s_logPath, prefix + "Log closed for rotation\r\n");
					File.Move(s_logPath, s_prevPath);
					File.WriteAllText(s_logPath, prefix + "Log opened - times in UTC\r\n");
					s_curLogDate = DateTime.Now.Date;
				}

				File.AppendAllText(s_logPath, prefix + msg + "\r\n");
			}
		}

		//
		// Remove titles older than 'titleAge' from the cache
		// Runs as separate thread.
		//
		private void TitleExpire()
		{
			try
			{
				while (true)
				{
					DateTime expiryTime = DateTime.Now.AddMinutes(-_titleAge);
					lock (_titleCache)
					{
						List<string> oldTitles = new List<string>();
						foreach (string title in _titleCache.Keys)
						{
							if (_titleCache[title] < expiryTime)
								oldTitles.Add(title);
						}
						foreach (string title in oldTitles)
						{
							//LogMessage("Expiring title: " + title);
							_titleCache.Remove(title);
						}
					}
					Thread.Sleep(900000);										// Every 15 min
				}
			}
			catch (ThreadInterruptedException)
			{
				LogMessage("Stopping title expiry thread");
			}
		}

		private void DeadStationThread()
		{
			try
			{
				while (true)
				{
					Thread.Sleep(10000);										// Every 10 sec
					lock (_stationList)
					{
						if (_stationList.Count > 0)
						{
							for (int i = _stationList.Count - 1; i >= 0; i--)		// Iterate backwards for removal safety
							{
								Station S = _stationList[i];
								if (S.LastRecvTime.AddSeconds(120) < DateTime.Now)	// Not seen for 2 minutes = dead
								{
									_stationList.RemoveAt(i);
									LogMessage("Station " + S.ID + " disappeared (" + _stationList.Count + " stns remaining)");
								}
							}
						}
					}
				}
			}
			catch (ThreadInterruptedException) { }
			LogMessage("Dead station thread exiting...");
		}

		//
		// Strip stuff from text that cannot be sent by Morse Code
		// HTML-decodes, then removes HTML tags and non-Morse characters,
		// and finally removes runs of more than one whitespace character,
		// replacing with a single space and uppercases it.
		//
		private string GetMorseInputText(string stuff)
		{
			string buf = HttpUtility.HtmlDecode(stuff);							// Decode HTML entities, etc.
			buf = Regex.Replace(buf, "<[^>]*>", " ");							// Remove HTML tags completely
			buf = Regex.Replace(buf, "[\\~\\^\\%\\|\\<\\>]", " ");				// Some characters we don't have translations for => space
			buf = Regex.Replace(buf, "[\\‘\\’\\`]", "'");						// Unicode left/right single quote, backtick -> ASCII single quote
			buf = Regex.Replace(buf, "[\\{\\[]", "(");							// Left brace/bracket -> left paren
			buf = Regex.Replace(buf, "[\\}\\]]", ")");							// Right brace/bracket -> Right paren
			buf = Regex.Replace(buf, "[—–]", "-");								// Unicode emdash/endash -> hyphen
			buf = Regex.Replace(buf, "\\s\\s+", " ").Trim().ToUpper();			// Compress running whitespace, fold all to upper case

			return buf;
		}

		//
		// Tries to get a correct pubDate (local time) from the RSS <item> node.
		//
		// This would be much easier if the @#$%^& date in pubDate wasn't the
		// old lazy-man's unix strftime()/RFC822 format, with its "whatever"
		// time zone abbrevations!!!! FeedBurner uses "EST" egad.
		//
		private DateTime GetPubDateUtc(XmlNode item)
		{
			DateTime corrDate;

			XmlNode n = item.SelectSingleNode("pubDate");						// Some feeds don't have this, so use now()
			if (n == null)
				return DateTime.Now;

			string dateStr = n.InnerText;
			if (!DateTime.TryParse(dateStr, out corrDate))
			{
				//
				// Probably an RFC 822 time with text time zone other than 'GMT"
				// Try FeedBurner's EST
				//
				string buf = dateStr.Replace(" EST", "-0500");					// FeedBurner sends EST times :-(
				if (DateTime.TryParse(buf, out corrDate))
				{
					if (corrDate > DateTime.Now)								// If in future (e.g. Science News!)
					{
						LogMessage("pubDate in future, skipped");
						return DateTime.MinValue;
					}
					else
						return corrDate;
				}
				else
				{
					LogMessage("Can't parse date: " + dateStr);
					return DateTime.MinValue;									// [sentinel]
				}
			}
			else																// Converted successfully!
				return corrDate;
		}

		//
		// Returns a list of morse-ready news stories from the RSS feed(s).
		//
		private List<string> GetLatestNews(List<Feed> FeedList)
		{
			//
			// Collect stories from RSS and make a list of datedRssItem
			// objects. These are needed because we may have to jump 
			// through hoops to parse the date. 
			//
			List<datedRssItem> stories = new List<datedRssItem>();

			//
			// For each feed, add its stories to the list
			//
			foreach (Feed feed in FeedList)
			{
				XmlDocument feedXml = new XmlDocument();
				LogMessage("Checking " + feed.Name + "...");
				try
				{
					feedXml.Load(feed.URL);						// Read right from the RSS feed server
				}
				catch (Exception ex)
				{
					LogMessage("Failed to get feed " + feed.Name + ": " + ex.Message);
					continue;
				}

				XmlNodeList items = feedXml.SelectNodes("/rss/channel/item");
				foreach (XmlNode item in items)
				{
					DateTime pubUtc = GetPubDateUtc(item);						// See comments above, sick hack
					if (pubUtc == DateTime.MinValue)
						continue;												// Bad pubDate, skip this story
					//
					// OK we have a story we can use, as we were able to parse the date.
					//
					datedRssItem n = new datedRssItem();
					n.feedName = feed.Name;
					n.titleAge = feed.TitleAge;
					n.pubDate = pubUtc;
					n.rssItem = item;
					stories.Add(n);
				}
			}

			if (stories.Count == 0)
				return null;
			LogMessage("Total of " + stories.Count + " stories, will send up to " + 
				(stories.Count > _storiesPerCycle ? _storiesPerCycle : stories.Count) + 
				" before re-checking RSS");
			//
			// Now we have all of the pubDates along with the RSS items for each.
			// Sort the list by date and take the _storiesPerCycle newest stories.
			// Also implements a title cache, used to prevent
			// sending multiple copies of the same story over a short time. Yahoo!
			// seems top update the <pubDate> tags on their RSS stores quite often,
			// without changing the story, presumably to force them to (re) appear 
			// in people's RSS readers. Here, in order to keep variety in the feed,
			// I prohibit a story with the same title as one seen in the last 
			// _titleAge (120 default) minutes from being fed again.
			//
			// This uses the overload of the List.Sort method that takes a Comparison<T> 
			// delegate (and thus lambda expression). Love this language!!
			//
			if(stories.Count > 1)
				stories.Sort((x, y) => DateTime.Compare(y.pubDate, x.pubDate));	// x <-> y for sort decreasing

			List<string> messages = new List<string>();
			int nMsg = 1;
			foreach (datedRssItem story in stories)
			{
				string title = GetMorseInputText(story.rssItem.SelectSingleNode("title").InnerText);
				//
				// Here we skip stories with titles that we've seen in the last _titleAge minutes.
				//
				lock (_titleCache)
				{
					if (_titleCache.ContainsKey(title))
					{
						//LogMessage("Recently sent: " + title + " -- skipped");
						continue;												// Recently sent, skip
					}
				}
				//
				// May be headline-only article, or a weird feed where the detail is much
				// shorter than the title (Quote of the day, title is quote, detail is author)
				//
				string time = story.pubDate.ToUniversalTime().ToString("HHmm") + "Z";
				//LogMessage("New story [" + time + "]: " + title);
				XmlNode detNode = story.rssItem.SelectSingleNode("description");	// Try for description
				string detail;
				if (detNode != null)
					detail = GetMorseInputText(detNode.InnerText);
				else
					detail = "";
				string msg;
				if (_morse.Mode == Morse.CodeMode.International)				// Radiotelegraphy
				{
					msg = "DE " + story.feedName + " " + time + " \\BT\\ ";
					if (detail.Length < title.Length)
						msg += title + " " + detail;
					else
						msg += detail;
					msg += " \\AR\\";
				}
				else															// American telegraph
				{
					// TODO - Make time zone name adapt to station TZ and DST
					string date = story.pubDate.ToString("MMM d h mm tt") + " MST";
					// TODO - Make "DD" station ID a config
					msg = "## DD DPR " + date + " = ";							// ## placeholder for number, No title or feed name
					if (detail.Length < title.Length)
						msg += title + " " + detail;
					else
						msg += detail;
					msg += " END";
				}
				messages.Add(msg);

				//
				// Cache titles of stories we send if feed has title aging implemented
				//
				if (story.titleAge > 0)
				{
					lock (_titleCache)
					{
						_titleCache.Add(title, DateTime.Now);
					}
				}
				
				//
				// Only send stories for _cycleMinutes, then go look
				// at news again. This keeps it fresh.
				//
				if (nMsg++ >= _storiesPerCycle) break;
			}
			if (messages.Count > 0)
			{
				messages.Reverse();												// Sort back to oldest -> newest
				//
				// For telegraph format, must number messages here, 
				// in sent order!
				//
				if (_morse.Mode == Morse.CodeMode.American)
				{
					for (int i = 0; i < messages.Count; i++)
					{
						messages[i] = messages[i].Replace("##", _msgNr.ToString());
						if (DateTime.Now.Date != _msgNrDay)						// New day, restart message #
							_msgNr = 1;
						else
							_msgNr += 1;
					}
				}
				return messages;
			}
			else
				return null;
		}

		//
		// Delegate, called back by the Morse class CwCom sender for
		// each array of timings corresponding to a character (and its
		// trailing space time).
		//
		private void Send(Int32[] code, string text)
		{
			//string tr = "\"" + text + "\" -> ";
			//for (int i = 0; i < code.Length; i++)
			//{
			//    string el = code[i].ToString();
			//    if (!el.StartsWith("-")) el = "+" + el;
			//    tr += el;
			//}
			//LogMessage(tr);
			_cw.SendCode(code, text, "Sending News");
			if (DateTime.Now > _nextConnect)
			{
				//
				// Avoid possible delay on missed ACK by using "blind" connect
				//
				_cw.Connect(_serverAddr, _serverPort, _botChannel, _botName, true);	// Blind connect
				_nextConnect = DateTime.Now.AddSeconds(_reconnSeconds);
			}
		}

		//
		// --------------------------
		// MAIN PROGRAM LOGIC IS HERE
		// --------------------------
		//
		private void Run()
		{
			try
			{
				//
				// Yahoo! feeds are the best I've found for this. They come right off
				// of the AP/Reuters/etc. newswires and are formatted like teletype
				// news. Also, Yahoo! uses GMT times in their RSS <PubDate> tags, so
				// the time can be understood by .NET's DateTime.Parse() method.
				//
				// Use caution and test thoroughly on news feeds from any other RSS
				// source.
				//
				// RssFeeds.txt has format name|url, for example:
				//          Top News|http://rss.news.yahoo.com/rss/topstories
				//
				string[] lines = File.ReadAllLines(s_appPath + Path.DirectorySeparatorChar + _botName + "-RssFeeds.txt");
				for (int i = 0; i < lines.Length; i++)
				{
					if (lines[i].Trim() != "")
					{
						string[] bits = lines[i].Split(new char[] { '|' });
						Feed F = new Feed();
						F.Name = bits[0];
						F.URL = bits[1];
						if (bits.Length > 2)
							F.TitleAge = Convert.ToInt32(bits[2]);
						else
							F.TitleAge = _titleAge;
						_rssFeeds.Add(F);
					}
				}

				lines = File.ReadAllLines(s_appPath + Path.DirectorySeparatorChar + _botName + "-PeriodicMessages.txt");
				for (int i = 0; i < lines.Length; i++)
				{
					if (lines[i].Trim() != "")
						_periodicMessages.Add(lines[i].Trim());
				}
				if (_periodicMessages.Count == 0)
					_nextPeriodicMessage = -1;									// [sentinel] No periodic messages
				else
					_nextPeriodicMessage = 0;									// Start with 1st periodic message

				_cw = new CwCom(ReceiveMessage, LogMessage);
				_morse = new Morse();
				_morse.CharacterWpm = _characterWpm;
				_morse.WordWpm = _wordWpm;
				_morse.Mode = _codeMode;
				_cw.Connect(_serverAddr, _serverPort, _botChannel, _botName, false);	// Not blind
				_nextConnect = DateTime.Now.AddSeconds(_reconnSeconds);
				_cw.Identify("Starting...");

				_titleExpireThread = new Thread(new ThreadStart(TitleExpire));
				_titleExpireThread.Name = _botName + " title exp";
				_titleExpireThread.Start();

				_deadStationThread = new Thread(new ThreadStart(DeadStationThread));
				_deadStationThread.Name = "Dead station harvester";
				_deadStationThread.Start();

				Thread.Sleep(5000);

				if (_codeMode == Morse.CodeMode.American)						// Open telegraph key for MorseKOB
					_cw.SendCode(new int[] {-5000, 2}, "", "Opening key");

				bool sentSome = false;
				while (true)
				{
					while (true)
					{
						lock (_stationList)
						{
							if (_stationList.Count > 0)
								break;
						}
						if (DateTime.Now > _nextConnect)						// Keep up the 30 sec reconnect here
						{
							_cw.Connect(_serverAddr, _serverPort, _botChannel, _botName, false);	// Not blind
							_nextConnect = DateTime.Now.AddSeconds(_reconnSeconds);
						}
						_cw.Identify("Idle, no listeners");
						Thread.Sleep(1000);
					}

					if (_nextPeriodicMessage >= 0 && DateTime.Now > _nextPerMsgTime)	// -1 -> no periodics
					{
						LogMessage("Sending periodic robot message");
						_cw.Identify("Message from robot");
					 	Thread.Sleep(5000);
						_morse.CwCom(_periodicMessages[_nextPeriodicMessage++], Send);
						if (_nextPeriodicMessage >= _periodicMessages.Count) _nextPeriodicMessage = 0;
						_nextPerMsgTime = DateTime.Now.AddMinutes(_perMsgIntMinutes);
						sentSome = true;
					}

					_cw.Identify("Checking news feeds...");
					List<string> messages = GetLatestNews(_rssFeeds);
					Thread.Sleep(5000);
					if (messages != null)
					{
						if (sentSome)											// Have been sending, last story of prev batch 0r ID above needs AS
							_morse.CwCom(_morse.Mode == Morse.CodeMode.International ? "\\AS\\" : " = ", Send);
						_cw.Identify("Stand by for news...");
						Thread.Sleep(5000);
						int nStories = messages.Count;
						foreach (string message in messages)
						{
							int z;
							if (_morse.Mode == Morse.CodeMode.International)
								z = message.IndexOf("\\BT\\") + 4;
							else
								z = message.IndexOf('=') + 1;
							string logMsg = message.Substring(z).Trim();
							if (logMsg.Length > 40)
								logMsg = logMsg.Substring(0, 37) + "...";
							LogMessage("Sending story " + logMsg);
							_cw.Identify("Sending News");
							_morse.CwCom(message, Send);
							if (--nStories > 0)
							{
								_morse.CwCom(_morse.Mode == Morse.CodeMode.International ? "\\AS\\" : " = ", Send);
								if (DateTime.Now > _nextConnect)
								{
									_cw.Connect(_serverAddr, _serverPort, _botChannel, _botName, false);	// Not blind
									_nextConnect = DateTime.Now.AddSeconds(_reconnSeconds);
								}
								_cw.Identify("Stand by for news...");
								Thread.Sleep(5000);
							}
							else if (DateTime.Now > _nextConnect)
							{
								_cw.Connect(_serverAddr, _serverPort, _botChannel, _botName, false);	// Not blind
								_nextConnect = DateTime.Now.AddSeconds(_reconnSeconds);
							}
							//
							// Quit and loop back if no listeners
							//
							lock (_stationList)
							{
								if (_stationList.Count == 0)
									break;
							}
						}
						sentSome = true;
					}
					else
					{
						if (sentSome && _nextPeriodicMessage >= 0)				// -1 -> no periodics
						{
							_morse.CwCom(_morse.Mode == Morse.CodeMode.International ? "\\AS\\" : " = ", Send);
							LogMessage("End of news, sending periodic robot message");
							_cw.Identify("Message from robot");
							Thread.Sleep(5000);
							_morse.CwCom(_periodicMessages[_nextPeriodicMessage++], Send);
							if (_nextPeriodicMessage >= _periodicMessages.Count) _nextPeriodicMessage = 0;
							_nextPerMsgTime = DateTime.Now.AddMinutes(_perMsgIntMinutes);
							sentSome = false;
							continue;											// Try for more news now
						}
						LogMessage("End of transmission");
						if (_morse.Mode == Morse.CodeMode.International) _morse.CwCom("\\SK\\", Send);
						_cw.Identify("End of transmission");
						Thread.Sleep(5000);
						LogMessage("Sleeping for 2 minutes...");
						for (int i = 120; i > 0; i--)							// Ident every sec for 2 min.
						{
							//
							// Loop back if no listeners
							//
							lock (_stationList)
							{
								if (_stationList.Count == 0)
									break;
							}

							if (DateTime.Now > _nextConnect)					// Keep up the 30 sec reconnect here
							{
								_cw.Connect(_serverAddr, _serverPort, _botChannel, _botName, false);	// Not blind
								_nextConnect = DateTime.Now.AddSeconds(_reconnSeconds);
							}
							//
							// Wait for 2 minutes then check news again
							//
							string buf = TimeSpan.FromSeconds(i).ToString().Substring(3);
							_cw.Identify("Check news in " + buf);
							Thread.Sleep(1000);
						}
					}
				}
			}
			catch (ThreadInterruptedException) { }
			LogMessage("Bot thread exiting...");
		}

		private void Start()
		{
			LogMessage("Starting bot " + _botName);
			_botThread = new Thread(new ThreadStart(Run));
			_botThread.Name = _botName + " bot";
			_botThread.Start();
		}

		// ==============
		// STATIC PORTION
		// ==============
		//
		// Statics for service mode operation
		//
		private static List<CwNewsBot> s_botList = new List<CwNewsBot>();
		private static bool s_serviceMode = false;								// True -> running as Windows service
		private static string s_appPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		private static AutoResetEvent s_exitFlag = new AutoResetEvent(false);
		private static Thread s_mainThread;
		private static string s_logPath = s_appPath + "\\log.txt";
		private static string s_prevPath = s_appPath + "\\prevlog.txt";
		private static Object s_logLock = new Object();
		private static DateTime s_curLogDate = DateTime.Now.Date.AddDays(-1);	// Assure log rotation on startup

		//
		// Last-chance exception handler
		//
		private static void LastChanceHandler(object sender, UnhandledExceptionEventArgs args)
		{
			Exception ex = ((Exception)(args.ExceptionObject));
			LogMessageS("*** FATAL EXCEPTION ***");
			LogMessageS(ex.ToString());
			LogMessageS("***********************");
			ShutdownBots();
		}

		//
		// Logger for static code
		//
		private static void LogMessageS(string msg)
		{
			if (s_serviceMode)
				Trace.WriteLine("[Main] " + msg);								// For DebugView when running as service
			else
				Console.WriteLine("[Main] " + msg);

			string prefix = "[" + DateTime.Now.ToUniversalTime().ToString("s") + " Main] ";
			lock (s_logLock)
			{
				if (DateTime.Now.Date != s_curLogDate)								// Time to rotate log
				{
					if (File.Exists(s_prevPath))
						File.Delete(s_prevPath);
					File.AppendAllText(s_logPath, prefix + "Log closed for rotation\r\n");
					File.Move(s_logPath, s_prevPath);
					File.WriteAllText(s_logPath, prefix + "Log opened - times in UTC\r\n");
					s_curLogDate = DateTime.Now.Date;
				}

				File.AppendAllText(s_logPath, prefix + msg + "\r\n");
			}
		}

		//
		// Control-C handler, allows clean exit. 
		//
		private static void CtrlCHandler(object sender, ConsoleCancelEventArgs args)
		{
			s_exitFlag.Set();
			s_mainThread.Join(10000);
		}

		//
		// Create the bots and return
		//
		private static void CreateBots()
		{
			//
			// Ident|ServerAddr|ServerPort|Channel|CharWPM|WordWPM
			//
			string[] lines = File.ReadAllLines(s_appPath + Path.DirectorySeparatorChar + "BotList.txt");
			for (int i = 0; i < lines.Length; i++)
			{
				string buf = lines[i].Trim();
				if (buf == "") continue;
				string[] bits = buf.Split(new char[] { '|' });
				if (bits.Length < 6)
				{
					LogMessageS("Bad line in BotList.txt" + buf);
					continue;
				}
				//
				// American only if present in config line and "american"
				// Nothing or anything else -> International
				//
				Morse.CodeMode mode;
				if (bits.Length == 7 && bits[6].ToLower() == "american")
					mode = Morse.CodeMode.American;
				else
					mode = Morse.CodeMode.International;
					
				CwNewsBot b = new CwNewsBot(bits[0], bits[1], Convert.ToInt32(bits[2]),
						Convert.ToInt16(bits[3]), Convert.ToInt32(bits[4]),
						Convert.ToInt32(bits[5]), mode);
				b.Start();
				s_botList.Add(b);
			}
		}

		//
		// Shut down the bots and return
		//
		private static void ShutdownBots()
		{
			foreach (CwNewsBot b in s_botList)
				b.CleanShutdown();
		}

		//
		// --------------------------
		// CONSOLE ENTRY POINT - MAIN
		// --------------------------
		//
		public static void Main(string[] args)
		{
#if !MONO_BUILD
			if (!Environment.UserInteractive)
			{
				ServiceBase.Run(new WindowsService.WindowsService());
			}
			else
			{
#endif
				AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(LastChanceHandler);
				s_mainThread = Thread.CurrentThread;
				s_serviceMode = false;
				Console.CancelKeyPress += new ConsoleCancelEventHandler(CtrlCHandler);	// Trap control-c
				LogMessageS("News engine started via command line...");
				CreateBots();
				s_exitFlag.WaitOne();
				ShutdownBots();
				LogMessageS("Program (Main) exited...");
//#if DEBUG
//                Console.WriteLine("Press return to exit...");
//                Console.ReadLine();
//#endif
#if !MONO_BUILD
			}
#endif
		}

#if !MONO_BUILD
		//
		// -------------------
		// SERVICE ENTRY POINT
		// -------------------
		//
		public static void ServiceRun()
		{
			AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(LastChanceHandler);
			s_mainThread = Thread.CurrentThread;
			s_serviceMode = true;
			LogMessageS("News engine started as system service...");
			CreateBots();
			s_exitFlag.WaitOne();
			ShutdownBots();
			LogMessageS("Service (ServiceRun) exited...");
		}

		//
		// SCM.OnStop() calls this to shut us down
		//
		public static void ServiceStop()
		{
			s_exitFlag.Set();
			s_mainThread.Join();
		}	
#endif
	}
		
}

#if !MONO_BUILD
//
// SUPPORT FOR USE AS WINDOWS SERVICE
//
namespace WindowsService
{
	//
	// Windows Service classes
	//
	class WindowsService : ServiceBase
	{
		private Thread _mainThread;

		public WindowsService()
		{
			this.ServiceName = "CWCom news robot";
			this.EventLog.Log = "Application";
			this.CanHandlePowerEvent = false;
			this.CanHandleSessionChangeEvent = false;
			this.CanPauseAndContinue = false;
			this.CanShutdown = false;
			this.CanStop = true;
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}

		protected override void OnStart(string[] args)
		{
			base.OnStart(args);
			_mainThread = new Thread(new ThreadStart(com.dc3.cwcom.CwNewsBot.ServiceRun));
			_mainThread.Name = "Main program";
			_mainThread.Start();
		}

		protected override void OnStop()
		{
			base.OnStop();
			com.dc3.cwcom.CwNewsBot.ServiceStop();
		}

		protected override void OnPause()
		{
			base.OnPause();
		}

		protected override void OnContinue()
		{
			base.OnContinue();
		}

		protected override void OnShutdown()
		{
			base.OnShutdown();
		}

		protected override void OnCustomCommand(int command)
		{
			base.OnCustomCommand(command);
		}

		protected override bool OnPowerEvent(PowerBroadcastStatus powerStatus)
		{
			return base.OnPowerEvent(powerStatus);
		}

		protected override void OnSessionChange(SessionChangeDescription changeDescription)
		{
			base.OnSessionChange(changeDescription);
		}
	}

	//
	// .NET SDK InstallUtil.exe:
	//
	// Install:    InstallUtil -i cwrss.exe
	// Uninstall:  InstallUtil -u cwrss.Exe
	//
	[RunInstaller(true)]
	public class WindowsServiceInstaller : Installer
	{
		public WindowsServiceInstaller()
		{
			ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
			ServiceInstaller serviceInstaller = new ServiceInstaller();

			//# Service Account Information
			// NOTE: I had to run this under LocalSystem on one machine as LocalService
			// was too restrictuve! I didn't bother to solve that one.
			serviceProcessInstaller.Account = ServiceAccount.LocalService;		// Minimum privileges!
			serviceProcessInstaller.Username = null;
			serviceProcessInstaller.Password = null;

			//# Service Information

			serviceInstaller.DisplayName = "CWCom News Robot";
			serviceInstaller.Description = "Sends Yahoo! RSS news feeds in Morse Code over the CWCom system";
			serviceInstaller.StartType = ServiceStartMode.Automatic;
			string[] deps = { "TCP/IP Protocol Driver" };
			serviceInstaller.ServicesDependedOn = deps;

			//# This must be identical to the WindowsService.ServiceBase name
			//# set in the constructor of the WindowsService class
			serviceInstaller.ServiceName = "CWCom news robot";

			this.Installers.Add(serviceProcessInstaller);
			this.Installers.Add(serviceInstaller);
		}
	}

}
#endif