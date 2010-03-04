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
	class CwNewsBot
	{
		private const int _reconnSeconds = 30;
		private const int _titleAge = 120;										// Keep titles in cache for two hours
		private const int _wordsPerStory = 40;
		private const int _cycleMinutes = 10;									// Send 10 min of stories in each cycle
		private const int _perMsgIntMinutes = 60;

		//
		// Config items from constructor
		//
		private string _botName = "";
		private string _serverAddr = "";
		private int _serverPort = 7890;
		private short _botChannel = 1111;
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
		private Dictionary<string, string> _rssFeeds = new Dictionary<string, string>();
		private int _storiesPerCycle = 5;										// _wordWpm * _cycleMinutes / _wordsPerStory;

		//
		// Represents an RSS item with a correct pubDate
		//
		private class datedRssItem
		{
			public string feedName { get; set; }
			public DateTime pubDate { get; set; }
			public XmlNode rssItem { get; set; }
		}

		//
		// Constructor
		//
		public CwNewsBot(string BotName, string ServerAddr, int ServerPort, short BotChannel, int CharWpm, int WordWpm)
		{
			_botName = BotName;
			_serverAddr = ServerAddr;
			_serverPort = ServerPort;
			_botChannel = BotChannel;
			_characterWpm = CharWpm;
			_wordWpm = WordWpm;
			_storiesPerCycle = _wordWpm * _cycleMinutes / _wordsPerStory;
		}

		//
		// Clean shutdown - shared by Control-C handler and the Service Stop
		// Called on a separate thread...
		//
		private void CleanShutdown()
		{
			if (_titleExpireThread != null)										// Could happen on shutdown during initial connect attempt
			{
				_titleExpireThread.Interrupt();
				_titleExpireThread.Join(1000);
			}
			LogMessage("Disconnecting...");
			_botThread.Interrupt();
			_botThread.Join(1000);
			_cw.Disconnect();
			LogMessage("Shutdown complete...");
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
							LogMessage("Expiring title: " + title);
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

		//
		// Strip stuff from text that cannot be sent by Morse Code
		// HTML-decodes, then removes HTML tags and non-Morse characters,
		// and finally removes runs of more than one whitespace character,
		// replacing with a single space and uppercases it.
		//
		private string GetPlainText(string stuff)
		{
			string buf = HttpUtility.HtmlDecode(stuff);
			buf = Regex.Replace(buf, "<[^>]*>", " ");
			buf = Regex.Replace(buf, "[\\~\\`\\^\\%\\=\\|\\{\\}\\[\\]\\<\\>]", " ");
			buf = Regex.Replace(buf, "\\s\\s+", " ").Trim().ToUpper();

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
			string dateStr = item.SelectSingleNode("pubDate").InnerText;
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
		private List<string> GetLatestNews(Dictionary<string, string> FeedList)
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
			foreach (string feedName in FeedList.Keys)
			{
				XmlDocument feedXml = new XmlDocument();
				LogMessage("Checking " + feedName + "...");
				try
				{
					feedXml.Load(FeedList[feedName]);							// Read right from the RSS feed server
				}
				catch (Exception ex)
				{
					LogMessage("Failed to get feed " + feedName + ": " + ex.Message);
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
					n.feedName = feedName;
					n.pubDate = pubUtc;
					n.rssItem = item;
					stories.Add(n);
				}
			}

			if (stories.Count == 0)
				return null;
			LogMessage("Total of " + stories.Count + " stories");
			//
			// Now we have all of the pubDates along with the RSS items for each.
			// Sort the list by date and take the 10 newest stories which are newer
			// than lastNewsDate... Also implements a title cache, used to prevent
			// sending multiple copies of the same story over a short time. Yahoo!
			// seems top update the <pubDate> tags on their RSS stores quite often,
			// without changing the story, presumably to force them to (re) appear 
			// in prople's RSS readers. Here, in order to keep variety in the feed,
			// I prohibit a story with the same title as one seen in the last 
			// _titleAge (120) minutes from being fed again.
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
				string title = GetPlainText(story.rssItem.SelectSingleNode("title").InnerText);
				//
				// Here we skip stories with titles that we've seen in the last _titleAge minutes.
				//
				lock (_titleCache)
				{
					if (_titleCache.ContainsKey(title))
					{
						LogMessage("Recently sent: " + title + " -- skipped");
						continue;												// Recently sent, skip
					}
				}
				//
				// May be headline-only article
				//
				string time = story.pubDate.ToUniversalTime().ToString("HHmm") + "Z";
				string detail = GetPlainText(story.rssItem.SelectSingleNode("description").InnerText);
				LogMessage("New story [" + time + "]: " + title);
				string msg = "DE " + story.feedName + " " + time + " \\BT\\" + title;
				if (detail != "")
					msg += " \\BT\\" + detail;
				msg += " \\AR\\";
				messages.Add(msg);

				//
				// Cache titles of stories we send
				//
				lock (_titleCache)
				{
					_titleCache.Add(title, DateTime.Now);
				}
				
				//
				// Only send stories for _cycleMinutes, then go look
				// at news again. This keeps it fresh
				//
				if (nMsg++ >= _storiesPerCycle) break;
			}
			if (messages.Count > 0)
			{
				messages.Reverse();												// Sort back to oldest -> newest
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
				_cw.Connect(_serverAddr, _serverPort, _botChannel, _botName);
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
				// Use caution and test thorougly on news feeds from any other RSS
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
						_rssFeeds.Add(bits[0], bits[1]);
					}
				}

				lines = File.ReadAllLines(s_appPath + Path.DirectorySeparatorChar + _botName + "-PeriodicMessages.txt");
				for (int i = 0; i < lines.Length; i++)
				{
					if (lines[i].Trim() != "")
						_periodicMessages.Add(lines[i].Trim());
				}
				_nextPeriodicMessage = 0;

				_cw = new CwCom(LogMessage);									// Tell CwCom class to use our logger
				_morse = new Morse();
				_morse.CharacterWpm = _characterWpm;
				_morse.WordWpm = _wordWpm;
				_cw.Connect(_serverAddr, _serverPort, _botChannel, _botName);
				_nextConnect = DateTime.Now.AddSeconds(_reconnSeconds);
				_cw.Identify("QRV : Starting...");

				_titleExpireThread = new Thread(new ThreadStart(TitleExpire));
				_titleExpireThread.Name = _botName + " title exp";
				_titleExpireThread.Start();

				Thread.Sleep(5000);

				bool sentSome = false;
				while (true)
				{
					if (DateTime.Now > _nextPerMsgTime)
					{
						LogMessage("Sending periodic robot message");
						_cw.Identify("QRA : Message from robot");
					 	Thread.Sleep(5000);
						_morse.CwCom(_periodicMessages[_nextPeriodicMessage++], Send);
						if (_nextPeriodicMessage >= _periodicMessages.Count) _nextPeriodicMessage = 0;
						_nextPerMsgTime = DateTime.Now.AddMinutes(_perMsgIntMinutes);
						sentSome = true;
					}

					_cw.Identify("QRL : Checking news feeds");
					List<string> messages = GetLatestNews(_rssFeeds);
					Thread.Sleep(5000);
					if (messages != null)
					{
						if (sentSome)											// Have been sending, last story of prev batch 0r ID above needs AS
							_morse.CwCom("\\AS\\", Send);
						_cw.Identify("QRV : Stand by for news");
						Thread.Sleep(5000);
						int nStories = messages.Count;
						foreach (string message in messages)
						{
							LogMessage("Sending story " + message.Substring(0, 40) + (message.Length > 40 ? "..." : ""));
							_morse.CwCom(message, Send);
							if (--nStories > 0)
							{
								_morse.CwCom("\\AS\\", Send);
								if (DateTime.Now > _nextConnect)
								{
									_cw.Connect(_serverAddr, _serverPort, _botChannel, _botName);
									_nextConnect = DateTime.Now.AddSeconds(_reconnSeconds);
								}
								_cw.Identify("QRL : Stand by for news");
								Thread.Sleep(5000);
							}
						}
						sentSome = true;
					}
					else
					{
						if (sentSome)
						{
							_morse.CwCom("\\AS\\", Send);
							LogMessage("End of news, sending periodic robot message");
							_cw.Identify("QRA : Message from robot");
							Thread.Sleep(5000);
							_morse.CwCom(_periodicMessages[_nextPeriodicMessage++], Send);
							if (_nextPeriodicMessage >= _periodicMessages.Count) _nextPeriodicMessage = 0;
							_nextPerMsgTime = DateTime.Now.AddMinutes(_perMsgIntMinutes);
							sentSome = false;
							continue;											// Try for more news now
						}
						LogMessage("End of transmission");
						_morse.CwCom("\\SK\\", Send);
						_cw.Identify("QRT : End of transmission");
						Thread.Sleep(5000);
						LogMessage("Sleeping for 2 minutes...");
						for (int i = 120; i > 0; i--)							// Ident every sec for 2 min.
						{
							if (DateTime.Now > _nextConnect)					// Keep up the 30 sec reconnect here
							{
								_cw.Connect(_serverAddr, _serverPort, _botChannel, _botName);
								_nextConnect = DateTime.Now.AddSeconds(_reconnSeconds);
							}
							//
							// Wait for 2 minutes then check news again
							//
							string buf = TimeSpan.FromSeconds(i).ToString().Substring(3);
							_cw.Identify("QRX : Check news in " + buf);
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

		//
		// Logger for static code
		//
		private static void LogMessageS(string msg)
		{
			if (s_serviceMode)
				Trace.WriteLine("[Main] " + msg);								// For DebugView when running as service
			else
				Console.WriteLine("[Main] " + msg);
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
				if (bits.Length != 6)
				{
					LogMessageS("Bad line in BotList.txt" + buf);
					continue;
				}
				CwNewsBot b = new CwNewsBot(bits[0], bits[1], Convert.ToInt32(bits[2]),
						Convert.ToInt16(bits[3]), Convert.ToInt32(bits[4]),
						Convert.ToInt32(bits[5]));
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
				s_mainThread = Thread.CurrentThread;
				s_serviceMode = false;
				Console.CancelKeyPress += new ConsoleCancelEventHandler(CtrlCHandler);	// Trap control-c
				CreateBots();
				s_exitFlag.WaitOne();
				ShutdownBots();
				LogMessageS("Main exited...");
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
			s_mainThread = Thread.CurrentThread;
			s_serviceMode = true;
			CreateBots();
			s_exitFlag.WaitOne();
			ShutdownBots();
			LogMessageS("ServiceRun exited...");
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