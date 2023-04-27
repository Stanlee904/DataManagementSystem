Public Class frm_LstAsk

    Public connectionDB As New DB_Agent()
    Public DB_Query As New DB_Query()
    Public LastMainJob As New LastMainJob()

    Dim App_Path = System.AppDomain.CurrentDomain.BaseDirectory() ' 프로그램 현재 경로
    Dim logFile As String
    Dim fileNumber As Integer = FreeFile()

#Region "FORM_LOAD"
    ''' <summary>
    ''' 폼 로드
    ''' </summary>
    ''' <param name="eventSender"></param> '이벤트를 발생된 개체
    ''' <param name="eventArgs"></param> '이벤트 발생에 추가적인 정보들을 담은 개체
    ''' Handles MyBase.Load -> 개체의 부모 개체에 Load 이벤트가 발생되면 이 프로시저를 실행하게 된다. 

    Private Sub Form_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        '영업일 조회
        t_day.Value = Date.Today
        't_day.Value = Date.Today.AddDays(-13).ToString("yyyy-MM-dd")

        ' 영업일이 아닐 경우 프로그램 정지
        If bizCheck() <> "0" Then
            cmdEditData.Enabled = False
            Exit Sub
        End If

        '그리드 세팅
        initGrid()

        '작업 수행
        cmdEditData.PerformClick()


    End Sub
#End Region

    ''' <summary>
    ''' 그리드 초기화
    ''' </summary>
    Private Sub initGrid()

        Dim gridAgent As DXGrid = New DXGrid()
        Dim dicParams As New Dictionary(Of String, String)
        Dim sSQL_Output As String = Nothing
        Dim dtblDATA As New DataTable
        Dim clDB As New DB_Agent
        Dim tempDtblData As New DataTable

        Try

            With gridAgent

                .initGrid(grdv_FINCHK_LIST, True, False, False, False, False, False, True, True, True, True)

                .addCheckboxColumn(grdv_FINCHK_LIST, "확인여부", "CHK_RCV", 160, True)
                .addGridColumn(grdv_FINCHK_LIST, "수신시간", "RECEIVE_TIME", 155, , 2)
                .addGridColumn(grdv_FINCHK_LIST, "완료", "COMPLETE", 155, , 2)

                dtblDATA = connectionDB.getData2(DB_Query.GRIDSET, dicParams, True, True)

                grd_FINCHK_LIST.DataSource = dtblDATA

            End With
            tempDtblData.Columns.Add("CHK_RCV")
            tempDtblData.Columns.Add("RECEIVE_TIME")
            tempDtblData.Columns.Add("COMPLETE")

            tempDtblData.Rows.Add(New String() {"1", "수신시간(오전:1130)", ""})
            tempDtblData.Rows.Add(New String() {"1", "수신시간(오후:1630)", ""})
            tempDtblData.Rows.Add(New String() {"1", "정정", ""})

            grd_FINCHK_LIST.DataSource = tempDtblData

        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try

    End Sub
    ''' <summary>
    ''' 시작 버튼 
    ''' </summary>
    Private Sub cmdEditData_Click(sender As Object, e As EventArgs) Handles cmdEditData.Click

        Try

            logFile = App_Path & "log\LastAkPrcValue(PM)_" & Format(t_day.Value, "yyyyMMdd") & ".txt"

            FileOpen(fileNumber, logFile, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "장외채권 최종호가수익률 시작!" & vbCrLf)
            FileClose()

            cmdEditData.Enabled = False

            LastMainJob.LastAKPRCValueMainProcess(logFile, fileNumber)

            ' 로그파일 삭제
            Utilities.DeleteLogFile()

            FileOpen(fileNumber, logFile, OpenMode.Append)
            Print(fileNumber, Date.Now & " | 최종호가수익률 작업 완료!" & vbCrLf)
            FileClose()

            Exit Sub

        Catch ex As Exception
            MsgBox(ex.ToString())
            Application.Exit()
        End Try
    End Sub

    ''' <summary>
    ''' 종료버튼
    ''' </summary>
    Private Sub cmdExit_Click(sender As Object, e As EventArgs) Handles cmdExit.Click
        Application.Exit()
    End Sub

    ''' <summary>
    ''' 영업일 확인
    ''' </summary>
    Private Function bizCheck() As String
        Dim dtblDATA As DataTable
        Dim dicParams As New Dictionary(Of String, String)

        Try
            dicParams.Add(":t_day", t_day.Value.ToString("yyyyMMdd"))
            dtblDATA = connectionDB.getData2(DB_Query.todayIsWorkDayCheck, dicParams, True)

            Return dtblDATA.Rows(0).Item("TYPE").ToString
        Catch ex As Exception
            MessageBox.Show(ex.Message & "  [Form] ReckSend , 프로시져 이름: bizCheck ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return "-1"
        End Try
    End Function
End Class
