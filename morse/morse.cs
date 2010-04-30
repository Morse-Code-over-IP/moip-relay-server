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
// 25-Mar-10	rbd		0.6.8 - Error is 8 dits not 6
// 01-Apr-10	rbd		0.7.1 - American Morse Code, new Mode property.
// 05-Apr-10	rbd		0.7.2 - Additional punctuation for American Morse. Fix
//						'error' code for CwCom sending in International mode.
//						Add separate error code for American mode. Refactor.
//						Add 1 second space before first packet sent, needed by
//						MorseKOB's decoder but harmless in any case. 
//						Add plain double quote to American, use Open/Left 
//						encoding. Correct punctuation per Les Kerr and the 
//						book The Telegraph Instructor by Dodge (Google Books),
//						they have Morse spaces in them, like a prosign.
// 07-Apr-10	rbd		0.7.2 - Fix American Morse '/'. Put unknown characters
//						into dotscii enclosed in [] instead of printing error
//						code in dotscii. Add unknown character delegate.
// 15-Apr-10	rbd		0.7.2 - If UnknownChar delegate is active (caller is 
//						being told of unknowns), don't send error code to
//						MorseMail and CwCom, instead just a blank.
// 28-Apr-10			1.1.0 - Mono exclusion for Unicode open and close double
//						quotes, they appear same to Mono, resulting in a 
//						duplicate key exception in ctor.
// 30-Apr-10	rbd		1.2.0 - Oops, unknown char -> space upset timing in
//						American, and caused tests to fail! Do not send extra
//						space on " " just the real error code! Throw exception
//						if WordWPM is changed to other than CharWPM when in
//						American mode.
//-----------------------------------------------------------------------------
//
using System;
using System.Collections.Generic;
using System.Text;

namespace com.dc3.morse
{
	/// <summary>
	/// Provides International or American Morse Code encoding from a text string.
	/// </summary>
	/// <remarks>
	/// The <b>Morse</b> class provides simple methods for encoding a text string into International or American Morse Code. 
	/// Three types of Morse Code output are available:
	/// <list type="bullet">
	///		<item>
	///			<description><b>Dotscii</b> - ASCII text containing dots and dashes (plus some special characters for American Morse Code).</description>
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
	/// By default, the Morse Code generated is so-called "modern" International Morse Code, conforming to the 
	/// <a href="http://www.godfreydykes.info/international%20morse%20code.pdf" target="_blank">ITU Recommendation
	/// ITU-R M.1677</a> and described on the WikiPedia <a href="http://en.wikipedia.org/wiki/Morse_code" 
	/// target="_blank">Morse Code</a> page. 
	/// <para>Alternatively, American Morse Code may be generated by first setting <see cref="Mode" />
	/// to <b>American</b>. There is no accepted international standard for American Morse Code. The encodings used herein are
	/// taken from the WikiPedia <a href="http://en.wikipedia.org/wiki/American_Morse_code" target="_blank">American 
	/// Morse Code</a> page, as well as the document
	/// <a href="http://www.morsetelegraphclub.org/library/files/html/dodge/index.htm" target="_blank">The Telegraph 
	/// Instructor</a> by G. M. Dodge (also available on Google Books as PDF). The latter document contains punctuation encodings
	/// that do not appear in the WikiPedia article. In addition, commonly accepted encodings for the equal 
	/// sign '=' and forward slash '/' are included in preference to their meanings as described by Miller (ibid.)
	/// which are the paragraph mark and shilling mark, respectively.
	/// <para>Sending of <em>prosigns</em> is supported. A prosign is the concatenation of two Morse characters
	/// which have a special meaning. See the WikiPedia article <a href="http://en.wikipedia.org/wiki/Prosigns_for_Morse_code" 
	/// target="_blank">Prosigns for Morse Code</a>. In American Morse mode, prosigns are send as two letters enclosed in 
	/// slash marks (e.g. "/AR/") with normal timing. Note that the trailing slash causes an extra half space time to be 
	/// added to the end of the prosign.</para>
	/// </para>
	/// <para>The mark/space timings generated for <see cref="CwCom"/> and 
	/// <see cref="MorseMail"/> are controlled by two properties,
	/// <see cref="CharacterWpm"/> and <see cref="WordWpm"/>. The separate properties for character and word speeds
	/// allow the <see cref="CwCom"/> and <see cref="MorseMail"/> encoders to generate timing in which the mark/space 
	/// timing of each character is at one WPM speed and the inter-character and inter-word spacing is lengthened 
	/// corresponding to a (slower) slower "word" WPM speed. Finally, (advanced usage), the time base (time 
	/// for a single dit at 1 WPM) can be adjusted via the <see cref="TimeBase" /> property. <u>Please note that 
	/// split-speeds are not supported in American Morse Code mode</u>.
	/// </para>
	/// <h1 class="heading">International Morse Code Timing</h1>
	/// <para>The word PARIS is the standard to determine Morse code speed. Each dit is one element, each dah is three 
	/// elements, intra-character spacing is one element, inter-character spacing is three elements, and inter-word 
	/// spacing is seven elements. The word PARIS (and coincidentally also the word MORSE!), followed by a single
	/// inter-word space, consists of exactly 50 elements. If you send "PARIS " 15 times a minute (15WPM) you have 
	/// sent 750 elements (using correct spacing). Each element, therefore, is 60/750 = 0.08 sec. or 80 milliseconds.
	/// For normal/standard timing, set the <see cref="CharacterWpm"/> and <see cref="WordWpm"/> properties to the
	/// same value.</para>
	/// <h1 class="heading">American Morse Code Timing</h1>
	/// <para>The mark/space timings for American Morse Code are considerably more complex. As per International code,
	/// the element ("dit") length is set to the time base divided by the character speed <see cref="CharacterWpm" />. 
	/// Inter-character spacing is 
	/// nominally 2.75 elements. Numbers, punctuation symbols, and the letter 'L' have an extra half an element time added to this
	/// inter-character spacing, for a total of 3.25 elements. Inter-word spacing is 6 elements minus the inter-character 
	/// spacing.
	/// </para>
	/// <para>Dits are always one element in length. Dahs are 3.3 elements in length unless being followed by another dah, 
	/// in which case all but the last dah are 2.7 elements in length. Furthermore, the time between consecutive dahs 
	/// is 1.6 elements. The last dah in a run of dahs is followed by the normal one element of time.
	/// </para>
	/// <para>Within a character, in addition to dits and dahs, American Morse has the "Morse space", a longer than 
	/// normal gap between dits and dahs. The letter 'C' is an example, it has two dits followed by a "short" pause
	/// then one more dit. This "Morse space" is 2.7 elements in length.
	/// </para>
	/// <para>Finally, the letter 'L' is sent by a long dah, 5.6 elements long, and the number '0' is sent by an 
	/// even longer dah, 8.9 elements long. As mentioned above, both are followed by the inter-character spacing plus
	/// a half an element.
	/// </para>
	/// <h1 class="heading">Split-Speed (Farnsworth) Timing</h1>
	/// <para>The Farnsworth method uses timing in which the character and word speeds are set separately, with the
	/// word speed being slower. The dits, dahs, and intra-character spacing are at the (higher) character speed.
	/// The inter-character and inter-word spacing are increased, bringing the overall speed down to the (slower)
	/// word speed. For example, to send at 15 WPM character rate (as above) but with a 5 WPM word rate, the dits 
	/// and intra-character spacing will still be 80 ms. with dah of 240 ms. However, the inter-character
	/// spacing will be 1.38 seconds, and the inter-word spacing will be 3.99 seconds.
	/// </para>
	/// <para>Split-speed timing is approximated for American Morse mode. The actual overall WPM will not be exact;
	/// it will be a bit fast. For split speed in American Morse mode, the inter-character and inter-word space 
	/// times are simply mutiplied by the ratio of the character speed to the word speed (always greater than or 
	/// equal to one).
	/// </para>
	/// </remarks>
	public class Morse
	{
		/// <summary>
		/// The type of Morse Code to be used.
		/// </summary>
		public enum CodeMode
		{
			/// <summary>
			/// International Morse Code
			/// </summary>
			International,
			/// <summary>
			/// American Morse Code
			/// </summary>
			American
		}

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

		/// <summary>
		/// Called whenever the encoder encounters a character for which it has no corresponding Morse.
		/// </summary>
		/// <param name="ch">The unknown character</param>
		/// <remarks>Defaults to null, must be set via <see cref="UnknownCharacter" />.</remarks>
		public delegate void UnknownCharacterHandler(char ch);

		//
		// International Morse encoding dictionary - taken from:
		//    http://en.wikipedia.org/wiki/Morse_code
		//    ITU Recommendation ITU-R M.1677
		//
		private Dictionary<char, string> _iMorse = new Dictionary<char, string>()
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

		//
		// American Morse encoding dictionary - taken from:
		//    http://en.wikipedia.org/wiki/American_Morse_code
		//    http://www.morsetelegraphclub.org/files/TelegraphCodes.pdf 
		//
		// Punctuation not implemented here:
		//    Pounds Sterling
		//    Colon Dash
		//    Not Code
		//    Quotation Marks Within A Quotation
		//
		// Underscore '_' is single unit space
		// Equal sign '=' is four unit mark ('L')
		// Sharp sign '#' is five unit mark ('0')
		//
		// It is amazing that I chose = and # the same as MorseKOB without first looking!
		//
		private Dictionary<char, string> _aMorse = new Dictionary<char, string>()
		{
			{ 'A', ".-" },
			{ 'B', "-..." },
			{ 'C', ".._." },
			{ 'D', "-.." },
			{ 'E', "." },
			{ 'F', ".-." },
			{ 'G', "--." },
			{ 'H', "...." },		// Also ampersand
			{ 'I', ".." },
			{ 'J', "-.-." },
			{ 'K', "-.-" },
			{ 'L', "=" },
			{ 'M', "--" },
			{ 'N', "-." },
			{ 'O', "._." },
			{ 'P', "....." },
			{ 'Q', "..-." },
			{ 'R', "._.." },
			{ 'S', "..." },
			{ 'T', "-" },
			{ 'U', "..-" },
			{ 'V', "...-" },
			{ 'W', ".--" },
			{ 'X', ".-.." },
			{ 'Y', ".._.." },
			{ 'Z', "..._." },
			{ '1', ".--." },
			{ '2', "..-.." },
			{ '3', "...-." },
			{ '4', "....-" },
			{ '5', "---" },
			{ '6', "......" },
			{ '7', "--.." },
			{ '8', "-...." },
			{ '9', "-..-" },
			{ '0', "#" },
			{ '.', "..--.." },
			{ ',', ".-.-" },
			{ '?', "-..-." },
			{ '!', "---." },
			{ '&', "._..." },		// (mind the Morse spaces in these, typ.)
			{ '=', "----" },		// Also paragraph mark, used as break too
			{ '/', ".._-" },
			{ ':', "-.-_._." },
			{ '$', "..._.-.." },
			{ '(', "....._-." },
			{ ')', "....._.._.." },
			{ '\'', "..-._.-.." },
#if !MONO_BUILD			
			{ '�', "..-._-." },		// Open/Left double quote
			{ '�', "..-._-.-." },	// Close/Right double quote
#endif
			{ '"', "..-._-." },		// Plain quote -> Open/Left double quote
			{ '[', "-..._.-.." },	// Same for L/R bracket
			{ ']', "-..._.-.." },
			{ '-', "...._.-.." },
			{ '%', "....._.." },	// ?? 'PI' ??
			{ '@', ".-_.-.." },		// Not really American, but need this, 'AX'
			{ '\r', " " },
			{ '\n', " " },
			{ '\t', " " },
			{ ' ', " " }
		};

		//
		// This has flags for adding a bit to the inter-character space after certain
		// characters (numbers and punctuation) in American Morse Code only.
		//
		private Dictionary<char, bool> _aSpaceEmph = new Dictionary<char, bool>()
		{
			{ 'A', false },
			{ 'B', false },
			{ 'C', false },
			{ 'D', false },
			{ 'E', false },
			{ 'F', false },
			{ 'G', false },
			{ 'H', false },
			{ 'I', false },
			{ 'J', false },
			{ 'K', false },
			{ 'L', true },
			{ 'M', false },
			{ 'N', false },
			{ 'O', false },
			{ 'P', false },
			{ 'Q', false },
			{ 'R', false },
			{ 'S', false },
			{ 'T', false },
			{ 'U', false },
			{ 'V', false },
			{ 'W', false },
			{ 'X', false },
			{ 'Y', false },
			{ 'Z', false },
			{ '1', true },
			{ '2', true },
			{ '3', true },
			{ '4', true },
			{ '5', true },
			{ '6', true },
			{ '7', true },
			{ '8', true },
			{ '9', true },
			{ '0', true },
			{ '.', true },
			{ ',', true },
			{ '?', true },
			{ '!', true },
			{ '&', true },
			{ '=', true },
			{ '/', true },
			{ ':', true },
			{ '$', true },
			{ '(', true },
			{ ')', true },
			{ '\'', true },
#if !MONO_BUILD
			{ '�', true },
			{ '�', true },
#endif
			{ '"', true },
			{ '[', true },
			{ ']', true },
			{ '-', true },
			{ '%', true },
			{ '@', true },
			{ '\r', false },				// TODO Should these have extra spacing?
			{ '\n', false },
			{ '\t', false },
			{ ' ', false },					// No extra spacing here!
			{ '\\', false }					// Needed for prosign handling
		};

		private const string _errCodeIntl = "........";
		private const string _errCodeAmer = "._._._._._. ";

		private CodeMode _mode;
		private string _errCode;			// Dotscii for error (Intl vs Amer)
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
		private UnknownCharacterHandler _unkChar;

		/// <summary>
		/// Constructor for the Morse class
		/// </summary>
		public Morse()
		{
			_mode = CodeMode.International;
			_errCode = _errCodeIntl;
			_tbase = 1200;
			_cwpm = 15;
			_wwpm = _cwpm;
			_ctime = _tbase / _cwpm;
			_stime = _ctime;
			_cstime = _stime * 2;
			_wstime = _stime * 4;
			_accSpace = 1000;							// Leading space for very first character
			_accWhite = "";
			_cwCode = new Int32[51];					// Max size for CWCode 
			_cwCount = 0;
			_unkChar = null;
		}

		private void _calcSpaceTime()					// Calculate space times for Farnsworth (word rate < char rate)
		{
			//
			// Do not allow word rate > char rate to cause shorter
			// than standard spaces. Property should prevent this!
			//
			if (_wwpm > _cwpm) _wwpm = _cwpm;

			if (_mode == CodeMode.International)
			{
				//
				// There are 50 units in "PARIS " - 36 are in the characters, 14 are in the spaces
				//
				int t_total = (_tbase / _wwpm) * 50;
				int t_chars = (_tbase / _cwpm) * 36;
				_stime = (t_total - t_chars) / 14;			// Time for 1 space (ms)
				_cstime = _stime * 2;						// Character and word spacing
				_wstime = _stime * 4;
			}
			else
			{
				//
				// American Morse, rather subjective. This produces nearly the same
				// timings as the MorseKOB program, which has one of the best American
				// Morse decoders around. Split timing is a fudge, may need to change.
				//
				_stime = _ctime;
				_cstime = (int)(_stime * 2.75);
				_wstime = (_stime * 6) - _cstime;
				if (_wwpm < _cwpm)
				{
					_cstime = (int)((double)_cstime * (double)_cwpm / (double)_wwpm);
					_wstime = (int)((double)_wstime * (double)_cwpm / (double)_wwpm);
				}
			}
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
		/// The type of Morse code (default International). For details, see the remarks
		/// for <see cref="Morse" />.
		/// </summary>
		/// <remarks>Changing this property sets <see cref="WordWpm" /> equal to <see cref="CharacterWpm" /></remarks>
		public CodeMode Mode
		{
			get { return _mode; }
			set 
			{ 
				_mode = value;
				_wwpm = _cwpm;
				if (value == CodeMode.International)
					_errCode = _errCodeIntl;
				else
					_errCode = _errCodeAmer;
				_accSpace = 1000;											// Reset the leading space just in case
				_calcSpaceTime();
			}
		}

		/// <summary>
		/// (advanced) The time base for mark/space timings.
		/// </summary>
		/// <remarks> This is the symbol/dit time in milliseconds at 1 WPM. 
		/// The default value is 1200 and should be used for most applications. 
		/// For details, see the remarks for <see cref="Morse" />.
		/// Changing this property sets <see cref="WordWpm" /> equal to <see cref="CharacterWpm" />
		/// </remarks>
		/// <seealso cref="CharacterWpm" />
		/// <seealso cref="WordWpm" />
		public int TimeBase
		{
			get { return _tbase; }
			set 
			{ 
				_tbase = value;
				_ctime = _tbase / _cwpm;
				_wwpm = _cwpm;
				_calcSpaceTime();
			}
		}

		/// <summary>
		/// The character speed in words per minute (default = 15). For details, see the remarks
		/// for <see cref="Morse" />.
		/// </summary>
		/// <remarks>Changing this property sets <see cref="WordWpm" /> equal to the value being set. For split
		/// speeds, set this property first, then <see cref="WordWpm" />.</remarks>
		/// <exception cref="System.ArgumentOutOfRangeException">Thrown when the value is set &lt; 5 WPM or &gt; 100 WPM.</exception>
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
				_wwpm = _cwpm;
				_ctime = _tbase / _cwpm;
				_calcSpaceTime();
			}
		}

		/// <summary>
		/// The word speed in words per minute (default = 15). For details, see the remarks
		/// for <see cref="Morse" />.
		/// </summary>
		/// <remarks>For split speed, set this
		/// property last, after <see cref="TimeBase" /> and/or <see cref="CharacterWpm" />.
		/// </remarks>
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

		/// <summary>
		/// The unknown character handler delegate.
		/// </summary>
		/// <remarks>Setting this to an <see cref="UnknownCharacterHandler" /> delegate will cause the Morse encoder to call
		/// the delegate for any input characters that are not in its encoding table. Use this for logging or diagnostics.</remarks>
		public UnknownCharacterHandler UnknownCharacter
		{
			get { return _unkChar; }
			set { _unkChar = value; }
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
		/// for example \AR\. For American Morse Code <see cref="Mode" />, the internal space 
		/// is represented by the underscore '_', the long dah for the letter 'L' is represented
		/// by the equal sign '=', and the longer dah for the number zero '0' is represented
		/// by the sharp dign '#'.
		/// </remarks>
		/// <returns>The 'dotscii' representation of the resulting Morse Code.</returns>
		/// <exception cref="System.ApplicationException">Thrown when the <b>Text</b> parameter is null or an empty string</exception>
		/// <example>
		///		This example shows how to call the DotDash() method (International <see cref="Mode" />):
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

			Dictionary<char, string> dict = (_mode == CodeMode.International ? _iMorse : _aMorse);
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
				if (dict.ContainsKey(c))
				{
					buf += dict[c];
				}
				else
				{
					buf += "[" + c + "]";									// Print unknown character
					if (_unkChar != null) _unkChar(c);
				}
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
		///		This example shows how to call the MorseMail() method (International <see cref="Mode" />):
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
			Dictionary<char, string> dict = (_mode == CodeMode.International ? _iMorse : _aMorse);

			foreach (char c in Text.ToUpper())
			{
				char c2 = c;												// Allow modifying char
				string dotscii;

				if (c2 == '\\')												// Prosign delimiter
				{
					if (_mode == CodeMode.International)					// True prosign processing
					{
						inProsign = !inProsign;
						if (!inProsign) space(_cstime);						// End prosign with char space
						continue;
					}
					else
						c2 = '/';											// American, put out /xx/ with normal spacing
				}

				if (dict.ContainsKey(c2))
				{
					dotscii = dict[c2];
				}
				else
				{
					if (_unkChar != null)
					{
						dotscii = " ";										// Caller knows so don't send error code
						_unkChar(c2);
					}
					else
						dotscii = _errCode;
				}

				for (int i = 0; i < dotscii.Length; i++)
				{
					char s = dotscii[i];
					char s1 = (i == dotscii.Length - 1 ? '|' : dotscii[i + 1]);
					switch (s)
					{
						case '.':											// Dit
							mm += mark_mm(_ctime);
							space(_ctime);
							break;
						case '-':											// International: Dah = 3 dits
							if (_mode == CodeMode.International)
							{
								mm += mark_mm(_ctime * 3);
								space(_ctime);
							}
							else											// American is more complicated
							{
								mm += mark_mm((int)(s1 == '-' ? 2.7 * _ctime : 3.3 * _ctime));
								space((int)(s1 == '-' ? 1.6 * _ctime : _ctime));
							}
							break;
//						---- American Only ----
						case '_':											// Embedded space (American only)
							mm += ((int)(-2.7 * _ctime)).ToString("+#;-#;#");
							_accSpace = 0;
							break;
						case '=':											// Four unit mark (American 'L')
							mm += mark_mm((int)(5.6 * _ctime));
							space(_ctime);
							break;
						case '#':											// Five unit mark (American zero)
							mm += mark_mm((int)(8.9 * _ctime));
							space(_ctime);
							break;
//						------------------------
						case ' ':											// Word break
							space(_wstime - _cstime);						// Preceding char has trailing character space
							break;
					}
				}
				if (!inProsign)												// Unless doing prosign
				{
					space(_cstime);											// Character space
					if (_mode == CodeMode.American && (dotscii == _errCode || _aSpaceEmph[c2]))
						space(_ctime / 2);
				}
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
		///		This example shows how to call the CwCom() method (International <see cref="Mode" />):
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

			Dictionary<char, string> dict = (_mode == CodeMode.International ? _iMorse : _aMorse);
			string prosign = "";
			bool inProsign = false;
			bool sendProsign = false;

			foreach (char c in Text.ToUpper())
			{
				string dotscii;
				char c2 = c;												// Allow modifying char

				if (!inProsign)
					start_cw();												// Not in prosign, start fresh
				else if (c2 != '\\')										// In prosign (and not ending delim)
					prosign += c2;											// Accumulate it for CwCode text

				if (c2 == '\\')												// Prosign delimiter
				{
					if (_mode == CodeMode.International)					// True prosign processing
					{
						inProsign = !inProsign;
						if (inProsign)										// Starting prosign
							continue;
						else												// Ending prosign
						{
							c2 = ' ';
							sendProsign = true;
						}
					}
					else
						c2 = '/';											// American, just change \ to /
				}

				if (dict.ContainsKey(c2))
				{
					dotscii = dict[c2];
				}
				else
				{
					if (_unkChar != null)
					{
						dotscii = " ";										// Caller knows so don't send error code
						_unkChar(c2);
					}
					else
						dotscii = _errCode;
				}

				for (int i = 0; i < dotscii.Length; i++)
				{
					char s = dotscii[i];
					char s1 = (i == dotscii.Length - 1 ? '|' : dotscii[i + 1]);
					switch (s)
					{
						case '.':											// Dit
							mark_cw(_ctime);
							space(_ctime);
							break;
						case '-':											// Dah = 3 dits
							if (_mode == CodeMode.International)
							{
								mark_cw(_ctime * 3);
								space(_ctime);
							}
							else											// American is more complicated
							{
								mark_cw((int)(s1 == '-' ? 2.7 * _ctime : 3.3 * _ctime));
								space((int)(s1 == '-' ? 1.6 * _ctime : _ctime));
							}
							break;
//						---- American Only ----
						case '_':											// Embedded space (American only)
							_cwCode[_cwCount++] = (int)(-2.7 * _ctime);
							_accSpace = 0;
							break;
						case '=':											// Four unit mark (American 'L')
							mark_cw((int)(5.6 * _ctime));
							space(_ctime);
							break;
						case '#':											// Five unit mark (American zero)
							mark_cw((int)(8.9 * _ctime));
							space(_ctime);
							break;
//						-----------------------
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
					Sender(code, _accWhite + (sendProsign ? prosign + "\r\n" : c2.ToString()));  // Call delegate for this character
					_accWhite = "";
					sendProsign = false;
					prosign = "";
				}
				if (!inProsign || _mode == CodeMode.American)				// Unless doing prosign in International mode
				{
					space(_cstime);											// Character space
					if (_mode == CodeMode.American && (dotscii == _errCode || _aSpaceEmph[c2]))
						space(_ctime / 2);
				}

			}
		}

	}
}
