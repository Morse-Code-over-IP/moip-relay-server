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
'* http://stackoverflow.com/questions/7895105/json-deserialize-c-sharp
'* http://www.tomasvera.com/programming/using-javascriptserializer-to-parse-json-objects/
'*
'* 13-Jul-2013  rbd     4.0.1.0 - For API 1.1 JSON many changes
'*
Namespace TwitterVB2

    Public Class TwitterTrendLocation
        Private _ID As String = String.Empty
        Private _LocationName As String = String.Empty
        Private _PlaceTypeCode As Long
        Private _PlaceTypeName As String = String.Empty
        Private _CountryName As String = String.Empty
        Private _CountryCode As Integer
        Private _Url As String = String.Empty


        Public Property ID() As String
            Get
                Return _ID
            End Get
            Set(ByVal value As String)
                _ID = value
            End Set
        End Property


        Public Property LocationName() As String
            Get
                Return _LocationName
            End Get
            Set(ByVal value As String)
                _LocationName = value
            End Set
        End Property


        Public Property PlaceTypeCode() As Long
            Get
                Return _PlaceTypeCode
            End Get
            Set(ByVal value As Long)
                _PlaceTypeCode = value
            End Set
        End Property


        Public Property PlaceTypeName() As String
            Get
                Return _PlaceTypeName
            End Get
            Set(ByVal value As String)
                _PlaceTypeName = value
            End Set
        End Property


        Public Property CountryName() As String
            Get
                Return _CountryName
            End Get
            Set(ByVal value As String)
                _CountryName = value
            End Set
        End Property


        Public Property CountryCode() As Integer
            Get
                Return _CountryCode
            End Get
            Set(ByVal value As Integer)
                _CountryCode = value
            End Set
        End Property


        Public Property Url() As String
            Get
                Return _Url
            End Get
            Set(ByVal value As String)
                _Url = value
            End Set
        End Property


        Public Sub New(ByVal TrendLocationDict As Dictionary(Of String, Object))
            Dim KV As KeyValuePair(Of String, Object)
            For Each KV In TrendLocationDict
                If Not KV.Value Is Nothing Then
                    Select Case KV.Key
                        Case "woeid"
                            Me.ID = KV.Value.ToString
                        Case "name"
                            Me.LocationName = KV.Value.ToString
                        Case "placeType"
                            Dim PlaceDict As Dictionary(Of String, Object) = CType(KV.Value, Dictionary(Of String, Object))
                            Me.PlaceTypeName = PlaceDict("name").ToString
                            Me.PlaceTypeCode = CLng(PlaceDict("code"))
                        Case "country"
                            Me.CountryName = KV.Value.ToString
                        Case "countryCode"
                            Me.CountryCode = CInt(KV.Value)
                        Case "url"
                            Me.Url = KV.Value.ToString
                    End Select
                End If
            Next
        End Sub
    End Class
End Namespace
