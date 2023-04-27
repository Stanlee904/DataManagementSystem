Public Class LastMainJob

    Public connectionDB As New DB_Agent()
    Public Utilities As New Utilities()
    Public LastDAO As New LastDAO()
    Public FTP_Agent As New FTP_Agent()

    Dim App_Path = System.AppDomain.CurrentDomain.BaseDirectory() ' 프로그램 현재 경로
    Dim txtFileReader As IO.StreamReader 'IO.StreamReader를 사용 
    Dim lineCount As Integer = 0 '라인 수 확인 용 
    Dim adjustLineCount As Integer = 0 '"정정" 개수 확인 용 
    Dim colNameVar_A001B As String = "" 'COL_NAME BINDING 변수(쿼리의 명령문을 추가적으로 넣기 위한 변수)
    Dim isCheckedMorningReceive As Boolean '오전 11시 30분 체크박스 체크확인 BOOL 변수
    Dim isCheckedAfterNoonReceive As Boolean 'G3027 체크박스 체크확인 BOOL 변수
    Dim receiveTimeCheck As String = "" ' grid receive_time 확인 변수
    Dim dataServerAddress26 As String = "\\222.111.237.26\udpkoscom\data\"
    Dim dtblDATA As New DataTable

#Region "장외채권최종호가수익률"

    Public Function LastAKPRCValueMainProcess(logFileName As String, fileNumber As Integer)

        Try

            Dim checkDataTable As DataTable

            LastDAO.j5077_File = App_Path & "Data\J5077_" & Format(frm_LstAsk.t_day.Value, "yyyyMMdd") & ".dat"
            LastDAO.j5077_Server_fileLocation = dataServerAddress26 & Format(frm_LstAsk.t_day.Value, "yyyyMMdd") & "\J5077.dat" ' 연결 서버의 DAT 파일명

            Dim fileServerCheckTodayJ5077 As New System.IO.FileInfo(LastDAO.j5077_Server_fileLocation)
            Dim fileCheckTodayJ5077 As New System.IO.FileInfo(LastDAO.j5077_File)

            If fileServerCheckTodayJ5077.Exists = True Then
                System.IO.File.Copy(LastDAO.j5077_Server_fileLocation, LastDAO.j5077_File) '파일 복사
            End If

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "26번서버 -> 로컬 파일로 복사 완료" & vbCrLf)
            FileClose()


            If fileCheckTodayJ5077.Exists = True Then

                ' [2023-02-09] 추후 DB 생성 되면 그때 시작!!!!
                'Dim index As Integer 'For 문 인덱스
                Dim dicParams = New Dictionary(Of String, String) ' 딕셔너리 선언
                'dicParams.Add(":tr", "J5077")

                'Dim dbTableSeqA001BData As DataTable = connectionDB.getData2(DB_Query.STR_SEQ_Query, dicParams, True, False)
                'Dim dbTableLenA001BData As DataTable = connectionDB.getData2(DB_Query.STR_LEN_Query, dicParams, True, False)
                'Dim dbTableColNameA001BData As DataTable = connectionDB.getData2(DB_Query.COL_NAME_Query, dicParams, True, False) ' 한 문장에서 필요한 값을 가져오기 위한 끝 인덱스 배열 DB 에서 호출


                'Dim dbTableDataStartA001BArray(dbTableSeqA001BData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_SEQ)
                'Dim dbTableDataEndA001BArray(dbTableLenA001BData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_LEN)
                'Dim dbTableDataColNameA001BArray(dbTableColNameA001BData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(COL_NAME)

                'For index = 0 To dbTableDataStartA001BArray.Length - 2
                '    dbTableDataStartA001BArray(index) = dbTableSeqA001BData.Rows(index).Item("STR_SEQ")
                '    dbTableDataEndA001BArray(index) = dbTableLenA001BData.Rows(index).Item("STR_LEN")
                '    dbTableDataColNameA001BArray(index) = dbTableColNameA001BData.Rows(index).Item("COL_NAME")
                'Next


                txtFileReader = My.Computer.FileSystem.OpenTextFileReader(LastDAO.j5077_File, System.Text.Encoding.Default) '파일 읽을 API

                ' [2023-02-09] 추후 DB 생성 되면 그때 시작!!!!
                'DB에서 가져온 COL_NAME값 변수에 대입
                'For index = 0 To dbTableDataColNameA001BArray.Length - 1
                '    If index = dbTableDataColNameA001BArray.Length - 1 Then
                '        colNameVar_A001B = colNameVar_A001B & dbTableDataColNameA001BArray(index)
                '    Else
                '        colNameVar_A001B = colNameVar_A001B & dbTableDataColNameA001BArray(index) & ","
                '    End If
                'Next

                dtblDATA = frm_LstAsk.grd_FINCHK_LIST.DataSource

                ' J5077 파일 데이터 추출 작업 진행
                While Not txtFileReader.EndOfStream

                    LastDAO.tempLine = ""

                    LastDAO.tempLine = txtFileReader.ReadLine()

                    LastDAO.temp_length = Utilities.ByteLen(LastDAO.tempLine)

                    If LastDAO.temp_length = 519 And (Mid(LastDAO.tempLine, 37, 2) = "01" Or Mid(LastDAO.tempLine, 37, 2) = "02") Then

                        LastDAO.PANC_DT = Mid(LastDAO.tempLine, 39, 8)
                        LastDAO.PANC_TIME = Mid(LastDAO.tempLine, 47, 6)
                        LastDAO.PROC_TP_CD = Mid(LastDAO.tempLine, 53, 1)
                        LastDAO.IN_SEQ_TP_CD = Mid(LastDAO.tempLine, 54, 4)
                        LastDAO.PANC_KND_TP_CD = Mid(LastDAO.tempLine, 58, 2)
                        LastDAO.PACN_STK_CD = Mid(LastDAO.tempLine, 60, 4)
                        LastDAO.LST_AKPRC_ERNR = Mid(LastDAO.tempLine, 64, 9)

                        FileOpen(fileNumber, logFileName, OpenMode.Append)
                        Print(fileNumber, Date.Now & " | " & LastDAO.IN_SEQ_TP_CD & "|" & LastDAO.PACN_STK_CD & "데이터 작업 시작!" & vbCrLf)
                        FileClose()

                        ' 공시일자, 공시구분코드, 처리구분코드로 값이 있는지 없는지 확인 (정정을 위해서 추가)
                        dicParams.Clear()
                        dicParams.Add(":panc_dt", "'" & LastDAO.PANC_DT & "'")
                        dicParams.Add(":pacn_stk_cd", "'" & LastDAO.PACN_STK_CD & "'")

                        checkDataTable = connectionDB.getData2(DB_Query.checkLstAkPrcValue, dicParams, False)

                        If checkDataTable.Rows.Count < 2 Then
                            FileOpen(fileNumber, logFileName, OpenMode.Append)
                            Print(fileNumber, Date.Now & " | " & LastDAO.IN_SEQ_TP_CD & "|" & LastDAO.PACN_STK_CD & "1130 / 1530 데이터가 없을경우 작업 진행!" & vbCrLf)
                            FileClose()

                            dicParams.Clear()
                            dicParams.Add(":panc_dt", "'" & LastDAO.PANC_DT & "'")
                            dicParams.Add(":panc_time", "'" & LastDAO.PANC_TIME & "'")
                            dicParams.Add(":proc_tp_cd", "'" & LastDAO.PROC_TP_CD & "'")
                            dicParams.Add(":in_seq_tp_cd", "'" & LastDAO.IN_SEQ_TP_CD & "'")
                            dicParams.Add(":panc_knd_tp_cd", "'" & LastDAO.PANC_KND_TP_CD & "'")
                            dicParams.Add(":pacn_stk_cd", "'" & LastDAO.PACN_STK_CD & "'")
                            dicParams.Add(":lst_akprc_ernr", LastDAO.LST_AKPRC_ERNR)
                            dicParams.Add(":lst_yn", "'1'")

                            'INSERT 작업 수행
                            connectionDB.saveData2(DB_Query.InsertLstAkPrcValue, Nothing, dicParams, False)
                            FileOpen(fileNumber, logFileName, OpenMode.Append)
                            Print(fileNumber, Date.Now & " | " & LastDAO.IN_SEQ_TP_CD & "|" & LastDAO.PACN_STK_CD & "INSERT 완료" & vbCrLf)
                            FileClose()

                            lineCount += 1

                        Else
                            For index = 0 To checkDataTable.Rows.Count - 1
                                ' 1130 또는 1530의 비교 
                                If LastDAO.IN_SEQ_TP_CD = checkDataTable.Rows(index).Item("IN_SEQ_TP_CD") Then
                                    ' 처리구분코드 비교
                                    If LastDAO.PACN_STK_CD = checkDataTable.Rows(index).Item("PACN_STK_CD") Then
                                        ' 값 비교 
                                        If Not (CDec(checkDataTable.Rows(index).Item("LST_AKPRC_ERNR")) = LastDAO.LST_AKPRC_ERNR) Then
                                            FileOpen(fileNumber, logFileName, OpenMode.Append)
                                            Print(fileNumber, Date.Now & " | 기존:" & LastDAO.IN_SEQ_TP_CD & "|" & LastDAO.PACN_STK_CD & "정정내역" & checkDataTable.Rows(index).Item("IN_SEQ_TP_CD") & checkDataTable.Rows(index).Item("PACN_STK_CD") & "확인!" & vbCrLf)
                                            FileClose()
                                            dicParams.Clear()
                                            dicParams.Add(":panc_dt", "'" & LastDAO.PANC_DT & "'")
                                            dicParams.Add(":in_seq_tp_cd", "'" & LastDAO.IN_SEQ_TP_CD & "'")
                                            dicParams.Add(":panc_knd_tp_cd", "'" & LastDAO.PANC_KND_TP_CD & "'")
                                            dicParams.Add(":pacn_stk_cd", "'" & LastDAO.PACN_STK_CD & "'")
                                            dicParams.Add(":lst_yn", "'1'")
                                            'LST_YN : 1 -> 0으로 수정 
                                            connectionDB.saveData2(DB_Query.UpdateLstAkPrcValue, Nothing, dicParams, False)

                                            FileOpen(fileNumber, logFileName, OpenMode.Append)
                                            Print(fileNumber, Date.Now & " | 기존:" & LastDAO.IN_SEQ_TP_CD & "|" & LastDAO.PACN_STK_CD & "정정내역" & checkDataTable.Rows(index).Item("IN_SEQ_TP_CD") & checkDataTable.Rows(index).Item("PACN_STK_CD") & "LST_YN:0으로 UPDATE 완료" & vbCrLf)
                                            FileClose()

                                            dicParams.Add(":lst_akprc_ernr", LastDAO.LST_AKPRC_ERNR)
                                            dicParams.Add(":panc_time", "'" & LastDAO.PANC_TIME & "'")
                                            dicParams.Add(":proc_tp_cd", "'" & LastDAO.PROC_TP_CD & "'")

                                            '정정 부분 INSERT 작업 수행
                                            connectionDB.saveData2(DB_Query.InsertLstAkPrcValue, Nothing, dicParams, False)
                                            FileOpen(fileNumber, logFileName, OpenMode.Append)
                                            Print(fileNumber, Date.Now & " | 기존:" & LastDAO.IN_SEQ_TP_CD & "|" & LastDAO.PACN_STK_CD & "정정내역" & checkDataTable.Rows(index).Item("IN_SEQ_TP_CD") & checkDataTable.Rows(index).Item("PACN_STK_CD") & "정정 데이터 INSERT 완료" & vbCrLf)
                                            FileClose()

                                            '개수 파악
                                            adjustLineCount += 1

                                        End If
                                    End If
                                End If
                            Next
                        End If

                        ' 오전/오후의 최종호가수익률 값에 대한 DB INSERT가 완료 했는지 판단 후 그리드에 값 SETTING
                        If lineCount = 18 And LastDAO.IN_SEQ_TP_CD = "1130" Then
                            For index = 0 To dtblDATA.Rows.Count - 1
                                receiveTimeCheck = frm_LstAsk.grdv_FINCHK_LIST.GetRowCellValue(index, "RECEIVE_TIME")
                                If receiveTimeCheck = "수신시간(오전:1130)" Then
                                    FileOpen(fileNumber, logFileName, OpenMode.Append)
                                    Print(fileNumber, Date.Now & " | 1130 INSERT 완료!" & vbCrLf)
                                    FileClose()
                                    frm_LstAsk.grdv_FINCHK_LIST.SetRowCellValue(index, "CHK_RCV", 0)
                                    frm_LstAsk.grdv_FINCHK_LIST.SetRowCellValue(index, "COMPLETE", "완료")
                                    Exit For
                                End If
                            Next

                        ElseIf lineCount = 36 And LastDAO.IN_SEQ_TP_CD = "1530" Then
                            For index = 0 To dtblDATA.Rows.Count - 1
                                receiveTimeCheck = frm_LstAsk.grdv_FINCHK_LIST.GetRowCellValue(index, "RECEIVE_TIME")
                                If receiveTimeCheck = "수신시간(오후:1630)" Then
                                    FileOpen(fileNumber, logFileName, OpenMode.Append)
                                    Print(fileNumber, Date.Now & " | 1530 INSERT 완료!" & vbCrLf)
                                    FileClose()
                                    frm_LstAsk.grdv_FINCHK_LIST.SetRowCellValue(index, "CHK_RCV", 0)
                                    frm_LstAsk.grdv_FINCHK_LIST.SetRowCellValue(index, "COMPLETE", "완료")
                                    Exit For
                                End If
                            Next

                            '정정 부분 개수 기입 및 체크박스 해제
                        ElseIf adjustLineCount > 0 Then
                            For index = 0 To dtblDATA.Rows.Count - 1
                                receiveTimeCheck = frm_LstAsk.grdv_FINCHK_LIST.GetRowCellValue(index, "RECEIVE_TIME")
                                If receiveTimeCheck = "정정" Then
                                    FileOpen(fileNumber, logFileName, OpenMode.Append)
                                    Print(fileNumber, Date.Now & " | 정정 발생으로 인한 개수 기입 완료!" & vbCrLf)
                                    FileClose()
                                    frm_LstAsk.grdv_FINCHK_LIST.SetRowCellValue(index, "CHK_RCV", 0)
                                    frm_LstAsk.grdv_FINCHK_LIST.SetRowCellValue(index, "COMPLETE", adjustLineCount)
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                End While

                ' 정정 부분이 없을 경우 0개 기입 
                If adjustLineCount = 0 Then
                    For index = 0 To dtblDATA.Rows.Count - 1
                        receiveTimeCheck = frm_LstAsk.grdv_FINCHK_LIST.GetRowCellValue(index, "RECEIVE_TIME")
                        If receiveTimeCheck = "정정" Then
                            FileOpen(fileNumber, logFileName, OpenMode.Append)
                            Print(fileNumber, Date.Now & " | 정정 개수가 없음으로 0으로 기입!" & vbCrLf)
                            FileClose()
                            frm_LstAsk.grdv_FINCHK_LIST.SetRowCellValue(index, "CHK_RCV", 0)
                            frm_LstAsk.grdv_FINCHK_LIST.SetRowCellValue(index, "COMPLETE", adjustLineCount)
                            Exit For
                        End If
                    Next
                End If
                txtFileReader.Close()
            End If
        Catch ex As Exception

            MsgBox("장외채권최종호가수익률 (LastAKPRCValueMainProcess) 오류 발생 : " & ex.ToString())
            Throw ex
        End Try


    End Function

#End Region

End Class
