Public Class DB_Query

#Region " 사용 하는 쿼리함수"
    ''' <summary>
    ''' 오전 선택시 이전 영업일 조회
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared checkExch_ETC As String _
= <sql><![CDATA[
SELECT FBN_CTRL_GET_STATUS(:log_date ,:job_status) flag 
FROM dual
]]></sql>.Value
    'Select FBN_CTRL_GET_STATUS('" & :log_date & "','" & :job_status & "') flag 
    'From dual

    ''' <summary>
    ''' TR의 STR_SEQ 데이터 조회
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared STR_SEQ_Query As String _
= <sql><![CDATA[
 SELECT STR_SEQ
           FROM KCMATD00
           WHERE TR = :tr AND REC_METH_CD = 'FTP' AND END_DAY = '99991231'
           ORDER BY STR_SEQ
]]></sql>.Value

    ''' <summary>
    ''' TR 의 STR_LEN 데이터 조회
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared STR_LEN_Query As String _
= <sql><![CDATA[
 SELECT STR_LEN
           FROM KCMATD00
           WHERE TR = :tr AND REC_METH_CD = 'FTP' AND END_DAY = '99991231'
           ORDER BY STR_SEQ
]]></sql>.Value

    ''' <summary>
    ''' TR 데이터 조회
    ''' TDY_DOCNT
    ''' TDY_DIDCNT</summary>
    ''' <remarks></remarks>
    Public Shared TR_DAY_Query As String _
= <sql><![CDATA[
SELECT '1' AS CHK , TR AS TR, NULL AS TDY_DOCNT , NULL AS TDY_DIDCNT ,NULL AS COMPLETE, REC_METH_CD
FROM KCMATC00
WHERE REC_METH_CD = 'FTP' AND TR IN('VD', 'V2', 'V7', 'V3', 'V8', 'V9')
]]></sql>.Value

    ''' <summary>
    ''' TR 데이터 조회
    ''' TDY_DOCNT
    ''' TDY_DIDCNT</summary>
    ''' <remarks></remarks>
    Public Shared TR_NIGHT_Query As String _
= <sql><![CDATA[
SELECT '1' AS CHK , TR AS TR, NULL AS TDY_DOCNT , NULL AS TDY_DIDCNT ,NULL AS COMPLETE, REC_METH_CD
FROM KCMATC00
WHERE REC_METH_CD = 'FTP' AND TR IN ('VD', 'V2', 'V7', 'V3', 'V8', 'V9', 'G300B', 'A001B')
]]></sql>.Value

    ''' <summary>
    ''' TR의 COL_NAME 데이터 조회
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared COL_NAME_Query As String _
= <sql><![CDATA[
 SELECT COL_NAME
           FROM KCMATD00
           WHERE TR = :tr AND REC_METH_CD = 'FTP' AND END_DAY = '99991231'
           ORDER BY STR_SEQ
]]></sql>.Value

    ''' <summary>
    ''' TR의 테이블 명 데이터 조회
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared TABLE_NAME_Query As String _
= <sql><![CDATA[
SELECT TABLE_NAME
FROM KCMATC00
where TR = :tr
]]></sql>.Value

    ''' <summary>
    ''' "VD" 내에 TAG:1 최종 종목으로 개수 확인
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared FindTagFinal As String _
= <sql><![CDATA[
SELECT COUNT(:bnd_id) CNT
FROM :tr_tablename
where :bnd_id = :vd_bnd_id AND TAG = '1'
]]></sql>.Value

    ''' <summary>
    ''' "VD" 내에 TAG:1 최종 종목 있을 경우 삭제
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared checkExistsAndDeleteTag As String _
= <sql><![CDATA[
DELETE
FROM :tr_tablename
WHERE :bnd_id = :vd_bnd_id AND TAG = '1'
]]></sql>.Value

    ''' <summary>
    ''' "VD" 내에 당일입수 정보로 TAG 1: 최종으로 입력.
    ''' </summary>
    ''' <remarks></remarks>
    ''' 투자유의채권구분코드 추가(투자유의채권구분코드|0: 해당없음 1: 지정예고 2: 지정)
    ''' 
    Public Shared InsertTodayVDdata As String _
= <sql><![CDATA[
INSERT INTO :tr_tablename
     (:colNameVar_VD
    TAG,
    REG_DTM, 
    REG_DT) 
VALUES (
    :vd_bnd_id, 
    :vd_apply_day, 
    :vd_mkt_id,
    :vd_trd_crc_id,
    :vd_trd_unit,
    :vd_shsl_psb_yn,
    :vd_cdt_ord_psb_yn,
    :vd_dicr_end_psb_yn,
    :vd_mkt_fmtn_psb_yn,
    :vd_hstl_prc, 
    :vd_lstl_prc,
    :vd_trd_stp_yn,
    :vd_adjs_sb_yn,
    :vd_list_dt,
    :vd_bnd_list_absh_dt,
    :vd_clprc,
    :vd_clprc_ernr,
    :vd_std_prc,
    :vd_sbst_prc,
    :vd_bnd_list_absh_cau_cd, 
    :vd_list_amt,
    :vd_bprd_int_paynt_dt,
    :vd_nxtm_int_paynt_dt,
    :vd_rtsl_bnd_clss_cd,
    :vd_samt_sb_knd_cd,
    :vd_ntbn_knd_cd,
    :vd_ntbn_exr_ycnt,
    :vd_ntbn_stk_tp_cd,
    :vd_repo_clss_cd,
    :vd_invt_atd_bnd_tp_cd,
    :vd_div_repay_dt,
    '1',  
    SYSDATE, 
    :today_date)
]]></sql>.Value
    ''' <summary>
    ''' "VD" 내에 당일입수 정보로 TAG 2 : HISTORY로 입력한다. 
    ''' </summary>
    ''' <remarks></remarks>
    ''' 

    Public Shared InsertVDdataHistory As String _
= <sql><![CDATA[
INSERT INTO :tr_tablename (:colNameVar_VD TAG,REG_DTM,REG_DT)
    SELECT :colNameVar_VD
        '2',
        REG_DTM,
        REG_DT
    FROM BN00TD13
    WHERE BND_ID = :vd_bnd_id 
          AND TAG = '1'
          AND REG_DT = :today_date
]]></sql>.Value

    ''' <summary>
    ''' 회사코드정보 조회 V7
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared checkCountOrg_ID As String _
= <sql><![CDATA[
SELECT :org_id
FROM :tr_tablename
WHERE ORG_ID = :v7_org_id
]]></sql>.Value

    ''' <summary>
    ''' 회사코드정보 update (CP_ORG_ID 가 없을 경우) V7
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared updateOrgCodeInformWithoutCpOrgId As String _
= <sql><![CDATA[
UPDATE :tr_tablename
SET ORG_NAME = :v7_org_name,
FIN_ORG_ID = :v7_fin_org_id,
SPC_ORG_ID = :v7_spc_org_id,
REMARK = :today_date
WHERE ORG_ID = :v7_org_id

]]></sql>.Value

    ''' <summary>
    ''' 회사코드정보 update (CP_ORG_ID 가 있을 경우) V7
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared updateOrgCodeInform As String _
= <sql><![CDATA[
UPDATE :tr_tablename
SET ORG_NAME = :v7_org_name,
CP_ORG_ID = :v7_cp_org_id,
FIN_ORG_ID = :v7_fin_org_id,
SPC_ORG_ID = :v7_spc_org_id,
REMARK = :today_date
WHERE ORG_ID = :v7_org_id

]]></sql>.Value

    ''' <summary>
    ''' 회사코드정보 update (CP_ORG_ID 가 있을 경우) V7
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared InsertOrgCodeInform As String _
= <sql><![CDATA[
INSERT INTO :tr_tablename (:colNameVar_V7 REMARK)
VALUES (:v7_org_name,:v7_org_id,:v7_cp_org_id,:v7_fin_org_id,:v7_spc_org_id,:today_date)

]]></sql>.Value


    ''' <summary>
    ''' 발행기관코드의 Count 확인 (V3)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared CheckCountIssueOrgID As String _
= <sql><![CDATA[
SELECT :issue_org_id
FROM :tr_tablename
WHERE :issue_org_id = :v3_issuer_org_id
]]></sql>.Value

    ''' <summary>
    ''' 발행기관코드가 0보다 크면 update 진행
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared UpdateV3 As String _
= <sql><![CDATA[
UPDATE :tr_tablename
SET ORG_FULL_NM = :v3_org_full_nm,
    ORG_SHORT_NM = :v3_org_short_nm,
    ORG_ENG_FULL_NM = :v3_org_eng_full_nm,
    ORG_ENG_SHORT_NM = :v3_org_eng_short_nm,
    INPUT_DAY = SYSDATE 
    WHERE ISSUE_ORG_ID = :v3_issuer_org_id 
]]></sql>.Value


    ''' <summary>
    ''' 발행기관코드가 0보다 작으면 INSERT 진행
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared InsertV3 As String _
= <sql><![CDATA[
INSERT INTO :tr_tablename (:colNameVar_V3 INPUT_DAY)
       VALUES (:v3_issuer_org_id, :v3_org_full_nm, :v3_org_short_nm, :v3_org_eng_full_nm, :v3_org_eng_short_nm, SYSDATE)
]]></sql>.Value

    ''' <summary>
    ''' Data Log Insert
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared InsertDataLog As String _
= <sql><![CDATA[
INSERT INTO LOG_DATAINSERT (DAY,FILE_NAME)
VALUES (TO_CHAR(SYSDATE,'YYYYMMDD'), ':rightBdata')
]]></sql>.Value


    ''' <summary>
    ''' (V2) TAG : 1 최종으로 종목 조회
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared finalTagCheckV2 As String _
= <sql><![CDATA[
SELECT COUNT(:bond_id) CNT
FROM :tr_tablename
WHERE BOND_ID = :v2_data_id AND TAG = '1'
]]></sql>.Value

    ''' <summary>
    ''' (V2) TAG : 1 최종으로 종목이 있을 경우 삭제한다. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared finalDeleteTagV2 As String _
= <sql><![CDATA[
DELETE 
    FROM :tr_tablename
    WHERE BOND_ID = :v2_data_id AND TAG = '1'
]]></sql>.Value

    ''' <summary>
    ''' (V2) 당일입수 정보로 TAG 1 최종으로 입력한다. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared finalInsertTagV2 As String _
= <sql><![CDATA[
INSERT INTO :tr_tablename (:colNameVar_V2 KOSCOM_IN_TIME, REG_DTM, TAG, REG_DT)
VALUES (:v2_final_Data SYSDATE, '1', :today_date)
]]></sql>.Value


    ''' <summary>
    ''' (V2) 당일입수 정보로 TAG 1 최종으로 입력한다. 
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared HistroyInsertTagV2 As String _
= <sql><![CDATA[
INSERT INTO :tr_tablename
     SELECT :colNameVar_V2 KOSCOM_IN_TIME, REG_DTM, '2', REG_DT
     FROM BN00TD00
     WHERE BOND_ID = :v2_data_id AND TAG = '1' AND REG_DT  = :today_date                               
]]></sql>.Value

    ''' <summary>
    ''' (V9) 데이터 INSERT
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared InsertV9 As String _
= <sql><![CDATA[
INSERT INTO :tr_tablename (:colNameVar_V9 KOSCOM_IN_TIME, DAY)
            VALUES (:v9_data SYSDATE)
]]></sql>.Value

    ''' <summary>
    ''' (V8) 데이터 INSERT
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared InsertV8 As String _
= <sql><![CDATA[
INSERT INTO :tr_tablename (:colNameVar_V8 KOSCOM_IN_TIME, DAY)
            VALUES (:v8_data SYSDATE)
]]></sql>.Value

    ''' <summary>
    ''' (V3) 데이터 INSERT
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared InsertKoscomV3 As String _
= <sql><![CDATA[
INSERT INTO :tr_tablename (DAY, ISSUER_ORG_ID, :colNameVar_V3 KOSCOM_IN_TIME)
            VALUES (SYSDATE, :v3_Data)
]]></sql>.Value

    ''' <summary>
    ''' (V7) 데이터 INSERT
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared InsertKoscomV7 As String _
= <sql><![CDATA[
INSERT INTO :tr_tablename (DAY, ORG_NM, :colNameVar_V7_org_Id, ISSUER_ORG_ID, :colNameVar_V7_fin_org_Id,:colNameVar_V7_spc_org_id,  KOSCOM_IN_TIME)
            VALUES (SYSDATE, :v7_Data)
]]></sql>.Value


    ''' <summary>
    ''' (KRX) 국고채 프라임 총수익지수, 순가격지수 이미 있는지 확인 (현재 사용 안함)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared CheckKRXData As String _
= <sql><![CDATA[
SELECT DAY, GUBUN_CD, INTEREST_NAME
FROM BN11TD16
WHERE GUBUN_CD IN ('73', '74') AND INPUT_DAY = ':Today_Date' 
]]></sql>.Value

    ''' <summary>
    ''' (KRX) 국고채 프라임 총수익지수, 순가격지수 데이터 삭제 (현재 사용 안함)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared DeleteKRXData As String _
= <sql><![CDATA[
DELETE
    FROM BN11TD16
    WHERE GUBUN_CD IN ('73', '74') AND INPUT_DAY = ':Today_Date' 
]]></sql>.Value

    ''' <summary>
    ''' (KRX) 데이터 조회 (현재 사용 안함)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared BN11TDKRXDataSearch As String _
= <sql><![CDATA[
SELECT DAY,LAST_MODIFIED_TIME,GROUP_CODE,MAT_CODE,TRUNC(CLEAN_INDEX, 2) CLEAN_INDEX,TRUNC(TOTAL_INDEX, 2) TOTAL_INDEX
FROM NICE.BN11TD15@TITAN
WHERE DAY = ':Today_Date' AND GROUP_CODE = '10000' AND MAT_CODE = '000' AND TO_NUMBER(SUBSTR(LAST_MODIFIED_TIME, 1, 4)) >= 1500
]]></sql>.Value

    ''' <summary>
    ''' (KRX) 국고채 프라임 총수익지수 데이터 삽입 (현재 사용 안함)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared InsertKRXTotal_Index As String _
= <sql><![CDATA[
INSERT INTO BN11TD16 (DAY,GUBUN_CD,INTEREST_NAME,REMN_TERM,PREV_INT,AM_INT,PM_INT,INT_VOL,SPREAD,YEAR_HIGHEST,YEAR_LOWEST,INPUT_DAY)
VALUES(':Today_Day','73','KRX국고채프라임총수익지수','',0,0,:total_Index,0,0,0,0,':Today_Date')
]]></sql>.Value

    ''' <summary>
    ''' (KRX) 국고채 프라임 총수익지수, 순가격지수 데이터 삽입(현재 사용 안함)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared InsertKRXClean_Index As String _
= <sql><![CDATA[
INSERT INTO BN11TD16 (DAY,GUBUN_CD,INTEREST_NAME,REMN_TERM,PREV_INT,AM_INT,PM_INT,INT_VOL,SPREAD,YEAR_HIGHEST,YEAR_LOWEST,INPUT_DAY)
VALUES(':Today_Day','74','KRX국고채프라임순가격지수','',0,0,:clean_Index,0,0,0,0,':Today_Date')
]]></sql>.Value


    ''' <summary>
    ''' (A0027) 채권소매시장 중 A0027 데이터 INSERT
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared InsertRetailA0027 As String _
= <sql><![CDATA[
INSERT INTO BN06TD00 
(:colNameVar_a0027 INPUT_DAY)

VALUES (
:a0027_bond_id,
:a0027_seq, 
:a0027_day , 
:a0027_retail_bond_type_code,
:a0027_kor_bond_nm , 
:a0027_eng_bond_nm , 
:a0027_list_type_code,
:a0027_bond_ancd_code , 
:a0027_guarantee_type_code , 
:a0027_int_pay_type_code,
:a0027_list_day , 
:a0027_issue_day , 
:a0027_mat_day , 
:a0027_sell_day,
:a0027_issue_amt , 
:a0027_coupon_rate , 
:a0027_int_pay_month_calc , 
:a0027_int_pay_time_type_code,
:a0027_int_pay_term_type_code , 
:a0027_int_end_month_type , 
:a0027_int_pay_unit_type,
:a0027_pre_sell_int_type_code , 
:a0027_prcp_amt , 
:a0027_list_amt , 
:a0027_cb_prcp_return_rate,
:a0027_installment_return_type , 
:a0027_unredeem_term , 
:a0027_refund_term_times , 
:a0027_deal_stop_type,
:a0027_prev_int_pay_day , 
:a0027_next_int_pay_day , 
:a0027_hybrid_bond_type,
:a0027_strips_tag , 
:a0027_over_ask_standard_price , 
:a0027_stlmt_trad_objt_tag,
:a0027_invt_atd_bnd_tp_cd,
:a0027_input_day
)
]]></sql>.Value

    ''' <summary>
    ''' (A0027) 채권소매시장 중 A0027 데이터 INSERT
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared InsertRetailA001B As String _
= <sql><![CDATA[
INSERT INTO BN06TD00
(:colNameVar_A001B INPUT_DAY)

VALUES (
:a001b_seq, 
:a001b_bond_cnt,
:a001b_day , 
:a001b_bond_id,
:a001b_bond_seq,
:a001b_retail_bond_type_code,
:a001b_kor_bond_nm , 
:a001b_eng_bond_nm , 
:a001b_tsc_id , 
:a001b_list_type_code,
:a001b_bond_ancd_code , 
:a001b_guarantee_type_code , 
:a001b_int_pay_type_code,
:a001b_list_day , 
:a001b_issue_day , 
:a001b_mat_day , 
:a001b_sell_day,
:a001b_issue_amt , 
:a001b_coupon_rate , 
:a001b_int_pay_month_calc , 
:a001b_int_pay_time_type_code,
:a001b_int_pay_term_type_code , 
:a001b_int_end_month_type , 
:a001b_int_pay_unit_type,
:a001b_pre_sell_int_type_code , 
:a001b_prcp_amt , 
:a001b_list_amt , 
:a001b_cb_prcp_return_rate,
:a001b_installment_return_type , 
:a001b_unredeem_term , 
:a001b_refund_term_times , 
:a001b_deal_stop_type,
:a001b_prev_int_pay_day , 
:a001b_next_int_pay_day , 
:a001b_hybrid_bond_type,
:a001b_strips_tag , 
:a001b_over_ask_standard_price , 
:a001b_stlmt_trad_objt_tag,
:a001b_invt_atd_bnd_tp_cd,
SYSDATE
)
]]></sql>.Value

    ''' <summary>
    ''' (G3027) 채권소매시장 중 G3027 데이터 INSERT
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared InsertRetailG3027 As String _
= <sql><![CDATA[
INSERT INTO BN06TD01 (:colNameVar_g3027 INPUT_DAY)
VALUES(:g3027_seq,:g3027_retail_bond_type_code,:g3027_kor_type_name,:g3027_eng_type_name,:g3027_ask_submit_type,:g3027_input_day)
]]></sql>.Value

    ''' <summary>
    ''' (G3027) 채권소매시장 중 G3027 데이터 INSERT
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared InsertRetailG300B As String _
= <sql><![CDATA[
INSERT INTO BN06TD01 (:colNameVar_G300B INPUT_DAY)
VALUES(:g300b_seq,:g300b_retail_bond_type_code,:g300b_kor_type_name,:g300b_eng_type_name,:g300b_ask_submit_type,SYSDATE)
]]></sql>.Value


    ''' <summary>
    ''' BN00TD00 정보를 PABNTD00에 
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared PBN_LOAD_BOND_INFO_Histroy As String _
= <sql><![CDATA[
        INSERT INTO PABNTD00 (SEQ, IN_DAY, IN_DAY_TIME, IN_ID, DAY, BOND_ID, KOR_BOND_NM, ENG_BOND_NM, ISSUER_ORG_ID,
                              LIST_TYPE_CODE, BOND_TYPE_CODE, BND_TYPE_CD, SPBN_ISCO_CD, LOCAL_TYPE_CODE, GUARANTEE_TYPE_CODE,
                              GUARANTEE_RATE, STOCK_TYPE_CODE, OPTION_TYPE_CODE, INT_PAY_TYPE_CODE, REFUND_TYPE_CODE,
                              ISSUE_TYPE_CODE, ABS_TYPE_CODE, DEFERRED_BOND_TYPE_CODE, LIST_DAY, ISSUE_DAY, MAT_DAY, SELL_DAY,
                              FIRST_COUPON_DAY, ISSUE_RATE, COUPON_RATE, INT_PERIOD_MONTH, INT_PAY_TIME_TYPE_CODE,
                              BND_INT_PAYNT_DD_STD_TP_CD, INT_MME_TP_CD, INT_UNDER_TRUNC_WAY, INT_PAYNT_UNIT_MONS,
                              SELL_TYPE_CODE, PRE_SELL_INT_TYPE_CODE, PRCP_AMT, LIST_AMT, BATCH_AMT_CONFIRM_GB, CURRENCY_CODE,
                              MAT_REFUND_RATE, GUARANTEE_YTM, GUARANTEE_YTM_DAY, ADD_YTM, ADD_YTM_DAY, EQUIPMENT_PRCP_AMT, OPERATION_PRCP_AMT,
                              REFUND_PRCP_AMT, ETC_PRCP_AMT, NAMED_GB, TAX_GB, ADMIN_ORG_ID, GUARANT_ORG_ID, CUSTODY_ORG_ID,
                              REGISTRATION_ORG_ID, PRCP_AGENCY_ORG_ID, SHORT_BOND_ID, LIST_ABOLISH_REASON, LIST_END_DAY,
                              CLAIM_TYPE_CODE, CB_STOCK_ID, CB_STOCK_NM, EXECUTE_PRICE, EXECUTE_RATE, CB_START_DAY, CB_END_DAY,
                              DEMAND_ORG, DIVIDEND_CALC_GB, PB_ACC_GB, BW_STOCK_ID, DIV_REPAY_GB, EQUAL_REPAY_AMT,
                              UNREDEEM_TERM, RPY_TERM_INT_TP_CD, DIV_REPAY_CNT, STD_YTM_WAY, ADDL_INTRT, INT_RATE_POINT,
                              STD_YTM_MAX, STD_YTM_MIN, INT_DECI_DAY, INT_ACCR_PAY_HLDY_TYPE_CODE,
                              MAT_STRUCTURE_CODE, COND_CAPT_SECR_TYPE_TP_CD, INTEREST_CODE, STRIPS_TAG, ORIGINAL_ID,
                              STRIPS_REMAIN_AMT, CPI_TAG, REF_CPI, INT_DECI_RATE_CODE, INT_CALC_TYPE_CODE, INT_ACCR_HLDY_TYPE_CODE,
                              INT_ACCR_RATE, PRCP_ACCR_PAY_HLDY_TYPE_CODE, PRCP_ACCR_HLDY_TYPE_CODE, PRCP_ACCR_RATE, SCBD_INTRT,
                              CALL_BEGIN_DAY_1, CALL_END_DAY_1, CALL_BEGIN_DAY_2, CALL_END_DAY_2,
                              PUT_BEGIN_DAY_1, PUT_END_DAY_1, PUT_BEGIN_DAY_2, PUT_END_DAY_2,
                              ELN_PRCP_PROTECT_RATE, PARTICIPATION_RATE, ELN_MAX_RETURN, KOSCOM_BOND_CLASS, ORG_ID)
                              
                      SELECT (SELECT NVL(MAX(SEQ), -1) + 1 FROM PABNTD00 WHERE BOND_ID = A.BOND_ID) SEQ,                      
                             :t_day IN_DAY, SYSDATE IN_DAY_TIME, 'KOSCOM' IN_ID, :t_day DAY, A.BOND_ID, A.KOR_BOND_NM, A.ENG_BOND_NM,
                             A.ISSUER_ORG_ID, A.LIST_TYPE_CODE, A.BOND_TYPE_CODE, A.BND_TYPE_CD, A.SPBN_ISCO_CD, A.LOCAL_TYPE_CODE,
                             A.GUARANTEE_TYPE_CODE, A.GUARANTEE_RATE, A.STOCK_TYPE_CODE, A.OPTION_TYPE_CODE, A.INT_PAY_TYPE_CODE,
                             A.REFUND_TYPE_CODE, A.ISSUE_TYPE_CODE, A.ABS_TYPE_CODE, A.DEFERRED_BOND_TYPE_CODE,
                             B.LIST_DT,A.ISSUE_DAY, A.MAT_DAY, A.SELL_DAY, A.FIRST_COUPON_DAY, A.ISSUE_RATE, A.COUPON_RATE, A.INT_PERIOD_MONTH,
                             A.INT_PAY_TIME_TYPE_CODE, A.BND_INT_PAYNT_DD_STD_TP_CD, A.INT_MME_TP_CD, A.INT_UNDER_TRUNC_WAY, A.INT_PAYNT_UNIT_MONS,
                             A.SELL_TYPE_CODE, A.PRE_SELL_INT_TYPE_CODE, A.PRCP_AMT, 
                             B.LIST_AMT, A.BATCH_AMT_CONFIRM_GB, A.CURRENCY_CODE, A.MAT_REFUND_RATE, A.GUARANTEE_YTM, A.GUARANTEE_YTM_DAY, A.ADD_YTM, A.ADD_YTM_DAY,
                             A.EQUIPMENT_PRCP_AMT, A.OPERATION_PRCP_AMT, A.REFUND_PRCP_AMT, A.ETC_PRCP_AMT, A.NAMED_GB,                             
                             A.TAX_GB, A.ADMIN_ORG_ID, A.GUARANT_ORG_ID, A.CUSTODY_ORG_ID, A.REGISTRATION_ORG_ID, A.PRCP_AGENCY_ORG_ID, A.SHORT_BOND_ID,                             
                             A.LIST_ABSH_CAU_CD, A.LIST_END_DAY, A.CLAIM_TYPE_CODE, A.CB_STOCK_ID, A.CB_STOCK_NM,                             
                             A.EXECUTE_PRICE, A.EXECUTE_RATE, A.CB_START_DAY, A.CB_END_DAY, A.DEMAND_ORG, A.DIVIDEND_CALC_GB, A.PB_ACC_GB,
                             A.BW_STOCK_ID, A.DIV_REPAY_GB, A.EQUAL_REPAY_AMT, A.UNREDEEM_TERM, A.RPY_TERM_INT_TP_CD, A.DIV_REPAY_CNT,
                             A.STD_YTM_WAY, A.ADDL_INTRT, A.INT_RATE_POINT, A.STD_YTM_MAX, A.STD_YTM_MIN, A.INT_DECI_DAY, A.INT_ACCR_PAY_HLDY_TYPE_CODE,
                             A.MAT_STRUCTURE_CODE, A.COND_CAPT_SECR_TYPE_TP_CD, A.INTEREST_CODE, A.STRIPS_TAG, A.ORIGINAL_ID,
                             A.STRIPS_REMAIN_AMT, A.CPI_TAG, A.REF_CPI, A.INT_DECI_RATE_CODE, A.INT_CALC_TYPE_CODE, A.INT_ACCR_HLDY_TYPE_CODE,
                             A.INT_ACCR_RATE, A.PRCP_ACCR_PAY_HLDY_TYPE_CODE, A.PRCP_ACCR_HLDY_TYPE_CODE, A.PRCP_ACCR_RATE, A.SCBD_INTRT,
                             A.CALL_BEGIN_DAY_1, A.CALL_END_DAY_1, A.CALL_BEGIN_DAY_2, A.CALL_END_DAY_2,
                             A.PUT_BEGIN_DAY_1, A.PUT_END_DAY_1, A.PUT_BEGIN_DAY_2, A.PUT_END_DAY_2,
                             A.ELN_PRCP_PROTECT_RATE, A.PARTICIPATION_RATE, A.ELN_MAX_RETURN, A.KOSCOM_BOND_CLASS, A.ORG_ID
                             
                         FROM
                              (SELECT TO_CHAR(APLY_DT, 'YYYY-MM-DD') DAY, BOND_ID, KOR_BOND_NM, ENG_BOND_NM,
                                      ISSUER_ORG_ID, LIST_TYPE_CODE, BOND_TYPE_CODE, BND_TYPE_CD, SPBN_ISCO_CD, LOCAL_TYPE_CODE,
                                      GUARANTEE_TYPE_CODE, GUARANTEE_RATE, STOCK_TYPE_CODE, OPTION_TYPE_CODE, INT_PAY_TYPE_CODE,
                                      REFUND_TYPE_CODE, ISSUE_TYPE_CODE, ABS_TYPE_CODE, DEFERRED_BOND_TYPE_CODE, ISSUE_DAY,
                                      MAT_DAY, SELL_DAY, FIRST_COUPON_DAY, ISSUE_RATE, COUPON_RATE, INT_PERIOD_MONTH, INT_PAY_TIME_TYPE_CODE,
                                      BND_INT_PAYNT_DD_STD_TP_CD, INT_MME_TP_CD, INT_UNDER_TRUNC_WAY, INT_PAYNT_UNIT_MONS,
                                      SELL_TYPE_CODE, PRE_SELL_INT_TYPE_CODE, PRCP_AMT, BATCH_AMT_CONFIRM_GB,
                                      CURRENCY_CODE, MAT_REFUND_RATE, GUARANTEE_YTM, GUARANTEE_YTM_DAY, ADD_YTM, ADD_YTM_DAY,
                                      EQUIPMENT_PRCP_AMT, OPERATION_PRCP_AMT, REFUND_PRCP_AMT, ETC_PRCP_AMT, NAMED_GB,
                                      TAX_GB, ADMIN_ORG_ID, DECODE(GUARANT_ORG_ID, '0000', NULL, GUARANT_ORG_ID) GUARANT_ORG_ID, CUSTODY_ORG_ID,
                                      REGISTRATION_ORG_ID, PRCP_AGENCY_ORG_ID, SHORT_BOND_ID, CLAIM_TYPE_CODE, CB_STOCK_ID, CB_STOCK_NM,
                                      EXECUTE_PRICE, EXECUTE_RATE, CB_START_DAY, CB_END_DAY, DEMAND_ORG, DIVIDEND_CALC_GB, PB_ACC_GB,
                                      BW_STOCK_ID, DIV_REPAY_GB, EQUAL_REPAY_AMT, UNREDEEM_TERM, RPY_TERM_INT_TP_CD, DIV_REPAY_CNT,
                                      STD_YTM_WAY, ADDL_INTRT, INT_RATE_POINT, STD_YTM_MAX, STD_YTM_MIN, INT_DECI_DAY, INT_ACCR_PAY_HLDY_TYPE_CODE,
                                      MAT_STRUCTURE_CODE, COND_CAPT_SECR_TYPE_TP_CD, INTEREST_CODE, STRIPS_TAG, ORIGINAL_ID,
                                      STRIPS_REMAIN_AMT, CPI_TAG, REF_CPI, INT_DECI_RATE_CODE, INT_CALC_TYPE_CODE, INT_ACCR_HLDY_TYPE_CODE,
                                      INT_ACCR_RATE, PRCP_ACCR_PAY_HLDY_TYPE_CODE, PRCP_ACCR_HLDY_TYPE_CODE, PRCP_ACCR_RATE, SCBD_INTRT,
                                      CALL_BEGIN_DAY_1, CALL_END_DAY_1, CALL_BEGIN_DAY_2, CALL_END_DAY_2,
                                      PUT_BEGIN_DAY_1, PUT_END_DAY_1, PUT_BEGIN_DAY_2, PUT_END_DAY_2,
                                      ELN_PRCP_PROTECT_RATE, PARTICIPATION_RATE, ELN_MAX_RETURN, KOSCOM_BOND_CLASS, ORG_ID, LIST_END_DAY, LIST_ABSH_CAU_CD                                      
                                 FROM BN00TD00
                                WHERE TAG = '1' 
                                  AND NVL(RCRD_PROC_TP_CD, 0) <> 'D' 
                                  AND BOND_ID IN (SELECT BOND_ID
                                                    FROM BN00TD00
                                                   WHERE ((REG_DT = :i_prev_work_day AND KOSCOM_IN_TIME = 'PM')
                                                       OR (REG_DT = :t_day AND KOSCOM_IN_TIME = 'AM'))
                                                     AND TAG = '1'
                                                   UNION
                                                  SELECT BND_ID
                                                    FROM BN00TD13
                                                    WHERE REG_DT = :t_day
                                                      AND TAG = '1')) A,
                              (SELECT BND_ID,
                                      LIST_DT,
                                      LIST_AMT                                      
                                 FROM BN00TD13
                                WHERE TAG = '1' 
                                  AND BND_ID IN (SELECT BOND_ID
                                                   FROM BN00TD00
                                                  WHERE ((REG_DT = :i_prev_work_day AND KOSCOM_IN_TIME = 'PM') 
                                                     OR (REG_DT = :t_day AND KOSCOM_IN_TIME = 'AM'))
                                                    AND TAG = '1'
                                                  UNION
                                                 SELECT BND_ID
                                                   FROM BN00TD13
                                                  WHERE REG_DT = :t_day
                                                    AND TAG = '1')) B
                        WHERE A.BOND_ID = B.BND_ID (+)
]]></sql>.Value

    ''' <summary>
    ''' 'Process_2 : 당일 발행물 발행정보 최종 테이블(PABNTD01)에 입력 : (PABNTD00 SEQ = 0 당일 발행물)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared PBN_LOAD_BOND_INFO_Last As String _
= <sql><![CDATA[

      INSERT INTO PABNTD01 (
					DAY, BOND_ID, RCRD_PROC_TP_CD, KOR_BOND_NM, ENG_BOND_NM, ISSUER_ORG_ID,
                    LIST_TYPE_CODE, BOND_TYPE_CODE, BND_TYPE_CD, SPBN_ISCO_CD, LOCAL_TYPE_CODE ,GUARANTEE_TYPE_CODE,
                    GUARANTEE_RATE, STOCK_TYPE_CODE, OPTION_TYPE_CODE, INT_PAY_TYPE_CODE, REFUND_TYPE_CODE,
                    ISSUE_TYPE_CODE, ABS_TYPE_CODE, DEFERRED_BOND_TYPE_CODE, LIST_DAY, ISSUE_DAY, MAT_DAY, SELL_DAY,
                    FIRST_COUPON_DAY, ISSUE_RATE, COUPON_RATE, INT_PERIOD_MONTH, INT_PAY_TIME_TYPE_CODE,
                    BND_INT_PAYNT_DD_STD_TP_CD, INT_MME_TP_CD, INT_UNDER_TRUNC_WAY, INT_PAYNT_UNIT_MONS,
                    SELL_TYPE_CODE, PRE_SELL_INT_TYPE_CODE, PRCP_AMT, LIST_AMT, BATCH_AMT_CONFIRM_GB, CURRENCY_CODE,
                    MAT_REFUND_RATE, GUARANTEE_YTM, GUARANTEE_YTM_DAY, ADD_YTM, ADD_YTM_DAY, EQUIPMENT_PRCP_AMT,
                    OPERATION_PRCP_AMT, REFUND_PRCP_AMT, ETC_PRCP_AMT, NAMED_GB, TAX_GB, ADMIN_ORG_ID, GUARANT_ORG_ID,
                    CUSTODY_ORG_ID, REGISTRATION_ORG_ID, PRCP_AGENCY_ORG_ID, SHORT_BOND_ID, LIST_ABOLISH_REASON, LIST_END_DAY,
                    CLAIM_TYPE_CODE, CB_STOCK_ID, CB_STOCK_NM, EXECUTE_PRICE, EXECUTE_RATE, CB_START_DAY, CB_END_DAY,
                    DEMAND_ORG, DIVIDEND_CALC_GB, PB_ACC_GB, BW_STOCK_ID, DIV_REPAY_GB, EQUAL_REPAY_AMT, UNREDEEM_TERM,
                    RPY_TERM_INT_TP_CD, DIV_REPAY_CNT, STD_YTM_WAY, ADDL_INTRT, INT_RATE_POINT, STD_YTM_MAX, STD_YTM_MIN, INT_DECI_DAY,
                    CALL_BEGIN_DAY_1, CALL_END_DAY_1, CALL_BEGIN_DAY_2, CALL_END_DAY_2,
                    PUT_BEGIN_DAY_1, PUT_END_DAY_1, PUT_BEGIN_DAY_2, PUT_END_DAY_2,
                    INT_ACCR_PAY_HLDY_TYPE_CODE, MAT_STRUCTURE_CODE, COND_CAPT_SECR_TYPE_TP_CD, INTEREST_CODE, ELN_PRCP_PROTECT_RATE,
                    PARTICIPATION_RATE, ELN_MAX_RETURN, STRIPS_TAG, ORIGINAL_ID, STRIPS_REMAIN_AMT, CPI_TAG, REF_CPI,
                    INT_DECI_RATE_CODE, INT_CALC_TYPE_CODE, INT_ACCR_HLDY_TYPE_CODE, INT_ACCR_RATE,
                    PRCP_ACCR_PAY_HLDY_TYPE_CODE, PRCP_ACCR_HLDY_TYPE_CODE, PRCP_ACCR_RATE, SCBD_INTRT,
                    DIV_REPAY_MONTH, KOSCOM_BOND_CLASS, BOND_DESC, BOND_GROUP_TYPE_CODE, INDEX_EXPT, ORG_ID, JGRTE_ORG_ID, ESG_TYP
				)
             SELECT DAY, BOND_ID, RCRD_PROC_TP_CD, KOR_BOND_NM, ENG_BOND_NM, ISSUER_ORG_ID,
                    LIST_TYPE_CODE, BOND_TYPE_CODE, BND_TYPE_CD, SPBN_ISCO_CD, LOCAL_TYPE_CODE ,GUARANTEE_TYPE_CODE,
                    GUARANTEE_RATE, STOCK_TYPE_CODE, OPTION_TYPE_CODE, INT_PAY_TYPE_CODE, REFUND_TYPE_CODE,
                    ISSUE_TYPE_CODE, ABS_TYPE_CODE, DEFERRED_BOND_TYPE_CODE, LIST_DAY, ISSUE_DAY, MAT_DAY, SELL_DAY,
                    FIRST_COUPON_DAY, ISSUE_RATE, COUPON_RATE, INT_PERIOD_MONTH, INT_PAY_TIME_TYPE_CODE,
                    BND_INT_PAYNT_DD_STD_TP_CD, INT_MME_TP_CD, INT_UNDER_TRUNC_WAY, INT_PAYNT_UNIT_MONS,
                    SELL_TYPE_CODE, PRE_SELL_INT_TYPE_CODE, PRCP_AMT, LIST_AMT, BATCH_AMT_CONFIRM_GB, CURRENCY_CODE,
                    MAT_REFUND_RATE, GUARANTEE_YTM, GUARANTEE_YTM_DAY, ADD_YTM, ADD_YTM_DAY, EQUIPMENT_PRCP_AMT,
                    OPERATION_PRCP_AMT, REFUND_PRCP_AMT, ETC_PRCP_AMT, NAMED_GB, TAX_GB, ADMIN_ORG_ID, GUARANT_ORG_ID,
                    CUSTODY_ORG_ID, REGISTRATION_ORG_ID, PRCP_AGENCY_ORG_ID, SHORT_BOND_ID, LIST_ABOLISH_REASON, LIST_END_DAY,
                    CLAIM_TYPE_CODE, CB_STOCK_ID, CB_STOCK_NM, EXECUTE_PRICE, EXECUTE_RATE, CB_START_DAY, CB_END_DAY,
                    DEMAND_ORG, DIVIDEND_CALC_GB, PB_ACC_GB, BW_STOCK_ID, DIV_REPAY_GB, EQUAL_REPAY_AMT, UNREDEEM_TERM,
                    RPY_TERM_INT_TP_CD, DIV_REPAY_CNT, STD_YTM_WAY, ADDL_INTRT, INT_RATE_POINT, STD_YTM_MAX, STD_YTM_MIN, INT_DECI_DAY,
                    CALL_BEGIN_DAY_1, CALL_END_DAY_1, CALL_BEGIN_DAY_2, CALL_END_DAY_2,
                    PUT_BEGIN_DAY_1, PUT_END_DAY_1, PUT_BEGIN_DAY_2, PUT_END_DAY_2,
                    INT_ACCR_PAY_HLDY_TYPE_CODE, MAT_STRUCTURE_CODE, COND_CAPT_SECR_TYPE_TP_CD, INTEREST_CODE, ELN_PRCP_PROTECT_RATE,
                    PARTICIPATION_RATE, ELN_MAX_RETURN, STRIPS_TAG, ORIGINAL_ID, STRIPS_REMAIN_AMT, CPI_TAG, REF_CPI,
                    INT_DECI_RATE_CODE, INT_CALC_TYPE_CODE, INT_ACCR_HLDY_TYPE_CODE, INT_ACCR_RATE,
                    PRCP_ACCR_PAY_HLDY_TYPE_CODE, PRCP_ACCR_HLDY_TYPE_CODE, PRCP_ACCR_RATE, SCBD_INTRT,
                    DIV_REPAY_MONTH, KOSCOM_BOND_CLASS, BOND_DESC, BOND_GROUP_TYPE_CODE, INDEX_EXPT, ORG_ID, JGRTE_ORG_ID, ESG_TYP
               FROM PABNTD00
              WHERE IN_DAY = :t_day
                AND SEQ = 0

]]></sql>.Value

    ''' <summary>
    ''' BATCH-LOAD의 작업건수를 확인한다.
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared LOAD_BATCH_COUNT As String _
= <sql><![CDATA[
            Select COUNT(*) as count
          FROM PABNTD00
         WHERE IN_DAY = :t_day
]]></sql>.Value


    ''' <summary>
    ''' BATCH 당발의 작업건수를 확인한다.
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared LOAD_BATCH_Today_COUNT As String _
= <sql><![CDATA[
            SELECT COUNT(*) as count
          FROM PABNTD00
         WHERE IN_DAY = :t_day
           AND SEQ = '000'
]]></sql>.Value

    ''' <summary>
    ''' BN00TD00와 PABNTD01 테이블에서 평가에 영향을 주지 않는 발행정보 추출 
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared EXTRACT_NONIMPACT_DATA As String _
= <sql><![CDATA[
SELECT A.BOND_ID  as bond_id
    , A.BOND_TYPE_CODE  as bond_type_code
    , A.LOCAL_TYPE_CODE as local_type_code
    , A.ABS_TYPE_CODE as abs_type_code
    , A.FIRST_COUPON_DAY as first_coupon_day
    , A.BATCH_AMT_CONFIRM_GB as batch_amt_confirm_gb
    , A.NAMED_GB as named_gb
    , A.ADMIN_ORG_ID as admin_org_id
    , A.GUARANT_ORG_ID as guarant_org_id
    , A.CUSTODY_ORG_ID as custody_org_id
    , A.REGISTRATION_ORG_ID as registration_org_id
    , A.PRCP_AGENCY_ORG_ID as prcp_agency_org_id                
    , A.INT_CALC_TYPE_CODE as int_calc_type_code
    , A.INT_ACCR_PAY_HLDY_TYPE_CODE as int_accr_pay_hldy_type_code
    , A.INT_ACCR_HLDY_TYPE_CODE as int_accr_hldy_type_code
    , A.INT_ACCR_RATE as int_accr_rate
    , A.PRCP_ACCR_PAY_HLDY_TYPE_CODE as prcp_accr_pay_hldy_type_code
    , A.PRCP_ACCR_HLDY_TYPE_CODE as prcp_accr_hldy_type_code
    , A.PRCP_ACCR_RATE as prcp_accr_rate
    , A.ISSUER_ORG_ID                    
 FROM BN00TD00 A 
    , PABNTD01 B
WHERE A.BOND_ID = B.BOND_ID
  AND A.REG_DT = :t_day
  AND A.TAG = '1' 
  AND NVL(A.RCRD_PROC_TP_CD, 0) <> 'D' 
  AND (B.BOND_ID IS NULL
    OR B.BOND_TYPE_CODE <> A.BOND_TYPE_CODE
    OR B.LOCAL_TYPE_CODE <> A.LOCAL_TYPE_CODE
    OR B.ABS_TYPE_CODE <> A.ABS_TYPE_CODE
    OR B.FIRST_COUPON_DAY <> A.FIRST_COUPON_DAY
    OR B.BATCH_AMT_CONFIRM_GB <> A.BATCH_AMT_CONFIRM_GB
    OR B.NAMED_GB <> A.NAMED_GB
    OR B.ADMIN_ORG_ID <> A.ADMIN_ORG_ID
    OR B.GUARANT_ORG_ID <> A.GUARANT_ORG_ID
    OR B.CUSTODY_ORG_ID <> A.CUSTODY_ORG_ID
    OR B.REGISTRATION_ORG_ID <> A.REGISTRATION_ORG_ID
    OR B.PRCP_AGENCY_ORG_ID <> A.PRCP_AGENCY_ORG_ID                    
    OR B.INT_CALC_TYPE_CODE <> A.INT_CALC_TYPE_CODE
    OR B.INT_ACCR_PAY_HLDY_TYPE_CODE <> A.INT_ACCR_PAY_HLDY_TYPE_CODE
    OR B.INT_ACCR_HLDY_TYPE_CODE <> A.INT_ACCR_HLDY_TYPE_CODE
    OR B.INT_ACCR_RATE <> A.INT_ACCR_RATE
    OR B.PRCP_ACCR_PAY_HLDY_TYPE_CODE <> A.PRCP_ACCR_PAY_HLDY_TYPE_CODE
    OR B.PRCP_ACCR_HLDY_TYPE_CODE <> A.PRCP_ACCR_HLDY_TYPE_CODE
    OR B.PRCP_ACCR_RATE <> A.PRCP_ACCR_RATE
    OR B.ISSUER_ORG_ID <> A.ISSUER_ORG_ID)

]]></sql>.Value

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared COPY_BOND_INFO_UPDATE As String _
= <sql><![CDATA[
            UPDATE PABNTD01
               SET BOND_TYPE_CODE               = :i_bond_type_code
                 , LOCAL_TYPE_CODE              = :i_local_type_code
                 , ABS_TYPE_CODE                = :i_abs_type_code
                 , FIRST_COUPON_DAY             = :i_first_coupon_day
                 , BATCH_AMT_CONFIRM_GB         = :i_batch_amt_confirm_gb
                 , NAMED_GB                     = :i_named_gb
                 , ADMIN_ORG_ID                 = :i_admin_org_id
                 , GUARANT_ORG_ID               = :i_guarant_org_id
                 , CUSTODY_ORG_ID               = :i_custody_org_id
                 , REGISTRATION_ORG_ID          = :i_registration_org_id
                 , PRCP_AGENCY_ORG_ID           = :i_prcp_agency_org_id
                 , INT_CALC_TYPE_CODE           = :i_int_calc_type_code
                 , INT_ACCR_PAY_HLDY_TYPE_CODE  = :i_int_accr_pay_hldy_type_code
                 , INT_ACCR_HLDY_TYPE_CODE      = :i_int_accr_hldy_type_code
                 , INT_ACCR_RATE                = :i_int_accr_rate
                 , PRCP_ACCR_PAY_HLDY_TYPE_CODE = :i_prcp_accr_pay_hldy_type_code
                 , PRCP_ACCR_HLDY_TYPE_CODE     = :i_prcp_accr_hldy_type_code
                 , PRCP_ACCR_RATE               = :i_prcp_accr_rate
                 , ISSUER_ORG_ID                = :i_issuer_org_id                  
             WHERE BOND_ID = :i_bond_id
]]></sql>.Value



    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared COPY_BOND_INFO_INSERT As String _
= <sql><![CDATA[
INSERT INTO PABNTD00 (SEQ, IN_DAY, IN_DAY_TIME, IN_ID, DAY
                        , BOND_ID, KOR_BOND_NM, ENG_BOND_NM, ISSUER_ORG_ID, LIST_TYPE_CODE
                        , BOND_TYPE_CODE, BND_TYPE_CD, SPBN_ISCO_CD, LOCAL_TYPE_CODE, GUARANTEE_TYPE_CODE
                        , GUARANTEE_RATE, STOCK_TYPE_CODE, OPTION_TYPE_CODE, INT_PAY_TYPE_CODE, REFUND_TYPE_CODE
                        , ISSUE_TYPE_CODE, ABS_TYPE_CODE, DEFERRED_BOND_TYPE_CODE, LIST_DAY, ISSUE_DAY
                        , MAT_DAY, SELL_DAY, FIRST_COUPON_DAY, ISSUE_RATE, COUPON_RATE
                        , INT_PERIOD_MONTH, INT_PAY_TIME_TYPE_CODE, BND_INT_PAYNT_DD_STD_TP_CD, INT_MME_TP_CD, INT_UNDER_TRUNC_WAY
                        , INT_PAYNT_UNIT_MONS, SELL_TYPE_CODE, PRE_SELL_INT_TYPE_CODE, PRCP_AMT, LIST_AMT
                        , BATCH_AMT_CONFIRM_GB, CURRENCY_CODE, MAT_REFUND_RATE, GUARANTEE_YTM, GUARANTEE_YTM_DAY
                        , ADD_YTM, ADD_YTM_DAY, EQUIPMENT_PRCP_AMT, OPERATION_PRCP_AMT, REFUND_PRCP_AMT
                        , ETC_PRCP_AMT, NAMED_GB, TAX_GB, ADMIN_ORG_ID, GUARANT_ORG_ID
                        , CUSTODY_ORG_ID, REGISTRATION_ORG_ID, PRCP_AGENCY_ORG_ID, SHORT_BOND_ID, LIST_ABOLISH_REASON
                        , LIST_END_DAY, CLAIM_TYPE_CODE, CB_STOCK_ID, CB_STOCK_NM, EXECUTE_PRICE
                        , EXECUTE_RATE, CB_START_DAY, CB_END_DAY, DEMAND_ORG, DIVIDEND_CALC_GB
                        , PB_ACC_GB, BW_STOCK_ID, DIV_REPAY_GB, EQUAL_REPAY_AMT, UNREDEEM_TERM
                        , RPY_TERM_INT_TP_CD, DIV_REPAY_CNT, STD_YTM_WAY, ADDL_INTRT, INT_RATE_POINT
                        , STD_YTM_MAX, STD_YTM_MIN, INT_DECI_DAY, CALL_BEGIN_DAY_1, CALL_END_DAY_1
                        , CALL_BEGIN_DAY_2, CALL_END_DAY_2, PUT_BEGIN_DAY_1, PUT_END_DAY_1, PUT_BEGIN_DAY_2
                        , PUT_END_DAY_2, INT_ACCR_PAY_HLDY_TYPE_CODE, MAT_STRUCTURE_CODE, COND_CAPT_SECR_TYPE_TP_CD, INTEREST_CODE
                        , ELN_PRCP_PROTECT_RATE, PARTICIPATION_RATE, ELN_MAX_RETURN, STRIPS_TAG, ORIGINAL_ID
                        , STRIPS_REMAIN_AMT, CPI_TAG, REF_CPI, INT_DECI_RATE_CODE, INT_CALC_TYPE_CODE
                        , INT_ACCR_HLDY_TYPE_CODE, INT_ACCR_RATE, PRCP_ACCR_PAY_HLDY_TYPE_CODE, PRCP_ACCR_HLDY_TYPE_CODE, PRCP_ACCR_RATE
                        , SCBD_INTRT, DIV_REPAY_MONTH, KOSCOM_BOND_CLASS, BOND_DESC, BOND_GROUP_TYPE_CODE
                        , INDEX_EXPT, ORG_ID, JGRTE_ORG_ID, ESG_TYP)           
                    SELECT B.SEQ + 1 SEQ, TO_DATE(TO_CHAR(SYSDATE, 'YYYY-MM-DD')) IN_DAY, SYSDATE IN_DAY_TIME, 'KOSCOM_AUTO' IN_ID, DAY
                        , BOND_ID, KOR_BOND_NM, ENG_BOND_NM, ISSUER_ORG_ID, LIST_TYPE_CODE
                        , BOND_TYPE_CODE, BND_TYPE_CD, SPBN_ISCO_CD, LOCAL_TYPE_CODE, GUARANTEE_TYPE_CODE
                        , GUARANTEE_RATE, STOCK_TYPE_CODE, OPTION_TYPE_CODE, INT_PAY_TYPE_CODE, REFUND_TYPE_CODE
                        , ISSUE_TYPE_CODE, ABS_TYPE_CODE, DEFERRED_BOND_TYPE_CODE, LIST_DAY, ISSUE_DAY
                        , MAT_DAY, SELL_DAY, FIRST_COUPON_DAY, ISSUE_RATE, COUPON_RATE
                        , INT_PERIOD_MONTH, INT_PAY_TIME_TYPE_CODE, BND_INT_PAYNT_DD_STD_TP_CD, INT_MME_TP_CD, INT_UNDER_TRUNC_WAY
                        , INT_PAYNT_UNIT_MONS, SELL_TYPE_CODE, PRE_SELL_INT_TYPE_CODE, PRCP_AMT, LIST_AMT
                        , BATCH_AMT_CONFIRM_GB, CURRENCY_CODE, MAT_REFUND_RATE, GUARANTEE_YTM, GUARANTEE_YTM_DAY
                        , ADD_YTM, ADD_YTM_DAY, EQUIPMENT_PRCP_AMT, OPERATION_PRCP_AMT, REFUND_PRCP_AMT
                        , ETC_PRCP_AMT, NAMED_GB, TAX_GB, ADMIN_ORG_ID, GUARANT_ORG_ID
                        , CUSTODY_ORG_ID, REGISTRATION_ORG_ID, PRCP_AGENCY_ORG_ID, SHORT_BOND_ID, LIST_ABOLISH_REASON
                        , LIST_END_DAY, CLAIM_TYPE_CODE, CB_STOCK_ID, CB_STOCK_NM, EXECUTE_PRICE
                        , EXECUTE_RATE, CB_START_DAY, CB_END_DAY, DEMAND_ORG, DIVIDEND_CALC_GB
                        , PB_ACC_GB, BW_STOCK_ID, DIV_REPAY_GB, EQUAL_REPAY_AMT, UNREDEEM_TERM
                        , RPY_TERM_INT_TP_CD, DIV_REPAY_CNT, STD_YTM_WAY, ADDL_INTRT, INT_RATE_POINT
                        , STD_YTM_MAX, STD_YTM_MIN, INT_DECI_DAY, CALL_BEGIN_DAY_1, CALL_END_DAY_1
                        , CALL_BEGIN_DAY_2, CALL_END_DAY_2, PUT_BEGIN_DAY_1, PUT_END_DAY_1, PUT_BEGIN_DAY_2
                        , PUT_END_DAY_2, INT_ACCR_PAY_HLDY_TYPE_CODE, MAT_STRUCTURE_CODE, COND_CAPT_SECR_TYPE_TP_CD, INTEREST_CODE
                        , ELN_PRCP_PROTECT_RATE, PARTICIPATION_RATE, ELN_MAX_RETURN, STRIPS_TAG, ORIGINAL_ID
                        , STRIPS_REMAIN_AMT, CPI_TAG, REF_CPI, INT_DECI_RATE_CODE, INT_CALC_TYPE_CODE
                        , INT_ACCR_HLDY_TYPE_CODE, INT_ACCR_RATE, PRCP_ACCR_PAY_HLDY_TYPE_CODE, PRCP_ACCR_HLDY_TYPE_CODE, PRCP_ACCR_RATE
                        , SCBD_INTRT, DIV_REPAY_MONTH, KOSCOM_BOND_CLASS, BOND_DESC, BOND_GROUP_TYPE_CODE
                        , INDEX_EXPT, ORG_ID, JGRTE_ORG_ID, ESG_TYP
                        FROM PABNTD01 A
                        , (SELECT NVL(MAX(SEQ), -1) SEQ
                            FROM PABNTD00
                            WHERE BOND_ID = :i_bond_id) B
                    WHERE A.BOND_ID = :i_bond_id
]]></sql>.Value

    ''' <summary>
    ''' 최종호가수익률 INSERT
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared InsertLstAkPrcValue As String _
= <sql><![CDATA[

INSERT INTO SB_LSTAKPRCERNRP 
    (PANC_DT,
     PANC_TIME, 
     PROC_TP_CD,
     IN_SEQ_TP_CD,
     PANC_KND_TP_CD,
     PACN_STK_CD,
     LST_AKPRC_ERNR,
     LST_YN,
     REG_DTM,
     REGR_ID, 
     MDFY_DTM,
     MDFYR_ID)
VALUES (
    :panc_dt,
    :panc_time, 
    :proc_tp_cd, 
    :in_seq_tp_cd,
    :panc_knd_tp_cd,
    :pacn_stk_cd,
    :lst_akprc_ernr,
    :lst_yn,
    SYSDATE,
    'KOSCOM/UDP_45',
    SYSDATE,
    'KOSCOM/UDP_45'
)
]]></sql>.Value

    ''' <summary>
    ''' 최종호가수익률 UPDATE (정정 발생의 경우 LST_YN 변경)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared UpdateLstAkPrcValue As String _
= <sql><![CDATA[

UPDATE SB_LSTAKPRCERNRP
SET LST_YN = '0'
WHERE PANC_DT = :panc_dt
AND IN_SEQ_TP_CD = :in_seq_tp_cd
AND PANC_KND_TP_CD = :panc_knd_tp_cd
AND PACN_STK_CD = :pacn_stk_cd
AND LST_YN = :lst_yn

]]></sql>.Value


    ''' <summary>
    ''' 값 여부 확인 
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared checkLstAkPrcValue As String _
= <sql><![CDATA[

SELECT *
FROM SB_LSTAKPRCERNRP
WHERE 1 = 1
AND PANC_DT = :panc_dt 
AND PACN_STK_CD = :pacn_stk_cd
]]></sql>.Value

    ''' <summary>
    ''' 오전 선택시 이전 영업일 조회
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared GRIDSET As String _
= <sql><![CDATA[
SELECT '1' AS CHK_RCV
    , NULL AS RECEIVE_TIME
    , NULL AS COMPLETE
FROM DUAL             
WHERE 1 <> 1
]]></sql>.Value

    ''' <summary>
    ''' 영업일인지 확인
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared todayIsWorkDayCheck As String _
= <sql><![CDATA[
SELECT TYPE
  FROM SY99TD02
 WHERE 1 = 1
   AND DAY= :t_day
]]></sql>.Value


#End Region

End Class
