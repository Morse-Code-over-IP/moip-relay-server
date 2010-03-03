'   Copyright © 2009 Les Kerr

'   This file is part of MorseKOB.

'   MorseKOB is free software: you can redistribute it and/or modify
'   it under the terms of the GNU General Public License as published by
'   the Free Software Foundation, either version 3 of the License, or
'   (at your option) any later version.

'   MorseKOB is distributed in the hope that it will be useful,
'   but WITHOUT ANY WARRANTY; without even the implied warranty of
'   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
'   GNU General Public License for more details.

'   You should have received a copy of the GNU General Public License
'   along with MorseKOB.  If not, see <http://www.gnu.org/licenses/>.

'   For more information about MorseKOB see <http://morsekob.org>.

Public Class KOBServer
    Private Const stnMax = 100  ' Maximum number of concurrently connected stations
    Private Const MIN_VERSION = "2.5"  ' Oldest KOB version supported

    Private Structure stnRecord
        Dim ep As System.Net.IPEndPoint  ' IP address and port
        Dim wn As Integer  ' Wire number
        Dim id As String  ' Station ID
        Dim vn As String  ' MorseKOB client version
        Dim dt As DateTime  ' Time last packet received
    End Structure

    Private stnList(stnMax) As stnRecord
    Private nStns As Integer = 0
    Private curStn As Integer
    Private Shared listChanged As Boolean = True
    Private Shared errMsg As String = ""
    Private nConnections As Integer = 0
    Private startTime As DateTime = Now
    Private udpSocket As System.Net.Sockets.UdpClient
    Private endPoint As System.Net.IPEndPoint
    Private bytBuffer() As Byte
    Private thdIOLoop As New System.Threading.Thread(AddressOf IOLoop)

    Private Sub KOBServer_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        With My.Application.Info.Version
            Me.Text = ProductName & " " & .Major & "." & .Minor
        End With
        udpSocket = New System.Net.Sockets.UdpClient(7890)
        thdIOLoop.IsBackground = True
        thdIOLoop.Priority = Threading.ThreadPriority.AboveNormal
        thdIOLoop.Start()
        Dbg(Me.Text & "." & My.Application.Info.Version.Revision & " restarted")
    End Sub

    Private Sub IOLoop()
        Do
            Try
                bytBuffer = udpSocket.Receive(endPoint)
                ProcessPacket()
            Catch ex As Exception
                Dbg("KOBServer error. " & ex.Message)
            End Try
        Loop
    End Sub

    Sub ProcessPacket()
        Dim bufLen = bytBuffer.GetLength(0)
        Select Case bufLen
            Case 4
                Dim cmd As Integer = UnpackShort(bytBuffer, 0)
                Select Case cmd
                    Case 2  ' Disconnect
                        RemoveStation()
                    Case 4  ' Connect
                        SendAck()
                        UpdateWireNo(UnpackShort(bytBuffer, 2))
                    Case Else
                        Dbg("Unexpected short record command type: " & cmd.ToString)
                End Select
            Case 496
                Dim cmd = UnpackShort(bytBuffer, 0)
                Select Case cmd
                    Case 3  ' ID or Code
                        UpdateID()
                        BroadcastPacket()
                    Case Else
                        Dbg("Unexpected long record command type: " & cmd.ToString)
                End Select
            Case Else
                Dbg("Unexpected packet length: " & bufLen.ToString & " bytes")
        End Select
    End Sub

    Private Sub RemoveStation()
        For i As Integer = 1 To nStns
            If stnList(i).ep.Equals(endPoint) Then
                For j As Integer = i To nStns - 1
                    stnList(j) = stnList(j + 1)
                Next
                nStns -= 1
                listChanged = True
                Exit For
            End If
        Next
    End Sub

    Private Sub SendAck()
        Dim ack As Byte() = {5, 0}
        Try
            udpSocket.Send(ack, 2, endPoint)
        Catch ex As Exception
            Dbg("Can't send Ack. " & ex.Message)
        End Try
    End Sub

    Private Sub UpdateWireNo(ByVal w As Integer)
        Dim i As Integer = nStns
        Do While i > 0
            If stnList(i).ep.Equals(endPoint) Then
                If stnList(i).wn = w Then
                    stnList(i).dt = Now
                Else
                    For j As Integer = i To nStns - 1
                        stnList(j) = stnList(j + 1)
                    Next
                    nStns -= 1
                    i = 0
                End If
                Exit Do
            End If
            i -= 1
        Loop
        If i = 0 Then
            Do While i <= nStns AndAlso stnList(i).wn < w
                i += 1
            Loop
            If nStns < stnMax Then
                For j As Integer = nStns To i Step -1
                    stnList(j + 1) = stnList(j)
                Next
                nStns += 1
                stnList(i).ep = endPoint
                stnList(i).wn = w
                stnList(i).id = ""
                stnList(i).vn = ""
                stnList(i).dt = Now
                nConnections += 1
                listChanged = True
            Else
                Dbg("Exceeded station limit")
            End If
        End If
    End Sub

    Private Sub UpdateID()
        For i As Integer = 1 To nStns
            If stnList(i).ep.Equals(endPoint) Then
                Dim id As String = UnpackString(bytBuffer, 4)
                Dim vn As String = UnpackString(bytBuffer, 360)
                If (Mid(vn, 1, 9) <> "MorseKOB " Or Mid(vn, 10, 3) < MIN_VERSION) Then
                    ' reject connections from invalid clients
                    PackString(bytBuffer, 4, _
                            "Versions older than MorseKOB " & MIN_VERSION & " are not supported. " _
                            & "Upgrade at www.morsekob.org.")
                    Try
                        udpSocket.Send(bytBuffer, 496, endPoint)
                        'Dbg("Old version rejected")
                    Catch ex As Exception
                        Dbg("Old version rejected, but can't send warning message. " & ex.Message)
                    End Try
                    PackString(bytBuffer, 4, id)
                ElseIf stnList(i).id <> id Then
                    stnList(i).id = id
                    stnList(i).vn = vn
                    Dbg("""" & stnList(i).id & """ on wire #" & stnList(i).wn _
                            & " from " & stnList(i).ep.ToString _
                            & " with " & stnList(i).vn)
                    listChanged = True
                End If
                Exit For
            End If
        Next
    End Sub

    Private Sub BroadcastPacket()
        For i As Integer = 1 To nStns
            If stnList(i).ep.Equals(endPoint) And stnList(i).vn <> "" Then
                Dim w As Integer = stnList(i).wn
                For j As Integer = 1 To nStns
                    If j = i Then
                        stnList(j).dt = Now
                    ElseIf stnList(j).wn = w And stnList(j).vn <> "" Then
                        Try
                            udpSocket.Send(bytBuffer, 496, stnList(j).ep)
                        Catch ex As Exception
                            Dbg("Can't send long packet. " & ex.Message)
                        End Try
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub PurgeStations()
        Dim i As Integer = 1
        Do While i <= nStns
            If stnList(i).dt.AddSeconds(60).CompareTo(Now) < 0 Then
                For j As Integer = i To nStns - 1
                    stnList(j) = stnList(j + 1)
                Next
                nStns -= 1
                listChanged = True
            Else
                i += 1
            End If
        Loop
    End Sub

    Sub Dbg(ByVal str As String)
        errMsg &= str & "<br>" & vbCrLf
    End Sub

    Private Sub UpdateLogFile()
        txtErrMsg.Text = errMsg
        If errMsg <> "" Then
            Const Append = True
            Dim sw As New System.IO.StreamWriter(txtWebFolder.Text & "\log.html", Append)
            sw.WriteLine("[" & Now.ToString & "] " & errMsg)
            sw.Close()
        End If
        errMsg = ""
    End Sub

    Private Sub UpdateWebPage()
        If listChanged Then
            Try
                Dim sw As New System.IO.StreamWriter(txtWebFolder.Text & "\index.html")
                sw.WriteLine("<html>")
                sw.WriteLine("<head>")
                sw.WriteLine("<title>MorseKOB Current Users</title>")
                sw.WriteLine("<meta http-equiv=""content-type"" content=""text/html; charset=ISO-8859-1"">")
                sw.WriteLine("<meta http-equiv=""pragma"" content=""no-cache"">")
                sw.WriteLine("<meta name=""authors"" content=""Ted Wagner, Les Kerr"">")
                sw.WriteLine("<meta http-equiv=""refresh"" content=""60"">")
                sw.WriteLine("</head>")
                sw.WriteLine("<body>")
                sw.WriteLine("<center>")
                sw.WriteLine("<p><font face=""Times New Roman"" size=""6"">" & Me.Text & "</font></p>")
                'sw.WriteLine("Powered by Ionosphere, courtesy of <A HREF=""http://www.mrx.com.au/"">MRX Software</A>")
                sw.WriteLine("<p><font face=""Times New Roman"" size=""4"">Users Currently on the Wire</font>")
                sw.WriteLine("<table cellspacing=""2"" border=""4"" cellpadding=""4"" align=""center"">")
                sw.WriteLine("  <tr><td align=""center""><b>Wire No.</b></td><td align=""center""><b>ID</b></td></tr>")
                For i As Integer = 1 To nStns
                    sw.WriteLine("  <tr><td align=""center"">" & stnList(i).wn.ToString & "</td><td>" & stnList(i).id.ToString & "</td></tr>")
                Next
                sw.WriteLine("</table>")
                sw.WriteLine("<!--  <center>")
                sw.WriteLine("  <script language=""javascript"">")
                sw.WriteLine("  document.write(Date())")
                sw.WriteLine("  </script>")
                sw.WriteLine("  </center> -->")
                sw.WriteLine(nConnections.ToString & " connections since " & startTime.ToString & "<br>")
                sw.WriteLine("Click this link for this server's <a href=""../info/"">information</a></p>")
                sw.WriteLine("</center>")
                sw.WriteLine("</body>")
                sw.WriteLine("</html>")
                sw.Close()
                listChanged = False
            Catch ex As Exception
                Dbg("Can't update web page. " & ex.Message)
                Exit Sub
            End Try
        End If
    End Sub

    Private Function UnpackShort(ByRef buf() As Byte, ByVal i As Integer) As Short
        Dim x As Short = 0
        For j As Integer = 1 To 2
            x <<= 8
            x += buf(i + 2 - j)
        Next
        UnpackShort = x
    End Function

    Private Function UnpackInteger(ByRef buf() As Byte, ByVal i As Integer) As Integer
        Dim x As Integer = 0
        For j As Integer = 1 To 4
            x <<= 8
            x += buf(i + 4 - j)
        Next
        UnpackInteger = x
    End Function

    Private Function UnpackString(ByRef buf() As Byte, ByVal i As Integer) As String
        Dim s As String = ""
        For j As Integer = 1 To 127
            If buf(i + j - 1) = 0 Then Exit For
            s &= Chr(buf(i + j - 1))
        Next
        UnpackString = s
    End Function

    Private Sub PackString(ByRef buf() As Byte, ByVal i As Integer, ByVal s As String)
        For j As Integer = 1 To Len(s)
            buf(i + j - 1) = Asc(Mid(s, j, 1))
        Next
        buf(i + Len(s)) = 0
    End Sub

    Private Sub tmrUpdate_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmrUpdate.Tick
        PurgeStations()
        UpdateWebPage()
        UpdateLogFile()
    End Sub
End Class
