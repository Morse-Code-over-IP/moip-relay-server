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
'* Bob Denny    13-Jul-2013     4.0.1.0 - For API 1.1 Completely changed
'*
Namespace TwitterVB2
    ''' <summary>
    ''' Result of a search. List of tweets and also metadata.
    ''' </summary>
    ''' <remarks>
    ''' The API 1.1 returns a list of TwitterStatus objects (tweets) as well as some meta-data about the
    ''' search operation. Included in the meta-data are SinceID and MaxID which can be used to page results.
    ''' </remarks>
    Public Class TwitterSearchResult

#Region "Prvate Members"
        Private _Statuses As New List(Of TwitterStatus)
        Private _MaxID As Int64
        Private _SinceID As Int64
        Private _RefreshUrl As String = String.Empty
        Private _NextResults As String = String.Empty
        Private _Count As Int64
        Private _CompletedIn As Double
        Private _Query As String = String.Empty
#End Region

#Region "Public Properties"
        ''' <summary>Returned tweets</summary>
        ''' <returns>A list of TwitterStatus objects</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Statuses() As List(Of TwitterStatus)
            Get
                Return _Statuses
            End Get
        End Property

        ''' <summary>
        ''' The highest ID of the returned tweets
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MaxID() As Int64
            Get
                Return _MaxID
            End Get
        End Property

        ''' <summary>
        ''' Tweets returned with ID greater than this
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property SinceID() As Int64
            Get
                Return _SinceID
            End Get
        End Property

        ''' <summary>
        ''' The URL query string to use for refreshing the search
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' This is local time.
        ''' </remarks>
        Public ReadOnly Property RefreshUrl() As String
            Get
                Return _RefreshURL
            End Get
        End Property

        ''' <summary>
        ''' The URL query string to use for the next block of results
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property NextResults() As String
            Get
                Return _NextResults
            End Get
        End Property

        ''' <summary>
        ''' Count of tweets returned
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Count() As Int64
            Get
                Return _Count
            End Get
        End Property

        ''' <summary>
        ''' Time (sec) for search to complete
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks>
        ''' Usernames are rendered as links in this text.
        ''' </remarks>
        Public ReadOnly Property CompletedIn() As Double
            Get
                Return _CompletedIn
            End Get
        End Property

        ''' <summary>
        ''' A copy of the search string used for this search
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Query() As String
            Get
                Return _Query
            End Get
        End Property

#End Region

        ''' <summary>
        ''' Creates a <c>TwitterSearchResult</c> object.
        ''' </summary>
        ''' <param name="JSON">A <c>JSON</c> block from the Twitter API response representing a search result.</param>
        ''' <remarks>This single JSON object contains within it the list of returned tweets plus metadata.</remarks>
        Public Sub New(ByVal JSON As String)
            Dim JS As JavaScriptSerializer = New JavaScriptSerializer()
            Dim ResultDict As Dictionary(Of String, Object) = JS.Deserialize(Of Dictionary(Of String, Object))(JSON)

            Dim KV As KeyValuePair(Of String, Object)
            For Each KV In CType(ResultDict("search_metadata"), Dictionary(Of String, Object))
                If Not KV.Value Is Nothing Then
                    Select Case KV.Key
                        Case "count"
                            _Count = CLng(KV.Value)
                        Case "max_id"
                            _MaxID = CLng(KV.Value)
                        Case "since_id"
                            _SinceID = CLng(KV.Value)
                        Case "refresh_url"
                            _RefreshUrl = KV.Value.ToString
                        Case "next_results"
                            _NextResults = KV.Value.ToString
                        Case "query"
                            _Query = KV.Value.ToString
                        Case "completed_in"
                            _CompletedIn = CDbl(KV.Value)
                    End Select
                End If
            Next
            If _Count > 0 Then
                Dim StatusList As ArrayList = CType(ResultDict("statuses"), ArrayList)
                For Each StatusDict As Dictionary(Of String, Object) In StatusList
                    Statuses.Add(New TwitterStatus(StatusDict))
                Next

            End If
        End Sub
    End Class
End Namespace
