Imports System.Text
Imports System
Imports System.Security.Cryptography
Imports System.Collections.Specialized
Imports System.IO
Imports System.Web
Imports System.Net

Namespace TwitterVB2
    Public Class ATwitterxAuth
        Inherits TwitterOAuth
        Public Const X_AUTHENTICATE As String = "https://api.twitter.com/oauth/access_token"
        Protected Const xAuthUserNamePrefix As String = "x_auth_username"
        Protected Const xAuthUserPasswordPrefix As String = "x_auth_password"
        Protected Const xAuthUserModePrefix As String = "x_auth_mode"

        'note, in order to keep with Twitter's TOS, as soon as we get the tokens back we need to delete these out

        Public xUserName As String = String.Empty
        Public xUserPass As String = String.Empty


        Public Sub New()

        End Sub

        Public Sub New(ByVal p_ConsumerKey As String, ByVal p_ConsumerKeySecret As String)
            Me.ConsumerKey = p_ConsumerKey
            Me.ConsumerSecret = p_ConsumerKeySecret

        End Sub

        Public Sub New(ByVal p_ConsumerKey As String, ByVal p_ConsumerSecret As String, ByVal p_UserName As String, ByVal p_Password As String)
            Me.ConsumerKey = p_ConsumerKey
            Me.ConsumerSecret = p_ConsumerSecret
            Me.xUserName = p_UserName
            Me.xUserPass = p_Password
        End Sub

        Public Overloads Sub GetXAccess()
            Call Me.GetXAccess(Me.xUserName, Me.xUserPass)



        End Sub

        Public Overloads Sub GetXAccess(ByVal p_strUserName As String, ByVal p_strPassword As String)
            Dim Response As String = OAuthWebRequest(Method.POST, X_AUTHENTICATE, "?x_auth_username=" & OAuthUrlEncode(p_strUserName) & "&x_auth_password=" & OAuthUrlEncode(p_strPassword) & "&x_auth_mode=client_mode")
            If Response.Length > 0 Then
                Dim qs As NameValueCollection = HttpUtility.ParseQueryString(Response)
                If qs("oauth_token") IsNot Nothing Then
                    Token = qs("oauth_token")
                End If
                If qs("oauth_token_secret") IsNot Nothing Then
                    TokenSecret = qs("oauth_token_secret")
                End If
            End If


        End Sub




    End Class
End Namespace
