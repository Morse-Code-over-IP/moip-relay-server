//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		cwcom.cs
//
// FACILITY:	RSS-Fed Morse Code News Robot 
//
// ABSTRACT:	Contains classes that implement the CWCom internet protocol.
//				Designed to be used together with the message wrapper classes
//				in cwmessages.cs.
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
// 04-Feb-10	rbd		0.5.1
// 04-Feb-10	rbd		0.5.2 - Make thread safe, as needed for Service Control 
//						Manager shutdown (and just to be sure anyway). Change
//						from calling Console.Log to calling via MessageLogger
//						delegate that client passes in constructor.
// 10-Feb-10	rbd		0.6.1 - Log connect only if not a reconnect.
// 01-Mar-10	rbd		0.6.4 - Split delay for word space befor and after
//						sending code group that starts with a space.
//-----------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace com.dc3.cwcom
{
	//
	// Implements a CWCom connection to a server or a CWCom
	// For now, this is transmit only.
	//
	public class CwCom
	{
		private UdpClient _udp;
		private object _objLock;
		private IPEndPoint _remIP;
		private bool _udpConn;
		private string _remHost;
		private int _remPort;
		private short _channel;
		private string _ident;
		private IdentMessage _idMsg;
		private DataMessage _dataMsg;
		private int _seqNo;
		private DateTime _nextTxIdent = DateTime.MinValue;
		private MessageLogger _logger;

		public string _TxStatus { get; set; }

		public delegate void MessageLogger(string msg);							// Cliant's logging function must be like this

		public CwCom(MessageLogger logger)
		{
			_logger = logger;
			_udp = new UdpClient();
			_objLock = new object();
			_udpConn = false;
			_remHost = "localhost";
			_remPort = 7860;
			_channel = 1000;
			_ident = "NoID";
			_seqNo = 3;
			_TxStatus = "TXing";

			_idMsg = new IdentMessage();
			_dataMsg = new DataMessage();
		}

		public void Connect(string Host, int Port, short Channel, string Ident)
		{
			bool justCon;

			lock (_objLock)
			{
				_remHost = Host;
				_remPort = Port;
				_channel = Channel;
				_ident = Ident;
				_idMsg.ID = _ident;
				_dataMsg.ID = _ident;

				while (true)													// Loop forever, till server reappears
				{
					//
					// This is in the loop to handle changes in the Ionosphere
					// server's dynamic address. If the connect fails, we'll close
					// the UDP socket and reopen it via DNS domain name, hopefully
					// jumping us to the new address. Oh, and the initial connect is
					// done here too!
					//
					if (!_udpConn)
					{
						_logger("Opening UDP");
						_udp.Connect(_remHost, _remPort);
						_udpConn = true;
						justCon = true;
					}
					else
						justCon = false;
					//
					// Connect and wait for ack
					//
					if (justCon) _logger("Sending connect message...");
					_udp.Send(new CtrlMessage(CtrlMessage.MessageTypes.Connect, _channel).Packet, CtrlMessage.Length);
					try
					{
						// TODO More robust way of telling ACK from junk (and other stations)
						byte[] ack = _udp.Receive(ref _remIP);
						if (justCon) _logger("Connected to " + _remIP.Address.ToString());
						break;
					}
					catch (Exception ex)
					{
						_logger("No ACK from server: " + ex.Message);
						_logger("Close and reopen...");
						_udp.Close();
						_udpConn = false;
						Thread.Sleep(60000);									// Wait an extra minute in this case
					}
					Thread.Sleep(5000);
				}
			}
		}

		public void Disconnect()
		{
			lock (_objLock)
			{
				if (!_udpConn) return;
				_udp.Send(new CtrlMessage(CtrlMessage.MessageTypes.Disconnect, _channel).Packet, CtrlMessage.Length);
				Thread.Sleep(100);
				_udp.Send(new CtrlMessage(CtrlMessage.MessageTypes.Disconnect, _channel).Packet, CtrlMessage.Length);
				Thread.Sleep(200);
				_udp.Close();
				_udpConn = false;
			}
		}

		public void Identify(string Text)
		{
			lock (_objLock)
			{
				if (!_udpConn) return;
				_idMsg.SequenceNo = _seqNo++;
				_idMsg.Text = Text;
				_udp.Send(_idMsg.Packet, IdentMessage.Length);
				_udp.Send(_idMsg.Packet, IdentMessage.Length);
			}
		}

		public void SendCode(Int32[] Code, string Text, string TxStatus)
		{
			lock (_objLock)
			{
				if (!_udpConn) return;
				if (DateTime.Now >= _nextTxIdent)							// Don't water tx down with needless idents!
				{
					Identify(_TxStatus);
					_nextTxIdent = DateTime.Now.AddSeconds(10);
				}

				int iStart = 0;
				int splitWait = 0;

				//
				// Pacing/timing here is a bit tricky. To avoid a noticeably long
				// pause between the first and second letters of a word (following 
				// a space), the beginning delay is split 50/50 before and after
				// sending the code group. This results in much better timing at
				// the receive end without things getting ahead.
				//
				if (Text.StartsWith(" "))
				{
					splitWait = Math.Abs(Code[0] / 2);
					Thread.Sleep(splitWait);
					iStart = 1;
				}

				_dataMsg.SequenceNo = _seqNo++;
				_dataMsg.Code = Code;
				_dataMsg.Text = Text;
				_udp.Send(_dataMsg.Packet, DataMessage.Length);
				_udp.Send(_dataMsg.Packet, DataMessage.Length);

				if (iStart == 1)
					Thread.Sleep(splitWait);

				int delay = 0;
				for (int i = iStart; i < Code.Length; i++)
					delay += Math.Abs(Code[i]);
				Thread.Sleep(delay); //Convert.ToInt32(0.95F * delay));
			}
		}
	}
}
