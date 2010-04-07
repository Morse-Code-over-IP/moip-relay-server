//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		cwmessages.cs
//
// FACILITY:	RSS-Fed Morse Code News Robot 
//
// ABSTRACT:	Contains classes that encapsulate UDP packets as used by the
//				CWCom internet morse code system. Provides object oriented
//				view of the messages. Designed to be used together with the
//				protocol implementation in cwcom.cs.
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
// 05-Feb-10	rbd		0.5.3 - Add code needed to receive control messages,
//						creating from byte array. Add checking of data wnen
//						setting the Packet property. Use type enums more.
// 07-Feb-10	rbd		0.5.4 - For Ionosphere, add remote IP to received
//						message.
// 07-Apr-10	rbd		0.7.2 - Change "TXing" to "Sending"
//-----------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace com.dc3.cwcom
{

	/// <summary>
	/// Encapsulates the 496 byte CWCom UDP packets that are used for Ident and Code
	/// timing messages. 
	/// </summary>
	public abstract class LargeMessage
	{
		/// <summary>
		/// The two types of information that can be in a Large Message packet.
		/// </summary>
		public enum MessageTypes { Data = 0, ID = 1, Unknown = -1 }

		/// <summary>
		/// The length of a Large Message packet (Data or Ident).
		/// </summary>
		public const int Length = 496;

		private byte[] _buf;

		protected LargeMessage()
		{
			_buf = new byte[Length];
			for (int i = 0; i < Length; i++)								// Clear the whole packet buffer
				_buf[i] = 0;
			Util.PackShort(_buf, 0, 3);										// CmdCode always 3 for either data or ID
			Util.PackShort(_buf, 2, Length - 4);							// Actual data length - 2 (must be this value!)
			Util.PackInteger(_buf, 144, 1000);								// Constant magic ???
			ID = "NoID";													// Initialize string ID
		}

		/// <summary>
		/// The raw packet payload, 
		/// </summary>
		public byte[] Packet												// Access to packet bytes
		{
			get { return _buf; }
			set 
			{
				if (value.Length != Length)
					throw new ArgumentOutOfRangeException("Packet", "Byte array is the wrong size, must be " + Length + " bytes");
				if (Util.UnpackShort(value, 0) != 3)
					throw new ArgumentException("Packet", "Invalid type code for Data or Ident message");
				if (Util.UnpackShort(value, 2) != Length - 4)
					throw new ArgumentException("Packet", "Wrong length in Data or Ident message");
				_buf = value;
			}
		}

		/// <summary>
		/// Station identifier
		/// </summary>
		/// <remarks>Maximum length 128 characters. Used for 'callsign' in CWCom, but may be anything.
		/// </remarks>
		public string ID
		{
			get { return Util.UnpackString(_buf, 4); }
			set { Util.PackString(_buf, 4, value); }
		}

		/// <summary>
		/// Packet sequence number
		/// </summary>
		/// <remarks>After connecting, each distinct Large Message packet (Data or Info) must be 
		/// assigned a monotonically increasing sequence number. The sequence numbers are shared
		/// by both Data and Info packets.</remarks>
		public Int32 SequenceNo
		{
			get { return Util.UnpackInteger(_buf, 136); }
			set { Util.PackInteger(_buf, 136, value); }
		}

		public MessageTypes Type
		{
			get 
			{
				MessageTypes t;
				try { t = (MessageTypes)PacketType; }						// Avoid fatal error on bad received packet
				catch (Exception) { t = MessageTypes.Unknown; }
				return t; 
			}
			set { PacketType = (Int32)value; }
		}

		protected Int32 PacketType											// 0 = data, 1 = ID
		{
			get { return Util.UnpackInteger(_buf, 140); }
			set { Util.PackInteger(_buf, 140, value); }
		}

		protected Int32 Magic												// 16777215 = data, 65535 for ID
		{
			get { return Util.UnpackInteger(_buf, 148); }
			set { Util.PackInteger(_buf, 148, value); }
		}

		/// <summary>
		/// Mark/space timings for the character or prosign
		/// </summary>
		/// <remarks>This array contains the mark and space timings for the character or 
		/// prosign for which this (Data) packet is being sent. The times are in milliseconds.
		/// Positive values represent mark, negative values represent space.
		/// <para>The array is limited to 50 elements.</para>
		/// </remarks>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown when setting the property to an array with more than 50 elements.</exception>
		public Int32[] Code
		{
			get 
			{
				Int32[] e = new Int32[CodeCount];							// May be empty array, that's right!
				for (int i = 0; i < CodeCount; i++)
					e[i] = Util.UnpackInteger(_buf, (152 + (4 * i)));
				return e;
			}
			set 
			{
				int i;
				CodeCount = value.Length;
				if (CodeCount > 50)
					throw new ArgumentOutOfRangeException("Property Code[]", "Array size exceeds maximum of 50 elements");
				for (i = 0; i < value.Length; i++)
					Util.PackInteger(_buf, (152 + (4 * i)), value[i]);
				for (; i < 51; i++)										// Clean out old junk from past uses
					Util.PackInteger(_buf, (152 + (4 * i)), 0);
			}
		}

		public Int32 CodeCount
		{
			get { return Util.UnpackInteger(_buf, 356); }
			private set { Util.PackInteger(_buf, 356, value); }
		}

		public string Text													// Status ("QRV" etc. plus comments) if ID packet
		{																	// Text (1 char) if data packet
			get { return Util.UnpackString(_buf, 360); }
			set { Util.PackString(_buf, 360, value); }
		}

		//
		// There are 8 more bytes at the end. What are they?
		//
	}

	/// <summary>
	/// A CwCom data packet containing mark/space timings for a single character or prosign.
	/// </summary>
	public class DataMessage : LargeMessage
	{
		public IPAddress RemIP;												// Used by Ionosphere

		public DataMessage()
		{
			Type = MessageTypes.Data;
			Magic = 0xFFFFFF;
			Text = "Sending";
		} 
	}

	/// <summary>
	/// A CwCom "ident" packet, used to display a message in the right hand pane of the CWCom channel window.
	/// </summary>
	public class IdentMessage : LargeMessage
	{
		public IdentMessage()
		{
			Type = MessageTypes.ID;
			Magic = 0xFFFF;
		}
	}

	/// <summary>
	/// A CwCom "large message" for which we don't yet know the type. Use this class for receiving messages 
	/// by setting the Packet property from the incoming data.
	/// </summary>
	public class ReceivedMessage : LargeMessage
	{
		public ReceivedMessage(byte[] data) 
		{
			Packet = data;													// This will check length, etc.
		}
	}

	/// <summary>
	/// A CWCom 'control' packet, used for state maintenance between client and server or peer clients.
	/// </summary>
	public class CtrlMessage
	{
		public enum MessageTypes { Unknown = 0, Connect = 4, Disconnect = 2, Ack = 5 }
		public const int Length = 4;

		private byte[] _buf;

		public CtrlMessage()												// For receiving messages
		{
			_buf = null;													// Must set Packet property first
		}

		public CtrlMessage(MessageTypes type, short Channel)
		{
			_buf = new byte[4] {0, 0, 0, 0};
			CmdCode = (Int16)type;
			//switch (type)
			//{
			//    case MessageTypes.Connect:		CmdCode = 4; break;
			//    case MessageTypes.Disconnect:	CmdCode = 2; break;
			//    case MessageTypes.Ack:			CmdCode = 5; break;
			//}
			this.Channel = Channel;
		}

		public byte[] Packet												// Access to packet bytes
		{
			get { return _buf; }
			set 
			{
				if (value.Length != Length)
					throw new ArgumentOutOfRangeException("Packet", "Byte array is the wrong size, must be " + Length + " bytes");
				_buf = value; 
			}
		}

		public MessageTypes Type
		{
			get 
			{
				MessageTypes t;
				try { t = (MessageTypes)(CmdCode); }
				catch (Exception) { t = MessageTypes.Unknown; }
				return t;
			}
			set { CmdCode = (Int16)value; }
		}

		private Int16 CmdCode
		{
			get { return Util.UnpackShort(_buf, 0); }
			set { Util.PackShort(_buf, 0, value); }
		}

		public Int16 Channel
		{
			get { return Util.UnpackShort(_buf, 2); }
			set { Util.PackShort(_buf, 2, value); }
		}
	}

	//
	// Packing/unpacking functions and sequence number maintenance
	//
	static class Util
	{
		public static void PackShort(byte[] buf, int offs, Int16 val)
		{
			for (int i = 0; i < 2; i++)
			{
				buf[offs + i] = (byte)(val & 0xff);
				val >>= 8;
			}
		}

		public static Int16 UnpackShort(byte[] buf, int offs)
		{
			Int16 x = buf[offs + 1];
			x <<= 8;
			x += buf[offs];
			return x;
		}

		public static void PackInteger(byte[] buf, int offs, Int32 val)
		{
			for (int i = 0; i < 4; i++)
			{
				buf[offs + i] = (byte)(val & 0xff);
				val >>= 8;
			}
		}

		public static Int32 UnpackInteger(byte[] buf, int offs)
		{
			Int32 x = 0;
			for (int i = 3; i >= 0; i--)
			{
				x <<= 8;
				x += buf[offs + i];
			}
			return x;
		}

		public static void PackString(byte[] buf, int offs, string val)
		{
			for (int i = offs; i < offs + 128; i++)
				buf[i] = 0;
			ASCIIEncoding.ASCII.GetBytes(val, 0, val.Length, buf, offs);
		}

		public static string UnpackString(byte[] buf, int offs)
		{
			string raw = ASCIIEncoding.ASCII.GetString(buf, offs, 128);
			return raw.Substring(0, raw.IndexOf('\0'));
		}
	}
}
