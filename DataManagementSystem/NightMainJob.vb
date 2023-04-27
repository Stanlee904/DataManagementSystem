Public Class NightMainJob
    Public DataDefineDAO As New DataDefineDAO()
    Public Utilities As New Utilities()
    Public DataManageMentSystemMethod As New DataManageMentSystemMethod()
    Public KoscomMethod As New KoscomMethod()
    Public connectionDB As New DB_Agent()
    Public RetailDataAndKRXInput As New RetailDataAndKRXInput()

    Dim lineCount_Vd As Long = 0 'VD라인수 
    Dim lineCount_V2 As Long = 0 'V2라인수
    Dim lineCount_V3 As Long = 0 'V3라인수
    Dim lineCount_V7 As Long = 0 'V7라인수
    Dim lineCount_V8 As Long = 0 'V8라인수
    Dim lineCount_V9 As Long = 0 'V9라인수

    Dim isCheckedV2 As Boolean 'V2 체크박스 체크확인 BOOL 변수
    Dim isCheckedVd As Boolean 'Vd 체크박스 체크확인 BOOL 변수
    Dim isCheckedV3 As Boolean 'V3 체크박스 체크확인 BOOL 변수
    Dim isCheckedV7 As Boolean 'V7 체크박스 체크확인 BOOL 변수
    Dim isCheckedV8 As Boolean 'V8 체크박스 체크확인 BOOL 변수
    Dim isCheckedV9 As Boolean 'V9 체크박스 체크확인 BOOL 변수

    Dim todayDoCountVD As Long 'VD 금일 처리 할 개수 
    Dim todayDoCountV2 As Long 'V2 금일 처리 할 개수 
    Dim todayDoCountV3 As Long 'V3 금일 처리 할 개수 
    Dim todayDoCountV7 As Long 'V7 금일 처리 할 개수 
    Dim todayDoCountV8 As Long 'V8 금일 처리 할 개수
    Dim todayDoCountV9 As Long 'V9 금일 처리 할 개수

    Dim inputLineCount As Long = 0 '입력라인수
    Dim progressLineCount As Long '진행라인수
    Dim oneLineRow As String ' 파일 한줄 
    Dim localTrgFileReader As IO.StreamReader 'IO.StreamReader를 사용 
    Dim trcode As String = "" ' grid TRCODE 확인 변수
    Dim dtblDATA As New DataTable

    Public Function MainNightJob(ByVal locFile As String, logFileName As String, fileNumber As Integer) As Boolean

        Try

            If Utilities.checkFinishedJobNight8000() = False Then
                Return False
            End If

            If Utilities.checkFinishedJobNigth1600(logFileName, fileNumber) = False Then
                Return False
            End If

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "(PM) MainNightJob 작업 시작!" & vbCrLf)
            FileClose()

            Utilities.Job_Begin("1600") 'DB에 오후 작업 번호 INSERT

            '소매채권입력
            'RetailDataAndKRXInput.RetailDataExecute(logFileName, fileNumber)
            '소매채권입력 KRX 차세대
            RetailDataAndKRXInput.RetailDataExecute2(logFileName, fileNumber)

            inputLineCount = 0
            progressLineCount = 1
            localTrgFileReader = My.Computer.FileSystem.OpenTextFileReader(locFile, System.Text.Encoding.Default) '파일 읽을 API

            dtblDATA = frmDataInserter.grd_RECEIVE_LIST.DataSource

            For index = 0 To dtblDATA.Rows.Count - 1
                trcode = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TR")

                Select Case trcode
                    Case "VD"
                        isCheckedVd = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "CHK") '체크박스 체크
                    Case "V2"
                        isCheckedV2 = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "CHK")'체크박스 체크
                    Case "V3"
                        isCheckedV3 = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "CHK")'체크박스 체크
                    Case "V7"
                        isCheckedV7 = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "CHK")'체크박스 체크
                    Case "V8"
                        isCheckedV8 = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "CHK") '체크박스 체크
                    Case "V9"
                        isCheckedV9 = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "CHK") '체크박스 체크
                End Select
            Next

            'KOSCOM 기초자료를 발행정보,주식관련채권 (구분자별로)등등 신규추가,수정,삭제 작업을 함
            While Not localTrgFileReader.EndOfStream
                oneLineRow = localTrgFileReader.ReadLine() '한 줄 씩 읽어오기

                '1 ~ 2번 까지의 TRCODE 추출 
                DataDefineDAO.trCode = Mid(oneLineRow, 1, 2)

                Select Case DataDefineDAO.trCode
                    Case "VD"
                        If isCheckedVd = True Then
                            DataManageMentSystemMethod.GetDataVD(oneLineRow, inputLineCount, logFileName, fileNumber) '파일 PATH , 길이 반환
                            '변수초기화 
                            DataManageMentSystemMethod.ClearData()
                            'VD라인수 증가식 추가
                            lineCount_Vd = lineCount_Vd + 1
                            '입력라인수 증가식 추가
                            inputLineCount = inputLineCount + 1
                            frmDataInserter.inputLineLabel.Text = "입력 라인수 : " & inputLineCount
                        End If
                    Case "V7"
                        If isCheckedV7 = True Then
                            DataManageMentSystemMethod.GetDataV7(oneLineRow, inputLineCount, logFileName, fileNumber)
                            '변수 초기화 
                            DataManageMentSystemMethod.ClearData()
                            'V7 라인수 증가식 추가
                            lineCount_V7 = lineCount_V7 + 1
                            '입력라인수 증가식 추가
                            inputLineCount = inputLineCount + 1
                            frmDataInserter.inputLineLabel.Text = "입력 라인수 : " & inputLineCount
                        End If
                    Case "V3"
                        If isCheckedV3 = True Then
                            DataManageMentSystemMethod.GetDataV3(oneLineRow, inputLineCount, logFileName, fileNumber)
                            '변수 초기화 
                            DataManageMentSystemMethod.ClearData()
                            'V3라인수 증가식 추가
                            lineCount_V3 = lineCount_V3 + 1
                            '입력라인수 증가식 추가
                            inputLineCount = inputLineCount + 1
                            frmDataInserter.inputLineLabel.Text = "입력 라인수 : " & inputLineCount
                        End If
                    Case "V2"
                        If isCheckedV2 = True Then
                            KoscomMethod.GetDataV2(oneLineRow, inputLineCount, logFileName, fileNumber)
                            lineCount_V2 = lineCount_V2 + 1
                            inputLineCount = inputLineCount + 1
                            frmDataInserter.inputLineLabel.Text = "입력 라인수 : " & inputLineCount
                        End If
                    Case "V8"
                        If isCheckedV8 = True Then
                            KoscomMethod.GetDataV8(oneLineRow, inputLineCount, logFileName, fileNumber)
                            lineCount_V8 = lineCount_V8 + 1
                            inputLineCount = inputLineCount + 1
                            frmDataInserter.inputLineLabel.Text = "입력 라인수 : " & inputLineCount
                        End If
                    Case "V9"
                        If isCheckedV9 = True Then
                            KoscomMethod.GetDataV9(oneLineRow, inputLineCount, logFileName, fileNumber)
                            lineCount_V9 = lineCount_V9 + 1
                            inputLineCount = inputLineCount + 1
                            frmDataInserter.inputLineLabel.Text = "입력 라인수 : " & inputLineCount
                        End If
                End Select
                progressLineCount = progressLineCount + 1
                frmDataInserter.progressLineLabel.Text = "진행 라인수 : " & progressLineCount

            End While

            dtblDATA = frmDataInserter.grd_RECEIVE_LIST.DataSource

            ' 처리 후 개수 GRID에 표시
            For index = 0 To dtblDATA.Rows.Count - 1
                trcode = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TR")

                Select Case trcode
                    Case "VD"
                        frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "TDY_DIDCNT", lineCount_Vd)
                        ' 처리 할 개수 / 처리 된 개수 값 가져오기
                        todayDoCountVD = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TDY_DOCNT")
                        ' 처리 할 개수와 처리 된 개수 값이 같은 경우
                        If lineCount_Vd = todayDoCountVD Then
                            ' 해당 ROW의 체크박스 해제
                            frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "COMPLETE", "완료")
                            frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "CHK", 0)
                        End If
                    Case "V2"
                        frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "TDY_DIDCNT", lineCount_V2)
                        todayDoCountV2 = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TDY_DOCNT")
                        If lineCount_V2 = todayDoCountV2 Then
                            frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "COMPLETE", "완료")
                            frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "CHK", 0)
                        End If
                    Case "V3"
                        frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "TDY_DIDCNT", lineCount_V3)
                        todayDoCountV3 = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TDY_DOCNT")
                        If lineCount_V3 = todayDoCountV3 Then
                            frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "COMPLETE", "완료")
                            frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "CHK", 0)
                        End If
                    Case "V7"
                        frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "TDY_DIDCNT", lineCount_V7)
                        todayDoCountV7 = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TDY_DOCNT")
                        If lineCount_V7 = todayDoCountV7 Then
                            frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "COMPLETE", "완료")
                            frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "CHK", 0)
                        End If

                    Case "V8"
                        frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "TDY_DIDCNT", lineCount_V8)
                        todayDoCountV8 = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TDY_DOCNT")
                        If lineCount_V8 = todayDoCountV8 Then
                            frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "COMPLETE", "완료")
                            frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "CHK", 0)
                        End If

                    Case "V9"
                        frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "TDY_DIDCNT", lineCount_V9)
                        todayDoCountV9 = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TDY_DOCNT")
                        If lineCount_V9 = todayDoCountV9 Then
                            frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "COMPLETE", "완료")
                            frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "CHK", 0)
                        End If

                End Select
            Next

            Dim dicParams = New Dictionary(Of String, String) ' 딕셔너리 선언
            dicParams.Add(":rightBdata", Utilities.RightB(locFile, 14)) '파일 이름 오른쪽으로 BYTE 단위로 짤라서 가져오기

            connectionDB.saveData2(DB_Query.InsertDataLog, Nothing, dicParams, False) '파일 이름 DB에 INSERT
            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "(PM) MainNightJob 완료 로그 DB INSERT" & vbCrLf)
            FileClose()

            '완료 로그 작성
            Utilities.DailyInputLog(logFileName, fileNumber, CDbl(inputLineCount), CDbl(progressLineCount))
            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "(PM) MainNightJob 완료 로그 작성 " & vbCrLf)
            FileClose()

            '완료 로그
            Utilities.Job_End("1600")

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "(PM) MainNightJob : 작업 완료!!!!! " & vbCrLf)
            FileClose()

            Return True

        Catch ex As Exception
            Throw ex

        End Try
    End Function

    Public Function TodayDoCountGridWriteNight(ByVal locFile As String, logFileName As String, fileNumber As Integer)

        Try

            localTrgFileReader = My.Computer.FileSystem.OpenTextFileReader(locFile, System.Text.Encoding.Default) '파일 읽을 API


            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "(PM) MainNightJob 처리 할 개수 파악 !" & vbCrLf)
            FileClose()

            ' VD , V7, V3 , V9,V8,V2 처리 할 개수 파악
            While Not localTrgFileReader.EndOfStream

                oneLineRow = localTrgFileReader.ReadLine() '한 줄 씩 읽어오기

                '1 ~ 2번 까지의 TRCODE 추출 
                DataDefineDAO.trCode = Mid(oneLineRow, 1, 2)

                Select Case DataDefineDAO.trCode
                    Case "VD"
                        'VD라인수 증가식 추가
                        lineCount_Vd = lineCount_Vd + 1
                    Case "V2"
                        lineCount_V2 = lineCount_V2 + 1
                    Case "V3"
                        'V3라인수 증가식 추가
                        lineCount_V3 = lineCount_V3 + 1
                    Case "V7"
                        'V7 라인수 증가식 추가
                        lineCount_V7 = lineCount_V7 + 1
                    Case "V8"
                        'V8 라인수 증가식 추가
                        lineCount_V8 = lineCount_V8 + 1
                    Case "V9"
                        'V8 라인수 증가식 추가
                        lineCount_V9 = lineCount_V9 + 1
                End Select
            End While
            oneLineRow = ""

            localTrgFileReader.Close()

            dtblDATA = frmDataInserter.grd_RECEIVE_LIST.DataSource

            For index = 0 To dtblDATA.Rows.Count - 1
                trcode = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TR")

                Select Case trcode
                    Case "VD"
                        frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "TDY_DOCNT", lineCount_Vd) '개수 확인
                    Case "V2"
                        frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "TDY_DOCNT", lineCount_V2)
                    Case "V3"
                        frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "TDY_DOCNT", lineCount_V3)
                    Case "V7"
                        frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "TDY_DOCNT", lineCount_V7)
                    Case "V8"
                        frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "TDY_DOCNT", lineCount_V8)
                    Case "V9"
                        frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "TDY_DOCNT", lineCount_V9)
                End Select
            Next

            '초기화 진행
            lineCount_Vd = 0
            lineCount_V2 = 0
            lineCount_V3 = 0
            lineCount_V7 = 0
            lineCount_V8 = 0
            lineCount_V9 = 0


            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "(PM) MainNightJob 처리 할 개수 파악 완료!" & vbCrLf)
            FileClose()

        Catch ex As Exception
            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "(PM) MainNightJob 처리 할 개수 파악 중 오류 발생!" & vbCrLf)
            Print(fileNumber, Date.Now & " | " & "오류 내용 : " & vbCrLf)
            FileClose()
            MsgBox("(PM) MainNightJob 처리 할 개수 파악 중 오류 발생! 오류 내용 : " & ex.ToString())
            Throw ex
        End Try
    End Function

End Class
