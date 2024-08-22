Imports System.Configuration
Imports System.Net
Imports System.ServiceModel
Imports RK.SchemaRecordingReservation

''' <summary>
''' �I�v�V�����ݒ�t�H�[��
''' </summary>
''' <remarks></remarks>
Public Class formSetting

#Region "�萔�E�ϐ�"
    ''' <summary>�ۑ��p�X�R���t�B�O���[�V����</summary>
    Protected SaveFileConfig As SavePathConfiguration
    ''' <summary>�\����</summary>
    Protected SettingRow As SchemaRecordingReservation.M_SETTINGRow
    ''' <summary>�n��g�f�o�C�X���</summary>
    Protected TerrestrialDevice As SchemaRecordingReservation.M_DEVICEDataTable
    ''' <summary>�a�r�P�P�O�b�r�f�o�C�X���</summary>
    Protected BS110CSDevice As SchemaRecordingReservation.M_DEVICEDataTable
    ''' <summary>DLL���</summary>
    Protected dllNames As String()
#End Region

#Region "�R���X�g���N�^"
    Public Sub New()
        ' ���̌Ăяo���́AWindows �t�H�[�� �f�U�C�i�ŕK�v�ł��B
        InitializeComponent()
        ' InitializeComponent() �Ăяo���̌�ŏ�������ǉ����܂��B
        Dim settingTable As New M_SETTINGDataTable
        Me.TerrestrialDevice = New M_DEVICEDataTable
        Me.BS110CSDevice = New M_DEVICEDataTable
        Using RecordingReservationManagerProxy As New ChannelProxy(Of IRecordingReservationManager)(ConstClass.RECORING_RESERVATION_MANAGER)
            RecordingReservationManagerProxy.Channel.GetSettings(settingTable, Me.TerrestrialDevice, Me.BS110CSDevice)
            Me.SettingRow = settingTable(0)
            dllNames = RecordingReservationManagerProxy.Channel.GetDllNames(Me.SettingRow.TVTEST_FULL_PATH)
        End Using
        Me.SaveFileConfig = New SavePathConfiguration()
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
        If System.Environment.GetCommandLineArgs.Length < 2 Then
            MessageBox.Show("����������܂���B" & System.Environment.GetCommandLineArgs(0))

            System.Environment.Exit(1)
            Return
        End If
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
    ''' �g���q�֘A�t���ݒ�
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnSetExtension_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSetExtension.Click
        Me.SetExtention()
    End Sub
    ''' <summary>
    ''' �g���q�֘A�t������
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnClearExtention_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearExtention.Click
        Me.ClearExtention()
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnShowFolderBrowser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowFolderBrowser.Click
        Me.SetSavePath()
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
            Me.txtSavePath.Text = SaveFileConfig.SavePath
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
        For Each fileName As String In Me.dllNames
            Dim dllName As String = System.IO.Path.GetFileName(fileName)
            If Me.TerrestrialDevice.Where(Function(deviceRow) deviceRow.DLL_NAME = dllName).Count = 0 _
            AndAlso Me.BS110CSDevice.Where(Function(deviceRow) deviceRow.DLL_NAME = dllName).Count = 0 Then
                DllList.Add(dllName, dllName)
            End If
        Next
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
        End With
    End Sub
    ''' <summary>
    ''' �n��g�f�o�C�X�ǉ�
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub AddTerrestrialDevice()
        If Not CheckDevice(Me.cboTerrestrialDeviceDllName.SelectedValue.ToString()) Then
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
        If Not CheckDevice(Me.cboBS110CSDeviceDllName.SelectedValue.ToString()) Then
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
    ''' �ݒ���X�V
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function UpdateSetting() As Boolean
        Me.GetSettingData()
        If Not CheckSetting() Then
            Return False
        End If
        Using RecordingReservationManagerProxy As New ChannelProxy(Of IRecordingReservationManager)(ConstClass.RECORING_RESERVATION_MANAGER)
            RecordingReservationManagerProxy.Channel.UpdateSetting(New M_SETTINGRowSerializer(Me.SettingRow), Me.TerrestrialDevice, Me.BS110CSDevice)
            Me.SaveFileConfig.SavePath = Me.txtSavePath.Text
        End Using
        Return True
    End Function
    ''' <summary>
    ''' �ݒ���`�F�b�N
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function CheckSetting() As Boolean
        '���ۑ��p�X�`�F�b�N
        If Not Me.CheckInputSavePath() Then
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
        If String.IsNullOrEmpty(Me.txtSavePath.Text) Then
            MessageBox.Show("�ۑ���t�H���_���w�肳��Ă��܂���B", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
            Return False
        End If
        Return True
    End Function
    ''' <summary>
    ''' �f�o�C�X�`�F�b�N
    ''' </summary>
    ''' <param name="dllName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function CheckDevice(ByVal dllName As String) As Boolean
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
    ''' �g���q�֘A�t���ݒ�
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetExtention()
        Try
            Me.SetExtentionInRegistry()
        Catch ex As System.UnauthorizedAccessException
            MessageBox.Show(ex.Message & System.Environment.NewLine & "�Ǘ��Ҍ����Ŏ��s���Ȃ��Ƃ��̋@�\�͎g���܂���B", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, 0, False)
            Return
        End Try
        MessageBox.Show("�g���q�̊֘A�t���ݒ���s���܂����B", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, 0, False)
        Me.Activate()
    End Sub
    ''' <summary>
    ''' �g���q�֘A�t������
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearExtention()
        Try
            Me.ClearExtentionInRegistry()
        Catch ex As System.UnauthorizedAccessException
            MessageBox.Show(ex.Message & System.Environment.NewLine & "�Ǘ��Ҍ����Ŏ��s���Ȃ��Ƃ��̋@�\�͎g���܂���B", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, 0, False)
            Return
        End Try
        MessageBox.Show("�g���q�̊֘A�t���������s���܂����B", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, 0, False)
        Me.Activate()
    End Sub

#Region "�g���q�֘A�t��"
    ''' <summary>
    ''' �g���q�֘A�t��
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetExtentionInRegistry()
        '�t�@�C���^�C�v��o�^
        For Each extension As String In New String() {"tvpi", "tvpid", "epg"}
            Me.DeleteFileExtsSubkey(extension)
            Dim regkey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("." & extension)
            regkey.SetValue("", extension & "_file")
            regkey.Close()
            '�t�@�C���^�C�v�Ƃ��̐�����o�^
            Dim shellkey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(extension & "_file")
            shellkey.SetValue("", "iEPG�^��\��")

            '�����Ƃ��̐�����o�^
            Dim openKey As Microsoft.Win32.RegistryKey = shellkey.CreateSubKey("shell\" + "open")
            openKey.SetValue("", "RecordingReservationManager�ŊJ��(&O)")

            '�R�}���h���C����o�^
            '���s����R�}���h���C��
            Dim commandline As String = System.Environment.GetCommandLineArgs(1) + " ""%1"""
            Dim commandKey As Microsoft.Win32.RegistryKey = openKey.CreateSubKey("command")
            commandKey.SetValue("", commandline)
            commandKey.Close()
            openKey.Close()
            shellkey.Close()
        Next
        '�t�@�C���^�C�v��
        Dim fileType As String = System.IO.Path.GetFileNameWithoutExtension(System.Environment.GetCommandLineArgs(1))
        Dim iconPath As String = System.Environment.GetCommandLineArgs(1)
        Dim iconkey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(fileType + "\DefaultIcon")
        iconkey.SetValue("", iconPath & "," & "0")
        iconkey.Close()
    End Sub
#End Region

#Region "�g���q�֘A�t���폜"
    ''' <summary>
    ''' �g���q�֘A�t���폜
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearExtentionInRegistry()
        '���W�X�g���L�[���폜
        '�t�@�C���^�C�v���폜
        For Each extension As String In New String() {"tvpi", "tvpid", "epg"}
            Me.DeleteFileExtsSubkey(extension)
            Me.DeleteSubKey(Microsoft.Win32.Registry.ClassesRoot, "." & extension)
            Me.DeleteSubKey(Microsoft.Win32.Registry.ClassesRoot, extension & "_file")
        Next
    End Sub
#End Region

#Region "�g���q�֘A�t�����W�X�g���폜"
    ''' <summary>
    ''' �g���q�֘A�t�����W�X�g���폜
    ''' </summary>
    ''' <param name="extension"></param>
    ''' <remarks></remarks>
    Protected Sub DeleteFileExtsSubkey(ByVal extension As String)
        Me.DeleteSubKey(Microsoft.Win32.Registry.CurrentUser, "Software\Microsoft\Windows\CurrentVersion\Explorer\ComDlg32\OpenSavePidlMRU\" & extension)
        Me.DeleteSubKey(Microsoft.Win32.Registry.CurrentUser, "Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\." & extension)
    End Sub
#End Region

#Region "���W�X�g���T�u�L�[�폜"
    ''' <summary>
    ''' ���W�X�g���T�u�L�[�폜
    ''' </summary>
    ''' <param name="registryKey"></param>
    ''' <param name="subKeyName"></param>
    ''' <remarks></remarks>
    Protected Sub DeleteSubKey(ByVal registryKey As Microsoft.Win32.RegistryKey, ByVal subKeyName As String)
        Try
            Dim subRegistryKey As Microsoft.Win32.RegistryKey = registryKey.OpenSubKey(subKeyName)
            If subRegistryKey IsNot Nothing Then
                subRegistryKey.Close()
                registryKey.DeleteSubKeyTree(subKeyName)
            End If
        Catch ex As System.ArgumentException
            Me.DeleteSubKeyTree(registryKey, subKeyName)
        End Try
    End Sub
#End Region

#Region "���W�X�g���T�u�L�[�폜�i�ċA�ďo�j"
    ''' <summary>
    ''' ���W�X�g���T�u�L�[�폜�i�ċA�ďo�j
    ''' </summary>
    ''' <param name="registryKey"></param>
    ''' <param name="subKeyName"></param>
    ''' <remarks></remarks>
    Protected Sub DeleteSubKeyTree(ByVal registryKey As Microsoft.Win32.RegistryKey, ByVal subKeyName As String)
        For Each keyName As String In registryKey.OpenSubKey(subKeyName).GetSubKeyNames
            If registryKey.OpenSubKey(subKeyName & "\" & keyName).SubKeyCount > 0 Then
                Me.DeleteSubKeyTree(registryKey, subKeyName & "\" & keyName)
            Else
                registryKey.DeleteSubKey(subKeyName & "\" & keyName)
            End If
        Next
    End Sub
#End Region

#Region " �A�N�e�B�u�R���g���[���ɐݒ� "
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

#End Region

End Class
