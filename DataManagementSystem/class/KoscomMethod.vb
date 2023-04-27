Imports DevExpress.XtraEditors


Public Class KoscomMethod

    Public DataDefineDAO As New DataDefineDAO()
    Public Utilities As New Utilities()
    Public FTP_Agent As New FTP_Agent()
    Public RetailDataAndKRXInput As New RetailDataAndKRXInput()
    Public connectionDB As New DB_Agent()
    Public Agent As New Agent()
#Region "FuncKoscomSrcFile"

    '***************************************************************
    ' ※ 오전일 경우 ※
    ' ------ BONDAV0 + BONDAVD 파일 MERGE 

    ' ※ 오후일 경우 ※
    ' 1. 26 or 25번 서버에서 A0027, G3027.dat 가져오기
    ' 2. A0027 작업 -> G3027 작업 진행
    ' 3. BONDPV0.YYYYMMDD ->> Data_PM_yyyyMMdd_n.txt 파일 다운로드
    '***************************************************************

    Public Function FuncKoscomSrcFile(t_day As DateTimePicker, logFileName As String, fileNumber As Integer) As String

        Dim temp_fileReader As IO.StreamReader
        Dim temp_fileWriter As IO.StreamWriter
        Dim App_Path = System.AppDomain.CurrentDomain.BaseDirectory() ' 프로그램 현재 경로
        Dim ftpIP As String = "222.111.237.6"
        Dim ftpUserId As String = "koscom"
        Dim ftpUserPwd As String = "koscom123"

        Try
            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "FuncKoscomSrcFile : 작업 시작" & vbCrLf)
            FileClose()

            'KOSCOM FTP 파일 데이터 위치==============================================================================
            DataDefineDAO.am_loc_trgfile = "\batch\bond_info\BONDAV0." & Format(frmDataInserter.t_day.Value, "yyyyMMdd")
            DataDefineDAO.am_loc_temp_trgfile = App_Path & "전체수신자료\BONDAV0." & Format(frmDataInserter.t_day.Value, "yyyyMMdd")
            DataDefineDAO.am_vd_loc_trgfile = "\batch\bond_info\BONDAVD." & Format(frmDataInserter.t_day.Value, "yyyyMMdd")
            DataDefineDAO.am_vd_loc_temp_trgfile = App_Path & "전체수신자료\BONDAVD." & Format(frmDataInserter.t_day.Value, "yyyyMMdd")
            DataDefineDAO.am_end_trgfile = "\batch\bond_info\BOND_AM.END." & Format(frmDataInserter.t_day.Value, "yyyyMMdd")
            DataDefineDAO.am_end_temp_trgfile = App_Path & "전체수신자료\BOND_AM.END." & Format(frmDataInserter.t_day.Value, "yyyyMMdd")
            DataDefineDAO.pm_loc_trgfile = "\batch\bond_info\BONDPV0." & Format(frmDataInserter.t_day.Value, "yyyyMMdd")
            DataDefineDAO.pm_loc_temp_trgfile = App_Path & "전체수신자료\BONDPV0." & Format(frmDataInserter.t_day.Value, "yyyyMMdd")
            DataDefineDAO.pm_end_trgfile = "\batch\bond_info\BOND_PM.end." & Format(frmDataInserter.t_day.Value, "yyyyMMdd")
            DataDefineDAO.pm_end_temp_trgfile = App_Path & "전체수신자료\BOND_PM.END." & Format(frmDataInserter.t_day.Value, "yyyyMMdd")
            DataDefineDAO.v_loc_trgfile = App_Path & "Data\V_DATA_" & Format(frmDataInserter.t_day.Value, "yyyyMMdd") & ".TXT"
            'KOSCOM FTP 파일 데이터 위치==============================================================================

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "FuncKoscomSrcFile : 파일 경로 변수 선언 작업 완료" & vbCrLf)
            FileClose()

            If frmDataInserter.opt_Day.Checked = True Then '오전

                If Utilities.checkFinishedJobDay(logFileName, fileNumber) = True Then
                    MsgBox("오전 자료관리 시스템이 이미 실행되었습니다.")
                    Application.Exit()
                End If

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | " & "FuncKoscomSrcFile : AM 작업 시작" & vbCrLf)
                FileClose()

                FTP_Agent = New FTP_Agent(ftpIP, ftpUserId, ftpUserPwd)


                ' 파일 확인 변수를 먼저 생성 및 초기화 하는 이유
                ' 장외채권복구 프로그램을 통해 어제가 평일이지만, 공휴일일 경우 파일 복사를 먼저 진행하기 때문에 
                ' 해당 영업일 파일이 있을 수 있다. 그러므로 파일경로의 파일을 먼저 확인하는 작업을 진행함. 
                Dim fileCheckTodayBondV0 As New System.IO.FileInfo(DataDefineDAO.am_loc_temp_trgfile)
                Dim fileCheckTodayBondAvd As New System.IO.FileInfo(DataDefineDAO.am_vd_loc_temp_trgfile)

                ' 전체수신자료에 작업을 진행 할 파일 (BONDAV0, BONDAVD)이 없으면 FTP 파일 다운로드 진행 
                If fileCheckTodayBondAvd.Exists = False And fileCheckTodayBondV0.Exists = False Then

                    '오전 end파일 체크
                    If FTP_Agent.FtpFileExists(DataDefineDAO.am_end_trgfile) = False Then
                        MsgBox("오류 - 증권전산원(수신) 오전 FTP수신데이터 " & DataDefineDAO.am_end_trgfile & " 없습니다. 확인하세요")
                        Application.Exit()
                    End If

                    '오전 자료 - BONDAV0.YYYYMMDD : 채권발행정보(V1), 채권발행기관코드_거래소(V3), 회사코드정보_KOSCOM(V7), 채권종류코드_KOSCOM(V8), 매매종류코드_KOSCOM(V9)
                    If FTP_Agent.FtpFileExists(DataDefineDAO.am_loc_trgfile) = False Then
                        MsgBox("오류 - 증권전산원(수신) 오전 FTP수신데이터 " & DataDefineDAO.am_loc_trgfile & " 없습니다. 확인하세요")
                        Application.Exit()
                    End If

                    '오전 자료 - BONDAVD.YYYYMMDD : 채권대용가정보(VD)
                    If FTP_Agent.FtpFileExists(DataDefineDAO.am_vd_loc_trgfile) = False Then
                        MsgBox("오류 - 증권전산원(수신) 오전 FTP수신데이터 " & DataDefineDAO.am_vd_loc_trgfile & " 없습니다. 확인하세요")
                        Application.Exit()
                    End If

                    FTP_Agent.Download(DataDefineDAO.am_loc_trgfile, DataDefineDAO.am_loc_temp_trgfile, True) 'BONDAV0.yyyyMMdd파일 다운로드
                    FTP_Agent.Download(DataDefineDAO.am_vd_loc_trgfile, DataDefineDAO.am_vd_loc_temp_trgfile, True) 'BONDAVD.YYYYMMDD파일 
                End If

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | " & "FuncKoscomSrcFile : (AM) / FTP 파일 체크 및 다운로드 진행 완료" & vbCrLf)
                FileClose()

                '오전 자료 두개를 한파일로 합친후 Local PC로 COPY파일의 변수를 다시 정의한다.(Data_am_YYYYMMDD.txt)
                DataDefineDAO.v_loc_trgfile = App_Path & "DATA\Data_AM_" & Format(frmDataInserter.t_day.Value, "yyyyMMdd") & "_d.txt"

                Dim fileCheckTodayMergeFile As New System.IO.FileInfo(DataDefineDAO.v_loc_trgfile)

                If fileCheckTodayMergeFile.Exists = False Then

                    temp_fileReader = My.Computer.FileSystem.OpenTextFileReader(DataDefineDAO.am_loc_temp_trgfile, System.Text.Encoding.Default) '파일 읽을 API

                    temp_fileWriter = My.Computer.FileSystem.OpenTextFileWriter(DataDefineDAO.am_vd_loc_temp_trgfile, True, System.Text.Encoding.Default) '파일 쓸 API

                    Dim firstLineChangeVar As Integer = 0

                    DataDefineDAO.tempLine = "" '기존에 한번 위에서 사용했기 때문에 초기화 
                    While Not temp_fileReader.EndOfStream

                        DataDefineDAO.tempLine = temp_fileReader.ReadLine() '한 줄 씩 읽어오기

                        If firstLineChangeVar = 0 Then
                            temp_fileWriter.WriteLine(vbCrLf & DataDefineDAO.tempLine)
                        Else
                            temp_fileWriter.WriteLine(DataDefineDAO.tempLine)

                        End If

                        firstLineChangeVar = firstLineChangeVar + 1 ' 첫줄은 한줄 변경해서 넣어야 하기 때문에 코드 추가 
                    End While

                    temp_fileReader.Close()
                    temp_fileWriter.Close()

                    System.IO.File.Copy(DataDefineDAO.am_vd_loc_temp_trgfile, DataDefineDAO.v_loc_trgfile) '파일 복사

                End If

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | " & "FuncKoscomSrcFile : (AM) / BONDV0 -> BONDV0 파일 덮어쓰기 후 파일 복사 완료 / 파일 경로 : " & DataDefineDAO.v_loc_trgfile & " | FuncKoscomSrcFile : 작업 완료" & vbCrLf)
                FileClose()

            Else '오후 

                '선행작업 여부 확인 (1)
                If Utilities.checkFinishedJobNight8000() = False Then
                    Application.Exit()
                End If

                '선행작업 여부 확인 (2)
                If Utilities.checkFinishedJobNigth1600(logFileName, fileNumber) = False Then
                    Application.Exit()
                End If

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | " & "FuncKoscomSrcFile : PM 작업 시작" & vbCrLf)
                FileClose()

                FTP_Agent = New FTP_Agent(ftpIP, ftpUserId, ftpUserPwd)

                If FTP_Agent.FtpFileExists(DataDefineDAO.pm_end_trgfile) = False Then
                    MsgBox("오류 - 증권전산원(수신) 오후 FTP수신데이터 " & DataDefineDAO.pm_end_trgfile & " 없습니다. 확인하세요")
                    Application.Exit()
                End If

                '오전 자료 - BONDPV0.YYYYMMDD : 채권발행정보(V1), 채권발행기관코드_거래소(V3), 회사코드정보_KOSCOM(V7), 채권종류코드_KOSCOM(V8), 매매종류코드_KOSCOM(V9)
                If FTP_Agent.FtpFileExists(DataDefineDAO.pm_loc_trgfile) = False Then
                    MsgBox("오류 - 오후 FTP수신데이터 " & DataDefineDAO.pm_loc_trgfile & " 없습니다. 확인하세요")
                    Application.Exit()
                End If

                '오후 자료를 Local PC로 COPY파일의 변수를 다시 정의한다.(Data_PM_YYYYMMDD.txt)
                DataDefineDAO.v_loc_trgfile = App_Path & "\Data\" & "Data_PM_" & Format(frmDataInserter.t_day.Value, "yyyyMMdd") & "_n.txt"

                '오후 자료를 Local PC로 COPY한다.(temp_pm_YYYYMMDD.txt) ->>  pm_loc_trgfile 파일 다운로드 진행  / 기존 BOND 저녁 파일도 같이 수신 -> 파일 이력 확인용 
                FTP_Agent.Download(DataDefineDAO.pm_loc_trgfile, DataDefineDAO.pm_loc_temp_trgfile, True)
                FTP_Agent.Download(DataDefineDAO.pm_loc_trgfile, DataDefineDAO.v_loc_trgfile, True)

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | " & "FuncKoscomSrcFile : FuncKoscomSrcFile : (PM) / BONDPM FTP 파일 다운 완료 | FuncKoscomSrcFile : 작업 완료" & vbCrLf)
                FileClose()

            End If

            Return DataDefineDAO.v_loc_trgfile

            Exit Function

        Catch ex As Exception
            If ex.ToString().Contains("Data_AM_") Then
                MsgBox(App_Path & "Data_AM_" & frmDataInserter.t_day.Value & "_d.txt 파일이 이미 존재합니다.")
                Throw ex
            ElseIf ex.ToString().Contains("Data_PM_") Then
                MsgBox(App_Path & "Data_PM_" & frmDataInserter.t_day.Value & "_n.txt 파일이 이미 존재합니다.")
                Throw ex
            Else
                MsgBox(ex.ToString())
                Throw ex
            End If
        End Try
    End Function
#End Region

#Region "V2 작업"
    Public Function GetDataV2(ByVal oneLineRow As String, inputLineCount As Integer, logFileName As String, fileNumber As Integer)
        DataDefineDAO.CheckLenLine = Utilities.ByteLen(oneLineRow)

        If frmDataInserter.optNight.Checked = True Then
            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | (PM) GetDataV2 작업 시작! 행 : " & inputLineCount & vbCrLf)
            FileClose()
        Else
            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | (AM) GetDataV2 작업 시작! 행 : " & inputLineCount & vbCrLf)
            FileClose()
        End If

        Try
            ' 라인 한줄의 길이가 2000byte가  아니라면 진행
            If DataDefineDAO.CheckLenLine <> 2000 Then
                GoTo SKIP
            End If

            Dim tagCountValue As Integer ' "최종" 으로 종목 조회 된 개수 
            Dim index As Integer 'For 문 인덱스
            Dim queryVar As String = "" '쿼리의 명령문을 추가적으로 넣기 위한 변수
            Dim colNameVar_V2 As String = "" 'COL_NAME BINDING 변수(쿼리의 명령문을 추가적으로 넣기 위한 변수)
            Dim dicParams = New Dictionary(Of String, String) ' 딕셔너리 선언

            dicParams.Add(":tr", "V2")

            Dim dbTableSeqData As DataTable = connectionDB.getData2(DB_Query.STR_SEQ_Query, dicParams, True, False)
            Dim dbTableLenData As DataTable = connectionDB.getData2(DB_Query.STR_LEN_Query, dicParams, True, False)
            Dim dbTableColNameData As DataTable = connectionDB.getData2(DB_Query.COL_NAME_Query, dicParams, True, False) ' 한 문장에서 필요한 값을 가져오기 위한 끝 인덱스 배열 DB 에서 호출
            Dim dbTableTableNameData As DataTable = connectionDB.getData2(DB_Query.TABLE_NAME_Query, dicParams, True, False) ' TRCODE 별 테이블 명 값 가져오기

            Dim dbTableDataStartArray(dbTableSeqData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_SEQ)
            Dim dbTableDataEndArray(dbTableLenData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_LEN)
            Dim dbTableDataColNameArray(dbTableColNameData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(COL_NAME)

            For index = 0 To dbTableDataStartArray.Length - 2
                dbTableDataStartArray(index) = dbTableSeqData.Rows(index).Item("STR_SEQ")
                dbTableDataEndArray(index) = dbTableLenData.Rows(index).Item("STR_LEN")
                dbTableDataColNameArray(index) = dbTableColNameData.Rows(index).Item("COL_NAME")
            Next

            '변수 초기화 
            DataDefineDAO.V2_DATA(0) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(0), dbTableDataEndArray(0), 3) '적용일
            DataDefineDAO.V2_DATA(0) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(0), "yyyyMMdd", Nothing) & "'"

            DataDefineDAO.V2_DATA(1) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(1), dbTableDataEndArray(1)) '종목코드
            DataDefineDAO.V2_DATA(2) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(2), dbTableDataEndArray(2)) '레코드처리구분코드

            DataDefineDAO.V2_DATA(3) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(3), dbTableDataEndArray(3)) '종목명
            DataDefineDAO.V2_DATA(4) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(4), dbTableDataEndArray(4)) '종목약명
            DataDefineDAO.V2_DATA(5) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(5), dbTableDataEndArray(5)) '종목영문명
            DataDefineDAO.V2_DATA(6) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(6), dbTableDataEndArray(6)) '종목영문약명
            DataDefineDAO.V2_DATA(7) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(7), dbTableDataEndArray(7)) '발행기관코드
            'Code Converting
            'Y:상장, D:상장폐지, N:비상장, I:미발행, E:기타) --> 0:비상장, 1:상장, 2:기타, 9:상장폐지(상환조치 포함)
            DataDefineDAO.V2_DATA(8) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(8), dbTableDataEndArray(8)) '채권상장구분코드
            Dim LIST_TYPE_CODE As String

            Select Case Replace(DataDefineDAO.V2_DATA(8), "'", "")
                Case "Y"
                    LIST_TYPE_CODE = 1
                Case "N"
                    LIST_TYPE_CODE = 0
                Case "D"
                    LIST_TYPE_CODE = 9
                Case Else
                    LIST_TYPE_CODE = 2
            End Select

            DataDefineDAO.V2_DATA(8) = "" & "'" & LIST_TYPE_CODE & "'"


            DataDefineDAO.V2_DATA(9) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(9), dbTableDataEndArray(9)) '채권분류코드 
            DataDefineDAO.V2_DATA(10) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(10), dbTableDataEndArray(10)) '채권유형코드
            DataDefineDAO.V2_DATA(11) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(11), dbTableDataEndArray(11)) '특수채발행체코드
            DataDefineDAO.V2_DATA(12) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(12), dbTableDataEndArray(12)) '지방채구분코드
            DataDefineDAO.V2_DATA(13) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(13), dbTableDataEndArray(13)) '채권보증구분코드
            DataDefineDAO.V2_DATA(14) = CDec(Utilities.CatchCol(oneLineRow, dbTableDataStartArray(14), dbTableDataEndArray(14), 3)) * 0.01 '지급보증율
            DataDefineDAO.V2_DATA(15) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(15), dbTableDataEndArray(15)) '특이채권유형코드
            DataDefineDAO.V2_DATA(16) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(16), dbTableDataEndArray(16)) '옵션부사채코드
            DataDefineDAO.V2_DATA(17) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(17), dbTableDataEndArray(17)) '이자지급방법코드

            'Code Converting
            '고정금리형(11:할인채, 12:복리채, 13:이표채, 14:단리채, 15:복5단2, 19:기타),
            '변동금리형(21:이표채, 22:복리채, 23:단리채, 29:기타) --> 11:할인채, 12:복리채, 13:이표채, 14:단리채, 15:복5단2, 21:변동금리, 99:기타

            Dim INT_PAY_TYPE_CODE As Integer

            If Replace(DataDefineDAO.V2_DATA(17), "'", "") = "" Or Replace(DataDefineDAO.V2_DATA(17), "'", "") = "Null" Then
                INT_PAY_TYPE_CODE = 0
            ElseIf CInt(Replace(DataDefineDAO.V2_DATA(17), "'", "")) >= 21 Then
                INT_PAY_TYPE_CODE = 21
            ElseIf CInt(Replace(DataDefineDAO.V2_DATA(17), "'", "")) = 19 Then
                INT_PAY_TYPE_CODE = 99
            Else
                INT_PAY_TYPE_CODE = CInt(Replace(DataDefineDAO.V2_DATA(17), "'", ""))
            End If

            DataDefineDAO.V2_DATA(17) = "" & "'" & INT_PAY_TYPE_CODE & "'"

            DataDefineDAO.V2_DATA(18) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(18), dbTableDataEndArray(18)) '리스트채권상환유형코드
            DataDefineDAO.V2_DATA(19) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(19), dbTableDataEndArray(19)) '채권발행방법코드
            DataDefineDAO.V2_DATA(20) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(20), dbTableDataEndArray(20)) '자산유동화구분코드
            DataDefineDAO.V2_DATA(21) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(21), dbTableDataEndArray(21)) '채무변제순위구분코드

            DataDefineDAO.V2_DATA(22) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(22), dbTableDataEndArray(22), 3) '발행일자
            If DataDefineDAO.V2_DATA(22) = "00000000" Then
                DataDefineDAO.V2_DATA(22) = "Null"
            Else
                DataDefineDAO.V2_DATA(22) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(22), "yyyyMMdd", Nothing) & "'"

            End If

            DataDefineDAO.V2_DATA(23) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(23), dbTableDataEndArray(23), 3) '상환일자
            If DataDefineDAO.V2_DATA(23) = "00000000" Then
                DataDefineDAO.V2_DATA(23) = "Null"
            Else
                DataDefineDAO.V2_DATA(23) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(23), "yyyyMMdd", Nothing) & "'"

            End If


            DataDefineDAO.V2_DATA(24) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(24), dbTableDataEndArray(24), 3) '매출일자
            If DataDefineDAO.V2_DATA(24) = "00000000" Then
                DataDefineDAO.V2_DATA(24) = "Null"
            Else
                DataDefineDAO.V2_DATA(24) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(24), "yyyyMMdd", Nothing) & "'"

            End If

            DataDefineDAO.V2_DATA(25) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(25), dbTableDataEndArray(25), 3) '최초이자지급일자
            If DataDefineDAO.V2_DATA(25) = "00000000" Then
                DataDefineDAO.V2_DATA(25) = "Null"
            Else
                DataDefineDAO.V2_DATA(25) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(25), "yyyyMMdd", Nothing) & "'"

            End If

            DataDefineDAO.V2_DATA(26) = CDec(Utilities.CatchCol(oneLineRow, dbTableDataStartArray(26), dbTableDataEndArray(26), 3)) * 0.01 '채권발행율
            DataDefineDAO.V2_DATA(27) = CDec(Utilities.CatchCol(oneLineRow, dbTableDataStartArray(27), dbTableDataEndArray(27), 3)) * 0.01 '표면이자율
            DataDefineDAO.V2_DATA(28) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(28), dbTableDataEndArray(28), 3) '이자지급계산월수
            DataDefineDAO.V2_DATA(29) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(29), dbTableDataEndArray(29)) '이프지급방법코드
            DataDefineDAO.V2_DATA(30) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(30), dbTableDataEndArray(30)) '채권이자지급일 기준구분코드

            If frmDataInserter.optNight.Checked = True Then
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) GetDataV2 변수 초기화 작업 채권이자지급일 기준구분코드 작업까지 완료! " & vbCrLf)
                FileClose()
            Else
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) GetDataV2 변수 초기화 작업 채권이자지급일 기준구분코드 작업까지 완료!" & vbCrLf)
                FileClose()
            End If

            DataDefineDAO.V2_DATA(31) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(31), dbTableDataEndArray(31)) '이자월말구분코드
            DataDefineDAO.V2_DATA(32) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(32), dbTableDataEndArray(32)) '이자원 단위미만 처리코드
            DataDefineDAO.V2_DATA(33) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(33), dbTableDataEndArray(33), 3) '이자지급단위월수
            DataDefineDAO.V2_DATA(34) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(34), dbTableDataEndArray(34)) '채권매출형태코드
            DataDefineDAO.V2_DATA(35) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(35), dbTableDataEndArray(35)) '채권선매출이자지급방법코드
            DataDefineDAO.V2_DATA(36) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(36), dbTableDataEndArray(36), 3) '발행금액
            DataDefineDAO.V2_DATA(37) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(37), dbTableDataEndArray(37)) '일괄금액확정여부
            DataDefineDAO.V2_DATA(38) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(38), dbTableDataEndArray(38)) '통화구분코드
            DataDefineDAO.V2_DATA(39) = CDec(Utilities.CatchCol(oneLineRow, dbTableDataStartArray(39), dbTableDataEndArray(39), 3)) * 0.01 '만기상환비율
            DataDefineDAO.V2_DATA(40) = CDec(Utilities.CatchCol(oneLineRow, dbTableDataStartArray(40), dbTableDataEndArray(40), 3)) * 0.01 '보장수익률

            DataDefineDAO.V2_DATA(41) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(41), dbTableDataEndArray(41), 3) '보장수익률적용일자
            If DataDefineDAO.V2_DATA(41) = "00000000" Then
                DataDefineDAO.V2_DATA(41) = "Null"
            Else
                DataDefineDAO.V2_DATA(41) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(41), "yyyyMMdd", Nothing) & "'"
            End If

            DataDefineDAO.V2_DATA(42) = CDec(Utilities.CatchCol(oneLineRow, dbTableDataStartArray(42), dbTableDataEndArray(42), 3)) * 0.01 '추가수익율

            DataDefineDAO.V2_DATA(43) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(43), dbTableDataEndArray(43), 3) '추가수익률적용일자
            If DataDefineDAO.V2_DATA(43) = "00000000" Then
                DataDefineDAO.V2_DATA(43) = "Null"
            Else
                DataDefineDAO.V2_DATA(43) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(43), "yyyyMMdd", Nothing) & "'"

            End If

            DataDefineDAO.V2_DATA(44) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(44), dbTableDataEndArray(44), 3) '시설자금
            DataDefineDAO.V2_DATA(45) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(45), dbTableDataEndArray(45), 3) '운영자금
            DataDefineDAO.V2_DATA(46) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(46), dbTableDataEndArray(46), 3) '차환자금
            DataDefineDAO.V2_DATA(47) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(47), dbTableDataEndArray(47), 3) '기타자금
            DataDefineDAO.V2_DATA(48) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(48), dbTableDataEndArray(48)) '기명여부
            DataDefineDAO.V2_DATA(49) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(49), dbTableDataEndArray(49)) '과세여부
            DataDefineDAO.V2_DATA(50) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(50), dbTableDataEndArray(50)) '채권주관회사코드
            DataDefineDAO.V2_DATA(51) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(51), dbTableDataEndArray(51)) '지급보증기관코드

            If frmDataInserter.optNight.Checked = True Then
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) GetDataV2 변수 초기화 작업 지급보증기관코드 작업까지 완료! " & vbCrLf)
                FileClose()
            Else
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) GetDataV2 변수 초기화 작업 지급보증기관코드 작업까지 완료!" & vbCrLf)
                FileClose()
            End If

            'Code Converting
            Dim GUARANT_ORG_ID As String

            If Replace(DataDefineDAO.V2_DATA(51), "'", "") = "0000" Then
                GUARANT_ORG_ID = ""
            Else
                GUARANT_ORG_ID = Replace(DataDefineDAO.V2_DATA(51), "'", "")
            End If

            DataDefineDAO.V2_DATA(51) = "" & "'" & GUARANT_ORG_ID & "'"
            DataDefineDAO.V2_DATA(52) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(52), dbTableDataEndArray(52)) '수탁기관코드
            DataDefineDAO.V2_DATA(53) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(53), dbTableDataEndArray(53)) '등록기관코드
            DataDefineDAO.V2_DATA(54) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(54), dbTableDataEndArray(54)) '원리금지급대행기관코드
            DataDefineDAO.V2_DATA(55) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(55), dbTableDataEndArray(55)) '종목단축코드
            DataDefineDAO.V2_DATA(56) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(56), dbTableDataEndArray(56)) '주식관련사채권리구분코드
            DataDefineDAO.V2_DATA(57) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(57), dbTableDataEndArray(57)) '대상종목코드
            DataDefineDAO.V2_DATA(58) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(58), dbTableDataEndArray(58)) '대상종목명
            DataDefineDAO.V2_DATA(59) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(59), dbTableDataEndArray(59), 3) '주식관련사채권리행사가격
            DataDefineDAO.V2_DATA(60) = CDec(Utilities.CatchCol(oneLineRow, dbTableDataStartArray(60), dbTableDataEndArray(60), 3)) * 0.01 '행사비율

            DataDefineDAO.V2_DATA(61) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(61), dbTableDataEndArray(61), 3) '행사개시일자
            If DataDefineDAO.V2_DATA(61) = "00000000" Then
                DataDefineDAO.V2_DATA(61) = "Null"
            Else
                DataDefineDAO.V2_DATA(61) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(61), "yyyyMMdd", Nothing) & "'"

            End If

            DataDefineDAO.V2_DATA(62) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(62), dbTableDataEndArray(62), 3) '행사종료일자
            If DataDefineDAO.V2_DATA(62) = "00000000" Then
                DataDefineDAO.V2_DATA(62) = "Null"
            Else
                DataDefineDAO.V2_DATA(62) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(62), "yyyyMMdd", Nothing) & "'"

            End If

            DataDefineDAO.V2_DATA(63) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(63), dbTableDataEndArray(63)) '청구기관코드
            DataDefineDAO.V2_DATA(64) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(64), dbTableDataEndArray(64)) '배당기산일구분코드
            DataDefineDAO.V2_DATA(65) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(65), dbTableDataEndArray(65)) '이익참가권누적여부

            'Code Converting
            '누적적:Y, 비누적적:N --> 0:비누적적, 1:누적적

            Dim PB_ACC_GB As Integer

            Select Case Replace(DataDefineDAO.V2_DATA(10), "'", "")
                Case "Y"
                    PB_ACC_GB = 1
                Case "N"
                    PB_ACC_GB = 0
            End Select

            DataDefineDAO.V2_DATA(65) = "" & "'" & PB_ACC_GB & "'"
            DataDefineDAO.V2_DATA(66) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(66), dbTableDataEndArray(66)) '신주인수권행사이후종목
            DataDefineDAO.V2_DATA(67) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(67), dbTableDataEndArray(67)) '분할상환구분코드
            DataDefineDAO.V2_DATA(68) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(68), dbTableDataEndArray(68), 3) '균등상환액
            DataDefineDAO.V2_DATA(69) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(69), dbTableDataEndArray(69), 3) '거치개월수

            'Code Converting
            Dim YY As Integer
            Dim MM As Integer
            Dim DD As Integer
            Dim UNREDEEM_TERM As String
            If Replace(DataDefineDAO.V2_DATA(69), "'", "") <> "" Then
                YY = CDbl(Replace(DataDefineDAO.V2_DATA(69), "'", "")) / 12
                MM = CDbl(Replace(DataDefineDAO.V2_DATA(69), "'", "")) Mod 12
                DD = 0
                UNREDEEM_TERM = Format(YY, "00") + Format(MM, "00") + Format(DD, "00")
            Else
                UNREDEEM_TERM = Replace(DataDefineDAO.V2_DATA(69), "'", "")
            End If

            DataDefineDAO.V2_DATA(69) = "" & "'" & UNREDEEM_TERM & "'"

            DataDefineDAO.V2_DATA(70) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(70), dbTableDataEndArray(70)) '상환기간이자구분코드
            DataDefineDAO.V2_DATA(71) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(71), dbTableDataEndArray(71), 3) '분할상환횟수
            DataDefineDAO.V2_DATA(72) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(72), dbTableDataEndArray(72)) '이자율결정기타기준금리명
            DataDefineDAO.V2_DATA(73) = CDec(Utilities.CatchCol(oneLineRow, dbTableDataStartArray(73), dbTableDataEndArray(73), 3)) * 0.01 '가산금리
            DataDefineDAO.V2_DATA(74) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(74), dbTableDataEndArray(74)) '이자율결정시점코드
            DataDefineDAO.V2_DATA(75) = CDec(Utilities.CatchCol(oneLineRow, dbTableDataStartArray(75), dbTableDataEndArray(75), 3)) * 0.01 '상한표면이자율
            DataDefineDAO.V2_DATA(76) = CDec(Utilities.CatchCol(oneLineRow, dbTableDataStartArray(76), dbTableDataEndArray(76), 3)) * 0.01 '하한표면이자율

            DataDefineDAO.V2_DATA(77) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(77), dbTableDataEndArray(77), 3) '이자율결정기준일자
            If DataDefineDAO.V2_DATA(77) = "00000000" Then
                DataDefineDAO.V2_DATA(77) = "Null"
            Else
                DataDefineDAO.V2_DATA(77) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(77), "yyyyMMdd", Nothing) & "'"

            End If

            DataDefineDAO.V2_DATA(78) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(78), dbTableDataEndArray(78)) '특이발행조건내용
            DataDefineDAO.V2_DATA(79) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(79), dbTableDataEndArray(79)) '은행휴무일이자지급방법
            DataDefineDAO.V2_DATA(80) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(80), dbTableDataEndArray(80)) '신종자본증권여부

            'Code Converting
            'Y:해당시, N:미해당시 --> 0:해당사항없음, 1:영구채에 준하는 만기구조
            Dim MAT_STRUCTURE_CODE As String

            Select Case Replace(DataDefineDAO.V2_DATA(80), "'", "")
                Case "Y"
                    MAT_STRUCTURE_CODE = 1
                Case "N"
                    MAT_STRUCTURE_CODE = 0
            End Select

            DataDefineDAO.V2_DATA(80) = "" & "'" & MAT_STRUCTURE_CODE & "'"

            DataDefineDAO.V2_DATA(81) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(81), dbTableDataEndArray(81)) '조건부자본증권유형구분코드
            DataDefineDAO.V2_DATA(82) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(82), dbTableDataEndArray(82)) '표현이자율확정여부

            'Code Converting
            'Y:확정, N:미확정 --> 1:확정, 2:미확정
            Dim INTEREST_CODE As String

            Select Case Replace(DataDefineDAO.V2_DATA(82), "'", "")
                Case "Y"
                    INTEREST_CODE = 1
                Case "N"
                    INTEREST_CODE = 0
            End Select

            DataDefineDAO.V2_DATA(82) = "" & "'" & INTEREST_CODE & "'"

            If frmDataInserter.optNight.Checked = True Then
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) GetDataV2 변수 초기화 작업 표현이자율확정여부 작업까지 완료! " & vbCrLf)
                FileClose()
            Else
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) GetDataV2 변수 초기화 작업 표현이자율확정여부 작업까지 완료!" & vbCrLf)
                FileClose()
            End If

            DataDefineDAO.V2_DATA(83) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(83), dbTableDataEndArray(83)) '채권스트립구분코드
            DataDefineDAO.V2_DATA(84) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(84), dbTableDataEndArray(84)) '대상원본채권코드
            DataDefineDAO.V2_DATA(85) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(85), dbTableDataEndArray(85), 3) '스트립미분리잔액
            DataDefineDAO.V2_DATA(86) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(86), dbTableDataEndArray(86)) '물가연동구분
            DataDefineDAO.V2_DATA(87) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(87), dbTableDataEndArray(87), 3) '발행일참조지수
            DataDefineDAO.V2_DATA(88) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(88), dbTableDataEndArray(88)) '표면이자율결정기준금리코드
            DataDefineDAO.V2_DATA(89) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(89), dbTableDataEndArray(89)) '채권단수일이자기준금리코드
            DataDefineDAO.V2_DATA(90) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(90), dbTableDataEndArray(90)) '은행유무일이자기준금리코드
            DataDefineDAO.V2_DATA(91) = CDec(Utilities.CatchCol(oneLineRow, dbTableDataStartArray(91), dbTableDataEndArray(91), 3)) * 0.01 '은행휴무일이자경과이자율
            DataDefineDAO.V2_DATA(92) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(92), dbTableDataEndArray(92)) '은행휴무일원금지급방법코드
            DataDefineDAO.V2_DATA(93) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(93), dbTableDataEndArray(93)) '은행휴무일원금기준금리코드
            DataDefineDAO.V2_DATA(94) = CDec(Utilities.CatchCol(oneLineRow, dbTableDataStartArray(94), dbTableDataEndArray(94), 3)) * 0.01 '은행휴무일원금경과이자율
            DataDefineDAO.V2_DATA(95) = CDec(Utilities.CatchCol(oneLineRow, dbTableDataStartArray(95), dbTableDataEndArray(95), 3)) * 0.01 '낙찰금리

            DataDefineDAO.V2_DATA(96) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(96), dbTableDataEndArray(96), 3) '콜행사개시일자1차
            If DataDefineDAO.V2_DATA(96) = "00000000" Then
                DataDefineDAO.V2_DATA(96) = "Null"
            Else
                DataDefineDAO.V2_DATA(96) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(96), "yyyyMMdd", Nothing) & "'"

            End If

            DataDefineDAO.V2_DATA(97) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(97), dbTableDataEndArray(97), 3) '콜행사종료일자1차
            If DataDefineDAO.V2_DATA(97) = "00000000" Then
                DataDefineDAO.V2_DATA(97) = "Null"
            Else
                DataDefineDAO.V2_DATA(97) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(97), "yyyyMMdd", Nothing) & "'"

            End If

            DataDefineDAO.V2_DATA(98) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(98), dbTableDataEndArray(98), 3) '콜행사개시일자2차
            If DataDefineDAO.V2_DATA(98) = "00000000" Then
                DataDefineDAO.V2_DATA(98) = "Null"
            Else
                DataDefineDAO.V2_DATA(98) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(98), "yyyyMMdd", Nothing) & "'"

            End If

            DataDefineDAO.V2_DATA(99) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(99), dbTableDataEndArray(99), 3) '콜행사종료일자2차
            If DataDefineDAO.V2_DATA(99) = "00000000" Then
                DataDefineDAO.V2_DATA(99) = "Null"
            Else
                DataDefineDAO.V2_DATA(99) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(99), "yyyyMMdd", Nothing) & "'"

            End If

            DataDefineDAO.V2_DATA(100) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(100), dbTableDataEndArray(100)) '콜행사사유

            DataDefineDAO.V2_DATA(101) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(101), dbTableDataEndArray(101), 3) '풋행사개시일자1차
            If DataDefineDAO.V2_DATA(101) = "00000000" Then
                DataDefineDAO.V2_DATA(101) = "Null"
            Else
                DataDefineDAO.V2_DATA(101) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(101), "yyyyMMdd", Nothing) & "'"

            End If

            DataDefineDAO.V2_DATA(102) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(102), dbTableDataEndArray(102), 3) '풋행사종료일자1차
            If DataDefineDAO.V2_DATA(102) = "00000000" Then
                DataDefineDAO.V2_DATA(102) = "Null"
            Else
                DataDefineDAO.V2_DATA(102) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(102), "yyyyMMdd", Nothing) & "'"

            End If

            DataDefineDAO.V2_DATA(103) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(103), dbTableDataEndArray(103), 3) '풋행사개시일자2차
            If DataDefineDAO.V2_DATA(103) = "00000000" Then
                DataDefineDAO.V2_DATA(103) = "Null"
            Else
                DataDefineDAO.V2_DATA(103) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(103), "yyyyMMdd", Nothing) & "'"

            End If

            DataDefineDAO.V2_DATA(104) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(104), dbTableDataEndArray(104), 3) '풋행사종료일자2차
            If DataDefineDAO.V2_DATA(104) = "00000000" Then
                DataDefineDAO.V2_DATA(104) = "Null"
            Else
                DataDefineDAO.V2_DATA(104) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(104), "yyyyMMdd", Nothing) & "'"
            End If

            DataDefineDAO.V2_DATA(105) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(105), dbTableDataEndArray(105)) '풋행사사유
            DataDefineDAO.V2_DATA(106) = CDec(Utilities.CatchCol(oneLineRow, dbTableDataStartArray(106), dbTableDataEndArray(106), 3)) * 0.01 '원금보장율
            DataDefineDAO.V2_DATA(107) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(107), dbTableDataEndArray(107)) '기초자산
            DataDefineDAO.V2_DATA(108) = CDec(Utilities.CatchCol(oneLineRow, dbTableDataStartArray(108), dbTableDataEndArray(108), 3)) * 0.01 '상승참여율
            DataDefineDAO.V2_DATA(109) = CDec(Utilities.CatchCol(oneLineRow, dbTableDataStartArray(109), dbTableDataEndArray(109), 3)) * 0.01  '최대수익률
            DataDefineDAO.V2_DATA(110) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(110), dbTableDataEndArray(110)) 'ELS조건내용1
            DataDefineDAO.V2_DATA(111) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(111), dbTableDataEndArray(111)) 'ELS조건내용2
            DataDefineDAO.V2_DATA(112) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(112), dbTableDataEndArray(112)) '신용평가기관코드1
            DataDefineDAO.V2_DATA(113) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(113), dbTableDataEndArray(113)) '기관별신용평가등급1
            DataDefineDAO.V2_DATA(114) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(114), dbTableDataEndArray(114)) 'SF평가여부1
            DataDefineDAO.V2_DATA(115) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(115), dbTableDataEndArray(115)) '신용평가기관코드2
            DataDefineDAO.V2_DATA(116) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(116), dbTableDataEndArray(116)) '기관별신용평가등급2
            DataDefineDAO.V2_DATA(117) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(117), dbTableDataEndArray(117)) 'SF평가여부2
            DataDefineDAO.V2_DATA(118) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(118), dbTableDataEndArray(118)) '신용평가기관코드3
            DataDefineDAO.V2_DATA(119) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(119), dbTableDataEndArray(119)) '기관별신용평가등급3
            DataDefineDAO.V2_DATA(120) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(120), dbTableDataEndArray(120)) 'SF평가여부3
            DataDefineDAO.V2_DATA(121) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(121), dbTableDataEndArray(121)) '신용평가기관코드4
            DataDefineDAO.V2_DATA(122) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(122), dbTableDataEndArray(122)) '기관별신용평가등급4
            DataDefineDAO.V2_DATA(123) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(123), dbTableDataEndArray(123)) 'SF평가여부4
            DataDefineDAO.V2_DATA(124) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(124), dbTableDataEndArray(124)) '코스콤채권종류코드
            DataDefineDAO.V2_DATA(125) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(125), dbTableDataEndArray(125)) '코스콤회사코드
            DataDefineDAO.V2_DATA(126) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(126), dbTableDataEndArray(126), 3) '채권상장폐지일자
            If DataDefineDAO.V2_DATA(126) = "00000000" Then
                DataDefineDAO.V2_DATA(126) = "Null"
            Else
                DataDefineDAO.V2_DATA(126) = "'" & DateTime.ParseExact(DataDefineDAO.V2_DATA(126), "yyyyMMdd", Nothing) & "'"
            End If
            DataDefineDAO.V2_DATA(127) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(127), dbTableDataEndArray(127)) '채권상장폐지사유코드


            If frmDataInserter.opt_Day.Checked = True Then
                DataDefineDAO.V2_DATA(128) = "'AM'" 'KOSCOM 수신 AM, PM 구분
            Else
                DataDefineDAO.V2_DATA(128) = "'PM'" 'KOSCOM 수신 AM, PM 구분
            End If

            If frmDataInserter.optNight.Checked = True Then
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) GetDataV2 변수 초기화 작업 완료! | " & inputLineCount & vbCrLf)
                FileClose()
            Else
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) GetDataV2 변수 초기화 작업 완료! | " & inputLineCount & vbCrLf)
                FileClose()
            End If

            connectionDB.beginTrans()
            dicParams.Clear()

            '1. TAG:1 최종으로 종목 조회
            dicParams.Add(":v2_data_id", DataDefineDAO.V2_DATA(1))
            dicParams.Add(":bond_id", dbTableDataColNameArray(1))
            dicParams.Add(":tr_tablename", dbTableTableNameData.Rows(0).Item("TABLE_NAME"))

            Dim finalTagCheckV2 As DataTable = connectionDB.getData2(DB_Query.finalTagCheckV2, dicParams) ' TAG :1 데이터가 있는지 DB에서 확인하여 추출 

            tagCountValue = finalTagCheckV2.Rows(0).Item("CNT") ' 해당 ROW 값 추출 

            '2. TAG:1 최종으로 종목이 있을 경우 삭제한다.
            If CInt(tagCountValue) = 1 Then
                connectionDB.saveData2(DB_Query.finalDeleteTagV2, Nothing, dicParams, False)
            End If

            '적용일부터 130(코스콤 수신 AM, PM 구분까지 입력) 변수 값 초기화
            For index = 0 To 129
                If index = 129 Then
                    queryVar = queryVar & DataDefineDAO.V2_DATA(index)
                Else
                    queryVar = queryVar & DataDefineDAO.V2_DATA(index) & ","
                End If
            Next

            dicParams.Clear()

            'DB에서 가져온 COL_NAME값 변수에 대입
            For index = 0 To dbTableDataColNameArray.Length - 1
                If index = dbTableDataColNameArray.Length - 1 Then
                    colNameVar_V2 = colNameVar_V2 & dbTableDataColNameArray(index)
                Else
                    colNameVar_V2 = colNameVar_V2 & dbTableDataColNameArray(index) & ","
                End If
            Next

            dicParams.Add(":v2_final_Data", queryVar)
            dicParams.Add(":today_date", "'" & Date.Today & "'")
            dicParams.Add(":colNameVar_V2", colNameVar_V2)
            dicParams.Add(":tr_tablename", dbTableTableNameData.Rows(0).Item("TABLE_NAME"))

            '3. 당일입수 정보로 TAG 1:최종으로 입력한다.
            connectionDB.saveData2(DB_Query.finalInsertTagV2, Nothing, dicParams, False)

            dicParams.Clear()
            dicParams.Add(":v2_data_id", DataDefineDAO.V2_DATA(1))
            dicParams.Add(":today_date", "'" & Date.Today & "'")
            dicParams.Add(":colNameVar_V2", colNameVar_V2)
            dicParams.Add(":tr_tablename", dbTableTableNameData.Rows(0).Item("TABLE_NAME"))

            '4. 당일입수 정보로 TAG 2:HISTORY로 입력한다.
            connectionDB.saveData2(DB_Query.HistroyInsertTagV2, Nothing, dicParams, False)
            connectionDB.commitTrans()

            If frmDataInserter.optNight.Checked = True Then

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) V2) - |" & DataDefineDAO.V2_DATA(1) & " | " & DataDefineDAO.V2_DATA(0) & "에 추가 되어습니다. | " & inputLineCount & vbCrLf)
                FileClose()
            Else

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) V2) - |" & DataDefineDAO.V2_DATA(1) & " | " & DataDefineDAO.V2_DATA(0) & "에 추가 되어습니다. | " & inputLineCount & vbCrLf)
                FileClose()
            End If

            Exit Function

SKIP:
            If frmDataInserter.optNight.Checked = True Then
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) Koscom V2의 고정길이가 맞지 않습니다. | " & inputLineCount & vbCrLf)
                FileClose()
            Else
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) Koscom V2의 고정길이가 맞지 않습니다. | " & inputLineCount & vbCrLf)
                FileClose()
            End If

            Exit Function

        Catch ex As Exception

            If frmDataInserter.optNight.Checked = True Then
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) KOSCOM V2 작업 중 오류 발생 : " & ex.ToString() & vbCrLf)
                FileClose()
            Else
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) KOSCOM V2 작업 중 오류 발생 : " & ex.ToString() & vbCrLf)
                FileClose()
            End If
            connectionDB.rollbackTrans()
            MsgBox("KOSCOM V2 작업 중 오류 발생 : " & ex.ToString())
            Throw ex

        Finally
            connectionDB.endTrans()
        End Try
    End Function
#End Region

#Region "V8 작업(채권종류코드_KOSCOM)"
    Public Function GetDataV8(ByVal oneLineRow As String, inputLineCount As Integer, logFileName As String, fileNumber As Integer)

        DataDefineDAO.CheckLenLine = Utilities.ByteLen(oneLineRow)
        If frmDataInserter.optNight.Checked = True Then
            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | (PM) GetDataV8 작업 시작! 라인 : " & inputLineCount & vbCrLf)
            FileClose()
        Else
            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | (AM) GetDataV8 작업 시작! 라인 : " & inputLineCount & vbCrLf)
            FileClose()
        End If

        Try

            ' 라인 한줄의 길이가 2000byte가  아니라면 진행
            If DataDefineDAO.CheckLenLine <> 191 Then
                GoTo SKIP
            End If

            Dim queryVar As String = "" '쿼리 변수
            Dim colNameVar_V8 As String = "" 'COL_NAME BINDING 변수(쿼리의 명령문을 추가적으로 넣기 위한 변수)
            Dim dicParams = New Dictionary(Of String, String) ' 딕셔너리 선언
            dicParams.Add(":tr", "V8")

            Dim dbTableSeqData As DataTable = connectionDB.getData2(DB_Query.STR_SEQ_Query, dicParams, True, False)
            Dim dbTableLenData As DataTable = connectionDB.getData2(DB_Query.STR_LEN_Query, dicParams, True, False)
            Dim dbTableColNameData As DataTable = connectionDB.getData2(DB_Query.COL_NAME_Query, dicParams, True, False) ' 한 문장에서 필요한 값을 가져오기 위한 끝 인덱스 배열 DB 에서 호출
            Dim dbTableTableNameData As DataTable = connectionDB.getData2(DB_Query.TABLE_NAME_Query, dicParams, True, False) ' TRCODE 별 테이블 명 값 가져오기

            Dim dbTableDataStartArray(dbTableSeqData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_SEQ)
            Dim dbTableDataEndArray(dbTableLenData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_LEN)
            Dim dbTableDataColNameArray(dbTableColNameData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(COL_NAME)

            For index = 0 To dbTableDataStartArray.Length - 2
                dbTableDataStartArray(index) = dbTableSeqData.Rows(index).Item("STR_SEQ")
                dbTableDataEndArray(index) = dbTableLenData.Rows(index).Item("STR_LEN")
                dbTableDataColNameArray(index) = dbTableColNameData.Rows(index).Item("COL_NAME")
            Next

            DataDefineDAO.V8_DATA(0) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(0), dbTableDataEndArray(0))
            DataDefineDAO.V8_DATA(1) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(1), dbTableDataEndArray(1))

            If frmDataInserter.opt_Day.Checked = True Then
                DataDefineDAO.V8_DATA(2) = "'AM'"
            Else
                DataDefineDAO.V8_DATA(2) = "'PM'"
            End If

            For i = 0 To 2
                queryVar = queryVar & DataDefineDAO.V8_DATA(i) & ","
            Next

            'DB에서 가져온 COL_NAME값 변수에 대입
            For index = 0 To dbTableDataColNameArray.Length - 1
                If index = dbTableDataColNameArray.Length - 1 Then
                    colNameVar_V8 = colNameVar_V8 & dbTableDataColNameArray(index)
                Else
                    colNameVar_V8 = colNameVar_V8 & dbTableDataColNameArray(index) & ","
                End If
            Next

            connectionDB.beginTrans()

            dicParams.Add(":v8_data", queryVar)
            dicParams.Add(":colNameVar_V8", colNameVar_V8)
            dicParams.Add(":tr_tablename", dbTableTableNameData.Rows(0).Item("TABLE_NAME"))

            connectionDB.saveData2(DB_Query.InsertV8, Nothing, dicParams, False)
            connectionDB.commitTrans()

            If frmDataInserter.optNight.Checked = True Then
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) V8) - " & DataDefineDAO.V8_DATA(0) & " | " & DataDefineDAO.V8_DATA(1) & "에 추가 되어습니다.  라인 : " & inputLineCount & vbCrLf)
                FileClose()
            Else
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) V8) - " & DataDefineDAO.V8_DATA(0) & " | " & DataDefineDAO.V8_DATA(1) & "에 추가 되어습니다.  라인 : " & inputLineCount & vbCrLf)
                FileClose()
            End If

            Exit Function

SKIP:
            If frmDataInserter.optNight.Checked = True Then
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) Koscom V8의 고정길이가 맞지 않습니다. 라인 : " & inputLineCount & vbCrLf)
                FileClose()
            Else
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) Koscom V8의 고정길이가 맞지 않습니다. 라인 : " & inputLineCount & vbCrLf)
                FileClose()
            End If

            Exit Function

        Catch ex As Exception
            If frmDataInserter.optNight.Checked = True Then
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) V8 (채권종류코드_KOSCOM) 작업 중 오류 발생 : " & ex.ToString() & " 라인 :  " & inputLineCount & vbCrLf)
                FileClose()
            Else
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) V8 (채권종류코드_KOSCOM) 작업 중 오류 발생 : " & ex.ToString() & " 라인 :  " & inputLineCount & vbCrLf)
                FileClose()
            End If
            connectionDB.rollbackTrans()
            MsgBox("V8 (채권종류코드_KOSCOM) 작업 중 오류 발생 : " & ex.ToString())
            Throw ex

        Finally
            connectionDB.endTrans()
        End Try
    End Function
#End Region

#Region "V9 작업(소액매매종류)"
    Public Function GetDataV9(ByVal oneLineRow As String, inputLineCount As Integer, logFileName As String, fileNumber As Integer)

        DataDefineDAO.CheckLenLine = Utilities.ByteLen(oneLineRow)

        If frmDataInserter.optNight.Checked = True Then
            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | (PM) GetDataV9 작업 시작! | 라인 :  " & inputLineCount & vbCrLf)
            FileClose()
        Else
            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | (AM) GetDataV9 작업 시작! | 라인 :  " & inputLineCount & vbCrLf)
            FileClose()
        End If

        Try
            ' 라인 한줄의 길이가 2000byte가  아니라면 진행
            If DataDefineDAO.CheckLenLine <> 191 Then
                GoTo SKIP
            End If

            Dim queryVar As String = "" '쿼리 변수
            Dim colNameVar_V9 As String = "" '쿼리 변수
            Dim dicParams = New Dictionary(Of String, String) ' 딕셔너리 선언
            dicParams.Add(":tr", "V9")

            Dim dbTableSeqData As DataTable = connectionDB.getData2(DB_Query.STR_SEQ_Query, dicParams, True, False)
            Dim dbTableLenData As DataTable = connectionDB.getData2(DB_Query.STR_LEN_Query, dicParams, True, False)
            Dim dbTableColNameData As DataTable = connectionDB.getData2(DB_Query.COL_NAME_Query, dicParams, True, False) ' 한 문장에서 필요한 값을 가져오기 위한 끝 인덱스 배열 DB 에서 호출
            Dim dbTableTableNameData As DataTable = connectionDB.getData2(DB_Query.TABLE_NAME_Query, dicParams, True, False) ' TRCODE 별 테이블 명 값 가져오기

            Dim dbTableDataStartArray(dbTableSeqData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_SEQ)
            Dim dbTableDataEndArray(dbTableLenData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_LEN)
            Dim dbTableDataColNameArray(dbTableColNameData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(COL_NAME)

            For index = 0 To dbTableDataStartArray.Length - 2
                dbTableDataStartArray(index) = dbTableSeqData.Rows(index).Item("STR_SEQ")
                dbTableDataEndArray(index) = dbTableLenData.Rows(index).Item("STR_LEN")
                dbTableDataColNameArray(index) = dbTableColNameData.Rows(index).Item("COL_NAME")
            Next

            DataDefineDAO.V9_DATA(0) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(0), dbTableDataEndArray(0))
            DataDefineDAO.V9_DATA(1) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(1), dbTableDataEndArray(1))
            DataDefineDAO.V9_DATA(2) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(2), dbTableDataEndArray(2))

            If frmDataInserter.optNight.Checked = True Then
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) GetDataV9 변수 초기화 작업 완료! | 라인 :  " & inputLineCount & vbCrLf)
                FileClose()
            Else
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) GetDataV9 변수 초기화 작업 완료! | 라인 :  " & inputLineCount & vbCrLf)
                FileClose()
            End If

            If frmDataInserter.opt_Day.Checked = True Then
                DataDefineDAO.V9_DATA(3) = "'AM'"
            Else
                DataDefineDAO.V9_DATA(3) = "'PM'"
            End If

            For i = 0 To 3
                queryVar = queryVar & DataDefineDAO.V9_DATA(i) & ","
            Next

            'DB에서 가져온 COL_NAME값 변수에 대입
            For index = 0 To dbTableDataColNameArray.Length - 1
                If index = dbTableDataColNameArray.Length - 1 Then
                    colNameVar_V9 = colNameVar_V9 & dbTableDataColNameArray(index)
                Else
                    colNameVar_V9 = colNameVar_V9 & dbTableDataColNameArray(index) & ","
                End If
            Next

            connectionDB.beginTrans()

            dicParams.Add(":v9_data", queryVar)
            dicParams.Add(":colNameVar_V9", colNameVar_V9)
            dicParams.Add(":tr_tablename", dbTableTableNameData.Rows(0).Item("TABLE_NAME"))

            connectionDB.saveData2(DB_Query.InsertV9, Nothing, dicParams, False)

            connectionDB.commitTrans()

            If frmDataInserter.optNight.Checked = True Then
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) V9) - " & DataDefineDAO.V9_DATA(0) & "," & DataDefineDAO.V9_DATA(1) & "에 추가 되어습니다. | 라인 :  " & inputLineCount & vbCrLf)
                FileClose()
            Else
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) V9) - " & DataDefineDAO.V9_DATA(0) & "," & DataDefineDAO.V9_DATA(1) & "에 추가 되어습니다. | 라인 :  " & inputLineCount & vbCrLf)
                FileClose()
            End If

            Exit Function
SKIP:
            If frmDataInserter.optNight.Checked = True Then
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) Koscom V9의 고정길이가 맞지 않습니다. | 라인 :  " & inputLineCount & vbCrLf)
                FileClose()
            Else
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) Koscom V9의 고정길이가 맞지 않습니다. | 라인 :  " & inputLineCount & vbCrLf)
                FileClose()
            End If

            Exit Function

        Catch ex As Exception
            If frmDataInserter.optNight.Checked = True Then
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) (PM) V9 (소액매매종류) 작업 중 오류 발생 : " & ex.ToString() & " | 라인 :  " & inputLineCount & vbCrLf)
                FileClose()

            Else
                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) V9 (소액매매종류) 작업 중 오류 발생 : " & ex.ToString() & " | 라인 :  " & inputLineCount & vbCrLf)
                FileClose()

            End If
            connectionDB.rollbackTrans()
            MsgBox("V9 (소액매매종류) 작업 중 오류 발생 : " & ex.ToString())
            Throw ex
        Finally
            connectionDB.endTrans()
        End Try
    End Function
#End Region

#Region "HISTORY 테이블에 데이터 INSERT(프로시저 작업 진행) _ PBN_LOAD_BOND_INFO"

    Public Function CmdHistory_Click_PBN_LOAD_BOND_INFO(logFileName As String, fileNumber As Integer)

        Try
            Dim dicParams = New Dictionary(Of String, String)
            Dim prev_t_day As Date = Date.Today.AddDays(-1)

            dicParams.Add(":i_prev_work_day", "'" & CStr(prev_t_day) & "'")
            dicParams.Add(":t_day", "'" & Date.Today & "'")

            connectionDB.beginTrans()

            'BATCH(V2:채권발행정보)로 들어온 발행정보를 발행정보 HISTORY 테이블(PABNTD00)에 입력
            connectionDB.saveData2(DB_Query.PBN_LOAD_BOND_INFO_Histroy, Nothing, dicParams, False)
            connectionDB.commitTrans()

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "(AM) CmdHistory_Click_PBN_LOAD_BOND_INFO : BATCH(V2:채권발행정보)로 들어온 발행정보를 발행정보 HISTORY 테이블(PABNTD00)에 입력 완료!" & vbCrLf)
            FileClose()

            '당일 발행물 발행정보 최종 테이블(PABNTD01)에 입력 : (PABNTD00 SEQ = 0 당일 발행물)
            connectionDB.saveData2(DB_Query.PBN_LOAD_BOND_INFO_Last, Nothing, dicParams, False)

        Catch ex As Exception
            connectionDB.rollbackTrans()
            MsgBox("HISTORY 테이블에 데이터 INSERT(프로시저 작업 (오전만 수행)) 중 오류 발생  : " & ex.ToString())
            Throw ex

        Finally
            connectionDB.endTrans()
        End Try

    End Function

#End Region

#Region "평가에 영향을 주지않는 발행정보 일괄복사프로시져 추가 "
    Public Function CmdHistory_Click_PBN_COPY_BOND_INFO(logFileName As String, fileNumber As Integer)

        Try
            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "(AM) CmdHistory_Click_PBN_COPY_BOND_INFO : 평가에 영향을 주지않는 발행정보 일괄복사프로시져 추가 시작!" & vbCrLf)
            FileClose()

            Dim index As Integer
            Dim dicParams = New Dictionary(Of String, String)

            Dim prev_t_day As Date = Date.Today.AddDays(-1)
            dicParams.Add(":t_day", "'" & Date.Today & "'")

            Dim dbTableData As DataTable = connectionDB.getData2(DB_Query.EXTRACT_NONIMPACT_DATA, dicParams, False)

            Dim returnCount As Integer = 0
            Dim returnValue As String = ""

            dicParams.Clear()

            'connectionDB.beginTrans()

            If dbTableData.Rows.Count > 0 Then '조회 데이터가 있을 경우 조건절 수행

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | " & "(AM) CmdHistory_Click_PBN_COPY_BOND_INFO : 조회 자료 있음" & vbCrLf)
                FileClose()

                For index = 0 To dbTableData.Rows.Count - 1  ' 데이터 있는 만큼 반복문 수행 (1. 조회 된 데이터 값 변수에 대입)
                    DataDefineDAO.i_bond_id = dbTableData.Rows(index).Item("bond_id")
                    DataDefineDAO.i_bond_type_code = dbTableData.Rows(index).Item("bond_type_code")

                    If IsDBNull(dbTableData.Rows(index).Item("local_type_code")) Then
                        DataDefineDAO.i_local_type_code = "Null"
                    Else
                        DataDefineDAO.i_local_type_code = dbTableData.Rows(index).Item("local_type_code")
                    End If

                    If IsDBNull(dbTableData.Rows(index).Item("abs_type_code")) Then
                        DataDefineDAO.i_abs_type_code = "Null"
                    Else
                        DataDefineDAO.i_abs_type_code = dbTableData.Rows(index).Item("abs_type_code")
                    End If

                    If IsDBNull(dbTableData.Rows(index).Item("first_coupon_day")) Then
                        DataDefineDAO.i_first_coupon_day = "Null"
                    Else
                        DataDefineDAO.i_first_coupon_day = Format(dbTableData.Rows(index).Item("first_coupon_day"), "yyyy-MM-dd")
                    End If


                    DataDefineDAO.i_batch_amt_confirm_gb = dbTableData.Rows(index).Item("batch_amt_confirm_gb")

                    If IsDBNull(dbTableData.Rows(index).Item("named_gb")) Then
                        DataDefineDAO.i_named_gb = "Null"
                    Else
                        DataDefineDAO.i_named_gb = dbTableData.Rows(index).Item("named_gb")
                    End If


                    DataDefineDAO.i_admin_org_id = dbTableData.Rows(index).Item("admin_org_id")

                    If IsDBNull(dbTableData.Rows(index).Item("guarant_org_id")) Then
                        DataDefineDAO.i_guarant_org_id = "Null"
                    Else
                        DataDefineDAO.i_guarant_org_id = dbTableData.Rows(index).Item("guarant_org_id")
                    End If

                    DataDefineDAO.i_custody_org_id = dbTableData.Rows(index).Item("custody_org_id")
                    DataDefineDAO.i_registration_org_id = dbTableData.Rows(index).Item("registration_org_id")
                    DataDefineDAO.i_prcp_agency_org_id = dbTableData.Rows(index).Item("prcp_agency_org_id")
                    DataDefineDAO.i_int_calc_type_code = dbTableData.Rows(index).Item("int_calc_type_code")

                    If IsDBNull(dbTableData.Rows(index).Item("int_accr_pay_hldy_type_code")) Then
                        DataDefineDAO.i_int_accr_pay_hldy_type_code = "Null"
                    Else
                        DataDefineDAO.i_int_accr_pay_hldy_type_code = dbTableData.Rows(index).Item("int_accr_pay_hldy_type_code")
                    End If

                    DataDefineDAO.i_int_accr_hldy_type_code = dbTableData.Rows(index).Item("int_accr_hldy_type_code")
                    DataDefineDAO.i_int_accr_rate = CStr(dbTableData.Rows(index).Item("int_accr_rate"))

                    If IsDBNull(dbTableData.Rows(index).Item("prcp_accr_pay_hldy_type_code")) Then
                        DataDefineDAO.i_prcp_accr_pay_hldy_type_code = "Null"

                    Else
                        DataDefineDAO.i_prcp_accr_pay_hldy_type_code = dbTableData.Rows(index).Item("prcp_accr_pay_hldy_type_code")
                    End If

                    DataDefineDAO.i_prcp_accr_hldy_type_code = dbTableData.Rows(index).Item("prcp_accr_hldy_type_code")
                    DataDefineDAO.i_prcp_accr_rate = CStr(dbTableData.Rows(index).Item("prcp_accr_rate"))
                    DataDefineDAO.i_issuer_org_id = dbTableData.Rows(index).Item("ISSUER_ORG_ID")


                    '2. 값을 담은 변수 값을 딕셔너리 변수에 대입
                    dicParams.Add(":i_bond_id", "'" & DataDefineDAO.i_bond_id & "'")
                    dicParams.Add(":i_bond_type_code", "'" & DataDefineDAO.i_bond_type_code & "'")

                    If DataDefineDAO.i_local_type_code = "Null" Then
                        dicParams.Add(":i_local_type_code", DataDefineDAO.i_local_type_code)
                    Else
                        dicParams.Add(":i_local_type_code", "'" & DataDefineDAO.i_local_type_code & "'")
                    End If

                    If DataDefineDAO.i_abs_type_code = "Null" Then
                        dicParams.Add(":i_abs_type_code", DataDefineDAO.i_abs_type_code)
                    Else
                        dicParams.Add(":i_abs_type_code", "'" & DataDefineDAO.i_abs_type_code & "'")
                    End If

                    If DataDefineDAO.i_first_coupon_day = "Null" Then
                        dicParams.Add(":i_first_coupon_day", DataDefineDAO.i_first_coupon_day)
                    Else
                        dicParams.Add(":i_first_coupon_day", "'" & DataDefineDAO.i_first_coupon_day & "'")
                    End If

                    dicParams.Add(":i_batch_amt_confirm_gb", "'" & DataDefineDAO.i_batch_amt_confirm_gb & "'")

                    If DataDefineDAO.i_named_gb = "Null" Then
                        dicParams.Add(":i_named_gb", DataDefineDAO.i_named_gb)
                    Else
                        dicParams.Add(":i_named_gb", "'" & DataDefineDAO.i_named_gb & "'")
                    End If

                    dicParams.Add(":i_admin_org_id", "'" & DataDefineDAO.i_admin_org_id & "'")

                    If DataDefineDAO.i_guarant_org_id = "Null" Then
                        dicParams.Add(":i_guarant_org_id", DataDefineDAO.i_guarant_org_id)
                    Else
                        dicParams.Add(":i_guarant_org_id", "'" & DataDefineDAO.i_guarant_org_id & "'")
                    End If

                    dicParams.Add(":i_custody_org_id", "'" & DataDefineDAO.i_custody_org_id & "'")
                    dicParams.Add(":i_registration_org_id", "'" & DataDefineDAO.i_registration_org_id & "'")
                    dicParams.Add(":i_prcp_agency_org_id", "'" & DataDefineDAO.i_prcp_agency_org_id & "'")
                    dicParams.Add(":i_int_calc_type_code", "'" & DataDefineDAO.i_int_calc_type_code & "'")

                    If DataDefineDAO.i_int_accr_pay_hldy_type_code = "Null" Then
                        dicParams.Add(":i_int_accr_pay_hldy_type_code", DataDefineDAO.i_int_accr_pay_hldy_type_code)

                    Else
                        dicParams.Add(":i_int_accr_pay_hldy_type_code", "'" & DataDefineDAO.i_int_accr_pay_hldy_type_code & "'")

                    End If
                    dicParams.Add(":i_int_accr_hldy_type_code", "'" & DataDefineDAO.i_int_accr_hldy_type_code & "'")
                    dicParams.Add(":i_int_accr_rate", DataDefineDAO.i_int_accr_rate)

                    If DataDefineDAO.i_prcp_accr_pay_hldy_type_code = "Null" Then
                        dicParams.Add(":i_prcp_accr_pay_hldy_type_code", DataDefineDAO.i_prcp_accr_pay_hldy_type_code)
                    Else
                        dicParams.Add(":i_prcp_accr_pay_hldy_type_code", "'" & DataDefineDAO.i_prcp_accr_pay_hldy_type_code & "'")
                    End If

                    dicParams.Add(":i_prcp_accr_hldy_type_code", "'" & DataDefineDAO.i_prcp_accr_hldy_type_code & "'")
                    dicParams.Add(":i_prcp_accr_rate", DataDefineDAO.i_prcp_accr_rate)
                    dicParams.Add(":i_issuer_org_id", "'" & DataDefineDAO.i_issuer_org_id & "'")

                    returnCount += 1

                    FileOpen(fileNumber, logFileName, OpenMode.Append)
                    Print(fileNumber, Date.Now & " | " & "(AM) CmdHistory_Click_PBN_COPY_BOND_INFO : " & index & "번째 | " & DataDefineDAO.i_bond_id & vbCrLf)
                    FileClose()

                    FileOpen(fileNumber, logFileName, OpenMode.Append)
                    Print(fileNumber, Date.Now & " | " & "(AM) CmdHistory_Click_PBN_COPY_BOND_INFO : PABNTD01(최종 테이블) 에 UPDATE 시작!" & vbCrLf)
                    FileClose()

                    ' PABNTD01(최종 테이블) 에 UPDATE
                    connectionDB.saveData2(DB_Query.COPY_BOND_INFO_UPDATE, Nothing, dicParams, False)

                    FileOpen(fileNumber, logFileName, OpenMode.Append)
                    Print(fileNumber, Date.Now & " | " & "(AM) CmdHistory_Click_PBN_COPY_BOND_INFO : PABNTD01(최종 테이블) 에 UPDATE 완료!" & vbCrLf)
                    FileClose()

                    dicParams.Clear()

                    dicParams.Add(":i_bond_id", "'" & DataDefineDAO.i_bond_id & "'")

                    FileOpen(fileNumber, logFileName, OpenMode.Append)
                    Print(fileNumber, Date.Now & " | " & "(AM) CmdHistory_Click_PBN_COPY_BOND_INFO : PABNTD00(히스토리 테이블) 에 INSERT 시작!" & vbCrLf)
                    FileClose()

                    ' PABNTD00(히스토리 테이블) 에 INSERT
                    connectionDB.saveData2(DB_Query.COPY_BOND_INFO_INSERT, Nothing, dicParams, False)

                    FileOpen(fileNumber, logFileName, OpenMode.Append)
                    Print(fileNumber, Date.Now & " | " & "(AM) CmdHistory_Click_PBN_COPY_BOND_INFO : PABNTD00(히스토리 테이블) 에 INSERT 완료!" & vbCrLf)
                    FileClose()

                    '                    connectionDB.commitTrans()

                    dicParams.Clear()

                Next
            End If

            Exit Function

        Catch ex As Exception
            'connectionDB.rollbackTrans()

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "(AM) CmdHistory_Click_PBN_COPY_BOND_INFO 에서 오류 발생! 오류 내용 :" & ex.ToString() & vbCrLf)
            FileClose()

            MsgBox("평가에 영향을 주지않는 발행정보 일괄복사프로시져 추가 작업 중 오류 발생 : " & ex.ToString())
            Throw ex

        Finally
            'connectionDB.endTrans()
        End Try

    End Function
#End Region
End Class
