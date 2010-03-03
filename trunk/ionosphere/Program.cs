// 10-Feb-10	rbd		1.0.1 - Match stations by both IP and port so can have multiple
//						stations on a single IP (like the news bot(s)). WHen broadcasting
//						check IP, Port -and- Channel.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Configuration.Install;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;
using System.ServiceProcess;

namespace com.dc3.cwcom
{
	class Station : ICloneable
	{
		public string ID { get; set; }											// Callsign
		public IPEndPoint RemEP { get; set; }									// Remote IP/port
		public Int16 Channel { get; set; }										// Ionosphere channel number
		public string Status { get; set; }										// Last status from station
		public DateTime LastRecvTime { get; set; }								// Last time station heard from
		public DateTime ConnectTime { get; set; }								// Time at which station first connected

		#region ICloneable Members

		public object Clone()
		{
			return base.MemberwiseClone();
		}

		#endregion
	}

	static class Ionosphere
	{
		private static Thread s_mainThread;
		private static Thread s_deadStationThread;
		private static List<Station> s_stationList = new List<Station>();
		private static UdpClient s_udp = new UdpClient(7890);
		private static bool s_serviceMode = false;
		private static DateTime s_startTime = DateTime.Now;
		private static byte[] s_recvBuf;
		private static WebServer s_webServer;

		//
		// Control-C handler, allows clean exit. 
		//
		private static void CtrlCHandler(object sender, ConsoleCancelEventArgs args)
		{
			CleanShutdown();
		}

		//
		// Clean shutdown - shared by Control-C handler and the Service Stop
		//
		private static void CleanShutdown()
		{
			s_webServer.Dispose();
			s_udp.Close();
			s_mainThread.Interrupt();
			s_mainThread.Join(1000);
			LogMessage("Shutdown complete.");
		}

		//
		// Writes to shell if running interactive, else traces so can be seen in 
		// (e.g.) the awesome DebugView tool if running as a service.
		//
		public static void LogMessage(string msg)
		{
			if (s_serviceMode)
				Trace.WriteLine(msg);											// For DebugView when running as service
			else
				Console.WriteLine(msg);
		}

		//
		// Generate HTML table rows for the web server.
		//
		public static string GenerateTableRows()
		{
			string text = "";
			lock (s_stationList)
			{
				// This uses the overload of the List.Sort method that takes a Comparison<T> 
				// delegate (and thus lambda expression). Love this language!!
				//
				if (s_stationList.Count > 1)									// Sort by increasing channel number
					s_stationList.Sort((x, y) => x.Channel.CompareTo(y.Channel));

				foreach (Station S in s_stationList)
				{
					text += "<tr><td>" + S.Channel + "</td>";
					text += "<td>" + S.ID + "</td>";
					text += "<td>" + S.Status + "</td>";
					text += "<td>" + S.ConnectTime.ToString("R") + "</td></tr>\r\n";
				}
			}
			return text;
		}

		//
		// Return Station object for given remote IP and Channel
		//
		private static Station FindStation(IPAddress IP, int Port)
		{
			lock (s_stationList)
			{
				foreach (Station S in s_stationList)
				{
					if (S.RemEP.Address.Equals(IP) && S.RemEP.Port.Equals(Port))				// Compare scalar address only
						return S;
				}
				return null;
			}
		}

		//
		// Received Connect message
		//
		private static void ConnectClient(CtrlMessage CtrlMsg, IPEndPoint Ep)
		{
			Station S = FindStation(Ep.Address, Ep.Port);										// See if this station already connected
			if (S == null)
			{
				S = new Station();
				S.RemEP = Ep;
				S.Channel = CtrlMsg.Channel;
				S.Status = "Just connected";
				S.ID = "Unknown";
				S.ConnectTime = S.LastRecvTime = DateTime.Now;
				lock (s_stationList) s_stationList.Add(S);
				LogMessage("Station at " + Ep.Address.ToString() + ":" + Ep.Port + " [" + S.Channel + "] connected (" + s_stationList.Count + " stns total)");
			}
			else
			{
				LogMessage("Station at " + Ep.Address.ToString() + ":" + Ep.Port + " [" + S.Channel + "] re-connected (" + s_stationList.Count + " stns total)");
				S.LastRecvTime = DateTime.Now;
				S.Channel = CtrlMsg.Channel;
			}
			s_udp.Send(new CtrlMessage(CtrlMessage.MessageTypes.Ack, CtrlMsg.Channel).Packet, CtrlMessage.Length - 2, Ep);	// Ack is special length!
		}

		//
		// Received Disconnect message
		//
		private static void DisconnectClient(CtrlMessage CtrlMsg, IPEndPoint Ep)
		{
			Station S = FindStation(Ep.Address, Ep.Port);
			if (S == null)
			{
				LogMessage("Unknown station at " + Ep.Address.ToString() + ":" + Ep.Port + " sent Disconnect");
				return;
			}
			lock (s_stationList) s_stationList.Remove(S);
			LogMessage("Station at " + Ep.Address.ToString() + ":" + Ep.Port + " disconnected (" + s_stationList.Count + " stns remaining)");
			//
			// Do not send anything here, the client has closed its socket and you'll
			// get a reset packet, which will cause a SocketException on the next UDP
			// receive operation.
			//
		}

		//
		// Received Identify message
		//
		private static void UpdateClient(ReceivedMessage RcvdMsg, Station Sender)
		{
			Sender.ID = RcvdMsg.ID;
			Sender.Status = RcvdMsg.Text;
			//LogMessage("Update station " /* + Sender.RemEP.Address.ToString() + " ID: "*/ + Sender.ID + " Status: " + Sender.Status);
		}

		//
		// Send message to all stations on the sender's channel, except sender himself!
		//
		private static void BroadcastMessage(ReceivedMessage RcvdMsg, Station Sender)
		{
			foreach (Station S in s_stationList)
			{
				if (S.Channel == Sender.Channel)
				{
					if (!S.RemEP.Address.Equals(Sender.RemEP.Address) || !S.RemEP.Port.Equals(Sender.RemEP.Port))
						s_udp.Send(RcvdMsg.Packet, ReceivedMessage.Length, S.RemEP);
				}
			}
			//LogMessage("Bcst " + RcvdMsg.Type.ToString() + " from " /*+ Sender.RemEP.Address.ToString() + " ID: "*/ + RcvdMsg.ID);
		}

		private static void DeadStationThread()
		{
			try
			{
				while (true)
				{
					Thread.Sleep(120000);										// Every 2 minutes
					lock (s_stationList)
					{
						for (int i = s_stationList.Count - 1; i >= 0; i--)		// Iterate backwards for removal safety
						{
							Station S = s_stationList[i];
							if (S.LastRecvTime.AddSeconds(120) < DateTime.Now)	// Not seen for 2 minutes = dead
							{
								LogMessage("Station at " + S.RemEP.Address.ToString() + ":" + S.RemEP.Port + 
											" disappeared (" + s_stationList.Count + " stns remaining)");
								s_stationList.RemoveAt(i);
							}
						}
					}
				}
			}
			catch (ThreadInterruptedException) { }
			LogMessage("Dead station thread exiting...");
		}

		//
		// ==================
		// COMMON ENTRY POINT
		// ==================
		//
		private static void Run(string[] args)
		{
			s_mainThread = Thread.CurrentThread;

			s_deadStationThread = new Thread(new ThreadStart(DeadStationThread));
			s_deadStationThread.Start();

			IPEndPoint epRecv = new IPEndPoint(IPAddress.Any, 0);				// Always receive from anyone

			LogMessage("Ionosphere starting...");

			s_webServer = new WebServer(7890);
			s_webServer.Start();

			// --------------
			// Receiving Loop
			// --------------

			while (true)
			{
				IPEndPoint recvEp = epRecv;
				try { s_recvBuf = s_udp.Receive(ref recvEp); }
				catch (SocketException) { break; }
				if (s_recvBuf.Length == 4)
				{
					//
					// Handle incoming Control packets
					//
					CtrlMessage ctrlMsg = new CtrlMessage();
					ctrlMsg.Packet = s_recvBuf;
					switch (ctrlMsg.Type)
					{
						case CtrlMessage.MessageTypes.Connect:
							ConnectClient(ctrlMsg, recvEp);
							break;
						case CtrlMessage.MessageTypes.Disconnect:
							DisconnectClient(ctrlMsg, recvEp);
							break;
						default:
							LogMessage("Unrecognized control message from " + recvEp.Address.ToString());
							break;
					}
				}
				else if (s_recvBuf.Length == 496)
				{
					ReceivedMessage rcvdMsg = new ReceivedMessage(s_recvBuf);
					Station Sender = FindStation(recvEp.Address, recvEp.Port);
					if (Sender == null)
					{
						LogMessage("Unknown station at " + recvEp.Address.ToString() + ":" + recvEp.Port + " sent data message");
						continue;												// Ignore message
					}
					switch(rcvdMsg.Type)
					{
						case ReceivedMessage.MessageTypes.Unknown:
							LogMessage("Received unknown large message, ignoring");
							break;
						case ReceivedMessage.MessageTypes.ID:
							Station prevState = (Station)Sender.Clone();
							UpdateClient(rcvdMsg, Sender);
							//
							// This throttles the scads of TXing ID messages sent at every space-to-mark transition.
							// Yes, this may result in clients missing some ID messages, but one will go out
							// every 2 seconds anyway...
							//
							if (Sender.ID != prevState.ID || Sender.Status != prevState.Status || Sender.LastRecvTime.AddSeconds(2) < DateTime.Now)
							BroadcastMessage(rcvdMsg, Sender);
							break;
						case ReceivedMessage.MessageTypes.Data:
							BroadcastMessage(rcvdMsg, Sender);
							break;
					}

				}
				else
					LogMessage("Received " + s_recvBuf.Length + " byte packet, ignoring.");

			}
		}

		//
		// ==========================
		// CONSOLE ENTRY POINT - MAIN
		// ==========================
		//
		public static void Main(string[] args)
		{
			if (!Environment.UserInteractive)
			{
				ServiceBase.Run(new WindowsService.WindowsService());
			}
			else
			{
				s_serviceMode = false;
				try { Run(args); }
				catch (ThreadInterruptedException) { }
				LogMessage("Main thread exited...");
			}
		}

		//
		// ===================
		// SERVICE ENTRY POINT
		// ===================
		//
		public static void ServiceRun()
		{
			string[] a = { };
			s_serviceMode = true;
			try { Run(a); }
			catch (ThreadInterruptedException) { }
			LogMessage("Main thread exited...");
		}

		//
		// SCM.OnStop() calls this to shut us down
		//
		public static void ServiceStop()
		{
			CleanShutdown();
		}
	}

}

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
			this.ServiceName = "Ionosphere Server";
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
			_mainThread = new Thread(new ThreadStart(com.dc3.cwcom.Ionosphere.ServiceRun));
			_mainThread.Start();
		}

		protected override void OnStop()
		{
			base.OnStop();
			com.dc3.cwcom.Ionosphere.ServiceStop();
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
	// Install:    InstallUtil -i ionosphere.exe
	// Uninstall:  InstallUtil -u ionosphere.Exe
	//
	[RunInstaller(true)]
	public class WindowsServiceInstaller : Installer
	{
		public WindowsServiceInstaller()
		{
			ServiceProcessInstaller serviceProcessInstaller = new ServiceProcessInstaller();
			ServiceInstaller serviceInstaller = new ServiceInstaller();

			//# Service Account Information

			serviceProcessInstaller.Account = ServiceAccount.LocalService;					// Minimum privileges!
			serviceProcessInstaller.Username = null;
			serviceProcessInstaller.Password = null;

			//# Service Information

			serviceInstaller.DisplayName = "Ionosphere Server";
			serviceInstaller.Description = "Provides multicasting of CwCom Morse Code traffic between stations";
			serviceInstaller.StartType = ServiceStartMode.Automatic;
			string[] deps = { "TCP/IP Protocol Driver" };
			serviceInstaller.ServicesDependedOn = deps;

			//# This must be identical to the WindowsService.ServiceBase name
			//# set in the constructor of the WindowsService class
			serviceInstaller.ServiceName = "Ionosphere Server";

			this.Installers.Add(serviceProcessInstaller);
			this.Installers.Add(serviceInstaller);
		}
	}

}
