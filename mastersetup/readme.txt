Morse Code Tools V3.0 (15-Jun-2012)
=====================

The News and Keyer applications have built-in help (Help me! links). Go there for usage and background info.

CHANGES AND ADDITIONS:

Version 3.0:
-----------

This is a fairly major update and includes new features in both programs, plus numerous tweaks and fixes in Morse News for Facebook and Twitter, and revised documentation. A Yahoo! group has been opened for use as a discussion group at http://groups.yahoo.com/group/MorseTools/

- More realistic noise, static crashes, and ionospheric fading to Morse News CW tone and spark-gap sound. 

- Removes ellipses (...) from news stories, and removes parens from (REUTERS) as it did before for (AP).

- NOTE: TWITTER URI'S HAVE CHANGED! THEY NOW HAVE THE USERNAME OR "" IN THE FIRST PART. See the help!

- The above is a consequence of new support for specifying the Twitter user/screen name for the feeds to be retrieved. This allows switching Morse News between Twitter feeds. You cannot mix Twitter feeds freom multiple accounts in a feed list though. Maybe some day, ha ha.

- RSS requests now include a real HTTP User-Agent: header (mimics Google Chrome). At some point in the past, this became required to successfully retrieve the Facebook notifications RSS feed. 

- A great deal of additional debug tracing has been added to Morse News, revealing a number of bugs including time-stamping of articles. If you're curious about the internals of Morse News, read about how to read this info in the Help me! doc.

- The Morse News documentation has been expanded in some areas that were a bit thin.

- Support for Twitter "direct messages" has been removed. As of last year, Twitter's permissions model prohibits Twitter apps from accessing direct messages unless they also have write permission. I did not want to give Morse News permission to write to peoples' Twitter timelines.

Version 2.5:
-----------

This update brings low-latency ASIO audio to the keyer, some command line options to the news reader, and a couple of bug fixes. See the docs for details.

Version 2.4
-----------

This update brings Atom feed capability to Morse News, allowing you to take feeds from the Associated Press and other sources that were previously unavailable since they were not RSS.

- Both RSS and Atom feeds are now supported. This makes it possible to use feeds from the Associated Press and other Atom-only sources.

- Both Morse News and Keyer allow selecting the sound output device ("sound card") to use, if desired.

- The keyer now has control of the tone envelope rise/fall times.

- The shapes of the tone envelope leading and trailing edges are now raised-cosine instead of straight ramp. This provides a more spectrally pure (free of harmonics) tone output.

- The keyer's hand-key (manual) sending now has a shaped envelope for key-up.

- Morse News now requires DirectX for sound. The old legacy support has been removed.

Version 2.3
-----------

This is a quick update after 2.2. The shapes of the tone envelope leading and trailing edges are now raised-cosine instead of straight ramp.

Version 2.2
-----------

This update brings you the following bug fixes and improvements to Morse News and Morse Keyer:

- Both Morse News and Keyer allow selecting the sound output device ("sound card") to use, if desired.

- Morse News now requires DirectX for sound. The old legacy support has been removed.

- The keyer's hand-key (manual) sending now has a shaped envelope for key-up.

_ The keyer now has control of the tone envelope rise/fall times.
