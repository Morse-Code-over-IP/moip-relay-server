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
'* Bob Denny    13-Jul-2013     4.0.1.0 - For API 1.1 many changes
'*
Namespace TwitterVB2
    ''' <summary>
    ''' An object that encapsulates the relationship between two Twitter users. This object provides an interface to the <c>friendships/show</c> Twitter REST API method.
    ''' An object that represents the relationship between two Twitter users. This object provides an interface to the <c>friendships/show</c> Twitter REST API method.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TwitterRelationship

        Private _Source As TwitterRelationshipElement
        Private _Target As TwitterRelationshipElement

        ''' <summary>
        ''' The relationship between the source and target users.
        ''' </summary>
        ''' <value></value>
        ''' <returns>A <c>TwitterRelationshipElement</c> representing the relationship between the source and target users, in the context of the source user.</returns>
        ''' <remarks></remarks>
        Public Property Source() As TwitterRelationshipElement
            Get
                Return _Source
            End Get
            Set(ByVal value As TwitterRelationshipElement)
                _Source = value
            End Set
        End Property

        ''' <summary>
        ''' The relationship between the source and target users.
        ''' </summary>
        ''' <value></value>
        ''' <returns>A <c>TwitterRelationshipElement</c> representing the relationship between the source and target users, in the context of the target user.</returns>
        ''' <remarks></remarks>
        Public Property Target() As TwitterRelationshipElement
            Get
                Return _Target
            End Get
            Set(ByVal value As TwitterRelationshipElement)
                _Target = value
            End Set
        End Property

        ''' <summary>
        ''' Creates a new <c>TwitterRelationshipElement</c> object.
        ''' </summary>
        ''' <param name="RelationshipDict">A <c>JSON</c> block from the Twitter API response representing a relationship. This is just the 'target' and 'source' with the outer 'relationship' stripped</param>
        ''' <remarks></remarks>
        ''' <exclude/>
        Public Sub New(ByVal RelationshipDict As Dictionary(Of String, Object))

            If RelationshipDict("source") IsNot Nothing Then
                Me.Source = New TwitterRelationshipElement(CType(RelationshipDict("source"), Dictionary(Of String, Object)))
            End If
            If RelationshipDict("target") IsNot Nothing Then
                Me.Target = New TwitterRelationshipElement(CType(RelationshipDict("target"), Dictionary(Of String, Object)))
            End If

        End Sub

        ''' <summary>
        ''' Encapsulates the relationship of one user to another
        ''' </summary>
        ''' <remarks></remarks>
        Public Class TwitterRelationshipElement
            Inherits XmlObjectBase

            Private _ID As Int64
            Private _ScreenName As String
            Private _Following As Boolean
            Private _FollowedBy As Boolean
            Private _NotificationsEnabled As Boolean

            ''' <summary>
            ''' The Twitter ID of the in-context user
            ''' </summary>
            ''' <value></value>
            ''' <returns>An <c>Int64</c> representing the Twitter ID of the in-context user.</returns>
            ''' <remarks></remarks>
            Public Property ID() As Int64
                Get
                    Return _ID
                End Get
                Set(ByVal value As Int64)
                    _ID = value
                End Set
            End Property

            ''' <summary>
            ''' The Twitter screen name of the in-context user
            ''' </summary>
            ''' <value></value>
            ''' <returns>An <c>string</c> representing the screen name of the in-context user.</returns>
            ''' <remarks></remarks>
            Public Property ScreenName() As String
                Get
                    Return _ScreenName
                End Get
                Set(ByVal value As String)
                    _ScreenName = value
                End Set
            End Property

            ''' <summary>
            ''' Whether the in-context user follows the other user
            ''' </summary>
            ''' <value></value>
            ''' <returns>An <c>boolean</c> that indicates if the in-context user follows the other user.</returns>
            ''' <remarks></remarks>
            Public Property Following() As Boolean
                Get
                    Return _Following
                End Get
                Set(ByVal value As Boolean)
                    _Following = value
                End Set
            End Property

            ''' <summary>
            ''' Whether the in-context user is followed by the other user
            ''' </summary>
            ''' <value></value>
            ''' <returns>An <c>boolean</c> that indicates if the in-context user is followed by the other user.</returns>
            ''' <remarks></remarks>
            Public Property FollowedBy() As Boolean
                Get
                    Return _FollowedBy
                End Get
                Set(ByVal value As Boolean)
                    _FollowedBy = value
                End Set
            End Property

            ''' <summary>
            ''' Whether the in-context user receives device notifications regarding status updates of other user
            ''' </summary>
            ''' <value></value>
            ''' <returns>An <c>boolean</c> that indicates if the in-context user receives device notifications regarding status updates of the other user.</returns>
            ''' <remarks>Due to its private nature, the Twitter API only populates this property in the context of the source user and only if the source user is authenticated.<para />The <c>NotificationsEnabled</c> property of the <c>TwitterRelationshipElement</c> object in the context of the target user will always be Null, as will the <c>NotificationsEnabled</c> property of the <c>TwitterRelationshipElement</c> object in the context of the source user if the source user is unauthenticated.</remarks>
            Public Property NotificationsEnabled() As Boolean
                Get
                    Return _NotificationsEnabled
                End Get
                Set(ByVal value As Boolean)
                    _NotificationsEnabled = value
                End Set
            End Property

            ''' <summary>
            ''' Creates a new <c>TwitterRelationshipElement</c> object.
            ''' </summary>
            ''' <param name="RelationshipElementDict">A Key-Value Dictionary from the Twitter API response representing a relationship element.</param>
            ''' <remarks>
            ''' As exposed by the Twitter API, a relationship between two users is made up of two elements. Each element shows Following and FollowedBy relationship in the context of one particular user.
            ''' In API 1.1, the source user's info includes additional info but this has not yet been implemented here. 
            ''' </remarks>
            ''' <exclude/>
            Public Sub New(ByVal RelationshipElementDict As Dictionary(Of String, Object))
                Dim KV As KeyValuePair(Of String, Object)
                For Each KV In RelationshipElementDict
                    If Not KV.Value Is Nothing Then
                        Select Case KV.Key
                            Case "id"
                                Me.ID = CLng(KV.Value)
                            Case "screen_name"
                                Me.ScreenName = KV.Value.ToString
                            Case "following"
                                Me.Following = CBool(KV.Value)
                            Case "followed_by"
                                Me.FollowedBy = CBool(KV.Value)
                            Case "notifications_enabled"
                                Me.NotificationsEnabled = CBool(KV.Value)
                        End Select
                    End If
                Next
            End Sub

        End Class

    End Class

End Namespace
