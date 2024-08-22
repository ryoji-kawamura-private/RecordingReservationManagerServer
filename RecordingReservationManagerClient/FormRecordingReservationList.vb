Imports RK.SchemaRecordingReservation

''' <summary>
''' �\��ꗗ
''' </summary>
''' <remarks></remarks>
Public Class FormRecordingReservationList

#Region "�萔�E�ϐ�"
    ''' <summary>�\����ꗗ</summary>
    Protected ReservationList As SchemaRecordingReservation.T_RESERVATIONDataTable
    ''' <summary>���C���t�H�[��</summary>
    Protected MainForm As FormMain
#End Region

#Region "�C�x���g"

#Region "�t�H�[��"
    ''' <summary>
    ''' �t�H�[�����[�h
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

#Region "�{�^��"
    ''' <summary>
    ''' ����{�^������
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnClose.Click
        Me.Close()
    End Sub
    ''' <summary>
    ''' �ҏW�{�^��
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnEdit.Click
        Me.ShowRecordingReservationRegistForm()
    End Sub
    ''' <summary>
    ''' �폜�{�^��
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDelete.Click
        DeleteReservation()
    End Sub
#End Region

#Region "�f�[�^�O���b�h"
    ''' <summary>
    ''' �f�[�^�o�C���h
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvReservation_DataBindingComplete(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewBindingCompleteEventArgs) Handles DgvReservation.DataBindingComplete
        If System.ComponentModel.ListChangedType.Reset.Equals(e.ListChangedType) Then
            For Each dgvColumn As DataGridViewColumn In Me.DgvReservation.Columns
                Dim columnProperty() As String = DirectCast(Me.DgvReservation.DataSource, SchemaRecordingReservation.T_RESERVATIONDataTable).Columns(dgvColumn.Index).Caption.Split(";"c)
                dgvColumn.HeaderText = columnProperty(0)
                dgvColumn.Visible = CType(columnProperty(1), Boolean)
            Next
        End If
    End Sub
    ''' <summary>
    ''' �\��ꗗ�_�u���N���b�N
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DgvReservation_CellDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DgvReservation.CellDoubleClick
        If Me.DgvReservation.SelectedRows.Count() = 1 Then
            Me.ShowRecordingReservationRegistForm()
        End If
    End Sub
    ''' <summary>
    ''' �\��ꗗ�}�E�X�_�E��
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DgvReservation_CellMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellMouseEventArgs) Handles DgvReservation.CellMouseDown
        If e.Button = Windows.Forms.MouseButtons.Right _
        AndAlso e.RowIndex >= 0 _
        AndAlso Me.DgvReservation.SelectedRows.Count = 1 Then
            Me.DgvReservation.ClearSelection()
            Me.DgvReservation.Rows(e.RowIndex).Selected = True
            Me.DgvReservation.CurrentCell = Me.DgvReservation.Rows(e.RowIndex).Cells(ReservationList.CHANNELColumn.Ordinal())
        End If
    End Sub
    ''' <summary>
    ''' ���j���[�X�g���b�v�\�����̏���
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub GridMenuStrip_Opening(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles GridMenuStrip.Opening
        If Me.DgvReservation.SelectedRows.Count >= 2 Then
            Me.EditToolStripMenuItem.Enabled = False
            Me.LogToolStripMenuItem.Enabled = False
        Else
            Me.EditToolStripMenuItem.Enabled = True
            Me.LogToolStripMenuItem.Enabled = True
        End If
    End Sub
    ''' <summary>
    ''' �ҏW�c�[���X�g���b�v
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub EditToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EditToolStripMenuItem.Click
        Me.ShowRecordingReservationRegistForm()
    End Sub
    ''' <summary>
    ''' �폜�c�[���X�g���b�v
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteToolStripMenuItem.Click
        Me.DeleteReservation()
    End Sub
    ''' <summary>
    ''' ���O�Q��
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub LogToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogToolStripMenuItem.Click
        Me.ShowLogForm()
    End Sub
#End Region

#End Region

#Region "�O�����\�b�h"
    ''' <summary>
    ''' �f�[�^�O���b�h�Ƀf�[�^��ݒ�
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub SetDataGrid()
        Me.TopMost = True
        Me.ReservationList = Me.MainForm.GetReservation()
        Me.ReservationList.START_DISPColumn.Expression = Me.ReservationList.START_DISPColumn.Caption.Split(";"c)(2)
        Me.ReservationList.END_DISPColumn.Expression = Me.ReservationList.END_DISPColumn.Caption.Split(";"c)(2)
        Me.DgvReservation.DataSource = Me.ReservationList
        Me.TopMost = True
        Me.Activate()
        Me.TopMost = False
    End Sub
#End Region

#Region "�������\�b�h"
    ''' <summary>
    ''' �ҏW�_�C�A���O�\��
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ShowRecordingReservationRegistForm()
        Using reservationRegistForm As New FormRegistRecordingReservation
            With reservationRegistForm
                If Me.DgvReservation.CurrentRow IsNot Nothing Then
                    Dim reservationRow As SchemaRecordingReservation.T_RESERVATIONRow _
                    = DirectCast(DirectCast(Me.DgvReservation.CurrentRow.DataBoundItem, DataRowView).Row, SchemaRecordingReservation.T_RESERVATIONRow)
                    'If (2L).Equals(reservationRow.RECORDED) Then
                    '    System.Windows.Forms.MessageBox.Show("�^�撆�ł��B�ҏW�ł��܂���B", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
                    '    Return
                    'End If
                    If .ShowDialog(Me, reservationRow) = System.Windows.Forms.DialogResult.OK Then
                        Me.SetDataGrid()
                    End If
                End If
            End With
        End Using
        Me.Activate()
    End Sub
    ''' <summary>
    ''' ���O�_�C�A���O�\��
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ShowLogForm()
        Using logForm As New FormLog
            With logForm
                If Me.DgvReservation.CurrentRow IsNot Nothing Then
                    Dim reservationRow As SchemaRecordingReservation.T_RESERVATIONRow _
                    = DirectCast(DirectCast(Me.DgvReservation.CurrentRow.DataBoundItem, DataRowView).Row, SchemaRecordingReservation.T_RESERVATIONRow)
                    If Not (reservationRow.RECORDED = 1L) Then
                        If System.Windows.Forms.DialogResult.OK.Equals(.ShowDialog(Me, reservationRow)) Then
                            Me.SetDataGrid()
                        End If
                    End If
                End If
            End With
        End Using
        Me.Activate()
    End Sub
    ''' <summary>
    ''' �\����폜
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub DeleteReservation()
        If Me.DgvReservation.SelectedRows.Count > 0 Then
            Dim deleteReservationTable As New T_RESERVATIONDataTable()
            For Each dgrReservation As DataGridViewRow In DgvReservation.SelectedRows
                Dim reservationRow As SchemaRecordingReservation.T_RESERVATIONRow = DirectCast(DirectCast(dgrReservation.DataBoundItem, DataRowView).Row, SchemaRecordingReservation.T_RESERVATIONRow)
                If (2L).Equals(reservationRow.RECORDED) Then
                    System.Windows.Forms.MessageBox.Show(reservationRow.PROGRAM_TITLE & "�͘^�撆�ł��B�폜�ł��܂���B", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
                    Return
                End If
                deleteReservationTable.ImportRow(reservationRow)
            Next
            If System.Windows.Forms.MessageBox.Show("�폜���܂����H", MESSAGEBOX_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, 0, False) = System.Windows.Forms.DialogResult.OK Then
                Me.MainForm.DeleteRecordingReservation(deleteReservationTable)
                Me.SetDataGrid()
            End If
        End If
        Me.Activate()
    End Sub
    ''' <summary>
    ''' �^��t�@�C���Đ�
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub PlayRecordingFile()
        Dim savePath As String = DirectCast(DirectCast(Me.DgvReservation.CurrentRow.DataBoundItem, DataRowView).Row, SchemaRecordingReservation.T_RESERVATIONRow).PATH
        Dim Process As System.Diagnostics.Process = System.Diagnostics.Process.Start(savePath)
        Process.WaitForExit()
    End Sub
#End Region

End Class