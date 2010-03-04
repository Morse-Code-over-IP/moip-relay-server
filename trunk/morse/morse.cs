//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		morse.cs
//
// FACILITY:	Morse Code class
//
// ABSTRACT:	Encodes ASCII text to International Morse Code.
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
// 05-Feb-10	rbd		0.5.3
// 10-Feb-10	rbd		0.6.1
// ??-Feb-10	rbd		0.6.2-0.6.4
// 03-Mar-10	rbd		0.6.5 - MonoDevelop notation above
//-----------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Text;

namespace com.dc3.morse
{
	/// <summary>
	/// Provides International Morse Code encoding from a text string.
	/// </summary>
	/// <remarks>
	/// The <b>Morse</b> class provides simple methods for encoding a text string into International Morse Code. 
	/// Three types of Morse Code output are available:
	/// <list type="bullet">
	///		<item>
	///			<description><b>Dotscii</b> - ASCII text containing dots and dashes.</description>
	///		</item>
	///		<item>
	///			<description><b>MorseMail</b> - ASCII text containing mark/space timing as used by the
	///			<a href="http://brasspounder.com:8873/" target="_blank">MorseMail</a> application.</description>
	///		</item>
	///		<item>
	///			<description><b>CWCom</b> - An array of Int32 values representing the mark/space timing as used by the 
	///			<a href="http://www.mrx.com.au/d_cwcom.htm" target="_blank">CWCom</a> and 
	///			<a href="http://home.comcast.net/~morsekob/" target="_blank">MorseKOB</a> applications.</description>
	///		</item>
	///	</list>
	/// The Morse Code generated is so-called "modern" International Morse Code, conforming to the 
	/// <a href="http://www.godfreydykes.info/international%20morse%20code.pdf" target="_blank">ITU Recommendation
	/// ITU-R M.1677</a>, and described on the WikiPedia <a href="http://en.wikipedia.org/wiki/Morse_code" 
	/// target="_blank">Morse Code</a>.
	/// <para>Sending of <em>prosigns</em> is supported. A prosign is the concatenation of two Morse characters
	/// which have a special meaning. See the WikiPedia article <a href="http://en.wikipedia.org/wiki/Prosigns_for_Morse_code" 
	/// target="_blank">Prosigns for Morse Code</a>. 
	/// </para>
	/// <para>The mark/space timings generated for <see cref="CwCom"/> and 
	/// <see cref="MorseMail"/> are controlled by two properties,
	/// <see cref="CharacterWpm"/> and <see cref="WordWpm"/>. The separate properties for character and word speeds
	/// allow the <see cref="CwCom"/> and <see cref="MorseMail"/> encoders to generate timing in which the mark/space 
	/// timing of each character is at one WPM speed and the inter-character and inter-word spacing is lengthened 
	/// corresponding to a (slower) slower "word" WPM speed. Finally, (advanced usage), the time base (time 
	/// for a single dit at 1 WPM) can be adjusted via the <see cref="TimeBase" /> property.
	/// </para>
	/// <h1 class="heading">Calculating Morse Code Timing</h1>
	/// <para>The word PARIS is the standard to determine Morse code speed. Each dit is one element, each dah is three 
	/// elements, intra-character spacing is one element, inter-character spacing is three elements, and inter-word 
	/// spacing is seven elements. The word PARIS (and coincidentally also the word MORSE!), followed by a single
	/// inter-word space, consists of exactly 50 elements. If you send "PARIS " 15 times a minute (15WPM) you have 
	/// sent 750 elements (using correct spacing). Each element, therefore, is 60/750 = 0.08 sec. or 80 milliseconds.
	/// For normal/standard timing, set the <see cref="CharacterWpm"/> and <see cref="WordWpm"/> properties to the
	/// same value.</para>
	/// <h1 class="heading">Split-Speed (Farnsworth) Timing</h1>
	/// <para>The Farnsworth method uses timing in which the character and word speeds are set separately, with the
	/// word speed being slower. The dits, dahs, and intra-character spacing are at the (higher) character speed.
	/// The inter-character and inter-word spacing are increased, bringing the overall speed down to the (slower)
	/// word speed. For example, to send at 15 WPM character rate (as above) but with a 5 WPM word rate, the dits 
	/// and intra-character spacing will still be 80 ms. with dah of 240 ms. However, the inter-character
	/// spacing will be 1.38 seconds, and the inter-word spacing will be 3.99 seconds.
	/// </para>
	/// 
	/// </remarks>
	public class Morse
	{
		/// <summary>
		/// Used by the <see cref="CwCom"/> encoder to return the Int32[] array of mark/space timings and the corresponding
		/// text for each character or prosign in the input text. The <see cref="CwCom"/> encoder calls the user provided
		/// delegate for each character.
		/// </summary>
		/// <param name="code">Array of Int32 values containing mark/space timings in milliseconds for one 
		/// character of the input text. This will include leading space time if there was a preceding character
		/// in the imput text. Positive values indicate a mark and negative values indicate a space.</param>
		/// <param name="text">ASCII text for the encoded character. This will be a single upper case
		/// character equal to the input character, possibly preceded by a space if the input character
		/// is preceded by a space. If a prosign was encoded, this will be the multiple-character 
		/// representation of the prosign, for example <b>AR</b>.</param>
		/// <remarks>This caller-supplied deldgate is called once by <see cref="CwCom" /> for 
		/// <em>each character or prosign</em> in the input text.
		/// See the example program in the documentation for <see cref="CwCom" />.</remarks>
		/// <seealso cref="CwCom" />
		public delegate void SendCode(Int32[] code, string text);

		//
		// Encoding dictionary - taken from:
		//    http://en.wikipedia.org/wiki/Morse_code
		//    ITU Recommendation ITU-R M.1677
		//
		private Dictionary<char, string> _morse = new Dictionary<char, string>()
		{
			{ 'A', ".-" },
			{ 'B', "-..." },
			{ 'C', "-.-." },
			{ 'D', "-.." },
			{ 'E', "." },
			{ 'F', "..-." },
			{ 'G', "--." },
			{ 'H', "...." },
			{ 'I', ".." },
			{ 'J', ".---" },
			{ 'K', "-.-" },
			{ 'L', ".-.." },
			{ 'M', "--" },
			{ 'N', "-." },
			{ 'O', "---" },
			{ 'P', ".--." },
			{ 'Q', "--.-" },
			{ 'R', ".-." },
			{ 'S', "..." },
			{ 'T', "-" },
			{ 'U', "..-" },
			{ 'V', "...-" },
			{ 'W', ".--" },
			{ 'X', "-..-" },
			{ 'Y', "-.--" },
			{ 'Z', "--.." },
			{ '1', ".----" },
			{ '2', "..---" },
			{ '3', "...--" },
			{ '4', "....-" },
			{ '5', "....." },
			{ '6', "-...." },
			{ '7', "--..." },
			{ '8', "---.." },
			{ '9', "----." },
			{ '0', "-----" },
			{ '.', ".-.-.-" },
			{ ',', "--..--" },
			{ ':', "---..." },
			{ ';', "-.-.-." },
			{ '?', "..--.." },
			{ '-', "-....-" },
			{ '+', ".-.-." },
			{ '/', "-..-." },
			{ '=', "-...-" },
			{ '@', ".--.-." },
			{ '!', "-.-.--" },
			{ '\'', ".----." },
			{ '(', "-.--." },
			{ ')', "-.--.-" },
			{ '$', "...-..-" },
			{ '&', ".-..." },
			{ '"', ".-..-." },
			{ '_', "..--.-" },
			{ '\r', " " },
			{ '\n', " " },
			{ '\t', " " },
			{ ' ', " " }
		};

		private int _tbase;					// Time base for millisecond timings. A standard 1WPM dit = 1200 ms mark, 1200 ms space
		private int _cwpm;					// Character WPM
		private int _wwpm;					// Word WPM
		private int _ctime;					// Character dit time (ms)
		private int _stime;					// Symbol space time (ms)
		private int _cstime;				// Character space time (ms)
		private int _wstime;				// Word space time (ms)
		private int _accSpace;				// Space accumulator for timing
		private string _accWhite;			// Whitespace accumulator for sent characters
		private Int32[] _cwCode;			// Array for generated CWCode 
		private int _cwCount;

		/// <summary>
		/// Constructor for the Morse class
		/// </summary>
		public Morse()
		{
			_tbase = 1200;
			_cwpm = 15;
			_wwpm = _cwpm;
			_ctime = _tbase / _cwpm;
			_stime = _ctime;
			_cstime = _stime * 2;
			_wstime = _stime * 4;
			_accSpace = 0;
			_accWhite = "";
			_cwCode = new Int32[51];					// Max size for CWCode 
			_cwCount = 0;
		}

		private void _calcSpaceTime()					// Calculate space times for Farnsworth (word rate < char rate)
		{
			//
			// Do not allow word rate > char rate to cause shorter
			// than standard spaces. Property should prevent this!
			//
			if (_wwpm > _cwpm) _wwpm = _cwpm;

			//
			// There are 50 units in "PARIS " - 36 are in the characters, 14 are in the spaces
			//
			int t_total = (_tbase / _wwpm) * 50;
			int t_chars = (_tbase / _cwpm) * 36;
			_stime = (t_total - t_chars) / 14;			// Time for 1 space (ms)
			_cstime = _stime * 2;						// Character and word spacing
			_wstime = _stime * 4;
		}

		//
		// For CWCode encoding
		//
		private void start_cw()
		{
			_cwCount = 0;
			for (int i = 0; i < _cwCode.Length; i++)
				_cwCode[i] = 0;							// Clean array to make debugging esaier
		}

		private void mark_cw(int ms)
		{
		    if (_accSpace > 0)
		    {
				_cwCode[_cwCount++] = -_accSpace;
		        _accSpace = 0;
		    }
		    _cwCode[_cwCount++] = ms;
		}

		//
		// For MorseMail encoding
		//
		private string mark_mm(int ms)
		{
			string buf = "";
			if (_accSpace > 0)
			{
				buf += (-_accSpace).ToString("+#;-#;#");
				_accSpace = 0;
			}
			buf += ms.ToString("+#;-#;#");
			return buf;
		}

		//
		// Common to both protocols, hold space time for next
		//
		private void space(int ms)
		{
		    _accSpace += ms;
		}

		// -----------------
		// Public Properties
		// -----------------

		/// <summary>
		/// (advanced) The time base for mark/space timings.
		/// </summary>
		/// <remarks> This is the symbol/dit time in milliseconds at 1 WPM. 
		/// The default value is 1200 and should be used for most applications. 
		/// For details, see the remarks for <see cref="Morse" />.
		/// </remarks>
		/// <seealso cref="CharacterWpm" />
		/// <seealso cref="WordWpm" />
		public int TimeBase
		{
			get { return _tbase; }
			set { _tbase = value; }
		}

		/// <summary>
		/// The character speed in words per minute (default = 15). For details, see the remarks
		/// for <see cref="Morse" />.
		/// </summary>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown when the value is set &lt; 5 WPM or &gt; 100 WPM</exception>
		/// <seealso cref="WordWpm" />
		/// <seealso cref="TimeBase" />
		public int CharacterWpm
		{
			get { return _cwpm; }
			set
			{
				if (value < 5 || value > 100)
					throw new ArgumentOutOfRangeException("CharacterWpm", "must be between 5 and 100 WPM");
				_cwpm = value;
				_ctime = _tbase / _cwpm;
				_calcSpaceTime();
			}
		}

		/// <summary>
		/// The word speed in words per minute (default = 15). For details, see the remarks
		/// for <see cref="Morse" />.
		/// </summary>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown when the value is set &lt; 5 WPM or &gt; 100 WPM, 
		/// or to a value greater than <see cref="CharacterWpm" />.</exception>
		/// <seealso cref="CharacterWpm" />
		/// <seealso cref="TimeBase" />
		public int WordWpm
		{
			get { return _wwpm; }
			set
			{
				if (value < 5 || value > 100)
					throw new ArgumentOutOfRangeException("WordWpm", "must be between 5 and 100 WPM");
				if (value > _cwpm)
					throw new ArgumentOutOfRangeException("WordWpm", "cannot be greater than CharacterWPM");
				_wwpm = value;
				_calcSpaceTime();
			}
		}

		// --------------
		// Public Methods
		// --------------

		/// <summary>
		/// Convert ASCII text to 'dotscii' text, a series of '.' and '-' characters.
		/// </summary>
		/// <param name="Text">The ASCII text to be encoded</param>
		/// <remarks>
		/// Spaces (or other whitespace like tabs) are represented by two spaces in the dotscii.
		/// Prosigns are supported. To encode a prosign, enclose the letters in backslashes, 
		/// for example \AR\.
		/// </remarks>
		/// <returns>The 'dotscii' representation of the resulting Morse Code.</returns>
		/// <exception cref="System.ApplicationException">Thrown when the <b>Text</b> parameter is null or an empty string</exception>
		/// <example>
		///		This example shows how to call the DotDash() method:
		///		<code lang="C#">
		///		using com.dc3.morse;
		///		...
		///		class TestDotscii
		///		{
		///			static int Main()
		///			{
		///				Morse M = new Morse();
		///				Console.WriteLine(M.DotDash("Hello test\AR\"));
		///				Console.WriteLine("Press return to exit...");
		///				Console.ReadLine();
		///			}
		///		}
		///		
		///		Result: .... . .-.. .-.. ---  - . ... -.-.-.
		///		
		///		</code>
		/// </example>
		public string DotDash(string Text)
		{
			if (Text == null || Text == "")
				throw new ArgumentNullException("Text", "empty or null input string");

			string buf = "";
			bool inProsign = false;

			foreach (char c in Text.ToUpper())
			{
				if (c == '\\')												// Prosign delimiter
				{
					inProsign = !inProsign;
					if (!inProsign) buf += " ";
					continue;
				}
				if (_morse.ContainsKey(c))
					buf += _morse[c];
				else
					buf += "......";
				if (c != ' ' && !inProsign)
					buf += " ";
			}
			return buf.Trim();												// Remove trailing space(s)
		}

		//
		// Encode string to MorseMail string (without the XML)
		//
		// Handles arbitrary prosigns delimited by \xx\
		//
		// NOTE: Trailing space is held for sending at the start of
		//       subsequent calls.
		//

		/// <summary>
		/// Convert ASCII text to a series of mark/space timings as used by the 
		/// <a href="http://brasspounder.com:8873/" target="_blank">MorseMail</a> program.
		/// </summary>
		/// <param name="Text">The ASCII text to be encoded</param>
		/// <remarks>Prosigns are supported. To encode a prosign, enclose the letters in backslashes, 
		/// for example \AR\. The enclosing &lt;MorseMail&gt;&lt;/MorseMail&gt; XML tags are not
		/// included in the returned string.
		/// </remarks>
		/// <note type="caution">The returned timings will include a leading negative (space) value which
		/// is the "held over" trailing space from a previous call to MorseMail().
		/// </note>
		/// <returns>The <a href="http://brasspounder.com:8873/" target="_blank">MorseMail</a>
		/// representation of the resulting Morse Code.
		/// </returns>
		/// <exception cref="System.ApplicationException">Thrown when the <b>Text</b> parameter is null or an empty string</exception>
		/// <example>
		///		This example shows how to call the MorseMail() method:
		///		<code lang="C#">
		///		using com.dc3.morse;
		///		...
		///		class TestMorseMail
		///		{
		///			static int Main()
		///			{
		///				Morse M = new Morse();
		///				M.CharacterWpm = 15;
		///				M.WordWpm = 15;
		///				Console.WriteLine(M.MorseMail("Hello test\AR\"));
		///				Console.WriteLine("Press return to exit...");
		///				Console.ReadLine();
		///			}
		///		}
		///		
		///		Result (15/15 WPM timings, no line breaks in actual return): 
		///		
		///		+80-80+80-80+80-80+80-240+80-240+80-80+240-80+80-80+80-240
		///		+80-80+240-80+80-80+80-240+240-80+240-80+240-560+240-240+80-240
		///		+80-80+80-80+80-240+240-80+80-80+240-80+80-80+240-80+80
		///		
		///		</code>
		/// </example>
		public string MorseMail(string Text)
		{
			if (Text == null || Text == "")
				throw new ArgumentNullException("Text", "empty or null input string");

			string mm = "";
			bool inProsign = false;

			foreach (char c in Text.ToUpper())
			{
				string dotscii;

				if (c == '\\')												// Prosign delimiter
				{
					inProsign = !inProsign;
					if (!inProsign) space(_cstime);							// End prosign with char space
					continue;
				}

				if (_morse.ContainsKey(c))
					dotscii = _morse[c];
				else
					dotscii = "......";
				foreach (char s in dotscii)
				{
					switch (s)
					{
						case '.':											// Dit
							mm += mark_mm(_ctime);
							space(_ctime);
							break;
						case '-':											// Dah = 3 dits
							mm += mark_mm(_ctime * 3);
							space(_ctime);
							break;
						case ' ':											// Word break
							space(_wstime - _cstime);						// Preceding char has trailing character space
							break;
					}
				}
				if (!inProsign)												// Unless doing prosign
					space(_cstime);											// Character space
			}
			return mm;
		}

		//
		// Encode string to CWCom Int32 code words. Calls the delegate once per
		// character with Int32[] of mark-space timings, the count of elements
		// and the ASCII character. It's up to the caller's delegate to send
		// the code words the right way.
		//
		// Handles arbitrary prosigns delimited by \xx\ (made it a bit complex, sorry)
		//
		// NOTE: Trailing space is held for sending at the start of
		//       subsequent calls. THis also applies for whitespace
		//       for text, also queued for prefixing.
		//

		/// <summary>
		/// Convert ASCII text to an array of mark/space timings as used by the 
		///	<a href="http://www.mrx.com.au/d_cwcom.htm" target="_blank">CWCom</a> and 
		///	<a href="http://home.comcast.net/~morsekob/" target="_blank">MorseKOB</a> applications.
		/// </summary>
		/// <param name="Text">The ASCII text to be encoded</param>
		/// <param name="Sender">Caller-supplied delegate which is called with an array of mark/space
		/// timings for each character or prosign in the input text.</param>
		/// <remarks>As the ASCII input text is converted, the <see cref="SendCode" /> delegate is called with
		/// an Int32 array of the mark/space timings (and the corresponding text) <em>once for each character or prosign</em>
		/// in the input text. For more details see delegate <see cref="SendCode" />.
		/// <para>Prosigns are supported. To encode a prosign, enclose the letters in backslashes, 
		/// for example \AR\. The text for a prosign ends with a newline, thus in CWCom subsequent text
		/// will begin on a new line. Unfortunately, this text is colored red (by default).
		/// </para>
		/// </remarks>
		/// <note type="caution">The returned timings will include a leading negative (space) value which
		/// is the "held over" trailing space from a previous call to MorseMail().
		/// </note>
		/// <exception cref="System.ApplicationException">Thrown when the <b>Text</b> parameter is null or an empty string</exception>
		/// <example>
		///		This example shows how to call the CwCom() method:
		///		<code lang="C#">
		///		using com.dc3.morse;
		///		...
		///		class TestCwCom
		///		{
		///			void ShowCwCode(Int32[] Code, string Text)			// Delegate
		///			{
		///				Console.WriteLine("-------------");
		///				Console.WriteLine("Text:  \"" + Text + "\"");
		///				Console.WriteLine("Count: " + Code.Length);
		///				for (int i = 0; i &lt; Code.Length; i++)
		///					Console.Write(Code[i].ToString("+#;-#;#"));
		///				Console.WriteLine();
		///			}
		///			
		///			static int Main()
		///			{
		///				Morse M = new Morse();
		///				M.CharacterWpm = 15;
		///				M.WordWpm = 15;
		///				M.CwCom("Hello test\\AR\\");					// \\ is '\' in C#
		///				Console.WriteLine("Press return to exit...");
		///				Console.ReadLine();
		///			}
		///		}
		///		
		///		Result:
		///		-------------
		///		Text:  "H"
		///		Count: 7
		///		+80-80+80-80+80-80+80
		///		-------------
		///		Text:  "E"
		///		Count: 2
		///		-240+80
		///		-------------
		///		Text:  "L"
		///		Count: 8
		///		-240+80-80+240-80+80-80+80
		///		-------------
		///		Text:  "L"
		///		Count: 8
		///		-240+80-80+240-80+80-80+80
		///		-------------
		///		Text:  "O"
		///		Count: 6
		///		-240+240-80+240-80+240
		///		-------------
		///		Text:  " T"				(note leading space)
		///		Count: 2
		///		-560+240
		///		-------------
		///		Text:  "E"
		///		Count: 2
		///		-240+80
		///		-------------
		///		Text:  "S"
		///		Count: 6
		///		-240+80-80+80-80+80
		///		-------------
		///		Text:  "T"
		///		Count: 2
		///		-240+240
		///		-------------
		///		Text:  " AR\r\n"		(note leading space and entire prosign, end-of-line)
		///		Count: 10
		///		-240+80-80+240-80+80-80+240-80+80
		///		Press return to exit...
		///		</code>
		///	</example>
		///
		public void CwCom(string Text, SendCode Sender)
		{
			if (Text == null || Text == "")
				throw new ArgumentNullException("Text", "empty or null input string");

			string prosign = "";
			bool inProsign = false;
			bool sendProsign = false;

			foreach (char c in Text.ToUpper())
			{
				string dotscii;
				char c2 = c;

				if (!inProsign)
					start_cw();												// Not in prosign, start fresh
				else if (c != '\\')											// In prosign (and not ending delim)
					prosign += c;											// Accumulate it for CwCode text

				if (c == '\\')												// Prosign delimiter
				{
					inProsign = !inProsign;
					if (inProsign)
						continue;
					else
					{
						c2 = ' ';											// End prosign with space
						sendProsign = true;
					}
				}

				if (_morse.ContainsKey(c2))
					dotscii = _morse[c2];
				else
					dotscii = "......";

				foreach (char s in dotscii)
				{
					switch (s)
					{
						case '.':											// Dit
							mark_cw(_ctime);
							space(_ctime);
							break;
						case '-':											// Dah = 3 dits
							mark_cw(_ctime * 3);
							space(_ctime);
							break;
						case ' ':											// Word break
							_accWhite += ' ';								// Accumulate whitespace for text
							space(_wstime - _cstime);						// Preceding char has trailing character space
							break;
					}
				}
				if (inProsign) continue;									// Still making prosign, don't send yet!
				if (dotscii.Trim() != "" || sendProsign)					// Unless dotscii is just whitespace or prosign finished
				{
					Int32[] code = new Int32[_cwCount];						// Create "just right size" array
					for (int i = 0; i < _cwCount; i++)
						code[i] = _cwCode[i];
					Sender(code, _accWhite + (sendProsign ? prosign + "\r\n" : c.ToString()));  // Call delegate for this character
					_accWhite = "";
					sendProsign = false;
					prosign = "";
				}
				if (!inProsign)												// Unless doing prosign
					space(_cstime);											// Character space
			}
		}

	}
}
