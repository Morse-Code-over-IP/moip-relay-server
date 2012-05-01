'*
'* This file is part of the TwitterVB software
'* Copyright (c) 2009, Duane Roelands <duane@getTwitterVB.com>
'* All rights reserved.
'*
'* TwitterVB is a port of the Twitterizer library <http://code.google.com/p/twitterizer/>
'* Copyright (c) 2008, Patrick "Ricky" Smith <ricky@digitally-born.com>
'* All rights reserved. 
'*
'* Redistribution and use in source and binary forms, with or without modification, are 
'* permitted provided that the following conditions are met:
'*
'* - Redistributions of source code must retain the above copyright notice, this list 
'*   of conditions and the following disclaimer.
'* - Redistributions in binary form must reproduce the above copyright notice, this list 
'*   of conditions and the following disclaimer in the documentation and/or other 
'*   materials provided with the distribution.
'* - Neither the name of TwitterVB nor the names of its contributors may be 
'*   used to endorse or promote products derived from this software without specific 
'*   prior written permission.
'*
'* THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
'* ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
'* WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
'* IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
'* INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
'* NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
'* PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
'* WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
'* ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
'* POSSIBILITY OF SUCH DAMAGE.
'*
Imports TwitterVB2
Namespace TwitterVB2
    Public Class Examples
#Region "Get Public Timeline"
        Public Sub GetPublicTimeline()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            For Each status As TwitterStatus In tw.PublicTimeline
                Console.WriteLine(status.User.ScreenName & " : " & status.Text)
            Next
        End Sub

        Public Sub GetPublicTimelineWithParameters()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

            ' Specify that we want 50 statuses instead of the default 20
            Dim tp As New TwitterParameters
            tp.Add(TwitterParameterNames.Count, 50)

            For Each status As TwitterStatus In tw.PublicTimeline(tp)
                Console.WriteLine(status.User.ScreenName & " : " & status.Text)
            Next
        End Sub
#End Region

#Region "Get Friends Timeline"
        Public Sub GetFriendsTimeline()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            For Each status As TwitterStatus In tw.FriendsTimeline
                Console.WriteLine(status.User.ScreenName & " : " & status.Text)
            Next
        End Sub

        Public Sub GetFriendsTimelineWithParameters()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

            ' Specify that we want 50 statuses instead of the default 20
            Dim tp As New TwitterParameters
            tp.Add(TwitterParameterNames.Count, 50)

            For Each status As TwitterStatus In tw.FriendsTimeline(tp)
                Console.WriteLine(status.User.ScreenName & " : " & status.Text)
            Next
        End Sub
#End Region

#Region "Get Home Timeline"
        Public Sub GetHomeTimeline()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            For Each status As TwitterStatus In tw.HomeTimeline
                Console.WriteLine(status.User.ScreenName & " : " & status.Text)
            Next
        End Sub

        Public Sub GetHomeTimelineWithParameters()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

            ' Specify that we want 50 statuses instead of the default 20
            Dim tp As New TwitterParameters
            tp.Add(TwitterParameterNames.Count, 50)

            For Each status As TwitterStatus In tw.HomeTimeline(tp)
                Console.WriteLine(status.User.ScreenName & " : " & status.Text)
            Next
        End Sub
#End Region

#Region "Get User Timeline"
        Public Sub GetUserTimeline()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            For Each status As TwitterStatus In tw.UserTimeline
                Console.WriteLine(status.User.ScreenName & " : " & status.Text)
            Next
        End Sub

        Public Sub GetUserTimeLineFromSpecificUser(ByVal Username As String)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            For Each status As TwitterStatus In tw.UserTimeline("DWRoelands")
                Console.WriteLine(status.User.ScreenName & " : " & status.Text)
            Next
        End Sub

        Public Sub GetUserTimelineWithParameters()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

            ' Specify that we want 50 statuses instead of the default 20
            Dim tp As New TwitterParameters
            tp.Add(TwitterParameterNames.Count, 50)

            For Each status As TwitterStatus In tw.UserTimeline(tp)
                Console.WriteLine(status.User.ScreenName & " : " & status.Text)
            Next
        End Sub
#End Region


#Region "Get Mentions"
        Public Sub GetMentions()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            For Each status As TwitterStatus In tw.Mentions
                Console.WriteLine(status.User.ScreenName & " : " & status.Text)
            Next
        End Sub

        Public Sub GetMentionsWithParameters()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

            ' Specify that we want 50 statuses instead of the default 20
            Dim tp As New TwitterParameters
            tp.Add(TwitterParameterNames.Count, 50)

            For Each status As TwitterStatus In tw.Mentions(tp)
                Console.WriteLine(status.User.ScreenName & " : " & status.Text)
            Next
        End Sub
#End Region

#Region "Get Replies"
        Public Sub GetReplies()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            For Each status As TwitterStatus In tw.Replies
                Console.WriteLine(status.User.ScreenName & " : " & status.Text)
            Next
        End Sub

        Public Sub GetRepliesWithParameters()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

            ' Specify that we want 50 statuses instead of the default 20
            Dim tp As New TwitterParameters
            tp.Add(TwitterParameterNames.Count, 50)

            For Each status As TwitterStatus In tw.Replies(tp)
                Console.WriteLine(status.User.ScreenName & " : " & status.Text)
            Next
        End Sub
#End Region

#Region "Get Account Information"
        Public Sub GetAccountInformation()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

            Dim MyAccountInfo As TwitterUser = tw.AccountInformation
            Console.WriteLine("Screen Name:" & MyAccountInfo.ScreenName)
            Console.WriteLine("Followers:" & MyAccountInfo.FollowersCount)
        End Sub
#End Region

#Region "Block User"
        Public Sub BlockUser(ByVal ID As Int64)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            tw.BlockUser(ID)
        End Sub

        Public Sub BlockUser(ByVal ScreenName As String)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            tw.BlockUser(ScreenName)
        End Sub
#End Region

#Region "Unblock User"
        Public Sub UnblockUser(ByVal ID As Int64)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            tw.UnblockUser(ID)
        End Sub

        Public Sub UnblockUser(ByVal ScreenName As String)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            tw.UnblockUser(ScreenName)
        End Sub
#End Region

#Region "Get Blocked IDs"
        Public Sub GetBlockedIDs()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

            For Each UserID As Int64 In tw.BlockedIDs
                Console.WriteLine(UserID)
            Next
        End Sub
#End Region

#Region "Get Blocked Users"
        Public Sub GetBlockedUsers()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

            For Each User As TwitterUser In tw.BlockedUsers
                Console.WriteLine(User.ScreenName)
            Next
        End Sub
#End Region

#Region "Get Direct Messages"
        Public Sub GetDirectMessages()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

            For Each Message As TwitterDirectMessage In tw.DirectMessages
                Console.WriteLine(Message.SenderScreenName & " : " & Message.Text)
            Next
        End Sub

        Public Sub GetDirectMessagesWithParameters()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

            ' Specify that we want 50 statuses instead of the default 20
            Dim tp As New TwitterParameters
            tp.Add(TwitterParameterNames.Count, 50)

            For Each Message As TwitterDirectMessage In tw.DirectMessages(tp)
                Console.WriteLine(Message.SenderScreenName & " : " & Message.Text)
            Next
        End Sub
#End Region

#Region "Get Sent Direct Messages"
        Public Sub GetSentDirectMessages()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

            For Each Message As TwitterDirectMessage In tw.DirectMessagesSent
                Console.WriteLine(Message.RecipientScreenName & " : " & Message.Text)
            Next
        End Sub

        Public Sub GetSentDirectMessagesWithParameters()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

            ' Specify that we want 50 statuses instead of the default 20
            Dim tp As New TwitterParameters
            tp.Add(TwitterParameterNames.Count, 50)

            For Each Message As TwitterDirectMessage In tw.DirectMessagesSent(tp)
                Console.WriteLine(Message.RecipientScreenName & " : " & Message.Text)
            Next
        End Sub
#End Region

#Region "Send Direct Message"
        Public Sub SendDirectMessage()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            Dim SentMessage As TwitterDirectMessage = tw.SendDirectMessage("username", "Hello!")
        End Sub
#End Region

#Region "Delete Direct Message"
        Public Sub DeleteDirectMessage(ByVal ID As Int64)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            tw.DeleteDirectMessage(ID)
        End Sub
#End Region

#Region "Get Favorites"
        Public Sub GetFavorites()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            For Each status As TwitterStatus In tw.Favorites
                Console.WriteLine(status.User.ScreenName & " : " & status.Text)
            Next
        End Sub

        Public Sub GetFavoritesWithParameters()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

            ' Specify which page of results we want.
            Dim tp As New TwitterParameters
            tp.Add(TwitterParameterNames.Page, 2)

            For Each status As TwitterStatus In tw.Favorites
                Console.WriteLine(status.User.ScreenName & " : " & status.Text)
            Next
        End Sub
#End Region

#Region "Create Favorite"
        Public Sub CreateFavorite(ByVal ID As Int64)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            tw.AddFavorite(ID)
        End Sub
#End Region

#Region "Delete Favorite"
        Public Sub DeleteFavorite(ByVal ID As Int64)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            tw.DeleteFavorite(ID)
        End Sub
#End Region

#Region "Basic Search"
        Public Sub BasicSearch()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

            For Each Result As TwitterSearchResult In tw.Search("TwitterVB")
                Console.WriteLine(Result.Title)
            Next
        End Sub
#End Region

#Region "Advanced Search"
        Public Sub AdvancedSearch()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

            Dim tsp As New TwitterSearchParameters
            tsp.Add(TwitterSearchParameterNames.SearchTerm, "TwitterVB")  'search term
            tsp.Add(TwitterSearchParameterNames.Rpp, 100)  ' 100 responses per page

            For Each Result As TwitterSearchResult In tw.Search(tsp)
                Console.WriteLine(Result.Title)
            Next
        End Sub
#End Region

#Region "Trends"
        Public Sub GetTrends()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

            ' Top trends
            For Each t As TwitterTrend In tw.Trends
                Console.WriteLine(t.TrendText)
            Next

            ' Current trends
            For Each t As TwitterTrend In tw.TrendsCurrent
                Console.WriteLine(t.TrendText)
            Next

            'Daily trends
            For Each t As TwitterTrend In tw.TrendsDaily
                Console.WriteLine(t.TrendText)
            Next

            'Weekly trends
            For Each t As TwitterTrend In tw.TrendsWeekly
                Console.WriteLine(t.TrendText)
            Next
        End Sub

#End Region

#Region "Follow User"
        Public Sub FollowUser(ByVal ID As Int64)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            tw.Follow(ID)
        End Sub

        Public Sub FollowUser(ByVal ScreenName As String)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            tw.Follow(ScreenName)
        End Sub
#End Region

#Region "Unfollow User"
        Public Sub UnfollowUser(ByVal ID As Int64)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            tw.UnFollow(ID)
        End Sub

        Public Sub UnfollowUser(ByVal ScreenName As String)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            tw.UnFollow(ScreenName)
        End Sub
#End Region

#Region "Social Graph Friends"
        Public Sub GetFriendIDs()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            For Each FriendID As Int64 In tw.FriendsIDs
                Console.WriteLine(FriendID)
            Next
        End Sub

        Public Sub GetFriendsIDs(ByVal ID As Int64)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            For Each FriendID As Int64 In tw.FriendsIDs(ID)
                Console.WriteLine(FriendID)
            Next
        End Sub

        Public Sub GetFriendsIDs(ByVal ScreenName As String)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            For Each FriendID As Int64 In tw.FriendsIDs(ScreenName)
                Console.WriteLine(FriendID)
            Next
        End Sub
#End Region

#Region "Social Graph Followers"
        Public Sub GetFollowersIDs()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            For Each FollowerID As Int64 In tw.FollowersIDs
                Console.WriteLine(FollowerID)
            Next
        End Sub

        Public Sub GetFollowersIDs(ByVal ID As Int64)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            For Each FollowerID As Int64 In tw.FollowersIDs(ID)
                Console.WriteLine(FollowerID)
            Next
        End Sub

        Public Sub GetFollowersIDs(ByVal ScreenName As String)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            For Each FollowerID As Int64 In tw.FollowersIDs(ScreenName)
                Console.WriteLine(FollowerID)
            Next
        End Sub
#End Region

#Region "Post a Tweet"
        Public Sub PostTweet(ByVal Text As String)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            tw.Update(Text)
        End Sub
#End Region

#Region "Post a Tweet from a specific location"
	Public Sub PostTweetFromLocation(byval p_Tweet as String, byval p_lat as Double, byval p_lon as double)
		dim tw as new TwitterAPI
		tw.AuthenticateWith("ConsumerKey","ConsumerSecret","Token","TokenSecret")
		tw.update(p_Tweet,p_lat_p_lon)
	end sub
#End Region

#Region "Post a Reply"
        Public Sub PostReply(ByVal Text As String, ByVal ID As Int64)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            tw.ReplyToUpdate(Text, ID)
        End Sub
#End Region

#Region "Delete a Tweet"
        Public Sub DeleteTweet(ByVal ID As Int64)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            tw.DeleteUpdate(ID)
        End Sub
#End Region

#Region "Show a Tweet"
        Public Sub ShowTweet(ByVal ID As Int64)
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            tw.ShowUpdate(ID)
        End Sub
#End Region

#Region "Get User Information"
        Public Function GetUserInfo(ByVal ID As Int64) As TwitterUser
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            Return tw.ShowUser(ID)
        End Function

        Public Function GetUserInfo(ByVal ScreenName As String) As TwitterUser
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            Return tw.ShowUser(ScreenName)
        End Function
#End Region

#Region "Get Followers"
        Public Sub GetFollowers()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
            For Each Follower As TwitterUser In tw.Followers
                Console.WriteLine(Follower.ScreenName)
            Next
        End Sub

        Public Sub GetFollowersWithParameters()
            Dim tw As New TwitterAPI
            tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

            'Specify that we want to see someone else's followers
            Dim tp As New TwitterParameters
            tp.Add(TwitterParameterNames.ScreenName, "DWRoelands")

            For Each Follower As TwitterUser In tw.Followers(tp)
                Console.WriteLine(Follower.ScreenName)
            Next
        End Sub

#End Region

#Region "Get Relationship"
		Public Sub GetRelationship()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			Dim tr As TwitterRelationship = tw.Relationship("yourScreenName", "DWRoelands")
			Console.WriteLine(String.Format("Do you follow Duane? {0}", IIf(tr.Source.Following, "Yes", "No")))
			Console.WriteLine(String.Format("Does Duane follow you? {0}", IIf(tr.Source.FollowedBy, "Yes", "No")))
		End Sub
#End Region

#Region "Reporting a Spammer"
		Public Sub ReportSpam()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			'Get the first tweet from the public timeline
			Dim FirstPublicTweet As TwitterStatus = tw.PublicTimeline()(0)

			' Report the poster of this tweet as a spammer
			tw.ReportSpam(FirstPublicTweet.User.ID)
		End Sub

		Public Sub ReportSpam(ByVal ScreenName As String)
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			'Get the first tweet from the public timeline
			Dim FirstPublicTweet As TwitterStatus = tw.PublicTimeline()(0)

			' Report the poster of this tweet as a spammer
			tw.ReportSpam(FirstPublicTweet.User.ScreenName)
		End Sub
#End Region

#Region "Retweeted By Me"
		Public Sub GetRetweetedByMe()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
			For Each status As TwitterStatus In tw.RetweetedByMe
				Console.WriteLine(status.User.ScreenName & " : " & status.Text)
			Next
		End Sub

		Public Sub GetRetweetedByMeWithParameters()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			' Specify that we want 50 statuses instead of the default 20
			Dim tp As New TwitterParameters
			tp.Add(TwitterParameterNames.Count, 50)

			For Each status As TwitterStatus In tw.RetweetedByMe(tp)
				Console.WriteLine(status.User.ScreenName & " : " & status.Text)
			Next
		End Sub
#End Region

#Region "Retweeted To Me"
		Public Sub GetRetweetedToMe()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
			For Each status As TwitterStatus In tw.RetweetedToMe
				Console.WriteLine(status.User.ScreenName & " : " & status.Text)
			Next
		End Sub

		Public Sub GetRetweetedToMeWithParameters()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			' Specify that we want 50 statuses instead of the default 20
			Dim tp As New TwitterParameters
			tp.Add(TwitterParameterNames.Count, 50)

			For Each status As TwitterStatus In tw.RetweetedToMe(tp)
				Console.WriteLine(status.User.ScreenName & " : " & status.Text)
			Next
		End Sub
#End Region

#Region "Retweets Of Me"
		Public Sub GetRetweetsOfMe()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
			For Each status As TwitterStatus In tw.RetweetsOfMe
				Console.WriteLine(status.User.ScreenName & " : " & status.Text)
			Next
		End Sub

		Public Sub GetRetweetsOfMeWithParameters()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			' Specify that we want 50 statuses instead of the default 20
			Dim tp As New TwitterParameters
			tp.Add(TwitterParameterNames.Count, 50)

			For Each status As TwitterStatus In tw.RetweetsOfMe(tp)
				Console.WriteLine(status.User.ScreenName & " : " & status.Text)
			Next
		End Sub
#End Region

#Region "Retweeting a Tweet"
		Public Sub Retweet()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			'Get the first tweet from the public timeline
			Dim FirstPublicTweet As TwitterStatus = tw.PublicTimeline()(0)

			'Retweet it
			tw.Retweet(FirstPublicTweet.ID)
		End Sub
#End Region

#Region "Retrieving Retweets"
		Public Sub GetRetweets()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			'Get the first tweet from the public timeline
			Dim FirstPublicTweet As TwitterStatus = tw.PublicTimeline()(0)

			' Get the first 50 retweets
			Dim FirstRetweets As List(Of TwitterStatus) = tw.Retweets(FirstPublicTweet.ID, 50)
			For Each Retweet As TwitterStatus In FirstRetweets
				Console.WriteLine(Retweet.Text)
			Next
		End Sub
#End Region

#Region "TwitPic"
		Public Sub UploadPhoto()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			Dim TwitPicUrl As String = tw.TwitPicUpload("C:\pictures\funnycats.jpg", "Funny Cats!", "TwitterUserName", "TwitterPassword")
			Console.WriteLine(TwitPicUrl)
		End Sub
#End Region

#Region "Create List"
		Public Sub CreateList()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			Dim NewList As TwitterList = tw.ListCreate("DWRoelands", "MyList", ListMode.Public, "This is my list")
		End Sub
#End Region

#Region "Update List"
		Public Sub UpdateList()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			Dim NewList As TwitterList = tw.ListUpdate("DWRoelands", "duanetest", "MyList", ListMode.Public, "This is my list")
		End Sub
#End Region

#Region "Get List"
		Public Sub GetList()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			Dim NewList As TwitterList = tw.ListGet("DWRoelands", "MyTestList")
		End Sub
#End Region

#Region "Get Lists"
		Public Sub GetLists()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			Dim NewList As List(Of TwitterList) = tw.ListsGet("DWRoelands")
			For Each TList As TwitterList In NewList
				Console.Write(TList.Name)
			Next
		End Sub
#End Region

#Region "Delete List"
		Public Sub DeleteList()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			Dim DeletedList As TwitterList = tw.ListDelete("DWRoelands", "MyTestList")
		End Sub
#End Region

#Region "Get List Statuses"
		Public Sub GetListStatuses()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			For Each s As TwitterStatus In tw.ListStatuses("DWRoelands", "MyTestList")
				Console.WriteLine(s.Text)
			Next
		End Sub
#End Region

#Region "Get List Memberships"
		Public Sub GetListMemberships()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			For Each MemberList As TwitterList In tw.ListMemberships("DWRoelands")
				Console.WriteLine(MemberList.Name)
			Next
		End Sub
#End Region

#Region "Get List Subscriptions"
		Public Sub GetListSubscriptions()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			For Each MemberList As TwitterList In tw.ListSubscriptions("DWRoelands")
				Console.WriteLine(MemberList.Name)
			Next
		End Sub
#End Region

#Region "Get List Members"
		Public Sub GetListMembers()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			For Each Member As TwitterUser In tw.ListMembers("DWRoelands", "MyTestList")
				Console.WriteLine(Member.ScreenName)
			Next
		End Sub
#End Region

#Region "Add Member To List"
		Public Sub AddMember(ByVal UserID As Int64)
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
			tw.ListMembersAdd("DWRoelands", "MyTestList", UserID)
		End Sub
#End Region

#Region "Remove Member From List"
		Public Sub RemoveMember(ByVal UserID As Int64)
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
			tw.ListMembersDelete("DWRoelands", "MyTestList", UserID)
		End Sub
#End Region

#Region "Check User Membership"
		Public Sub CheckList(ByVal UserID As Int64)
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			If tw.ListMembersCheck("DWRoelands", "MyTestList", UserID) Then
				Console.WriteLine("The member belongs to the list")
			Else
				Console.WriteLine("The member does not belong to the list.")
			End If
		End Sub
#End Region

#Region "Get List Subscribers"
		Public Sub GetListSubscribers()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			For Each Member As TwitterUser In tw.ListSubscribers("DWRoelands", "MyTestList")
				Console.WriteLine(Member.ScreenName)
			Next
		End Sub
#End Region

#Region "Subscribe To List"
		Public Sub Subscribe()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
			tw.ListSubscribe("DWRoelands", "MyTestList")
		End Sub
#End Region

#Region "Unsubscribe From List"
		Public Sub Unsubscribe()
            Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")
			tw.ListUnsubscribe("DWRoelands", "MyTestList")
        End Sub
#End Region

#Region "Check User Subscription"
		Public Sub CheckSubscriber(ByVal UserID As Int64)
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			If tw.ListSubscribersCheck("DWRoelands", "MyTestList", UserID) Then
				Console.WriteLine("The member subscribes to the list")
			Else
				Console.WriteLine("The member does not subscribe to the list.")
			End If
		End Sub
#End Region

#Region "TweetPhoto"
		Public Sub UploadPhotoToTweetPhoto()
			Dim tw As New TwitterAPI
			tw.AuthenticateWith("ConsumerKey","ConsumerSecret","TokenKey","TokenSecret")

			Dim TweetPhotoUrl As String = tw.TweetPhotoUpload("C:\pictures\funnycats.jpg", "Funny Cats!", "TwitterUserName", "TwitterPassword", "TweetPhotoApiKey")
			Console.WriteLine(TweetPhotoUrl)
		End Sub
#End Region

#Region "Simple OAuth"
        '
        ' Runs as a console program. Shows the use of the browser to log into
        ' Twitter with the user's credentials, then get a PIN for OAuth. The
        ' PIN is used to retrieve the AuthToken and AuthTokenSecret. These 
        ' two strings may be cached locally and used to authenticate as shown
        ' in most of the other examples in this documentation set.
        '
        Public Sub Main()
            Dim tw As New TwitterAPI
            Dim link As String
            Dim pin As String

            link = tw.GetAuthenticationLink("ConsumerKey", "ConsumerSecret")
            System.Diagnostics.Process.Start(link) ' Use browser to log in and get PIN
            pin = ""
            Do
                System.Console.Write("Enter PIN:")
                pin = System.Console.ReadLine()
                If pin = "" Then Exit Sub ' Exit the program
                If tw.ValidatePIN(pin) Then Exit Do
            Loop
            System.Console.WriteLine("Access granted to program")
            ' The following will be the same for a given user once they log in and grant acccess to
            ' the app (using its "consumer" key pair above in GetAuthenticationLink()).
            Console.WriteLine("  oAuthToken       = " + tw.OAuth_Token)
            Console.WriteLine("  oAuthTokenSecret = " + tw.OAuth_TokenSecret)
            Console.Write("Press enter to quit...")
            Console.ReadLine()
        End Sub
#End Region



    End Class

End Namespace
