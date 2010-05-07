using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace com.dc3.morse
{
	class Paddle : IDisposable
	{
		
		private bool _ditDown;
		private bool _ditWas;
		private bool _dahDown;
		private bool _dahWas;
		private object _stateLock;
		private SendSymbol _sender;
		private AutoResetEvent _trigger;
		private Thread _iambicKeyer;

		public bool ModeB { get; set; }

		public enum PaddleEvent
		{
			DitPress,
			DitRelease,
			DahPress,
			DahRelease
		}

		public enum Symbol
		{
			Dit,
			Dah
		}

		public Paddle(SendSymbol Sender)
		{
			ModeB = true;
			_ditDown = _ditWas = _dahDown = _dahWas = false;
			_trigger = new AutoResetEvent(false);
			_stateLock = new object();
			_sender = Sender;
			_iambicKeyer = new Thread(new ThreadStart(IambicKeyer));
			_iambicKeyer.Start();
		}

		public void  Dispose()
		{
 			_iambicKeyer.Interrupt();
			_iambicKeyer.Join(1000);
		}

		public delegate void SendSymbol(Symbol S);

		public void FireEvent(PaddleEvent Evt)
		{
			//Debug.Print("-->" + Evt.ToString());
			lock (_stateLock)
			{
				switch (Evt)													// This used only for Mode B
				{
					case PaddleEvent.DitPress: _ditDown = _ditWas = true; break;
					case PaddleEvent.DitRelease: _ditDown = false; break;
					case PaddleEvent.DahPress: _dahDown = _dahWas = true; break;
					case PaddleEvent.DahRelease: _dahDown = false; break;
				}
			}
			_trigger.Set();
		}

		private void IambicKeyer()
		{
			bool alt = false;
			bool lastDit = true;

			while (true)
			{
				try { _trigger.WaitOne(); }
				catch (ThreadInterruptedException) { break; }					// Program exiting

				//Debug.Print("(outer-loop)");
				if (!_ditDown && !_ditWas && !_dahDown && !_dahWas)
				{
					//Debug.Print("False False False False");
					//Debug.Print("alt=" + alt.ToString() + " lastSent=" + (lastDit ? "Dit" : "Dah"));
					if (alt && ModeB)
					{
						//Debug.Print(lastDit ? " *dah!" : " *dit!");
						_sender(lastDit ? Symbol.Dah : Symbol.Dit);
					}
					//Debug.Print("");
					alt = false;
				}
				while (_ditDown || _ditWas || _dahDown || _dahWas)
				{
					//Debug.Print("1 " +_ditDown.ToString() + " " + _ditWas.ToString() + " " + _dahDown.ToString() + " " + _dahWas.ToString());
					alt = false; // (_ditDown && _dahDown);
					if (_ditDown || _ditWas)
					{
						//Debug.Print("  dit!");
						_sender(Symbol.Dit);
						lock (_stateLock) { _ditWas = false; }
						lastDit = true;
					}
					//Debug.Print("2 " + _ditDown.ToString() + " " + _ditWas.ToString() + " " + _dahDown.ToString() + " " + _dahWas.ToString());
					alt |= (_ditDown && _dahDown);
					if (_dahDown || _dahWas)
					{
						//Debug.Print("  dah!");
						_sender(Symbol.Dah);
						lock (_stateLock) { _dahWas = false; }
						lastDit = false;
					}
					//Debug.Print("3 " + _ditDown.ToString() + " " + _ditWas.ToString() + " " + _dahDown.ToString() + " " + _dahWas.ToString());
					alt |= (_ditDown && _dahDown);
				}

			}
		}
	}
}
