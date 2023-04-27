'Imports Oracle.ManagedDataAccess.Client 'ODP.NET Oracle managed provider\
Imports System.Transactions
Imports System.Configuration
Imports Oracle.DataAccess.Client ' ODP.NET Oracle managed provider
Imports Oracle.DataAccess.Types

Public Class Agent
    ''' <summary>
    ''' 오라클 DB와의 통신 담당 클래스
    ''' </summary>
    ''' <remarks></remarks>
    ' 프로그램 시작부터 종료시까지 실행되는 동안 Oracle DB 연결 
    Private Shared m_Conn As OracleConnection

    ' 트랜잭션 기능 구현 추가
    ' 2014-12-26 swpark : 추후 Transaction적용 시, Private으로 바꿔야 
    Private m_Trans As OracleTransaction

#Region "생성자"

    Sub New()

    End Sub

    ''' <summary>
    ''' Constructor
    '''  - 2014.09.23 박병조: 환경변수에 NLS_LANG을 자동으로 등록하게 수정
    ''' </summary>
    ''' <remarks>공유 생성자. Private Shared인 m_Conn 멤버변수는 이 함수에서 단 한번만 Connect된 후 cmBasic.BIZ_Agent.closeConnection()함수를 호출하기 전 까지 Connection이 유지됨</remarks>
    '''  GN 
    Shared Sub New()
        '개발계
        'Dim niceDB As String = "Data Source=(DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST = 10.0.1.41)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = DEVDB)));User Id=PDV;Password=pwd0719;"
        'Dim niceDB As String = "Data Source=(DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST = 10.0.1.41)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = DEVDB)));User Id=nps;Password=nps1260;"


        '운영계
        Dim niceDB As String = "Data Source=(DESCRIPTION =(ADDRESS_LIST =(ADDRESS = (PROTOCOL = TCP)(HOST = 10.0.1.30)(PORT = 1521)))(CONNECT_DATA =(SERVICE_NAME = PNIDB)));User Id=nice;Password=nps1260;"

        Try
            Environment.SetEnvironmentVariable("NLS_LANG", "KOREAN_KOREA.KO16KSC5601")
            m_Conn = New OracleConnection(niceDB) 'DB 연결
            m_Conn.Open()
        Catch ex As Exception
            Throw New Exception("DB 연결중에 오류가 발생했습니다.")
        End Try
    End Sub

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="bUseTransaction">트랜잭션 사용</param>
    ''' <remarks></remarks>
    Sub New(ByVal bUseTransaction As Boolean)
        Try
            If bUseTransaction Then
                m_Trans = m_Conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted)
            End If
        Catch ex As Exception
            Throw New Exception("Begin Transaction Failed.")
        End Try
    End Sub

#End Region

#Region "Connection Function"
    ''' <summary>
    ''' Connection 조회
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function getConn() As OracleConnection
        Return m_Conn
    End Function

    ''' <summary>
    ''' Open Connection
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub openConn()
        m_Conn.Open()
    End Sub

    ''' <summary>
    ''' Close Connection
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub closeConn()
        m_Conn.Close()
    End Sub

    ''' <summary>
    ''' DB 연결 종료
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub endConn()
        m_Conn.Dispose()
        m_Conn = Nothing
    End Sub

#End Region

#Region "DCL 실행"

    ''' <summary>
    ''' 트랜잭션 설정 함수
    ''' </summary>
    ''' <returns>0:정상처리, -1:오류</returns>
    ''' <remarks></remarks>
    Public Function setTrans(ByVal bTransUse As Boolean) As Integer
        Try
            If bTransUse Then
                m_Trans = m_Conn.BeginTransaction(System.Data.IsolationLevel.ReadCommitted)
            Else
                m_Trans.Dispose()
                m_Trans = Nothing
            End If
        Catch ex As Exception
            Throw ex
            Return -1
        End Try
        Return 0
    End Function

    ''' <summary>
    ''' Transaction Commit
    ''' </summary>
    ''' <returns>0:정상처리, -1:오류, 1:m_Trans가 Nothing</returns>
    ''' <remarks></remarks>
    Public Function commitTrans() As Integer
        Try
            If Not IsNothing(m_Trans) Then
                m_Trans.Commit()

            Else
                Return 1
            End If
        Catch ex As Exception
            Throw ex
            Return -1
        End Try
        Return 0
    End Function

    ''' <summary>
    ''' Transaction Rollback
    ''' </summary>
    ''' <returns>0:정상처리, -1:오류, 1:m_Trans가 Nothing</returns>
    ''' <remarks></remarks>
    Public Function rollbackTrans() As Integer
        Try
            If Not IsNothing(m_Trans) Then
                m_Trans.Rollback()
            Else
                Return 1
            End If
        Catch ex As Exception
            Throw ex
            Return -1
        End Try
        Return 0
    End Function
#End Region
#Region "DML 실행"

    ''' <summary>
    ''' 쿼리를 실행하고 쿼리에서 반환된 결과 집합의 첫 번째 행의 첫 번째 열을 반환. 추가 열이나 행은 무시.
    ''' </summary>
    ''' <param name="sSQL">실행할 SQL 문</param>
    ''' <param name="sParams">바인드 변수에 할당될 인수 배열</param>
    ''' <param name="bUseVariables">오라클 바인드 변수 사용여부</param>
    ''' <returns>결과 집합의 첫 행의 첫 열 또는 결과 집합이 비어있을 경우 Nothing</returns>
    ''' <remarks></remarks>
    Public Function selectScalar(ByRef sSQL As String, Optional ByVal sParams() As String = Nothing, Optional ByVal bUseVariables As Boolean = False) As String
        Dim sReturnValue As String = Nothing
        Dim OraDR As OracleDataReader = Nothing

        Using cmd As New OracleCommand()
            Try
                With cmd
                    .Connection = m_Conn
                    .CommandText = sSQL
                    .CommandType = CommandType.Text
                    .BindByName = True
                End With

                If bUseVariables AndAlso Not IsNothing(sParams) Then
                    For index = 0 To sParams.Length - 1
                        If Not IsNothing(sParams(index)) Then    'And sParams(index) <> "" Then
                            cmd.Parameters.Add(":param" + index.ToString(), sParams(index))
                        End If
                    Next
                End If

                OraDR = cmd.ExecuteReader()
                If OraDR.Read() Then
                    sReturnValue = OraDR.Item(0).ToString()
                End If
            Catch oraEx As OracleException ' catches only Oracle errors
                Throw New Exception("[DB오류] " + oraEx.Message)
            Catch ex As Exception
                Throw ex
            Finally
                If IsNothing(OraDR) = False Then OraDR.Close()
            End Try
        End Using
        Return sReturnValue
    End Function

    ''' <summary>
    ''' 쿼리를 실행하고 쿼리에서 반환된 결과 집합의 첫 번째 행의 첫 번째 열을 반환. 추가 열이나 행은 무시.
    ''' </summary>
    ''' <param name="sSQL">실행할 SQL 문</param>
    ''' <param name="dicParams">매개변수에 대한 변수명-값 Dictionary</param>
    ''' <param name="bUseVariables">오라클 바인드 변수 사용여부</param>
    ''' <returns>결과 집합의 첫 행의 첫 열 또는 결과 집합이 비어있을 경우 Nothing</returns>
    ''' <remarks></remarks>
    Public Function selectScalar_dicParams(ByRef sSQL As String, Optional ByVal dicParams As Dictionary(Of String, String) = Nothing, Optional ByVal bUseVariables As Boolean = False) As String
        Dim sReturnValue As String = Nothing
        Dim OraDR As OracleDataReader = Nothing

        Using cmd As New OracleCommand()
            Try
                With cmd
                    .Connection = m_Conn
                    .CommandText = sSQL
                    .CommandType = CommandType.Text
                    .BindByName = True
                End With

                If bUseVariables AndAlso Not IsNothing(dicParams) Then
                    Dim pair As KeyValuePair(Of String, String)

                    For Each pair In dicParams
                        cmd.Parameters.Add(pair.Key, pair.Value)
                    Next
                End If

                OraDR = cmd.ExecuteReader()
                If OraDR.Read() Then
                    sReturnValue = OraDR.Item(0).ToString()
                End If
            Catch oraEx As OracleException ' catches only Oracle errors
                Throw New Exception("[DB오류] " + oraEx.Message)
            Catch ex As Exception
                Throw ex
            Finally
                If IsNothing(OraDR) = False Then OraDR.Close()
            End Try
        End Using
        Return sReturnValue
    End Function

    ''' <summary>
    ''' 쿼리를 실행하고 쿼리에서 반환된 결과 집합 반환
    ''' </summary>
    ''' <param name="sSQL">실행할 SQL 문</param>
    ''' <param name="sParams">바인드 변수에 할당될 인수 배열</param>
    ''' <param name="bUseVariables">오라클 바인드 변수 사용여부</param>
    ''' <returns>쿼리를 실행하고 쿼리에서 반환된 결과 집합. 비어있을 경우 Nothing</returns>
    ''' <remarks></remarks>
    Public Function selectData(ByRef sSQL As String, Optional ByVal sParams() As String = Nothing, Optional ByVal bUseVariables As Boolean = False) As System.Data.DataTable
        Dim dtReturn As DataTable = Nothing

        Using cmd As New OracleCommand()
            Try
                With cmd
                    .Connection = m_Conn
                    .CommandText = sSQL
                    .CommandType = CommandType.Text
                    .BindByName = True
                End With

                If bUseVariables AndAlso Not IsNothing(sParams) Then
                    For index = 0 To sParams.Length - 1
                        If Not IsNothing(sParams(index)) Then    ' And sParams(index) <> "" Then
                            cmd.Parameters.Add(":param" + index.ToString(), sParams(index))
                        End If
                    Next
                End If

                Dim da As OracleDataAdapter = New OracleDataAdapter(cmd)
                Dim ds As DataSet = New DataSet()

                da.Fill(ds)
                dtReturn = ds.Tables(0)
            Catch oraEx As OracleException ' catches only Oracle errors
                Throw New Exception("[DB오류] " + oraEx.Message)
            Catch ex As Exception
                Throw ex
            Finally

            End Try
        End Using
        Return dtReturn
    End Function

    ''' <summary>
    ''' 쿼리를 실행하고 쿼리에서 반환된 결과 집합 반환
    ''' </summary>
    ''' <param name="sSQL">실행할 SQL 문</param>
    ''' <param name="dicParams">매개변수에 대한 변수명-값 Dictionary</param>
    ''' <param name="bUseVariables">오라클 바인드 변수 사용여부</param>
    ''' <returns>쿼리를 실행하고 쿼리에서 반환된 결과 집합. 비어있을 경우 Nothing</returns>
    ''' <remarks></remarks>
    Public Function selectData2(ByRef sSQL As String,
                                         Optional ByVal dicParams As Dictionary(Of String, String) = Nothing,
                                         Optional ByVal bUseVariables As Boolean = False) As System.Data.DataTable
        Dim dtReturn As DataTable = Nothing

        Using cmd As New OracleCommand()
            Try
                With cmd
                    .Connection = m_Conn
                    .CommandText = sSQL
                    .CommandType = CommandType.Text
                    .BindByName = True
                End With

                If bUseVariables AndAlso Not IsNothing(dicParams) Then
                    Dim pair As KeyValuePair(Of String, String)

                    For Each pair In dicParams
                        cmd.Parameters.Add(pair.Key, pair.Value)
                    Next
                End If

                Dim da As OracleDataAdapter = New OracleDataAdapter(cmd)
                Dim ds As DataSet = New DataSet()

                da.Fill(ds)
                dtReturn = ds.Tables(0)

            Catch oraEx As OracleException ' catches only Oracle errors
                Throw New Exception("[DB오류] " + oraEx.Message)
            Catch ex As Exception
                Throw ex
            Finally

            End Try
        End Using
        Return dtReturn
    End Function

    ''' <summary>
    ''' SQL 문을 실행하고 영향을 받는 행의 수를 반환
    ''' </summary>
    ''' <param name="sSQL">실행할 SQL 문</param>
    ''' <param name="sParams">바인드 변수에 할당될 인수 배열</param>
    ''' <param name="bUseVariables">오라클 바인드 변수 사용여부</param>
    ''' <returns>영향 받는 행의 수</returns>
    ''' <remarks></remarks>

    Public Function execute(ByRef sSQL As String, Optional ByVal sParams() As String = Nothing, Optional ByVal bUseVariables As Boolean = False, Optional ByVal bAutoCommit As Boolean = True) As Integer
        Dim iReturn As Integer = -1

        Using cmd As New OracleCommand()
            Try
                With cmd
                    .Connection = m_Conn
                    .CommandText = sSQL
                    .CommandType = CommandType.Text
                    .BindByName = True

                    '트랜잭션 기능 구현 추가
                    If Not bAutoCommit And Not IsNothing(m_Trans) Then
                        .Transaction = m_Trans
                    End If
                End With

                If bUseVariables AndAlso Not IsNothing(sParams) Then
                    cmd.Parameters.Clear()
                    For index = 0 To sParams.Length - 1
                        If Not IsNothing(sParams(index)) Then
                            cmd.Parameters.Add(":param" + CStr(index), sParams(index))
                        End If
                    Next

                End If

                iReturn = cmd.ExecuteNonQuery()
            Catch oraEx As OracleException ' catches only Oracle errors
                Throw New Exception("[DB오류] " + oraEx.Message)
            Catch ex As Exception
                Throw ex
            Finally

            End Try
        End Using

        Return iReturn
    End Function

    ''' <summary>
    ''' SQL 문을 실행하고 영향을 받는 행의 수를 반환
    ''' </summary>
    ''' <param name="sSQL">실행할 SQL 문</param>
    ''' <param name="dicParams">>매개변수에 대한 변수명-값 Dictionary</param>
    ''' <param name="bUseVariables">오라클 바인드 변수 사용여부</param>
    ''' <returns>영향 받는 행의 수</returns>
    ''' <remarks></remarks>
    ''' 
    Public Function execute2(ByRef sSQL As String, Optional ByVal dicParams As Dictionary(Of String, String) = Nothing, Optional ByVal bUseVariables As Boolean = False, Optional ByVal bAutoCommit As Boolean = True) As Integer
        Dim iReturn As Integer = -1

        Using cmd As New OracleCommand()
            Try
                With cmd
                    .Connection = m_Conn
                    .CommandText = sSQL
                    .CommandType = CommandType.Text
                    .BindByName = True

                    '트랜잭션 기능 구현 추가
                    If Not bAutoCommit And Not IsNothing(m_Trans) Then
                        .Transaction = m_Trans
                    End If
                End With

                If bUseVariables AndAlso Not IsNothing(dicParams) Then
                    Dim pair As KeyValuePair(Of String, String)

                    For Each pair In dicParams
                        cmd.Parameters.Add(pair.Key, pair.Value)
                    Next
                End If

                iReturn = cmd.ExecuteNonQuery()
            Catch oraEx As OracleException ' catches only Oracle errors
                Throw New Exception("[DB오류] " + oraEx.Message)
            Catch ex As Exception
                Throw ex
            Finally

            End Try
        End Using

        Return iReturn
    End Function

#End Region

End Class
