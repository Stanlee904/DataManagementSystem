Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Columns
Imports DevExpress.Utils
Imports DevExpress.XtraGrid.Views.BandedGrid
Imports System.Data
Public Class DXGrid

    Dim m_bgrdvOBJ As BandedGridView

#Region "Grid Control"

    Private Property m_grdvOBJ As GridView

    ''' <summary>
    ''' 그리드에 일반 컬럼 추가
    ''' </summary>
    ''' <param name="grdvOBJ">대상 그리드</param>
    ''' <param name="sColName">컬럼명</param>
    ''' <param name="sFieldName">DB 필드명</param>
    ''' <param name="iWidth">컬럼 폭</param>
    ''' <param name="iAlignment">컬럼 정렬</param>
    ''' <param name="bVisible">컬럼 보이기/숨기기</param>
    ''' <remarks></remarks>
    Public Sub addGridColumn(ByRef grdvOBJ As GridView,
                             ByVal sColName As String,
                             ByVal sFieldName As String,
                             Optional ByVal iWidth As Integer = 100,
                             Optional ByVal bVisible As Boolean = True,
                             Optional ByVal iAlignment As DevExpress.Utils.HorzAlignment = DevExpress.Utils.HorzAlignment.Default,
                             Optional ByVal bMerge As Boolean = False)
        Dim gc As GridColumn = New GridColumn()
        With gc
            .FieldName = sFieldName
            .Caption = sColName
            .Width = iWidth
            .Visible = bVisible
            'If bMerge = True Then
            '    .OptionsColumn.AllowMerge = DefaultBoolean.True
            'Else
            '    .OptionsColumn.AllowMerge = DefaultBoolean.False
            'End If
        End With

        grdvOBJ.Columns.Add(gc)
        grdvOBJ.Columns(sFieldName).AppearanceCell.TextOptions.HAlignment = iAlignment
        grdvOBJ.Columns(sFieldName).OptionsColumn.AllowMerge = If(bMerge, DevExpress.Utils.DefaultBoolean.True, DevExpress.Utils.DefaultBoolean.False)

    End Sub

    ''' <summary>
    ''' 그리드에 체크박스 컬럼 추가
    ''' </summary>
    ''' <param name="grdvOBJ">대상 그리드</param>
    ''' <param name="sColName">컬럼명</param>
    ''' <param name="sFieldName">DB 필드명</param>
    ''' <param name="iWidth">컬럼 폭</param>
    ''' <param name="bTypeString">true : value는 "1" / "0", false : value는 true/false</param>
    ''' <remarks></remarks>
    Public Sub addCheckboxColumn(ByRef grdvOBJ As GridView,
                                 ByVal sColName As String,
                                 Optional ByVal sFieldName As String = "",
                                 Optional ByVal iWidth As Integer = 100,
                                 Optional ByVal bTypeString As Boolean = False)
        Dim gc As GridColumn = New GridColumn()
        With gc
            .FieldName = sFieldName
            .Caption = sColName
            .Width = iWidth
            .Visible = True
            .OptionsColumn.AllowMerge = DefaultBoolean.True
        End With

        Dim colEditor As New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()

        grdvOBJ.GridControl.RepositoryItems.Add(colEditor)
        gc.ColumnEdit = colEditor

        If bTypeString = True Then
            colEditor.ValueUnchecked = "0"
            colEditor.ValueChecked = "1"

        End If

        grdvOBJ.Columns.Add(gc)

    End Sub

    ''' <summary>
    ''' Vertical Grid 초기화
    ''' </summary>
    ''' <param name="vgrdOBJ">초기화 대상 그리드</param>
    ''' <param name="bEditable">수정가능여부</param>
    ''' <param name="bVisiable">Visible 여부</param>
    ''' <param name="bDragRowHeaders">Row 헤더 드래그 이동 가능 여부</param>
    ''' <param name="bHeaderRowShow">Row 헤더 Display 여부</param>
    ''' <param name="iRecordsInterval">레코드 사이 간격값</param>
    ''' <param name="iHeaderRowWidth">Row 헤더 넓이값</param>
    ''' <param name="iDataRowWidth">Row 높이값</param>
    ''' <param name="iTreeButtonStyle">계층구조 버튼 형식(1:Explorer Style, 2:Treeview Style, 0:Default Style</param>
    ''' <remarks></remarks>
    Public Sub initVerticalGrid(ByRef vgrdOBJ As DevExpress.XtraVerticalGrid.VGridControl,
                                Optional bEditable As Boolean = True,
                                Optional bVisiable As Boolean = True,
                                Optional bDragRowHeaders As Boolean = True,
                                Optional bHeaderRowShow As Boolean = True,
                                Optional iRecordsInterval As Integer = 0,
                                Optional iHeaderRowWidth As Integer = 200,
                                Optional iDataRowWidth As Integer = 150,
                                Optional iTreeButtonStyle As Integer = 1,
                                Optional bValidate As Boolean = False)
        Try
            With vgrdOBJ
                .OptionsBehavior.Editable = bEditable
                .OptionsBehavior.DragRowHeaders = bDragRowHeaders
                .OptionsView.ShowRows = bHeaderRowShow
                .RecordsInterval = iRecordsInterval
                .RecordWidth = iDataRowWidth
                .RowHeaderWidth = iHeaderRowWidth
                Select Case iTreeButtonStyle
                    Case 1
                        .TreeButtonStyle = DevExpress.XtraVerticalGrid.TreeButtonStyle.ExplorerBar
                    Case 2
                        .TreeButtonStyle = DevExpress.XtraVerticalGrid.TreeButtonStyle.TreeView
                    Case Else
                        .TreeButtonStyle = DevExpress.XtraVerticalGrid.TreeButtonStyle.Default
                End Select
                .TreeButtonStyle = DevExpress.XtraVerticalGrid.TreeButtonStyle.ExplorerBar
                .Visible = bVisiable

                If bValidate = True Then
                    AddHandler vgrdOBJ.ValidatingEditor, AddressOf vgrdOBJ_ValidatingEditor
                End If


            End With
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Vertical Grid 일반 Row 추가
    ''' </summary>
    ''' <param name="vgrdOBJ">Row가 추가 될 대상 Vertical 그리드</param>
    ''' <param name="sRowid">Row ID</param>
    ''' <param name="sCaption">헤더에 표시될 Caption</param>
    ''' <param name="sFieldName">Datatable Binding Field Name</param>
    ''' <param name="bVisible">Display 여부</param>
    ''' <param name="iHeight">row 높이값</param>
    ''' <remarks></remarks>
    Public Sub addVGridRow(ByRef vgrdOBJ As DevExpress.XtraVerticalGrid.VGridControl,
                            ByVal sRowid As String,
                            ByVal sCaption As String,
                            ByVal sFieldName As String,
                            Optional bVisible As Boolean = True,
                            Optional bReadonly As Boolean = False,
                            Optional iHeight As Integer = 16)
        Try
            Dim row As New DevExpress.XtraVerticalGrid.Rows.EditorRow
            With row
                .Name = sRowid
                .Height = iHeight
                .Properties.Caption = sCaption
                .Properties.ReadOnly = bReadonly
                '.Properties.RowEdit = Nothing
                .Properties.FieldName = sFieldName
                .Visible = bVisible
            End With
            vgrdOBJ.Rows.Add(row)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Vertical Grid 멀티플 Row 추가
    ''' </summary>
    ''' <param name="vgrdOBJ">Row가 추가 될 대상 Vertical 그리드</param>
    ''' <param name="sRowid">Row ID</param>
    ''' <param name="bVisible">Display 여부</param>
    ''' <param name="iSeparatorKind">멀티플 데이터 구분자 1:Line, 2:입력스트링</param>
    ''' <param name="sSeparatorString">멀티플 데이터 구분자 스트링(멀티플 데이터 구분자가 입력스트링인경우 사용)</param>
    ''' <remarks></remarks>
    Public Sub addVGridMultipleRow(ByRef vgrdOBJ As DevExpress.XtraVerticalGrid.VGridControl,
                                   ByVal sRowid As String,
                                   Optional bVisible As Boolean = True,
                                   Optional iSeparatorKind As Integer = 1,
                                   Optional sSeparatorString As String = "",
                                   Optional iHeight As Integer = 16)
        Try
            Dim row As New DevExpress.XtraVerticalGrid.Rows.MultiEditorRow
            Try
                With row
                    .Name = sRowid
                    If iSeparatorKind = 1 Then
                        .SeparatorKind = DevExpress.XtraVerticalGrid.Rows.SeparatorKind.VertLine
                    Else
                        .SeparatorKind = DevExpress.XtraVerticalGrid.Rows.SeparatorKind.String
                    End If

                    .SeparatorString = sSeparatorString
                    .Visible = bVisible
                    .Height = iHeight
                End With

                vgrdOBJ.Rows.Add(row)
            Catch ex As Exception
                Throw ex
            End Try
        Catch ex As Exception

        End Try
    End Sub

    ''' <summary>
    ''' Vertical Grid 멀티플 Row 항목 추가
    ''' </summary>
    ''' <param name="mrowOBJ">항목이 추가될 멀티플 row</param>
    ''' <param name="sFieldName">Datatable Binding Field Name</param>
    ''' <param name="sCaption">헤더에 표시될 Caption</param>
    ''' <param name="iCellWidth">row에서 추가하는 데이터가 차지하는 cell넓이</param>
    ''' <param name="iWidth">row헤더에서 추가하는 데이터헤더명이 차지하는 넓이</param>
    ''' <param name="bReadonly">읽기전용 여부</param>
    ''' <param name="iRowType">추가되는 row type 1:텍스트, 2:콤보, 3:라디오, 4:체크, 5:스핀, 6:날짜</param>
    ''' <param name="dtblDATA">콤보, 라디오 형태일 경우 멤버 데이터테이블</param>
    ''' <param name="sVALUE_FIELD">데이터 목록 DataTable의 값 Field 명</param>
    ''' <param name="sDISPLAY_FIELD">데이터 목록 DataTable의 Display Field 명</param>
    ''' <remarks></remarks>
    Public Sub addVGridMultipleRowMember(ByRef mrowOBJ As DevExpress.XtraVerticalGrid.Rows.MultiEditorRow,
                                         ByVal sFieldName As String,
                                         ByVal sCaption As String,
                                         ByVal iCellWidth As Integer,
                                         ByVal iWidth As Integer,
                                         Optional bReadonly As Boolean = False,
                                         Optional iRowType As Integer = 1,
                                         Optional ByRef dtblDATA As DataTable = Nothing,
                                         Optional sVALUE_FIELD As String = "COMN_CD",
                                         Optional sDISPLAY_FIELD As String = "COMN_CD_NM")
        Try
            Dim rowp As New DevExpress.XtraVerticalGrid.Rows.MultiEditorRowProperties
            Dim riText As New DevExpress.XtraEditors.Repository.RepositoryItemTextEdit()
            Dim riCombo As New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
            Dim riRadio As New DevExpress.XtraEditors.Repository.RepositoryItemRadioGroup()
            Dim riCheck As New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()
            Dim riSpin As New DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit()
            Dim riDate As New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()

            rowp.FieldName = sFieldName
            rowp.Caption = sCaption
            rowp.CellWidth = iCellWidth
            rowp.Width = iWidth
            rowp.ReadOnly = bReadonly

            Select Case iRowType
                Case 1  'TEXT Edit
                    rowp.RowEdit = riText
                Case 2  'Combobox Edit
                    riCombo.DataSource = dtblDATA
                    riCombo.ValueMember = sVALUE_FIELD
                    riCombo.DisplayMember = sDISPLAY_FIELD
                    rowp.RowEdit = riCombo
                Case 3  'Radio Edit
                    For i = 0 To dtblDATA.Rows.Count - 1
                        riRadio.Items.Add(New DevExpress.XtraEditors.Controls.RadioGroupItem(dtblDATA(i)(sVALUE_FIELD), dtblDATA(i)(sDISPLAY_FIELD)))
                    Next
                    rowp.RowEdit = riRadio
                Case 4  'Checkbox Edit
                    rowp.RowEdit = riCheck
                Case 5  'Spin Edit
                    rowp.RowEdit = riSpin
                Case 6  'Date Edit
                    rowp.RowEdit = riDate
                Case Else   'Text Edit
                    rowp.RowEdit = riText
            End Select

            mrowOBJ.PropertiesCollection.Add(rowp)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub vgrdOBJ_ValidatingEditor(ByVal sender As System.Object, ByVal e As DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs)
        If IsNothing(e.Value) Or e.Value.ToString = "" Then
            e.Value = Nothing
        End If
    End Sub
    Public Sub setColsEditable(ByRef grdvOBJ As DevExpress.XtraGrid.Views.Grid.GridView, ByVal FieldName() As String, Optional ByVal bEditable As Boolean = False)
        grdvOBJ.OptionsBehavior.Editable = True
        grdvOBJ.OptionsBehavior.ReadOnly = False

        For Each Col As DevExpress.XtraGrid.Columns.GridColumn In grdvOBJ.Columns
            Dim ColFieldName As String = Col.FieldName
            If (From Field In FieldName Where Field = ColFieldName Select Field).Count = 0 Then
                Col.OptionsColumn.ReadOnly = True
                Col.OptionsColumn.AllowEdit = bEditable
            End If
        Next
    End Sub
    ''' <summary>
    ''' 그리드 기본 속성 지정
    ''' </summary>
    ''' <param name="grdvOBJ">대상 그리드</param>
    ''' <param name="bReadOnly"></param>
    ''' <param name="bEditable"></param>
    ''' <param name="bShowGroupPanel">그룹패널 보이기/숨기기 여부</param>
    ''' <param name="bColumnAutoWidth">자동컬럼폭 화면에 맞게 조정 여부</param>
    ''' <param name="bAllowCellMerge">셀병합 허용</param>
    ''' <param name="bMultiSelect">다중 행 선택 허용</param>
    ''' <param name="bHeaderTextCenter">헤더 캡션 가운데 정렬</param>
    ''' <param name="iSortHeadMode">Sorting 할 때 Grid를 제일위에 고정 -2013.11.21 동호추가</param>
    ''' <param name="bValidate">숫자형 컬럼에 빈공간 넣었을 때 예외처리 -2013.11.21 동호추가</param>
    ''' <param name="bDelKey">Del키 입력 허용 -2013.11.21 동호추가 
    '''                      (그리드 자체적으로 Key_Down 이벤트를 사용하면 쓰지 말것 - 사용해도됨. 두군데 다 이벤트 함. 문제없음 확인 -2013.11.22)
    '''                      (CellValueChanged나 CellValueChanging 이벤트가 있는 그리드에서는 충분히 테스트 해볼 것)    </param>
    ''' <remarks></remarks>
    Public Sub initGrid(ByRef grdvOBJ As GridView,
                        Optional ByVal bReadOnly As Boolean = False,
                        Optional ByVal bEditable As Boolean = True,
                        Optional ByVal bShowGroupPanel As Boolean = False,
                        Optional ByVal bShowAutoFilterRow As Boolean = False,
                        Optional ByVal bColumnAutoWidth As Boolean = False,
                        Optional ByVal bAllowCellMerge As Boolean = False,
                        Optional ByVal bMultiSelect As Boolean = True,
                        Optional ByVal bCellSelect As Boolean = True,
                        Optional ByVal bCopyToClipboardWithColumnHeaders As Boolean = False,
                        Optional ByVal bHeaderTextCenter As Boolean = True,
                        Optional ByVal bColumnClear As Boolean = True,
                        Optional ByVal iIndicatorWidth As Integer = 0,
                        Optional ByVal iSortHeadMode As Integer = 0,
                        Optional ByVal bValidate As Boolean = False,
                        Optional ByVal bDelKey As Boolean = False,
                        Optional ByVal bPaste As Boolean = False
                        )

        With grdvOBJ
            If bColumnClear Then
                .Columns.Clear()
            End If
            .OptionsBehavior.ReadOnly = bReadOnly
            .OptionsBehavior.Editable = bEditable

            .OptionsView.ShowGroupPanel = bShowGroupPanel
            .OptionsView.ShowAutoFilterRow = bShowAutoFilterRow
            .OptionsView.ColumnAutoWidth = bColumnAutoWidth    '.BestFitColumns()
            .OptionsView.AllowCellMerge = bAllowCellMerge

            .OptionsSelection.MultiSelect = bMultiSelect
            If bCellSelect = True Then
                .OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect
            Else
                .OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect
            End If

            .OptionsBehavior.CopyToClipboardWithColumnHeaders = bCopyToClipboardWithColumnHeaders

            If bHeaderTextCenter Then
                .Appearance.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center
            End If

            If iIndicatorWidth > 0 Then
                .OptionsView.ShowIndicator = True  'DevExpress Default Value
                .IndicatorWidth = iIndicatorWidth
            Else
                .OptionsView.ShowIndicator = False  'DevExpress Default Value
            End If

            If iSortHeadMode = 1 Then
                m_grdvOBJ = grdvOBJ
                AddHandler grdvOBJ.EndSorting, AddressOf grdv_INFO_EndSorting
            End If

            If bValidate = True Then
                m_grdvOBJ = grdvOBJ
                AddHandler grdvOBJ.ValidatingEditor, AddressOf grdv_ValidatingEditor
            End If

            If bDelKey = True Then
                m_grdvOBJ = grdvOBJ
                AddHandler grdvOBJ.KeyDown, AddressOf grdv_KeyDown
            End If

            If bPaste = True Then
                m_grdvOBJ = grdvOBJ
                AddHandler grdvOBJ.KeyPress, AddressOf grdv_KeyPress
            End If

        End With
    End Sub

    Private Sub grdv_ValidatingEditor(sender As Object, e As DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs)
        'Throw New NotImplementedException
    End Sub

    Private Sub grdv_KeyPress(sender As Object, e As KeyPressEventArgs)
        'Throw New NotImplementedException
    End Sub

    Private Sub grdv_KeyDown(sender As Object, e As KeyEventArgs)
        'Throw New NotImplementedException
    End Sub

    Private Sub grdv_INFO_EndSorting(sender As Object, e As EventArgs)
        'Throw New NotImplementedException
    End Sub

    ''' <summary>
    ''' 그리드에 콤보 컬럼 추가
    ''' </summary>
    ''' <param name="grdvOBJ">대상 그리드</param>
    ''' <param name="sColName">그리드 컬럼명</param>
    ''' <param name="sFieldName">그리드 컬럼에 매핑되는 DB 필드명</param>
    ''' <param name="dtblSource">콤보박스에 표시된 데이터 2열(코드 키-코드 이름)</param>
    ''' <param name="sDisplayFieldName">코드 이름</param>
    ''' <param name="sValueFieldName">코드 키</param>
    ''' <param name="iWidth">그리드 컬럼 폭</param>
    ''' <param name="bVisible">표시여부</param>
    ''' <param name="iAlignment">정렬종류 (0 : String : 좌측정렬, Number : 우측정렬 / 1: 좌측정렬 / 2:가운데정렬 / 3: 우측정렬)</param>
    ''' <param name="bNullValue">전체선택(Combo Blank 추가)</param>
    ''' <param name="iTextMode">Text 모드 추가(0: Standard, 1:HideTextEditor, 2:DisableTextEditor) Default = 2</param>
    ''' <remarks></remarks>
    Public Sub addComboboxColumn(ByRef grdvOBJ As GridView,
                                 ByVal sColName As String,
                                 ByVal sFieldName As String,
                                 ByRef dtblSource As DataTable,
                                 ByVal sDisplayFieldName As String,
                                 ByVal sValueFieldName As String,
                                 Optional ByVal iWidth As Integer = 100,
                                 Optional ByVal bVisible As Boolean = True,
                                 Optional ByVal iAlignment As DevExpress.Utils.HorzAlignment = DevExpress.Utils.HorzAlignment.Default,
                                 Optional ByVal bNullValue As Boolean = False,
                                 Optional ByVal iTextMode As Integer = 2)
        Dim gc As GridColumn = New GridColumn()
        With gc
            .FieldName = sFieldName
            .Caption = sColName
            .Width = iWidth
            .Visible = bVisible
            .OptionsColumn.AllowMerge = DefaultBoolean.False


            .AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far
        End With

        Dim colEditor As New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
        grdvOBJ.GridControl.RepositoryItems.Add(colEditor)

        colEditor.Appearance.TextOptions.HAlignment = iAlignment

        gc.ColumnEdit = colEditor

        If bNullValue = True Then
            If dtblSource.Rows.Count > 0 Then
                Dim sRow As DataRow = dtblSource.NewRow()
                sRow(sDisplayFieldName) = ""
                sRow(sValueFieldName) = String.Empty
                dtblSource.Rows.InsertAt(sRow, 0)
            End If
        End If
        With colEditor
            .DataSource = dtblSource
            .DisplayMember = sDisplayFieldName
            .ValueMember = sValueFieldName
            .NullText = ""
            '.ShowHeader = False    '컬럼헤더 숨기기

            '2013-11-14 동호추가
            If iTextMode = 0 Then
                .TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
            ElseIf iTextMode = 1 Then
                .TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor
            Else
                .TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
            End If



            ' 단일 컬럼으로
            .Columns.Clear()
            .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo(sDisplayFieldName))

            .Columns(0).Caption = sColName
        End With

        grdvOBJ.Columns.Add(gc)

        If iAlignment = 2 Then
            For Each c As DevExpress.XtraGrid.Columns.GridColumn In grdvOBJ.Columns
                If c.Caption = sColName Then
                    c.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Center
                End If
            Next c
        ElseIf iAlignment = 1 Then
            For Each c As DevExpress.XtraGrid.Columns.GridColumn In grdvOBJ.Columns
                If c.Caption = sColName Then
                    c.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near
                End If
            Next c
        ElseIf iAlignment = 0 Then
            For Each c As DevExpress.XtraGrid.Columns.GridColumn In grdvOBJ.Columns
                If c.Caption = sColName Then
                    c.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Default
                End If
            Next c
        ElseIf iAlignment = 3 Then
            For Each c As DevExpress.XtraGrid.Columns.GridColumn In grdvOBJ.Columns
                If c.Caption = sColName Then
                    c.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Far
                End If
            Next c
        End If
    End Sub

#End Region

#Region "BandedGrid Control"

    ''' <summary>
    ''' 그리드 기본 속성 지정
    ''' </summary>
    ''' <param name="bgrdvOBJ">대상 그리드</param>
    ''' <param name="bReadOnly"></param>
    ''' <param name="bEditable"></param>
    ''' <param name="bShowGroupPanel">그룹패널 보이기/숨기기 여부</param>
    ''' <param name="bColumnAutoWidth">자동컬럼폭 화면에 맞게 조정 여부</param>
    ''' <param name="bAllowCellMerge">셀병합 허용</param>
    ''' <param name="bMultiSelect">다중 행 선택 허용</param>
    ''' <param name="bHeaderTextCenter">헤더 캡션 가운데 정렬</param>
    ''' <remarks></remarks>
    Public Sub initBandedGrid(ByRef bgrdvOBJ As BandedGridView,
                              Optional ByVal bReadOnly As Boolean = False,
                              Optional ByVal bEditable As Boolean = True,
                              Optional ByVal bShowGroupPanel As Boolean = False,
                              Optional ByVal bShowAutoFilterRow As Boolean = False,
                              Optional ByVal bColumnAutoWidth As Boolean = False,
                              Optional ByVal bAllowCellMerge As Boolean = False,
                              Optional ByVal bMultiSelect As Boolean = True,
                              Optional ByVal bCellSelect As Boolean = True,
                              Optional ByVal bCopyToClipboardWithColumnHeaders As Boolean = False,
                              Optional ByVal bHeaderTextCenter As Boolean = True,
                              Optional ByVal bColumnClear As Boolean = True,
                              Optional ByVal iIndicatorWidth As Integer = 0,
                              Optional ByVal iSortHeadMode As Integer = 0,
                              Optional ByVal bValidate As Boolean = False,
                              Optional ByVal bDelKey As Boolean = False)

        With bgrdvOBJ
            .Bands.Clear()
            If bColumnClear Then
                .Columns.Clear()
            End If

            .OptionsBehavior.ReadOnly = bReadOnly
            .OptionsBehavior.Editable = bEditable

            .OptionsView.ShowGroupPanel = bShowGroupPanel
            .OptionsView.ShowAutoFilterRow = bShowAutoFilterRow
            .OptionsView.ColumnAutoWidth = bColumnAutoWidth    '.BestFitColumns()
            .OptionsView.AllowCellMerge = bAllowCellMerge

            .OptionsSelection.MultiSelect = bMultiSelect
            If bCellSelect = True Then
                .OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect
            Else
                .OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect
            End If

            .OptionsBehavior.CopyToClipboardWithColumnHeaders = bCopyToClipboardWithColumnHeaders

            If bHeaderTextCenter Then
                .Appearance.BandPanel.TextOptions.HAlignment = HorzAlignment.Center
                .Appearance.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center
            End If

            If iIndicatorWidth > 0 Then
                .OptionsView.ShowIndicator = True  'DevExpress Default Value
                .IndicatorWidth = iIndicatorWidth
            Else
                .OptionsView.ShowIndicator = False  'DevExpress Default Value
            End If

            If iSortHeadMode = 1 Then
                m_bgrdvOBJ = bgrdvOBJ
                AddHandler bgrdvOBJ.EndSorting, AddressOf bgrdv_INFO_EndSorting
            End If

            If bValidate = True Then
                m_bgrdvOBJ = bgrdvOBJ
                AddHandler bgrdvOBJ.ValidatingEditor, AddressOf bgrdv_ValidatingEditor
            End If

            If bDelKey = True Then
                m_bgrdvOBJ = bgrdvOBJ
                AddHandler bgrdvOBJ.KeyDown, AddressOf bgrdv_KeyDown
            End If
            '.OptionsBehavior.CopyToClipboardWithColumnHeaders = False

            '.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect
            '.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseUp
        End With
    End Sub

    Private Sub bgrdv_KeyDown(sender As System.Object, e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Delete Then
            deleteGridCell(m_bgrdvOBJ)
        End If
    End Sub

    Private Sub bgrdv_ValidatingEditor(sender As System.Object, e As DevExpress.XtraEditors.Controls.BaseContainerValidateEditorEventArgs)
        Dim View As DevExpress.XtraGrid.Views.Grid.GridView = CType(sender, DevExpress.XtraGrid.Views.Grid.GridView)
        If View.FocusedColumn.ColumnType.ToString.Contains("Decimal") Or View.FocusedColumn.ColumnType.ToString.Contains("Int") Then
            If IsNothing(e.Value) Or e.Value.ToString = "" Then
                e.Value = Nothing
            End If
        End If
    End Sub


    Private Sub bgrdv_INFO_EndSorting(sender As System.Object, e As System.EventArgs)
        m_bgrdvOBJ.FocusedRowHandle = 0
    End Sub

    ''' <summary>
    ''' 그리드에 일반 컬럼 추가
    ''' </summary>
    ''' <param name="bgrdvOBJ">대상 그리드</param>
    ''' <param name="sColName">컬럼명</param>
    ''' <param name="sFieldName">DB 필드명</param>
    ''' <param name="iWidth">컬럼 폭</param>
    ''' <param name="bVisible">컬럼 보이기/숨기기</param>
    ''' <param name="iAlignment">컬럼 정렬</param>
    ''' <remarks></remarks>
    Public Sub addBandedGridColumn(ByRef bgrdvOBJ As BandedGridView,
                                   ByRef gridBand As GridBand,
                                   ByVal sColName As String,
                                   ByVal sFieldName As String,
                                   Optional ByVal iWidth As Integer = 100,
                                   Optional ByVal bVisible As Boolean = True,
                                   Optional ByVal iAlignment As DevExpress.Utils.HorzAlignment = DevExpress.Utils.HorzAlignment.Default,
                                   Optional ByVal bMerge As Boolean = False,
                                   Optional ByVal ftFormatType As DevExpress.Utils.FormatType = FormatType.None,
                                   Optional ByVal sFormatString As String = "")
        Dim gc As BandedGridColumn = New BandedGridColumn()
        With gc
            .FieldName = sFieldName
            .Caption = sColName
            .Width = iWidth
            .Visible = bVisible
            If bMerge = True Then
                .OptionsColumn.AllowMerge = DefaultBoolean.True
            Else
                .OptionsColumn.AllowMerge = DefaultBoolean.False
            End If

            .DisplayFormat.FormatType = ftFormatType
            If sFormatString <> "" Then
                .DisplayFormat.FormatString = sFormatString
            End If
        End With

        bgrdvOBJ.Columns.AddRange(New DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn() {gc})
        'gridBand.Caption = BandGroupName
        gridBand.Columns.Add(gc)
        bgrdvOBJ.Columns(sFieldName).AppearanceCell.TextOptions.HAlignment = iAlignment

    End Sub


    ''' <summary>
    ''' 밴디드그리드에 체크박스 컬럼 추가
    ''' </summary>
    ''' <param name="BGridView">대상 그리드</param>
    ''' <param name="gridBand">대상 그리드</param>
    ''' <param name="sColName">컬럼명</param>
    ''' <param name="sFieldName">DB 필드명</param>
    ''' <param name="iWidth">컬럼 폭</param>
    ''' <param name="bTypeString">true : value는 "1" / "0", false : value는 true/false</param>
    ''' <remarks></remarks>
    Public Sub addCheckboxColumn(ByRef BGridView As BandedGridView,
                                 ByRef gridBand As GridBand,
                                 ByVal sColName As String,
                                 Optional ByVal sFieldName As String = "",
                                 Optional ByVal iWidth As Integer = 100,
                                 Optional ByVal bTypeString As Boolean = False)
        Dim gc As BandedGridColumn = New BandedGridColumn()
        With gc
            .FieldName = sFieldName
            .Caption = sColName
            .Width = iWidth
            .Visible = True
            .OptionsColumn.AllowMerge = DefaultBoolean.False
        End With

        Dim colEditor As New DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit()
        'gridBand.GridControl.RepositoryItems.Add(colEditor)
        'gridBand.Columns
        gc.ColumnEdit = colEditor

        BGridView.Columns.AddRange(New DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn() {gc})
        gridBand.Columns.Add(gc)

        If bTypeString = True Then
            Dim CE As DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit = BGridView.Columns(sFieldName).ColumnEdit
            CE.ValueUnchecked = "0"
            CE.ValueChecked = "1"
        End If
    End Sub



    ''' <summary>
    ''' BandedGridView의 특정 Column만 키입력 가능하도록 속성 변경
    ''' </summary>
    ''' <param name="BGV">속성 변경 대상 BandedGridView</param>
    ''' <param name="FieldName">Editable 대상 Column의 FieldName</param>
    ''' <param name="bEditable">나머지 Column의 Editable 여부</param>
    ''' <remarks>입력받은 GridView는 강제로 ReadOnly:False / Editable:True 로 변경됨</remarks>
    Public Sub setColsEditable(ByRef BGV As DevExpress.XtraGrid.Views.BandedGrid.BandedGridView,
                               ByVal FieldName() As String, Optional ByVal bEditable As Boolean = False)
        BGV.OptionsBehavior.Editable = True
        BGV.OptionsBehavior.ReadOnly = False

        For Each Col As DevExpress.XtraGrid.Columns.GridColumn In BGV.Columns
            Dim ColFieldName As String = Col.FieldName
            If (From Field In FieldName Where Field = ColFieldName Select Field).Count = 0 Then
                Col.OptionsColumn.ReadOnly = True
                Col.OptionsColumn.AllowEdit = bEditable
            End If
        Next
    End Sub



    ''' <summary>
    ''' 그리드 컬럼 Default.xml 파일 작성 함수
    ''' </summary>
    ''' <param name="frmOBJ">그리드가 포함되어 있는 해당 폼</param>
    ''' <param name="bgrdvOBJ">해당 그리드 뷰</param>
    ''' <remarks>
    ''' 그리드 기본 컬럼 정보는 해당 로컬 pc의 프로그램 경로/폼명/그리드뷰명/DEFAULT.xml로 저장됨
    ''' 추후 유지보수시 컬럼 순서가 바뀔 수 있기 때문에
    ''' DEFAULT.xml파일 존재시 저장 하지 않는 기능 주석처리
    ''' </remarks>
    Public Sub saveDefaultGrid(ByVal sProject As String,
                               ByRef frmOBJ As Form,
                               ByRef bgrdvOBJ As DevExpress.XtraGrid.Views.BandedGrid.BandedGridView)
        Dim sPath As String = "./XML/" & sProject & "/" & Replace(Mid(frmOBJ.Text, 2, 5), "]", "") & "/" & bgrdvOBJ.Name
        Dim sFile As String = ""
        Dim dirXML As New IO.DirectoryInfo(sPath)

        If dirXML.Exists = False Then
            dirXML.Create()
        End If

        sFile = sPath & "/Default.xml"

        bgrdvOBJ.OptionsLayout.Columns.RemoveOldColumns = False
        bgrdvOBJ.OptionsLayout.Columns.StoreAllOptions = True
        bgrdvOBJ.SaveLayoutToXml(sFile)
        bgrdvOBJ.OptionsLayout.Columns.RemoveOldColumns = True
        bgrdvOBJ.OptionsLayout.Columns.StoreAllOptions = False
    End Sub

    ''' <summary>
    ''' 그리드에 콤보 컬럼 추가
    ''' </summary>
    ''' <param name="bgrdvOBJ">대상 그리드</param>
    ''' <param name="gridBand">대상 그리드</param>
    ''' <param name="sColName">그리드 컬럼명</param>
    ''' <param name="sFieldName">그리드 컬럼에 매핑되는 DB 필드명</param>
    ''' <param name="dtblSource">콤보박스에 표시된 데이터 2열(코드 키-코드 이름)</param>
    ''' <param name="sDisplayFieldName">코드 이름</param>
    ''' <param name="sValueFieldName">코드 키</param>
    ''' <param name="iWidth">그리드 컬럼 폭</param>
    ''' <param name="bVisible">표시여부</param>
    ''' <param name="iAlignment">정렬종류</param>
    ''' <remarks></remarks>
    Public Sub addComboboxColumn(ByRef bgrdvOBJ As BandedGridView,
                                 ByRef gridBand As GridBand,
                                 ByVal sColName As String,
                                 ByVal sFieldName As String,
                                 ByRef dtblSource As DataTable,
                                 ByVal sDisplayFieldName As String,
                                 ByVal sValueFieldName As String,
                                 Optional ByVal iWidth As Integer = 100,
                                 Optional ByVal bVisible As Boolean = True,
                                 Optional ByVal iAlignment As DevExpress.Utils.HorzAlignment = DevExpress.Utils.HorzAlignment.Default)
        Dim gc As BandedGridColumn = New BandedGridColumn()
        With gc
            .FieldName = sFieldName
            .Caption = sColName
            .Width = iWidth
            .Visible = bVisible
            .OptionsColumn.AllowMerge = DefaultBoolean.False
        End With

        Dim colEditor As New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()
        bgrdvOBJ.GridControl.RepositoryItems.Add(colEditor)
        colEditor.Appearance.TextOptions.HAlignment = iAlignment

        gc.ColumnEdit = colEditor

        With colEditor
            .DataSource = dtblSource
            .DisplayMember = sDisplayFieldName
            .ValueMember = sValueFieldName
            .NullText = ""
            '.ShowHeader = False    '컬럼헤더 숨기기

            ' 단일 컬럼으로
            .Columns.Clear()
            .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo(sDisplayFieldName))

            .Columns(0).Caption = sColName
        End With

        bgrdvOBJ.Columns.AddRange(New DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn() {gc})
        gridBand.Columns.Add(gc)
        'bgrdvOBJ.Columns.Add(gc)
    End Sub


#End Region

#Region "Delete Key Event"
    'DelKey gridv용
    '컬럼 속성이 ReadOnly = True, Editable = False 면 지우지 않음.
    Public Shared Sub deleteGridCell(ByVal grdvOBJ As DevExpress.XtraGrid.Views.Grid.GridView)

        Dim grdCellSelectCell() As DevExpress.XtraGrid.Views.Base.GridCell
        Dim iSelectRow() As Integer

        With grdvOBJ
            If .OptionsSelection.MultiSelect = True Then
                'If Col.OptionsColumn.ReadOnly = False Then
                If .OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect Then
                    grdCellSelectCell = grdvOBJ.GetSelectedCells
                    For i = 0 To grdCellSelectCell.Length - 1
                        If DirectCast(grdCellSelectCell(i), DevExpress.XtraGrid.Views.Base.GridCell).Column.OptionsColumn.AllowEdit = True And DirectCast(grdCellSelectCell(i), DevExpress.XtraGrid.Views.Base.GridCell).Column.OptionsColumn.ReadOnly = False Then
                            grdvOBJ.SetRowCellValue(DirectCast(grdCellSelectCell(i), DevExpress.XtraGrid.Views.Base.GridCell).RowHandle, DirectCast(grdCellSelectCell(i), DevExpress.XtraGrid.Views.Base.GridCell).Column, Nothing)
                        End If
                    Next
                ElseIf .OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect Then
                    iSelectRow = grdvOBJ.GetSelectedRows
                    For i = 0 To iSelectRow.Length - 1
                        For j = 0 To grdvOBJ.Columns.Count - 1
                            If grdvOBJ.Columns(j).OptionsColumn.AllowEdit = True And grdvOBJ.Columns(j).OptionsColumn.ReadOnly = False Then
                                grdvOBJ.SetRowCellValue(iSelectRow(i), grdvOBJ.Columns(j).FieldName, Nothing)
                            End If
                        Next
                    Next
                End If
            Else
                If .OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect Then
                    grdCellSelectCell = grdvOBJ.GetSelectedCells
                    For i = 0 To grdCellSelectCell.Length - 1
                        If DirectCast(grdCellSelectCell(i), DevExpress.XtraGrid.Views.Base.GridCell).Column.OptionsColumn.AllowEdit = True And DirectCast(grdCellSelectCell(i), DevExpress.XtraGrid.Views.Base.GridCell).Column.OptionsColumn.ReadOnly = False Then
                            grdvOBJ.SetRowCellValue(DirectCast(grdCellSelectCell(i), DevExpress.XtraGrid.Views.Base.GridCell).RowHandle, DirectCast(grdCellSelectCell(i), DevExpress.XtraGrid.Views.Base.GridCell).Column, Nothing)
                        End If
                    Next
                ElseIf .OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect Then
                    iSelectRow = grdvOBJ.GetSelectedRows
                    For i = 0 To iSelectRow.Length - 1
                        For j = 0 To grdvOBJ.Columns.Count - 1
                            If grdvOBJ.Columns(j).OptionsColumn.AllowEdit = True And grdvOBJ.Columns(j).OptionsColumn.ReadOnly = False Then
                                grdvOBJ.SetRowCellValue(iSelectRow(i), grdvOBJ.Columns(j).FieldName, Nothing)
                            End If
                        Next
                    Next
                End If
            End If
        End With
    End Sub

    'DelKey bgrdv용
    Public Shared Sub deleteGridCell(ByVal bgrdvOBJ As DevExpress.XtraGrid.Views.BandedGrid.BandedGridView)

        Dim grdCellSelectCell() As DevExpress.XtraGrid.Views.Base.GridCell
        Dim iSelectRow() As Integer

        With bgrdvOBJ
            If .OptionsSelection.MultiSelect = True Then
                'If Col.OptionsColumn.ReadOnly = False Then
                If .OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect Then
                    grdCellSelectCell = bgrdvOBJ.GetSelectedCells
                    For i = 0 To grdCellSelectCell.Length - 1
                        If DirectCast(grdCellSelectCell(i), DevExpress.XtraGrid.Views.Base.GridCell).Column.OptionsColumn.AllowEdit = True And DirectCast(grdCellSelectCell(i), DevExpress.XtraGrid.Views.Base.GridCell).Column.OptionsColumn.ReadOnly = False Then
                            bgrdvOBJ.SetRowCellValue(DirectCast(grdCellSelectCell(i), DevExpress.XtraGrid.Views.Base.GridCell).RowHandle, DirectCast(grdCellSelectCell(i), DevExpress.XtraGrid.Views.Base.GridCell).Column, Nothing)
                        End If
                    Next
                ElseIf .OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect Then
                    iSelectRow = bgrdvOBJ.GetSelectedRows
                    For i = 0 To iSelectRow.Length - 1
                        For j = 0 To bgrdvOBJ.Columns.Count - 1
                            If bgrdvOBJ.Columns(j).OptionsColumn.AllowEdit = True And bgrdvOBJ.Columns(j).OptionsColumn.ReadOnly = False Then
                                bgrdvOBJ.SetRowCellValue(iSelectRow(i), bgrdvOBJ.Columns(j).FieldName, Nothing)
                            End If
                        Next
                    Next
                End If
            Else
                If .OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect Then
                    grdCellSelectCell = bgrdvOBJ.GetSelectedCells
                    For i = 0 To grdCellSelectCell.Length - 1
                        If DirectCast(grdCellSelectCell(i), DevExpress.XtraGrid.Views.Base.GridCell).Column.OptionsColumn.AllowEdit = True And DirectCast(grdCellSelectCell(i), DevExpress.XtraGrid.Views.Base.GridCell).Column.OptionsColumn.ReadOnly = False Then
                            bgrdvOBJ.SetRowCellValue(DirectCast(grdCellSelectCell(i), DevExpress.XtraGrid.Views.Base.GridCell).RowHandle, DirectCast(grdCellSelectCell(i), DevExpress.XtraGrid.Views.Base.GridCell).Column, Nothing)
                        End If
                    Next
                ElseIf .OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect Then
                    iSelectRow = bgrdvOBJ.GetSelectedRows
                    For i = 0 To iSelectRow.Length - 1
                        For j = 0 To bgrdvOBJ.Columns.Count - 1
                            If bgrdvOBJ.Columns(j).OptionsColumn.AllowEdit = True And bgrdvOBJ.Columns(j).OptionsColumn.ReadOnly = False Then
                                bgrdvOBJ.SetRowCellValue(iSelectRow(i), bgrdvOBJ.Columns(j).FieldName, Nothing)
                            End If
                        Next
                    Next
                End If
            End If
        End With
    End Sub

#End Region

#Region "VerticalGrid Control"
    ''' <summary>
    ''' Vertical Grid Date Row 추가
    ''' </summary>
    ''' <param name="vgrdOBJ">Row가 추가 될 대상 Vertical 그리드</param>
    ''' <param name="sRowid">Row ID</param>
    ''' <param name="sCaption">헤더에 표시될 Caption</param>
    ''' <param name="sFieldName">Datatable Binding Field Name</param>
    ''' <param name="bVisible">Display 여부</param>
    ''' <param name="iHeight">row 높이값</param>
    ''' <remarks></remarks>
    Public Sub addVGridDateRow(ByRef vgrdOBJ As DevExpress.XtraVerticalGrid.VGridControl,
                            ByVal sRowid As String,
                            ByVal sCaption As String,
                            ByVal sFieldName As String,
                            Optional bVisible As Boolean = True,
                            Optional bReadonly As Boolean = False,
                            Optional iHeight As Integer = 16)
        Try
            Dim row As New DevExpress.XtraVerticalGrid.Rows.EditorRow
            Dim riDate As New DevExpress.XtraEditors.Repository.RepositoryItemDateEdit()

            With row
                .Name = sRowid
                .Height = iHeight
                .Properties.Caption = sCaption
                .Properties.ReadOnly = bReadonly
                '.Properties.RowEdit = Nothing
                .Properties.FieldName = sFieldName
                .Visible = bVisible
                .Properties.RowEdit = riDate
            End With
            vgrdOBJ.Rows.Add(row)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Vertical Grid 콤보 row 추가
    ''' </summary>
    ''' <param name="vgrdOBJ">Row가 추가 될 대상 Vertical 그리드</param>
    ''' <param name="sRowid">Row ID</param>
    ''' <param name="sCaption">헤더에 표시될 Caption</param>
    ''' <param name="sFieldName">Datatable Binding Field Name</param>
    ''' <param name="dtblDATA">콤보박스 데이터 목록 DataTable</param>
    ''' <param name="sVALUE_FIELD">콤보박스 데이터 목록 DataTable의 값 Field 명</param>
    ''' <param name="sDISPLAY_FIELD">콤보박스 데이터 목록 DataTable의 Display Field 명</param>
    ''' <param name="bVisible">Display 여부</param>
    ''' <param name="iHeight">row 높이값</param>
    ''' <remarks></remarks>
    Public Sub addVGridComboRow(ByRef vgrdOBJ As DevExpress.XtraVerticalGrid.VGridControl,
                                ByVal sRowid As String,
                                ByVal sCaption As String,
                                ByVal sFieldName As String,
                                ByRef dtblDATA As DataTable,
                                ByVal sVALUE_FIELD As String,
                                ByVal sDISPLAY_FIELD As String,
                                Optional bVisible As Boolean = True,
                                Optional bReadOnly As Boolean = False,
                                Optional iHeight As Integer = 16,
                                Optional bNullValue As Boolean = False)
        Try
            Dim row As New DevExpress.XtraVerticalGrid.Rows.EditorRow
            Dim riCombo As New DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit()

            If bNullValue = True Then
                If dtblDATA.Rows.Count > 0 Then
                    Dim sRow As DataRow = dtblDATA.NewRow()
                    sRow(sDISPLAY_FIELD) = ""
                    sRow(sVALUE_FIELD) = String.Empty
                    dtblDATA.Rows.InsertAt(sRow, 0)
                End If
            End If

            With riCombo
                .DataSource = dtblDATA
                .DisplayMember = sDISPLAY_FIELD
                .ValueMember = sVALUE_FIELD
                .ShowHeader = False
                .NullText = ""
                .Columns.Clear()
                .Columns.Add(New DevExpress.XtraEditors.Controls.LookUpColumnInfo(.DisplayMember))
            End With

            With row
                .Name = sRowid
                .Visible = True
                .Height = iHeight
                .Properties.Caption = sCaption
                .Properties.ReadOnly = bReadOnly
                .Properties.RowEdit = riCombo
                .Properties.FieldName = sFieldName
                .Visible = bVisible
            End With


            vgrdOBJ.Rows.Add(row)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    ''' <summary>
    ''' Vertical Grid Search 콤보 row 추가
    ''' </summary>
    ''' <param name="vgrdOBJ">Row가 추가 될 대상 Vertical 그리드</param>
    ''' <param name="sRowid">Row ID</param>
    ''' <param name="sCaption">헤더에 표시될 Caption</param>
    ''' <param name="sFieldName">Datatable Binding Field Name</param>
    ''' <param name="dtblDATA">콤보박스 데이터 목록 DataTable</param>
    ''' <param name="sVALUE_FIELD">콤보박스 데이터 목록 DataTable의 값 Field 명</param>
    ''' <param name="sDISPLAY_FIELD">콤보박스 데이터 목록 DataTable의 Display Field 명</param>
    ''' <param name="bVisible">Display 여부</param>
    ''' <param name="iHeight">row 높이값</param>
    ''' <remarks></remarks>
    Public Sub addVGridSearchRow(ByRef vgrdOBJ As DevExpress.XtraVerticalGrid.VGridControl,
                                 ByVal sRowid As String,
                                 ByVal sCaption As String,
                                 ByVal sFieldName As String,
                                 ByRef dtblDATA As DataTable,
                                 ByVal sVALUE_FIELD As String,
                                 ByVal sDISPLAY_FIELD As String,
                                 Optional bVisible As Boolean = True,
                                 Optional bReadOnly As Boolean = False,
                                 Optional iHeight As Integer = 16)
        Try
            Dim row As New DevExpress.XtraVerticalGrid.Rows.EditorRow
            Dim riCombo As New DevExpress.XtraEditors.Repository.RepositoryItemSearchLookUpEdit()

            With riCombo
                .DataSource = dtblDATA
                .DisplayMember = sDISPLAY_FIELD
                .ValueMember = sVALUE_FIELD
                .NullText = ""
            End With

            With row
                .Name = sRowid
                .Visible = True
                .Height = iHeight
                .Properties.Caption = sCaption
                .Properties.ReadOnly = bReadOnly
                .Properties.RowEdit = riCombo
                .Properties.FieldName = sFieldName
                .Visible = bVisible
            End With
            vgrdOBJ.Rows.Add(row)
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

#End Region

End Class
