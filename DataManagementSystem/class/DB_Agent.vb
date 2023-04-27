Public Class DB_Agent

    Public m_DBAgent As New Agent()

#Region "Connection 함수"

    ''' <summary>
    ''' DB 연결 객체 소멸자
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub endConn()
        Agent.endConn()
    End Sub


    ''' <summary>
    ''' Check Connection
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function checkConn() As Data.ConnectionState
        Return Agent.getConn.State
    End Function


    Public Shared Function getServiceName() As String
        Return Agent.getConn.ServiceName
    End Function

    ''' <summary>
    ''' Close Connection
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub openConn()
        Agent.openConn()
    End Sub

    ''' <summary>
    ''' Close Connection
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub closeConn()
        Agent.closeConn()
    End Sub

#End Region

#Region "Transaction 함수"

    ''' <summary>
    ''' 트랜잭션을 생성하기 위하여 트랜잭션을 생성하는 함수
    ''' </summary>
    ''' <returns>0:정상처리, -1:오류</returns>
    ''' <remarks></remarks>
    Public Function beginTrans() As Integer
        Dim iDBProc As Integer = 0
        Try
            'iDBProc = New Agent().setTrans(bTransUse)
            iDBProc = m_DBAgent.setTrans(True)
        Catch ex As Exception
            Throw ex
        End Try
        Return iDBProc
    End Function

    ''' <summary>
    ''' 트랜잭션 사용 후 트랜잭션을 초기화 시키는 함수
    ''' </summary>
    ''' <returns>0:정상처리, -1:오류</returns>
    ''' <remarks></remarks>
    Public Function endTrans() As Integer
        Dim iDBProc As Integer = 0
        Try
            'iDBProc = New Agent().setTrans(bTransUse)
            iDBProc = m_DBAgent.setTrans(False)
        Catch ex As Exception
            Throw ex
        End Try
        Return iDBProc
    End Function

    ''' <summary>
    ''' 현재 트랜잭션 Commit
    ''' </summary>
    ''' <returns>0:정상처리, -1:오류, 1:트랜잭션이 시작되지 않았음(오류)</returns>
    ''' <remarks></remarks>
    Public Function commitTrans() As Integer
        Dim iDBProc As Integer = 0
        Try
            'iDBProc = New Agent().commitTrans
            iDBProc = m_DBAgent.commitTrans
        Catch ex As Exception
            Throw ex
        End Try
        Return iDBProc
    End Function

    ''' <summary>
    ''' 현재 트랜잭션 rollback
    ''' </summary>
    ''' <returns>0:정상처리, -1:오류, 1:트랜잭션이 시작되지 않았음(오류)</returns>
    ''' <remarks></remarks>
    Public Function rollbackTrans() As Integer
        Dim iDBProc As Integer = 0
        Try
            'iDBProc = New Agent().rollbackTrans
            iDBProc = m_DBAgent.rollbackTrans
        Catch ex As Exception
            Throw ex
        End Try
        Return iDBProc
    End Function
#End Region

#Region "DML 실행"

    ''' <summary>
    ''' SQL 데이터 조회
    ''' </summary>
    ''' <param name="sSQL">SQL문의 변수명</param>
    ''' <param name="sParams">매개변수에 대한 인수 배열</param>
    ''' <param name="bUseVariables">오라클 바인드 변수 사용여부</param>
    ''' <returns>조회 결과 Datatable</returns>
    ''' <remarks></remarks>
    Public Function getData(ByRef sSQL As String,
          Optional ByVal sParams() As String = Nothing,
          Optional ByVal bUseVariables As Boolean = True) As System.Data.DataTable
        Dim dtblOUT As DataTable = Nothing
        Dim SQL As String = sSQL
        'If sSQL.StartsWith("R_") OrElse sSQL.StartsWith("   OrElse sSQL.StartsWith("U_") OrElse sSQL.StartsWith("D_") OrElse sSQL.StartsWith("I_") Then
        '    SQL = Utilities.retrieveSql(sSQL)
        'End If

        Try
            If Not bUseVariables AndAlso Not IsNothing(sParams) Then
                SQL = Utilities.replaceParams(SQL, sParams)
            End If

            dtblOUT = New Agent().selectData(SQL, sParams, bUseVariables)
        Catch ex As Exception
            Throw ex
        End Try

        Return dtblOUT
    End Function


    ''' <summary>
    ''' SQL 데이터 조회
    ''' </summary>
    ''' <param name="sSQL">SQL문의 변수명</param>
    ''' <param name="sSQL_Output">실행 SQL문을 저장할 변수명</param>
    ''' <param name="sParams">매개변수에 대한 인수 배열</param>
    ''' <param name="bUseVariables">오라클 바인드 변수 사용여부</param>
    ''' <returns>조회 결과 Datatable</returns>
    ''' <remarks></remarks>
    Public Function getData(ByRef sSQL As String,
                            ByRef sSQL_Output As String,
                            Optional ByVal sParams() As String = Nothing,
                            Optional ByVal bUseVariables As Boolean = True) As System.Data.DataTable
        Dim dtblOUT As DataTable = Nothing
        Dim SQL As String = sSQL
        'If sSQL.StartsWith("R_") OrElse sSQL.StartsWith("C_") OrElse sSQL.StartsWith("U_") OrElse sSQL.StartsWith("D_") OrElse sSQL.StartsWith("I_") Then
        '    SQL = Utilities.retrieveSql(sSQL)
        'End If

        Try
            If Not IsNothing(sParams) Then
                If Not bUseVariables Then
                    SQL = Utilities.replaceParams(SQL, sParams)
                    sSQL_Output = SQL
                Else
                    sSQL_Output = Utilities.replaceParams(SQL, sParams)
                End If
            End If

            dtblOUT = New Agent().selectData(SQL, sParams, bUseVariables)
        Catch ex As Exception
            Throw ex
        End Try

        Return dtblOUT
    End Function


    ''' <summary>
    ''' SQL 데이터 조회
    ''' </summary>
    ''' <param name="sSQL">SQL문의 변수명</param>
    ''' <param name="dicParams">매개변수에 대한 변수명-값 Dictionary</param>
    ''' <param name="bUseVariables">오라클 바인드 변수 사용여부</param>
    ''' <returns>조회 결과 Datatable</returns>
    ''' <remarks></remarks>
    Public Function getData2(ByRef sSQL As String,
                             Optional ByVal dicParams As Dictionary(Of String, String) = Nothing,
                             Optional ByVal bUseVariables As Boolean = False,
                             Optional ByVal bUseSingleQuotes As Boolean = False) As System.Data.DataTable
        Dim dtblOUT As DataTable = Nothing
        Dim SQL As String = sSQL

        Try
            If Not bUseVariables AndAlso Not IsNothing(dicParams) Then
                If bUseSingleQuotes Then
                    SQL = Utilities.replaceParams2(SQL, dicParams)
                Else
                    SQL = Utilities.replaceParams(SQL, dicParams)
                End If
            End If

            dtblOUT = New Agent().selectData2(SQL, dicParams, bUseVariables)
        Catch ex As Exception
            Throw ex
        End Try

        Return dtblOUT
    End Function


    ''' <summary>
    ''' SQL 데이터 조회
    ''' </summary>
    ''' <param name="sSQL">SQL문의 변수명</param>
    ''' <param name="sSQL_Output">실행 SQL문을 저장할 변수명</param>
    ''' <param name="dicParams">매개변수에 대한 변수명-값 Dictionary</param>
    ''' <param name="bUseVariables">오라클 바인드 변수 사용여부</param>
    ''' <returns>조회 결과 Datatable</returns>
    ''' <remarks></remarks>
    Public Function getData2(ByRef sSQL As String,
                             ByRef sSQL_Output As String,
                             Optional ByVal dicParams As Dictionary(Of String, String) = Nothing,
                             Optional ByVal bUseVariables As Boolean = False,
                             Optional ByVal bUseSingleQuotes As Boolean = False) As System.Data.DataTable
        Dim dtblOUT As DataTable = Nothing
        Dim SQL As String = sSQL
        sSQL_Output = sSQL

        Try
            If Not IsNothing(dicParams) Then
                If Not bUseVariables Then
                    If bUseSingleQuotes Then
                        SQL = Utilities.replaceParams2(SQL, dicParams)
                        sSQL_Output = Utilities.replaceParams2(SQL, dicParams)
                    Else
                        SQL = Utilities.replaceParams(SQL, dicParams)
                        sSQL_Output = Utilities.replaceParams(SQL, dicParams)
                    End If
                Else
                    sSQL_Output = Utilities.replaceParams2(SQL, dicParams)
                End If
            End If

            dtblOUT = New Agent().selectData2(SQL, dicParams, bUseVariables)
        Catch ex As Exception
            Throw ex
        End Try

        Return dtblOUT
    End Function

    ''' <summary>
    ''' SELECT 결과 조회
    ''' </summary>
    ''' <param name="sSQL">SQL String</param>
    ''' <param name="dicParams">파라미터 Dictionary</param>
    ''' <param name="sReplacedSQL">sSQL의 파라미터 자리를 실제 값으로 Replace한 SQL을 By Reference로 반환</param>
    ''' <returns>조회 결과 Datatable</returns>
    ''' <remarks></remarks>
    Public Function getData3(ByRef sSQL As String,
                             Optional ByVal dicParams As Dictionary(Of String, String) = Nothing,
                             Optional ByRef sReplacedSQL As String = Nothing) As System.Data.DataTable
        Dim dtblOUT As DataTable = Nothing
        Dim bUseVariables As Boolean = False
        Dim dicParamsActual As Dictionary(Of String, String) = Nothing

        Try
            If Not IsNothing(dicParams) Then
                bUseVariables = True
                dicParamsActual = New Dictionary(Of String, String)

                Dim pair As KeyValuePair(Of String, String)
                For Each pair In dicParams
                    If sSQL.Contains(pair.Key) Then
                        dicParamsActual.Add(pair.Key, pair.Value)
                    End If
                Next

                If Not IsNothing(sReplacedSQL) Then
                    sReplacedSQL = Utilities.replaceParams2(sSQL, dicParamsActual)
                End If
            End If

            dtblOUT = New Agent().selectData2(sSQL, dicParamsActual, bUseVariables)
        Catch ex As Exception
            Throw ex
        End Try

        Return dtblOUT
    End Function


    ''' <summary>
    ''' Insert/Update/Delete 등 단위 저장 SQL 수행
    ''' </summary>
    ''' <param name="sSQL">SQL문의 변수명</param>
    ''' <param name="sSQL_Output">실행 SQL문을 저장할 변수명</param>
    ''' <param name="sParams">매개변수에 대한 인수 배열</param>
    ''' <param name="bUseVariables">오라클 바인드 변수 사용여부</param>
    ''' <returns>영향 받는 행의 수</returns>
    ''' <remarks></remarks>
    Public Function saveData(ByRef sSQL As String,
                             ByRef sSQL_Output As String,
                             Optional ByVal sParams() As String = Nothing,
                             Optional ByVal bUseVariables As Boolean = False,
                             Optional ByVal bAutoCommit As Boolean = True) As Integer
        Dim iReturn As Integer = -1
        Dim SQL As String = sSQL
        'If sSQL.StartsWith("R_") OrElse sSQL.StartsWith("C_") OrElse sSQL.StartsWith("U_") OrElse sSQL.StartsWith("D_") OrElse sSQL.StartsWith("I_") Then
        '    SQL = Utilities.retrieveSql(sSQL)
        'End If

        Try
            If Not bUseVariables AndAlso Not IsNothing(sParams) Then
                SQL = Utilities.replaceParams(SQL, sParams)
            End If
            sSQL_Output = SQL

            iReturn = New Agent().execute(SQL, sParams, bUseVariables, bAutoCommit)
        Catch ex As Exception
            Throw ex
        End Try

        Return iReturn
    End Function


    ''' <summary>
    ''' Insert/Update/Delete 등 단위 저장 SQL 수행
    ''' </summary>
    ''' <param name="sSQL">SQL문의 변수명</param>
    ''' <param name="sSQL_Output">실행 SQL문을 저장할 변수명</param>
    ''' <param name="dicParams">매개변수에 대한 변수명-값 Dictionary</param>
    ''' <param name="bUseVariables">오라클 바인드 변수 사용여부</param>
    ''' <returns>영향 받는 행의 수</returns>
    ''' <remarks></remarks>
    Public Function saveData2(ByRef sSQL As String,
                              ByRef sSQL_Output As String,
                              Optional ByVal dicParams As Dictionary(Of String, String) = Nothing,
                              Optional ByVal bUseVariables As Boolean = False,
                              Optional ByVal bUseSingleQuotes As Boolean = False,
                              Optional ByVal bAutoCommit As Boolean = True) As Integer
        Dim iReturn As Integer = -1
        Dim SQL As String = sSQL

        Try
            If Not IsNothing(dicParams) And Not bUseVariables Then
                If bUseSingleQuotes Then
                    SQL = Utilities.replaceParams2(SQL, dicParams)
                Else
                    SQL = Utilities.replaceParams(SQL, dicParams)
                End If
            End If

            sSQL_Output = SQL


            iReturn = New Agent().execute2(SQL, dicParams, bUseVariables, bAutoCommit)
        Catch ex As Exception
            Throw ex
        End Try

        Return iReturn
    End Function



#End Region
End Class
