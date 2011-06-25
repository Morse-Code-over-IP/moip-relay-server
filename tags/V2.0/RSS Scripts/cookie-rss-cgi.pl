#!/local/perl/bin/perl -w
# =============================================================================
#
#   Generate fortune cookie messages for Darrick Brown's LED Sign Java
#	applet. Use this as the Script parameter in the <applet> stuff
#	to have fortune cookie(s) show on the LED Sign display. LEDSign 2.5b
#	supports the Chain command in LED stripts, to this can be chained
#	into a more complex sequence of LED Sign stuff.
#
#	Robert B. Denny 15-Feb-96.	Taken from the code by Kevin O. Grover.
#	Robert B. Denny	06-Apr-02	Wow, back in business! New path for XP system
#	Robert B. Denny 08-Apr-02	Supress perl warning use of <QTS> w/o defined()
#								Fix concatenation of script cmds by not printing
#								blank lines. Long standing bug.
# =============================================================================

$fortfileqts = $ENV{"FORTUNE"} || "c:/cookie/cookies-rss.txt";       # Quotes
$fortfileidx = $ENV{"FORTIDX"} || "c:/cookie/cookies-rss.idx";       # Index into quotes
#$fortfileqts = $ENV{"FORTUNE"} || "d:/dev/misc/cookie/cookies.txt";  # Quotes
#$fortfileidx = $ENV{"FORTIDX"} || "d:/dev/misc/cookie/cookies.idx";  # Index into quotes

#
# Send the HTML header
#
print "Content-type: text/xml\n\n";

$maxlen = 250;
$count = 3;

#
# Open the index file, read the total fortune count. Then open the
# fortune data file and get the offset to EOF for later length
# computations.
#
open (IDX,"<$fortfileidx") || die "Can't open index, $fortfileidx: $!\n";
binmode IDX;								# This file has binary data
$tmp = 0;									# Create a buffer
sysread(IDX, $tmp, 4);						# Read the binary fortune count
($numfortunes) = unpack("L", $tmp);			# Convert to perl integer

open(QTS, "<$fortfileqts")  || die "Can open quotes, $fortfileqts: $!\n";
seek(QTS, 0, 2);							# Seek to end
$endpos = tell(QTS);						# Remember offset to last char

#
# The original srand(time) changes way too slowly for this use
# Since we are running as a CGI program, use XOR of the the
# IP address of the client and the time as the srand() seed.
# That ought to do it! 
#
# OOPS, split() fails on localhost.
#
#srand(time ^ unpack("L", pack("CCCC", split(/./, $ENV{"REMOTE_ADDRESS"}))));
$tmp=rand(1);								# Use one number, toss
	
#
# Send $count fortunes in RSS
#
print "<rss version=\"2.0\">\n<channel>\n";
for($i = 0; $i < $count; $i++) {
    print "<item>\n<title>cookie</title>\n<description>\n";
    &sendfortune();
    print "</description>\n</item>\n";
}
print "</channel>\n</rss>\n";


close(IDX);									# Close index file
close(QTS);									# Close cookie file

# -----------------------------------------------------------------------------
# sendfortune() - Send a fortune, given $maxlen max length in chars
#
# Uses globals $numfortunes, $maxlen, $endpos
# -----------------------------------------------------------------------------
sub sendfortune {
	local($num, $pos, $nextpos);

	while() {	
		$num = int(rand($numfortunes));		# Get random fortune index
		if($num >= ($numfortunes - 1)) {	# If $num = last fortune
			$nextpos = $endpos + 1;			# "next" is endpos+1
		} else {							# $num not last fortune
			seek (IDX, ($num + 1) * 4, 0);	# Seek to $num+1 fortune index
			sysread (IDX, $tmp, 4);			# Read its byte offset
			$nextpos = unpack ("L", $tmp);	# Convert to integer
		}
	
		if ($num ==  0) {					# If requested first fortune
			$pos = 0;						# Shortcut, know offset = 0
		} else {							# Otherwise...
			seek (IDX, $num*4, 0);			# Seek to $num fortune index
	 		sysread (IDX, $tmp, 4);			# Read its byte offset
	 		$pos = unpack ("L", $tmp);		# Convert to integer
		}
		redo if (($nextpos - $pos) > $maxlen); # LOOPBACK: Too long test
		
		seek (QTS, $pos, 0);				# Seek to 1st line of this cookie
		while (defined(QTS) && ($_ = <QTS>) && !m/^%/o) {
			s/^\s+//;						# Remove leading whitespace
			if ($_) {
	 			print $_;
			}
		}
		last;								# EXITLOOP
	}
}
