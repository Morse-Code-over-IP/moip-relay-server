//tabs=4
//-----------------------------------------------------------------------------
// TITLE:		PrecisionDelay.cs
//
// FACILITY:	RSS to Morse tool
//
// ABSTRACT:	Thread.Sleep is not accurate enough for timing of Morse Code.
//				This class uses the WIndows Multimedia Timer services to 
//				implement a precise delay without hogging CPU resources. The
//				wait is ended via setting an Auto-Reset event so spin-looping
//				is avoided.
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
// 03-May-10	rbd		1.3.2 - Initial edit.
// 07-May-10	rbd		1.5.0 - Refactoring into separate assy, make this class
//						public.
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;

namespace com.dc3
{

	public static class PreciseDelay
	{
#if !MONO_BUILD
		[StructLayout(LayoutKind.Sequential)]
		public struct TimeCaps
		{
			public UInt32 wPeriodMin;
			public UInt32 wPeriodMax;
		};

		[Flags]
		public enum fuEvent : uint
		{
			TIME_ONESHOT = 0,						//Event occurs once, after uDelay milliseconds. 
			TIME_PERIODIC = 1,
			TIME_CALLBACK_FUNCTION = 0x0000,		// callback is function
			//TIME_CALLBACK_EVENT_SET = 0x0010,		// callback is event - use SetEvent */
			//TIME_CALLBACK_EVENT_PULSE = 0x0020	// callback is event - use PulseEvent */
		}

		[DllImport("winmm.dll", SetLastError = true)]
		static extern UInt32 timeGetDevCaps(ref TimeCaps timeCaps, UInt32 sizeTimeCaps);
		[DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
		public static extern uint MM_BeginPeriod(uint uMilliseconds);
		[DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
		public static extern uint MM_EndPeriod(uint uMilliseconds);
		[DllImport("winmm.dll", SetLastError=true)]
		static extern UInt32 timeSetEvent( UInt32 msDelay, UInt32 msResolution,
					TimerCallback handler, UIntPtr userCtx, UInt32 eventType );
		[DllImport("winmm.dll", SetLastError=true)]
		static extern UInt32 timeKillEvent( UInt32 timerEventId );

		//Delegate definition for the API callback
		delegate void TimerCallback(uint uTimerID, uint uMsg, UIntPtr dwUser, UIntPtr dw1, UIntPtr dw2);


		private static object _lockObj = new object();
		private static TimeCaps _tc = new TimeCaps();
		private static UInt32 _timerRes;
		private static UInt32 _timerId = 0;										// [sentinel]
		private static AutoResetEvent _complEvt = new AutoResetEvent(false);

		public static void Initialize()
		{
			timeGetDevCaps(ref _tc, (uint)Marshal.SizeOf(_tc));
			_timerRes = Math.Min(Math.Max(_tc.wPeriodMin, 1), _tc.wPeriodMax);	// Want 1 ms resolution
			MM_BeginPeriod(_timerRes);
			_timerId = 0;
		}

		public static void Cleanup()
		{
			MM_EndPeriod(_timerRes);
		}

		private static void TimerCb(uint uTimerID, uint uMsg, UIntPtr dwUser, UIntPtr dw1, UIntPtr dw2)
		{
			lock (_lockObj)
			{
				_complEvt.Set();
			}
		}

		private static TimerCallback thisCB = TimerCb;

		public static void Wait(int ms)
		{
			if (ms <= 1) return;												// Don't bother at 1 ms or less

			lock (_lockObj)
			{
				if (_timerId != 0)
				{
					timeKillEvent(_timerId);
					_timerId = 0;
				}

				_timerId = timeSetEvent((uint)ms, _timerRes, thisCB, UIntPtr.Zero, 
						(uint)(fuEvent.TIME_CALLBACK_FUNCTION | fuEvent.TIME_ONESHOT));	
			}
			if (_timerId == 0)
				throw new ApplicationException("Failed to start timer");

			_complEvt.WaitOne(5000);
		}
#else
		public static void Initialize()
		{
			return;
		}
		
		public static void Cleanup()
		{
			return;
		}
		
		public static void Wait(int ms)
		{
			Thread.Sleep(ms);
		}
#endif
	}
}
