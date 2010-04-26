This is an RSS info robot for Morse Code using the CwCom/MorseKOB protocol. It can feed Morse-Over-IP to wither program (with the appropriate selection of International or American code for CwCom or MorseKOB respectively).

The robot looks for a file called BotList.txt in the same directory the executable is in. The format of the file is one bot per line, | delimited. The fields are

bot-name|relay-server-ip/name|ionosphere-server-port|channel|char-wpm|word-wpm|code-mode|station-id

The bot-name is what shows up in the station list. The relay server for CwCOm is usually morsecode.dyndns.org, port 7890, and for MorseKOB it's mtc-kob.dyndns.org, port 7890. The code-mode is either "International" for CwCom or "American" for MorseKOB. The station-id is required only for American mode/MorseKOB, and is a 2-letter "sine" for the station. If you don't know what this is get up to speed on MorseKOB. Look in the folder sample-config for samples.

The robot also looks for an RSS-feed list and a periodic message file for EACH BOT LISTED IN BOTLIST.TXT. 

The rss-feed file has a name of bot-name-RssFeeds.txt. It is | delimited with fields

feed-friendly-name|feed-xml-url|title-age|message-class

The feed-friendly-name is what is shown as the station name and in the transmitted info preamble. The feed-xml-url is a URL to an XML/RSS feed. title-age is the number of minutes after which an RSS item which has been sent to when it again becomes eligible for sending. The message-class is used only in AMerican/MorseKOB mode, and is a telegram-style code for the tariff (e.g. "DPR" for Day Press Rate). If it is omitted, no message class will be sent.

The periodic message file is used to get the text for the bot-generated special messages that are sent out every 30 minutes. The format is one message per line in final "telegram" format with prosigns delimited by \\. If this file is empty, no periodic messsages will bs sent.

See the sample-config folder for a set of config files. I think you'll be able to figure out how all of this works once you look at them.

FEED SELECTION:

I have found after a lot of trial and error that the Yahoo! news feeds are by far the best for the "telegram" format that this robot produces. 
