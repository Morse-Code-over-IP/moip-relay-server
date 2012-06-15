Morse Code Tools V2.6 (14-Jun-2012)
=====================

The News application and the Keyer application have built-in help. Go there for usage and background info.

CHANGES AND ADDITIONS:

Version 2.6:
-----------

This is a fairly major update and includes numerous tweaks and fixes in Morse News for Facebook and Twitter. 

- More realistic noise, static crashes, and ionospheric fading to Morse News CW tone and spark-gap sound. 

- Removes ellipses (...) from news stories, and removes parens from (REUTERS) as it did before for (AP).

- NOTE: TWITTER URI'S HAVE CHANGED! THEY NOW HAVE THE USERNAME OR "" IN THE FIRST PART. See the help!

- The above is a consequence of new support for specifying the Twitter user/screen name for the feeds to be retrieved. This allows switching Morse News between Twitter feeds. You cannot mix Twitter feeds freom multiple accounts in a feed list though. Maybe some day, ha ha.

- A great deal of additional Debug tracing has been added, revealiong a number of bugs including time-stamping of articles. 

- Debug tracing and DebugView have been documented. Handy even for end users.

- Support for twitter:///messages has been removed. Twitter's permissions model prohibits apps from accessing direct messages unless they also have write permission. I did not want to give Morse News permission to write to peoples' Twitter timelines.

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
