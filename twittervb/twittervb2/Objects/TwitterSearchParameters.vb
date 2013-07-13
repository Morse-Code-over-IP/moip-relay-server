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
'* - Neither the name of Twitter nor the names of its contributors may be 
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
'* Bob Denny    06-Jun-2011     3.1.1.0 - Add ResultType to TwitterSearchParameters.
'*                              Clean leading '+' from SearchString
'* Bob Denny    10-Jun-2011     3.1.1.0 - Update documentation links in comments. Change
'*                              ResultType to an enum instead of a string.
'* Bob Denny    13-Jul-2013     4.0.1.0 - For API 1.1, no more search/query string building here.
'*

Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace TwitterVB2

    ''' <summary>
    ''' The parameters that can be used to define a search. For more information see The <a href="https://dev.twitter.com/docs/api/1.1/get/search/tweets">GET search/tweets</a>
    ''' and <a href="https://dev.twitter.com/docs/using-search">Using the Twitter Search API</a>.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum TwitterSearchParameterNames

        ''' <summary>
        ''' The text for which to search.
        ''' </summary>
        ''' <remarks> This can contain a number of <em>Search Operators</em> which modify the the behavior of the search. 
        ''' See <a href="https://dev.twitter.com/docs/using-search">Using the Twitter Serarch API</a>, Search Operators section.
        ''' NOTE: Some of these are not really parameters, but convenient ways of building the searth term with search operators.
        ''' The <c>from:</c> and <c>to:</c> operators are implemented by the <c>FromUser</c>. <c>NotFromUser</c>, and <c>ToUser</c> 
        ''' parameters here. Any other operators must be explicitly included in the <c>SearchTerm</c> parameter string.</remarks>
        SearchTerm

        ''' <summary>
        ''' Restricts tweets to the given language.
        ''' </summary>
        ''' <remarks>
        ''' The language must be provided as an <a href="http://en.wikipedia.org/wiki/ISO_639-1">ISO 639-1 code</a>
        ''' </remarks>
        Lang

        ''' <summary>
        ''' Specify the language of the query you are sending (only ja is currently effective). This is intended for language-specific clients and the default should work in the majority of cases.
        ''' </summary>
        ''' <remarks></remarks>
        Locale

        ''' <summary>
        ''' The number of tweets to return per page, up to a max of 100.
        ''' </summary>
        ''' <remarks></remarks>
        Count

        ''' <summary>
        ''' The page number (starting at 1) to return, up to a max of roughly 1500 results (based on rpp).
        ''' </summary>
        ''' <remarks>
        ''' There are <a href="http://dev.twitter.com/pages/every_developer#there-are-pagination-limits">pagination limits.</a>
        ''' </remarks>
        Page

        ''' <summary>
        ''' Returns tweets with status ids greater than the given id.
        ''' </summary>
        ''' <remarks></remarks>
        SinceID

        ''' <summary>
        ''' Returns tweets with status ids less than or equal to the given id.
        ''' </summary>
        ''' <remarks></remarks>
        MaxID

        ''' <summary>
        ''' Returns tweets before the given date.
        ''' </summary>
        ''' <remarks></remarks>
        Until

        ''' <summary>
        ''' Specify the result type: <c>Mixed</c>, <c>Recent</c>, or <c>Popular</c>.
        ''' </summary>
        ''' <remarks></remarks>
        ResultType


    End Enum

    ''' <summary>
    ''' A class that defines a search.
    ''' </summary>
    ''' <remarks>
    ''' Most search tasks can be accomplished by just calling the <c>Search</c> method of the <c>SearchMethods</c> object and passing it the
    ''' text you are searching for.  If you're looking for more control over your search results, you'll want to pass a <c>TwitterSearchParameters</c> object.
    ''' <para/>
    ''' You define a search by creating an instance of this class, and adding parameters to it.  At the least, you must define the <c>SearchTerm</c>.  See the example below.
    ''' <code source="TwitterVB2\examples.vb" region="Advanced Search" lang="vbnet" title="Using TwitterSearchParameters"></code>
    ''' </remarks>
    Public Class TwitterSearchParameters
        Inherits Dictionary(Of TwitterSearchParameterNames, Object)

        ''' <summary>
        ''' Default constructor
        ''' </summary>
        ''' <remarks></remarks>
        ''' <exclude/>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Builds the search Url
        ''' </summary>
        ''' <param name="Url">The base url that will be used to build the complete Url.</param>
        ''' <returns>A <c>String</c> containing the complete Url.</returns>
        ''' <remarks></remarks>
        ''' <exclude/>
        Public Function BuildUrl(ByVal Url As String) As String
            If Count = 0 Then
                Return Url
            End If

            Dim ParameterString As String = String.Empty
            Dim SearchTermString As String = String.Empty

            For Each Key As TwitterSearchParameterNames In Keys
                Select Case Key
                    Case TwitterSearchParameterNames.SearchTerm
                        ParameterString = String.Format("{0}&q={1}", ParameterString, System.Uri.EscapeDataString(MyBase.Item(Key).ToString))
                    Case TwitterSearchParameterNames.Lang
                        ParameterString = String.Format("{0}&lang={1}", ParameterString, MyBase.Item(Key).ToString)
                    Case TwitterSearchParameterNames.Locale
                        ParameterString = String.Format("{0}&locale={1}", ParameterString, MyBase.Item(Key).ToString)
                    Case TwitterSearchParameterNames.ResultType
                        ParameterString = String.Format("{0}&result_type={1}", ParameterString, MyBase.Item(Key).ToString)
                    Case TwitterSearchParameterNames.Count
                        ParameterString = String.Format("{0}&count={1}", ParameterString, MyBase.Item(Key).ToString)
                    Case TwitterSearchParameterNames.Until
                        ParameterString = String.Format("{0}&until={1}", ParameterString, MyBase.Item(Key).ToString)
                    Case TwitterSearchParameterNames.SinceID
                        ParameterString = String.Format("{0}&since_id={1}", ParameterString, MyBase.Item(Key).ToString)
                    Case TwitterSearchParameterNames.MaxID
                        ParameterString = String.Format("{0}&max_id={1}", ParameterString, MyBase.Item(Key).ToString)
                    Case TwitterSearchParameterNames.Page
                        ParameterString = String.Format("{0}&page={1}", ParameterString, MyBase.Item(Key).ToString)

                End Select
            Next

            If String.IsNullOrEmpty(ParameterString) Then
                Return Url
            End If

            ' First char of parameterString is a leading & that should be removed
            Return String.Format("{0}?{1}", Url, ParameterString.Remove(0, 1))

        End Function

        ''' <summary>
        ''' Adds a parameter to the collection.
        ''' </summary>
        ''' <param name="Key">The name of the parameter being added.</param>
        ''' <param name="Value">The value to be assigned to the parameter.</param>
        ''' <remarks></remarks>
        Public Shadows Sub Add(ByVal Key As TwitterSearchParameterNames, ByVal Value As Object)

            If MyBase.ContainsKey(Key) Then
                Throw New ApplicationException("This Parameter already exist.")
                Exit Sub
            End If

            If Key = TwitterSearchParameterNames.ResultType Then
                If Not (TypeOf Value Is ResultType) Then
                    Throw New ApplicationException("Value given for result type was not a ResultType.")
                End If
                MyBase.Add(Key, Value.ToString.ToLower)     ' lower case!
            Else
                MyBase.Add(Key, Value.ToString)
            End If

        End Sub
    End Class

End Namespace