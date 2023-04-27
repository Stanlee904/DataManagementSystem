<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmDataInserter
    Inherits System.Windows.Forms.Form

    'Form은 Dispose를 재정의하여 구성 요소 목록을 정리합니다.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Windows Form 디자이너에 필요합니다.
    Private components As System.ComponentModel.IContainer

    '참고: 다음 프로시저는 Windows Form 디자이너에 필요합니다.
    '수정하려면 Windows Form 디자이너를 사용하십시오.  
    '코드 편집기에서는 수정하지 마세요.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.OracleCommand1 = New Oracle.ManagedDataAccess.Client.OracleCommand()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.opt_26 = New System.Windows.Forms.RadioButton()
        Me.opt_25 = New System.Windows.Forms.RadioButton()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdEditData = New System.Windows.Forms.Button()
        Me.t_day = New System.Windows.Forms.DateTimePicker()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.opt_Day = New System.Windows.Forms.RadioButton()
        Me.optNight = New System.Windows.Forms.RadioButton()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Panel8 = New System.Windows.Forms.Panel()
        Me.grd_RECEIVE_LIST = New DevExpress.XtraGrid.GridControl()
        Me.grdv_RECEIVE_LIST = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.Panel7 = New System.Windows.Forms.Panel()
        Me.progressLineLabel = New System.Windows.Forms.Label()
        Me.inputLineLabel = New System.Windows.Forms.Label()
        Me.OracleCommand2 = New Oracle.ManagedDataAccess.Client.OracleCommand()
        Me.Panel6.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.Panel8.SuspendLayout()
        CType(Me.grd_RECEIVE_LIST, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grdv_RECEIVE_LIST, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel7.SuspendLayout()
        Me.SuspendLayout()
        '
        'OracleCommand1
        '
        Me.OracleCommand1.Transaction = Nothing
        '
        'Panel6
        '
        Me.Panel6.Controls.Add(Me.GroupBox3)
        Me.Panel6.Controls.Add(Me.Label2)
        Me.Panel6.Controls.Add(Me.cmdExit)
        Me.Panel6.Controls.Add(Me.cmdEditData)
        Me.Panel6.Controls.Add(Me.t_day)
        Me.Panel6.Controls.Add(Me.GroupBox2)
        Me.Panel6.Location = New System.Drawing.Point(3, 3)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(522, 121)
        Me.Panel6.TabIndex = 61
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.opt_26)
        Me.GroupBox3.Controls.Add(Me.opt_25)
        Me.GroupBox3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.GroupBox3.Location = New System.Drawing.Point(302, 48)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(200, 66)
        Me.GroupBox3.TabIndex = 68
        Me.GroupBox3.TabStop = False
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("굴림", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Label4.Location = New System.Drawing.Point(14, 28)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(50, 13)
        Me.Label4.TabIndex = 63
        Me.Label4.Text = "서버 : "
        '
        'opt_26
        '
        Me.opt_26.AutoSize = True
        Me.opt_26.Checked = True
        Me.opt_26.Location = New System.Drawing.Point(65, 42)
        Me.opt_26.Name = "opt_26"
        Me.opt_26.Size = New System.Drawing.Size(129, 16)
        Me.opt_26.TabIndex = 1
        Me.opt_26.TabStop = True
        Me.opt_26.Text = "222.111.237.26 서버"
        Me.opt_26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.opt_26.UseVisualStyleBackColor = True
        '
        'opt_25
        '
        Me.opt_25.AutoSize = True
        Me.opt_25.Location = New System.Drawing.Point(65, 12)
        Me.opt_25.Name = "opt_25"
        Me.opt_25.Size = New System.Drawing.Size(129, 16)
        Me.opt_25.TabIndex = 1
        Me.opt_25.TabStop = True
        Me.opt_25.Text = "222.111.237.25 서버"
        Me.opt_25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.opt_25.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("굴림", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 23)
        Me.Label2.Margin = New System.Windows.Forms.Padding(8, 15, 0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(78, 13)
        Me.Label2.TabIndex = 60
        Me.Label2.Text = "기준일자 : "
        '
        'cmdExit
        '
        Me.cmdExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdExit.Font = New System.Drawing.Font("굴림", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.cmdExit.Location = New System.Drawing.Point(430, 13)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.Size = New System.Drawing.Size(72, 30)
        Me.cmdExit.TabIndex = 1
        Me.cmdExit.Text = "종료"
        Me.cmdExit.UseVisualStyleBackColor = True
        '
        'cmdEditData
        '
        Me.cmdEditData.Font = New System.Drawing.Font("굴림", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.cmdEditData.Location = New System.Drawing.Point(325, 13)
        Me.cmdEditData.Name = "cmdEditData"
        Me.cmdEditData.Size = New System.Drawing.Size(72, 30)
        Me.cmdEditData.TabIndex = 1
        Me.cmdEditData.Text = "시작"
        Me.cmdEditData.UseVisualStyleBackColor = True
        '
        't_day
        '
        Me.t_day.CausesValidation = False
        Me.t_day.Enabled = False
        Me.t_day.Location = New System.Drawing.Point(91, 18)
        Me.t_day.Name = "t_day"
        Me.t_day.Size = New System.Drawing.Size(200, 21)
        Me.t_day.TabIndex = 66
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.opt_Day)
        Me.GroupBox2.Controls.Add(Me.optNight)
        Me.GroupBox2.Location = New System.Drawing.Point(38, 45)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(200, 69)
        Me.GroupBox2.TabIndex = 67
        Me.GroupBox2.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("굴림", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Label3.Location = New System.Drawing.Point(20, 31)
        Me.Label3.Margin = New System.Windows.Forms.Padding(0, 50, 40, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(78, 13)
        Me.Label3.TabIndex = 56
        Me.Label3.Text = "작업시간 : "
        '
        'opt_Day
        '
        Me.opt_Day.AutoSize = True
        Me.opt_Day.Location = New System.Drawing.Point(106, 16)
        Me.opt_Day.Name = "opt_Day"
        Me.opt_Day.Size = New System.Drawing.Size(70, 16)
        Me.opt_Day.TabIndex = 1
        Me.opt_Day.TabStop = True
        Me.opt_Day.Text = "오전(_d)"
        Me.opt_Day.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.opt_Day.UseVisualStyleBackColor = True
        '
        'optNight
        '
        Me.optNight.AutoSize = True
        Me.optNight.Location = New System.Drawing.Point(106, 44)
        Me.optNight.Name = "optNight"
        Me.optNight.Size = New System.Drawing.Size(70, 16)
        Me.optNight.TabIndex = 1
        Me.optNight.TabStop = True
        Me.optNight.Text = "오후(_n)"
        Me.optNight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.optNight.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Panel6, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel8, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel7, 0, 2)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 12)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 249.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(528, 447)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'Panel8
        '
        Me.Panel8.Controls.Add(Me.grd_RECEIVE_LIST)
        Me.Panel8.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel8.Location = New System.Drawing.Point(3, 131)
        Me.Panel8.Name = "Panel8"
        Me.Panel8.Size = New System.Drawing.Size(522, 243)
        Me.Panel8.TabIndex = 64
        '
        'grd_RECEIVE_LIST
        '
        Me.grd_RECEIVE_LIST.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grd_RECEIVE_LIST.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.grd_RECEIVE_LIST.Location = New System.Drawing.Point(0, 0)
        Me.grd_RECEIVE_LIST.MainView = Me.grdv_RECEIVE_LIST
        Me.grd_RECEIVE_LIST.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.grd_RECEIVE_LIST.Name = "grd_RECEIVE_LIST"
        Me.grd_RECEIVE_LIST.Size = New System.Drawing.Size(522, 243)
        Me.grd_RECEIVE_LIST.TabIndex = 61
        Me.grd_RECEIVE_LIST.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.grdv_RECEIVE_LIST})
        '
        'grdv_RECEIVE_LIST
        '
        Me.grdv_RECEIVE_LIST.GridControl = Me.grd_RECEIVE_LIST
        Me.grdv_RECEIVE_LIST.GroupSummary.AddRange(New DevExpress.XtraGrid.GridSummaryItem() {New DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.None, "", Nothing, "")})
        Me.grdv_RECEIVE_LIST.Name = "grdv_RECEIVE_LIST"
        '
        'Panel7
        '
        Me.Panel7.Controls.Add(Me.progressLineLabel)
        Me.Panel7.Controls.Add(Me.inputLineLabel)
        Me.Panel7.Location = New System.Drawing.Point(3, 380)
        Me.Panel7.Name = "Panel7"
        Me.Panel7.Size = New System.Drawing.Size(522, 41)
        Me.Panel7.TabIndex = 63
        '
        'progressLineLabel
        '
        Me.progressLineLabel.AutoSize = True
        Me.progressLineLabel.Font = New System.Drawing.Font("굴림", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.progressLineLabel.Location = New System.Drawing.Point(58, 12)
        Me.progressLineLabel.Name = "progressLineLabel"
        Me.progressLineLabel.Size = New System.Drawing.Size(111, 16)
        Me.progressLineLabel.TabIndex = 3
        Me.progressLineLabel.Text = "진행라인수 : "
        '
        'inputLineLabel
        '
        Me.inputLineLabel.AutoSize = True
        Me.inputLineLabel.Font = New System.Drawing.Font("굴림", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.inputLineLabel.Location = New System.Drawing.Point(286, 12)
        Me.inputLineLabel.Name = "inputLineLabel"
        Me.inputLineLabel.Size = New System.Drawing.Size(111, 16)
        Me.inputLineLabel.TabIndex = 1
        Me.inputLineLabel.Text = "입력라인수 : "
        '
        'OracleCommand2
        '
        Me.OracleCommand2.Transaction = Nothing
        '
        'frmDataInserter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(556, 471)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmDataInserter"
        Me.Text = "데이터입력자료관리시스템 v1.0(20221209)"
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.Panel8.ResumeLayout(False)
        CType(Me.grd_RECEIVE_LIST, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grdv_RECEIVE_LIST, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel7.ResumeLayout(False)
        Me.Panel7.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents RadioGroup1 As DevExpress.XtraEditors.RadioGroup
    Friend WithEvents RadioButton1 As RadioButton
    Friend WithEvents RadioButton2 As RadioButton
    Friend WithEvents RadioButton3 As RadioButton
    Friend WithEvents RadioButton4 As RadioButton
    Friend WithEvents OracleCommand1 As Oracle.ManagedDataAccess.Client.OracleCommand
    Friend WithEvents Panel6 As Panel
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Label4 As Label
    Friend WithEvents opt_26 As RadioButton
    Friend WithEvents opt_25 As RadioButton
    Friend WithEvents Label2 As Label
    Friend WithEvents cmdExit As Button
    Friend WithEvents cmdEditData As Button
    Friend WithEvents t_day As DateTimePicker
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label3 As Label
    Friend WithEvents opt_Day As RadioButton
    Friend WithEvents optNight As RadioButton
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel8 As Panel
    Friend WithEvents Panel7 As Panel
    Friend WithEvents progressLineLabel As Label
    Friend WithEvents inputLineLabel As Label
    Friend WithEvents grd_RECEIVE_LIST As DevExpress.XtraGrid.GridControl
    Friend WithEvents grdv_RECEIVE_LIST As DevExpress.XtraGrid.Views.Grid.GridView
    Friend WithEvents OracleCommand2 As Oracle.ManagedDataAccess.Client.OracleCommand
End Class
