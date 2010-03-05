//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		WebServer.cs
//
// FACILITY:	Ionosphere server for CW Communicator
//
// ABSTRACT:	Provides a web service which shows who is connected, their status
//			and when they connected.
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
// 03-Mar-10	rbd		1.0.2 - For Mono, need path separator
// 04-Mar-09	rbd		1.0.3 - SF Artifact 2963796: Make domain and port dynamic, 
//						from server config
//-----------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Web;

namespace com.dc3.cwcom
{

	public class WebResponder
	{
		string _webRoot;
		TcpClient _client;
		NetworkStream _clientStream;
		byte[] _buf = new byte[8192];

		public WebResponder(TcpClient Client, string WebRoot)
		{
			_client = Client;
			_webRoot = WebRoot;
		}

		private void SendResponseHeader(string Status, string ContentType, int ContentLength)
		{
			string nowDate = DateTime.Now.ToString("R");
			string respHdr = "HTTP/1.0 " + Status + "\r\n";
			respHdr += "Date: " + nowDate + "\r\n";
			respHdr += "Server: Ionosphere Morse Code Relay Server\r\n";
			respHdr += "Connection: Close\r\n";
			respHdr += "Last-Modified: " + nowDate + "\r\n";
			respHdr += "ContentType: " + ContentType + "\r\n";
			respHdr += "Content-Length: " + ContentLength.ToString() + "\r\n\r\n";
			byte[] respBuf = Encoding.ASCII.GetBytes(respHdr);
			_clientStream.Write(respBuf, 0, respBuf.Length);
		}

		private void SendResponseHtml(string Status, string Html)
		{
			byte[] respBuf = Encoding.ASCII.GetBytes(Html);
			SendResponseHeader(Status, "text/html", respBuf.Length);
			_clientStream.Write(respBuf, 0, respBuf.Length);
			_clientStream.Close();
			_client.Close();
		}

		private void SendResponseBinary(byte[] Data, string ContentType)
		{
			SendResponseHeader("200 OK", ContentType, Data.Length);
			_clientStream.Write(Data, 0, Data.Length);
			_clientStream.Close();
			_client.Close();
		}

		private void SendError(string Status, string Message)
		{
			string respHtml = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">";
			respHtml += "<html xmlns=\"http://www.w3.org/1999/xhtml\">";
			respHtml += "<head><title>" + Status + "</title></head>\r\n";
			respHtml += "<body><h2>" + Status + "</h2>\r\n";
			respHtml += "<p><b>" + Message + "</byte></p></body></html>\r\n";
			SendResponseHtml(Status, respHtml);
		}

		//
		// Runs as separate thread, one for each incoming request
		//
		public void ProcessRequest()
		{
			_clientStream = _client.GetStream();
			int nBytes = _clientStream.Read(_buf, 0, _buf.Length);
			string rawHeader = Encoding.ASCII.GetString(_buf).Substring(0, nBytes);
			string[] lines = rawHeader.Replace("\r\n", "\n").Split('\n');		// Individual header lines (handle illegal but common Unix line endings)
			string[] reqLine = lines[0].Split(' ');								// First line GET path HTTP/1.x
			if (reqLine.Length != 3)
			{
				SendError("400 Bad Request", "Malformed HTTP request");
				return;															// END PROCESSING (TYP.)
			}
			if (reqLine[0].Trim().ToUpper() != "GET")
			{
				SendError("400 Bad Request", "Server supports only GET");
				return;
			}
			//
			// We don't care about any of the other request headers, only
			// the path which is the second part of the request line.
			//
			string webPath = reqLine[1].Trim();
			if (webPath == "/")
				webPath = "/index.html";
			webPath = HttpUtility.UrlDecode(webPath);
			string filePath = _webRoot + webPath.Replace("/", Path.DirectorySeparatorChar.ToString());
			if (!File.Exists(filePath))
			{
				SendError("404 Not Found", "Can't find " + reqLine[1].Trim());
				return;
			}
			string fileExtension = Path.GetExtension(filePath);
			string contentType = WebServer.ContentTypes[fileExtension];

			if (contentType == "text/html")
			{
				//
				// Perform ##TABLE## substitution then send the text
				//
				string text = File.ReadAllText(filePath);
				text = text.Replace("##DOMAIN##", Ionosphere.DomainName);
				text = text.Replace("##PORT##", Ionosphere.UdpPort.ToString());
				text = text.Replace("##TABLE##", Ionosphere.GenerateTableRows());
				SendResponseHtml("200 OK", text);
			}
			else
			{
				//
				// Just send the binary of the file
				//
				byte[] buf = File.ReadAllBytes(filePath);
				SendResponseBinary(buf, contentType);
			}
		}
	}

	//
	// Dirt-simple web server used only to show the Ionosphere status
	//
	class WebServer
	{
		private int _port;
		private string _webRoot;
		private TcpListener _listener;
		private Thread _svrThread;

		public static Dictionary<string, string> ContentTypes = new Dictionary<string, string>()
		{
			{ ".html",	"text/html" },
			{ ".htm",	"text/html" },
			{ ".css",	"text/css" },
			{ ".js",	"text/javascript" },
			{ ".png",	"image/png" },
			{ ".gif",	"image/gif" },
			{ ".jpg",	"image/jpeg" },
			{ ".jpeg",	"image/jpeg" },
			{ ".ico",   "image/x-icon" }
		};

		public WebServer(int Port)
		{
			_port = Port;
			_webRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + 
						Path.DirectorySeparatorChar + "Web";
		}

		~WebServer()
		{
			this.Dispose();
		}

		public void Start()
		{
			_svrThread = new Thread(new ThreadStart(Run));
			_svrThread.Start();
		}

		//
		// Runs in a thread that lasts for the lifetime of the Ionosphere server
		//
		private void Run()
		{
			_listener = new TcpListener(IPAddress.Any, _port);
			_listener.Start();
			Ionosphere.LogMessage("Web server started");

			while (true)
			{
				TcpClient client;
				try { client = _listener.AcceptTcpClient(); }
				catch (SocketException) { break; }
				//
				// Each incoming request is handled in its own thread.
				//
				WebResponder resp = new WebResponder(client, _webRoot);
				Thread respThread = new Thread(new ThreadStart(resp.ProcessRequest));
				respThread.Start();
			}
		}

		#region IDisposable Members

		public void Dispose()
		{
			if (_listener != null)												
			{
				_listener.Stop();
				_listener = null;
				_svrThread.Interrupt();
				_svrThread.Join(1000);
				Ionosphere.LogMessage("Web server stopped");
			}
		}

		#endregion
	}
}
