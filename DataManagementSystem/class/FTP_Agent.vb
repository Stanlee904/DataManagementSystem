Imports System.Diagnostics
Imports System.Data
Imports System.Collections
Imports System.Collections.Generic
Imports System.Net
Imports System.IO
Imports System.Text.RegularExpressions


Public Class FTP_Agent

    Private m_sLastDirectory As String = ""
    Private m_sHostName As String
    Private m_sUserName As String
    Private m_sPassword As String
    Private m_sCurrentDirectory As String = "/"

    Public Sub New()
    End Sub

    Public Sub New(sHostname As String)
        m_sHostName = sHostname
    End Sub

    Public Sub New(sHostname As String, sUsername As String, sPassword As String)
        m_sHostName = sHostname
        m_sUserName = sUsername
        m_sPassword = sPassword
    End Sub

    Public Function ListDirectory(sDirectory As String) As List(Of String)
        Dim FTP As System.Net.FtpWebRequest = GetRequest(GetDirectory(sDirectory))
        FTP.Method = System.Net.WebRequestMethods.Ftp.ListDirectory

        Dim str As String = GetStringResponse(FTP)
        str = str.Replace(vbCr & vbLf, vbCr).TrimEnd(ControlChars.Cr)

        Dim result As New List(Of String)()
        result.AddRange(str.Split(ControlChars.Cr))
        Return result
    End Function

    Public Function ListDirectoryDetail(sDirectory As String) As FTP_Dir
        Dim FTP As System.Net.FtpWebRequest = GetRequest(GetDirectory(sDirectory))
        FTP.Method = System.Net.WebRequestMethods.Ftp.ListDirectoryDetails

        Dim str As String = GetStringResponse(FTP)
        str = str.Replace(vbCr & vbLf, vbCr).TrimEnd(ControlChars.Cr)
        Return New FTP_Dir(str, m_sLastDirectory)
    End Function

    Public Function Upload(sLocalFileName As String, sTargetFileName As String) As Boolean
        If Not File.Exists(sLocalFileName) Then
            Throw (New ApplicationException("File " & sLocalFileName & " not found"))
        End If

        Dim fi As New FileInfo(sLocalFileName)
        Return Upload(fi, sTargetFileName)
    End Function

    Public Function Upload(fi As FileInfo, sTargetFileName As String) As Boolean
        Dim target As String
        If sTargetFileName.Trim() = "" Then
            target = Me.CurrentDirectory & fi.Name
        ElseIf sTargetFileName.Contains("/") Then
            target = AdjustDir(sTargetFileName)
        Else
            target = CurrentDirectory & sTargetFileName
        End If

        Dim URI As String = Hostname & target
        Dim FTP As System.Net.FtpWebRequest = GetRequest(URI)

        FTP.Method = System.Net.WebRequestMethods.Ftp.UploadFile
        FTP.UseBinary = True

        FTP.ContentLength = fi.Length

        Const BufferSize As Integer = 2048
        Dim content As Byte() = New Byte(BufferSize - 1) {}
        Dim dataRead As Integer

        Using fs As FileStream = fi.OpenRead()
            Try
                Using rs As Stream = FTP.GetRequestStream()
                    Do
                        dataRead = fs.Read(content, 0, BufferSize)
                        rs.Write(content, 0, dataRead)
                    Loop While Not (dataRead < BufferSize)
                    rs.Close()

                End Using

            Catch generatedExceptionName As Exception
            Finally
                fs.Close()

            End Try
        End Using


        FTP = Nothing
        Return True

    End Function

    ''' <summary>
    ''' 파일 업로드: 임시 파일명으로 파일 올린 후 RENAME 방식
    ''' </summary>
    ''' <param name="sLocalFileName"></param>
    ''' <param name="sTargetFileName"></param>
    ''' <param name="bUseBinary"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Upload(sLocalFileName As String, sTargetFileName As String, bUseBinary As Boolean) As Boolean
        Dim bRtn As Boolean = True

        '로컬에 파일 존재하는지 Check
        If Not File.Exists(sLocalFileName) Then
            'Throw (New ApplicationException("File " & sLocalFileName & " not found"))
            Return bRtn = False
        End If

        Dim fi As New FileInfo(sLocalFileName)
        Dim sTarget As String
        'Dim sTmpTarget As String = "/NICEfiling_" & Now.ToString("yyyyMMddHHmmssfffffff")
        Dim sTmpTarget As String
        Dim URI As String
        Dim tmpURI As String
        Dim FTP As System.Net.FtpWebRequest

        'FTP 경로에 맞게 Setting
        If sTargetFileName.Trim() = "" Then
            sTarget = Me.CurrentDirectory & fi.Name
        ElseIf sTargetFileName.Contains("/") Then
            sTarget = AdjustDir(sTargetFileName)
        Else
            sTarget = CurrentDirectory & sTargetFileName
        End If

        Try
            sTmpTarget = sTarget & "." & Now.ToString("HHmmssff")
            URI = Hostname & sTarget
            tmpURI = Hostname & sTmpTarget

            FTP = GetRequest(tmpURI)   'FTP 접속

            FTP.Method = System.Net.WebRequestMethods.Ftp.UploadFile
            FTP.Timeout = 60000         '60초로 제한
            FTP.UseBinary = bUseBinary
            FTP.ContentLength = fi.Length
            FTP.UsePassive = False

            Const BufferSize As Integer = 2048
            Dim content As Byte() = New Byte(BufferSize - 1) {}
            Dim dataRead As Integer

            'File Upload
            Using fs As FileStream = fi.OpenRead()
                Try
                    Using rs As Stream = FTP.GetRequestStream()
                        Do
                            dataRead = fs.Read(content, 0, BufferSize)
                            rs.Write(content, 0, dataRead)
                        Loop While Not (dataRead < BufferSize)
                        rs.Close()
                    End Using
                Catch generatedExceptionName As Exception
                    bRtn = False
                Finally
                    fs.Close()
                End Try
            End Using

            Try
                If FtpFileExists(sTarget) Then  '기존 파일이 있을 경우 삭제 후 rename
                    If Not FtpDelete(sTarget) Then
                        bRtn = False    '삭제 중 실패
                    End If
                End If
                bRtn = FtpRename(sTmpTarget, sTarget)
            Catch ex As Exception
                bRtn = False
            End Try

            'FTP = Nothing
        Catch ex As Exception
            bRtn = False
        Finally
            FTP = Nothing
        End Try

        Return bRtn
    End Function

    Public Function Download(sSourceFileName As String, sLocalFileName As String, bOverwrite As Boolean) As Boolean
        Try
            'PSJ[2018-06-15] 기존 파일 존재할 경우 rename 후 다운받게 수정
            Dim refi As New FileInfo(sLocalFileName)
            If refi.Exists Then
                refi.MoveTo(refi.FullName & "." & Now.ToString("HHmmssff"))
            End If

            Dim fi As New FileInfo(sLocalFileName)
            Return Me.Download(sSourceFileName, fi, bOverwrite)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function Download(file As FTP_File, sLocalFileName As String, bOverwrite As Boolean) As Boolean
        Return Me.Download(file.FullName, sLocalFileName, bOverwrite)
    End Function

    Public Function Download(file As FTP_File, localFI As FileInfo, bOverwrite As Boolean) As Boolean
        Return Me.Download(file.FullName, localFI, bOverwrite)
    End Function

    Public Function Download(sSourceFileName As String, targetFI As FileInfo, bOverwrite As Boolean) As Boolean
        Try
            If targetFI.Exists AndAlso Not (bOverwrite) Then
                Throw (New ApplicationException("Target file already exists"))
            End If

            Dim target As String
            If sSourceFileName.Trim() = "" Then
                Throw (New ApplicationException("File not specified"))
            ElseIf sSourceFileName.Contains("/") Then
                target = AdjustDir(sSourceFileName)
            Else
                target = CurrentDirectory & sSourceFileName
            End If

            Dim URI As String = Hostname & target

            Dim FTP As System.Net.FtpWebRequest = GetRequest(URI)
            FTP.Method = System.Net.WebRequestMethods.Ftp.DownloadFile
            FTP.UseBinary = True

            Using response As FtpWebResponse = DirectCast(FTP.GetResponse(), FtpWebResponse)
                Using responseStream As Stream = response.GetResponseStream()
                    Using fs As FileStream = targetFI.OpenWrite()
                        Try
                            Dim buffer As Byte() = New Byte(2047) {}
                            Dim read As Integer = 0
                            Do
                                read = responseStream.Read(buffer, 0, buffer.Length)
                                fs.Write(buffer, 0, read)
                            Loop While Not (read = 0)
                            responseStream.Close()
                            fs.Flush()
                            fs.Close()
                        Catch generatedExceptionName As Exception
                            fs.Close()
                            targetFI.Delete()
                            Throw
                        End Try
                    End Using

                    responseStream.Close()
                End Using

                response.Close()
            End Using

            Return True
        Catch ex As WebException
            Throw ex
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function FtpDelete(sFileName As String) As Boolean
        Dim URI As String = Me.Hostname & GetFullPath(sFileName)

        Dim FTP As System.Net.FtpWebRequest = GetRequest(URI)
        FTP.Method = System.Net.WebRequestMethods.Ftp.DeleteFile
        Try
            Dim str As String = GetStringResponse(FTP)
        Catch generatedExceptionName As Exception
            Return False
        End Try
        Return True
    End Function

    ' FTP 내에 파일이 존재하는지 유무 확인 
    Public Function FtpFileExists(sFileName As String) As Boolean
        Try
            Dim size As Long = GetFileSize(sFileName)

            Return True
        Catch ex As Exception
            If TypeOf ex Is System.Net.WebException Then
                If ex.Message.Contains("550") Then
                    Return False
                Else
                    Throw
                End If
            Else
                Throw
            End If
        End Try
    End Function

    ' 해당 파일의 크기 반환
    Public Function GetFileSize(sFileName As String) As Long
        Dim sPath As String
        If sFileName.Contains("/") Then
            sPath = AdjustDir(sFileName)
        Else
            sPath = Me.CurrentDirectory & sFileName ' 현재 디렉토리+sFileName을 sPath로 초기화
        End If
        Dim URI As String = Me.Hostname & sPath
        Dim FTP As System.Net.FtpWebRequest = GetRequest(URI)
        FTP.Method = System.Net.WebRequestMethods.Ftp.GetFileSize
        Dim tmp As String = Me.GetStringResponse(FTP)
        Return GetSize(FTP)
    End Function

    Public Function FtpRename(sSourceFileName As String, sNewName As String) As Boolean
        Dim sSource As String = GetFullPath(sSourceFileName)
        If Not FtpFileExists(sSource) Then
            Throw (New FileNotFoundException("File " & sSource & " not found"))
        End If

        Dim sTarget As String = GetFullPath(sNewName)
        If sTarget = sSource Then
            Throw (New ApplicationException("Source and target are the same"))
        ElseIf FtpFileExists(sTarget) Then
            Throw (New ApplicationException("Target file " & sTarget & " already exists"))
        End If

        Dim URI As String = Me.Hostname & sSource

        Dim FTP As System.Net.FtpWebRequest = GetRequest(URI)
        FTP.Method = System.Net.WebRequestMethods.Ftp.Rename

        '파일명만 입력해야 rename됨
        'FTP.RenameTo = "." & sTarget
        FTP.RenameTo = sTarget.Substring(sTarget.LastIndexOf("/") + 1)

        Try
            Dim str As String = GetStringResponse(FTP)
        Catch generatedExceptionName As Exception
            Return False
        End Try
        Return True
    End Function

    Public Function FtpCreateDirectory(sDirPath As String) As Boolean
        Dim URI As String = Me.Hostname & AdjustDir(sDirPath)
        Dim FTP As System.Net.FtpWebRequest = GetRequest(URI)
        FTP.Method = System.Net.WebRequestMethods.Ftp.MakeDirectory

        Try
            Dim str As String = GetStringResponse(FTP)
        Catch generatedExceptionName As Exception
            Return False
        End Try
        Return True
    End Function

    Public Function FtpDeleteDirectory(sDirPath As String) As Boolean
        Dim URI As String = Me.Hostname & AdjustDir(sDirPath)
        Dim FTP As System.Net.FtpWebRequest = GetRequest(URI)
        FTP.Method = System.Net.WebRequestMethods.Ftp.RemoveDirectory

        Try
            Dim str As String = GetStringResponse(FTP)
        Catch generatedExceptionName As Exception
            Return False
        End Try
        Return True
    End Function

    Private Function GetRequest(URI As String) As FtpWebRequest
        Dim result As FtpWebRequest = DirectCast(FtpWebRequest.Create(URI), FtpWebRequest)
        result.Credentials = GetCredentials()
        result.KeepAlive = False

        Return result
    End Function

    Private Function GetCredentials() As System.Net.ICredentials
        Return New System.Net.NetworkCredential(Username, Password)
    End Function

    Private Function GetFullPath(sFileName As String) As String
        If sFileName.Contains("/") Then
            Return AdjustDir(sFileName)
        Else
            Return Me.CurrentDirectory & sFileName
        End If
    End Function

    '파일 내에 . 또는 / 부분 조정 
    Private Function AdjustDir(sPath As String) As String
        sPath = If(sPath.StartsWith("."), sPath.Remove(0, 1), sPath)
        sPath = (If((sPath.StartsWith("/")), "", "/")).ToString() & sPath
        Return sPath
    End Function

    Private Function GetDirectory(sDirectory As String) As String
        Dim URI As String
        If sDirectory = "" Then
            URI = Hostname & Me.CurrentDirectory
            m_sLastDirectory = Me.CurrentDirectory
        Else
            If Not sDirectory.StartsWith("/") Then
                Throw (New ApplicationException("Directory should start with /"))
            End If
            URI = Me.Hostname & sDirectory
            m_sLastDirectory = sDirectory
        End If
        Return URI
    End Function

    Private Function GetStringResponse(FTP As FtpWebRequest) As String
        Dim result As String = ""
        Using response As FtpWebResponse = DirectCast(FTP.GetResponse(), FtpWebResponse)
            Dim size As Long = response.ContentLength
            Using datastream As Stream = response.GetResponseStream()
                Using sr As New StreamReader(datastream)
                    result = sr.ReadToEnd()
                    sr.Close()
                End Using

                datastream.Close()
            End Using

            response.Close()
        End Using

        Return result
    End Function

    Private Function GetSize(FTP As FtpWebRequest) As Long
        Dim size As Long
        Using response As FtpWebResponse = DirectCast(FTP.GetResponse(), FtpWebResponse)
            size = response.ContentLength
            response.Close()
        End Using

        Return size
    End Function

    Public Property Hostname() As String
        Get
            If m_sHostName.StartsWith("ftp://") Then
                Return m_sHostName
            Else
                Return "ftp://" & m_sHostName
            End If
        End Get
        Set(value As String)
            m_sHostName = value
        End Set
    End Property

    Public Property Username() As String
        Get
            Return (If(m_sUserName = "", "anonymous", m_sUserName))
        End Get
        Set(value As String)
            m_sUserName = value
        End Set
    End Property

    Public Property Password() As String
        Get
            Return m_sPassword
        End Get
        Set(value As String)
            m_sPassword = value
        End Set
    End Property

    Public Property CurrentDirectory() As String
        Get
            Return m_sCurrentDirectory & (If((m_sCurrentDirectory.EndsWith("/")), "", "/")).ToString()
        End Get
        Set(value As String)
            If Not value.StartsWith("/") Then
                Throw (New ApplicationException("Directory should start with /"))
            End If
            m_sCurrentDirectory = value
        End Set
    End Property




End Class
Public Class FTP_File

    Private m_sFileName As String
    Private m_sPath As String
    Private m_FileType As DirectoryEntryTypes
    Private m_iSize As Long
    Private m_dtFileDateTime As DateTime
    Private m_sPermission As String
    Private Shared m_sParseFormats As String() = New String() {"(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\w+\s+\w+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{4})\s+(?<name>.+)", "(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\d+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{4})\s+(?<name>.+)", "(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\d+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{1,2}:\d{2})\s+(?<name>.+)", "(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})\s+\d+\s+\w+\s+\w+\s+(?<size>\d+)\s+(?<timestamp>\w+\s+\d+\s+\d{1,2}:\d{2})\s+(?<name>.+)", "(?<dir>[\-d])(?<permission>([\-r][\-w][\-xs]){3})(\s+)(?<size>(\d+))(\s+)(?<ctbit>(\w+\s\w+))(\s+)(?<size2>(\d+))\s+(?<timestamp>\w+\s+\d+\s+\d{2}:\d{2})\s+(?<name>.+)", "(?<timestamp>\d{2}\-\d{2}\-\d{2}\s+\d{2}:\d{2}[Aa|Pp][mM])\s+(?<dir>\<\w+\>){0,1}(?<size>\d+){0,1}\s+(?<name>.+)"}

    Public ReadOnly Property FullName() As String
        Get
            Return Path & Filename
        End Get
    End Property
    Public ReadOnly Property Filename() As String
        Get
            Return m_sFileName
        End Get
    End Property
    Public ReadOnly Property Path() As String
        Get
            Return m_sPath
        End Get
    End Property
    Public ReadOnly Property FileType() As DirectoryEntryTypes
        Get
            Return m_FileType
        End Get
    End Property
    Public ReadOnly Property Size() As Long
        Get
            Return m_iSize
        End Get
    End Property
    Public ReadOnly Property FileDateTime() As DateTime
        Get
            Return m_dtFileDateTime
        End Get
    End Property
    Public ReadOnly Property Permission() As String
        Get
            Return m_sPermission
        End Get
    End Property
    Public ReadOnly Property Extension() As String
        Get
            Dim i As Integer = Me.Filename.LastIndexOf(".")
            If i >= 0 AndAlso i < (Me.Filename.Length - 1) Then
                Return Me.Filename.Substring(i + 1)
            Else
                Return ""
            End If
        End Get
    End Property
    Public ReadOnly Property NameOnly() As String
        Get
            Dim i As Integer = Me.Filename.LastIndexOf(".")
            If i > 0 Then
                Return Me.Filename.Substring(0, i)
            Else
                Return Me.Filename
            End If
        End Get
    End Property

    Public Enum DirectoryEntryTypes
        File
        Directory
    End Enum

    Public Sub New(line As String, sPath As String)
        Dim m As Match = GetMatchingRegex(line)
        If m Is Nothing Then
            Throw (New ApplicationException("Unable to parse line: " & line))
        Else
            m_sFileName = m.Groups("name").Value
            m_sPath = sPath

            Int64.TryParse(m.Groups("size").Value, m_iSize)

            m_sPermission = m.Groups("permission").Value
            Dim _dir As String = m.Groups("dir").Value
            If _dir <> "" AndAlso _dir <> "-" Then
                m_FileType = DirectoryEntryTypes.Directory
            Else
                m_FileType = DirectoryEntryTypes.File
            End If

            Try
                m_dtFileDateTime = DateTime.Parse(m.Groups("timestamp").Value)
            Catch generatedExceptionName As Exception
                m_dtFileDateTime = Nothing
            End Try
        End If
    End Sub

    Private Function GetMatchingRegex(line As String) As Match
        Dim rx As Regex
        Dim m As Match
        For i As Integer = 0 To m_sParseFormats.Length - 1
            rx = New Regex(m_sParseFormats(i))
            m = rx.Match(line)
            If m.Success Then
                Return m
            End If
        Next
        Return Nothing
    End Function
End Class

Public Class FTP_Dir
    Inherits List(Of FTP_File)

    Private Const SLASH As Char = "/"c

    Public Sub New()
    End Sub

    Public Sub New(dir As String, sPath As String)
        For Each line As String In dir.Replace(vbLf, "").Split(System.Convert.ToChar(ControlChars.Cr))
            If line <> "" Then
                Me.Add(New FTP_File(line, sPath))
            End If
        Next
    End Sub

    Public Function GetFiles(ext As String) As FTP_Dir
        Return Me.GetFileOrDir(FTP_File.DirectoryEntryTypes.File, ext)
    End Function

    Public Function GetDirectories() As FTP_Dir
        Return Me.GetFileOrDir(FTP_File.DirectoryEntryTypes.Directory, "")
    End Function

    Private Function GetFileOrDir(type As FTP_File.DirectoryEntryTypes, ext As String) As FTP_Dir
        Dim result As New FTP_Dir()
        For Each fi As FTP_File In Me
            If fi.FileType = type Then
                If ext = "" Then
                    result.Add(fi)
                ElseIf ext = fi.Extension Then
                    result.Add(fi)
                End If
            End If
        Next

        Return result
    End Function

    Public Function FileExists(sFileName As String) As Boolean
        For Each ftpfile As FTP_File In Me
            If ftpfile.Filename = sFileName Then
                Return True
            End If
        Next

        Return False
    End Function

    Public Shared Function GetParentDirectory(dir As String) As String
        Dim tmp As String = dir.TrimEnd(SLASH)
        Dim i As Integer = tmp.LastIndexOf(SLASH)

        If i > 0 Then
            Return tmp.Substring(0, i - 1)
        Else
            Throw (New ApplicationException("No parent for root"))
        End If
    End Function

End Class
