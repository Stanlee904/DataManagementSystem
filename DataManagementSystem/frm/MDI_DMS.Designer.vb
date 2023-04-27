<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MDI_DMS
    Inherits System.Windows.Forms.Form

    'Form은 Dispose를 재정의하여 구성 요소 목록을 정리합니다.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.tabManager = New DevExpress.XtraTabbedMdi.XtraTabbedMdiManager(Me.components)
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.자료관리시스템ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.최종호가수익률ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        CType(Me.tabManager, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tabManager
        '
        Me.tabManager.MdiParent = Me
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.자료관리시스템ToolStripMenuItem, Me.최종호가수익률ToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(800, 24)
        Me.MenuStrip1.TabIndex = 1
        Me.MenuStrip1.Text = "MenuStrip"
        '
        '자료관리시스템ToolStripMenuItem
        '
        Me.자료관리시스템ToolStripMenuItem.Name = "자료관리시스템ToolStripMenuItem"
        Me.자료관리시스템ToolStripMenuItem.Size = New System.Drawing.Size(103, 20)
        Me.자료관리시스템ToolStripMenuItem.Text = "자료관리시스템"
        '
        '최종호가수익률ToolStripMenuItem
        '
        Me.최종호가수익률ToolStripMenuItem.Name = "최종호가수익률ToolStripMenuItem"
        Me.최종호가수익률ToolStripMenuItem.Size = New System.Drawing.Size(103, 20)
        Me.최종호가수익률ToolStripMenuItem.Text = "최종호가수익률"
        '
        'MDI_DMS
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.MenuStrip1)
        Me.IsMdiContainer = True
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "MDI_DMS"
        Me.Text = "데이터자료관리시스템/최종호가수익률 v1.0 (20230406)"
        CType(Me.tabManager, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents tabManager As DevExpress.XtraTabbedMdi.XtraTabbedMdiManager
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents 자료관리시스템ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents 최종호가수익률ToolStripMenuItem As ToolStripMenuItem
End Class
