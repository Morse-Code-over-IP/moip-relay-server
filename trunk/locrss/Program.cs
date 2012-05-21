//
// MUST BE BUILT FOR X86 FOR DIRECTX! 
//
// 20-May-12	rbd		2.5.0 SF #3527171 Command line options
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace com.dc3
{
	static class Program
	{
		public static bool s_bAutoStart = false;
		public static string s_sFeedUrl = "";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			DC3.Utility.Arguments arg = new DC3.Utility.Arguments(new string[] { "s", "start" });	// These are the bool/simple opts
			arg.Parse(args);
			if (arg["s"] != null || arg["start"] != null) s_bAutoStart = true;
			if (arg["f"] != null) s_sFeedUrl = arg["f"];
			if (arg["feedurl"] != null) s_sFeedUrl = arg["feedurl"];
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
