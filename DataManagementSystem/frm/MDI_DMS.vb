Public Class MDI_DMS
    ''' <summary>
    ''' 폼 오픈 함수
    ''' </summary>
    ''' <param name="thisChild"></param>
    ''' <remarks></remarks>
    Private Sub openForm(ByVal thisChild As Form)

        thisChild.WindowState = FormWindowState.Maximized
        thisChild.BackColor = Color.White
        thisChild.MdiParent = Me
        thisChild.Show()

    End Sub
    ''' <summary>
    ''' 자료관리시스템
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub 자료관리시스템ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 자료관리시스템ToolStripMenuItem.Click
        Me.Size = New Size(572, 530)
        openForm(frmDataInserter)

    End Sub

    Private Sub 최종호가수익률ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 최종호가수익률ToolStripMenuItem.Click
        Me.Size = New Size(495, 281)
        openForm(frm_LstAsk)
    End Sub

    Private Sub MDI_DMS_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tabManager.MdiParent = Me
        Dim sCmd As String = Interaction.Command().Trim()

        If sCmd.Trim() <> "" Then
            If CInt(sCmd.Trim()) = 1 Then
                자료관리시스템ToolStripMenuItem_Click(sender, e)

            ElseIf CInt(sCmd.Trim()) = 2 Then
                최종호가수익률ToolStripMenuItem_Click(sender, e)

            End If
        End If

    End Sub
End Class