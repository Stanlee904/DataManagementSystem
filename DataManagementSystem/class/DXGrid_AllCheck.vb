Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.Windows.Forms
Imports DevExpress.Utils.Drawing
Imports DevExpress.XtraGrid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.XtraEditors.Controls
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Grid.ViewInfo
Imports System.Data
''' <summary>
''' 기존꺼 ALL Check Box 수정판  -2014.03.03
''' </summary> Check Box 드레그 및 grid.Datasource의 DataTable로 Check Box Control
''' Check Box 클릭을 2번 이상해야 체크되던거 수정 -> 한번만 체크하면 바로 체크됨
''' 사용방법, 기존과 동일(넣으려는 그리드에 CHK로 체크박스를 추가하고 아래 부분에 클래스 호출하면 됨
''' (DB 조회후 System.Type.GetType("System.Boolean") 을 추가하던 루틴 제외해도 됨) 
''' 사용법은 ELN의 9911, 9514, pppFND_UNAS화면 참고하면 됨. 
''' 단, 컬럼을 Group 지었을때 체크하는 기능은 아직 미구현.. 시간되면 조금씩 더 하는 걸로....
''' <remarks></remarks>
Public Class DXGrid_AllCheck
    Protected m_View As GridView
    Private m_Column As GridColumn
    Private m_Edit As RepositoryItemCheckEdit
    Private m_sColName As String
    Private m_iColWidth As Integer
    Private m_grdOBJ As DevExpress.XtraGrid.GridControl
    Private m_bDrag As Boolean = False
    Private m_bRowCellStyle As Boolean = False
    Private m_dX As Double = 0
    Private m_iPRE_RowHandle As Integer = -5

    Private Const CHECKBOXINDENT As Integer = 4

    Public Sub New()
        MyBase.New()
    End Sub


    '2013.06.22 동호추가
    'Invalidate 우선 순위가 GridView를 그리고 다음에 Grid를 그리게 되어있음.
    '또한 이 클래스는 GridView에 직접 컬럼을 추가하는 구조.(GridView에만 추가하고 Grid에는 추가하지 않기 때문에
    'Grid의 컬럼값이 체크가 되어도 추가되지 않음. 그러므로 Grid를 같이 받아와서 동시에 값을 바꿔 줘야함)
    Public Sub New(ByVal grdOBJ As DevExpress.XtraGrid.GridControl, ByVal grdvOBJ As GridView,
                   ByVal sColName As String, ByVal iColWidth As Integer, Optional ByVal bDrag As Boolean = True, Optional ByVal bRowCellStyle As Boolean = True)
        Me.New()

        m_sColName = sColName
        m_iColWidth = iColWidth
        m_grdOBJ = grdOBJ
        m_bDrag = bDrag
        m_bRowCellStyle = bRowCellStyle

        Me.View = grdvOBJ
    End Sub

    Public Property View() As GridView
        Get
            Return m_View
        End Get
        Set(ByVal value As GridView)
            If m_View IsNot value Then
                Detach()
                Attach(value)
            End If
        End Set
    End Property

    Protected Overridable Sub Detach()
        If m_View Is Nothing Then
            Return
        End If
        If m_Column IsNot Nothing Then
            m_Column.Dispose()
        End If
        If m_Edit IsNot Nothing Then
            m_View.GridControl.RepositoryItems.Remove(m_Edit)
            m_Edit.Dispose()
        End If

        RemoveHandler View.CustomDrawColumnHeader, AddressOf View_CustomDrawColumnHeader
        RemoveHandler View.RowCellClick, AddressOf View_RowCellClick


        RemoveHandler View.MouseDown, AddressOf view_MouseDown

        If m_bDrag = True Then
            RemoveHandler View.MouseMove, AddressOf View_MouseMove
        End If

        If m_bRowCellStyle = True Then
            RemoveHandler View.RowCellStyle, AddressOf View_RowCellStyle
        End If
        m_View = Nothing
    End Sub

    Protected Overridable Sub Attach(ByVal view As GridView)
        If view Is Nothing Then
            Return
        End If
        Me.m_View = view
        'view.BeginUpdate()

        If Not IsNothing(view.Columns.ColumnByFieldName("CHK")) Then
            m_Edit = TryCast(view.GridControl.RepositoryItems.Add("CheckEdit"), RepositoryItemCheckEdit)
            m_Edit.ValueChecked = "1"
            m_Edit.ValueUnchecked = "0"
            AddHandler m_Edit.EditValueChanged, AddressOf edit_EditValueChanged
            m_Column = view.Columns("CHK")
            m_Column.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False
            m_Column.VisibleIndex = Integer.MaxValue
            m_Column.FieldName = IIf(m_sColName = "", "Selection", m_sColName)  ' "CheckMarkSelection"
            m_Column.Caption = "All"
            m_Column.OptionsColumn.ShowCaption = False
            m_Column.UnboundType = DevExpress.Data.UnboundColumnType.Boolean
            m_Column.VisibleIndex = 0
            m_Column.ColumnEdit = m_Edit
            m_Column.Width = IIf(m_iColWidth = 0, GetCheckBoxWidth(), m_iColWidth)
        End If

        AddHandler view.CustomDrawColumnHeader, AddressOf View_CustomDrawColumnHeader
        AddHandler view.RowCellClick, AddressOf View_RowCellClick
        AddHandler view.MouseDown, AddressOf view_MouseDown

        If m_bDrag = True Then
            AddHandler view.MouseMove, AddressOf View_MouseMove
        End If

        If m_bRowCellStyle = True Then
            AddHandler view.RowCellStyle, AddressOf View_RowCellStyle
        End If


    End Sub

    Private Sub View_RowCellStyle(ByVal sender As System.Object, ByVal e As DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs)
        Dim viewSender As DevExpress.XtraGrid.Views.Base.ColumnView = sender

        If e.RowHandle = m_View.FocusedRowHandle Then
            e.Appearance.BackColor = SystemColors.Highlight
            e.Appearance.ForeColor = SystemColors.HighlightText
        End If

    End Sub

    Private Sub View_MouseMove(sender As System.Object, e As System.Windows.Forms.MouseEventArgs)
        If e.Button = MouseButtons.Left Then
            'Console.WriteLine(e.Y)

            Dim info As GridHitInfo
            Dim pt As Point = m_View.GridControl.PointToClient(Control.MousePosition)
            info = m_View.CalcHitInfo(pt)

            If info.RowHandle < 0 Then
                m_iPRE_RowHandle = info.RowHandle
                Exit Sub
            End If
            If info.RowHandle = m_iPRE_RowHandle Then
                Exit Sub
            End If

            Dim grdRowIndex As Integer = m_View.GetDataSourceRowIndex(info.RowHandle)

            Dim dtblTable As DataTable = New DataTable

            dtblTable = m_grdOBJ.DataSource

            If dtblTable.Rows(grdRowIndex)("CHK") = "1" Then
                dtblTable.Rows(grdRowIndex)("CHK") = "0"
            Else
                dtblTable.Rows(grdRowIndex)("CHK") = "1"
            End If

            m_iPRE_RowHandle = info.RowHandle

            Invalidate()
        End If

    End Sub

    Protected Function GetCheckBoxWidth() As Integer
        Dim info As DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo = TryCast(m_Edit.CreateViewInfo(), DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo)
        Dim width As Integer = 0
        GraphicsInfo.Default.AddGraphics(Nothing)
        Try
            width = info.CalcBestFit(GraphicsInfo.Default.Graphics).Width
        Finally
            GraphicsInfo.Default.ReleaseGraphics()
        End Try
        Return width + CHECKBOXINDENT * 2
    End Function

    Private Sub edit_EditValueChanged(ByVal sender As Object, ByVal e As EventArgs)
        Invalidate()
    End Sub

    Private Sub View_CustomDrawColumnHeader(ByVal sender As Object, ByVal e As ColumnHeaderCustomDrawEventArgs)
        If e.Column Is m_Column Then
            e.Info.InnerElements.Clear()
            e.Painter.DrawObject(e.Info)
            Dim dtblTable As DataTable = New DataTable

            dtblTable = m_grdOBJ.DataSource
            If IsNothing(dtblTable) Then
                DrawCheckBox(e.Graphics, e.Bounds, False)
            Else
                'Dim irr As Integer = 0
                'Dim dtblTMP As DataTable = New DataTable

                'irr = dtblTable.Select("CHK = 1").Count

                'dtblTMP = dtblTable.Select("CHK = 0").CopyToDataTable
                Dim bTrue As Boolean = True
                If dtblTable.Rows.Count = 0 Then
                    bTrue = False
                Else
                    For i = 0 To dtblTable.Rows.Count - 1
                        If dtblTable.Rows(i)("CHK") = "0" Then
                            bTrue = False
                            Exit For
                        End If
                    Next
                End If
                DrawCheckBox(e.Graphics, e.Bounds, bTrue)
            End If

            e.Handled = True
        End If
    End Sub

    Private Sub View_RowCellClick(sender As System.Object, e As DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs)
        If e.Column Is m_Column Then
            m_View.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect

            Dim grdRowIndex As Integer = m_View.GetDataSourceRowIndex(e.RowHandle)
            Dim dtblTable As DataTable = New DataTable

            dtblTable = m_grdOBJ.DataSource

            If dtblTable.Rows(grdRowIndex)("CHK") = "1" Then
                dtblTable.Rows(grdRowIndex)("CHK") = "0"
            Else
                dtblTable.Rows(grdRowIndex)("CHK") = "1"
            End If
            m_View.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect
        Else
            m_View.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect
        End If

        Invalidate()
    End Sub

    Protected Sub DrawCheckBox(ByVal g As Graphics, ByVal r As Rectangle, ByVal Checked As Boolean)
        Dim info As DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo
        Dim painter As DevExpress.XtraEditors.Drawing.CheckEditPainter
        Dim args As DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs
        info = TryCast(m_Edit.CreateViewInfo(), DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo)
        painter = TryCast(m_Edit.CreatePainter(), DevExpress.XtraEditors.Drawing.CheckEditPainter)
        If Checked = True Then
            info.EditValue = "1"
        Else
            info.EditValue = "0"
        End If

        info.Bounds = r
        info.CalcViewInfo(g)
        args = New DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs(info, New DevExpress.Utils.Drawing.GraphicsCache(g), r)
        painter.Draw(args)
        args.Cache.Dispose()
    End Sub

    Private Sub Invalidate()
        Try
            m_View.CloseEditor()
            m_View.BeginUpdate()
            m_View.EndUpdate()
        Catch ex As Exception
            ' 가격분석,민감도분석 오류처리를 위해 수정
        End Try
    End Sub

    Public Sub SelectAll(ByVal strValue As String)
        Dim dataSource As ICollection = TryCast(m_View.DataSource, ICollection)
        If strValue = 1 Then
            For i As Integer = 0 To m_View.DataRowCount - 1 ' slow
                If Not IsNothing(m_grdOBJ) Then
                    If m_grdOBJ.DataSource.ToString = "System.Data.DataView" Then
                        DirectCast(m_grdOBJ.DataSource, System.Data.DataView).Table.Rows(i)("CHK") = 1
                    Else
                        DirectCast(m_grdOBJ.DataSource, System.Data.DataTable).Rows(i)("CHK") = 1
                    End If
                End If
            Next i
        Else
            For i As Integer = 0 To m_View.DataRowCount - 1 ' slow
                If Not IsNothing(m_grdOBJ) Then
                    If m_grdOBJ.DataSource.ToString = "System.Data.DataView" Then
                        DirectCast(m_grdOBJ.DataSource, System.Data.DataView).Table.Rows(i)("CHK") = 0
                    Else
                        DirectCast(m_grdOBJ.DataSource, System.Data.DataTable).Rows(i)("CHK") = 0
                    End If
                End If
            Next i
        End If

        Invalidate()
        If Not IsNothing(m_grdOBJ) Then
            m_grdOBJ.Invalidate()
        End If
    End Sub

    Private Sub view_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
        If e.Clicks = 1 AndAlso e.Button = MouseButtons.Left Then

            Dim info As GridHitInfo
            Dim pt As Point = m_View.GridControl.PointToClient(Control.MousePosition)
            info = m_View.CalcHitInfo(pt)



            If info.InRow AndAlso info.Column IsNot m_Column AndAlso m_View.IsDataRow(info.RowHandle) Then
                If info.RowHandle < 0 Then
                    m_iPRE_RowHandle = info.RowHandle
                    Exit Sub
                End If

                If IsNothing(info.Column) Then
                    Exit Sub
                End If

                If info.Column.FieldName <> m_Column.FieldName Then
                    Exit Sub
                End If

                Dim grdRowIndex As Integer = m_View.GetDataSourceRowIndex(info.RowHandle) '선택한 그리드의 DataTable Row Num 찾아내는 함수

                Dim dtblTable As DataTable = New DataTable

                dtblTable = m_grdOBJ.DataSource
                m_iPRE_RowHandle = info.RowHandle

                If dtblTable.Rows(grdRowIndex)("CHK") = "1" Then
                    dtblTable.Rows(grdRowIndex)("CHK") = "0"
                Else
                    dtblTable.Rows(grdRowIndex)("CHK") = "1"
                End If

                Invalidate()
            End If

            '헤더 선택했을 때,
            If info.InColumn AndAlso info.Column Is m_Column Then
                'If info.RowHandle < 0 Then
                '    m_iPRE_RowHandle = info.RowHandle
                '    Exit Sub
                'End If

                Dim bTrue As Boolean = True
                Dim dtblTable As DataTable = New DataTable

                dtblTable = m_grdOBJ.DataSource

                If IsNothing(dtblTable) Then
                    Exit Sub
                End If

                For i = 0 To dtblTable.Rows.Count - 1
                    If dtblTable.Rows(i)("CHK") = "0" Then
                        bTrue = False
                        Exit For
                    End If

                Next

                If bTrue = True Then
                    SelectAll("0")
                Else
                    SelectAll("1")
                End If

                Invalidate()
            End If

            '그룹지었을때 코딩 추가해야함.
            If info.InRow AndAlso m_View.IsGroupRow(info.RowHandle) AndAlso info.HitTest <> GridHitTest.RowGroupButton Then
                'Dim selected As Boolean = IsGroupRowSelected(info.RowHandle)
                MessageBox.Show("")
                'Dim dtTable As DataTable = New DataTable
                'If IsNothing(m_grdOBJ) Then
                '    'SelectGroup(info.RowHandle, selected)
                '    '   SelectGroup(info.RowHandle, (Not selected))
                'Else
                '    Dim strValue As String = m_View.GetGroupRowValue(info.RowHandle)
                '    Dim strColumn_NM = m_View.GroupedColumns.Item(0).FieldName

                '    dtTable = m_grdOBJ.DataSource
                '    If dtTable.Columns.Contains("CHK") Then
                '        Dim bBool As Boolean = (Not IsGroupRowSelected(info.RowHandle))
                '        Dim dr() As DataRow

                '        dr = dtTable.Select(strColumn_NM + "=" + "'" + strValue.ToString + "'")

                '        For i As Integer = 0 To m_View.GetChildRowCount(info.RowHandle) - 1
                '            Dim row As Object = m_View.GetRow(m_View.GetChildRowHandle(info.RowHandle, i))
                '            If bBool = False Then
                '                m_Selection.Remove(row)
                '            Else

                '                m_Selection.Add(row)
                '            End If
                '            dr(i)("CHK") = bBool
                '        Next
                '    Else
                '        SelectGroup(info.RowHandle, selected)
                '    End If
                'End If

            End If
        End If
    End Sub

End Class
