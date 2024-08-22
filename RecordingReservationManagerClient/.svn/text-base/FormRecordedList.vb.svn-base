Imports RK.SchemaRecordingReservation

''' <summary>
''' 録画一覧
''' </summary>
''' <remarks></remarks>
Public Class FormRecordedList

#Region "定数・変数"
    ''' <summary>予約情報一覧</summary>
    Protected RecordedList As SchemaRecordingReservation.T_RESERVATIONDataTable
    ''' <summary>メインフォーム</summary>
    Protected MainForm As FormMain
    ''' <summary>保存パス</summary>
    Protected SavePath As String
#End Region

#Region "イベント"

#Region "フォーム"
    ''' <summary>
    ''' フォームロード
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        Dim ownerForm As Form = Me.Owner
        Do Until Me.MainForm IsNot Nothing OrElse ownerForm Is Nothing
            If TypeOf ownerForm Is FormMain Then
                Me.MainForm = DirectCast(ownerForm, FormMain)
            End If
            ownerForm = ownerForm.Owner
        Loop
        Me.SavePath = New SavePathConfiguration().SavePath
        SetDataGrid()
    End Sub
#End Region

#Region "ボタン"
    ''' <summary>
    ''' 再生ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnPlay_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnPlay.Click
        Me.PlayRecordingFile()
    End Sub
    ''' <summary>
    ''' 削除ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDelete.Click
        Me.DeleteRecordedFile()
    End Sub
    ''' <summary>
    ''' 閉じるボタン押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub
#End Region

#Region "データグリッド"
    ''' <summary>
    ''' データバインド
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DgvReservation_DataBindingComplete(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewBindingCompleteEventArgs) Handles DgvRecordedList.DataBindingComplete
        If System.ComponentModel.ListChangedType.Reset.Equals(e.ListChangedType) Then
            For Each dgvColumn As DataGridViewColumn In Me.DgvRecordedList.Columns
                Dim reservationCol As DataColumn = DirectCast(Me.DgvRecordedList.DataSource, SchemaRecordingReservation.T_RESERVATIONDataTable).Columns(dgvColumn.Index)
                Dim columnProperty() As String = reservationCol.Caption.Split(";"c)
                dgvColumn.HeaderText = columnProperty(0)
                dgvColumn.Visible = CType(columnProperty(1), Boolean)
            Next
        End If
    End Sub
    ''' <summary>
    ''' 録画一覧マウスクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DgvRecordedList_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DgvRecordedList.CellMouseDown
        If e.Button = Windows.Forms.MouseButtons.Right _
        AndAlso e.RowIndex >= 0 _
        AndAlso Me.DgvRecordedList.SelectedRows.Count = 1 Then
            Me.DgvRecordedList.ClearSelection()
            Me.DgvRecordedList.Rows(e.RowIndex).Selected() = True
            Me.DgvRecordedList.CurrentCell = Me.DgvRecordedList.Rows(e.RowIndex).Cells(Me.RecordedList.CHANNELColumn.Ordinal())
        End If
    End Sub
    ''' <summary>
    ''' 録画一覧ダブルクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DgvRecordedList_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DgvRecordedList.CellDoubleClick
        If Me.DgvRecordedList.SelectedRows.Count = 1 Then
            Me.PlayRecordingFile()
        End If
    End Sub
    ''' <summary>
    ''' メニューストリップ表示時の処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub GridMenuStrip_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles GridMenuStrip.Opening
        If Me.DgvRecordedList.SelectedRows.Count >= 2 Then
            Me.PlayToolStripMenuItem.Enabled() = False
            Me.LogToolStripMenuItem.Enabled() = False
        Else
            Me.PlayToolStripMenuItem.Enabled() = True
            Me.LogToolStripMenuItem.Enabled() = True
        End If
    End Sub
    ''' <summary>
    ''' 再生
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub PlayToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PlayToolStripMenuItem.Click
        Me.PlayRecordingFile()
    End Sub
    ''' <summary>
    ''' 削除ツールストリップ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        Me.DeleteRecordedFile()
    End Sub
    ''' <summary>
    ''' ログ参照
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub LogToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogToolStripMenuItem.Click
        Me.ShowLogForm()
    End Sub
#End Region

#End Region

#Region "外部メソッド"
    ''' <summary>
    ''' データグリッドにデータを設定
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub SetDataGrid()
        Dim sortColumnIndex As Int32? = Nothing
        If Me.DgvRecordedList.SortedColumn IsNot Nothing Then
            sortColumnIndex = Me.DgvRecordedList.SortedColumn.Index
        End If
        Dim sortDirection As SortOrder = Me.DgvRecordedList.SortOrder
        Me.RecordedList = Me.MainForm.GetRecordedList()
        Me.RecordedList.START_DISPColumn.Expression = Me.RecordedList.START_DISPColumn.Caption.Split(";"c)(2)
        Me.RecordedList.END_DISPColumn.Expression = Me.RecordedList.END_DISPColumn.Caption.Split(";"c)(2)
        Me.DgvRecordedList.DataSource = Me.RecordedList
        If sortColumnIndex IsNot Nothing Then
            Me.DgvRecordedList.Sort(Me.DgvRecordedList.Columns.Item(CType(sortColumnIndex, Int32)), _
                                    CType(sortDirection - 1, System.ComponentModel.ListSortDirection))
        Else
            Me.DgvRecordedList.Sort(Me.DgvRecordedList.Columns.Item(8), System.ComponentModel.ListSortDirection.Descending)
        End If
        Me.TopMost = True
        Me.Activate()
        Me.TopMost = False
    End Sub
#End Region

#Region "内部メソッド"
    ''' <summary>
    ''' ログダイアログ表示
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ShowLogForm()
        Using logForm As New FormLog
            With logForm
                If Me.DgvRecordedList.CurrentRow IsNot Nothing Then
                    Dim reservationRow As SchemaRecordingReservation.T_RESERVATIONRow _
                    = DirectCast(DirectCast(Me.DgvRecordedList.CurrentRow.DataBoundItem, DataRowView).Row, SchemaRecordingReservation.T_RESERVATIONRow)
                    If System.Windows.Forms.DialogResult.OK.Equals(.ShowDialog(Me, reservationRow)) Then
                        Me.SetDataGrid()
                    End If
                End If
            End With
        End Using
        Me.Activate()
    End Sub
    ''' <summary>
    ''' 録画情報削除
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub DeleteRecordedFile()
        If Me.DgvRecordedList.SelectedRows.Count > 0 Then
            Dim deleteReservationTable As New T_RESERVATIONDataTable()
            For Each dgrRecorded As DataGridViewRow In DgvRecordedList.SelectedRows
                Dim reservationRow As SchemaRecordingReservation.T_RESERVATIONRow = DirectCast(DirectCast(dgrRecorded.DataBoundItem, DataRowView).Row, SchemaRecordingReservation.T_RESERVATIONRow)
                deleteReservationTable.ImportRow(reservationRow)
            Next
            If System.Windows.Forms.MessageBox.Show("削除しますか？", MESSAGEBOX_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, 0, False) _
            = System.Windows.Forms.DialogResult.OK Then
                Me.MainForm.DeleteRecordingReservation(deleteReservationTable)
                Me.SetDataGrid()
            End If
        End If
        Me.Activate()
    End Sub
    ''' <summary>
    ''' 録画ファイル再生
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub PlayRecordingFile()
        If Me.DgvRecordedList.CurrentRow IsNot Nothing Then
            Dim savePath As String = IO.Path.Combine(Me.SavePath, _
            IO.Path.GetFileName(DirectCast(DirectCast(Me.DgvRecordedList.CurrentRow.DataBoundItem, DataRowView).Row, SchemaRecordingReservation.T_RESERVATIONRow).PATH))
            Try
                Dim Process As System.Diagnostics.Process = System.Diagnostics.Process.Start(savePath)
            Catch ex As System.ComponentModel.Win32Exception
                System.Windows.Forms.MessageBox.Show(ex.Message, MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
                Me.Activate()
            Catch ex As System.IO.FileNotFoundException
                System.Windows.Forms.MessageBox.Show("ファイルが削除されています。再生できません。", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
                Me.Activate()
            End Try
        End If
    End Sub
#End Region

End Class