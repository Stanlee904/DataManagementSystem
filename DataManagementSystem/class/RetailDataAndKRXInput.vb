Public Class RetailDataAndKRXInput

    Public connectionDB As New DB_Agent()
    Public DataDefineDAO As New DataDefineDAO()
    Public Utilities As New Utilities()
    Public FTP_Agent As New FTP_Agent()

    Dim App_Path = System.AppDomain.CurrentDomain.BaseDirectory() ' 프로그램 현재 경로
    Dim txtFileReader As IO.StreamReader 'IO.StreamReader를 사용 
    Dim lineCount As Integer = 0 '라인 수 확인 용 
    Dim lineCountA001B As Integer = 0 ' A0027 라인 수 확인용
    Dim lineCountGridA001B As Integer = 0 ' A0027 라인 수 확인용
    Dim lineCountG300B As Integer = 0 ' G3027 라인 수 확인용
    Dim lineCountGridG300B As Integer = 0 ' G3027 라인 수 확인용
    Dim todayDoCountA001B As Integer 'A0027 금일 처리 할 개수 
    Dim todayDoCountG300B As Integer 'G3027 금일 처리 할 개수 
    '---------------------------------------------------------------
    Dim lineCountA0027 As Integer = 0 ' A0027 라인 수 확인용
    Dim lineCountGridA0027 As Integer = 0 ' A0027 라인 수 확인용
    Dim lineCountG3027 As Integer = 0 ' G3027 라인 수 확인용
    Dim lineCountGridG3027 As Integer = 0 ' G3027 라인 수 확인용
    Dim todayDoCountA0027 As Integer 'A0027 금일 처리 할 개수 
    Dim todayDoCountG3027 As Integer 'G3027 금일 처리 할 개수 
    '---------------------------------------------------------------
    Dim colNameVar_A001B As String = "" 'COL_NAME BINDING 변수(쿼리의 명령문을 추가적으로 넣기 위한 변수)
    Dim colNameVar_G300B As String = "" 'COL_NAME BINDING 변수(쿼리의 명령문을 추가적으로 넣기 위한 변수)
    '---------------------------------------------------------------
    Dim colNameVar_A0027 As String = "" 'COL_NAME BINDING 변수(쿼리의 명령문을 추가적으로 넣기 위한 변수)
    Dim colNameVar_g3027 As String = "" 'COL_NAME BINDING 변수(쿼리의 명령문을 추가적으로 넣기 위한 변수)

    Dim trcode As String = "" ' grid TRCODE 확인 변수
    Dim isCheckedA001B As Boolean 'A0027 체크박스 체크확인 BOOL 변수
    Dim isCheckedG300B As Boolean 'G3027 체크박스 체크확인 BOOL 변수
    '---------------------------------------------------------------
    Dim isCheckedA0027 As Boolean 'A0027 체크박스 체크확인 BOOL 변수
    Dim isCheckedG3027 As Boolean 'G3027 체크박스 체크확인 BOOL 변수

    Dim retailDataServerAddress26 As String = "\\222.111.237.26\udpkoscom\data\"
    Dim retailDataServerAddress25 As String = "\\222.111.237.25\udpkoscom\data\"
    Dim dtblDATA As New DataTable
#Region "채권소매 관련데이터 변경_차세대 KRX 버전"

    Public Function RetailDataExecute2(logFileName As String, fileNumber As Integer)

        DataDefineDAO.tempLine = ""
        DataDefineDAO.loc_A001B_temp_trgFile = App_Path & "Data\A001B_" & Format(frmDataInserter.t_day.Value, "yyyyMMdd") & ".dat"
        DataDefineDAO.loc_G300B_temp_trgFile = App_Path & "Data\G300B_" & Format(frmDataInserter.t_day.Value, "yyyyMMdd") & ".dat"
        Dim fileCheckTodayA001B As New System.IO.FileInfo(DataDefineDAO.loc_A001B_temp_trgFile)
        Dim fileCheckTodayG300B As New System.IO.FileInfo(DataDefineDAO.loc_G300B_temp_trgFile)

        FileOpen(fileNumber, logFileName, OpenMode.Append)
        Print(fileNumber, Date.Now & " | " & "(PM) RetailDataExecute : 작업 시작!" & vbCrLf)
        FileClose()

        If fileCheckTodayA001B.Exists = True And fileCheckTodayG300B.Exists = True Then
            Try
                Dim index As Integer 'For 문 인덱스
                Dim dicParams = New Dictionary(Of String, String) ' 딕셔너리 선언
                dicParams.Add(":tr", "A001B")

                Dim dbTableSeqA001BData As DataTable = connectionDB.getData2(DB_Query.STR_SEQ_Query, dicParams, True, False)
                Dim dbTableLenA001BData As DataTable = connectionDB.getData2(DB_Query.STR_LEN_Query, dicParams, True, False)
                Dim dbTableColNameA001BData As DataTable = connectionDB.getData2(DB_Query.COL_NAME_Query, dicParams, True, False) ' 한 문장에서 필요한 값을 가져오기 위한 끝 인덱스 배열 DB 에서 호출


                Dim dbTableDataStartA001BArray(dbTableSeqA001BData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_SEQ)
                Dim dbTableDataEndA001BArray(dbTableLenA001BData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_LEN)
                Dim dbTableDataColNameA001BArray(dbTableColNameA001BData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(COL_NAME)

                For index = 0 To dbTableDataStartA001BArray.Length - 2
                    dbTableDataStartA001BArray(index) = dbTableSeqA001BData.Rows(index).Item("STR_SEQ")
                    dbTableDataEndA001BArray(index) = dbTableLenA001BData.Rows(index).Item("STR_LEN")
                    dbTableDataColNameA001BArray(index) = dbTableColNameA001BData.Rows(index).Item("COL_NAME")
                Next

                dicParams.Clear()
                dicParams.Add(":tr", "G300B")

                Dim dbTableSeqG300BData As DataTable = connectionDB.getData2(DB_Query.STR_SEQ_Query, dicParams, True, False)
                Dim dbTableLenG300BData As DataTable = connectionDB.getData2(DB_Query.STR_LEN_Query, dicParams, True, False)
                Dim dbTableColNameG300BData As DataTable = connectionDB.getData2(DB_Query.COL_NAME_Query, dicParams, True, False) ' 한 문장에서 필요한 값을 가져오기 위한 끝 인덱스 배열 DB 에서 호출


                Dim dbTableDataStartG300BArray(dbTableSeqG300BData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_SEQ)
                Dim dbTableDataEndG300BArray(dbTableLenG300BData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_LEN)
                Dim dbTableDataColNameG300BArray(dbTableColNameG300BData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(COL_NAME)

                For index = 0 To dbTableDataStartG300BArray.Length - 2
                    dbTableDataStartG300BArray(index) = dbTableSeqG300BData.Rows(index).Item("STR_SEQ")
                    dbTableDataEndG300BArray(index) = dbTableLenG300BData.Rows(index).Item("STR_LEN")
                    dbTableDataColNameG300BArray(index) = dbTableColNameG300BData.Rows(index).Item("COL_NAME")
                Next

                dtblDATA = frmDataInserter.grd_RECEIVE_LIST.DataSource '그리드에 있는 데이터 값을 가져오기 

                '그리드에 TRCODE Check 확인
                For index = 0 To dtblDATA.Rows.Count - 1
                    trcode = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TR")
                    If trcode = "A001B" Then
                        isCheckedA001B = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "CHK")
                        Exit For
                    End If
                Next

                'txtFileReader.Close()
                txtFileReader = My.Computer.FileSystem.OpenTextFileReader(DataDefineDAO.loc_A001B_temp_trgFile, System.Text.Encoding.Default) '파일 읽을 API

                'DB에서 가져온 COL_NAME값 변수에 대입
                For index = 0 To dbTableDataColNameA001BArray.Length - 1
                    If index = dbTableDataColNameA001BArray.Length - 1 Then
                        colNameVar_A001B = colNameVar_A001B & dbTableDataColNameA001BArray(index)
                    Else
                        colNameVar_A001B = colNameVar_A001B & dbTableDataColNameA001BArray(index) & ","
                    End If
                Next

                'A0027이 화면상에 체크가 되어 있다면 작업 진행
                If isCheckedA001B = True Then
                    'A0027 채권소매시장 종목배치
                    While Not txtFileReader.EndOfStream '마지막줄까지 남김없이 읽어오기

                        DataDefineDAO.tempLine = txtFileReader.ReadLine()

                        DataDefineDAO.temp_length = Utilities.ByteLen(DataDefineDAO.tempLine)

                        'DataDefineDAO.trCode = Mid(DataDefineDAO.tempLine, 1, 5)

                        lineCount += 1

                        'KOSCOM UDP는 마지막 라인 종목코드 필드에 (999999999999) 들어오면 입력하지 않는다.
                        If DataDefineDAO.temp_length = 327 And Utilities.ByteMidStrTrim(DataDefineDAO.tempLine, dbTableDataStartA001BArray(3), dbTableDataEndA001BArray(3)) <> "999999999999" Then

                            DataDefineDAO.A001B_SEQ = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(0)), CInt(dbTableDataEndA001BArray(0))) '정보분배일련번호 0
                            DataDefineDAO.A001B_BOND_CNT = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(1)), CInt(dbTableDataEndA001BArray(1)), 3) '정보분배총종목인덱스 (종목인덱스 중 마지막 종목의 값) 1

                            DataDefineDAO.A001B_DAY = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(2)), CInt(dbTableDataEndA001BArray(2)), 3) '적용일 2
                            If DataDefineDAO.A001B_DAY = "00000000" Then
                                DataDefineDAO.A001B_DAY = "Null"
                            Else
                                DataDefineDAO.A001B_DAY = "'" & DateTime.ParseExact(DataDefineDAO.A001B_DAY, "yyyyMMdd", Nothing) & "'"
                            End If

                            DataDefineDAO.A001B_BOND_ID = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(3)), CInt(dbTableDataEndA001BArray(3))) '종목코드 3
                            DataDefineDAO.A001B_BOND_SEQ = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(4)), CInt(dbTableDataEndA001BArray(4)), 3) '정보분배종목인덱스 (당일 종목 식별용으로 부여되는 일련번호) 4
                            DataDefineDAO.A001B_RETAIL_BOND_TYPE_CODE = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(5)), CInt(dbTableDataEndA001BArray(5))) '소매채권분류코드 5
                            DataDefineDAO.A001B_KOR_BOND_NM = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(6)), CInt(dbTableDataEndA001BArray(6))) '종목약명 6
                            DataDefineDAO.A001B_ENG_BOND_NM = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(7)), CInt(dbTableDataEndA001BArray(7))) '종목영문약명 7
                            DataDefineDAO.A001B_TSC_ID = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(8)), CInt(dbTableDataEndA001BArray(8))) '장운영상품그룹ID (동일한 장운영(TSC, Trading Schedule Control) 대상이 되는 상품들의 집합을 식별하기 위한 ID) 8
                            DataDefineDAO.A001B_LIST_TYPE_CODE = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(9)), CInt(dbTableDataEndA001BArray(9))) '채권상장구분코드 9
                            DataDefineDAO.A001B_BOND_ANCD_CODE = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(10)), CInt(dbTableDataEndA001BArray(10))) '채권분류코드 10
                            DataDefineDAO.A001B_GUARANTEE_TYPE_CODE = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(11)), CInt(dbTableDataEndA001BArray(11))) '채권보증구분코드 11
                            DataDefineDAO.A001B_INT_PAY_TYPE_CODE = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(12)), CInt(dbTableDataEndA001BArray(12))) '이자지급방법코드 12

                            DataDefineDAO.A001B_LIST_DAY = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(13)), CInt(dbTableDataEndA001BArray(13)), 3) '상장일자 13
                            If DataDefineDAO.A001B_LIST_DAY = "00000000" Or DataDefineDAO.A001B_LIST_DAY = "Null" Then
                                DataDefineDAO.A001B_LIST_DAY = "Null"
                            Else
                                DataDefineDAO.A001B_LIST_DAY = "'" & DateTime.ParseExact(DataDefineDAO.A001B_LIST_DAY, "yyyyMMdd", Nothing) & "'"

                            End If

                            DataDefineDAO.A001B_ISSUE_DAY = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(14)), CInt(dbTableDataEndA001BArray(14)), 3) '발행일자 14
                            If DataDefineDAO.A001B_ISSUE_DAY = "00000000" Or DataDefineDAO.A001B_ISSUE_DAY = "Null" Then
                                DataDefineDAO.A001B_ISSUE_DAY = "Null"
                            Else
                                DataDefineDAO.A001B_ISSUE_DAY = "'" & DateTime.ParseExact(DataDefineDAO.A001B_ISSUE_DAY, "yyyyMMdd", Nothing) & "'"
                            End If

                            DataDefineDAO.A001B_MAT_DAY = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(15)), CInt(dbTableDataEndA001BArray(15)), 3) '상환일자 15
                            If DataDefineDAO.A001B_MAT_DAY = "00000000" Or DataDefineDAO.A001B_MAT_DAY = "Null" Then
                                DataDefineDAO.A001B_MAT_DAY = "Null"
                            Else
                                DataDefineDAO.A001B_MAT_DAY = "'" & DateTime.ParseExact(DataDefineDAO.A001B_MAT_DAY, "yyyyMMdd", Nothing) & "'"
                            End If

                            DataDefineDAO.A001B_SELL_DAY = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(16)), CInt(dbTableDataEndA001BArray(16)), 3) '매출일자 16
                            If DataDefineDAO.A001B_SELL_DAY = "00000000" Or DataDefineDAO.A001B_SELL_DAY = "Null" Then
                                DataDefineDAO.A001B_SELL_DAY = "Null"
                            Else
                                DataDefineDAO.A001B_SELL_DAY = "'" & DateTime.ParseExact(DataDefineDAO.A001B_SELL_DAY, "yyyyMMdd", Nothing) & "'"
                            End If

                            DataDefineDAO.A001B_ISSUE_AMT = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(17)), CInt(dbTableDataEndA001BArray(17)), 3)  '채권발행율(99999V999999), (수신 : 99999V999999 %, DB : %) 17
                            DataDefineDAO.A001B_COUPON_RATE = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(18)), CInt(dbTableDataEndA001BArray(18)), 3)  '표면이자율(9999999V99999), (수신 : 9999999V99999 &, DB : %) 18
                            DataDefineDAO.A001B_INT_PAY_MONTH_CALC = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(19)), CInt(dbTableDataEndA001BArray(19)), 3) '이자지급계산월수 19
                            DataDefineDAO.A001B_INT_PAY_TIME_TYPE_CODE = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(20)), CInt(dbTableDataEndA001BArray(20))) '이표지급방법코드 20
                            DataDefineDAO.A001B_INT_PAY_TERM_TYPE_CODE = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(21)), CInt(dbTableDataEndA001BArray(21))) '채권이자지급일기준구분코드 21
                            DataDefineDAO.A001B_INT_END_MONTH_TYPE = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(22)), CInt(dbTableDataEndA001BArray(22))) '이자월말구분코드 22
                            DataDefineDAO.A001B_INT_PAY_UNIT_TYPE = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(23)), CInt(dbTableDataEndA001BArray(23))) '이자원단위미만처리코드 23
                            DataDefineDAO.A001B_PRE_SELL_INT_TYPE_CODE = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(24)), CInt(dbTableDataEndA001BArray(24))) '채권선매출이자지급방법코드 24
                            DataDefineDAO.A001B_PRCP_AMT = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(25)), CInt(dbTableDataEndA001BArray(25))) '발행금액(999999999999999999) 25
                            DataDefineDAO.A001B_LIST_AMT = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(26)), CInt(dbTableDataEndA001BArray(26))) '상장금액(999999999999999999) 26
                            DataDefineDAO.A001B_CB_PRCP_RETURN_RATE = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(27)), CInt(dbTableDataEndA001BArray(27)), 3) '만기상환비율(99999V999999), (수신 : 99999V999999 %, DB : %) 27
                            DataDefineDAO.A001B_INSTALLMENT_RETURN_TYPE = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(28)), CInt(dbTableDataEndA001BArray(28))) '분할상환유형구분코드 28
                            DataDefineDAO.A001B_UNREDEEM_TERM = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(29)), CInt(dbTableDataEndA001BArray(29))) '거치개월수 29
                            DataDefineDAO.A001B_REFUND_TERM_TIMES = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(30)), CInt(dbTableDataEndA001BArray(30))) '분할상환횟수 30

                            'Code Converting
                            'AS - IS : 1:중단, 2:정지, 3:상환일거래정지, 0:정상
                            'TO - BE : Y:거래정지, N:정상

                            DataDefineDAO.A001B_DEAL_STOP_TYPE = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(31)), CInt(dbTableDataEndA001BArray(31))) '거래정지여부 31

                            Dim DEAL_STOP_TYPE As Integer

                            If Replace(DataDefineDAO.A001B_DEAL_STOP_TYPE, "'", "") = "Y" Then
                                DEAL_STOP_TYPE = 2
                            Else
                                DEAL_STOP_TYPE = 0
                            End If

                            DataDefineDAO.A001B_DEAL_STOP_TYPE = "" & "'" & DEAL_STOP_TYPE & "'"

                            DataDefineDAO.A001B_PREV_INT_PAY_DAY = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(32)), CInt(dbTableDataEndA001BArray(32)), 3) '전기이자지급일자 32
                            If DataDefineDAO.A001B_PREV_INT_PAY_DAY = "00000000" Or DataDefineDAO.A001B_PREV_INT_PAY_DAY = "Null" Then
                                DataDefineDAO.A001B_PREV_INT_PAY_DAY = "Null"
                            Else
                                DataDefineDAO.A001B_PREV_INT_PAY_DAY = "'" & DateTime.ParseExact(DataDefineDAO.A001B_PREV_INT_PAY_DAY, "yyyyMMdd", Nothing) & "'"
                            End If

                            DataDefineDAO.A001B_NEXT_INT_PAY_DAY = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(33)), CInt(dbTableDataEndA001BArray(33)), 3) '차기이자지급일자 33
                            If DataDefineDAO.A001B_NEXT_INT_PAY_DAY = "00000000" Or DataDefineDAO.A001B_NEXT_INT_PAY_DAY = "Null" Then
                                DataDefineDAO.A001B_NEXT_INT_PAY_DAY = "Null"
                            Else
                                DataDefineDAO.A001B_NEXT_INT_PAY_DAY = "'" & DateTime.ParseExact(DataDefineDAO.A001B_NEXT_INT_PAY_DAY, "yyyyMMdd", Nothing) & "'"
                            End If

                            DataDefineDAO.A001B_HYBRID_BOND_TYPE = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(34)), CInt(dbTableDataEndA001BArray(34))) '영구채권만기구조여부 34
                            DataDefineDAO.A001B_STRIPS_TAG = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(35)), CInt(dbTableDataEndA001BArray(35))) '채권스트립구분코드 35
                            DataDefineDAO.A001B_OVER_ASK_STANDARD_PRICE = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(36)), CInt(dbTableDataEndA001BArray(36)), 3) '기준가격 36
                            DataDefineDAO.A001B_STLMT_TRAD_OBJT_TAG = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(37)), CInt(dbTableDataEndA001BArray(37)), 3) '정리매매여부 37

                            'Code Converting
                            'AS - IS : 0: 정리매매비대상, 1: 정리매매대상
                            'TO - BE : N: 정리매매비대상, Y: 정리매매대상
                            Dim STLMT_TRAD_OBJT_TAG As Integer

                            If Replace(DataDefineDAO.A001B_STLMT_TRAD_OBJT_TAG, "'", "") = "N" Then
                                STLMT_TRAD_OBJT_TAG = 0
                            ElseIf Replace(DataDefineDAO.A001B_STLMT_TRAD_OBJT_TAG, "'", "") = "Y" Then
                                STLMT_TRAD_OBJT_TAG = 1
                            End If

                            DataDefineDAO.A001B_STLMT_TRAD_OBJT_TAG = "" & "'" & STLMT_TRAD_OBJT_TAG & "'"


                            '2016-11-25 주정석 투자유의채권구분코드(INVT_ATD_BND_TP_CD-0: 해당없음 1: 지정예고 2: 지정)
                            DataDefineDAO.A001B_INVT_ATD_BND_TP_CD = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartA001BArray(35)), CInt(dbTableDataEndA001BArray(35))) '투자유의채권구분코드 38

                            'DataDefineDAO.A001B_INPUT_DAY = "'" & frmDataInserter.t_day.Value & "'"

                            'connectionDB.beginTrans()

                            dicParams.Clear()
                            '딕셔너리 add 작업 
                            dicParams.Add(":a001b_seq", DataDefineDAO.A001B_SEQ)
                            dicParams.Add(":a001b_bond_cnt", DataDefineDAO.A001B_BOND_CNT)
                            dicParams.Add(":a001b_day", DataDefineDAO.A001B_DAY)
                            dicParams.Add(":a001b_bond_id", DataDefineDAO.A001B_BOND_ID)
                            dicParams.Add(":a001b_bond_seq", DataDefineDAO.A001B_BOND_SEQ)
                            dicParams.Add(":a001b_retail_bond_type_code", DataDefineDAO.A001B_RETAIL_BOND_TYPE_CODE)
                            dicParams.Add(":a001b_kor_bond_nm", DataDefineDAO.A001B_KOR_BOND_NM)
                            dicParams.Add(":a001b_eng_bond_nm", DataDefineDAO.A001B_ENG_BOND_NM)
                            dicParams.Add(":a001b_tsc_id", DataDefineDAO.A001B_TSC_ID)
                            dicParams.Add(":a001b_list_type_code", DataDefineDAO.A001B_LIST_TYPE_CODE)
                            dicParams.Add(":a001b_bond_ancd_code", DataDefineDAO.A001B_BOND_ANCD_CODE)
                            dicParams.Add(":a001b_guarantee_type_code", DataDefineDAO.A001B_GUARANTEE_TYPE_CODE)
                            dicParams.Add(":a001b_int_pay_type_code", DataDefineDAO.A001B_INT_PAY_TYPE_CODE)
                            dicParams.Add(":a001b_list_day", DataDefineDAO.A001B_LIST_DAY)
                            dicParams.Add(":a001b_issue_day", DataDefineDAO.A001B_ISSUE_DAY)
                            dicParams.Add(":a001b_mat_day", DataDefineDAO.A001B_MAT_DAY)
                            dicParams.Add(":a001b_sell_day", DataDefineDAO.A001B_SELL_DAY)
                            dicParams.Add(":a001b_issue_amt", DataDefineDAO.A001B_ISSUE_AMT)
                            dicParams.Add(":a001b_coupon_rate", DataDefineDAO.A001B_COUPON_RATE)
                            dicParams.Add(":a001b_int_pay_month_calc", DataDefineDAO.A001B_INT_PAY_MONTH_CALC)
                            dicParams.Add(":a001b_int_pay_time_type_code", DataDefineDAO.A001B_INT_PAY_TIME_TYPE_CODE)
                            dicParams.Add(":a001b_int_pay_term_type_code", DataDefineDAO.A001B_INT_PAY_TERM_TYPE_CODE)
                            dicParams.Add(":a001b_int_end_month_type", DataDefineDAO.A001B_INT_END_MONTH_TYPE)
                            dicParams.Add(":a001b_int_pay_unit_type", DataDefineDAO.A001B_INT_PAY_UNIT_TYPE)
                            dicParams.Add(":a001b_pre_sell_int_type_code", DataDefineDAO.A001B_PRE_SELL_INT_TYPE_CODE)
                            dicParams.Add(":a001b_prcp_amt", DataDefineDAO.A001B_PRCP_AMT)
                            dicParams.Add(":a001b_list_amt", DataDefineDAO.A001B_LIST_AMT)
                            dicParams.Add(":a001b_cb_prcp_return_rate", DataDefineDAO.A001B_CB_PRCP_RETURN_RATE)
                            dicParams.Add(":a001b_installment_return_type", DataDefineDAO.A001B_INSTALLMENT_RETURN_TYPE)
                            dicParams.Add(":a001b_unredeem_term", DataDefineDAO.A001B_UNREDEEM_TERM)
                            dicParams.Add(":a001b_refund_term_times", DataDefineDAO.A001B_REFUND_TERM_TIMES)
                            dicParams.Add(":a001b_deal_stop_type", DataDefineDAO.A001B_DEAL_STOP_TYPE)
                            dicParams.Add(":a001b_prev_int_pay_day", DataDefineDAO.A001B_PREV_INT_PAY_DAY)
                            dicParams.Add(":a001b_next_int_pay_day", DataDefineDAO.A001B_NEXT_INT_PAY_DAY)
                            dicParams.Add(":a001b_hybrid_bond_type", DataDefineDAO.A001B_HYBRID_BOND_TYPE)
                            dicParams.Add(":a001b_strips_tag", DataDefineDAO.A001B_STRIPS_TAG)
                            dicParams.Add(":a001b_over_ask_standard_price", DataDefineDAO.A001B_OVER_ASK_STANDARD_PRICE)
                            dicParams.Add(":a001b_stlmt_trad_objt_tag", DataDefineDAO.A001B_STLMT_TRAD_OBJT_TAG)
                            dicParams.Add(":a001b_invt_atd_bnd_tp_cd", DataDefineDAO.A001B_INVT_ATD_BND_TP_CD)
                            dicParams.Add(":colNameVar_A001B", colNameVar_A001B)

                            'INSERT 작업 수행
                            connectionDB.saveData2(DB_Query.InsertRetailA001B, Nothing, dicParams, False)
                            'connectionDB.commitTrans()

                            lineCountA001B = lineCountA001B + 1
                            FileOpen(fileNumber, logFileName, OpenMode.Append)
                            Print(fileNumber, Date.Now & " | " & "(PM) 소매채권데이터 A001B : |" & DataDefineDAO.A001B_BOND_ID & " | " & DataDefineDAO.A001B_DAY & " | 라인 : " & lineCount & vbCrLf)
                            FileClose()

                        Else
                            If DataDefineDAO.temp_length = 327 And Utilities.ByteMidStrTrim(DataDefineDAO.tempLine, dbTableDataStartA001BArray(0), dbTableDataEndA001BArray(0)) <> "999999999999" Then

                                FileOpen(fileNumber, logFileName, OpenMode.Append)
                                Print(fileNumber, Date.Now & " | " & "(PM) 오류- A001B 채권소매시장 종목배치 길이(280)가 일치하지 않습니다. | 라인 : " & lineCount & vbCrLf)
                                FileClose()

                            End If
                        End If
                    End While
                End If

                '그리드에 처리 된 개수 기입 (A001B)
                For index = 0 To dtblDATA.Rows.Count - 1
                    trcode = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TR")
                    If trcode = "A001B" Then
                        frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "TDY_DIDCNT", lineCountA001B)
                        todayDoCountA001B = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TDY_DOCNT")
                        ' 처리 할 개수와 처리 된 개수 값이 같은 경우
                        If lineCountA001B = todayDoCountA001B Then
                            ' 해당 ROW의 완료 작성
                            frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "COMPLETE", "완료")
                            frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "CHK", 0)
                        End If
                        Exit For
                    End If
                Next

                '그리드에 TRCODE Check 확인
                For index = 0 To dtblDATA.Rows.Count - 1
                    trcode = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TR")
                    If trcode = "G300B" Then
                        isCheckedG300B = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "CHK")
                        Exit For
                    End If
                Next

                txtFileReader.Close()
                txtFileReader = My.Computer.FileSystem.OpenTextFileReader(DataDefineDAO.loc_G300B_temp_trgFile, System.Text.Encoding.Default) '파일 읽을 API

                'DB에서 가져온 COL_NAME값 변수에 대입
                For index = 0 To dbTableDataColNameG300BArray.Length - 1
                    If index = dbTableDataColNameG300BArray.Length - 1 Then
                        colNameVar_G300B = colNameVar_G300B & dbTableDataColNameG300BArray(index)
                    Else
                        colNameVar_G300B = colNameVar_G300B & dbTableDataColNameG300BArray(index) & ","
                    End If
                Next

                'G3027이 화면상에 체크가 되어 있다면 작업 진행
                If isCheckedG300B = True Then
                    While Not txtFileReader.EndOfStream '마지막줄까지 남김없이 읽어오기

                        DataDefineDAO.tempLine = txtFileReader.ReadLine()

                        DataDefineDAO.temp_length = Utilities.ByteLen(DataDefineDAO.tempLine)

                        'DataDefineDAO.trCode = Mid(DataDefineDAO.tempLine, 1, 5)

                        lineCount += 1

                        'KOSCOM UDP는 마지막 라인 소매종목분류한글명 필드에 (99999999999999999999) 들어오면 입력하지 않는다.
                        If DataDefineDAO.temp_length = 76 And Utilities.ByteMidStrTrim(DataDefineDAO.tempLine, CInt(dbTableDataStartG300BArray(0)), CInt(dbTableDataEndG300BArray(0))) <> "99999999" Then
                            DataDefineDAO.G300B_SEQ = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartG300BArray(0)), CInt(dbTableDataEndG300BArray(0)), 3) '일련번호
                            DataDefineDAO.G300B_RETAIL_BOND_TYPE_CODE = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartG300BArray(1)), CInt(dbTableDataEndG300BArray(1))) '소매채권분류코드
                            DataDefineDAO.G300B_KOR_TYPE_NAME = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartG300BArray(2)), CInt(dbTableDataEndG300BArray(2))) '소매종목분류한글명
                            DataDefineDAO.G300B_ENG_TYPE_NAME = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartG300BArray(3)), CInt(dbTableDataEndG300BArray(3))) '소매종목분류영문명

                            'Code Converting
                            'AS - IS : 0:조성호가제출불가, 1:조성호가제출가능
                            'TO - BE : Y:조성호가제출가능, N:불가

                            DataDefineDAO.G300B_ASK_SUBMIT_TYPE = Utilities.CatchCol(DataDefineDAO.tempLine, CInt(dbTableDataStartG300BArray(4)), CInt(dbTableDataEndG300BArray(4))) '소매채권조성호가가능여부

                            Dim ASK_SUBMIT_TYPE As Integer

                            If Replace(DataDefineDAO.G300B_ASK_SUBMIT_TYPE, "'", "") = "N" Then
                                ASK_SUBMIT_TYPE = 0
                            ElseIf Replace(DataDefineDAO.G300B_ASK_SUBMIT_TYPE, "'", "") = "Y" Then
                                ASK_SUBMIT_TYPE = 1
                            End If

                            DataDefineDAO.G300B_ASK_SUBMIT_TYPE = "" & "'" & ASK_SUBMIT_TYPE & "'"

                            'DataDefineDAO.G300B_INPUT_DAY = "'" & Format(Date.Now, "yyyy-MM-dd HH:mm:ss") & "'" '입력일

                            'connectionDB.beginTrans()
                            dicParams.Clear()

                            '딕셔너리 add
                            dicParams.Add(":g300b_seq", DataDefineDAO.G300B_SEQ)
                            dicParams.Add(":g300b_retail_bond_type_code", DataDefineDAO.G300B_RETAIL_BOND_TYPE_CODE)
                            dicParams.Add(":g300b_kor_type_name", DataDefineDAO.G300B_KOR_TYPE_NAME)
                            dicParams.Add(":g300b_eng_type_name", DataDefineDAO.G300B_ENG_TYPE_NAME)
                            dicParams.Add(":g300b_ask_submit_type", DataDefineDAO.G300B_ASK_SUBMIT_TYPE)
                            dicParams.Add(":colNameVar_G300B", colNameVar_G300B)

                            'INSERT 작업 수행
                            connectionDB.saveData2(DB_Query.InsertRetailG300B, Nothing, dicParams, False)
                            'connectionDB.commitTrans()

                            lineCountG300B = lineCountG300B + 1

                            FileOpen(fileNumber, logFileName, OpenMode.Append)
                            Print(fileNumber, Date.Now & " | " & "(PM) 소매채권데이터 G300B : |" & DataDefineDAO.G300B_RETAIL_BOND_TYPE_CODE & " | " & DataDefineDAO.G300B_SEQ & " | " & DataDefineDAO.G300B_KOR_TYPE_NAME & " | " & DataDefineDAO.G300B_ENG_TYPE_NAME & " | 라인 : " & lineCount & vbCrLf)
                            FileClose()

                        Else
                            If DataDefineDAO.temp_length = 76 And Utilities.ByteMidStrTrim(DataDefineDAO.tempLine, CInt(dbTableDataStartG300BArray(0)), CInt(dbTableDataEndG300BArray(0))) <> "99999999" Then

                                FileOpen(fileNumber, logFileName, OpenMode.Append)
                                Print(fileNumber, Date.Now & " | " & "(PM) 오류- G300B 채권소매시장 종목배치 길이(60)가 일치하지 않습니다. | 라인 : " & lineCount & vbCrLf)
                                FileClose()

                            End If
                        End If
                    End While
                End If

                '그리드에 처리 된 개수 기입 (G3027)
                For index = 0 To dtblDATA.Rows.Count - 1
                    trcode = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TR")
                    If trcode = "G300B" Then
                        frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "TDY_DIDCNT", lineCountG300B)
                        todayDoCountG300B = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TDY_DOCNT")
                        ' 처리 할 개수와 처리 된 개수 값이 같은 경우
                        If lineCountG300B = todayDoCountG300B Then
                            ' 해당 ROW의 완료 여부
                            frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "COMPLETE", "완료")
                            frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "CHK", 0)
                        End If
                        Exit For
                    End If
                Next

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | " & "(PM) 소매채권데이터 A001B / G300B DB에 입력 완료! " & vbCrLf)
                FileClose()

                Exit Function


            Catch ex As Exception
                MsgBox("소매채권입력 작업 중 오류 발생 : " & ex.ToString())
                Throw ex
            End Try
        End If

    End Function

#End Region

#Region "채권소매 데이터 그리드 작성_차세대 KRX 버전"
    Public Function TodayDoCountGridWriteNight2(logFileName As String, fileNumber As Integer)

        Try
            DataDefineDAO.loc_A001B_temp_trgFile = App_Path & "Data\A001B_" & Format(frmDataInserter.t_day.Value, "yyyyMMdd") & ".dat"
            DataDefineDAO.loc_G300B_temp_trgFile = App_Path & "Data\G300B_" & Format(frmDataInserter.t_day.Value, "yyyyMMdd") & ".dat"

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "(PM) TodayDoCountGridWriteNight : 처리 할 개수 그리드 기입 시작!" & vbCrLf)
            FileClose()

            '서버에 있는 파일 다운로드 후 작업 진행하는 로직 변경 (Cuz : 파일을 직접 접근하여 사용하면 해당 서버 또는 파일 자체가 문제가 발생 할 수 있기 때문)
            If frmDataInserter.opt_26.Checked = True Then
                DataDefineDAO.loc_A001B_trgFile = retailDataServerAddress26 & Format(frmDataInserter.t_day.Value, "yyyyMMdd") & "\A001B.dat" ' 연결 서버의 DAT 파일명
                DataDefineDAO.loc_G300B_trgFile = retailDataServerAddress26 & Format(frmDataInserter.t_day.Value, "yyyyMMdd") & "\G300B.dat" ' 연결 서버의 DAT 파일명
            Else
                DataDefineDAO.loc_A001B_trgFile = retailDataServerAddress25 & Format(frmDataInserter.t_day.Value, "yyyyMMdd") & "\A001B.dat" ' 연결 서버의 DAT 파일명
                DataDefineDAO.loc_G300B_trgFile = retailDataServerAddress25 & Format(frmDataInserter.t_day.Value, "yyyyMMdd") & "\G300B.dat" ' 연결 서버의 DAT 파일명
            End If

            Dim fileCheckTodayA001B As New System.IO.FileInfo(DataDefineDAO.loc_A001B_trgFile)
            Dim fileCheckTodayG300B As New System.IO.FileInfo(DataDefineDAO.loc_G300B_trgFile)
            Dim fileCheckTodayLocalA001B As New System.IO.FileInfo(DataDefineDAO.loc_A001B_temp_trgFile)
            Dim fileCheckTodayLocalG300B As New System.IO.FileInfo(DataDefineDAO.loc_G300B_temp_trgFile)


            If fileCheckTodayA001B.Exists = True And fileCheckTodayG300B.Exists = True Then

                'LTH[2023-03-20] 로컬 파일 내에 A001B, G300B 파일이 있을 경우 바로 진행하도록 수정
                If fileCheckTodayLocalA001B.Exists = False And fileCheckTodayLocalG300B.Exists = False Then

                    FileOpen(fileNumber, logFileName, OpenMode.Append)
                    Print(fileNumber, Date.Now & " | " & "(PM) TodayDoCountGridWriteNight : A001B, G300B이 서버 로컬에 파일이 없고 UDP에 존재하여 파일 복사 진행!" & vbCrLf)
                    FileClose()

                    System.IO.File.Copy(DataDefineDAO.loc_A001B_trgFile, DataDefineDAO.loc_A001B_temp_trgFile) '파일 복사
                    System.IO.File.Copy(DataDefineDAO.loc_G300B_trgFile, DataDefineDAO.loc_G300B_temp_trgFile) '파일 복사

                End If

                Dim index As Integer 'For 문 인덱스
                Dim dicParams = New Dictionary(Of String, String) ' 딕셔너리 선언
                dicParams.Clear()
                dicParams.Add(":tr", "A001B")

                Dim dbTableSeqA001BData As DataTable = connectionDB.getData2(DB_Query.STR_SEQ_Query, dicParams, True, False)
                Dim dbTableLenA001BData As DataTable = connectionDB.getData2(DB_Query.STR_LEN_Query, dicParams, True, False)

                Dim dbTableDataStartA001BArray(dbTableSeqA001BData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_SEQ)
                Dim dbTableDataEndA001BArray(dbTableLenA001BData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_LEN)

                For index = 0 To dbTableDataStartA001BArray.Length - 2
                    dbTableDataStartA001BArray(index) = dbTableSeqA001BData.Rows(index).Item("STR_SEQ")
                    dbTableDataEndA001BArray(index) = dbTableLenA001BData.Rows(index).Item("STR_LEN")
                Next

                dicParams.Clear()
                dicParams.Add(":tr", "G300B")

                Dim dbTableSeqG300BData As DataTable = connectionDB.getData2(DB_Query.STR_SEQ_Query, dicParams, True, False)
                Dim dbTableLenG300BData As DataTable = connectionDB.getData2(DB_Query.STR_LEN_Query, dicParams, True, False)

                Dim dbTableDataStartG300BArray(dbTableSeqG300BData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_SEQ)
                Dim dbTableDataEndG300BArray(dbTableLenG300BData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_LEN)

                For index = 0 To dbTableDataStartG300BArray.Length - 2
                    dbTableDataStartG300BArray(index) = dbTableSeqG300BData.Rows(index).Item("STR_SEQ")
                    dbTableDataEndG300BArray(index) = dbTableLenG300BData.Rows(index).Item("STR_LEN")
                Next

                dtblDATA = frmDataInserter.grd_RECEIVE_LIST.DataSource '그리드에 있는 데이터 값을 가져오기 

                txtFileReader = My.Computer.FileSystem.OpenTextFileReader(DataDefineDAO.loc_A001B_temp_trgFile, System.Text.Encoding.Default) '파일 읽을 API

                While Not txtFileReader.EndOfStream '마지막줄까지 남김없이 읽어오기
                    DataDefineDAO.tempLine = txtFileReader.ReadLine()

                    DataDefineDAO.temp_length = Utilities.ByteLen(DataDefineDAO.tempLine)

                    If DataDefineDAO.temp_length = 327 And Utilities.ByteMidStrTrim(DataDefineDAO.tempLine, dbTableDataStartA001BArray(3), dbTableDataEndA001BArray(3)) <> "999999999999" Then
                        lineCountGridA001B += 1
                    End If
                End While

                dbTableSeqA001BData.Clear()
                dbTableLenA001BData.Clear()

                '그리드에 처리 할 개수 기입 
                For index = 0 To dtblDATA.Rows.Count - 1
                    trcode = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TR")
                    If trcode = "A001B" Then
                        frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "TDY_DOCNT", lineCountGridA001B)
                        Exit For
                    End If
                Next

                txtFileReader.Close()
                'G300B 채권소매시장 소매종류코드
                txtFileReader = My.Computer.FileSystem.OpenTextFileReader(DataDefineDAO.loc_G300B_temp_trgFile, System.Text.Encoding.Default) '파일 읽을 API

                While Not txtFileReader.EndOfStream '마지막줄까지 남김없이 읽어오기
                    DataDefineDAO.tempLine = txtFileReader.ReadLine()
                    DataDefineDAO.temp_length = Utilities.ByteLen(DataDefineDAO.tempLine)

                    If DataDefineDAO.temp_length = 76 And Utilities.ByteMidStrTrim(DataDefineDAO.tempLine, CInt(dbTableDataStartG300BArray(0)), CInt(dbTableDataEndG300BArray(0))) <> "99999999" Then
                        lineCountGridG300B += 1
                    End If
                End While

                dbTableSeqG300BData.Clear()
                dbTableLenG300BData.Clear()

                ' 그리드에 처리 할 개수 기입(G300B)
                For index = 0 To dtblDATA.Rows.Count - 1
                    trcode = frmDataInserter.grdv_RECEIVE_LIST.GetRowCellValue(index, "TR")
                    If trcode = "G300B" Then
                        frmDataInserter.grdv_RECEIVE_LIST.SetRowCellValue(index, "TDY_DOCNT", lineCountGridG300B)
                        Exit For
                    End If
                Next

                txtFileReader.Close()

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | " & "(PM) TodayDoCountGridWriteNight : 처리 할 개수 그리드 기입 끝!" & vbCrLf)
                FileClose()

            End If

        Catch ex As Exception
            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "(PM) TodayDoCountGridWriteNight : 처리 할 개수 그리드 작성 중 오류 발생!" & vbCrLf)
            FileClose()

            MsgBox("TodayDoCountGridWriteNight 오류 발생 : " & ex.ToString())

            Throw ex
        End Try

    End Function
#End Region

End Class
