Imports System.IO
Imports System.Configuration

Public Class Utilities

    Public connectionDB As New DB_Agent()
    Public DataDefineDAO As New DataDefineDAO()
    Public Agent As New Agent()



#Region "네트워트 설정 조회"
    Protected Friend Shared Function getIPAdress() As String
        Dim ipAddress As String = ""
        Try
            Dim strHostName As String = System.Net.Dns.GetHostName()
            Dim ipEntry As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(strHostName)

            For i = 0 To ipEntry.AddressList.Length - 1
                If IsNumeric(ipEntry.AddressList(i).ToString().Replace(".", "")) Then
                    ipAddress = ipEntry.AddressList(i).ToString()
                    Exit For
                End If
            Next
            'ipAddress = ipEntry.AddressList(0).ToString()
        Catch ex As Exception
            '
        End Try
        Return ipAddress
    End Function

#End Region
#Region "SQL 정의 ->>> 실행문 변환 관련 함수"
    Protected Friend Shared Function replaceParams(ByRef sSql As String, ByRef params() As String) As String
        Dim sReturn As String = sSql

        For index = params.Length - 1 To 0 Step -1

            sReturn = Microsoft.VisualBasic.Strings.Replace(sReturn, ":param" + index.ToString(), params(index), 1, -1, CompareMethod.Text)
        Next

        Return sReturn
    End Function

    Protected Friend Shared Function replaceParams(ByRef sSql As String, ByRef params As Dictionary(Of String, String)) As String
        Dim sReturn As String = sSql


        ' Get list of keys.
        Dim keys As List(Of String) = params.Keys.ToList
        ' Sort the keys.
        keys.Sort()
        ' Loop over the sorted keys.
        For i = keys.Count - 1 To 0 Step -1
            sReturn = Microsoft.VisualBasic.Strings.Replace(sReturn, keys(i), params.Item(keys(i)), 1, -1, CompareMethod.Text)
        Next
        Return sReturn
    End Function

    '위의 replaceParams와 같아 보이지만 다른 이유는 sReturn 내에 Replace 파라미터가 다름
    Protected Friend Shared Function replaceParams_changeReplace(ByRef sSql As String, ByRef params() As String) As String
        Dim sReturn As String = sSql

        For index = params.Length - 1 To 0 Step -1

            sReturn = Microsoft.VisualBasic.Strings.Replace(sReturn, ":param" + index.ToString(), "'" + params(index) + "'", 1, -1, CompareMethod.Text)
        Next

        Return sReturn
    End Function

    Protected Friend Shared Function replaceParams2(ByRef sSql As String, ByRef params As Dictionary(Of String, String)) As String
        Dim sReturn As String = sSql

        ' Get list of keys.
        Dim keys As List(Of String) = params.Keys.ToList
        ' Sort the keys.
        keys.Sort()
        ' Loop over the sorted keys.
        For i = keys.Count - 1 To 0 Step -1
            sReturn = Microsoft.VisualBasic.Strings.Replace(sReturn, keys(i), "'" + params.Item(keys(i)) + "'", 1, -1, CompareMethod.Text)
        Next

        Return sReturn
    End Function

#End Region


#Region "기타 기능"

    ''' <summary>
    ''' 주어진 문자열을 날짜형식으로 변환
    ''' </summary>
    ''' <param name="dat"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function changeDate(ByVal dat As String) As String
        Dim tmpDate As String
        tmpDate = dat.Substring(0, 4) & "-" & dat.Substring(4, 2) & "-" & dat.Substring(6, 2)
        Return tmpDate
    End Function

    ''' <summary>
    ''' 주어진 문자열이 날짜인지 검사
    ''' </summary>
    ''' <param name="tmp_day"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function CheckDate(ByVal tmp_day As String) As String
        Try

            If tmp_day = "00000000" Or tmp_day = "        " Then
                Return "19000101"
            ElseIf IsDate(changeDate(tmp_day)) = False Then
                Return "19000101"
            Else
                Return tmp_day
            End If

        Catch ex As Exception
            Return "19000101"
        End Try

    End Function

    ''' <summary>
    ''' 지정된 이름을 변수명으로 지정된 길이만큼 잘라서 Dictionary로 만든다.
    ''' </summary>
    ''' <param name="sLine"></param>
    ''' <param name="Name"></param>
    ''' <param name="len"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function StringCut(ByVal sLine As String, ByVal Name() As String, ByVal len() As Integer) As Dictionary(Of String, String)
        Dim dicParams As New Dictionary(Of String, String)
        Dim buf() As Byte
        Dim ksEncoding As System.Text.Encoding = System.Text.Encoding.GetEncoding("ks_c_5601-1987")
        Dim lenCheck As Integer
        Dim i As Integer

        buf = ksEncoding.GetBytes(sLine)
        lenCheck = 0

        Do Until Name(i) = Nothing
            dicParams(Name(i)) = ksEncoding.GetString(buf, lenCheck, len(i))
            lenCheck = lenCheck + len(i)
            i = i + 1
        Loop

        Return dicParams
    End Function

    '//////////////////////////////////////////////////////////////////////////
    '// function : ByteLen()                                                 //
    '// -----------------------------------------------------------------------
    '// 바이트 단위로 문자개수를 추출하는 함수                               //
    '//////////////////////////////////////////////////////////////////////////
    Public Function ByteLen(str)
        Dim Z As Long
        Dim chlen As Long
        For Z = 1 To Len(str)
            If Asc(Mid(str, Z, 1)) < 0 Then
                chlen = chlen + 2
            Else
                chlen = chlen + 1
            End If
        Next
        ByteLen = chlen
    End Function

    '//////////////////////////////////////////////////////////////////////////
    '// function : ByteMidStr() 와 동일하지만 결과값에 TRIM을 씌워 리턴      //
    '// -----------------------------------------------------------------------
    '// 바이트 단위로 문자열을 추출하는 함수                                 //
    '// 쿼리문 작성이 아닌 문자열 편집을 위해서만 사용                       //
    '//////////////////////////////////////////////////////////////////////////
    Public Function ByteMidStrTrim(str As String, startposition As Integer, num As Integer) As String

        Dim i As Long
        Dim chlen As Long
        Dim result As String
        Dim tmp As String

        Dim ch As String

        For i = 1 To Len(str)
            tmp = Mid(str, i, 1)
            If Asc(tmp) < 0 Then
                chlen = chlen + 2
            Else
                chlen = chlen + 1
            End If

            If (chlen >= startposition) And (chlen <= startposition + num - 1) Then
                ch = Mid(str, CInt(i), 1)
                result = result + ch
            End If
        Next

        ByteMidStrTrim = Trim(result)

    End Function

    '//////////////////////////////////////////////////////////////////////////
    '// function : ByteMid()                                                 //
    '// -----------------------------------------------------------------------
    '// 바이트 단위로 문자열을 추출하는 함수                                 //
    '// 쿼리문 작성을 위해서만 사용할것("'" 문자열 앞에 "'" 문자가 추가됨)   //
    '//////////////////////////////////////////////////////////////////////////
    Public Function ByteMid(oneLineRow As String, startposition As Integer, endPosition As Integer) As String
        Dim index As Long 'index
        Dim charLength As Long 'charLength
        Dim result As String
        Dim temp_Char As String 'temp_Char

        Dim check_Char As String 'check_Char

        For index = 1 To Len(oneLineRow)
            temp_Char = Mid(oneLineRow, index, 1)
            If Asc(temp_Char) < 0 Then
                charLength = charLength + 2
            Else
                charLength = charLength + 1
            End If

            If (charLength >= startposition) And (charLength <= startposition + endPosition - 1) Then
                check_Char = Mid(oneLineRow, CInt(index), 1)

                If check_Char = "'" Then

                    'result = result + "'" + ch
                    '[2010-02-11 박새암] 작은 따옴표를 정상적으로 표현하기 위해 수정

                    result = result + "'||chr(39)||'"
                Else
                    result = result + check_Char
                End If
            End If
        Next

        ByteMid = Trim(result)

    End Function

    '//////////////////////////////////////////////////////////////////////////
    '// function : ByteLen()                                                 //
    '// -----------------------------------------------------------------------
    '// 바이트 단위로 문자개수를 추출하는 함수                               //
    '//////////////////////////////////////////////////////////////////////////
    Public Function ByteLen(str As String)
        Dim index As Long
        Dim charLength As Long
        For index = 1 To Len(str)
            If Asc(Mid(str, index, 1)) < 0 Then
                charLength = charLength + 2
            Else
                charLength = charLength + 1
            End If
        Next
        ByteLen = charLength
    End Function

    Public Function FileChk(ByVal trgFile As String) As Boolean

        On Error GoTo ErrHandler

        'YTM파일이 있는지 유무 체크(폴더경로가 1개 일때)
        Dim fs_file, file_name

        On Error Resume Next
        '파일존재 유무
        fs_file = CreateObject("Scripting.FileSystemObject")

        If fs_file.fileExists(trgFile) = True Then
            FileChk = True
        Else
            FileChk = False
        End If

        file_name = Nothing
        fs_file = Nothing

        Exit Function

ErrHandler:

        FileChk = False

    End Function

    Public Function checkFinishedJobNigth1600(logFileName As String, fileNumber As Integer) As Boolean


        FileOpen(fileNumber, logFileName, OpenMode.Append)
        Print(fileNumber, Date.Now & " | " & "(PM) checkFinishedJobNigth1600 : 작업 시작!" & vbCrLf)
        FileClose()

        If frmDataInserter.optNight.Checked And checkExch_ETC(Format(frmDataInserter.t_day.Value, "yyyy-MM-dd"), "1600") = True Then

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "(PM) checkFinishedJobNigth1600 : 오후 자료관리 시스템이 이미 실행되었습니다. " & vbCrLf)
            FileClose()

            MsgBox("오후 자료관리 시스템이 이미 실행되었습니다.")
            Return False
        Else

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "(PM) checkFinishedJobNigth1600(함수) : 작업 완료!" & vbCrLf)
            FileClose()

            Return True
        End If
    End Function

    Public Function checkFinishedJobNight8000() As Boolean
        If frmDataInserter.optNight.Checked And checkExch_ETC(Format(frmDataInserter.t_day.Value, "yyyy-MM-dd"), "8000") = False Then
            MsgBox("전송 전 DATA 체크가 완료되지 않았습니다.")
            Return False
        Else
            Return True
        End If
    End Function

    Public Function checkFinishedJobDay(logFileName As String, fileNumber As Integer) As Boolean

        FileOpen(fileNumber, logFileName, OpenMode.Append)
        Print(fileNumber, Date.Now & " | " & "(AM) checkFinishedJobDay : 작업 시작" & vbCrLf)
        FileClose()

        If frmDataInserter.opt_Day.Checked And checkExch_ETC(frmDataInserter.t_day.Value, "1500") = True Then

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "(AM) checkFinishedJobDay : 오전 자료관리 시스템이 이미 실행되었습니다. " & vbCrLf)
            FileClose()

            MsgBox("오전 자료관리 시스템이 이미 실행되었습니다.")
            Return True
        Else

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "(AM) checkFinishedJobDay : 작업 완료 " & vbCrLf)
            FileClose()
            Return False
        End If
    End Function

    Public Function checkExch_ETC(ByVal editValue As Date, job_status As String) As Boolean



        Dim dicParams = New Dictionary(Of String, String)
        Dim ResultCheckETC As String
        Dim isCheckETC As Boolean

        dicParams.Clear()
        dicParams.Add(":log_date", editValue.ToString("yyyy-MM-dd"))
        dicParams.Add(":job_status", job_status)

        Dim dbTableData As DataTable = connectionDB.getData2(DB_Query.checkExch_ETC, dicParams, True)

        ResultCheckETC = dbTableData.Rows(0)("flag").ToString()

        If ResultCheckETC = "2" Then
            isCheckETC = True
        Else
            isCheckETC = False

        End If

        Return isCheckETC

    End Function


    Public Function CatchCol(ByVal oneLineRow As String, ByVal startPosition As Integer, ByVal endPosition As Integer, Optional sType As Integer = 1) As String
        Dim index As Long '인덱스
        Dim charLen As Long ' 한글자씩 아스키코드 값을 확인.
        Dim result As String '
        Dim tempStr As String
        Dim tempCh As String

        For index = 1 To Len(oneLineRow)
            tempStr = Mid(oneLineRow, index, 1)
            ' 아스키코드로 0보다 작은지 확인
            If Asc(tempStr) < 0 Then
                charLen = charLen + 2
            Else
                charLen = charLen + 1
            End If

            If (charLen >= startPosition) And (charLen <= startPosition + endPosition - 1) Then
                tempCh = Mid(oneLineRow, CInt(index), 1)
                If tempCh = "'" Then
                    result = result + "'" + tempCh
                Else
                    result = result + tempCh
                End If
            End If
        Next

        result = IIf(Trim(result) = "", "Null", Trim(result)) 'result가 공백이라면, null을 / 아니면 result에 Trim 적용

        If sType <> 3 And result <> "Null" Then
            result = "'" & result & "'"
        End If

        If sType = 2 And result = "'00000000'" Then
            result = "Null"
        End If

        Return result

    End Function

    '문자열의 오른쪽부터 Byte 단위로 반환
    Public Shared Function RightB(ByVal stTarget As String, ByVal iByteSize As Integer) As String
        Dim hEncoding As System.Text.Encoding = System.Text.Encoding.GetEncoding("Shift_JIS")
        Dim btBytes As Byte() = hEncoding.GetBytes(stTarget)
        Return hEncoding.GetString(btBytes, btBytes.Length - iByteSize, iByteSize)
    End Function


    '완료 로그 작성
    Public Function DailyInputLog(logFileName As String, fileNumber As Integer, inputLineCount As Double, progressLineCount As Double)
        ' 20221007 -> 기존 파일 아침 / 오후 별로 개수 확인용 로그를 만들었으나, 하나의 로그 파일 안에 작성하는 걸로 수정
        Try
            'Dim filePathAM As String  '오전 로그
            'Dim filePathPM As String  '오후 로그
            'Dim logWriterAM As StreamWriter
            'Dim logWriterPM As StreamWriter

            'Dim App_Path = System.AppDomain.CurrentDomain.BaseDirectory() ' 프로그램 현재 경로

            'filePathAM = App_Path & "log\Data" & Format(Date.Today, "yyyyMMdd") & "_AM.log"
            'filePathPM = App_Path & "log\Data" & Format(Date.Today, "yyyyMMdd") & "_PM.log"

            'If frmDataInserter.opt_Day.Checked And Not System.IO.File.Exists(filePathAM) Then '오전 파일 확인
            '    System.IO.File.Create(filePathAM).Dispose()
            '    logWriterAM = New StreamWriter(filePathAM) 'StreamWriter를 객체로 선언
            'End If

            'If frmDataInserter.optNight.Checked And Not System.IO.File.Exists(filePathPM) Then
            '    System.IO.File.Create(filePathPM).Dispose()
            '    logWriterPM = New StreamWriter(filePathPM) 'StreamWriter를 객체로 선언
            'End If

            If frmDataInserter.opt_Day.Checked And System.IO.File.Exists(logFileName) Then
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | " & "[입력 완료 시간] : " & vbTab & Format(Date.Today, "yyyy-MM-dd HH:mm:ss") & vbCrLf)
                Print(fileNumber, Date.Now & " | " & "[입력 라인 수] : " & vbTab & inputLineCount & " 개" & vbCrLf)
                Print(fileNumber, Date.Now & " | " & "[진행 라인 수] : " & vbTab & progressLineCount & " 개" & vbCrLf)
                FileClose()
                'logWriterAM.WriteLine("[입력 완료 시간] : " & vbTab & Format(Date.Today, "yyyy-MM-dd HH:mm:ss"))
                'logWriterAM.WriteLine("[입력 라인 수] : " & vbTab & inputLineCount & " 개")
                'logWriterAM.WriteLine("[진행 라인 수] : " & vbTab & progressLineCount & " 개" & vbCrLf & vbCrLf)
                'logWriterAM.Close()
            End If

            If frmDataInserter.optNight.Checked And System.IO.File.Exists(logFileName) Then
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | " & "[입력 완료 시간] : " & vbTab & Format(Date.Today, "yyyy-MM-dd HH:mm:ss") & vbCrLf)
                Print(fileNumber, Date.Now & " | " & "[입력 라인 수] : " & vbTab & inputLineCount & " 개" & vbCrLf)
                Print(fileNumber, Date.Now & " | " & "[진행 라인 수] : " & vbTab & progressLineCount & " 개" & vbCrLf)
                FileClose()
                'logWriterPM.WriteLine("[입력 완료 시간] : " & vbTab & Format(Date.Today, "yyyy-MM-dd HH:mm:ss"))
                'logWriterPM.WriteLine("[입력 라인 수] : " & vbTab & inputLineCount & " 개")
                'logWriterPM.WriteLine("[진행 라인 수] : " & vbTab & progressLineCount & " 개" & vbCrLf & vbCrLf)
                'logWriterPM.Close()
            End If
        Catch ex As Exception
            MsgBox("완료 로그 파일 작성 중 오류 발생!")
            Exit Function
        End Try

    End Function

#End Region

#Region "Job Begin"

    ''' <summary>
    ''' 프로그램 제어
    ''' 선행작업이 처리가 되었는지 안 되었는지 확인
    ''' </summary>
    ''' <param name="iJOB_NUMBER"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Shared Function Job_Begin(ByVal iJOB_NUMBER As Integer,
                                     Optional ByVal sSTD_DT As String = Nothing) As Boolean

        Dim cmd As Oracle.DataAccess.Client.OracleCommand
        Dim sDate As String
        Dim sIPAddres As String
        Dim sProcName As String
        Dim iResult As Integer = 0
        Dim sPrev_job As String = ""

        Try
            Job_Begin = True

            sIPAddres = getIPAdress()

            If IsNothing(sSTD_DT) Then
                sDate = Now.ToString("yyyy-MM-dd")
            Else
                sDate = sSTD_DT
            End If

            sProcName = "CTRL_JOB_BEGIN_ADDIP"
            cmd = New Oracle.DataAccess.Client.OracleCommand(sProcName, Agent.getConn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Clear()
            cmd.Parameters.Add("I_DAY", Oracle.DataAccess.Client.OracleDbType.Varchar2, 255).Direction = ParameterDirection.Input
            cmd.Parameters.Add("I_STATUS_NO", Oracle.DataAccess.Client.OracleDbType.Int32).Direction = ParameterDirection.Input
            cmd.Parameters.Add("I_IP_ADDRESS", Oracle.DataAccess.Client.OracleDbType.Varchar2, 255).Direction = ParameterDirection.Input
            cmd.Parameters.Add("O_RESULT", Oracle.DataAccess.Client.OracleDbType.Int32).Direction = ParameterDirection.Output
            cmd.Parameters.Add("O_PREV_JOB", Oracle.DataAccess.Client.OracleDbType.Varchar2, 255).Direction = ParameterDirection.Output

            cmd.Parameters("I_DAY").Value = sDate
            cmd.Parameters("I_STATUS_NO").Value = iJOB_NUMBER
            cmd.Parameters("I_IP_ADDRESS").Value = sIPAddres

            cmd.ExecuteNonQuery()


            If cmd.Parameters("O_RESULT").Value = 0 Then
                MsgBox("이전 작업(" + cmd.Parameters("O_PREV_JOB").Value + ")이 완료되지 않았습니다.")
                Job_Begin = False
            End If

            cmd.Dispose()
        Catch ex As Exception
            Throw ex
        End Try

    End Function

#End Region

#Region "Job End"

    ''' <summary>
    ''' 선행작업이 완료가 되었는지 확인
    ''' </summary>
    ''' <param name="iJOB_NUMBER"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Job_End(ByVal iJOB_NUMBER As Integer,
                                   Optional ByVal sSTD_DT As String = Nothing)
        Dim cmd As Oracle.DataAccess.Client.OracleCommand
        Dim sDate As String
        Dim sProcName As String

        Try
            If IsNothing(sSTD_DT) Then
                sDate = Now.ToString("yyyy-MM-dd")
            Else
                sDate = sSTD_DT
            End If

            sProcName = "CTRL_JOB_END"
            cmd = New Oracle.DataAccess.Client.OracleCommand(sProcName, Agent.getConn)
            cmd.CommandType = CommandType.StoredProcedure

            cmd.Parameters.Add("I_DAY", Oracle.DataAccess.Client.OracleDbType.Varchar2, 255).Direction = ParameterDirection.Input
            cmd.Parameters.Add("I_STATUS_NO", Oracle.DataAccess.Client.OracleDbType.Int32).Direction = ParameterDirection.Input

            cmd.Parameters("I_DAY").Value = sDate
            cmd.Parameters("I_STATUS_NO").Value = iJOB_NUMBER

            cmd.ExecuteNonQuery()

            Job_End = True
            cmd.Dispose()
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#End Region
#Region "deleteLogFile"

    ''' <summary>
    ''' 로그파일 30일 이상 지난 파일에 대해서 파일 삭제 
    ''' </summary>
    ''' <param ></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function DeleteLogFile()
        Dim App_Path = System.AppDomain.CurrentDomain.BaseDirectory() ' 프로그램 현재 경로
        Try
            Dim directory As New IO.DirectoryInfo(App_Path & "log")

            For Each file As IO.FileInfo In directory.GetFiles
                If (Now - file.CreationTime).Days > 30 Then file.Delete()
            Next

        Catch ex As Exception
            Throw ex
        End Try
    End Function

#End Region
End Class


