Public Class DataDefineDAO
    ''' <summary>
    '''  객체지향 언어의 캡슐화를 이용하여 
    '''  
    ''' Proverty 프로시저를 이용하여 get / set으로 데이터 초기화 
    ''' 
    ''' </summary>
#Region "변수 선언"


    '채권발행기관코드_거래소(V3)
    Private _V3_ISSUER_ORG_ID As String
    Private _V3_ORG_FULL_NM As String
    Private _V3_ORG_SHORT_NM As String
    Private _V3_ORG_ENG_FULL_NM As String
    Private _V3_ORG_ENG_SHORT_NM As String

    '회사코드정보(V7)
    Private _temp_name As String
    Private _name_tag As String
    Private _name_len As Integer
    Private _V7_ORG_NAME As String '회사명
    Private _V7_ORG_ID As String 'KOSCOM 회사코드
    Private _V7_CP_ORG_ID As String '회사채고유코드
    Private _V7_FIN_ORG_ID As String '기관코드
    Private _V7_SPC_ORG_ID As String '특수채코드

    '대용가(VD)
    Private _VD_BND_ID As String '종목코드
    Private _VD_APPLY_DAY As String '적용일자
    Private _VD_MKT_ID As String '시장ID
    Private _VD_TRD_CRC_ID As String '거래통화ID
    Private _VD_TRD_UNIT As String '거래단위(999999999.99999999)
    Private _VD_SHSL_PSB_YN As String '공매도가능여부
    Private _VD_CDT_ORD_PSB_YN As String '신용주문가능여부
    Private _VD_DICR_END_PSB_YN As String '임의종료가능여부
    Private _VD_MKT_FMTN_PSB_YN As String '시장조정가능여부
    Private _VD_HSTL_PRC As String '상한가(9999999.999)
    Private _VD_LSTL_PRC As String '하한가(9999999.999)
    Private _VD_TRD_STP_YN As String '거래정지여부
    Private _VD_ADJS_SB_YN As String '정리매매여부
    Private _VD_LIST_DT As String '상장일자
    Private _VD_BND_LIST_ABSH_DT As String '채권상장폐지일자
    Private _VD_CLPRC As String '종가(9999999.999)
    Private _VD_CLPRC_ERNR As String '종가수익률(999999.999999)
    Private _VD_STD_PRC As String '기준가격(9999999.999)
    Private _VD_SBST_PRC As String '대용가격(9999999.999)
    Private _VD_BND_LIST_ABSH_CAU_CD As String '채권상장폐지사유코드
    Private _VD_LIST_AMT As String '상장금액(발행잔액(999999999999999999.999))
    Private _VD_BPRD_INT_PAYNT_DT As String '전기이자지급일자
    Private _VD_NXTM_INT_PAYNT_DT As String '차기이자지급일
    Private _VD_RTSL_BND_CLSS_CD As String '소매채권분류코드
    Private _VD_SAMT_SB_KND_CD As String '소액매매종류코드
    Private _VD_NTBN_KND_CD As String '국채종류코드
    Private _VD_NTBN_EXR_YCNT As String '국채만기연수
    Private _VD_NTBN_STK_TP_CD As String '국채종목구분코드
    Private _VD_REPO_CLSS_CD As String 'REPO 분류코드
    Private _VD_INVT_ATD_BND_TP_CD As String  '2016-11-25 투자유의채권구분코드 추가(투자유의채권구분코드|0: 해당없음 1: 지정예고 2: 지정)
    Private _VD_DIV_REPAY_DT As String  '2019-03-19 이건민 분할상환일자 


    'A0027 : 채권소매시장 종목배치
    Private _A0027_BOND_ID As String  '종목코드
    Private _A0027_SEQ As String '일련번호
    Private _A0027_DAY As String '적용일
    Private _A0027_RETAIL_BOND_TYPE_CODE As String '소매채권분류코드
    Private _A0027_KOR_BOND_NM As String '한글종목명
    Private _A0027_ENG_BOND_NM As String '영문종목약명
    Private _A0027_LIST_TYPE_CODE As String '채권상장구분코드
    Private _A0027_BOND_ANCD_CODE As String '채권분류코드
    Private _A0027_GUARANTEE_TYPE_CODE As String '채권보증구분코드
    Private _A0027_INT_PAY_TYPE_CODE As String '이자지급방법코드
    Private _A0027_LIST_DAY As String '상장일
    Private _A0027_ISSUE_DAY As String '발행일
    Private _A0027_MAT_DAY As String '상환일
    Private _A0027_SELL_DAY As String '매출일
    Private _A0027_ISSUE_AMT As String '채권발행율(99999V999999)
    Private _A0027_COUPON_RATE As String '표면이자율(9999999V99999)
    Private _A0027_INT_PAY_MONTH_CALC As String '이자지급계산월수
    Private _A0027_INT_PAY_TIME_TYPE_CODE As String '이표지급방법코드
    Private _A0027_INT_PAY_TERM_TYPE_CODE As String '채권이자지급일기준구분코드
    Private _A0027_INT_END_MONTH_TYPE As String '이자월말구분코드
    Private _A0027_INT_PAY_UNIT_TYPE As String '이자원단위미만처리코드
    Private _A0027_PRE_SELL_INT_TYPE_CODE As String '채권선매출이자지급방법코드
    Private _A0027_PRCP_AMT As String '발행금액(999999999999999999)
    Private _A0027_LIST_AMT As String '상장금액(999999999999999999)
    Private _A0027_CB_PRCP_RETURN_RATE As String '만기상환비율(99999V999999)
    Private _A0027_INSTALLMENT_RETURN_TYPE As String '분할상환유형구분코드
    Private _A0027_UNREDEEM_TERM As String '거치개월수
    Private _A0027_REFUND_TERM_TIMES As String '분할상환횟수
    Private _A0027_DEAL_STOP_TYPE As String '거래정지여부
    Private _A0027_PREV_INT_PAY_DAY As String '전기이자지급일자
    Private _A0027_NEXT_INT_PAY_DAY As String '차기이자지급일자
    Private _A0027_HYBRID_BOND_TYPE As String '영구채권만기구조여부
    Private _A0027_STRIPS_TAG As String '채권스트립구분코드
    Private _A0027_OVER_ASK_STANDARD_PRICE As String '기준가격
    Private _A0027_STLMT_TRAD_OBJT_TAG As String '정리매매여부
    Private _A0027_INPUT_DAY As String '입력일
    Private _A0027_INVT_ATD_BND_TP_CD As String '2016-11-25 주정석 투자유의채권구분코드(해당없음 1: 지정예고 2: 지정)
    '---------------------------------------------------------------------------------------------------------------
    'A001B : 채권소매시장 종목배치
    Private _A001B_BOND_ID As String  '종목코드
    Private _A001B_SEQ As String '일련번호
    Private _A001B_DAY As String '적용일
    Private _A001B_RETAIL_BOND_TYPE_CODE As String '소매채권분류코드
    Private _A001B_KOR_BOND_NM As String '한글종목명
    Private _A001B_ENG_BOND_NM As String '영문종목약명
    Private _A001B_LIST_TYPE_CODE As String '채권상장구분코드
    Private _A001B_BOND_ANCD_CODE As String '채권분류코드
    Private _A001B_GUARANTEE_TYPE_CODE As String '채권보증구분코드
    Private _A001B_INT_PAY_TYPE_CODE As String '이자지급방법코드
    Private _A001B_LIST_DAY As String '상장일
    Private _A001B_ISSUE_DAY As String '발행일
    Private _A001B_MAT_DAY As String '상환일
    Private _A001B_SELL_DAY As String '매출일
    Private _A001B_ISSUE_AMT As String '채권발행율(99999V999999)
    Private _A001B_COUPON_RATE As String '표면이자율(9999999V99999)
    Private _A001B_INT_PAY_MONTH_CALC As String '이자지급계산월수
    Private _A001B_INT_PAY_TIME_TYPE_CODE As String '이표지급방법코드
    Private _A001B_INT_PAY_TERM_TYPE_CODE As String '채권이자지급일기준구분코드
    Private _A001B_INT_END_MONTH_TYPE As String '이자월말구분코드
    Private _A001B_INT_PAY_UNIT_TYPE As String '이자원단위미만처리코드
    Private _A001B_PRE_SELL_INT_TYPE_CODE As String '채권선매출이자지급방법코드
    Private _A001B_PRCP_AMT As String '발행금액(999999999999999999)
    Private _A001B_LIST_AMT As String '상장금액(999999999999999999)
    Private _A001B_CB_PRCP_RETURN_RATE As String '만기상환비율(99999V999999)
    Private _A001B_INSTALLMENT_RETURN_TYPE As String '분할상환유형구분코드
    Private _A001B_UNREDEEM_TERM As String '거치개월수
    Private _A001B_REFUND_TERM_TIMES As String '분할상환횟수
    Private _A001B_DEAL_STOP_TYPE As String '거래정지여부
    Private _A001B_PREV_INT_PAY_DAY As String '전기이자지급일자
    Private _A001B_NEXT_INT_PAY_DAY As String '차기이자지급일자
    Private _A001B_HYBRID_BOND_TYPE As String '영구채권만기구조여부
    Private _A001B_STRIPS_TAG As String '채권스트립구분코드
    Private _A001B_OVER_ASK_STANDARD_PRICE As String '기준가격
    Private _A001B_STLMT_TRAD_OBJT_TAG As String '정리매매여부
    Private _A001B_INPUT_DAY As String '입력일
    Private _A001B_INVT_ATD_BND_TP_CD As String '2016-11-25 주정석 투자유의채권구분코드(해당없음 1: 지정예고 2: 지정)
    Private _A001B_BOND_CNT As String '정보분배총종목인덱스 (종목인덱스 중 마지막 종목의 값)
    Private _A001B_BOND_SEQ As String '정보분배종목인덱스 (당일 종목 식별용으로 부여되는 일련번호)
    Private _A001B_TSC_ID As String '장운영상품그룹ID (동일한 장운영(TSC, Trading Schedule Control) 대상이 되는 상품들의 집합을 식별하기 위한 ID)


    'G300B : 채권소매시장 소매종류코드
    Dim _G300B_SEQ As String '일련번호
    Dim _G300B_RETAIL_BOND_TYPE_CODE As String '소매채권분류코드
    Dim _G300B_KOR_TYPE_NAME As String '소매종목분류한글명
    Dim _G300B_ENG_TYPE_NAME As String '소매종목분류영문명
    Dim _G300B_ASK_SUBMIT_TYPE As String '소매채권조성호가가능여부
    Dim _G300B_INPUT_DAY As String '입력일'
    '---------------------------------------------------------------
    'G300B : 채권소매시장 소매종류코드
    Dim _G3027_SEQ As String '일련번호
    Dim _G3027_RETAIL_BOND_TYPE_CODE As String '소매채권분류코드
    Dim _G3027_KOR_TYPE_NAME As String '소매종목분류한글명
    Dim _G3027_ENG_TYPE_NAME As String '소매종목분류영문명
    Dim _G3027_ASK_SUBMIT_TYPE As String '소매채권조성호가가능여부
    Dim _G3027_INPUT_DAY As String '입력일'

    '(V2) ->>> 변수 초기화가 쉽지 않기 때문에 Public 으로 진행
    Public V2_DATA(129) As String

    '(V9) ->> 배열 변수 초기화
    Public V9_DATA(4) As String

    '(V8) ->> 배열 변수 초기화
    Public V8_DATA(3) As String

    '(V3) ->> 배열 변수 초기화
    Public V3_DATA(6) As String

    '(V7) ->> 배열 변수 초기화
    Public V7_DATA(6) As String


    '기타 변수 선언
    Private _locFile As String ' 반환되는 파일 값는 변수

    'FuncKoscomSrcFile 메소드
    Private _loc_trgfile As String
    Private _all_loc_trgfile As String
    Private _v_loc_trgfile As String
    Private _am_loc_trgfile As String
    Private _am_loc_temp_trgfile As String
    Private _pm_loc_trgfile As String
    Private _pm_loc_temp_trgfile As String
    Private _am_vd_loc_trgfile As String
    Private _am_vd_loc_temp_trgfile As String
    Private _temp_loc_trgfile As String
    Private _Win_loc_trgfile As String
    Private _temp_length As Integer
    Private _tempLine As String
    Private _index As Double
    Private _trCode As String
    Private _am_end_trgfile As String
    Private _am_end_temp_trgfile As String
    Private _pm_end_trgfile As String
    Private _pm_end_temp_trgfile As String
    Private _am_loc_trgfile_beforeTDay As String
    Private _am_loc_temp_trgfile_beforeTDay As String
    Private _am_vd_loc_trgfile_beforeTDay As String
    Private _am_vd_loc_temp_trgfile_beforeTDay As String
    Private _am_end_trgfile_beforeTDay As String
    Private _am_end_temp_trgfile_beforeTDay As String
    Private _loc_A0027_trgFile As String
    Private _loc_G3027_trgFile As String
    Private _loc_A0027_temp_trgFile As String
    Private _loc_G3027_temp_trgFile As String
    Private _loc_A001B_trgFile As String
    Private _loc_G300B_trgFile As String
    Private _loc_A001B_temp_trgFile As String
    Private _loc_G300B_temp_trgFile As String
    Private _all_loc_temp_trgfile As String

    'DataManagementSystemMethod 메소드
    Private _newData As Integer ' 신규 데이터
    Private _checkLenLine As Integer ' 데이터 한 줄의 길이 확인
    Private _InputLineCount As Integer '입력라인수
    Private _progressLineCount As Integer ' 진행라인수

    'KOSCOM HISTORY INSERT에 필요한 변수
    Private _i_bond_id As String
    Private _i_bond_type_code As String              '채권분류코드
    Private _i_local_type_code As String              '지방채구분코드   
    Private _i_abs_type_code As String              '자산유동화구분코드    
    Private _i_first_coupon_day As String                    '최초이자지급일자 (DATE)
    Private _i_batch_amt_confirm_gb As String      '일괄금액확정여부    
    Private _i_named_gb As String                   '기명여부    
    Private _i_admin_org_id As String               '채권주관회사코드 
    Private _i_guarant_org_id As String '지급보증기관코드    
    Private _i_custody_org_id As String '수탁기관코드
    Private _i_registration_org_id As String '등록기관코드        
    Private _i_prcp_agency_org_id As String '원리금지급대행기관코드     
    Private _i_int_calc_type_code As String '채권단수일이자기준구분코드
    Private _i_int_accr_pay_hldy_type_code As String '은행휴무일이자지급방법코드
    Private _i_int_accr_hldy_type_code As String '은행휴무일이자기준금리코드
    Private _i_int_accr_rate As String                     '은행휴무일이자경과이자율 (NUMBER)
    Private _i_prcp_accr_pay_hldy_type_code As String '은행휴무일원금지급방법코드
    Private _i_prcp_accr_hldy_type_code As String  '은행휴무일원금기준금리코드
    Private _i_prcp_accr_rate As String                  '은행휴무일원금경과이자율 (NUMBER)
    Private _i_issuer_org_id As String               '발행기관코드

#End Region


#Region "채권발행기관코드_거래소(V3)"
    Property V3_ISSUER_ORG_ID As String
        Get
            Return Me._V3_ISSUER_ORG_ID
        End Get
        Set(value As String)
            Me._V3_ISSUER_ORG_ID = value
        End Set
    End Property

    Property V3_ORG_FULL_NM As String
        Get
            Return Me._V3_ORG_FULL_NM
        End Get
        Set(value As String)
            Me._V3_ORG_FULL_NM = value
        End Set
    End Property

    Property V3_ORG_SHORT_NM As String
        Get
            Return Me._V3_ORG_SHORT_NM
        End Get
        Set(value As String)
            Me._V3_ORG_SHORT_NM = value
        End Set
    End Property

    Property V3_ORG_ENG_FULL_NM As String
        Get
            Return Me._V3_ORG_ENG_FULL_NM
        End Get
        Set(value As String)
            Me._V3_ORG_ENG_FULL_NM = value
        End Set
    End Property

    Property V3_ORG_ENG_SHORT_NM As String
        Get
            Return Me._V3_ORG_ENG_SHORT_NM
        End Get
        Set(value As String)
            Me._V3_ORG_ENG_SHORT_NM = value
        End Set
    End Property


#End Region

#Region "회사코드정보(V7)"
    Property temp_name As String
        Get
            Return Me._temp_name
        End Get
        Set(value As String)
            Me._temp_name = value
        End Set
    End Property

    Property name_tag As String
        Get
            Return Me._name_tag
        End Get
        Set(value As String)
            Me._name_tag = value
        End Set
    End Property

    Property name_len As Integer
        Get
            Return Me._name_len
        End Get
        Set(value As Integer)
            Me._name_len = value
        End Set
    End Property

    Property V7_ORG_NAME As String
        Get
            Return Me._V7_ORG_NAME
        End Get
        Set(value As String)
            Me._V7_ORG_NAME = value
        End Set
    End Property

    Property V7_ORG_ID As String
        Get
            Return Me._V7_ORG_ID
        End Get
        Set(value As String)
            Me._V7_ORG_ID = value
        End Set
    End Property

    Property V7_CP_ORG_ID As String
        Get
            Return Me._V7_CP_ORG_ID
        End Get
        Set(value As String)
            Me._V7_CP_ORG_ID = value
        End Set
    End Property

    Property V7_FIN_ORG_ID As String
        Get
            Return Me._V7_FIN_ORG_ID
        End Get
        Set(value As String)
            Me._V7_FIN_ORG_ID = value
        End Set
    End Property

    Property V7_SPC_ORG_ID As String
        Get
            Return Me._V7_SPC_ORG_ID
        End Get
        Set(value As String)
            Me._V7_SPC_ORG_ID = value
        End Set
    End Property

#End Region

#Region "대용가(VD)"

    Property VD_BND_ID As String
        Get
            Return Me._VD_BND_ID
        End Get
        Set(value As String)
            Me._VD_BND_ID = value
        End Set
    End Property

    Property VD_APPLY_DAY As String
        Get
            Return Me._VD_APPLY_DAY
        End Get
        Set(value As String)
            Me._VD_APPLY_DAY = value
        End Set
    End Property

    Property VD_MKT_ID As String
        Get
            Return Me._VD_MKT_ID
        End Get
        Set(value As String)
            Me._VD_MKT_ID = value
        End Set
    End Property

    Property VD_TRD_CRC_ID As String
        Get
            Return Me._VD_TRD_CRC_ID
        End Get
        Set(value As String)
            Me._VD_TRD_CRC_ID = value
        End Set
    End Property

    Property VD_TRD_UNIT As String
        Get
            Return Me._VD_TRD_UNIT
        End Get
        Set(value As String)
            Me._VD_TRD_UNIT = value
        End Set
    End Property

    Property VD_SHSL_PSB_YN As String
        Get
            Return Me._VD_SHSL_PSB_YN
        End Get
        Set(value As String)
            Me._VD_SHSL_PSB_YN = value
        End Set
    End Property

    Property VD_CDT_ORD_PSB_YN As String
        Get
            Return Me._VD_CDT_ORD_PSB_YN
        End Get
        Set(value As String)
            Me._VD_CDT_ORD_PSB_YN = value
        End Set
    End Property

    Property VD_DICR_END_PSB_YN As String
        Get
            Return Me._VD_DICR_END_PSB_YN
        End Get
        Set(value As String)
            Me._VD_DICR_END_PSB_YN = value
        End Set
    End Property

    Property VD_MKT_FMTN_PSB_YN As String
        Get
            Return Me._VD_MKT_FMTN_PSB_YN
        End Get
        Set(value As String)
            Me._VD_MKT_FMTN_PSB_YN = value
        End Set
    End Property

    Property VD_HSTL_PRC As String
        Get
            Return Me._VD_HSTL_PRC
        End Get
        Set(value As String)
            Me._VD_HSTL_PRC = value
        End Set
    End Property

    Property VD_LSTL_PRC As String
        Get
            Return Me._VD_LSTL_PRC
        End Get
        Set(value As String)
            Me._VD_LSTL_PRC = value
        End Set
    End Property

    Property VD_TRD_STP_YN As String
        Get
            Return Me._VD_TRD_STP_YN
        End Get
        Set(value As String)
            Me._VD_TRD_STP_YN = value
        End Set
    End Property

    Property VD_ADJS_SB_YN As String
        Get
            Return Me._VD_ADJS_SB_YN
        End Get
        Set(value As String)
            Me._VD_ADJS_SB_YN = value
        End Set
    End Property

    Property VD_LIST_DT As String
        Get
            Return Me._VD_LIST_DT
        End Get
        Set(value As String)
            Me._VD_LIST_DT = value
        End Set
    End Property

    Property VD_BND_LIST_ABSH_DT As String
        Get
            Return Me._VD_BND_LIST_ABSH_DT
        End Get
        Set(value As String)
            Me._VD_BND_LIST_ABSH_DT = value
        End Set
    End Property

    Property VD_CLPRC As String
        Get
            Return Me._VD_CLPRC
        End Get
        Set(value As String)
            Me._VD_CLPRC = value
        End Set
    End Property

    Property VD_CLPRC_ERNR As String
        Get
            Return Me._VD_CLPRC_ERNR
        End Get
        Set(value As String)
            Me._VD_CLPRC_ERNR = value
        End Set
    End Property

    Property VD_STD_PRC As String
        Get
            Return Me._VD_STD_PRC
        End Get
        Set(value As String)
            Me._VD_STD_PRC = value
        End Set
    End Property

    Property VD_SBST_PRC As String
        Get
            Return Me._VD_SBST_PRC
        End Get
        Set(value As String)
            Me._VD_SBST_PRC = value
        End Set
    End Property

    Property VD_BND_LIST_ABSH_CAU_CD As String
        Get
            Return Me._VD_BND_LIST_ABSH_CAU_CD
        End Get
        Set(value As String)
            Me._VD_BND_LIST_ABSH_CAU_CD = value
        End Set
    End Property

    Property VD_LIST_AMT As String
        Get
            Return Me._VD_LIST_AMT
        End Get
        Set(value As String)
            Me._VD_LIST_AMT = value
        End Set
    End Property

    Property VD_BPRD_INT_PAYNT_DT As String
        Get
            Return Me._VD_BPRD_INT_PAYNT_DT
        End Get
        Set(value As String)
            Me._VD_BPRD_INT_PAYNT_DT = value
        End Set
    End Property

    Property VD_NXTM_INT_PAYNT_DT As String
        Get
            Return Me._VD_NXTM_INT_PAYNT_DT
        End Get
        Set(value As String)
            Me._VD_NXTM_INT_PAYNT_DT = value
        End Set
    End Property

    Property VD_RTSL_BND_CLSS_CD As String
        Get
            Return Me._VD_RTSL_BND_CLSS_CD
        End Get
        Set(value As String)
            Me._VD_RTSL_BND_CLSS_CD = value
        End Set
    End Property

    Property VD_SAMT_SB_KND_CD As String
        Get
            Return Me._VD_SAMT_SB_KND_CD
        End Get
        Set(value As String)
            Me._VD_SAMT_SB_KND_CD = value
        End Set
    End Property

    Property VD_NTBN_KND_CD As String
        Get
            Return Me._VD_NTBN_KND_CD
        End Get
        Set(value As String)
            Me._VD_NTBN_KND_CD = value
        End Set
    End Property

    Property VD_NTBN_EXR_YCNT As String
        Get
            Return Me._VD_NTBN_EXR_YCNT
        End Get
        Set(value As String)
            Me._VD_NTBN_EXR_YCNT = value
        End Set
    End Property

    Property VD_NTBN_STK_TP_CD As String
        Get
            Return Me._VD_NTBN_STK_TP_CD
        End Get
        Set(value As String)
            Me._VD_NTBN_STK_TP_CD = value
        End Set
    End Property

    Property VD_REPO_CLSS_CD As String
        Get
            Return Me._VD_REPO_CLSS_CD
        End Get
        Set(value As String)
            Me._VD_REPO_CLSS_CD = value
        End Set
    End Property

    Property VD_INVT_ATD_BND_TP_CD As String
        Get
            Return Me._VD_INVT_ATD_BND_TP_CD
        End Get
        Set(value As String)
            Me._VD_INVT_ATD_BND_TP_CD = value
        End Set
    End Property

    Property VD_DIV_REPAY_DT As String
        Get
            Return Me._VD_DIV_REPAY_DT
        End Get
        Set(value As String)
            Me._VD_DIV_REPAY_DT = value
        End Set
    End Property

#End Region

#Region "채권소매시장 A0027"
    Property A001B_BOND_ID As String
        Get
            Return Me._A001B_BOND_ID
        End Get
        Set(value As String)
            Me._A001B_BOND_ID = value
        End Set
    End Property
    Property A001B_SEQ As String
        Get
            Return Me._A001B_SEQ
        End Get
        Set(value As String)
            Me._A001B_SEQ = value
        End Set
    End Property
    Property A001B_DAY As String
        Get
            Return Me._A001B_DAY
        End Get
        Set(value As String)
            Me._A001B_DAY = value
        End Set
    End Property
    Property A001B_RETAIL_BOND_TYPE_CODE As String
        Get
            Return Me._A001B_RETAIL_BOND_TYPE_CODE
        End Get
        Set(value As String)
            Me._A001B_RETAIL_BOND_TYPE_CODE = value
        End Set
    End Property
    Property A001B_KOR_BOND_NM As String
        Get
            Return Me._A001B_KOR_BOND_NM
        End Get
        Set(value As String)
            Me._A001B_KOR_BOND_NM = value
        End Set
    End Property
    Property A001B_ENG_BOND_NM As String
        Get
            Return Me._A001B_ENG_BOND_NM
        End Get
        Set(value As String)
            Me._A001B_ENG_BOND_NM = value
        End Set
    End Property
    Property A001B_LIST_TYPE_CODE As String
        Get
            Return Me._A001B_LIST_TYPE_CODE
        End Get
        Set(value As String)
            Me._A001B_LIST_TYPE_CODE = value
        End Set
    End Property
    Property A001B_BOND_ANCD_CODE As String
        Get
            Return Me._A001B_BOND_ANCD_CODE
        End Get
        Set(value As String)
            Me._A001B_BOND_ANCD_CODE = value
        End Set
    End Property
    Property A001B_GUARANTEE_TYPE_CODE As String
        Get
            Return Me._A001B_GUARANTEE_TYPE_CODE
        End Get
        Set(value As String)
            Me._A001B_GUARANTEE_TYPE_CODE = value
        End Set
    End Property
    Property A001B_INT_PAY_TYPE_CODE As String
        Get
            Return Me._A001B_INT_PAY_TYPE_CODE
        End Get
        Set(value As String)
            Me._A001B_INT_PAY_TYPE_CODE = value
        End Set
    End Property
    Property A001B_LIST_DAY As String
        Get
            Return Me._A001B_LIST_DAY
        End Get
        Set(value As String)
            Me._A001B_LIST_DAY = value
        End Set
    End Property
    Property A001B_ISSUE_DAY As String
        Get
            Return Me._A001B_ISSUE_DAY
        End Get
        Set(value As String)
            Me._A001B_ISSUE_DAY = value
        End Set
    End Property
    Property A001B_MAT_DAY As String
        Get
            Return Me._A001B_MAT_DAY
        End Get
        Set(value As String)
            Me._A001B_MAT_DAY = value
        End Set
    End Property
    Property A001B_SELL_DAY As String
        Get
            Return Me._A001B_SELL_DAY
        End Get
        Set(value As String)
            Me._A001B_SELL_DAY = value
        End Set
    End Property
    Property A001B_ISSUE_AMT As String
        Get
            Return Me._A001B_ISSUE_AMT
        End Get
        Set(value As String)
            Me._A001B_ISSUE_AMT = value
        End Set
    End Property
    Property A001B_COUPON_RATE As String
        Get
            Return Me._A001B_COUPON_RATE
        End Get
        Set(value As String)
            Me._A001B_COUPON_RATE = value
        End Set
    End Property
    Property A001B_INT_PAY_MONTH_CALC As String
        Get
            Return Me._A001B_INT_PAY_MONTH_CALC
        End Get
        Set(value As String)
            Me._A001B_INT_PAY_MONTH_CALC = value
        End Set
    End Property
    Property A001B_INT_PAY_TIME_TYPE_CODE As String
        Get
            Return Me._A001B_INT_PAY_TIME_TYPE_CODE
        End Get
        Set(value As String)
            Me._A001B_INT_PAY_TIME_TYPE_CODE = value
        End Set
    End Property
    Property A001B_INT_PAY_TERM_TYPE_CODE As String
        Get
            Return Me._A001B_INT_PAY_TERM_TYPE_CODE
        End Get
        Set(value As String)
            Me._A001B_INT_PAY_TERM_TYPE_CODE = value
        End Set
    End Property
    Property A001B_INT_END_MONTH_TYPE As String
        Get
            Return Me._A001B_INT_END_MONTH_TYPE
        End Get
        Set(value As String)
            Me._A001B_INT_END_MONTH_TYPE = value
        End Set
    End Property
    Property A001B_INT_PAY_UNIT_TYPE As String
        Get
            Return Me._A001B_INT_PAY_UNIT_TYPE
        End Get
        Set(value As String)
            Me._A001B_INT_PAY_UNIT_TYPE = value
        End Set
    End Property
    Property A001B_PRE_SELL_INT_TYPE_CODE As String
        Get
            Return Me._A001B_PRE_SELL_INT_TYPE_CODE
        End Get
        Set(value As String)
            Me._A001B_PRE_SELL_INT_TYPE_CODE = value
        End Set
    End Property
    Property A001B_PRCP_AMT As String
        Get
            Return Me._A001B_PRCP_AMT
        End Get
        Set(value As String)
            Me._A001B_PRCP_AMT = value
        End Set
    End Property
    Property A001B_LIST_AMT As String
        Get
            Return Me._A001B_LIST_AMT
        End Get
        Set(value As String)
            Me._A001B_LIST_AMT = value
        End Set
    End Property
    Property A001B_CB_PRCP_RETURN_RATE As String
        Get
            Return Me._A001B_CB_PRCP_RETURN_RATE
        End Get
        Set(value As String)
            Me._A001B_CB_PRCP_RETURN_RATE = value
        End Set
    End Property
    Property A001B_INSTALLMENT_RETURN_TYPE As String
        Get
            Return Me._A001B_INSTALLMENT_RETURN_TYPE
        End Get
        Set(value As String)
            Me._A001B_INSTALLMENT_RETURN_TYPE = value
        End Set
    End Property
    Property A001B_UNREDEEM_TERM As String
        Get
            Return Me._A001B_UNREDEEM_TERM
        End Get
        Set(value As String)
            Me._A001B_UNREDEEM_TERM = value
        End Set
    End Property
    Property A001B_REFUND_TERM_TIMES As String
        Get
            Return Me._A001B_REFUND_TERM_TIMES
        End Get
        Set(value As String)
            Me._A001B_REFUND_TERM_TIMES = value
        End Set
    End Property
    Property A001B_DEAL_STOP_TYPE As String
        Get
            Return Me._A001B_DEAL_STOP_TYPE
        End Get
        Set(value As String)
            Me._A001B_DEAL_STOP_TYPE = value
        End Set
    End Property
    Property A001B_PREV_INT_PAY_DAY As String
        Get
            Return Me._A001B_PREV_INT_PAY_DAY
        End Get
        Set(value As String)
            Me._A001B_PREV_INT_PAY_DAY = value
        End Set
    End Property
    Property A001B_NEXT_INT_PAY_DAY As String
        Get
            Return Me._A001B_NEXT_INT_PAY_DAY
        End Get
        Set(value As String)
            Me._A001B_NEXT_INT_PAY_DAY = value
        End Set
    End Property
    Property A001B_HYBRID_BOND_TYPE As String
        Get
            Return Me._A001B_HYBRID_BOND_TYPE
        End Get
        Set(value As String)
            Me._A001B_HYBRID_BOND_TYPE = value
        End Set
    End Property
    Property A001B_STRIPS_TAG As String
        Get
            Return Me._A001B_STRIPS_TAG
        End Get
        Set(value As String)
            Me._A001B_STRIPS_TAG = value
        End Set
    End Property
    Property A001B_OVER_ASK_STANDARD_PRICE As String
        Get
            Return Me._A001B_OVER_ASK_STANDARD_PRICE
        End Get
        Set(value As String)
            Me._A001B_OVER_ASK_STANDARD_PRICE = value
        End Set
    End Property
    Property A001B_STLMT_TRAD_OBJT_TAG As String
        Get
            Return Me._A001B_STLMT_TRAD_OBJT_TAG
        End Get
        Set(value As String)
            Me._A001B_STLMT_TRAD_OBJT_TAG = value
        End Set
    End Property
    Property A001B_INPUT_DAY As String
        Get
            Return Me._A001B_INPUT_DAY
        End Get
        Set(value As String)
            Me._A001B_INPUT_DAY = value
        End Set
    End Property
    Property A001B_INVT_ATD_BND_TP_CD As String
        Get
            Return Me._A001B_INVT_ATD_BND_TP_CD
        End Get
        Set(value As String)
            Me._A001B_INVT_ATD_BND_TP_CD = value
        End Set
    End Property
    Property A001B_BOND_CNT As String
        Get
            Return Me._A001B_BOND_CNT
        End Get
        Set(value As String)
            Me._A001B_BOND_CNT = value
        End Set
    End Property
    Property A001B_BOND_SEQ As String
        Get
            Return Me._A001B_BOND_SEQ
        End Get
        Set(value As String)
            Me._A001B_BOND_SEQ = value
        End Set
    End Property
    Property A001B_TSC_ID As String
        Get
            Return Me._A001B_TSC_ID
        End Get
        Set(value As String)
            Me._A001B_TSC_ID = value
        End Set
    End Property
    ' 소매채권입력 AS-IS 
    Property A0027_BOND_ID As String
        Get
            Return Me._A0027_BOND_ID
        End Get
        Set(value As String)
            Me._A0027_BOND_ID = value
        End Set
    End Property
    Property A0027_SEQ As String
        Get
            Return Me._A0027_SEQ
        End Get
        Set(value As String)
            Me._A0027_SEQ = value
        End Set
    End Property
    Property A0027_DAY As String
        Get
            Return Me._A0027_DAY
        End Get
        Set(value As String)
            Me._A0027_DAY = value
        End Set
    End Property
    Property A0027_RETAIL_BOND_TYPE_CODE As String
        Get
            Return Me._A0027_RETAIL_BOND_TYPE_CODE
        End Get
        Set(value As String)
            Me._A0027_RETAIL_BOND_TYPE_CODE = value
        End Set
    End Property
    Property A0027_KOR_BOND_NM As String
        Get
            Return Me._A0027_KOR_BOND_NM
        End Get
        Set(value As String)
            Me._A0027_KOR_BOND_NM = value
        End Set
    End Property
    Property A0027_ENG_BOND_NM As String
        Get
            Return Me._A0027_ENG_BOND_NM
        End Get
        Set(value As String)
            Me._A0027_ENG_BOND_NM = value
        End Set
    End Property
    Property A0027_LIST_TYPE_CODE As String
        Get
            Return Me._A0027_LIST_TYPE_CODE
        End Get
        Set(value As String)
            Me._A0027_LIST_TYPE_CODE = value
        End Set
    End Property
    Property A0027_BOND_ANCD_CODE As String
        Get
            Return Me._A0027_BOND_ANCD_CODE
        End Get
        Set(value As String)
            Me._A0027_BOND_ANCD_CODE = value
        End Set
    End Property
    Property A0027_GUARANTEE_TYPE_CODE As String
        Get
            Return Me._A0027_GUARANTEE_TYPE_CODE
        End Get
        Set(value As String)
            Me._A0027_GUARANTEE_TYPE_CODE = value
        End Set
    End Property
    Property A0027_INT_PAY_TYPE_CODE As String
        Get
            Return Me._A0027_INT_PAY_TYPE_CODE
        End Get
        Set(value As String)
            Me._A0027_INT_PAY_TYPE_CODE = value
        End Set
    End Property
    Property A0027_LIST_DAY As String
        Get
            Return Me._A0027_LIST_DAY
        End Get
        Set(value As String)
            Me._A0027_LIST_DAY = value
        End Set
    End Property
    Property A0027_ISSUE_DAY As String
        Get
            Return Me._A0027_ISSUE_DAY
        End Get
        Set(value As String)
            Me._A0027_ISSUE_DAY = value
        End Set
    End Property
    Property A0027_MAT_DAY As String
        Get
            Return Me._A0027_MAT_DAY
        End Get
        Set(value As String)
            Me._A0027_MAT_DAY = value
        End Set
    End Property
    Property A0027_SELL_DAY As String
        Get
            Return Me._A0027_SELL_DAY
        End Get
        Set(value As String)
            Me._A0027_SELL_DAY = value
        End Set
    End Property
    Property A0027_ISSUE_AMT As String
        Get
            Return Me._A0027_ISSUE_AMT
        End Get
        Set(value As String)
            Me._A0027_ISSUE_AMT = value
        End Set
    End Property
    Property A0027_COUPON_RATE As String
        Get
            Return Me._A0027_COUPON_RATE
        End Get
        Set(value As String)
            Me._A0027_COUPON_RATE = value
        End Set
    End Property
    Property A0027_INT_PAY_MONTH_CALC As String
        Get
            Return Me._A0027_INT_PAY_MONTH_CALC
        End Get
        Set(value As String)
            Me._A0027_INT_PAY_MONTH_CALC = value
        End Set
    End Property
    Property A0027_INT_PAY_TIME_TYPE_CODE As String
        Get
            Return Me._A0027_INT_PAY_TIME_TYPE_CODE
        End Get
        Set(value As String)
            Me._A0027_INT_PAY_TIME_TYPE_CODE = value
        End Set
    End Property
    Property A0027_INT_PAY_TERM_TYPE_CODE As String
        Get
            Return Me._A0027_INT_PAY_TERM_TYPE_CODE
        End Get
        Set(value As String)
            Me._A0027_INT_PAY_TERM_TYPE_CODE = value
        End Set
    End Property
    Property A0027_INT_END_MONTH_TYPE As String
        Get
            Return Me._A0027_INT_END_MONTH_TYPE
        End Get
        Set(value As String)
            Me._A0027_INT_END_MONTH_TYPE = value
        End Set
    End Property
    Property A0027_INT_PAY_UNIT_TYPE As String
        Get
            Return Me._A0027_INT_PAY_UNIT_TYPE
        End Get
        Set(value As String)
            Me._A0027_INT_PAY_UNIT_TYPE = value
        End Set
    End Property
    Property A0027_PRE_SELL_INT_TYPE_CODE As String
        Get
            Return Me._A0027_PRE_SELL_INT_TYPE_CODE
        End Get
        Set(value As String)
            Me._A0027_PRE_SELL_INT_TYPE_CODE = value
        End Set
    End Property
    Property A0027_PRCP_AMT As String
        Get
            Return Me._A0027_PRCP_AMT
        End Get
        Set(value As String)
            Me._A0027_PRCP_AMT = value
        End Set
    End Property
    Property A0027_LIST_AMT As String
        Get
            Return Me._A0027_LIST_AMT
        End Get
        Set(value As String)
            Me._A0027_LIST_AMT = value
        End Set
    End Property
    Property A0027_CB_PRCP_RETURN_RATE As String
        Get
            Return Me._A0027_CB_PRCP_RETURN_RATE
        End Get
        Set(value As String)
            Me._A0027_CB_PRCP_RETURN_RATE = value
        End Set
    End Property
    Property A0027_INSTALLMENT_RETURN_TYPE As String
        Get
            Return Me._A0027_INSTALLMENT_RETURN_TYPE
        End Get
        Set(value As String)
            Me._A0027_INSTALLMENT_RETURN_TYPE = value
        End Set
    End Property
    Property A0027_UNREDEEM_TERM As String
        Get
            Return Me._A0027_UNREDEEM_TERM
        End Get
        Set(value As String)
            Me._A0027_UNREDEEM_TERM = value
        End Set
    End Property
    Property A0027_REFUND_TERM_TIMES As String
        Get
            Return Me._A0027_REFUND_TERM_TIMES
        End Get
        Set(value As String)
            Me._A0027_REFUND_TERM_TIMES = value
        End Set
    End Property
    Property A0027_DEAL_STOP_TYPE As String
        Get
            Return Me._A0027_DEAL_STOP_TYPE
        End Get
        Set(value As String)
            Me._A0027_DEAL_STOP_TYPE = value
        End Set
    End Property
    Property A0027_PREV_INT_PAY_DAY As String
        Get
            Return Me._A0027_PREV_INT_PAY_DAY
        End Get
        Set(value As String)
            Me._A0027_PREV_INT_PAY_DAY = value
        End Set
    End Property
    Property A0027_NEXT_INT_PAY_DAY As String
        Get
            Return Me._A0027_NEXT_INT_PAY_DAY
        End Get
        Set(value As String)
            Me._A0027_NEXT_INT_PAY_DAY = value
        End Set
    End Property
    Property A0027_HYBRID_BOND_TYPE As String
        Get
            Return Me._A0027_HYBRID_BOND_TYPE
        End Get
        Set(value As String)
            Me._A0027_HYBRID_BOND_TYPE = value
        End Set
    End Property
    Property A0027_STRIPS_TAG As String
        Get
            Return Me._A0027_STRIPS_TAG
        End Get
        Set(value As String)
            Me._A0027_STRIPS_TAG = value
        End Set
    End Property
    Property A0027_OVER_ASK_STANDARD_PRICE As String
        Get
            Return Me._A0027_OVER_ASK_STANDARD_PRICE
        End Get
        Set(value As String)
            Me._A0027_OVER_ASK_STANDARD_PRICE = value
        End Set
    End Property
    Property A0027_STLMT_TRAD_OBJT_TAG As String
        Get
            Return Me._A0027_STLMT_TRAD_OBJT_TAG
        End Get
        Set(value As String)
            Me._A0027_STLMT_TRAD_OBJT_TAG = value
        End Set
    End Property
    Property A0027_INPUT_DAY As String
        Get
            Return Me._A0027_INPUT_DAY
        End Get
        Set(value As String)
            Me._A0027_INPUT_DAY = value
        End Set
    End Property
    Property A0027_INVT_ATD_BND_TP_CD As String
        Get
            Return Me._A0027_INVT_ATD_BND_TP_CD
        End Get
        Set(value As String)
            Me._A0027_INVT_ATD_BND_TP_CD = value
        End Set
    End Property


#End Region

#Region "채권소매시장 소매종류코드(G300B)"
    Property G300B_SEQ As String
        Get
            Return Me._G300B_SEQ
        End Get
        Set(value As String)
            Me._G300B_SEQ = value
        End Set
    End Property
    Property G300B_RETAIL_BOND_TYPE_CODE As String
        Get
            Return Me._G300B_RETAIL_BOND_TYPE_CODE
        End Get
        Set(value As String)
            Me._G300B_RETAIL_BOND_TYPE_CODE = value
        End Set
    End Property
    Property G300B_KOR_TYPE_NAME As String
        Get
            Return Me._G300B_KOR_TYPE_NAME
        End Get
        Set(value As String)
            Me._G300B_KOR_TYPE_NAME = value
        End Set
    End Property
    Property G300B_ENG_TYPE_NAME As String
        Get
            Return Me._G300B_ENG_TYPE_NAME
        End Get
        Set(value As String)
            Me._G300B_ENG_TYPE_NAME = value
        End Set
    End Property
    Property G300B_ASK_SUBMIT_TYPE As String
        Get
            Return Me._G300B_ASK_SUBMIT_TYPE
        End Get
        Set(value As String)
            Me._G300B_ASK_SUBMIT_TYPE = value
        End Set
    End Property
    Property G300B_INPUT_DAY As String
        Get
            Return Me._G300B_INPUT_DAY
        End Get
        Set(value As String)
            Me._G300B_INPUT_DAY = value
        End Set
    End Property
    Property G3027_SEQ As String
        Get
            Return Me._G3027_SEQ
        End Get
        Set(value As String)
            Me._G3027_SEQ = value
        End Set
    End Property
    Property G3027_RETAIL_BOND_TYPE_CODE As String
        Get
            Return Me._G3027_RETAIL_BOND_TYPE_CODE
        End Get
        Set(value As String)
            Me._G3027_RETAIL_BOND_TYPE_CODE = value
        End Set
    End Property
    Property G3027_KOR_TYPE_NAME As String
        Get
            Return Me._G3027_KOR_TYPE_NAME
        End Get
        Set(value As String)
            Me._G3027_KOR_TYPE_NAME = value
        End Set
    End Property
    Property G3027_ENG_TYPE_NAME As String
        Get
            Return Me._G3027_ENG_TYPE_NAME
        End Get
        Set(value As String)
            Me._G3027_ENG_TYPE_NAME = value
        End Set
    End Property
    Property G3027_ASK_SUBMIT_TYPE As String
        Get
            Return Me._G3027_ASK_SUBMIT_TYPE
        End Get
        Set(value As String)
            Me._G3027_ASK_SUBMIT_TYPE = value
        End Set
    End Property
    Property G3027_INPUT_DAY As String
        Get
            Return Me._G3027_INPUT_DAY
        End Get
        Set(value As String)
            Me._G3027_INPUT_DAY = value
        End Set
    End Property
#End Region

#Region "기타 변수"


    Property locFile As String
        Get
            Return Me._locFile
        End Get
        Set(value As String)
            Me._locFile = value
        End Set
    End Property



#End Region

#Region "FuncKoscomSrcFile 메소드 변수"
    Property loc_trgfile As String
        Get
            Return Me._loc_trgfile
        End Get
        Set(value As String)
            Me._loc_trgfile = value
        End Set
    End Property
    Property all_loc_trgfile As String
        Get
            Return Me._all_loc_trgfile
        End Get
        Set(value As String)
            Me._all_loc_trgfile = value
        End Set
    End Property
    Property v_loc_trgfile As String
        Get
            Return Me._v_loc_trgfile
        End Get
        Set(value As String)
            Me._v_loc_trgfile = value
        End Set
    End Property
    Property am_loc_trgfile As String
        Get
            Return Me._am_loc_trgfile
        End Get
        Set(value As String)
            Me._am_loc_trgfile = value
        End Set
    End Property
    Property pm_loc_trgfile As String
        Get
            Return Me._pm_loc_trgfile
        End Get
        Set(value As String)
            Me._pm_loc_trgfile = value
        End Set
    End Property
    Property am_vd_loc_trgfile As String
        Get
            Return Me._am_vd_loc_trgfile
        End Get
        Set(value As String)
            Me._am_vd_loc_trgfile = value
        End Set
    End Property
    Property temp_loc_trgfile As String
        Get
            Return Me._temp_loc_trgfile
        End Get
        Set(value As String)
            Me._temp_loc_trgfile = value
        End Set
    End Property
    Property Win_loc_trgfile As String
        Get
            Return Me._Win_loc_trgfile
        End Get
        Set(value As String)
            Me._Win_loc_trgfile = value
        End Set
    End Property
    Property temp_length As Integer
        Get
            Return Me._temp_length
        End Get
        Set(value As Integer)
            Me._temp_length = value
        End Set
    End Property
    Property tempLine As String
        Get
            Return Me._tempLine
        End Get
        Set(value As String)
            Me._tempLine = value
        End Set
    End Property
    Property index As String
        Get
            Return Me._index
        End Get
        Set(value As String)
            Me._index = value
        End Set
    End Property
    Property trCode As String
        Get
            Return Me._trCode
        End Get
        Set(value As String)
            Me._trCode = value
        End Set
    End Property

    Property am_end_trgfile As String
        Get
            Return Me._am_end_trgfile
        End Get
        Set(value As String)
            Me._am_end_trgfile = value
        End Set
    End Property

    Property am_loc_temp_trgfile As String
        Get
            Return Me._am_loc_temp_trgfile
        End Get
        Set(value As String)
            Me._am_loc_temp_trgfile = value
        End Set
    End Property

    Property pm_loc_temp_trgfile As String
        Get
            Return Me._pm_loc_temp_trgfile
        End Get
        Set(value As String)
            Me._pm_loc_temp_trgfile = value
        End Set
    End Property

    Property pm_end_trgfile As String
        Get
            Return Me._pm_end_trgfile
        End Get
        Set(value As String)
            Me._pm_end_trgfile = value
        End Set
    End Property

    Property am_end_temp_trgfile As String
        Get
            Return Me._am_end_temp_trgfile
        End Get
        Set(value As String)
            Me._am_end_temp_trgfile = value
        End Set
    End Property

    Property pm_end_temp_trgfile As String
        Get
            Return Me._pm_end_temp_trgfile
        End Get
        Set(value As String)
            Me._pm_end_temp_trgfile = value
        End Set
    End Property

    Property am_vd_loc_temp_trgfile As String
        Get
            Return Me._am_vd_loc_temp_trgfile
        End Get
        Set(value As String)
            Me._am_vd_loc_temp_trgfile = value
        End Set
    End Property
    Property am_loc_trgfile_beforeTDay As String
        Get
            Return Me._am_loc_trgfile_beforeTDay
        End Get
        Set(value As String)
            Me._am_loc_trgfile_beforeTDay = value
        End Set
    End Property
    Property am_loc_temp_trgfile_beforeTDay As String
        Get
            Return Me._am_loc_temp_trgfile_beforeTDay
        End Get
        Set(value As String)
            Me._am_loc_temp_trgfile_beforeTDay = value
        End Set
    End Property
    Property am_vd_loc_trgfile_beforeTDay As String
        Get
            Return Me._am_vd_loc_trgfile_beforeTDay
        End Get
        Set(value As String)
            Me._am_vd_loc_trgfile_beforeTDay = value
        End Set
    End Property
    Property am_vd_loc_temp_trgfile_beforeTDay As String
        Get
            Return Me._am_vd_loc_temp_trgfile_beforeTDay
        End Get
        Set(value As String)
            Me._am_vd_loc_temp_trgfile_beforeTDay = value
        End Set
    End Property
    Property am_end_trgfile_beforeTDay As String
        Get
            Return Me._am_end_trgfile_beforeTDay
        End Get
        Set(value As String)
            Me._am_end_trgfile_beforeTDay = value
        End Set
    End Property
    Property am_end_temp_trgfile_beforeTDay As String
        Get
            Return Me._am_end_temp_trgfile_beforeTDay
        End Get
        Set(value As String)
            Me._am_end_temp_trgfile_beforeTDay = value
        End Set
    End Property
    Property loc_A0027_trgFile As String
        Get
            Return Me._loc_A0027_trgFile
        End Get
        Set(value As String)
            Me._loc_A0027_trgFile = value
        End Set
    End Property
    Property loc_G3027_trgFile As String
        Get
            Return Me._loc_G3027_trgFile
        End Get
        Set(value As String)
            Me._loc_G3027_trgFile = value
        End Set
    End Property
    Property loc_A0027_temp_trgFile As String
        Get
            Return Me._loc_A0027_temp_trgFile
        End Get
        Set(value As String)
            Me._loc_A0027_temp_trgFile = value
        End Set
    End Property
    Property loc_G3027_temp_trgFile As String
        Get
            Return Me._loc_G3027_temp_trgFile
        End Get
        Set(value As String)
            Me._loc_G3027_temp_trgFile = value
        End Set
    End Property

    'KRX 차세대 관련 변수 추가
    Property loc_A001B_trgFile As String
        Get
            Return Me._loc_A001B_trgFile
        End Get
        Set(value As String)
            Me._loc_A001B_trgFile = value
        End Set
    End Property
    Property loc_G300B_trgFile As String
        Get
            Return Me._loc_G300B_trgFile
        End Get
        Set(value As String)
            Me._loc_G300B_trgFile = value
        End Set
    End Property
    Property loc_A001B_temp_trgFile As String
        Get
            Return Me._loc_A001B_temp_trgFile
        End Get
        Set(value As String)
            Me._loc_A001B_temp_trgFile = value
        End Set
    End Property
    Property loc_G300B_temp_trgFile As String
        Get
            Return Me._loc_G300B_temp_trgFile
        End Get
        Set(value As String)
            Me._loc_G300B_temp_trgFile = value
        End Set
    End Property


    Property all_loc_temp_trgfile As String
        Get
            Return Me._all_loc_temp_trgfile
        End Get
        Set(value As String)
            Me._all_loc_temp_trgfile = value
        End Set
    End Property
#End Region

#Region "GetData 메소드 변수"
    Property newData As Integer
        Get
            Return Me._newData
        End Get
        Set(value As Integer)
            Me._newData = value
        End Set
    End Property

    Property InputLineCount As Integer
        Get
            Return Me._InputLineCount
        End Get
        Set(value As Integer)
            Me._InputLineCount = value
        End Set
    End Property

    Property CheckLenLine As Integer
        Get
            Return Me._checkLenLine
        End Get
        Set(value As Integer)
            Me._checkLenLine = value
        End Set
    End Property
    Property progressLineCount As Integer
        Get
            Return Me._progressLineCount
        End Get
        Set(value As Integer)
            Me._progressLineCount = value
        End Set
    End Property
#End Region

#Region "KOSCOM HISTORY INSERT에 필요한 변수"
    Property i_bond_id As String
        Get
            Return Me._i_bond_id
        End Get
        Set(value As String)
            Me._i_bond_id = value
        End Set
    End Property
    Property i_local_type_code As String
        Get
            Return Me._i_local_type_code
        End Get
        Set(value As String)
            Me._i_local_type_code = value
        End Set
    End Property
    Property i_abs_type_code As String
        Get
            Return Me._i_abs_type_code
        End Get
        Set(value As String)
            Me._i_abs_type_code = value
        End Set
    End Property
    Property i_first_coupon_day As String
        Get
            Return Me._i_first_coupon_day
        End Get
        Set(value As String)
            Me._i_first_coupon_day = value
        End Set
    End Property
    Property i_batch_amt_confirm_gb As String
        Get
            Return Me._i_batch_amt_confirm_gb
        End Get
        Set(value As String)
            Me._i_batch_amt_confirm_gb = value
        End Set
    End Property
    Property i_named_gb As String
        Get
            Return Me._i_named_gb
        End Get
        Set(value As String)
            Me._i_named_gb = value
        End Set
    End Property
    Property i_prcp_accr_rate As String
        Get
            Return Me._i_prcp_accr_rate
        End Get
        Set(value As String)
            Me._i_prcp_accr_rate = value
        End Set
    End Property
    Property i_admin_org_id As String
        Get
            Return Me._i_admin_org_id
        End Get
        Set(value As String)
            Me._i_admin_org_id = value
        End Set
    End Property
    Property i_guarant_org_id As String
        Get
            Return Me._i_guarant_org_id
        End Get
        Set(value As String)
            Me._i_guarant_org_id = value
        End Set
    End Property
    Property i_custody_org_id As String
        Get
            Return Me._i_custody_org_id
        End Get
        Set(value As String)
            Me._i_custody_org_id = value
        End Set
    End Property
    Property i_registration_org_id As String
        Get
            Return Me._i_registration_org_id
        End Get
        Set(value As String)
            Me._i_registration_org_id = value
        End Set
    End Property
    Property i_prcp_agency_org_id As String
        Get
            Return Me._i_prcp_agency_org_id
        End Get
        Set(value As String)
            Me._i_prcp_agency_org_id = value
        End Set
    End Property
    Property i_int_calc_type_code As String
        Get
            Return Me._i_int_calc_type_code
        End Get
        Set(value As String)
            Me._i_int_calc_type_code = value
        End Set
    End Property
    Property i_int_accr_hldy_type_code As String
        Get
            Return Me._i_int_accr_hldy_type_code
        End Get
        Set(value As String)
            Me._i_int_accr_hldy_type_code = value
        End Set
    End Property
    Property i_int_accr_pay_hldy_type_code As String
        Get
            Return Me._i_int_accr_pay_hldy_type_code
        End Get
        Set(value As String)
            Me._i_int_accr_pay_hldy_type_code = value
        End Set
    End Property
    Property i_int_accr_rate As String
        Get
            Return Me._i_int_accr_rate
        End Get
        Set(value As String)
            Me._i_int_accr_rate = value
        End Set
    End Property
    Property i_prcp_accr_pay_hldy_type_code As String
        Get
            Return Me._i_prcp_accr_pay_hldy_type_code
        End Get
        Set(value As String)
            Me._i_prcp_accr_pay_hldy_type_code = value
        End Set
    End Property
    Property i_prcp_accr_hldy_type_code As String
        Get
            Return Me._i_prcp_accr_hldy_type_code
        End Get
        Set(value As String)
            Me._i_prcp_accr_hldy_type_code = value
        End Set
    End Property
    Property i_issuer_org_id As String
        Get
            Return Me._i_issuer_org_id
        End Get
        Set(value As String)
            Me._i_issuer_org_id = value
        End Set
    End Property
    Property i_bond_type_code As String
        Get
            Return Me._i_bond_type_code
        End Get
        Set(value As String)
            Me._i_bond_type_code = value
        End Set
    End Property


#End Region


End Class
