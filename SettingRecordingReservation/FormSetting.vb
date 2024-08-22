Imports RK.SchemaRecordingReservation

''' <summary>
''' �I�v�V�����ݒ�t�H�[��
''' </summary>
''' <remarks></remarks>
Public Class formSetting

#Region "�萔�E�ϐ�"
    ''' <summary>�G���e�B�e�B</summary>
    Protected Entity As EntityRecordingReservation
    ''' <summary>�\����</summary>
    Protected SettingRow As SchemaRecordingReservation.M_SETTINGRow
    ''' <summary>�n��g�f�o�C�X���</summary>
    Protected TerrestrialDevice As SchemaRecordingReservation.M_DEVICEDataTable
    ''' <summary>�a�r�P�P�O�b�r�f�o�C�X���</summary>
    Protected BS110CSDevice As SchemaRecordingReservation.M_DEVICEDataTable
#End Region

#Region "�R���X�g���N�^"
    Public Sub New()
        ' ���̌Ăяo���́AWindows �t�H�[�� �f�U�C�i�ŕK�v�ł��B
        InitializeComponent()
        ' InitializeComponent() �Ăяo���̌�ŏ�������ǉ����܂��B
        Me.Entity = New EntityRecordingReservation
        With Entity
            Me.SettingRow = .GetSetting()(0)
            Me.TerrestrialDevice = .GetDevice(BROADCAST_TYPE.TERRESTRIAL)
            Me.BS110CSDevice = .GetDevice(BROADCAST_TYPE.BS110CS)
        End With
    End Sub
#End Region

#Region "�C�x���g"

#Region "�t�H�[��"
    ''' <summary>
    ''' �t�H�[�����[�h
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        MyBase.OnLoad(e)
        '���p���[�I���C���^�[�o��
        Dim powerOnIntervalMinutesList As New Dictionary(Of String, Object)
        For minutes As System.Int64 = 1L To 999L
            powerOnIntervalMinutesList.Add(minutes.ToString.PadLeft(3), minutes)
        Next
        SetComboList(Me.cboPowerOnInterval, powerOnIntervalMinutesList)
        '���d�w�d�N���C���^�[�o��
        Dim exeBootIntervalMinutesList As New Dictionary(Of String, Object)
        For minutes As System.Int64 = 1L To 999L
            exeBootIntervalMinutesList.Add(minutes.ToString.PadLeft(3), minutes)
        Next
        SetComboList(Me.cboExeBootInterval, exeBootIntervalMinutesList)
        '���X�^�[�g�C���^�[�o��
        Dim secondsList As New Dictionary(Of String, Object)
        For seconds As System.Int64 = 1 To 60L
            secondsList.Add(seconds.ToString.PadLeft(3), seconds)
        Next
        SetComboList(Me.cboStartInterval, secondsList)
        '���p���[�I���C���^�[�o��
        Dim checkStartIntervalMinutesList As New Dictionary(Of String, Object)
        For minutes As System.Int64 = 1L To 10L
            checkStartIntervalMinutesList.Add(minutes.ToString.PadLeft(3), minutes)
        Next
        SetComboList(Me.cboCheckStartInterval, checkStartIntervalMinutesList)
        Me.SetSettingData()
    End Sub
#End Region

#Region "�{�^��"
    ''' <summary>
    ''' �n�j�{�^����������
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub BtnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        If Not Me.UpdateSetting() Then
            Return
        End If
        System.Environment.Exit(0)
    End Sub
    ''' <summary>
    ''' �L�����Z���{�^����������
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        System.Environment.Exit(1)
    End Sub
    ''' <summary>
    ''' �t�H���_�_�C�A���O�\��
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnShowFolderBrowser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowFolderBrowser.Click
        Me.SetSavePath()
    End Sub
    ''' <summary>
    ''' TVTest�t�@�C���_�C�A���O�\��
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnShowTVTestFileDialog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowTVTestFileDialog.Click
        Me.SetExePath()
    End Sub
    ''' <summary>
    ''' �n��g�f�o�C�X�ǉ�
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddTerrestrialDevice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTerrestrialDevice.Click
        Me.AddTerrestrialDevice()
    End Sub
    ''' <summary>
    ''' �a�r�P�P�O�b�r�f�o�C�X�ǉ�
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddBS110CSDevice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddBS110CSDevice.Click
        Me.AddBS110CSDevice()
    End Sub
    ''' <summary>
    ''' �n��g�f�o�C�X�폜
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDeleteTerrestrialDevice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteTerrestrialDevice.Click
        Me.DeleteTerrestrialDevice()
    End Sub
    ''' <summary>
    ''' �a�r�P�P�O�b�r�f�o�C�X�폜
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDeleteBS110CSDevice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteBS110CSDevice.Click
        Me.DeleteBS110CSDevice()
    End Sub
    ''' <summary>
    ''' �n��g�f�o�C�X�A�b�v
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnTerrestrialUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTerrestrialUp.Click
        Me.UpDevicePriority(Me.dgvTerrestrialDevice)
    End Sub
    ''' <summary>
    ''' �n��g�f�o�C�X�_�E��
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnTerrestrialDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTerrestrialDown.Click
        Me.DownDevicePriority(Me.dgvTerrestrialDevice)
    End Sub
    ''' <summary>
    ''' �a�r�P�P�O�b�r�f�o�C�X�A�b�v
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnBS110CSUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBS110CSUp.Click
        Me.UpDevicePriority(Me.dgvBS110CSDevice)
    End Sub
    ''' <summary>
    ''' �a�r�P�P�O�b�r�f�o�C�X�_�E��
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnBS110CSDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBS110CSDown.Click
        Me.DownDevicePriority(Me.dgvBS110CSDevice)
    End Sub
#End Region

#End Region

#Region "�������\�b�h"
    ''' <summary>
    ''' �R���{�Z�b�e�B���O
    ''' </summary>
    ''' <param name="combo"></param>
    ''' <param name="listItem"></param>
    ''' <remarks></remarks>
    Protected Sub SetComboList(ByVal combo As ComboBox, ByVal listItem As Dictionary(Of String, Object))
        combo.DisplayMember = "Key"
        combo.ValueMember = "Value"
        combo.DataSource = listItem.ToList()
    End Sub
    ''' <summary>
    ''' ��ʂɐݒ�
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetSettingData()
        With Me.SettingRow
            Me.cboPowerOnInterval.SelectedValue = .POWER_ON_INTERVAL_SECONDS
            Me.cboExeBootInterval.SelectedValue = .EXE_BOOT_INTERVAL_SECONDS
            Me.cboStartInterval.SelectedValue = .START_INTERVAL_SECONDS
            Me.cboCheckStartInterval.SelectedValue = .CHECK_START_INTERVAL_MINUTES
            Me.chkSuspendAfterRecording.Checked = (1L).Equals(.SUSPEND_AFTER_RECORDING)
            Me.chkOutputErrorLog.Checked = (1L).Equals(.OUTPUT_ERRORLOG)
            Me.chkExitApplicationAfterRecording.Checked = (1L).Equals(.EXIT_APPLICATION_AFTER_RECORDING)
            Me.chkPreview.Checked = (1L).Equals(.PREVIEW)
            Me.txtTVTestFileFullPath.Text = .TVTEST_FULL_PATH
            Me.txtSavePath.Text = .SAVE_PATH
        End With
        Me.SetTerrestrialDevice()
        Me.SetBS110CSDevice()
    End Sub
    ''' <summary>
    ''' DLL���X�g�̍쐬
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetDllComboList()
        Dim DllList As New Dictionary(Of String, Object)
        If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(Me.txtTVTestFileFullPath.Text.Trim)) Then
            For Each fileName As String In System.IO.Directory.GetFiles( _
            System.IO.Path.GetDirectoryName(Me.txtTVTestFileFullPath.Text.Trim), "BonDriver*.dll")
                Dim dllName As String = System.IO.Path.GetFileName(fileName)
                If Me.TerrestrialDevice.Where(Function(deviceRow) deviceRow.DLL_NAME = dllName).Count = 0 _
                AndAlso Me.BS110CSDevice.Where(Function(deviceRow) deviceRow.DLL_NAME = dllName).Count = 0 Then
                    DllList.Add(dllName, dllName)
                End If
            Next
        End If
        SetComboList(Me.cboTerrestrialDeviceDllName, DllList)
        SetComboList(Me.cboBS110CSDeviceDllName, DllList)
    End Sub
    ''' <summary>
    ''' ��ʃf�[�^���擾
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub GetSettingData()
        With Me.SettingRow
            .POWER_ON_INTERVAL_SECONDS = Convert.ToInt64(Me.cboPowerOnInterval.SelectedValue)
            .EXE_BOOT_INTERVAL_SECONDS = Convert.ToInt64(Me.cboExeBootInterval.SelectedValue)
            .START_INTERVAL_SECONDS = Convert.ToInt64(Me.cboStartInterval.SelectedValue)
            .CHECK_START_INTERVAL_MINUTES = Convert.ToInt64(Me.cboCheckStartInterval.SelectedValue)
            If Me.chkSuspendAfterRecording.Checked Then .SUSPEND_AFTER_RECORDING = 1L Else .SUSPEND_AFTER_RECORDING = 0L
            If Me.chkOutputErrorLog.Checked Then .OUTPUT_ERRORLOG = 1L Else .OUTPUT_ERRORLOG = 0L
            If Me.chkExitApplicationAfterRecording.Checked Then .EXIT_APPLICATION_AFTER_RECORDING = 1L Else .EXIT_APPLICATION_AFTER_RECORDING = 0L
            If Me.chkPreview.Checked Then .PREVIEW = 1L Else .PREVIEW = 0L
            .TVTEST_FULL_PATH = txtTVTestFileFullPath.Text
            .SAVE_PATH = Me.txtSavePath.Text
        End With
    End Sub
    ''' <summary>
    ''' �n��g�f�o�C�X�ǉ�
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub AddTerrestrialDevice()
        If Not CheckDevice(Me.cboTerrestrialDeviceDllName.SelectedValue) Then
            Me.SelectControl(Me.cboTerrestrialDeviceDllName)
            Return
        End If
        Me.TerrestrialDevice.AddM_DEVICERow(Me.cboTerrestrialDeviceDllName.SelectedValue.ToString(), Me.dgvTerrestrialDevice.RowCount)
        Me.SetTerrestrialDevice()
    End Sub
    ''' <summary>
    ''' �a�r�P�P�O�b�r�f�o�C�X�ǉ�
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub AddBS110CSDevice()
        If Not CheckDevice(Me.cboBS110CSDeviceDllName.SelectedValue) Then
            Me.SelectControl(Me.cboBS110CSDeviceDllName)
            Return
        End If
        Me.BS110CSDevice.AddM_DEVICERow(Me.cboBS110CSDeviceDllName.SelectedValue.ToString(), Me.dgvBS110CSDevice.RowCount)
        Me.SetBS110CSDevice()
    End Sub
    ''' <summary>
    ''' �n��g�f�o�C�X�폜
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub DeleteTerrestrialDevice()
        Dim terrestrialDeviceRow As SchemaRecordingReservation.M_DEVICERow = Me.GetCurrentTerrestrialRow()
        If terrestrialDeviceRow IsNot Nothing Then
            Me.TerrestrialDevice.RemoveM_DEVICERow(terrestrialDeviceRow)
            Me.SetTerrestrialDevice()
        End If
    End Sub
    ''' <summary>
    ''' �a�r�P�P�O�b�r�f�o�C�X�폜
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub DeleteBS110CSDevice()
        Dim BS110CSDeviceRow As SchemaRecordingReservation.M_DEVICERow = Me.GetCurrentBS110CSRow()
        If BS110CSDeviceRow IsNot Nothing Then
            Me.BS110CSDevice.RemoveM_DEVICERow(BS110CSDeviceRow)
            Me.SetBS110CSDevice()
        End If
    End Sub
    ''' <summary>
    ''' �n��g�f�o�C�X�O���b�h�ݒ�
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetTerrestrialDevice()
        With Me.dgvTerrestrialDevice
            .DataSource = Me.TerrestrialDevice
            .Columns(1).Visible = False
            .Sort(.Columns(1), System.ComponentModel.ListSortDirection.Ascending)
        End With
        Me.SetDllComboList()
    End Sub
    ''' <summary>
    ''' �a�r�P�P�O�b�r�O���b�h�ݒ�
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetBS110CSDevice()
        With Me.dgvBS110CSDevice
            .DataSource = Me.BS110CSDevice
            .Columns(1).Visible = False
            .Sort(.Columns(1), System.ComponentModel.ListSortDirection.Ascending)
        End With
        Me.SetDllComboList()
    End Sub
    ''' <summary>
    ''' �J�����g�s�Ƀo�C���h����Ă���f�[�^�s���擾
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function GetCurrentTerrestrialRow() As SchemaRecordingReservation.M_DEVICERow
        If Me.dgvTerrestrialDevice.CurrentRow IsNot Nothing Then
            Return DirectCast(DirectCast(Me.dgvTerrestrialDevice.CurrentRow.DataBoundItem, DataRowView).Row, SchemaRecordingReservation.M_DEVICERow)
        Else
            Return Nothing
        End If
    End Function
    ''' <summary>
    ''' �J�����g�s�Ƀo�C���h����Ă���f�[�^�s���擾
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function GetCurrentBS110CSRow() As SchemaRecordingReservation.M_DEVICERow
        If Me.dgvBS110CSDevice.CurrentRow IsNot Nothing Then
            Return DirectCast(DirectCast(Me.dgvBS110CSDevice.CurrentRow.DataBoundItem, DataRowView).Row, SchemaRecordingReservation.M_DEVICERow)
        Else
            Return Nothing
        End If
    End Function
    ''' <summary>
    ''' �v���C�I���e�B�A�b�v
    ''' </summary>
    ''' <param name="dgvDevice"></param>
    ''' <remarks></remarks>
    Protected Sub UpDevicePriority(ByVal dgvDevice As DataGridView)
        If dgvDevice.SelectedRows(0).Index > 0 Then
            DirectCast(DirectCast(dgvDevice.Rows(dgvDevice.SelectedRows(0).Index - 1).DataBoundItem,  _
            DataRowView).Row, SchemaRecordingReservation.M_DEVICERow).PRIORITY += 1
            DirectCast(DirectCast(dgvDevice.SelectedRows(0).DataBoundItem, DataRowView).Row,  _
            SchemaRecordingReservation.M_DEVICERow).PRIORITY -= 1
            Me.SetTerrestrialDevice()
        End If
    End Sub

    Protected Sub DownDevicePriority(ByVal dgvDevice As DataGridView)
        If dgvDevice.SelectedRows(0).Index < dgvDevice.RowCount - 1 Then
            DirectCast(DirectCast(dgvDevice.Rows(dgvDevice.SelectedRows(0).Index + 1).DataBoundItem,  _
            DataRowView).Row, SchemaRecordingReservation.M_DEVICERow).PRIORITY -= 1
            DirectCast(DirectCast(dgvDevice.SelectedRows(0).DataBoundItem, DataRowView).Row,  _
            SchemaRecordingReservation.M_DEVICERow).PRIORITY += 1
            Me.SetTerrestrialDevice()
        End If
    End Sub
    ''' <summary>
    ''' �^��t�@�C���ۑ��p�X�ݒ�
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetSavePath()
        Using folderDialog As New System.Windows.Forms.FolderBrowserDialog
            With folderDialog
                .Description = "�^�悷��t�@�C���̕ۑ�����w�肵�Ă��������B"
                .SelectedPath = Me.txtSavePath.Text
                If System.Windows.Forms.DialogResult.OK.Equals(.ShowDialog()) Then
                    Me.txtSavePath.Text = .SelectedPath
                End If
            End With
        End Using
    End Sub
    ''' <summary>
    ''' ���s�t�@�C���p�X�ݒ�
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetExePath()
        Using fileDialog As New System.Windows.Forms.OpenFileDialog
            With fileDialog
                .AddExtension = True
                .DefaultExt = ".exe"
                .SupportMultiDottedExtensions = True
                .CheckFileExists = True
                .DereferenceLinks = True
                .InitialDirectory = Me.txtTVTestFileFullPath.Text
                .Multiselect = False
                .ShowHelp = False
                .Filter = "TVTest|TVTest.exe"
                .FileName = Me.txtTVTestFileFullPath.Text
                If System.Windows.Forms.DialogResult.OK.Equals(.ShowDialog) Then
                    Me.txtTVTestFileFullPath.Text = .FileName
                    Me.SetDllComboList()
                End If
            End With
        End Using
    End Sub
    ''' <summary>
    ''' �ݒ���X�V
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function UpdateSetting() As Boolean
        Me.GetSettingData()
        If Not CheckSetting() Then
            Return False
        End If
        With Entity
            .BeginTran()
            Try
                .DeleteSetting()
                .InsertSetting(SettingRow)
                .DeleteDevice()
                For Each terrestrialDeviceRow As SchemaRecordingReservation.M_DEVICERow In Me.TerrestrialDevice
                    .InsertDevice(BROADCAST_TYPE.TERRESTRIAL, terrestrialDeviceRow.DLL_NAME, terrestrialDeviceRow.PRIORITY)
                Next
                For Each bs110csDeviceRow As SchemaRecordingReservation.M_DEVICERow In Me.BS110CSDevice
                    .InsertDevice(BROADCAST_TYPE.BS110CS, bs110csDeviceRow.DLL_NAME, bs110csDeviceRow.PRIORITY)
                Next
            Catch ex As System.UnauthorizedAccessException
                MessageBox.Show(ex.Message & System.Environment.NewLine & "�Ǘ��Ҍ����Ŏ��s���Ȃ��Ƃ��̋@�\�͎g���܂���B", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, 0, False)
            Finally
                .Commit()
            End Try
        End With
        Return True
    End Function
    ''' <summary>
    ''' �ݒ���`�F�b�N
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function CheckSetting() As Boolean
        '��TVTest�p�X�`�F�b�N
        If Not Me.CheckTVTestPath() Then
            Return False
        End If
        '���ۑ��p�X�`�F�b�N
        If Not Me.CheckInputSavePath() Then
            Return False
        End If
        Return True
    End Function
    ''' <summary>
    ''' �^����s�t�@�C���p�X���̓`�F�b�N
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function CheckTVTestPath() As Boolean
        If String.IsNullOrEmpty(Me.SettingRow.TVTEST_FULL_PATH) Then
            MessageBox.Show("TVTest�̃p�X���w�肳��Ă��܂���B", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
            Return False
        End If
        Return True
    End Function
    ''' <summary>
    ''' �ۑ���t�H���_���̓`�F�b�N
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function CheckInputSavePath() As Boolean
        If String.IsNullOrEmpty(Me.SettingRow.SAVE_PATH) Then
            MessageBox.Show("�ۑ���t�H���_���w�肳��Ă��܂���B", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
            Return False
        End If
        Return True
    End Function
    ''' <summary>
    ''' �f�o�C�X�`�F�b�N
    ''' </summary>
    ''' <param name="dllNamObject"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function CheckDevice(ByVal dllNamObject As Object) As Boolean
        If dllNamObject Is Nothing Then
            Return False
        End If
        Dim dllName As String = DirectCast(dllNamObject, String)
        If String.IsNullOrEmpty(dllName) Then
            MessageBox.Show("�f�o�C�X����͂��Ă��������B", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
            Return False
        End If
        If Not Me.TerrestrialDevice.All(Function(terrestrialDeviceRow As SchemaRecordingReservation.M_DEVICERow) Not terrestrialDeviceRow.DLL_NAME = dllName) Then
            MessageBox.Show(dllName & " �͒n��f�W�^���f�o�C�X�Ɋ��ɓo�^����Ă��܂��B", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
            Return False
        End If
        If Not Me.BS110CSDevice.All(Function(bs110csDeviceRow As SchemaRecordingReservation.M_DEVICERow) Not bs110csDeviceRow.DLL_NAME = dllName) Then
            MessageBox.Show(dllName & " �͂a�r�P�P�O�b�r�f�o�C�X�Ɋ��ɓo�^����Ă��܂��B", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
            Return False
        End If
        Return True
    End Function
    ''' <summary>
    ''' �A�N�e�B�u�R���g���[���ɐݒ�
    ''' </summary>
    ''' <param name="control"></param>
    ''' <remarks></remarks>
    Protected Sub SelectControl(ByVal control As Control)
        control.Select()
        If TypeOf control Is TextBox Then DirectCast(control, TextBox).SelectAll()
    End Sub
#End Region

End Class
