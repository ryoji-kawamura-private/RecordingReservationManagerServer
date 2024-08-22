Imports RK.SchemaRecordingReservation

''' <summary>
''' ���O�Q��
''' </summary>
''' <remarks></remarks>
Public Class FormLog

#Region "�萔�E�ϐ�"
    ''' <summary>�\����ꗗ</summary>
    Protected ReservationRow As SchemaRecordingReservation.T_RESERVATIONRow
    ''' <summary>���O�ꗗ</summary>
    Protected LogList As SchemaRecordingReservation.T_LOGDataTable
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
    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub
    ''' <summary>
    ''' �폜�{�^��
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnDelete.Click
        Me.DeleteLog()
        Me.SetDataGrid()
    End Sub
#End Region

#Region "�f�[�^�O���b�h"
    ''' <summary>
    ''' �f�[�^�o�C���h
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

#Region "�O�����\�b�h"
    ''' <summary>
    ''' �_�C�A���O�Ƃ��ĕ\��
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

#Region "�������\�b�h"
    ''' <summary>
    ''' �\����폜
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub DeleteLog()
        If System.Windows.Forms.DialogResult.OK.Equals(System.Windows.Forms.MessageBox.Show("�폜���܂����H", MESSAGEBOX_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, 0, False)) Then
            Me.MainForm.DeleteLog(Me.ReservationRow.ID)
        End If
    End Sub
    ''' <summary>
    ''' �f�[�^�O���b�h�Ƀf�[�^��ݒ�
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetDataGrid()
        Me.LogList = Me.MainForm.GetLog(Me.ReservationRow)
        Me.dgvReservation.DataSource = Me.LogList
    End Sub
#End Region

End Class