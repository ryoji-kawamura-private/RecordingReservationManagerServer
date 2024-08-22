Imports System.Net
Imports System.Security.Principal
Imports System.ServiceModel
Imports RK
Imports RK.Result
Imports RK.SchemaRecordingReservation

''' <summary>
''' メインフォーム
''' </summary>
''' <remarks></remarks>
Public Class FormMain

#Region " 定数・変数 "
    ''' <summary>設定実行ファイル名</summary>
    Protected Const SETTING_EXE_NAME As String = "SettingRecordingReservationClient.exe"
    ''' <summary>録画一覧フォーム</summary>
    Protected RecordedListForm As FormRecordedList
    ''' <summary>予約一覧フォーム</summary>
    Protected ListForm As FormRecordingReservationList
    ''' <summary>予約登録フォーム</summary>
    Protected RegistForm As FormRegistRecordingReservation
    ''' <summary>チャンネル設定フォーム</summary>
    Protected SettingChannelForm As formSettingChannel
    ''' <summary>ファイル名使用禁止文字</summary>
    Protected KINSI_MOJI() As String = New String() {"\", "/", ":", "*", "?", """", "<", ">", "|", "&amp;", "&quot;", "&lt;", "&gt;"}
    Protected KINSI_MOJI_REPLACE() As String = New String() {"￥", "／", "：", "＊", "？", "", "＜", "＞", "｜", "&", "", "＜", "＞"}
    ''' <summary>電源状態のメッセージ</summary>
    Private Const WM_POWERBROADCAST As System.Int32 = &H218
    ''' <summary>サスペンドへ移行</summary>
    Private PBT_APMQUERYSUSPEND As System.IntPtr = New System.IntPtr(&H0)
    ''' <summary>メッセージの拒否</summary>
    Private Const BROADCAST_QUERY_DENY As System.Int32 = &H424D5144
    ''' <summary>メッセージボックスタイトル</summary>
    Public Const MESSAGEBOX_TITLE As String = "録画予約"
    ''' <summary>日付フォーマット定数</summary>
    Public Const DATE_FORMAT As String = "yyyyMMddHHmm"
    ''' <summary>日付表示フォーマット定数</summary>
    Public Const DATE_DISP_FORMAT As String = "yyyy/MM/dd HH:mm"
#End Region

#Region " コンストラクタ "
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Me.Visible = False
        ' この呼び出しは、Windows フォーム デザイナで必要です。
        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        InitializeComponent()
        If Me.IsAdmin Then
            Me.SettingToolStripMenuItem.Image = Nothing
        Else
            Me.SettingToolStripMenuItem.Image = System.Drawing.SystemIcons.Shield.ToBitmap()
        End If
    End Sub
#End Region

#Region " イベント "

#Region " フォーム "
    '''' <summary>
    '''' オンロード
    '''' </summary>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        Me.Visible = False
        MyBase.OnLoad(e)
    End Sub
#End Region

#Region " タスクトレイアイコン "
    ''' <summary>
    ''' タスクトレイアイコンクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ReservationIcon_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ReservationIcon.MouseClick
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Left
                If Me.ListForm IsNot Nothing AndAlso Me.ListForm.Created Then
                    Me.ShowListDialog()
                ElseIf Me.RecordedListForm IsNot Nothing AndAlso Me.RecordedListForm.Created Then
                    Me.ShowRecordedListDialog()
                ElseIf Me.RegistForm IsNot Nothing AndAlso Me.RegistForm.Created Then
                    Me.ShowRegistDialog()
                ElseIf Me.SettingChannelForm IsNot Nothing AndAlso Me.SettingChannelForm.Created Then
                    Me.ShowSettingChannelDialog()
                End If
        End Select
    End Sub
    ''' <summary>
    ''' タスクトレイアイコンダブルクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ReservationIcon_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ReservationIcon.MouseDoubleClick
        Select Case e.Button
            Case Windows.Forms.MouseButtons.Left
                Me.ShowListDialog()
        End Select
    End Sub
    ''' <summary>
    ''' 録画一覧ダイアログ出力
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub RecordedListStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles RecordedListStripMenuItem.Click
        Me.ShowRecordedListDialog()
    End Sub
    ''' <summary>
    ''' 予約一覧ダイアログ表示
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ReservationListToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReservationListToolStripMenuItem.Click
        Me.ShowListDialog()
    End Sub
    ''' <summary>
    ''' 予約ダイアログ表示
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ReservationToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ReservationToolStripMenuItem.Click
        Me.ShowRegistDialog()
    End Sub
    ''' <summary>
    ''' チャンネル設定ダイアログ表示
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub SettingChannelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingChannelToolStripMenuItem.Click
        Me.ShowSettingChannelDialog()
    End Sub
    ''' <summary>
    ''' 設定ダイアログ表示
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub SettingToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SettingToolStripMenuItem.Click
        Me.ShowSettingDialog()
    End Sub
    ''' <summary>
    ''' アプリケーションを終了
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub FinishToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FinishToolStripMenuItem.Click
        If System.Windows.Forms.DialogResult.OK.Equals(MessageBox.Show("録画予約マネージャを終了しますか？", MESSAGEBOX_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, 0, False)) Then
            Me.ExitApplication()
        End If
    End Sub
#End Region

#End Region

#Region " 外部メソッド "
    ''' <summary>
    ''' 予約情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function GetReservation() As T_RESERVATIONDataTable
        Using RecordingReservationManagerProxy As New ChannelProxy(Of IRecordingReservationManager)(ConstClass.RECORING_RESERVATION_MANAGER)
            Return RecordingReservationManagerProxy.Channel.GetReservation()
        End Using
    End Function
    ''' <summary>
    ''' 録画一覧取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function GetRecordedList() As T_RESERVATIONDataTable
        Using RecordingReservationManagerProxy As New ChannelProxy(Of IRecordingReservationManager)(ConstClass.RECORING_RESERVATION_MANAGER)
            Return RecordingReservationManagerProxy.Channel.GetRecordedList()
        End Using
    End Function
    ''' <summary>
    '''ログ取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function GetLog(ByVal reservationRow As T_RESERVATIONRow) As T_LOGDataTable
        Using RecordingReservationManagerProxy As New ChannelProxy(Of IRecordingReservationManager)(ConstClass.RECORING_RESERVATION_MANAGER)
            Return RecordingReservationManagerProxy.Channel.GetLog(New T_RESERVATIONRowSerializer(reservationRow))
        End Using
    End Function
    ''' <summary>
    ''' 録画予約
    ''' </summary>
    ''' <param name="iEPGFilePath"></param>
    ''' <remarks></remarks>
    Friend Sub SetRecordingReservation(ByVal iEPGFilePath As String)
        Dim reservationRow As T_RESERVATIONRow = Me.GetReservationRow(iEPGFilePath)
        Me.SetRecordingReservation(reservationRow, False)
    End Sub
    ''' <summary>
    ''' 録画予約
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <remarks></remarks>
    Friend Function SetRecordingReservation(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow, ByVal force As Boolean) As Boolean
        Dim result As Result
        Using RecordingReservationManagerProxy As New ChannelProxy(Of IRecordingReservationManager)(ConstClass.RECORING_RESERVATION_MANAGER)
            result = RecordingReservationManagerProxy.Channel.SetRecordingReservation(New T_RESERVATIONRowSerializer(reservationRow), force)
        End Using
        Select Case result.ResultValue
            Case RESULT_VALUE.SUCCESS
                Me.ShowBalloon("番組予約登録", result.Message)
                Me.CloseAllForm()
                Me.ShowListDialog()
                Return True
            Case RESULT_VALUE.UNREGIST_CHANNEL_ERROR
                Me.ShowSettingChannelDialog(reservationRow)
                Return Me.SetRecordingReservation(reservationRow, force)
            Case RESULT_VALUE.DUPLICATE_PROGRAM_WARNING
                If MessageBox.Show(result.Message, MESSAGEBOX_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, 0, False) = Windows.Forms.DialogResult.OK Then
                    Me.SetRecordingReservation(reservationRow, True)
                End If
            Case Else
                MessageBox.Show(result.Message _
                              , MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, 0, False)
        End Select
    End Function
    ''' <summary>
    ''' 録画予約更新
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function UpdateRecordingReservation(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow) As Boolean
        Dim result As Result
        Using RecordingReservationManagerProxy As New ChannelProxy(Of IRecordingReservationManager)(ConstClass.RECORING_RESERVATION_MANAGER)
            result = RecordingReservationManagerProxy.Channel.UpdateRecordingReservation(New T_RESERVATIONRowSerializer(reservationRow))
        End Using
        If result.ResultValue = RESULT_VALUE.SUCCESS Then
            Me.ShowBalloon("番組予約修正", result.Message)
            Return True
        Else
            Return False
        End If
    End Function
    ''' <summary>
    ''' 録画予約削除
    ''' </summary>
    ''' <param name="reservationTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function DeleteRecordingReservation(ByVal reservationTable As T_RESERVATIONDataTable) As Boolean
        Dim result As Result
        Using RecordingReservationManagerProxy As New ChannelProxy(Of IRecordingReservationManager)(ConstClass.RECORING_RESERVATION_MANAGER)
            result = RecordingReservationManagerProxy.Channel.DeleteRecordingReservation(reservationTable)
        End Using
        If result.ResultValue = RESULT_VALUE.SUCCESS Then
            Me.ShowBalloon("削除", result.Message)
            Return True
        End If
        Return True
    End Function
    ''' <summary>
    ''' ログ削除
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function DeleteLog(ByVal id As Int64) As Boolean
        Using RecordingReservationManagerProxy As New ChannelProxy(Of IRecordingReservationManager)(ConstClass.RECORING_RESERVATION_MANAGER)
            RecordingReservationManagerProxy.Channel.DeleteLog(id)
        End Using
        Return True
    End Function
    ''' <summary>
    ''' 放送局情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Function GetStationList() As SchemaRecordingReservation.M_STATIONDataTable
        Using RecordingReservationManagerProxy As New ChannelProxy(Of IRecordingReservationManager)(ConstClass.RECORING_RESERVATION_MANAGER)
            Return RecordingReservationManagerProxy.Channel.GetStationList()
        End Using
    End Function
    ''' <summary>
    ''' チャンネル情報更新
    ''' </summary>
    ''' <param name="stationRow"></param>
    ''' <remarks></remarks>
    Friend Sub InsertStation(ByVal stationRow As SchemaRecordingReservation.M_STATIONRow)
        Using RecordingReservationManagerProxy As New ChannelProxy(Of IRecordingReservationManager)(ConstClass.RECORING_RESERVATION_MANAGER)
            RecordingReservationManagerProxy.Channel.InsertStation(New M_STATIONRowSerializer(stationRow))
        End Using
    End Sub
    ''' <summary>
    ''' チャンネル情報更新
    ''' </summary>
    ''' <param name="stationList"></param>
    ''' <remarks></remarks>
    Friend Sub UpdateStation(ByVal stationList As SchemaRecordingReservation.M_STATIONDataTable)
        Using RecordingReservationManagerProxy As New ChannelProxy(Of IRecordingReservationManager)(ConstClass.RECORING_RESERVATION_MANAGER)
            RecordingReservationManagerProxy.Channel.UpdateStation(stationList)
        End Using
    End Sub

#Region " 拡張子関連付け "
    ''' <summary>
    ''' 拡張子関連付け
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub SetExtentionInRegistry()
        'ファイルタイプを登録
        For Each extension As String In New String() {"tvpi", "tvpid", "epg"}
            Me.DeleteFileExtsSubkey(extension)
            Dim regkey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey("." & extension)
            regkey.SetValue("", extension & "_file")
            regkey.Close()
            'ファイルタイプとその説明を登録
            Dim shellkey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(extension & "_file")
            shellkey.SetValue("", "iEPG録画予約")

            '動詞とその説明を登録
            Dim openKey As Microsoft.Win32.RegistryKey = shellkey.CreateSubKey("shell\" + "open")
            openKey.SetValue("", "RecordingReservationManagerで開く(&O)")

            'コマンドラインを登録
            '実行するコマンドライン
            Dim commandline As String = System.Environment.GetCommandLineArgs(1) + " ""%1"""
            Dim commandKey As Microsoft.Win32.RegistryKey = openKey.CreateSubKey("command")
            commandKey.SetValue("", commandline)
            commandKey.Close()
            openKey.Close()
            shellkey.Close()
        Next
        'ファイルタイプ名
        Dim fileType As String = System.IO.Path.GetFileNameWithoutExtension(System.Environment.GetCommandLineArgs(1))
        Dim iconPath As String = System.Environment.GetCommandLineArgs(1)
        Dim iconkey As Microsoft.Win32.RegistryKey = Microsoft.Win32.Registry.ClassesRoot.CreateSubKey(fileType + "\DefaultIcon")
        iconkey.SetValue("", iconPath & "," & "0")
        iconkey.Close()
    End Sub
#End Region

#Region " 拡張子関連付け削除 "
    ''' <summary>
    ''' 拡張子関連付け削除
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ClearExtentionInRegistry()
        'レジストリキーを削除
        'ファイルタイプを削除
        For Each extension As String In New String() {"tvpi", "tvpid", "epg"}
            Me.DeleteFileExtsSubkey(extension)
            Me.DeleteSubKey(Microsoft.Win32.Registry.ClassesRoot, "." & extension)
            Me.DeleteSubKey(Microsoft.Win32.Registry.ClassesRoot, extension & "_file")
        Next
    End Sub
#End Region

#End Region

#Region " 内部メソッド "
    ''' <summary>
    ''' 全フォームクローズ
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CloseAllForm()
        For Each ownedForm As Form In Me.OwnedForms
            ownedForm.Visible = False
            ownedForm.Close()
            ownedForm.Dispose()
        Next
    End Sub
    ''' <summary>
    ''' 予約登録ダイアログ表示
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ShowRegistDialog()
        If Me.RegistForm Is Nothing OrElse (Not Me.RegistForm.Created) Then
            If Me.RegistForm IsNot Nothing Then
                Me.RegistForm.Dispose()
            End If
            Dim reservationContextMenuStrip As System.Windows.Forms.ContextMenuStrip = ClearContextMenuStrip()
            Try
                Me.RegistForm = New FormRegistRecordingReservation
                Dim reservationTbl As New SchemaRecordingReservation.T_RESERVATIONDataTable
                Using RecordingReservationManagerProxy As New ChannelProxy(Of IRecordingReservationManager)(ConstClass.RECORING_RESERVATION_MANAGER)
                    reservationTbl.ImportRow(RecordingReservationManagerProxy.Channel.GetSetting()(0))
                End Using
                reservationTbl(0).RECORDED = 0
                Me.RegistForm.ShowDialog(Me, reservationTbl(0))
            Finally
                If Me.RegistForm IsNot Nothing Then
                    Me.RegistForm.Dispose()
                    Me.RegistForm = Nothing
                End If
                Me.AddReservationContextMenu(reservationContextMenuStrip)
            End Try
        Else
            Me.RegistForm.Activate()
        End If
    End Sub
    ''' <summary>
    ''' 録画リストダイアログ表示
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ShowRecordedListDialog()
        If Me.RecordedListForm Is Nothing OrElse (Not Me.RecordedListForm.Created) Then
            If Me.RecordedListForm IsNot Nothing Then
                Me.RecordedListForm.Dispose()
            End If
            Dim reservationContextMenuStrip As System.Windows.Forms.ContextMenuStrip = ClearContextMenuStrip()
            Try
                Me.RecordedListForm = New FormRecordedList
                Me.RecordedListForm.ShowDialog(Me)
            Finally
                Me.AddReservationContextMenu(reservationContextMenuStrip)
                If Me.RecordedListForm IsNot Nothing Then
                    Me.RecordedListForm.Dispose()
                    Me.RecordedListForm = Nothing
                End If
            End Try
        Else
            Me.RecordedListForm.SetDataGrid()
        End If
    End Sub
    ''' <summary>
    ''' 予約リストダイアログ表示
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ShowListDialog()
        If Me.ListForm Is Nothing OrElse (Not Me.ListForm.Created) Then
            If Me.ListForm IsNot Nothing Then
                Me.ListForm.Dispose()
            End If
            Dim reservationContextMenuStrip As System.Windows.Forms.ContextMenuStrip = ClearContextMenuStrip()
            Try
                Me.ListForm = New FormRecordingReservationList
                Me.ListForm.ShowDialog(Me)
            Finally
                Me.AddReservationContextMenu(reservationContextMenuStrip)
                If Me.ListForm IsNot Nothing Then
                    Me.ListForm.Dispose()
                    Me.ListForm = Nothing
                End If
            End Try
        Else
            Me.ListForm.SetDataGrid()
        End If
    End Sub
    ''' <summary>
    ''' 設定情報ダイアログを起動
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ShowSettingDialog()
        Dim reservationContextMenuStrip As System.Windows.Forms.ContextMenuStrip = ClearContextMenuStrip()
        Try
            Dim settingProcesses() As System.Diagnostics.Process = Me.GetProcesses(System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), SETTING_EXE_NAME))
            If (0).Equals(settingProcesses.Length) Then
                Dim settingProcessInfo As New System.Diagnostics.ProcessStartInfo
                With settingProcessInfo
                    .CreateNoWindow = False
                    .UseShellExecute = True
                    .WorkingDirectory = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath)
                    .FileName = SETTING_EXE_NAME
                    If IsAdmin() Then
                        .Verb = ""
                    Else
                        .Verb = "runas"
                    End If
                    .Arguments = """" & System.Windows.Forms.Application.ExecutablePath & """"
                End With
                '★設定ダイアログプロセススタート
                Dim settingProcess As System.Diagnostics.Process = System.Diagnostics.Process.Start(settingProcessInfo)
            End If
        Catch ex As System.ComponentModel.Win32Exception
            '//NOP
        Catch
            Throw
        Finally
            Me.AddReservationContextMenu(reservationContextMenuStrip)
        End Try
    End Sub
    ''' <summary>
    ''' 管理者権限かどうかを取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function IsAdmin() As Boolean
        Dim usrId As WindowsIdentity = WindowsIdentity.GetCurrent()
        Dim principal As WindowsPrincipal = New WindowsPrincipal(usrId)
        Return principal.IsInRole("BUILTIN\Administrators")
    End Function
    ''' <summary>
    ''' プロセス存在チェック
    ''' </summary>
    ''' <remarks></remarks>
    Protected Function GetProcesses(ByVal fullPath As String) As System.Diagnostics.Process()
        Return System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(fullPath))
    End Function
    ''' <summary>
    ''' 録画リスト再設定
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ResetListForm()
        If Me.ListForm IsNot Nothing AndAlso Me.ListForm.Created Then
            Me.ListForm.SetDataGrid()
        End If
    End Sub
    ''' <summary>
    ''' 録画済リスト再設定
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ResetRecordedListForm()
        If Me.RecordedListForm IsNot Nothing AndAlso Me.RecordedListForm.Created Then
            Me.RecordedListForm.SetDataGrid()
        End If
    End Sub
    ''' <summary>
    ''' チャンネル設定ダイアログを起動
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ShowSettingChannelDialog()
        If Me.SettingChannelForm Is Nothing OrElse (Not Me.SettingChannelForm.Created) Then
            If Me.SettingChannelForm IsNot Nothing Then
                Me.SettingChannelForm.Dispose()
            End If
            Try
                Dim stationList As SchemaRecordingReservation.M_STATIONDataTable = Nothing
                Using RecordingReservationManagerProxy As New ChannelProxy(Of IRecordingReservationManager)(ConstClass.RECORING_RESERVATION_MANAGER)
                    stationList = RecordingReservationManagerProxy.Channel.GetStationList()
                End Using
                Me.SettingChannelForm = New formSettingChannel
                If Not Windows.Forms.DialogResult.OK.Equals(Me.SettingChannelForm.ShowDialog(Me, stationList, Nothing)) Then
                    Return
                End If
            Finally
                If Me.SettingChannelForm IsNot Nothing Then
                    Me.SettingChannelForm.Dispose()
                    Me.SettingChannelForm = Nothing
                End If
            End Try
        Else
            Me.SettingChannelForm.Activate()
        End If
    End Sub
    ''' <summary>
    ''' チャンネル設定ダイアログを起動
    ''' </summary>
    ''' <remarks></remarks>
    Protected Function ShowSettingChannelDialog(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow) As Windows.Forms.DialogResult
        If Me.SettingChannelForm IsNot Nothing Then
            If Me.SettingChannelForm.Created Then
                Me.SettingChannelForm.Close()
                Me.SettingChannelForm.Dispose()
            End If
        End If
        Try
            Dim stationList As SchemaRecordingReservation.M_STATIONDataTable
            Using RecordingReservationManagerProxy As New ChannelProxy(Of IRecordingReservationManager)(ConstClass.RECORING_RESERVATION_MANAGER)
                stationList = RecordingReservationManagerProxy.Channel.GetStationList()
            End Using
            Me.SettingChannelForm = New formSettingChannel
            Return Me.SettingChannelForm.ShowDialog(Me, stationList, reservationRow)
        Finally
            If Me.SettingChannelForm IsNot Nothing Then
                Me.SettingChannelForm.Dispose()
                Me.SettingChannelForm = Nothing
            End If
        End Try
    End Function
    ''' <summary>
    ''' トゥールチップを消す。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function ClearContextMenuStrip() As System.Windows.Forms.ContextMenuStrip
        Dim reservationContextMenuStrip As ContextMenuStrip = Me.ReservationIcon.ContextMenuStrip
        Me.ReservationIcon.ContextMenuStrip = Nothing
        RemoveHandler Me.ReservationIcon.MouseDoubleClick, AddressOf Me.ReservationIcon_MouseDoubleClick
        Return reservationContextMenuStrip
    End Function
    ''' <summary>
    ''' トゥールチップを戻す。
    ''' </summary>
    ''' <param name="reservationContextMenuStrip"></param>
    ''' <remarks></remarks>
    Protected Sub AddReservationContextMenu(ByVal reservationContextMenuStrip As System.Windows.Forms.ContextMenuStrip)
        If reservationContextMenuStrip IsNot Nothing Then
            Me.ReservationIcon.ContextMenuStrip = reservationContextMenuStrip
            AddHandler Me.ReservationIcon.MouseDoubleClick, AddressOf Me.ReservationIcon_MouseDoubleClick
        End If
    End Sub
    ''' <summary>
    ''' 番組録画情報をiEPGファイルから取得
    ''' </summary>
    ''' <remarks></remarks>
    Protected Function GetReservationRow(ByVal iEPGFilePath As String) As SchemaRecordingReservation.T_RESERVATIONRow
        '★文字コード(ここでは、Shift JIS)
        Dim enc As System.Text.Encoding = System.Text.Encoding.GetEncoding(932)
        '★行ごとの配列として、テキストファイルの中身をすべて読み込む
        Dim lines As String() = System.IO.File.ReadAllLines(iEPGFilePath, enc)
        Using ReservationTbl As New SchemaRecordingReservation.T_RESERVATIONDataTable
            Dim stationList As SchemaRecordingReservation.M_STATIONDataTable
            Using RecordingReservationManagerProxy As New ChannelProxy(Of IRecordingReservationManager)(ConstClass.RECORING_RESERVATION_MANAGER)
                stationList = RecordingReservationManagerProxy.Channel.GetStationList()
                ReservationTbl.ImportRow(RecordingReservationManagerProxy.Channel.GetSetting()(0))
            End Using
            Dim reservationRow As SchemaRecordingReservation.T_RESERVATIONRow = ReservationTbl(0)
            Dim year As String = String.Empty
            Dim month As String = String.Empty
            Dim day As String = String.Empty
            Dim startTime As String = String.Empty
            Dim endTime As String = String.Empty
            For Each line As String In lines
                Select Case True
                    Case line.Trim.StartsWith("station:")
                        reservationRow.STATION = line.Replace("station:", String.Empty).Trim
                    Case line.Trim.StartsWith("station-name:")
                        reservationRow.STATION_NAME = line.Replace("station-name:", String.Empty).Trim
                    Case line.Trim.StartsWith("year:")
                        year = line.Replace("year:", String.Empty).Trim.PadLeft(4, "0"c)
                    Case line.Trim.StartsWith("month:")
                        month = line.Replace("month:", String.Empty).Trim.PadLeft(2, "0"c)
                    Case line.Trim.StartsWith("date:")
                        day = line.Replace("date:", String.Empty).Trim.PadLeft(2, "0"c)
                    Case line.Trim.StartsWith("start:")
                        startTime = line.Replace("start:", String.Empty).Replace(":", String.Empty).Trim
                    Case line.Trim.StartsWith("end:")
                        endTime = line.Replace("end:", String.Empty).Replace(":", String.Empty).Trim
                    Case line.Trim.StartsWith("program-title:")
                        reservationRow.PROGRAM_TITLE = line.Replace("program-title:", String.Empty).Trim
                        For i As Integer = 0 To Me.KINSI_MOJI.Length - 1
                            reservationRow.PROGRAM_TITLE = reservationRow.PROGRAM_TITLE.Replace(KINSI_MOJI(i), KINSI_MOJI_REPLACE(i))
                        Next
                    Case line.Trim.StartsWith("program-subtitle:")
                        reservationRow.PROGRAM_SUBTITLE = line.Replace("program-subtitle:", String.Empty).Trim
                        For i As Integer = 0 To Me.KINSI_MOJI.Length - 1
                            reservationRow.PROGRAM_SUBTITLE = reservationRow.PROGRAM_SUBTITLE.Replace(KINSI_MOJI(i), KINSI_MOJI_REPLACE(i))
                        Next
                    Case line.Trim.StartsWith("extend:")
                        reservationRow.EXTEND = Convert.ToInt64(line.Replace("extend:", String.Empty).Trim)
                    Case line.Trim.StartsWith("performer:")
                        reservationRow.PERFORMER = line.Replace("performer:", String.Empty).Trim
                    Case line.Trim.StartsWith("genre:")
                        reservationRow.GENRE = Convert.ToInt64(line.Replace("genre:", String.Empty).Trim)
                    Case line.Trim.StartsWith("subgenre:")
                        reservationRow.SUBGENRE = Convert.ToInt64(line.Replace("subgenre:", String.Empty).Trim)
                    Case line.Trim.StartsWith("’")
                        reservationRow.NOTE = line.Replace("’", String.Empty).Trim
                End Select
            Next
            reservationRow.START_YYYYMMDDHHMM = year & month & day & startTime
            Dim endHour As System.Int32 = Convert.ToInt32(endTime.Substring(0, 2))
            If endHour >= 24 Then
                endHour -= 24
                endTime = endHour.ToString.PadLeft(2, "0"c) & endTime.Substring(2, 2)
            End If
            If startTime < endTime Then
                reservationRow.END_YYYYMMDDHHMM = year & month & day & endTime
            Else
                reservationRow.END_YYYYMMDDHHMM = DateTime.ParseExact(year & month & day & endTime, DATE_FORMAT, Nothing).AddDays(1D).ToString(DATE_FORMAT)
            End If
            reservationRow.RECORDED = 0L
            reservationRow.CLIENT_PC_NAME = System.Environment.MachineName
            Return reservationRow
        End Using
    End Function
    ''' <summary>
    ''' バルーン表示
    ''' </summary>
    ''' <param name="title"></param>
    ''' <param name="text"></param>
    ''' <remarks></remarks>
    Protected Sub ShowBalloon(ByVal title As String, ByVal text As String)
        Me.ReservationIcon.Visible = True
        Me.ReservationIcon.BalloonTipTitle = title
        Me.ReservationIcon.BalloonTipText = text
        Me.ReservationIcon.ShowBalloonTip(60000)
    End Sub
    ''' <summary>
    ''' バルーン非表示
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearBallon()
        Me.ReservationIcon.Visible = False
        Me.ReservationIcon.Visible = True
    End Sub

#Region "拡張子関連付けレジストリ削除"
    ''' <summary>
    ''' 拡張子関連付けレジストリ削除
    ''' </summary>
    ''' <param name="extension"></param>
    ''' <remarks></remarks>
    Protected Sub DeleteFileExtsSubkey(ByVal extension As String)
        Me.DeleteSubKey(Microsoft.Win32.Registry.CurrentUser, "Software\Microsoft\Windows\CurrentVersion\Explorer\ComDlg32\OpenSavePidlMRU\" & extension)
        Me.DeleteSubKey(Microsoft.Win32.Registry.CurrentUser, "Software\Microsoft\Windows\CurrentVersion\Explorer\FileExts\." & extension)
    End Sub
#End Region

#Region "レジストリサブキー削除"
    ''' <summary>
    ''' レジストリサブキー削除
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

#Region "レジストリサブキー削除（再帰呼出）"
    ''' <summary>
    ''' レジストリサブキー削除（再帰呼出）
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

    ''' <summary>
    ''' 終了処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ExitApplication()
        System.Windows.Forms.Application.Exit()
    End Sub
#End Region

End Class