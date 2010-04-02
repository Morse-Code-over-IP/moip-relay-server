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
			Assert.AreEqual("..--.. .-.- -..-. ---. ._...", M.DotDash(".,?!&"));
		}

		[Test, Category("Pre-Requisite"), Description("Tests the WPM properties")]
		public void Test_Wpm()
		{
			Morse M = new Morse();
			Assert.AreEqual(1200, M.TimeBase);									// Default value
			M.TimeBase = 1234;
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
			M.TimeBase = 1200;													// MorseMail's time base
			M.CharacterWpm = 15;
			M.WordWpm = 15;
			Assert.AreEqual("+80-80+80-80+80-80+80-240+80-240+80-80+240-80+80-80+80-240+80-80+240-80+80-80+80-240+240-80+240-80+240",
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

			// TODO - Fails miserably, way off. These timings came from MorseKOB.

			//string amTest = "THE QUICK BROWN FOX JUMPED OVER THE LAZY DOGS BACK 0123456789";
			//string amMm = "-565+266-301+80-80+80-80+80-80+80-299+82-565+80-80+80-84+264-84+80-297+82-82+80-80+265-299+82-80+80" + 
			//    "-299+82-80+80-217+82-297+268-82+80-80+268-566+264-84+80-80+80-82+80-297+82-217+82-82+80-299+82-216+82" +
			//    "-299+80-80+219-131+264-299+269-80+80-569+78-80+265-82+81-298+82-217+82-297+84-80+266-82+80-82+80-564+264-82" +
			//    "+80-82+266-80+80-297+82-80+80-80+266-297+218-129+266-297+80-82+80-80+80-80+80-82+80-297+80-299+264-82+80-80+80" +
			//    "-563+80-217+82-298+80-83+80-80+80-82+263-297+80-297+82-217+82-80+80-565+263-297+82-80+80-81+82-80+80-297" +
			//    "+80-564+447-338+80-80+266-297+80-80+80-82+80-217+82-297+80-82+78-219+80-80+80-565+263-82+80-81+80-296+81-216" +
			//    "+82-297+217-131+264-80+80-297+82-80+80-80+80-566+264-80+80-80+80-80+81-296+82-81+265-297+80-82+80-217+82-295" + 
			//    "+266-82+80-80+265-563+713-338+80-80+217-129+265-80+80-336+82-80+80-81+265-80+80-80+80-338+80-82+81-80+80-80" +
			//    "+263-81+80-337+81-82+80-80+80-80+80-80+266-336+218-129+217-129+266-337+81-82+80-80+80-80+80-80+80-80+80-336" +
			//    "+219-131+264-80+80-80+80-338+264-82+80-81+80-80+80-80+80-338+265-81+80-82+78-80+265";
			//M.Mode = Morse.CodeMode.American;
			//Assert.AreEqual(amMm, M.MorseMail(amTest));


		}

		//
		// The stuff below is for the CWCom tests
		//
		private int[] corrCount = { 1, 2, 6, 2, 10, 10, 10, 2, 4, 6, 10 };
		private string[] corrText = { "T", "E", "S", "T", " 1", " 2", " 3", " E", "N", "D", " AR\r\n" };
		private Int32[][] corrCode = new Int32[11][]
			{
				new Int32[1]  { +240 },											// T
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
		private int index;

		public void CwComSend(Int32[] code, string text)
		{
			//Console.WriteLine("send '" + ch + "' idx=" + index + "cnt=" + count);
			Assert.AreEqual(corrCount[index], code.Length);
			Assert.AreEqual(corrText[index], text);
			for (int i = 0; i < code.Length; i++)
				Assert.AreEqual(corrCode[index][i], code[i]);
			index += 1;
		}

		[Test, Category("Final"), Description("Verifies the CWCom encoding. Must pass Test_MorseMail first.")]
		public void Test_CwCom()
		{
			Morse M = new Morse();
			M.TimeBase = 1200;															// MorseMail's time base
			M.CharacterWpm = 15;
			M.WordWpm = 15;

			index = 0;
			M.CwCom("test 1 2 3\nend\\AR\\", CwComSend);

			index = 0;
			Assert.Throws<ArgumentNullException>(delegate { M.CwCom("", CwComSend); });
			Assert.Throws<ArgumentNullException>(delegate { M.CwCom(null, CwComSend); });
		}
	}
}
