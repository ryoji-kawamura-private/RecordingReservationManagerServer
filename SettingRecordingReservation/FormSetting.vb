Imports RK.SchemaRecordingReservation

''' <summary>
''' オプション設定フォーム
''' </summary>
''' <remarks></remarks>
Public Class formSetting

#Region "定数・変数"
    ''' <summary>エンティティ</summary>
    Protected Entity As EntityRecordingReservation
    ''' <summary>予約情報</summary>
    Protected SettingRow As SchemaRecordingReservation.M_SETTINGRow
    ''' <summary>地上波デバイス情報</summary>
    Protected TerrestrialDevice As SchemaRecordingReservation.M_DEVICEDataTable
    ''' <summary>ＢＳ１１０ＣＳデバイス情報</summary>
    Protected BS110CSDevice As SchemaRecordingReservation.M_DEVICEDataTable
#End Region

#Region "コンストラクタ"
    Public Sub New()
        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()
        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        Me.Entity = New EntityRecordingReservation
        With Entity
            Me.SettingRow = .GetSetting()(0)
            Me.TerrestrialDevice = .GetDevice(BROADCAST_TYPE.TERRESTRIAL)
            Me.BS110CSDevice = .GetDevice(BROADCAST_TYPE.BS110CS)
        End With
    End Sub
#End Region

#Region "イベント"

#Region "フォーム"
    ''' <summary>
    ''' フォームロード
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        MyBase.OnLoad(e)
        '★パワーオンインターバル
        Dim powerOnIntervalMinutesList As New Dictionary(Of String, Object)
        For minutes As System.Int64 = 1L To 999L
            powerOnIntervalMinutesList.Add(minutes.ToString.PadLeft(3), minutes)
        Next
        SetComboList(Me.cboPowerOnInterval, powerOnIntervalMinutesList)
        '★ＥＸＥ起動インターバル
        Dim exeBootIntervalMinutesList As New Dictionary(Of String, Object)
        For minutes As System.Int64 = 1L To 999L
            exeBootIntervalMinutesList.Add(minutes.ToString.PadLeft(3), minutes)
        Next
        SetComboList(Me.cboExeBootInterval, exeBootIntervalMinutesList)
        '★スタートインターバル
        Dim secondsList As New Dictionary(Of String, Object)
        For seconds As System.Int64 = 1 To 60L
            secondsList.Add(seconds.ToString.PadLeft(3), seconds)
        Next
        SetComboList(Me.cboStartInterval, secondsList)
        '★パワーオンインターバル
        Dim checkStartIntervalMinutesList As New Dictionary(Of String, Object)
        For minutes As System.Int64 = 1L To 10L
            checkStartIntervalMinutesList.Add(minutes.ToString.PadLeft(3), minutes)
        Next
        SetComboList(Me.cboCheckStartInterval, checkStartIntervalMinutesList)
        Me.SetSettingData()
    End Sub
#End Region

#Region "ボタン"
    ''' <summary>
    ''' ＯＫボタン押下処理
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
    ''' キャンセルボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        System.Environment.Exit(1)
    End Sub
    ''' <summary>
    ''' フォルダダイアログ表示
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub BtnShowFolderBrowser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowFolderBrowser.Click
        Me.SetSavePath()
    End Sub
    ''' <summary>
    ''' TVTestファイルダイアログ表示
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnShowTVTestFileDialog_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnShowTVTestFileDialog.Click
        Me.SetExePath()
    End Sub
    ''' <summary>
    ''' 地上波デバイス追加
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddTerrestrialDevice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddTerrestrialDevice.Click
        Me.AddTerrestrialDevice()
    End Sub
    ''' <summary>
    ''' ＢＳ１１０ＣＳデバイス追加
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnAddBS110CSDevice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddBS110CSDevice.Click
        Me.AddBS110CSDevice()
    End Sub
    ''' <summary>
    ''' 地上波デバイス削除
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDeleteTerrestrialDevice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteTerrestrialDevice.Click
        Me.DeleteTerrestrialDevice()
    End Sub
    ''' <summary>
    ''' ＢＳ１１０ＣＳデバイス削除
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDeleteBS110CSDevice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDeleteBS110CSDevice.Click
        Me.DeleteBS110CSDevice()
    End Sub
    ''' <summary>
    ''' 地上波デバイスアップ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnTerrestrialUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTerrestrialUp.Click
        Me.UpDevicePriority(Me.dgvTerrestrialDevice)
    End Sub
    ''' <summary>
    ''' 地上波デバイスダウン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnTerrestrialDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTerrestrialDown.Click
        Me.DownDevicePriority(Me.dgvTerrestrialDevice)
    End Sub
    ''' <summary>
    ''' ＢＳ１１０ＣＳデバイスアップ
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnBS110CSUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBS110CSUp.Click
        Me.UpDevicePriority(Me.dgvBS110CSDevice)
    End Sub
    ''' <summary>
    ''' ＢＳ１１０ＣＳデバイスダウン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnBS110CSDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBS110CSDown.Click
        Me.DownDevicePriority(Me.dgvBS110CSDevice)
    End Sub
#End Region

#End Region

#Region "内部メソッド"
    ''' <summary>
    ''' コンボセッティング
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
    ''' 画面に設定
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
    ''' DLLリストの作成
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
    ''' 画面データを取得
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
    ''' 地上波デバイス追加
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
    ''' ＢＳ１１０ＣＳデバイス追加
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
    ''' 地上波デバイス削除
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
    ''' ＢＳ１１０ＣＳデバイス削除
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
    ''' 地上波デバイスグリッド設定
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
    ''' ＢＳ１１０ＣＳグリッド設定
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
    ''' カレント行にバインドされているデータ行を取得
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
    ''' カレント行にバインドされているデータ行を取得
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
    ''' プライオリティアップ
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
    ''' 録画ファイル保存パス設定
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetSavePath()
        Using folderDialog As New System.Windows.Forms.FolderBrowserDialog
            With folderDialog
                .Description = "録画するファイルの保存先を指定してください。"
                .SelectedPath = Me.txtSavePath.Text
                If System.Windows.Forms.DialogResult.OK.Equals(.ShowDialog()) Then
                    Me.txtSavePath.Text = .SelectedPath
                End If
            End With
        End Using
    End Sub
    ''' <summary>
    ''' 実行ファイルパス設定
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
    ''' 設定情報更新
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
                MessageBox.Show(ex.Message & System.Environment.NewLine & "管理者権限で実行しないとこの機能は使えません。", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, 0, False)
            Finally
                .Commit()
            End Try
        End With
        Return True
    End Function
    ''' <summary>
    ''' 設定情報チェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function CheckSetting() As Boolean
        '★TVTestパスチェック
        If Not Me.CheckTVTestPath() Then
            Return False
        End If
        '★保存パスチェック
        If Not Me.CheckInputSavePath() Then
            Return False
        End If
        Return True
    End Function
    ''' <summary>
    ''' 録画実行ファイルパス入力チェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function CheckTVTestPath() As Boolean
        If String.IsNullOrEmpty(Me.SettingRow.TVTEST_FULL_PATH) Then
            MessageBox.Show("TVTestのパスが指定されていません。", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
            Return False
        End If
        Return True
    End Function
    ''' <summary>
    ''' 保存先フォルダ入力チェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function CheckInputSavePath() As Boolean
        If String.IsNullOrEmpty(Me.SettingRow.SAVE_PATH) Then
            MessageBox.Show("保存先フォルダが指定されていません。", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
            Return False
        End If
        Return True
    End Function
    ''' <summary>
    ''' デバイスチェック
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
            MessageBox.Show("デバイスを入力してください。", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
            Return False
        End If
        If Not Me.TerrestrialDevice.All(Function(terrestrialDeviceRow As SchemaRecordingReservation.M_DEVICERow) Not terrestrialDeviceRow.DLL_NAME = dllName) Then
            MessageBox.Show(dllName & " は地上デジタルデバイスに既に登録されています。", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
            Return False
        End If
        If Not Me.BS110CSDevice.All(Function(bs110csDeviceRow As SchemaRecordingReservation.M_DEVICERow) Not bs110csDeviceRow.DLL_NAME = dllName) Then
            MessageBox.Show(dllName & " はＢＳ１１０ＣＳデバイスに既に登録されています。", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
            Return False
        End If
        Return True
    End Function
    ''' <summary>
    ''' アクティブコントロールに設定
    ''' </summary>
    ''' <param name="control"></param>
    ''' <remarks></remarks>
    Protected Sub SelectControl(ByVal control As Control)
        control.Select()
        If TypeOf control Is TextBox Then DirectCast(control, TextBox).SelectAll()
    End Sub
#End Region

End Class
