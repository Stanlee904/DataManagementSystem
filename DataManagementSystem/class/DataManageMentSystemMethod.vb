Public Class DataManageMentSystemMethod

    Public DataDefineDAO As New DataDefineDAO()
    Public Utilities As New Utilities()
    Public connectionDB As New DB_Agent()


#Region "Case VD"
    '대응가(VD) 일 경우
    Public Function GetDataVD(ByVal oneLineRow As String, ByVal inputLineCount As Integer, logFileName As String, fileNumber As Integer) As String
        DataDefineDAO.CheckLenLine = Utilities.ByteLen(oneLineRow) '한줄 길이 가져오기


        FileOpen(fileNumber, logFileName, OpenMode.Append)
        Print(fileNumber, Date.Now & " | GetDataVD : 작업 시작 | 라인 : " & inputLineCount & vbCrLf)
        FileClose()

        Try

            '한 문장의 길이가  200 BYTE 가  아니라면 진행
            If DataDefineDAO.CheckLenLine <> 200 Then
                GoTo SKIP
            End If

            Dim tagCountValue As Integer ' "최종" 으로 종목 조회 된 개수 
            Dim index As Integer 'For 문 인덱스
            Dim colNameVar_VD As String = "" 'COL_NAME BINDING 변수(쿼리의 명령문을 추가적으로 넣기 위한 변수)
            Dim dicParams = New Dictionary(Of String, String) ' 딕셔너리 선언
            dicParams.Add(":tr", "VD") ' TR 코드 확인

            Dim dbTableSeqData As DataTable = connectionDB.getData2(DB_Query.STR_SEQ_Query, dicParams, True, False) '한 문장에서 필요한 값을 가져오기 위한 시작 인덱스 배열 DB 에서 호출
            Dim dbTableLenData As DataTable = connectionDB.getData2(DB_Query.STR_LEN_Query, dicParams, True, False) ' 한 문장에서 필요한 값을 가져오기 위한 끝 인덱스 배열 DB 에서 호출
            Dim dbTableColNameData As DataTable = connectionDB.getData2(DB_Query.COL_NAME_Query, dicParams, True, False) ' TRCODE 별 컬럼 이름 값 가져오기
            Dim dbTableTableNameData As DataTable = connectionDB.getData2(DB_Query.TABLE_NAME_Query, dicParams, True, False) ' TRCODE 별 테이블 명 값 가져오기

            Dim dbTableDataStartArray(dbTableSeqData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_SEQ)
            Dim dbTableDataEndArray(dbTableLenData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_LEN)
            Dim dbTableDataColNameArray(dbTableColNameData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(COL_NAME)

            ' dbTableDataStartArray가 32개이지만 실질적 데이터는 30개만 들어가있음. 
            For index = 0 To dbTableDataStartArray.Length - 2
                dbTableDataStartArray(index) = dbTableSeqData.Rows(index).Item("STR_SEQ")
                dbTableDataEndArray(index) = dbTableLenData.Rows(index).Item("STR_LEN")
                dbTableDataColNameArray(index) = dbTableColNameData.Rows(index).Item("COL_NAME")
            Next

            '1. DB에서 데이터 추출 시작 / 끝 값을 가져온다. dbTableDataStartArray(0), dbTableDataEndArray(0)
            '2. 1줄에서 내가 원하는 데이터를 추출하여 변수에 대입한다. CatchCol
            DataDefineDAO.VD_BND_ID = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(0)), CInt(dbTableDataEndArray(0))) '종목코드
            DataDefineDAO.VD_APPLY_DAY = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(1)), CInt(dbTableDataEndArray(1)), 3) '적용일자
            DataDefineDAO.VD_APPLY_DAY = "'" & DateTime.ParseExact(DataDefineDAO.VD_APPLY_DAY, "yyyyMMdd", Nothing) & "'"

            DataDefineDAO.VD_MKT_ID = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(2)), CInt(dbTableDataEndArray(2))) ' 시장 ID
            DataDefineDAO.VD_TRD_CRC_ID = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(3)), CInt(dbTableDataEndArray(3))) ' 거래통화ID
            DataDefineDAO.VD_TRD_UNIT = CInt(Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(4)), CInt(dbTableDataEndArray(4)), 3)) ' 거래단위
            DataDefineDAO.VD_SHSL_PSB_YN = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(5)), CInt(dbTableDataEndArray(5))) ' 공매도 가능여부
            DataDefineDAO.VD_CDT_ORD_PSB_YN = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(6)), CInt(dbTableDataEndArray(6))) ' 신용주문가능여부
            DataDefineDAO.VD_DICR_END_PSB_YN = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(7)), CInt(dbTableDataEndArray(7))) ' 임의종료가능여부
            DataDefineDAO.VD_MKT_FMTN_PSB_YN = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(8)), CInt(dbTableDataEndArray(8))) '시장조정가능여부
            DataDefineDAO.VD_HSTL_PRC = CDbl(Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(9)), CInt(dbTableDataEndArray(9)), 3)) '상한가
            DataDefineDAO.VD_LSTL_PRC = CDbl(Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(10)), CInt(dbTableDataEndArray(10)), 3)) '하한가
            DataDefineDAO.VD_TRD_STP_YN = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(11)), CInt(dbTableDataEndArray(11))) '거래정지여부
            DataDefineDAO.VD_ADJS_SB_YN = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(12)), CInt(dbTableDataEndArray(12))) '정리매매여부
            DataDefineDAO.VD_LIST_DT = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(13)), CInt(dbTableDataEndArray(13)), 3) '상장일자
            DataDefineDAO.VD_LIST_DT = "'" & DateTime.ParseExact(DataDefineDAO.VD_LIST_DT, "yyyyMMdd", Nothing) & "'"

            DataDefineDAO.VD_BND_LIST_ABSH_DT = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(14)), CInt(dbTableDataEndArray(14)), 3) '채권상장폐지일자
            If DataDefineDAO.VD_BND_LIST_ABSH_DT = "00000000" Then
                DataDefineDAO.VD_BND_LIST_ABSH_DT = "Null"
            End If

            DataDefineDAO.VD_CLPRC = CInt(Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(15)), CInt(dbTableDataEndArray(15)), 3)) '종가
            DataDefineDAO.VD_CLPRC_ERNR = CInt(Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(16)), CInt(dbTableDataEndArray(16)), 3)) '종가수익률
            DataDefineDAO.VD_STD_PRC = CDbl(Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(17)), CInt(dbTableDataEndArray(17)), 3)) '기준가격
            DataDefineDAO.VD_SBST_PRC = CInt(Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(18)), CInt(dbTableDataEndArray(18)), 3)) '대용가격
            DataDefineDAO.VD_BND_LIST_ABSH_CAU_CD = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(19)), CInt(dbTableDataEndArray(19))) '채권상장폐지사유코드
            DataDefineDAO.VD_LIST_AMT = CLng(Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(20)), CInt(dbTableDataEndArray(20)), 3)) '상장금액(발행잔액(999999999999999999.999))

            DataDefineDAO.VD_BPRD_INT_PAYNT_DT = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(21)), CInt(dbTableDataEndArray(21)), 3) ' 전기이자지급일자
            If DataDefineDAO.VD_BPRD_INT_PAYNT_DT = "00000000" Then
                DataDefineDAO.VD_BPRD_INT_PAYNT_DT = "Null"
            End If

            DataDefineDAO.VD_NXTM_INT_PAYNT_DT = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(22)), CInt(dbTableDataEndArray(22)), 3) '차기이자지급일
            If DataDefineDAO.VD_NXTM_INT_PAYNT_DT = "00000000" Then
                DataDefineDAO.VD_NXTM_INT_PAYNT_DT = "Null"
            Else
                DataDefineDAO.VD_NXTM_INT_PAYNT_DT = "'" & DateTime.ParseExact(DataDefineDAO.VD_NXTM_INT_PAYNT_DT, "yyyyMMdd", Nothing) & "'"
            End If

            DataDefineDAO.VD_RTSL_BND_CLSS_CD = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(23)), CInt(dbTableDataEndArray(23))) '소매채권분류코드
            DataDefineDAO.VD_SAMT_SB_KND_CD = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(24)), CInt(dbTableDataEndArray(24))) '소액매매종류코드
            DataDefineDAO.VD_NTBN_KND_CD = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(25)), CInt(dbTableDataEndArray(25))) '국채종류코드
            DataDefineDAO.VD_NTBN_EXR_YCNT = CInt(Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(26)), CInt(dbTableDataEndArray(26)), 3)) '국채만기연수
            DataDefineDAO.VD_NTBN_STK_TP_CD = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(27)), CInt(dbTableDataEndArray(27))) '국채종목구분코드
            DataDefineDAO.VD_REPO_CLSS_CD = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(28)), CInt(dbTableDataEndArray(28))) 'REPO 분류코드
            DataDefineDAO.VD_INVT_ATD_BND_TP_CD = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(29)), CInt(dbTableDataEndArray(29))) 'REPO 분류코드(0: 해당없음 1: 지정예고 2: 지정)
            DataDefineDAO.VD_DIV_REPAY_DT = Utilities.CatchCol(oneLineRow, CInt(dbTableDataStartArray(30)), CInt(dbTableDataEndArray(30)), 3) '분할상환일자
            If DataDefineDAO.VD_DIV_REPAY_DT = "00000000" Then
                DataDefineDAO.VD_DIV_REPAY_DT = "Null"
            Else
                DataDefineDAO.VD_DIV_REPAY_DT = "'" & DataDefineDAO.VD_DIV_REPAY_DT & "'"
            End If

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | (AM) | 대응가(VD)  변수 초기화 완료! | 라인 : " & inputLineCount & vbCrLf)
            FileClose()

            connectionDB.beginTrans()

            dicParams.Clear()

            dicParams.Add(":vd_bnd_id", DataDefineDAO.VD_BND_ID)
            dicParams.Add(":bnd_id", dbTableDataColNameArray(0))
            dicParams.Add(":tr_tablename", dbTableTableNameData.Rows(0).Item("TABLE_NAME"))

            ' TAG = 1 인 값 추출
            Dim findFinalTag As DataTable = connectionDB.getData2(DB_Query.FindTagFinal, dicParams)

            ' tag : 1 인 최종으로 종목 
            tagCountValue = findFinalTag.Rows(0).Item("CNT")

            ' "최종"  으로 종목이 있을 경우 삭제
            If CInt(tagCountValue) = 1 Then
                connectionDB.saveData2(DB_Query.checkExistsAndDeleteTag, Nothing, dicParams, False)
            End If

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | (AM) | 대응가(VD) TAG =1 값 추출 / 최종 으로 종목 있을 경우 삭제 진행! | 라인 : " & inputLineCount & vbCrLf)
            FileClose()

            dicParams.Clear()

            'DB에서 가져온 COL_NAME값 변수에 대입
            For index = 0 To dbTableDataColNameArray.Length - 1
                If index = dbTableDataColNameArray.Length - 1 Then
                    colNameVar_VD = colNameVar_VD & dbTableDataColNameArray(index)
                Else
                    colNameVar_VD = colNameVar_VD & dbTableDataColNameArray(index) & ","
                End If
            Next

            ' insert를 위한 딕셔너리 파라미터 추가 
            dicParams.Add(":vd_bnd_id", DataDefineDAO.VD_BND_ID)
            dicParams.Add(":vd_apply_day", DataDefineDAO.VD_APPLY_DAY)
            dicParams.Add(":vd_mkt_id", DataDefineDAO.VD_MKT_ID)
            dicParams.Add(":vd_trd_crc_id", DataDefineDAO.VD_TRD_CRC_ID)
            dicParams.Add(":vd_trd_unit", CInt(DataDefineDAO.VD_TRD_UNIT))
            dicParams.Add(":vd_shsl_psb_yn", DataDefineDAO.VD_SHSL_PSB_YN)
            dicParams.Add(":vd_cdt_ord_psb_yn", DataDefineDAO.VD_CDT_ORD_PSB_YN)
            dicParams.Add(":vd_dicr_end_psb_yn", DataDefineDAO.VD_DICR_END_PSB_YN)
            dicParams.Add(":vd_mkt_fmtn_psb_yn", DataDefineDAO.VD_MKT_FMTN_PSB_YN)
            dicParams.Add(":vd_hstl_prc", CDbl(DataDefineDAO.VD_HSTL_PRC))
            dicParams.Add(":vd_lstl_prc", CDbl(DataDefineDAO.VD_LSTL_PRC))
            dicParams.Add(":vd_trd_stp_yn", DataDefineDAO.VD_TRD_STP_YN)
            dicParams.Add(":vd_adjs_sb_yn", DataDefineDAO.VD_ADJS_SB_YN)
            dicParams.Add(":vd_list_dt", DataDefineDAO.VD_LIST_DT)
            dicParams.Add(":vd_bnd_list_absh_dt", If(DataDefineDAO.VD_BND_LIST_ABSH_DT = "Null", DataDefineDAO.VD_BND_LIST_ABSH_DT, "'" & DataDefineDAO.VD_BND_LIST_ABSH_DT & "'"))
            dicParams.Add(":vd_clprc", CInt(DataDefineDAO.VD_CLPRC))
            dicParams.Add(":vd_clprc_ernr", CInt(DataDefineDAO.VD_CLPRC_ERNR))
            dicParams.Add(":vd_std_prc", CDbl(DataDefineDAO.VD_STD_PRC))
            dicParams.Add(":vd_sbst_prc", CInt(DataDefineDAO.VD_SBST_PRC))
            dicParams.Add(":vd_bnd_list_absh_cau_cd", DataDefineDAO.VD_BND_LIST_ABSH_CAU_CD)
            dicParams.Add(":vd_list_amt", CLng(DataDefineDAO.VD_LIST_AMT))
            dicParams.Add(":vd_bprd_int_paynt_dt", If(DataDefineDAO.VD_BPRD_INT_PAYNT_DT = "Null", DataDefineDAO.VD_BPRD_INT_PAYNT_DT, "'" & DataDefineDAO.VD_BPRD_INT_PAYNT_DT & "'"))
            dicParams.Add(":vd_nxtm_int_paynt_dt", If(DataDefineDAO.VD_NXTM_INT_PAYNT_DT = "Null", DataDefineDAO.VD_NXTM_INT_PAYNT_DT, DataDefineDAO.VD_NXTM_INT_PAYNT_DT))
            dicParams.Add(":vd_rtsl_bnd_clss_cd", DataDefineDAO.VD_RTSL_BND_CLSS_CD)
            dicParams.Add(":vd_samt_sb_knd_cd", DataDefineDAO.VD_SAMT_SB_KND_CD)
            dicParams.Add(":vd_ntbn_knd_cd", DataDefineDAO.VD_NTBN_KND_CD)
            dicParams.Add(":vd_ntbn_exr_ycnt", CInt(DataDefineDAO.VD_NTBN_EXR_YCNT))
            dicParams.Add(":vd_ntbn_stk_tp_cd", DataDefineDAO.VD_NTBN_STK_TP_CD)
            dicParams.Add(":vd_repo_clss_cd", DataDefineDAO.VD_REPO_CLSS_CD)
            dicParams.Add(":vd_invt_atd_bnd_tp_cd", DataDefineDAO.VD_INVT_ATD_BND_TP_CD)
            dicParams.Add(":vd_div_repay_dt", DataDefineDAO.VD_DIV_REPAY_DT)
            dicParams.Add(":today_date", "'" & Date.Today & "'")
            dicParams.Add(":colNameVar_VD", colNameVar_VD)
            dicParams.Add(":tr_tablename", dbTableTableNameData.Rows(0).Item("TABLE_NAME"))

            '데이터 INSERT 진행 (최종)'
            connectionDB.saveData2(DB_Query.InsertTodayVDdata, Nothing, dicParams, False)

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | (AM) | 대응가(VD) 데이터 INSERT 진행 (최종) | 라인 : " & inputLineCount & vbCrLf)
            FileClose()

            '데이터 INSERT 진행 (HISTORY)'
            connectionDB.saveData2(DB_Query.InsertVDdataHistory, Nothing, dicParams, False)
            connectionDB.commitTrans()

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | (AM) | 대응가(VD) 데이터 INSERT 진행 (HISTORY) | 라인 : " & inputLineCount & vbCrLf)
            Print(fileNumber, Date.Now & " | (AM) | 대응가(VD) - " & DataDefineDAO.VD_BND_ID & " | " & DataDefineDAO.VD_APPLY_DAY & "에 추가 되어습니다. | 라인 : " & inputLineCount & vbCrLf)
            FileClose()

            Exit Function

SKIP:
            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | (AM) | 대응가(VD) -  VD의 고정길이가 맞지 않습니다. | 라인 : " & inputLineCount & vbCrLf)
            FileClose()

            Exit Function

        Catch ex As Exception
            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | (AM) | 대응가(VD) - 작업 중 오류 발생! | 라인 : " & inputLineCount & "오류 내용 : " & ex.ToString() & vbCrLf)
            FileClose()

            MsgBox("VD 대용가 작업 중 오류 발생! 행 위치 : " & inputLineCount & "오류 내용 : " & ex.ToString())
            connectionDB.rollbackTrans()
            Throw ex
        Finally
            connectionDB.endTrans()
        End Try

    End Function
#End Region

    '회사코드정보 일 경우
    Public Function GetDataV7(ByVal oneLineRow As String, ByVal inputLineCount As Integer, logFileName As String, fileNumber As Integer)

        DataDefineDAO.CheckLenLine = Utilities.ByteLen(oneLineRow) '한줄 길이 가져오기

        If frmDataInserter.optNight.Checked = True Then

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | (PM) GetDataV7 : 작업 시작 | 라인 : " & inputLineCount & vbCrLf)
            FileClose()
        Else

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | (AM) GetDataV7 : 작업 시작 | 라인 : " & inputLineCount & vbCrLf)
            FileClose()
        End If

        Try
            ' 한줄의 바이트 길이가 191이 아니라면 SKIP으로 이동
            If DataDefineDAO.CheckLenLine <> 191 Then
                GoTo SKIP
            End If

            Dim index As Integer 'For 문 인덱스
            Dim queryVar As String = "" '쿼리 변수
            Dim colNameVar_V7 As String = "" 'COL_NAME BINDING 변수(쿼리의 명령문을 추가적으로 넣기 위한 변수)
            Dim colNameVar_V7_org_Id As String = ""
            Dim colNameVar_V7_fin_org_Id As String = ""
            Dim colNameVar_V7_spc_org_id As String = ""
            Dim dicParams = New Dictionary(Of String, String) ' 딕셔너리 선언

            dicParams.Add(":tr", "V7")

            Dim dbTableSeqData As DataTable = connectionDB.getData2(DB_Query.STR_SEQ_Query, dicParams, True, False) '한 문장에서 필요한 값을 가져오기 위한 시작 인덱스 배열 DB 에서 호출
            Dim dbTableLenData As DataTable = connectionDB.getData2(DB_Query.STR_LEN_Query, dicParams, True, False) '한 문장에서 필요한 값을 가져오기 위한 끝 인덱스 배열 DB 에서 호출
            Dim dbTableColNameData As DataTable = connectionDB.getData2(DB_Query.COL_NAME_Query, dicParams, True, False) 'TRCODE 별 컬럼 명 값 가져오기
            Dim dbTableTableNameData As DataTable = connectionDB.getData2(DB_Query.TABLE_NAME_Query, dicParams, True, False) ' TRCODE 별 테이블 명 값 가져오기


            Dim dbTableDataStartArray(dbTableSeqData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_SEQ)
            Dim dbTableDataEndArray(dbTableLenData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_LEN)
            Dim dbTableDataColNameArray(dbTableColNameData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(COL_NAME)


            For index = 0 To dbTableDataStartArray.Length - 2
                dbTableDataStartArray(index) = dbTableSeqData.Rows(index).Item("STR_SEQ")
                dbTableDataEndArray(index) = dbTableLenData.Rows(index).Item("STR_LEN")
                dbTableDataColNameArray(index) = dbTableColNameData.Rows(index).Item("COL_NAME")
            Next

            '--------------------------------------------일반 V7 작업--------------------------------------------

            '1. DB에서 데이터 추출 시작 / 끝 값을 가져온다. 
            '2. Byte 단위로 값을 추출한다. 
            DataDefineDAO.temp_name = Trim(Utilities.ByteMid(oneLineRow, dbTableDataStartArray(0), dbTableDataEndArray(0)))

            connectionDB.beginTrans()

            If Utilities.ByteMid(DataDefineDAO.temp_name, 1, 4) = "(주)" Or Utilities.ByteMid(DataDefineDAO.temp_name, 1, 4) = "(유)" Or Utilities.ByteMid(DataDefineDAO.temp_name, 1, 4) = "(의)" Then  ' (주), (유), (의) 일 경우 
                DataDefineDAO.name_tag = Utilities.ByteMid(DataDefineDAO.temp_name, 1, 4)
                DataDefineDAO.name_len = Utilities.ByteLen(DataDefineDAO.temp_name)
                DataDefineDAO.temp_name = Utilities.ByteMid(DataDefineDAO.temp_name, 5, DataDefineDAO.name_len)
            End If

            DataDefineDAO.V7_ORG_NAME = DataDefineDAO.temp_name & DataDefineDAO.name_tag
            DataDefineDAO.V7_ORG_ID = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(1), dbTableDataEndArray(1)) 'KOSCOM 회사코드
            DataDefineDAO.V7_CP_ORG_ID = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(2), dbTableDataEndArray(2)) '회사채고유코드
            DataDefineDAO.V7_FIN_ORG_ID = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(3), dbTableDataEndArray(3)) '기관코드
            DataDefineDAO.V7_SPC_ORG_ID = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(4), dbTableDataEndArray(4)) '특수채코드

            If frmDataInserter.optNight.Checked = True Then

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) GetDataV7 : 변수 초기화 완료! | 라인 : " & inputLineCount & vbCrLf)
                FileClose()
            Else

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) GetDataV7 : 변수 초기화 완료! | 라인 : " & inputLineCount & vbCrLf)
                FileClose()
            End If

            dicParams.Add(":v7_org_id", DataDefineDAO.V7_ORG_ID) 'KOSCOM 회사코드
            dicParams.Add(":org_id", dbTableDataColNameArray(1)) 'KOSCOM 회사코드
            dicParams.Add(":tr_tablename", dbTableTableNameData.Rows(0).Item("TABLE_NAME"))

            '회사코드정보 조회
            Dim checkCountOrgId As DataTable = connectionDB.getData2(DB_Query.checkCountOrg_ID, dicParams, False, False)

            '2016-09-07 주정석 00494는 KEB하나은행으로 입력되도록 수정함
            '           거래소에 하나은행에서 사명변경요청을 해야 발행자명이 변경되는데
            '           하나은행에서 요청하지 않아 하드코딩함(이승엽대리 요청)

            If DataDefineDAO.V7_ORG_ID = "00494" Then
                DataDefineDAO.V7_ORG_NAME = "KEB하나은행"
            End If

            dicParams.Add(":v7_org_name", "'" & DataDefineDAO.V7_ORG_NAME & "'")
            dicParams.Add(":v7_cp_org_id", DataDefineDAO.V7_CP_ORG_ID)
            dicParams.Add(":v7_fin_org_id", DataDefineDAO.V7_FIN_ORG_ID)
            dicParams.Add(":v7_spc_org_id", DataDefineDAO.V7_SPC_ORG_ID)
            dicParams.Add(":today_date", "'" & Date.Now & " : 회사코드정보(V7) 정보에 의해 수정 됨 " & "'")

            'DB에서 가져온 COL_NAME값 변수에 대입
            For index = 0 To dbTableDataColNameArray.Length - 1
                If index = dbTableDataColNameArray.Length - 1 Then
                    colNameVar_V7 = colNameVar_V7 & dbTableDataColNameArray(index)
                Else
                    colNameVar_V7 = colNameVar_V7 & dbTableDataColNameArray(index) & ","
                End If
            Next

            '회사코드정보(V7)의 정보가 0보다 크면  UPDATE
            If checkCountOrgId.Rows.Count > 0 Then

                If DataDefineDAO.V7_ORG_ID <> "00000" And DataDefineDAO.V7_ORG_ID <> "" And IsNothing(DataDefineDAO.V7_ORG_ID) = False Then

                    '데이터 UPDATE 진행 '
                    connectionDB.saveData2(DB_Query.updateOrgCodeInform, Nothing, dicParams, False)

                    '완료 되면 로그 찍기 
                    If frmDataInserter.optNight.Checked = True Then

                        FileOpen(fileNumber, logFileName, OpenMode.Append)
                        Print(fileNumber, Date.Now & " | (PM) 회사코드정보(V7) - |" & DataDefineDAO.V7_ORG_NAME & " | " & DataDefineDAO.V7_ORG_ID & "가 수정 처리 되었습니다 | 라인 : " & inputLineCount & vbCrLf)
                        FileClose()
                    Else

                        FileOpen(fileNumber, logFileName, OpenMode.Append)
                        Print(fileNumber, Date.Now & " | (AM) 회사코드정보(V7) - |" & DataDefineDAO.V7_ORG_NAME & " | " & DataDefineDAO.V7_ORG_ID & "가 수정 처리 되었습니다 | 라인 : " & inputLineCount & vbCrLf)
                        FileClose()
                    End If

                Else
                    '데이터 UPDATE 진행 '
                    connectionDB.saveData2(DB_Query.updateOrgCodeInformWithoutCpOrgId, Nothing, dicParams, False)

                    '완료 되면 로그 찍기 
                    If frmDataInserter.optNight.Checked = True Then

                        FileOpen(fileNumber, logFileName, OpenMode.Append)
                        Print(fileNumber, Date.Now & " | (PM) 회사코드정보(V7) - |" & DataDefineDAO.V7_ORG_NAME & " | " & DataDefineDAO.V7_ORG_ID & "가 수정 처리 되었습니다 | 라인 : " & inputLineCount & vbCrLf)
                        FileClose()
                    Else

                        FileOpen(fileNumber, logFileName, OpenMode.Append)
                        Print(fileNumber, Date.Now & " | (AM) 회사코드정보(V7) - |" & DataDefineDAO.V7_ORG_NAME & " | " & DataDefineDAO.V7_ORG_ID & "가 수정 처리 되었습니다 | 라인 : " & inputLineCount & vbCrLf)
                        FileClose()
                    End If

                End If
            Else
                dicParams.Clear()

                dicParams.Add(":v7_org_id", DataDefineDAO.V7_ORG_ID) 'KOSCOM 회사코드
                dicParams.Add(":v7_org_name", "'" & DataDefineDAO.V7_ORG_NAME & "'")
                dicParams.Add(":v7_cp_org_id", DataDefineDAO.V7_CP_ORG_ID)
                dicParams.Add(":v7_fin_org_id", DataDefineDAO.V7_FIN_ORG_ID)
                dicParams.Add(":v7_spc_org_id", DataDefineDAO.V7_SPC_ORG_ID)
                dicParams.Add(":today_date", "'" & Date.Now & " : 회사코드정보(V7) 정보에 의해 입력 됨 " & "'")
                dicParams.Add(":colNameVar_V7", colNameVar_V7)
                dicParams.Add(":tr_tablename", dbTableTableNameData.Rows(0).Item("TABLE_NAME"))


                ' 회사코드정보(V7)의 정보가 0보다 작으면 INSERT
                connectionDB.saveData2(DB_Query.InsertOrgCodeInform, Nothing, dicParams, False)

                '완료 되면 로그 찍기 
                If frmDataInserter.optNight.Checked = True Then

                    FileOpen(fileNumber, logFileName, OpenMode.Append)
                    Print(fileNumber, Date.Now & " | (PM) 회사코드정보(V7) - |" & DataDefineDAO.V7_ORG_NAME & " | " & DataDefineDAO.V7_ORG_ID & "가 신규 입력 되었습니다 | 라인 : " & inputLineCount & vbCrLf)
                    FileClose()
                Else

                    FileOpen(fileNumber, logFileName, OpenMode.Append)
                    Print(fileNumber, Date.Now & " | (AM) 회사코드정보(V7) - |" & DataDefineDAO.V7_ORG_NAME & " | " & DataDefineDAO.V7_ORG_ID & "가 신규 입력 되었습니다 | 라인 : " & inputLineCount & vbCrLf)
                    FileClose()
                End If

            End If

            dicParams.Clear()
            dbTableTableNameData.Clear()

            '--------------------------------------------KOSCOM V7 작업--------------------------------------------

            DataDefineDAO.V7_DATA(0) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(0), dbTableDataEndArray(0))
            DataDefineDAO.V7_DATA(1) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(1), dbTableDataEndArray(1))
            DataDefineDAO.V7_DATA(2) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(2), dbTableDataEndArray(2))
            DataDefineDAO.V7_DATA(3) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(3), dbTableDataEndArray(3))
            DataDefineDAO.V7_DATA(4) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(4), dbTableDataEndArray(4))

            If frmDataInserter.opt_Day.Checked = True Then
                DataDefineDAO.V7_DATA(5) = "'AM'"
            Else
                DataDefineDAO.V7_DATA(5) = "'PM'"
            End If

            '2016-09-07 주정석 00494는 KEB하나은행으로 입력되도록 수정함
            '           거래소에 하나은행에서 사명변경요청을 해야 발행자명이 변경되는데
            '           하나은행에서 요청하지 않아 하드코딩함(이승엽대리 요청)

            If DataDefineDAO.V7_DATA(1) = "00494" Then
                DataDefineDAO.V7_DATA(0) = "KEB하나은행"
            End If


            For i = 0 To 5
                If i = 5 Then
                    queryVar = queryVar & DataDefineDAO.V7_DATA(i)
                Else
                    queryVar = queryVar & DataDefineDAO.V7_DATA(i) & ","
                End If
            Next

            dicParams.Add(":tr", "KOSCOM V7")
            dbTableTableNameData = connectionDB.getData2(DB_Query.TABLE_NAME_Query, dicParams, True, False) ' TRCODE 별 테이블 명 값 가져오기(KOSCOM V7)

            dicParams.Add(":v7_Data", queryVar)
            dicParams.Add(":colNameVar_V7_org_Id", dbTableDataColNameArray(1))
            dicParams.Add(":colNameVar_V7_fin_org_Id", dbTableDataColNameArray(3))
            dicParams.Add(":colNameVar_V7_spc_org_id", dbTableDataColNameArray(4))
            dicParams.Add(":tr_tablename", dbTableTableNameData.Rows(0).Item("TABLE_NAME"))


            connectionDB.saveData2(DB_Query.InsertKoscomV7, Nothing, dicParams, False)
            connectionDB.commitTrans()
            If frmDataInserter.optNight.Checked = True Then

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) KOSCOM V7 - |" & DataDefineDAO.V7_ORG_NAME & " | " & DataDefineDAO.V7_ORG_ID & "가 입력 되었습니다 | 라인 : " & inputLineCount & vbCrLf)
                FileClose()
            Else

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) KOSCOM V7 - |" & DataDefineDAO.V7_ORG_NAME & " | " & DataDefineDAO.V7_ORG_ID & "가 입력 되었습니다 | 라인 : " & inputLineCount & vbCrLf)
                FileClose()
            End If

            Exit Function

SKIP:
            If frmDataInserter.optNight.Checked = True Then

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) V7의 고정길이가 맞지 않습니다. | 라인 : " & inputLineCount & vbCrLf)
                FileClose()
            Else

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) V7의 고정길이가 맞지 않습니다. | 라인 : " & inputLineCount & vbCrLf)
                FileClose()
            End If
            Exit Function

        Catch ex As Exception
            If frmDataInserter.optNight.Checked = True Then

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) GetDataV7 오류 발생! 오류 내용 : " & ex.ToString() & "| 라인 : " & inputLineCount & vbCrLf)
                FileClose()
            Else

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) GetDataV7 오류 발생! 오류 내용 : " & ex.ToString() & "| 라인 : " & inputLineCount & vbCrLf)
                FileClose()
            End If
            connectionDB.rollbackTrans()
            MsgBox("V7 회사코드정보 작업 중 오류 발생! 행 위치 : " & inputLineCount & "오류 내용 : " & ex.ToString())
            Throw ex

        Finally
            connectionDB.endTrans()
        End Try

    End Function


    '채권발행기관코드_거래소 일 경우
    Public Function GetDataV3(ByVal oneLineRow As String, ByVal inputLineCount As String, logFileName As String, fileNumber As Integer)

        If frmDataInserter.optNight.Checked = True Then

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | (PM) GetDataV3 : 작업 시작" & "| 라인 : " & inputLineCount & vbCrLf)
            FileClose()
        Else

            FileOpen(fileNumber, logFileName, OpenMode.Append)
            Print(fileNumber, Date.Now & " | (AM) GetDataV3 : 작업 시작" & "| 라인 : " & inputLineCount & vbCrLf)
            FileClose()
        End If

        DataDefineDAO.CheckLenLine = Utilities.ByteLen(oneLineRow)

        Try
            If DataDefineDAO.CheckLenLine <> 300 Then
                GoTo SKIP
            End If

            Dim queryVar As String = "" '쿼리 변수
            Dim index As Integer 'For 문 인덱스
            Dim colNameVar_V3 As String = "" 'COL_NAME BINDING 변수(쿼리의 명령문을 추가적으로 넣기 위한 변수)
            Dim colNameVar_V3_issuer_org_id As String = "" 'KOSCOM V3 발행기관코드(쿼리의 명령문을 추가적으로 넣기 위한 변수)
            Dim dicParams = New Dictionary(Of String, String) ' 딕셔너리 선언

            dicParams.Clear()

            dicParams.Add(":tr", "V3")

            Dim dbTableSeqData As DataTable = connectionDB.getData2(DB_Query.STR_SEQ_Query, dicParams, True, False)
            Dim dbTableLenData As DataTable = connectionDB.getData2(DB_Query.STR_LEN_Query, dicParams, True, False)
            Dim dbTableColNameData As DataTable = connectionDB.getData2(DB_Query.COL_NAME_Query, dicParams, True, False)
            Dim dbTableTableNameData As DataTable = connectionDB.getData2(DB_Query.TABLE_NAME_Query, dicParams, True, False) ' TRCODE 별 테이블 명 값 가져오기

            Dim dbTableDataStartArray(dbTableSeqData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_SEQ)
            Dim dbTableDataEndArray(dbTableLenData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(STR_LEN)
            Dim dbTableDataColNameArray(dbTableColNameData.Rows.Count) As String '해당 쿼리의 결과물 개수 만큼 배열 생성(COL_NAME)


            For index = 0 To dbTableDataStartArray.Length - 2
                dbTableDataStartArray(index) = dbTableSeqData.Rows(index).Item("STR_SEQ")
                dbTableDataEndArray(index) = dbTableLenData.Rows(index).Item("STR_LEN")
                dbTableDataColNameArray(index) = dbTableColNameData.Rows(index).Item("COL_NAME")
            Next

            '--------------------------------------------일반 V3 작업-------------------------------------------------------------------------

            DataDefineDAO.V3_ISSUER_ORG_ID = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(0), dbTableDataEndArray(0)) '발행기관코드
            DataDefineDAO.V3_ORG_FULL_NM = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(1), dbTableDataEndArray(1)) '회사명
            DataDefineDAO.V3_ORG_SHORT_NM = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(2), dbTableDataEndArray(2)) '회사약명
            DataDefineDAO.V3_ORG_ENG_FULL_NM = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(3), dbTableDataEndArray(3)) '회사영문명
            DataDefineDAO.V3_ORG_ENG_SHORT_NM = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(4), dbTableDataEndArray(4)) '회사영문약명

            If frmDataInserter.optNight.Checked = True Then

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) GetDataV3 변수 초기화 완료! | 라인 : " & inputLineCount & vbCrLf)
                FileClose()
            Else

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) GetDataV3 : 변수 초기화 완료! | 라인 : " & inputLineCount & vbCrLf)
                FileClose()
            End If

            'DB에서 가져온 COL_NAME값 변수에 대입
            For index = 0 To dbTableDataColNameArray.Length - 1
                If index = dbTableDataColNameArray.Length - 1 Then
                    colNameVar_V3 = colNameVar_V3 & dbTableDataColNameArray(index)
                Else
                    colNameVar_V3 = colNameVar_V3 & dbTableDataColNameArray(index) & ","
                End If
            Next

            connectionDB.beginTrans()

            '채권발행기관코드_거래소
            dicParams.Clear()
            dicParams.Add(":v3_issuer_org_id", DataDefineDAO.V3_ISSUER_ORG_ID)
            dicParams.Add(":v3_org_full_nm", DataDefineDAO.V3_ORG_FULL_NM)
            dicParams.Add(":v3_org_short_nm", DataDefineDAO.V3_ORG_SHORT_NM)
            dicParams.Add(":v3_org_eng_full_nm", DataDefineDAO.V3_ORG_ENG_FULL_NM)
            dicParams.Add(":v3_org_eng_short_nm", DataDefineDAO.V3_ORG_ENG_SHORT_NM)
            dicParams.Add(":issue_org_id", dbTableDataColNameArray(0))
            dicParams.Add(":colNameVar_V3", colNameVar_V3)
            dicParams.Add(":tr_tablename", dbTableTableNameData.Rows(0).Item("TABLE_NAME"))

            ' 발행기관코드가 있는지 데이터 조회 
            Dim issueOrgIdNumber As DataTable = connectionDB.getData2(DB_Query.CheckCountIssueOrgID, dicParams, False, False)

            '발행기관코드가 0보다 크면 update / 작으면 INSERT
            If issueOrgIdNumber.Rows.Count > 0 Then

                '발행기관코드가 0보다 크면 update / 작으면 INSERT
                connectionDB.saveData2(DB_Query.UpdateV3, Nothing, dicParams, False)

                '완료 되면 로그 찍기 
                If frmDataInserter.optNight.Checked = True Then

                    FileOpen(fileNumber, logFileName, OpenMode.Append)
                    Print(fileNumber, Date.Now & " | (PM) 채권발행기관코드_거래소(V3) - |" & DataDefineDAO.V3_ORG_SHORT_NM & " | " & DataDefineDAO.V3_ISSUER_ORG_ID & "가 수정 처리 되었습니다 | 라인 : " & inputLineCount & vbCrLf)
                    FileClose()
                Else

                    FileOpen(fileNumber, logFileName, OpenMode.Append)
                    Print(fileNumber, Date.Now & " | (AM) 채권발행기관코드_거래소(V3) - |" & DataDefineDAO.V3_ORG_SHORT_NM & " | " & DataDefineDAO.V3_ISSUER_ORG_ID & "가 수정 처리 되었습니다 | 라인 : " & inputLineCount & vbCrLf)
                    FileClose()
                End If

            Else
                '발행기관코드가 0보다 크면 update / 작으면 INSERT
                connectionDB.saveData2(DB_Query.InsertV3, Nothing, dicParams, False)

                '완료 되면 로그 찍기 
                If frmDataInserter.optNight.Checked = True Then

                    FileOpen(fileNumber, logFileName, OpenMode.Append)
                    Print(fileNumber, Date.Now & " | (PM) 채권발행기관코드_거래소(V3) - |" & DataDefineDAO.V3_ORG_SHORT_NM & " | " & DataDefineDAO.V3_ISSUER_ORG_ID & "가 신규 입력 되었습니다 | 라인 : " & inputLineCount & vbCrLf)
                    FileClose()
                Else

                    FileOpen(fileNumber, logFileName, OpenMode.Append)
                    Print(fileNumber, Date.Now & " | (AM) 채권발행기관코드_거래소(V3) - |" & DataDefineDAO.V3_ORG_SHORT_NM & " | " & DataDefineDAO.V3_ISSUER_ORG_ID & "가 신규 입력 되었습니다 | 라인 : " & inputLineCount & vbCrLf)
                    FileClose()
                End If

            End If

            dicParams.Clear()
            dbTableTableNameData.Clear()

            '--------------------------------------------KOSCOM V3 작업--------------------------------------------

            DataDefineDAO.V3_DATA(0) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(0), dbTableDataEndArray(0))
            DataDefineDAO.V3_DATA(1) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(1), dbTableDataEndArray(1))
            DataDefineDAO.V3_DATA(2) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(2), dbTableDataEndArray(2))
            DataDefineDAO.V3_DATA(3) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(3), dbTableDataEndArray(3))
            DataDefineDAO.V3_DATA(4) = Utilities.CatchCol(oneLineRow, dbTableDataStartArray(4), dbTableDataEndArray(4))


            If frmDataInserter.opt_Day.Checked = True Then
                DataDefineDAO.V3_DATA(5) = "'AM'"
            Else
                DataDefineDAO.V3_DATA(5) = "'PM'"
            End If

            For i = 0 To 5
                If i = 5 Then
                    queryVar = queryVar & DataDefineDAO.V3_DATA(i)
                Else
                    queryVar = queryVar & DataDefineDAO.V3_DATA(i) & ","
                End If
            Next

            colNameVar_V3 = "" 'ISSUE_ORG_ID 값을 빼고 컬럼 값을 넣기 위해 다시 초기화

            'DB에서 가져온 COL_NAME값 변수에 대입
            For index = 1 To dbTableDataColNameArray.Length - 1
                If index = dbTableDataColNameArray.Length - 1 Then
                    colNameVar_V3 = colNameVar_V3 & dbTableDataColNameArray(index)
                Else
                    colNameVar_V3 = colNameVar_V3 & dbTableDataColNameArray(index) & ","
                End If
            Next

            dicParams.Add(":tr", "KOSCOM V3")
            dbTableTableNameData = connectionDB.getData2(DB_Query.TABLE_NAME_Query, dicParams, True, False) ' TRCODE 별 테이블 명 값 가져오기


            dicParams.Add(":v3_Data", queryVar)
            dicParams.Add(":colNameVar_V3_issuer_org_id", dbTableDataColNameArray(0))
            dicParams.Add(":colNameVar_V3", colNameVar_V3)
            dicParams.Add(":tr_tablename", dbTableTableNameData.Rows(0).Item("TABLE_NAME"))

            connectionDB.saveData2(DB_Query.InsertKoscomV3, Nothing, dicParams, False)
            connectionDB.commitTrans()

            If frmDataInserter.optNight.Checked = True Then

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) 채권발행기관코드_거래소_KOSCOM V3 - |" & DataDefineDAO.V3_ORG_SHORT_NM & " | " & DataDefineDAO.V3_ISSUER_ORG_ID & "에 추가 되어습니다. | 라인 : " & inputLineCount & vbCrLf)
                FileClose()
            Else

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) 채권발행기관코드_거래소_KOSCOM V3 - |" & DataDefineDAO.V3_ORG_SHORT_NM & " | " & DataDefineDAO.V3_ISSUER_ORG_ID & "에 추가 되어습니다. | 라인 : " & inputLineCount & vbCrLf)
                FileClose()
            End If

            Exit Function

SKIP:
            If frmDataInserter.optNight.Checked = True Then

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) V3의 고정길이가 맞지 않습니다. | 라인 : " & inputLineCount & vbCrLf)
                FileClose()
            Else

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) V3의 고정길이가 맞지 않습니다. | 라인 : " & inputLineCount & vbCrLf)
                FileClose()
            End If


            Exit Function

        Catch ex As Exception
            If frmDataInserter.optNight.Checked = True Then

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (PM) 채권발행기관코드_거래소(V3) 오류 발생! 행 위치 : " & inputLineCount & "오류 내용 : " & ex.ToString() & vbCrLf)
                FileClose()
            Else

                FileOpen(fileNumber, logFileName, OpenMode.Append)
                Print(fileNumber, Date.Now & " | (AM) 채권발행기관코드_거래소(V3) 오류 발생! 행 위치 : " & inputLineCount & "오류 내용 : " & ex.ToString() & vbCrLf)
                FileClose()
            End If
            connectionDB.rollbackTrans()

            MsgBox("V3 채권발행기관코드_거래소 작업 중 오류 발생! 행 위치 : " & inputLineCount & "오류 내용 : " & ex.ToString())
            Throw ex

        Finally
            connectionDB.endTrans()
        End Try

    End Function

#Region "변수 초기화"
    Public Function ClearData()
        '채권발행기관코드_거래소(V3)
        DataDefineDAO.V3_ISSUER_ORG_ID = ""
        DataDefineDAO.V3_ORG_FULL_NM = ""
        DataDefineDAO.V3_ORG_SHORT_NM = ""
        DataDefineDAO.V3_ORG_ENG_FULL_NM = ""
        DataDefineDAO.V3_ORG_ENG_SHORT_NM = ""


        '대용가(VD)
        DataDefineDAO.VD_BND_ID = ""
        DataDefineDAO.VD_APPLY_DAY = ""
        DataDefineDAO.VD_MKT_ID = ""
        DataDefineDAO.VD_TRD_CRC_ID = ""
        DataDefineDAO.VD_TRD_UNIT = ""
        DataDefineDAO.VD_SHSL_PSB_YN = ""
        DataDefineDAO.VD_CDT_ORD_PSB_YN = ""
        DataDefineDAO.VD_DICR_END_PSB_YN = ""
        DataDefineDAO.VD_MKT_FMTN_PSB_YN = ""
        DataDefineDAO.VD_HSTL_PRC = ""
        DataDefineDAO.VD_LSTL_PRC = ""
        DataDefineDAO.VD_TRD_STP_YN = ""
        DataDefineDAO.VD_ADJS_SB_YN = ""
        DataDefineDAO.VD_LIST_DT = ""
        DataDefineDAO.VD_BND_LIST_ABSH_DT = ""
        DataDefineDAO.VD_CLPRC = ""
        DataDefineDAO.VD_CLPRC_ERNR = ""
        DataDefineDAO.VD_STD_PRC = ""
        DataDefineDAO.VD_SBST_PRC = ""
        DataDefineDAO.VD_BND_LIST_ABSH_CAU_CD = ""
        DataDefineDAO.VD_LIST_AMT = ""
        DataDefineDAO.VD_BPRD_INT_PAYNT_DT = ""
        DataDefineDAO.VD_NXTM_INT_PAYNT_DT = ""
        DataDefineDAO.VD_RTSL_BND_CLSS_CD = ""
        DataDefineDAO.VD_SAMT_SB_KND_CD = ""
        DataDefineDAO.VD_NTBN_KND_CD = ""
        DataDefineDAO.VD_NTBN_EXR_YCNT = ""
        DataDefineDAO.VD_NTBN_STK_TP_CD = ""
        DataDefineDAO.VD_REPO_CLSS_CD = ""
        DataDefineDAO.VD_INVT_ATD_BND_TP_CD = ""

        '회사코드정보(V7)
        DataDefineDAO.V7_ORG_NAME = ""
        DataDefineDAO.V7_ORG_ID = ""
        DataDefineDAO.V7_CP_ORG_ID = ""
        DataDefineDAO.V7_FIN_ORG_ID = ""
        DataDefineDAO.V7_SPC_ORG_ID = ""
        DataDefineDAO.name_tag = ""
        DataDefineDAO.name_len = 0
    End Function
#End Region
End Class
