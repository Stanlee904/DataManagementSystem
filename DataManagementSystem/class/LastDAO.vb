Public Class LastDAO

#Region "변수 선언"

    '장외채권최종호가수익률 관련 변수
    Private _PANC_DT As String '공시일자
    Private _PANC_TIME As String '공시시간
    Private _PROC_TP_CD As String '처리구분코드
    Private _IN_SEQ_TP_CD As String '입력회차구분코드
    Private _LST_AKPRC_ERNR As String '최종호가수익률
    Private _PACN_STK_CD As String '공시종목코드
    Private _PANC_KND_TP_CD As String '공시종류구분코드
    Private _REGR_ID As String '등록자ID
    Private _MDFYR_ID As String '수정자ID
    Private _tempLine As String '임시로 확인하는 라인텍스트
    Private _temp_length As String '데이터 라인 당 길이 확인 변수
    Private _j5077_Server_file As String '26번 서버의 j5077 파일 경로
    Private _j5077_File As String '26번 서버의 j5077 파일 경로

#End Region

#Region "장외채권최종호가수익률 property 선언"

    Property PANC_DT As String
        Get
            Return Me._PANC_DT
        End Get
        Set(value As String)
            Me._PANC_DT = value
        End Set
    End Property
    Property PANC_TIME As String
        Get
            Return Me._PANC_TIME
        End Get
        Set(value As String)
            Me._PANC_TIME = value
        End Set
    End Property
    Property PROC_TP_CD As String
        Get
            Return Me._PROC_TP_CD
        End Get
        Set(value As String)
            Me._PROC_TP_CD = value
        End Set
    End Property

    Property IN_SEQ_TP_CD As String
        Get
            Return Me._IN_SEQ_TP_CD
        End Get
        Set(value As String)
            Me._IN_SEQ_TP_CD = value
        End Set
    End Property

    Property LST_AKPRC_ERNR As String
        Get
            Return Me._LST_AKPRC_ERNR
        End Get
        Set(value As String)
            Me._LST_AKPRC_ERNR = value
        End Set
    End Property

    Property PANC_KND_TP_CD As String
        Get
            Return Me._PANC_KND_TP_CD
        End Get
        Set(value As String)
            Me._PANC_KND_TP_CD = value
        End Set
    End Property

    Property PACN_STK_CD As String
        Get
            Return Me._PACN_STK_CD
        End Get
        Set(value As String)
            Me._PACN_STK_CD = value
        End Set
    End Property

    Property REGR_ID As String
        Get
            Return Me._REGR_ID
        End Get
        Set(value As String)
            Me._REGR_ID = value
        End Set
    End Property

    Property MDFYR_ID As String
        Get
            Return Me._MDFYR_ID
        End Get
        Set(value As String)
            Me._MDFYR_ID = value
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

    Property j5077_Server_fileLocation As String
        Get
            Return Me._j5077_Server_file
        End Get
        Set(value As String)
            Me._j5077_Server_file = value
        End Set
    End Property

    Property j5077_File As String
        Get
            Return Me._j5077_File
        End Get
        Set(value As String)
            Me._j5077_File = value
        End Set
    End Property

    Property temp_length As String
        Get
            Return Me._temp_length
        End Get
        Set(value As String)
            Me._temp_length = value
        End Set
    End Property
#End Region
End Class
