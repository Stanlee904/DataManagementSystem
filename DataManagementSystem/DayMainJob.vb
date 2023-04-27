
Public Class DayMainJob

    Public DataDefineDAO As New DataDefineDAO()
    Public Utilities As New Utilities()
    Public DataManageMentSystemMethod As New DataManageMentSystemMethod()
    Public KoscomMethod As New KoscomMethod()
    Public connectionDB As New DB_Agent()

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

    Dim todayDoCountVD As Integer 'VD 금일 처리 할 개수 
    Dim todayDoCountV2 As Integer 'V2 금일 처리 할 개수 
    Dim todayDoCountV3 As Integer 'V3 금일 처리 할 개수 
    Dim todayDoCountV7 As Integer 'V7 금일 처리 할 개수 
    Dim todayDoCountV8 As Integer 'V8 금일 처리 할 개수
    Dim todayDoCountV9 As Integer 'V9 금일 처리 할 개수

    Dim inputLineCount As Long '입력라인수
    Dim progressLineCount As Long '진행라인수
    Dim oneLineRow As String ' 파일 한줄 
    Dim localTrgFileReader As IO.StreamReader 'IO.StreamReader를 사용 
    Dim dtblDATA As New DataTable
    Dim trcode As String = "" ' grid TRCODE 확인 변수

    Public Function MainDayJob(ByVal locFile As String, logFileName As String, fileNumber As Integer) As Boolean

        Try
            localTrgFileReader = My.Computer.FileSystem.OpenTextFileReader(locFile, System.Text.Encoding.Default) '파일 읽을 API

            '선행작업 확인
            If Utilities.checkExch_ETC(Format(frmDataInserter.t_day.Value, "yyyy-MM-dd"), "1400") = True Then
                '선행작업 확인
                Utilities.Job_Begin("1500")

                inputLineCount = 0 '입력 라인수 
                progressLineCount = 1 ' 진행 라인 수 


                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | " & "(AM) MainDayJob 작업 시작!" & vbCrLf)
                FileClose()

                dtblDATA = frmDataInserter.grd_RECEIVE_LIST.DataSource

                '처리 할 개수 확인 & 그리드에 기입 / TRCODE별 체크박스 체크
                For index = 0 To dtblDATA.Rows.Count - 1
                    trcode = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TR")

                    Select Case trcode
                        Case "VD"
                            isCheckedVd = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "CHK") '체크박스 확인
                        Case "V2"
                            isCheckedV2 = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "CHK")'체크박스 확인
                        Case "V3"
                            isCheckedV3 = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "CHK")'체크박스 확인
                        Case "V7"
                            isCheckedV7 = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "CHK")'체크박스 확인
                        Case "V8"
                            isCheckedV8 = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "CHK") '체크박스 확인
                        Case "V9"
                            isCheckedV9 = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "CHK") '체크박스 확인
                    End Select
                Next


                'KOSCOM 기초자료를 발행정보,주식관련채권 (구분자별로)등등 신규추가,수정,삭제 작업을 
                ' VD , V7, V3 , V9,V8,V2  진행
                While Not localTrgFileReader.EndOfStream

                    oneLineRow = localTrgFileReader.ReadLine() '한 줄 씩 읽어오기

                    '1 ~ 2번 까지의 TRCODE 추출 
                    DataDefineDAO.trCode = Mid(oneLineRow, 1, 2)

                    Select Case DataDefineDAO.trCode
                        Case "VD" '대용가 
                            If isCheckedVd = True Then '체크박스 체크 확인 
                                DataManageMentSystemMethod.GetDataVD(oneLineRow, inputLineCount, logFileName, fileNumber) ' 파일 내용의 1줄 매개인자로
                                'VD라인수 증가식 추가
                                lineCount_Vd = lineCount_Vd + 1
                                '변수초기화 
                                DataManageMentSystemMethod.ClearData()
                                '입력라인수 증가식 추가
                                inputLineCount = inputLineCount + 1
                                frmDataInserter.inputLineLabel.Text = "입력 라인수 : " & inputLineCount
                            End If
                        Case "V7"
                            If isCheckedV7 = True Then
                                DataManageMentSystemMethod.GetDataV7(oneLineRow, inputLineCount, logFileName, fileNumber)
                                '변수 초기화 
                                DataManageMentSystemMethod.ClearData()
                                '입력라인수 증가식 추가
                                lineCount_V7 = lineCount_V7 + 1
                                inputLineCount = inputLineCount + 1
                                frmDataInserter.inputLineLabel.Text = "입력 라인수 : " & inputLineCount
                            End If
                        Case "V3"
                            If isCheckedV3 = True Then
                                DataManageMentSystemMethod.GetDataV3(oneLineRow, inputLineCount, logFileName, fileNumber)
                                '변수 초기화 
                                DataManageMentSystemMethod.ClearData()
                                '입력라인수 증가식 추가
                                lineCount_V3 = lineCount_V3 + 1
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
                dicParams.Clear()

                connectionDB.saveData2(DB_Query.InsertDataLog, Nothing, dicParams, False) '완료 로그 DB에 INSERT
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | " & "(AM) MainDayJob : 완료 로그 DB INSERT 완료!" & vbCrLf)
                FileClose()

                '완료 로그 작성
                Utilities.DailyInputLog(logFileName, fileNumber, CDbl(inputLineCount), CDbl(progressLineCount))
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | " & "(AM) MainDayJob : 완료 로그 파일 작성 완료!" & vbCrLf)
                FileClose()

                'BATCH(V2:채권발행정보)로 들어온 발행정보를 HISTORY 테이블(PABNTD00)에 넣기
                KoscomMethod.CmdHistory_Click_PBN_LOAD_BOND_INFO(logFileName, fileNumber)

                '평가에 영향을 주지않는 발행정보 일괄복사프로시져 추가
                '당일 BATCH로 수신받은 코스콤 발행정보 중 평가에 영향을 미치지 않는 정보에 한해, 일괄적으로 PABNTD01에 업데이트 하고, PABNTD00에 히스토리를 남김.
                KoscomMethod.CmdHistory_Click_PBN_COPY_BOND_INFO(logFileName, fileNumber)

                Utilities.Job_End("1500")

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | " & "(AM) MainDayJob : 작업 완료 !!!!" & vbCrLf)
                FileClose()

                Return True

            Else
                MsgBox("예탁원 자료수신(오전) 작업이 완료 되지 않았습니다.")
                Return False
            End If
        Catch ex As Exception
            MsgBox("오전 작업에서 오류 발생 : " & ex.ToString())
            Throw ex
        End Try
    End Function

    Public Function TodayDoCountGridWriteDay(ByVal locFile As String, logFileName As String, fileNumber As Integer)

        Try
            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "(AM) MainDayJob 처리 할 개수 파악 !" & vbCrLf)
            FileClose()

            localTrgFileReader = My.Computer.FileSystem.OpenTextFileReader(locFile, System.Text.Encoding.Default) '파일 읽을 API

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

            '처리 할 개수 확인 & 그리드에 기입 / TRCODE별 체크박스 체크
            For index = 0 To dtblDATA.Rows.Count - 1
                trcode = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TR")

                '개수 기입
                Select Case trcode
                    Case "VD"
                        frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "TDY_DOCNT", lineCount_Vd)
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
            Print(fileNumber, Date.Now & " | " & "(AM) MainDayJob 처리 할 개수 파악 완료!" & vbCrLf)
            FileClose()

        Catch ex As Exception

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "(AM) MainDayJob 처리 할 개수 파악 발생!" & vbCrLf)
            Print(fileNumber, Date.Now & " | " & "오류 내용 : " & vbCrLf)

            FileClose()

            MsgBox("(AM) MainDayJob 처리 할 개수 파악 중 오류 발생! 오류 내용 : " & ex.ToString())
            Throw ex
        End Try
    End Function
End Class
