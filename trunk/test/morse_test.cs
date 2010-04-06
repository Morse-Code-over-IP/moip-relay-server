//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		morse_test.cs
//
// FACILITY:	Morse Code class
//
// ABSTRACT:	Contains unit tests for the Morse class.
//
// ENVIRONMENT:	Microsoft.NET 2.0/3.5
//				Developed under Visual Studio.NET 2008
//				Requires the NUnit unit testing package for Visual Studio
//				http://www.nunit.org/   (used version 2.5.3)
//
// AUTHOR:		Bob Denny, <rdenny@dc3.com>
//
// Edit Log:
//
// When			Who		What
//----------	---		-------------------------------------------------------
// Jan-Feb 10	rbd		Initial edits
// 04-Feb-10	rbd		0.5.1
// 25-Mar-10	rbd		0.6.8 - Error is 8 dots not 6
// 01-Apr-10	rbd		0.7.1 - Tests for American Morse Code
// 05-Apr-10	rbd		0.7.2 - Additional punctuation for American Morse
//-----------------------------------------------------------------------------
//

using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;

using com.dc3.morse;

namespace test.morse
{
	[TestFixture]
	public class morse_test
	{
		[Test, Category("Pre-Requisite"), Description("Verifies the Morse code itself as well as 'dotscii' output")]
		public void Test_DotDash()
		{
			Morse M = new Morse();
			Assert.AreEqual("- .... .. ...  .. ...  .-  - . ... - .-.-.-", M.DotDash("This is a test."));
			Assert.AreEqual(".- -... -.-. -.. . ..-. --. .... .. .--- -.- .-.. --", M.DotDash("ABCDEFGHIJKLM"));
			Assert.AreEqual("-. --- .--. --.- .-. ... - ..- ...- .-- -..- -.-- --..", M.DotDash("NOPQRSTUVWXYZ"));
			Assert.AreEqual("----- .---- ..--- ...-- ....- ..... -.... --... ---.. ----.", M.DotDash("0123456789"));
			Assert.AreEqual(".-.-.- --..-- ---... -.-.-. ..--.. -....- .-.-. -..-. -...-", M.DotDash(".,:;?-+/="));
			Assert.AreEqual(".--.-. -.-.-- .----. -.--. -.--.- ...-..- .-... .-..-. ..--.-", M.DotDash("@!'()$&\"_"));
			Assert.AreEqual(".-  .-.-.  -... ...-.-", M.DotDash("A \\AR\\ B\\SK\\"));
			Assert.AreEqual("........", M.DotDash("~"));
			Assert.Throws<ArgumentNullException>(delegate { M.DotDash(null); });
			Assert.Throws<ArgumentNullException>(delegate { M.DotDash(""); });
			M.Mode = Morse.CodeMode.American;
			Assert.AreEqual(".- -... .._. -.. . .-. --. .... .. -.-. -.- = --", M.DotDash("ABCDEFGHIJKLM"));
			Assert.AreEqual("-. ._. ..... ..-. ._.. ... - ..- ...- .-- .-.. .._.. ..._.", M.DotDash("NOPQRSTUVWXYZ"));
			Assert.AreEqual("# .--. ..-.. ...-. ....- --- ...... --.. -.... -..-", M.DotDash("0123456789"));
			Assert.AreEqual("..--.. .-.- -..-. ---. ._... ---- ..-- -.-.. ....-..", M.DotDash(".,?!&=/:$"));
			Assert.AreEqual(".....-. ......... ..-..-.. ..-.-. ..-.-.-. -....-.. -....-..", M.DotDash("()'“”[]"));
			Assert.AreEqual(".....-.. .......", M.DotDash("-%"));
		}

		[Test, Category("Pre-Requisite"), Description("Tests the WPM properties")]
		public void Test_Wpm()
		{
			Morse M = new Morse();
			Assert.AreEqual(1200, M.TimeBase);									// Default value
			M.TimeBase = 1234;													// MorseMail's time base for comparison
			Assert.AreEqual(1234, M.TimeBase);
			M.CharacterWpm = 20;
			Assert.AreEqual(20, M.CharacterWpm);
			Assert.Throws<ArgumentOutOfRangeException>(delegate { M.CharacterWpm = 4; });
			Assert.Throws<ArgumentOutOfRangeException>(delegate { M.CharacterWpm = 101; });
			M.WordWpm = 18;
			Assert.AreEqual(18, M.WordWpm);
			Assert.Throws<ArgumentOutOfRangeException>(delegate { M.WordWpm = 4; });
			Assert.Throws<ArgumentOutOfRangeException>(delegate { M.WordWpm = 101; });	// Test absolute limit (first test)
			Assert.Throws<ArgumentOutOfRangeException>(delegate { M.WordWpm = 21; });	// Test WordWpm > CharacterWpm
		}

		[Test, Category("Final"), Description("Verifies the MorseMail encoding and split rate (Farnsworth) timing. Must pass Test_DotDash and Test_Wpm first.")]
		public void Test_MorseMail()
		{
			Morse M = new Morse();
			M.CharacterWpm = 15;
			Assert.AreEqual("-1000+80-80+80-80+80-80+80-240+80-240+80-80+240-80+80-80+80-240+80-80+240-80+80-80+80-240+240-80+240-80+240",
							M.MorseMail("hello"));
			Assert.AreEqual("-240+240-240+80-240+80-80+80-80+80-240+240-560+80-80+240-80+240-80+240-80+240-560+80-80+80-80+240-80+240-80+240"
							+ "-560+80-80+80-80+80-80+240-80+240-560+80-80+240-80+80-80+240-80+80-560+80-240+240-80+80-240+240-80+80-80+80",
							M.MorseMail("test 1 2 3\n\\AR\\ end"));
			Console.WriteLine("Paste the following into MorseMail. Should send \"Testing MorseMail.\"");
			Console.WriteLine("<MorseMail>morse_test\r\n" + M.MorseMail("Testing MorseMail.") + "\r\n</MorseMail>");
			Assert.Throws<ArgumentNullException>(delegate { M.MorseMail(""); });
			Assert.Throws<ArgumentNullException>(delegate { M.MorseMail(null); });

			M.CharacterWpm = 15;
			M.WordWpm = 5;
			Assert.AreEqual("-240+80-80+240-1382+240-80+80-80+80-80+80-3986+80-1382+80-80+80-80+240-80+80", M.MorseMail("ab ef"));

			//
			// These timings came from MorseKOB, but had to be slightly edited (in the last digit)
			// due to (apparently) randome rounding/truncation in MorseKOB. For example the values
			// of 80 were frequently 81, 82, etc.
			//
			string amTest = "THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS BACK 0123456789";
			string amMm = "-1000+264-300+80-80+80-80+80-80+80-300+80-560+80-80+80-80+264-80+80-300+80-80+80-80+264-300+80-80+80" +
				"-300+80-80+80-216+80-300+264-80+80-80+264-560+264-80+80-80+80-80+80-300+80-216+80-80+80-300+80-216+80" +
				"-300+80-80+216-128+264-300+264-80+80-560+80-80+264-80+80-300+80-216+80-300+80-80+264-80+80-80+80-560+264-80" +
				"+80-80+264-80+80-300+80-80+80-80+264-300+216-128+264-300+80-80+80-80+80-80+80-80+80-300+80-300+264-80+80-80+80" +
				"-560+80-216+80-300+80-80+80-80+80-80+264-300+80-300+80-216+80-80+80-560+264-300+80-80+80-80+80-80+80-300" +
				"+80-560+448-340+80-80+264-300+80-80+80-80+80-216+80-300+80-80+80-216+80-80+80-560+264-80+80-80+80-300+80-216" +
				"+80-300+216-128+264-80+80-300+80-80+80-80+80-560+264-80+80-80+80-80+80-300+80-80+264-300+80-80+80-216+80-300" +
				"+264-80+80-80+264-560+712-340+80-80+216-128+264-80+80-340+80-80+80-80+264-80+80-80+80-340+80-80+80-80+80-80" +
				"+264-80+80-340+80-80+80-80+80-80+80-80+264-340+216-128+216-128+264-340+80-80+80-80+80-80+80-80+80-80+80-340" +
				"+216-128+264-80+80-80+80-340+264-80+80-80+80-80+80-80+80-340+264-80+80-80+80-80+264";
			M = new Morse();
			M.Mode = Morse.CodeMode.American;
			M.CharacterWpm = 15;
			Assert.AreEqual(amMm, M.MorseMail(amTest));


		}

		//
		// The stuff below is for the CWCom tests
		//
		private int index;

		private string[] corrTextIntl = { "T", "E", "S", "T", " 1", " 2", " 3", " E", "N", "D", " AR\r\n" };
		private int[] corrCountIntl = { 2, 2, 6, 2, 10, 10, 10, 2, 4, 6, 10 };
		private Int32[][] corrCodeIntl = new Int32[11][]
			{
				new Int32[2]  { -1000,+240 },									// T
				new Int32[2]  { -240,+80 },										// E
				new Int32[6]  { -240,+80,-80,+80,-80,+80 },						// S
				new Int32[2]  { -240,+240 },									// T
				new Int32[10] { -560,+80,-80,+240,-80,+240,-80,+240,-80,+240 },	//  1
				new Int32[10] { -560,+80,-80,+80,-80,+240,-80,+240,-80,+240 },	//  2
				new Int32[10] { -560,+80,-80,+80,-80,+80,-80,+240,-80,+240 },	//  3
				new Int32[2]  { -560,+80 },										// <LF>E
				new Int32[4]  { -240,+240,-80,+80 },							// N
				new Int32[6]  { -240,+240,-80,+80,-80,+80 },					// D
				new Int32[10] { -240,+80,-80,+240,-80,+80,-80,+240,-80,+80 }	// \AR\
			};

		private string[] corrTextAmer = { "T", "E", "S", "T", " 1", " 2", " 3", " E", "N", "D", "/", "A", "R", "/" };
		private int[] corrCountAmer = { 2, 2, 6, 2, 8, 10, 10, 2, 4, 6, 8, 4, 6, 8 };
		private Int32[][] corrCodeAmer = new Int32[14][]
			{
				new Int32[2]  { -1000,+264 },									// T
				new Int32[2]  { -300,+80 },										// E
				new Int32[6]  { -300,+80,-80,+80,-80,+80 },						// S
				new Int32[2]  { -300,+264 },									// T
				new Int32[8]  { -560,+80,-80,+216,-128,+264,-80,+80 },			//  1
				new Int32[10] { -600,+80,-80,+80,-80,+264,-80,+80,-80,+80 },	//  2
				new Int32[10] { -600,+80,-80,+80,-80,+80,-80,+264,-80,+80 },	//  3
				new Int32[2]  { -600,+80 },										// <LF>E
				new Int32[4]  { -300,+264,-80,+80 },							// N
				new Int32[6]  { -300,+264,-80,+80,-80,+80 },					// D
				new Int32[8]  { -300,+80,-80,+80,-80,+216,-128,+264 },			// /
				new Int32[4]  { -340,+80,-80,+264 },							// A
				new Int32[6]  { -300,+80,-216,+80,-80,+80 },					// R
				new Int32[8]  { -300,+80,-80,+80,-80,+216,-128,+264 }			// /
			};

		public void CwComSendIntl(Int32[] code, string text)
		{
			//Console.WriteLine("send '" + ch + "' idx=" + index + "cnt=" + count);
			Assert.AreEqual(corrCountIntl[index], code.Length);
			Assert.AreEqual(corrTextIntl[index], text);
			for (int i = 0; i < code.Length; i++)
				Assert.AreEqual(corrCodeIntl[index][i], code[i]);
			index += 1;
		}

		public void CwComSendAmer(Int32[] code, string text)
		{
			//Console.WriteLine("send '" + ch + "' idx=" + index + "cnt=" + count);
			Assert.AreEqual(corrCountAmer[index], code.Length);
			Assert.AreEqual(corrTextAmer[index], text);
			for (int i = 0; i < code.Length; i++)
				Assert.AreEqual(corrCodeAmer[index][i], code[i]);
			index += 1;
		}

		[Test, Category("Final"), Description("Verifies the CWCom encoding. Must pass Test_MorseMail first.")]
		public void Test_CwCom()
		{
			Morse M = new Morse();
			M.CharacterWpm = 15;

			index = 0;
			M.CwCom("test 1 2 3\nend\\AR\\", CwComSendIntl);

			index = 0;
			Assert.Throws<ArgumentNullException>(delegate { M.CwCom("", CwComSendIntl); });
			Assert.Throws<ArgumentNullException>(delegate { M.CwCom(null, CwComSendIntl); });

			M = new Morse();
			M.Mode = Morse.CodeMode.American;
			M.CharacterWpm = 15;

			index = 0;
			M.CwCom("test 1 2 3\nend\\AR\\", CwComSendAmer);
		}
	}
}
