Imports DevExpress.XtraEditors
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports System.IO

Public Class frmDataInserter

    Public DataDefineDAO As New DataDefineDAO()
    Public KoscomMethod As New KoscomMethod()
    Public Utilities As New Utilities()
    Public DataManageMentSystemMethod As New DataManageMentSystemMethod()
    Public connectionDB As New DB_Agent()
    Public DayMainJob As New DayMainJob()
    Public NightMainJob As New NightMainJob()
    Public DB_Query As New DB_Query()
    Public RetailDataAndKRXInput As New RetailDataAndKRXInput()


    Dim App_Path = System.AppDomain.CurrentDomain.BaseDirectory() ' 프로그램 현재 경로
    Dim logFile As String
    Dim fileNumber As Integer = FreeFile()


#Region "폼 전용 함수"
    ''' <summary>
    ''' 폼 로드
    ''' </summary>
    ''' <param name="eventSender"></param> '이벤트를 발생된 개체
    ''' <param name="eventArgs"></param> '이벤트 발생에 추가적인 정보들을 담은 개체
    ''' Handles MyBase.Load -> 개체의 부모 개체에 Load 이벤트가 발생되면 이 프로시저를 실행하게 된다. 

    Private Sub Form_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        '영업일 조회
        t_day.Value = Date.Today

        '시간 확인
        Dim tempTodayValue = Format(Date.Now, "yyyy-MM-dd tt")

        '시작 시간 버튼 자동 체크
        autoCheckTime(tempTodayValue)

        '그리드 세팅
        initGrid()

        '파일 취합 및 금일 처리 할 개수 기입 
        TodayFileCheckWriteCount()

    End Sub
    ''' <summary>
    ''' 오후 버튼 클릭
    ''' </summary>
    Private Sub optNight_CheckedChanged_1(sender As Object, e As EventArgs) Handles optNight.Click
        If optNight.Checked = True Then
            opt_Day.Checked = False
        End If
    End Sub
    ''' <summary>
    ''' 오전 버튼 클릭
    ''' </summary>
    Private Sub opt_Day_CheckedChanged_1(sender As Object, e As EventArgs) Handles opt_Day.Click
        If opt_Day.Checked = True Then
            optNight.Checked = False
        End If
    End Sub
    ''' <summary>
    ''' 26번 서버 클릭
    ''' </summary>
    Private Sub opt_26_CheckedChanged_1(sender As Object, e As EventArgs) Handles opt_26.Click
        If opt_26.Checked = True Then
            opt_25.Checked = False
        End If
    End Sub
    ''' <summary>
    ''' 25번 서버 클릭
    ''' </summary>
    Private Sub opt_25_CheckedChanged_1(sender As Object, e As EventArgs) Handles opt_25.Click
        If opt_25.Checked = True Then
            opt_26.Checked = False
        End If
    End Sub
    ''' <summary>
    ''' 시작 버튼 
    ''' </summary>
#Region "작업의 Main 문"
    Private Sub cmdEditData_Click(sender As Object, e As EventArgs) Handles cmdEditData.Click

        Try
            ' 반환된 파일 이름이 없을 경우 작업 종료 ( 즉,오류가 발생한 경우)
            If DataDefineDAO.locFile = "" Then
                MsgBox("생성된 금일 작업 파일이 없습니다. 금일 작업 파일을 확인해주세요!")
                Exit Sub
            End If

            cmdEditData.Enabled = False

            ' 오전 작업 진행
            If opt_Day.Checked = True Then
                If DayMainJob.MainDayJob(DataDefineDAO.locFile, logFile, fileNumber) = False Then
                    Application.Exit()
                End If
                ' 오후 작업 진행
            ElseIf optNight.Checked = True Then
                If NightMainJob.MainNightJob(DataDefineDAO.locFile, logFile, fileNumber) = False Then
                    Application.Exit()
                End If
            End If

            ' 로그파일 삭제
            Utilities.DeleteLogFile()

            Exit Sub
        Catch ex As Exception
            MsgBox(ex.ToString())
            Exit Sub
        End Try
    End Sub
    ''' <summary>
    ''' 종료버튼
    ''' </summary>
    Private Sub cmdExit_Click(sender As Object, e As EventArgs) Handles cmdExit.Click
        Application.Exit()
    End Sub

    ''' <summary>
    ''' TRCODE DB에서 가져와서 대입
    ''' </summary>
    Private Sub initGrid()

        Dim gridAgent As DXGrid = New DXGrid()
        Dim dicParams As New Dictionary(Of String, String)
        Dim sSQL_Output As String = Nothing
        Dim dtblDATA As New DataTable
        Dim clDB As New DB_Agent

        Try
            With gridAgent

                .initGrid(grdv_RECEIVE_LIST, True, False, False, False, False, False, True, True, True, True)

                .addCheckboxColumn(grdv_RECEIVE_LIST, "ALL", "CHK", 30, True)

                .addGridColumn(grdv_RECEIVE_LIST, "TRCODE명", "TR", 100, , 2)
                .addGridColumn(grdv_RECEIVE_LIST, "금일 처리 할 개수", "TDY_DOCNT", 130, , HorzAlignment.Center)
                .addGridColumn(grdv_RECEIVE_LIST, "금일 처리 된 개수", "TDY_DIDCNT", 130, , HorzAlignment.Center)
                .addGridColumn(grdv_RECEIVE_LIST, "완료", "COMPLETE", 130, , HorzAlignment.Center)

                Dim dxALL_CHK As New DXGrid_AllCheck(grd_RECEIVE_LIST, grdv_RECEIVE_LIST, "CHK", 30, False, False)

                If opt_Day.Checked = True Then
                    dtblDATA = connectionDB.getData2(DB_Query.TR_DAY_Query, sSQL_Output, dicParams, True, True)
                ElseIf optNight.Checked = True Then
                    dtblDATA = connectionDB.getData2(DB_Query.TR_NIGHT_Query, sSQL_Output, dicParams, True, True)
                End If

                grd_RECEIVE_LIST.DataSource = dtblDATA

                If dtblDATA.Rows.Count = 0 Then
                    MessageBox.Show("조회결과가 없습니다.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If

            End With

        Catch ex As Exception
            MsgBox(ex.ToString())
        End Try
    End Sub
    ''' <summary>
    ''' 시간 확인해서 12시가 넘으면 오후 / 오전에 체크
    ''' </summary>
    Private Sub autoCheckTime(ByVal tempTodayValue As String)

        If tempTodayValue.Contains("오전") Then
            opt_Day.Checked = True
            optNight.Enabled = False
        ElseIf tempTodayValue.Contains("오후") Then
            optNight.Checked = True
            opt_Day.Enabled = False
        End If

    End Sub
    ''' <summary>
    ''' 금일 파일 체크 및 그리드에 처리 할 개수 기입
    ''' </summary>
    Private Sub TodayFileCheckWriteCount()
        Try

            '로그 파일 (AM / PM 구분하여 진행)
            If optNight.Checked = True Then
                logFile = App_Path & "log\DataMagementSystemLog(PM)_" & Format(t_day.Value, "yyyyMMdd") & ".txt"
            Else
                logFile = App_Path & "log\DataMagementSystemLog(AM)_" & Format(t_day.Value, "yyyyMMdd") & ".txt"
            End If

            FileOpen(fileNumber, logFile, OpenMode.Append)
            Print(fileNumber, Date.Now & " | " & "자료관리시스템 작업 시작!" & vbCrLf)
            FileClose()

            ' 1. 오전 / 오후 나눠서 파일 취합
            DataDefineDAO.locFile = KoscomMethod.FuncKoscomSrcFile(t_day, logFile, fileNumber)

            '2. 그리드에 오전 / 오후 별로 그리드에 처리 할 개수 기입
            If optNight.Checked = True And Not DataDefineDAO.locFile = "" Then
                'RetailDataAndKRXInput.TodayDoCountGridWriteNight(logFile, fileNumber)
                RetailDataAndKRXInput.TodayDoCountGridWriteNight2(logFile, fileNumber)
                NightMainJob.TodayDoCountGridWriteNight(DataDefineDAO.locFile, logFile, fileNumber)
            ElseIf opt_Day.Checked = True And Not DataDefineDAO.locFile = "" Then
                DayMainJob.TodayDoCountGridWriteDay(DataDefineDAO.locFile, logFile, fileNumber)
            End If
        Catch ex As Exception
            MsgBox(ex.ToString())
            Exit Sub
        End Try

    End Sub

#End Region

#End Region

End Class