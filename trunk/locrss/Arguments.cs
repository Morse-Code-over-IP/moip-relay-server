//
// Arguments class: application arguments interpreter
//
// Based on: http://www.codeproject.com/KB/recipes/command_line.aspx
//
// Valid parameters forms:  {-,/,--}param{ ,=,:}((",')value(",'))
// Examples: -param1 value1 --param2 /param3:"Test-:-work" /param4=happy -param5 '--=nice=--'
//
// 04-Nov-09 rbd	Heavily modified and enhanced. There were several
//					holes and it couldn't deal with naked args or bool
//					options preceding naked args.
//

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DC3.Utility
{

	public class Arguments
	{
		const string sOptErr = "Argument processing error: unexpected single token";
		private List<string> smplOpts;
		private Dictionary<string, string> optDict;
		private List<string> plainArgs;

		public Arguments(string[] SimpleOpts)
		{
			smplOpts = new List<string>();
			for (int i = 0; i < SimpleOpts.Length; i++)
				smplOpts.Add(SimpleOpts[i]);
		}

		public void Parse(string[] Args)
		{
			//
			// Note that the Splitter pattern will happily split a quoted argument
			// which contains an option signal or delimiter. Thus we first use Tester
			// to detect tokens which do NOT start with an option signal.
			//
			Regex Tester = new Regex(@"^-{1,2}|^/", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Regex Splitter = new Regex(@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Regex Remover= new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			string opt = null;

			optDict = new Dictionary<string, string>();
			plainArgs = new List<string>();

			foreach (string arg in Args)
			{
				//
				// Trick alert: Look for tokens which don't start with an option
				// signal. But don't look at one that is an option value! To tell
				// the latter, look at param - if it's non-null then this token
				// is an option value that happens to start with one of the
				// option signals.
				//
				if (!Tester.IsMatch(arg))
				{
					if (opt == null)											// Naked argument?
					{
						plainArgs.Add(arg);										// Not an option
					}
					else														// Found a value (for the just-found option, space delimited)
					{
						if (!optDict.ContainsKey(opt))							// Use only 1st occurrence of a given option (typ.)
						{
							optDict.Add(opt, Remover.Replace(arg, "$1"));		// Remove possible enclosing/quoting characters (",')
						}
						opt = null;
					}
					continue;
				}
				else
				{
					//
					// Look for new parameters (-,/ or --) and a possible enclosed value (=,:)
					// Note that the Splitter Regex turns the option signal into a blank
					// first element in the bits[] array, so there should never be just
					// one element in that array!
					//
					string[] bits = Splitter.Split(arg, 3);
					switch (bits.Length)
					{
						case 1:
							throw new ApplicationException(sOptErr);
						//
						// Found just an option
						//
						case 2:
							if (opt != null && !optDict.ContainsKey(opt))
								throw new ApplicationException(sOptErr);

							if (smplOpts.Contains(bits[1]) && !optDict.ContainsKey(bits[1]))	// If a simple option
								optDict.Add(bits[1], "true");					// Add it now
							else
								opt = bits[1];									// A "waiting" option
							break;
						//
						// Option with ':' or '=' delimited value
						//
						case 3:
							if (opt != null && !optDict.ContainsKey(opt))
								throw new ApplicationException(sOptErr);

							// Process this option and its value in one stroke
							if (!optDict.ContainsKey(bits[1]))
							{
								bits[2] = Remover.Replace(bits[2], "$1");		// Remove possible enclosing/quoting characters (",')
								optDict.Add(bits[1], bits[2]);
							}
							break;
					}
				}
			}
			if (opt != null && !optDict.ContainsKey(opt))
				throw new ApplicationException(sOptErr);
		}

		// ========================
		// Direct Access Properties
		// ========================

		public Dictionary<string, string> Options
		{
			get { return optDict; }
		}

		public List<string> PlainArgs
		{
			get { return plainArgs; }
		}

		// ========================
		// Array Indexer Properties
		// ========================

		//
		// Return a parameter value if it exists, else return null
		//
		public string this [string Param]
		{
			get
			{
				if (optDict.ContainsKey(Param))
					return optDict[Param];
				else
					return null;
			}
		}

		//
		// Return a naked argument by positional index
		//
		public string this [int index]
		{
			get
			{
				if (index < plainArgs.Count)
					return plainArgs[index];
				else
					return null;
			}
		}
	}
}
