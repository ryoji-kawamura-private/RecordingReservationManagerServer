Imports RK.SchemaRecordingReservation

''' <summary>
''' ログ参照
''' </summary>
''' <remarks></remarks>
Public Class FormLog

#Region "定数・変数"
    ''' <summary>予約情報一覧</summary>
    Protected ReservationRow As SchemaRecordingReservation.T_RESERVATIONRow
    ''' <summary>ログ一覧</summary>
    Protected LogList As SchemaRecordingReservation.T_LOGDataTable
    ''' <summary>メインフォーム</summary>
    Protected MainForm As FormMain
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
        SetDataGrid()
    End Sub
#End Region

#Region "ボタン"
    ''' <summary>
    ''' 閉じるボタン押下
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    ''' <summary>
    ''' 削除ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDelete.Click
        Me.DeleteLog()
        Me.SetDataGrid()
    End Sub
#End Region

#Region "データグリッド"
    ''' <summary>
    ''' データバインド
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvReservation_DataBindingComplete(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewBindingCompleteEventArgs) Handles dgvReservation.DataBindingComplete
        For Each dgvColumn As DataGridViewColumn In Me.dgvReservation.Columns
            Dim columnProperty() As String = DirectCast(Me.dgvReservation.DataSource, SchemaRecordingReservation.T_LOGDataTable).Columns(dgvColumn.Index).Caption.Split(":"c)
            dgvColumn.HeaderText = columnProperty(0)
            dgvColumn.Visible = CType(columnProperty(1), Boolean)
        Next
    End Sub
#End Region

#End Region

#Region "外部メソッド"
    ''' <summary>
    ''' ダイアログとして表示
    ''' </summary>
    ''' <param name="parentForm"></param>
    ''' <param name="pReservationRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Overloads Function ShowDialog(ByVal parentForm As Form, ByVal pReservationRow As SchemaRecordingReservation.T_RESERVATIONRow) As System.Windows.Forms.DialogResult
        Me.ReservationRow = pReservationRow
        Return Me.ShowDialog(parentForm)
    End Function
#End Region

#Region "内部メソッド"
    ''' <summary>
    ''' 予約情報削除
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub DeleteLog()
        If System.Windows.Forms.DialogResult.OK.Equals(System.Windows.Forms.MessageBox.Show("削除しますか？", MESSAGEBOX_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, 0, False)) Then
            Me.MainForm.DeleteLog(Me.ReservationRow.ID)
        End If
    End Sub
    ''' <summary>
    ''' データグリッドにデータを設定
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetDataGrid()
        Me.LogList = Me.MainForm.GetLog(Me.ReservationRow)
        Me.dgvReservation.DataSource = Me.LogList
    End Sub
#End Region

End Class