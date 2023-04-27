<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frm_LstAsk
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.grd_FINCHK_LIST = New DevExpress.XtraGrid.GridControl()
        Me.grdv_FINCHK_LIST = New DevExpress.XtraGrid.Views.Grid.GridView()
        Me.Panel6 = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdEditData = New System.Windows.Forms.Button()
        Me.t_day = New System.Windows.Forms.DateTimePicker()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.grd_FINCHK_LIST, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.grdv_FINCHK_LIST, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel6.SuspendLayout()
        Me.SuspendLayout()
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 61.98934!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.01066!))
        Me.TableLayoutPanel1.Controls.Add(Me.grd_FINCHK_LIST, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel6, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 26.8595!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 73.1405!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(479, 242)
        Me.TableLayoutPanel1.TabIndex = 0
        '
        'grd_FINCHK_LIST
        '
        Me.grd_FINCHK_LIST.Cursor = System.Windows.Forms.Cursors.Default
        Me.grd_FINCHK_LIST.Dock = System.Windows.Forms.DockStyle.Fill
        Me.grd_FINCHK_LIST.EmbeddedNavigator.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.grd_FINCHK_LIST.Location = New System.Drawing.Point(3, 70)
        Me.grd_FINCHK_LIST.MainView = Me.grdv_FINCHK_LIST
        Me.grd_FINCHK_LIST.Margin = New System.Windows.Forms.Padding(3, 5, 3, 5)
        Me.grd_FINCHK_LIST.Name = "grd_FINCHK_LIST"
        Me.grd_FINCHK_LIST.Size = New System.Drawing.Size(473, 167)
        Me.grd_FINCHK_LIST.TabIndex = 63
        Me.grd_FINCHK_LIST.ViewCollection.AddRange(New DevExpress.XtraGrid.Views.Base.BaseView() {Me.grdv_FINCHK_LIST})
        '
        'grdv_FINCHK_LIST
        '
        Me.grdv_FINCHK_LIST.GridControl = Me.grd_FINCHK_LIST
        Me.grdv_FINCHK_LIST.GroupSummary.AddRange(New DevExpress.XtraGrid.GridSummaryItem() {New DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.None, "", Nothing, "")})
        Me.grdv_FINCHK_LIST.Name = "grdv_FINCHK_LIST"
        '
        'Panel6
        '
        Me.Panel6.Controls.Add(Me.Label2)
        Me.Panel6.Controls.Add(Me.cmdExit)
        Me.Panel6.Controls.Add(Me.cmdEditData)
        Me.Panel6.Controls.Add(Me.t_day)
        Me.Panel6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel6.Location = New System.Drawing.Point(3, 3)
        Me.Panel6.Name = "Panel6"
        Me.Panel6.Size = New System.Drawing.Size(473, 59)
        Me.Panel6.TabIndex = 62
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("굴림", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.Label2.Location = New System.Drawing.Point(14, 24)
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
        Me.cmdExit.Location = New System.Drawing.Point(379, 14)
        Me.cmdExit.Name = "cmdExit"
        Me.cmdExit.Size = New System.Drawing.Size(72, 30)
        Me.cmdExit.TabIndex = 1
        Me.cmdExit.Text = "종료"
        Me.cmdExit.UseVisualStyleBackColor = True
        '
        'cmdEditData
        '
        Me.cmdEditData.Font = New System.Drawing.Font("굴림", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(129, Byte))
        Me.cmdEditData.Location = New System.Drawing.Point(301, 14)
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
        Me.t_day.Location = New System.Drawing.Point(95, 19)
        Me.t_day.Name = "t_day"
        Me.t_day.Size = New System.Drawing.Size(200, 21)
        Me.t_day.TabIndex = 66
        '
        'frm_LstAsk
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(479, 242)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Name = "frm_LstAsk"
        Me.Text = "장외채권최종호가수익률 v1.0(20230207)"
        Me.TableLayoutPanel1.ResumeLayout(False)
        CType(Me.grd_FINCHK_LIST, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.grdv_FINCHK_LIST, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel6.ResumeLayout(False)
        Me.Panel6.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents Panel6 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents cmdExit As Button
    Friend WithEvents cmdEditData As Button
    Friend WithEvents t_day As DateTimePicker
    Friend WithEvents grd_FINCHK_LIST As DevExpress.XtraGrid.GridControl
    Friend WithEvents grdv_FINCHK_LIST As DevExpress.XtraGrid.Views.Grid.GridView
End Class
