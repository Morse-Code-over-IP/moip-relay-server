#! /usr/bin/perl -w

#******************************************************************************
# COPYRIGHT:  MRX Software Pty Ltd 2001-2010
#
# Author: Andrew Spurrier
#
# MODULE: ionosphere.pl
#
# BRIEF: Morse World's Ionosphere.  Facilitates the broadcasting of 
#          transmissions to other stations on the same frequency.
# LANGUAGE: Perl version 5.004_04
# HISTORY:
# VER INIT DATE         COMMENT
# ------------------------------------------------------------------------------
# 1.00 AWS 17/05/00 Initial version.
#                   Message formats:
#                     1) - TYPE       unsigned short (little-endian)
#
#                     2) - TYPE       unsigned short (little-endian)
#                          FREQUENCY  unsigned short (little-endian)
#
#                     3) - TYPE       unsigned short (little-endian)
#                          LENGTH     unsigned short (little-endian)
#                          DATA[*]    BYTE, unsigned char buffer
#
#*******************************************************************************


require 5.002;
use strict;
use integer;
use Socket;
use Sys::Hostname;
use File::Basename;


# ***** CONSTANTS     so to speak! *********************************************
#my $BUILD_DATE      = 980000000;    # Time in seconds.
#my $BUILD_DATE      = 980000000;    # Time in seconds.
#my $BUILD_DATE      = 1201601092;
#my $BUILD_DATE      = 1500000000;

my $VERSION         = "1.0";
my $IONOSPHERE_PORT = 7890;        # UDP port number the Ionosphere listens on.
my $EXPIRE_DATE     = $BUILD_DATE + (3600 * 24 * 7 * 52);   # 16 week trial.
#my $MAX_STATIONS    = 10;     # Max numr of stations allowed to connect at once.
my $MAX_STATIONS    = 3;     # Max numr of stations allowed to connect at once.

my $MSG_SHUTDOWN    = 1;   # Message Format 1.
my $MSG_LOGOFF      = 2;   # Message Format 1.
my $MSG_BROADCAST   = 3;   # Message Format 3.
my $MSG_CHANGE_FREQ = 4;   # Message Format 2.
my $MSG_ACK_STATION = 5;   # Message Format 1.
my $MSG_NAK_STATION = 6;   # Message Format 3.

my $HB_TIME_OUT     = 300;   # In seconds.  Small value for testing purposes.
my $MAX_BCAST_LEN   = 1000;   # Max bytes in the payload of a broadcast packet.
my $DEBUG_B         = 0;     # Boolean signalling Debug status.

# Error Messages.
my $ERR_MAX_STATIONS = "Ionosphere has reached its limit of $MAX_STATIONS stations.";

# The file this programme executes from.
my ($PROG, $PROG_DIR) = fileparse($0);

$PROG =~ s/.pl$/.exe/;

# ***** PRIVATE GLOBAL VARIABLES ***********************************************
# "stations" is a hash of hashes.  The primary key is PADDR and the secondary
#     are FREQ and ALARM.
my %stations;

# "frequencies" is a hash of lists.  The key is the frequency value.
#     The lists store station PADDRs.
my %frequencies = ();

# ***** PRIVATE FUNCTION DEFINITIONS *******************************************

sub RxMsg
#  Brief: Processes messages received on the socket.
#  Parameters: [0] - The address of the sender of the message.
#  Parameters: [1] - The contents of the message received.
# PRE:  none.
# POST: none.
# Returns: nothing.
{
  my ($station_paddr, $msg) = @_;

  my $iaddr;
  my $port;
  my $iaddr_str;
  my $host;
  my ($type, $freq, $len, $data);

  ($type, $msg) = unpack("va*", $msg);

  if ($type == $MSG_CHANGE_FREQ)
    {
      ($freq) = unpack("v", $msg);
      if (defined $freq)
        {
          ChangeFreq($station_paddr, $freq);
        }
    }
  elsif ($type == $MSG_BROADCAST)
    {
      ($len, $data) = unpack("va*", $msg);
      if ($len > 0 and $len <= length($data))
        {
          Broadcast($station_paddr, $len, $data);
        }
      else
      {
          print" Error in message: type:($type) Length:($len) \n";
      }
    }
  elsif ($type == $MSG_LOGOFF)
    {
      Logoff($station_paddr);
    }
  else
    {
     print" Unknown message: ($type) \n";
    }
  return 1;
}


sub PurgeDeadStations
# Brief: Remove any stations that have not sent a heart beat for awhile.
{
  my $current = time();

  foreach (keys %stations)
  {
    # Has this station's alarm recently expired?
    Logoff($_)  if ($stations{$_}{ALARM} <= $current);
  }
  return;
}


sub ChangeFreq
# Brief:  Sets a stations frequency.  Adds entries in %stations and
#           %frequencies if the station has not been seen before.  If the
#           station is known its frequency is updated and the station is
#           removed from the old frequency and add onto the new frequency.
#           Creates a new entry in %frequencies is necessary.
#           If this is the first time the station has been seen an ACK is sent
#           back.
# Parameters:  [0] - PADDR of the station altering its frequency.
#              [1] - Frequency the station has selected.
# Returns:  Nothing.
{
  my ($station_paddr, $freq) = @_;
  my $oldFreq;
  my $msg;

  if ($DEBUG_B)
  {
    my $port;
    my $iaddr;  
    my $iaddr_str;

    ($port, $iaddr) = sockaddr_in($station_paddr);
    $iaddr_str = inet_ntoa($iaddr);
    print "Station ($iaddr_str) on port ($port) is on frequency ($freq).\n";
  }

  if (exists $stations{$station_paddr})                  # Have to do an update.
  {
    $oldFreq = $stations{$station_paddr}{FREQ};
    if (exists $frequencies{$oldFreq}{$station_paddr})
      {
        delete $frequencies{$oldFreq}{$station_paddr};
        if ((scalar keys %{$frequencies{$oldFreq}}) == 0)
          {
            delete $frequencies{$oldFreq};
          }
          $msg = pack("v", $MSG_ACK_STATION);
          defined(send(IONOSPHERE_SOCKET, $msg, 0, $station_paddr)) ||
            warn "$PROG: send() - $!";
      }
    }
  else   # New Station.
    {
      # Has the station limit been reached?
      if ((scalar keys %stations) < $MAX_STATIONS)  
        {
          $msg = pack("v", $MSG_ACK_STATION);
          defined(send(IONOSPHERE_SOCKET, $msg, 0, $station_paddr)) ||
            warn "$PROG: send() - $!";
        }
      else   # Maximum stations already configured.
        {
          $msg = pack("vvC*", $MSG_NAK_STATION, length($ERR_MAX_STATIONS), $ERR_MAX_STATIONS);
          defined(send(IONOSPHERE_SOCKET, $msg, 0, $station_paddr)) ||
            warn "$PROG: send() - $!";
          return;
        }
    }
  $stations{$station_paddr}{FREQ}  = $freq;
  $stations{$station_paddr}{ALARM} = time() + $HB_TIME_OUT;
  $frequencies{$freq}{$station_paddr} = 1;
  return;
}


sub Broadcast
# Brief:  Broadcasts a transmission to all other stations on the same frequency
#           as the transmitter.
# Parameters:  [0] - PADDR of the station broadcasting.
#              [1] - Length of the data following in bytes.
#              [2] - The buffer of data to be broadcast.
# Returns:  Nothing.
{
  my ($station_paddr, $len, $data) = @_;
  my $freq;
  my $next_paddr;
  my $msg;
  
#Debug
my $port;
my $iaddr;

  return  if ($len > $MAX_BCAST_LEN);
  #return  if (length($data) != $len);

  if (exists $stations{$station_paddr})
  {
    $freq = $stations{$station_paddr}{FREQ};
    $msg  = pack("vva*", $MSG_BROADCAST, $len, $data);
    if (exists $frequencies{$freq})
    {
      foreach $next_paddr (keys %{$frequencies{$freq}})
      {
        ($port, $iaddr) = sockaddr_in($next_paddr);
        unless ($station_paddr eq $next_paddr)
        {
          print "Broadcasting on frequency ($freq) to ",
                inet_ntoa($iaddr), " on port $port.\n"    if ($DEBUG_B);

          defined(send(IONOSPHERE_SOCKET, $msg, 0, $next_paddr)) ||
            warn "$PROG: send() - $!";
        }
      }
    }
    $stations{$station_paddr}{ALARM} = time() + $HB_TIME_OUT;
  }
  return;
}


sub Logoff
# Brief:  A station has indicated its intention to power down.
# Parameters:  [0] - PADDR of the station.
# Returns:  Nothing.
{
  my ($station_paddr) = @_;
  my $oldFreq;

  # If the station does not exist then ignore it!
  if (exists $stations{$station_paddr})                  # Have to do an update.
  {
    if ($DEBUG_B)
    {
      my $port;
      my $iaddr;  
      my $iaddr_str;

      ($port, $iaddr) = sockaddr_in($station_paddr);
      $iaddr_str = inet_ntoa($iaddr);
      print "Station ($iaddr_str) on port ($port) is logging-off.\n";
    }
    $oldFreq = $stations{$station_paddr}{FREQ};
    if (exists $frequencies{$oldFreq}{$station_paddr})
    {
      delete $frequencies{$oldFreq}{$station_paddr};
      if ((scalar keys %{$frequencies{$oldFreq}}) == 0)
      {
        delete $frequencies{$oldFreq};
      }
    }
    delete $stations{$station_paddr};
  }
  return;
}


#***** MAIN BLOCK **************************************************************

{
  my $switch = $ARGV[0];

  my ($my_iaddr, $proto, $my_paddr);
  my ($sender_paddr);
  my $msg;
  my ($rin, $rout);
  my $goAgain = 1;
  my $accessed_secs;
  my $modified_secs;

  if (defined $switch && ($switch ne "-debug"))
    {
      print "Brief:\n";
      print "  This programme '$PROG' facilitates the broadcasting of signals\n";
      print "    between stations operating on the same frequency.\n\n";
      print "USAGE:\n";
      print "  $PROG [ -debug ]\n\n";
      print "Where:\n";
      print "  -debug      Diagnostic messages will be displayed on the console.\n\n";
      print "You can press <Control>+<C> or 'kill' the console window at any time\n",
            "\tto shutdown '$PROG'.\n";
      print "This trial version allows $MAX_STATIONS stations to communicate.\n\n";
      exit;
    }
  $accessed_secs = (stat("$PROG_DIR$PROG"))[8];
  $modified_secs = (stat("$PROG_DIR$PROG"))[9];

#  unless (defined $accessed_secs && defined $modified_secs)
#  {
#    print "\n\n$PROG (Trial version $VERSION)\n";
#    print "=" x (length($PROG)), "\n";
#    print "\n\nCan not determine the file that this programme is executing from!\n";
#    print "Thought it should have been '$PROG_DIR$PROG'.\n";
#    print "Now exiting, sorry for any inconvience.\n\n";
#    exit;
#  }
#  if (($BUILD_DATE > time())    ||
#      ($accessed_secs > time()) ||
#      ($modified_secs > time())    )
#  {
#    print "\n\n$PROG (Trial version $VERSION)\n";
#    print "=" x (length($PROG)), "\n";
#    print "\n\nThe system date is incorrect!\n";
#    print "The trial version of this software requires the system date be set correctly.\n";
#    print "The registered version does not share this requirement.\n";
#    print "Please reset the system date and start this programme again.\n";
#    print "If the date is correct please re-install '$PROG'.\n";
#    print "Sorry for any inconvience.\n\n";
#    exit;
#  }
  
  if (defined $switch && ($switch eq "-debug"))
  {
    $DEBUG_B = 1;
  }
  
#  if (($EXPIRE_DATE < time())                            ||
#      (($accessed_secs + (3600 * 24 * 7 * 16)) < time()) ||
#      (($modified_secs + (3600 * 24 * 7 * 16)) < time())    )
#  {

#    my $current = time();

#    print "Time: ($current) ";

#    print "\n\n$PROG (Trial version $VERSION)\n";
#    print "=" x (length($PROG)), "\n";
#    print "Your trial period has concluded.\n";
#    print "Hope you found this software useful.\n";
#    print "Please purchase a registered version from  http://www.mrx.com.au\n";
#    print "or delete the software from this computer.\n\n";
#    exit;
#  }


  print "time " time();
  
  $my_iaddr = gethostbyname(hostname()) ||  die "$PROG: gethostbyname() - $!";
  $proto = getprotobyname("udp");
  $my_paddr = sockaddr_in($IONOSPHERE_PORT, INADDR_ANY);
  socket(IONOSPHERE_SOCKET, PF_INET, SOCK_DGRAM, $proto) ||
    die "$PROG: socket() - $!";
  bind(IONOSPHERE_SOCKET, $my_paddr) || die "$PROG: bind() - $!";
  
  while ($goAgain)
    {
      $rin = '';
      vec($rin, fileno(IONOSPHERE_SOCKET), 1) = 1;

      # timeout after 1.0 second
      while (select($rout = $rin, undef, undef, 1.0))
        {
          ($sender_paddr = recv(IONOSPHERE_SOCKET, $msg, 1000, 0)) ||
            die "$PROG: recv() - $!";
          die "$PROG: recv() - $!\n" unless ($sender_paddr);
          $goAgain = RxMsg($sender_paddr, $msg);
        }
      PurgeDeadStations;
      if ($EXPIRE_DATE < time())
      {
        print "\n\n$PROG (Trial version $VERSION)\n";
        print "=" x (length($PROG)), "\n";
        print "Your trial period has concluded.\n";
        print "Hope you found this software useful.\n";
        print "Please purchase a registered version from  http://www.mrx.com.au\n";
        print "or delete the software from this computer.\n\n";
        exit;
      }
    }
  close (IONOSPHERE_SOCKET);
}
exit;
