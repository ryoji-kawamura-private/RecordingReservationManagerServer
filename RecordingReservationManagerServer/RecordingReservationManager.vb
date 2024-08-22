Imports System.ServiceModel
Imports System.Security.Principal
Imports RK.SchemaRecordingReservation
Imports RK.Result
Imports System.Net
Imports System.Runtime.InteropServices
Imports System.Threading

<ServiceBehavior(InstanceContextMode:=InstanceContextMode.Single)> _
Public Class RecordingReservationManager : Inherits MarshalByRefObject : Implements IRecordingReservationManager

#Region " 定数・変数 "
    ''' <summary>設定実行ファイル名</summary>
    Protected Const SETTING_EXE_NAME As String = "SettingRecordingReservation.exe"
    ''' <summary>
    ''' イベント引数
    ''' </summary>
    ''' <remarks></remarks>
    Public Class RecordingReservationEventArgs : Inherits EventArgs
        ''' <summary>
        ''' 予約行
        ''' </summary>
        ''' <remarks></remarks>
        Protected _ReservationRow As SchemaRecordingReservation.T_RESERVATIONRow
        ''' <summary>
        ''' 予約行
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ReservationRow() As SchemaRecordingReservation.T_RESERVATIONRow
            Get
                Return Me._ReservationRow
            End Get
        End Property
        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        ''' <param name="reservationRow"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow)
            Me._ReservationRow = reservationRow
        End Sub
    End Class
    ''' <summary>
    ''' 録画開始イベント
    ''' </summary>
    ''' <param name="sender">イベントが発生したオブジェクト</param>
    ''' <param name="e">イベント固有引数</param>
    ''' <remarks></remarks>
    Public Event StartRecording(ByVal sender As Object, ByVal e As RecordingReservationEventArgs)
    ''' <summary>
    ''' 録画終了イベント
    ''' </summary>
    ''' <param name="sender">イベントが発生したオブジェクト</param>
    ''' <param name="e">イベント固有引数</param>
    ''' <remarks></remarks>
    Public Event EndRecording(ByVal sender As Object, ByVal e As RecordingReservationEventArgs)
    ''' <summary>唯一のインスタンス</summary>
    Protected Shared _Instance As RecordingReservationManager
    ''' <summary>マルチスレッド時のロック</summary>
    Protected Shared syncRoot As Object = New Object()
    ''' <summary>エンティティ</summary>
    Protected Entity As EntityRecordingReservation
    ''' <summary>ファイル名使用禁止文字</summary>
    Protected KINSI_MOJI() As String = New String() {"\", "/", ":", "*", "?", """", "<", ">", "|", "&amp;", "&quot;", "&lt;", "&gt;"}
    Protected KINSI_MOJI_REPLACE() As String = New String() {"￥", "／", "：", "＊", "？", "", "＜", "＞", "｜", "&", "", "＜", "＞"}
    ''' <summary>ＥＸＥ起動後録画開始時間</summary>
    Protected Const DELAY_SECONDS As System.Int64 = 5L
    '''' <summary>セッティング</summary>
    'Protected SettingRow As SchemaRecordingReservation.M_SETTINGRow
    ''' <summary>録画デリゲート</summary>
    Protected Delegate Function RecordingDelegate(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow) As SchemaRecordingReservation.T_RESERVATIONRow
    ''' <summary>録画スレッドリストロック</summary>
    Protected Shared ReadOnly LockRecordingThreadDictionary As Object = New Object()
    ''' <summary>録画スレッドディクショナリ</summary>
    Protected Shared RecordingThreadDictionary As Generic.Dictionary(Of System.Int64, System.Threading.Thread) = New Generic.Dictionary(Of System.Int64, System.Threading.Thread)
    ''' <summary>録画情報リストロック</summary>
    Protected Shared ReadOnly LockRecordingReservationDictionary As Object = New Object()
    ''' <summary>録画スレッドディクショナリ</summary>
    Protected Shared RecordingReservationDictionary As Generic.Dictionary(Of System.Int64, SchemaRecordingReservation.T_RESERVATIONRow) = New Generic.Dictionary(Of System.Int64, SchemaRecordingReservation.T_RESERVATIONRow)
    ''' <summary>プロセスＩＤリストロック</summary>
    Protected Shared ReadOnly LockProcessIDDictionary As Object = New Object()
    ''' <summary>プロセスＩＤリスト</summary>
    Protected Shared ProcessIDDictionary As Generic.Dictionary(Of String, Int32) = New Generic.Dictionary(Of String, Int32)
    ''' <summary>電源状態のメッセージ</summary>
    Private Const WM_POWERBROADCAST As System.Int32 = &H218
    ''' <summary>サスペンドへ移行</summary>
    Private PBT_APMQUERYSUSPEND As System.IntPtr = New System.IntPtr(&H0)
    ''' <summary>メッセージの拒否</summary>
    Private Const BROADCAST_QUERY_DENY As System.Int32 = &H424D5144
#End Region

#Region " プロパティ "
    ''' <summary>
    ''' シングルトンインスタンス
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property Instance() As RecordingReservationManager
        Get
            If _Instance Is Nothing Then
                SyncLock syncRoot
                    RecordingReservationManager._Instance = New RecordingReservationManager
                End SyncLock
            End If
            Return _Instance
        End Get
    End Property
#End Region

#Region " ＡＰＩ関数の定義 "

    Protected Const INFINITE As Int32 = &HFFFFFFFF
    Protected Const ES_SYSTEM_REQUIRED As System.Int32 = &H1
    Protected Const ES_DISPLAY_REQUIRED As System.Int32 = &H2
    Protected Const ES_USER_PRESENT As System.Int32 = &H4
    Protected Const ES_CONTINUOUS As System.Int32 = &H80000000
    Protected Const STANDARD_RIGHTS_REQUIRED As System.UInt32 = &HF0000
    Protected Const SYNCHRONIZE As UInt32 = &H100000
    Protected Const TIMER_QUERY_STATE As UInt32 = &H1
    Protected Const TIMER_MODIFY_STATE As UInt32 = &H2
    Protected Const TIMER_ALL_ACCESS As UInt32 = (STANDARD_RIGHTS_REQUIRED Or SYNCHRONIZE Or TIMER_QUERY_STATE Or TIMER_MODIFY_STATE)
    Protected Const WM_COMMAND As System.Int32 = &H111
    Protected Const BN_CLICKED As System.Int32 = &H0

    Public Delegate Sub TimerSetDelegate()
    <System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet:=System.Runtime.InteropServices.CharSet.Auto)> _
    Private Shared Function CreateWaitableTimer(ByVal lpSemaphoreAttributes As System.IntPtr, _
                                                ByVal bManualReset As System.Boolean, _
                                                ByVal lpName As String) As System.IntPtr
    End Function
    <System.Runtime.InteropServices.DllImport("kernel32.dll", CharSet:=System.Runtime.InteropServices.CharSet.Auto)> _
    Private Shared Function OpenWaitableTimer(ByVal dwDesiredAccess As System.UInt32, _
                                              ByVal bInheritHandle As System.Boolean, _
                                              ByVal lpName As System.String) As System.IntPtr
    End Function
    <System.Runtime.InteropServices.DllImport("kernel32.dll")> _
    Private Shared Function SetWaitableTimer(ByVal hTimer As System.IntPtr, _
                                             <System.Runtime.InteropServices.In()> ByRef lpDueTime As System.Int64, _
                                             ByVal lPeriod As System.Int32, _
                                             ByVal pfnCompletionRoutine As TimerSetDelegate, _
                                             ByVal pArgToCompletionRoutine As System.IntPtr, _
                                             ByVal fResume As System.Boolean) As System.Boolean
    End Function
    <System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError:=True, ExactSpelling:=True)> _
    Private Shared Function WaitForSingleObject(ByVal hHandle As IntPtr, ByVal dwMilliseconds As System.Int32) As System.Int32
    End Function
    <System.Runtime.InteropServices.DllImport("kernel32.dll")> _
    Private Shared Function CancelWaitableTimer(ByVal hTimer As System.IntPtr) As System.Boolean
    End Function
    <System.Runtime.InteropServices.DllImport("kernel32.dll")> _
    Private Shared Function SetThreadExecutionState(ByVal esFlags As System.Int32) As System.Int32
    End Function
    <System.Runtime.InteropServices.DllImport("user32.dll", CharSet:=System.Runtime.InteropServices.CharSet.Auto)> _
    Private Shared Function PostMessage(ByVal hWnd As IntPtr, ByVal wMsg As System.Int32, ByVal wParam As System.Int32, ByVal lParam As System.Int32) As System.Int32
    End Function
    <System.Runtime.InteropServices.DllImport("user32.dll", CharSet:=System.Runtime.InteropServices.CharSet.Auto)> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal wMsg As System.Int32, ByVal wParam As System.Int32, ByVal lParam As System.Int32) As System.Int32
    End Function
    <System.Runtime.InteropServices.DllImport("user32.dll", CharSet:=System.Runtime.InteropServices.CharSet.Auto)> _
    Private Shared Function FindWindowEx(ByVal hWnd1 As System.IntPtr, ByVal hWnd2 As System.Int32, ByVal lpsz1 As String, ByVal lpsz2 As String) As System.IntPtr
    End Function
    <System.Runtime.InteropServices.DllImport("user32.dll", CharSet:=System.Runtime.InteropServices.CharSet.Auto)> _
    Private Shared Function GetDlgCtrlID(ByVal hWnd As System.IntPtr) As System.Int32
    End Function
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure STARTUPINFO
        Public cb As Integer
        Public lpReserved As [String]
        Public lpDesktop As [String]
        Public lpTitle As [String]
        Public dwX As UInteger
        Public dwY As UInteger
        Public dwXSize As UInteger
        Public dwYSize As UInteger
        Public dwXCountChars As UInteger
        Public dwYCountChars As UInteger
        Public dwFillAttribute As UInteger
        Public dwFlags As UInteger
        Public wShowWindow As Short
        Public cbReserved2 As Short
        Public lpReserved2 As IntPtr
        Public hStdInput As IntPtr
        Public hStdOutput As IntPtr
        Public hStdError As IntPtr
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure PROCESS_INFORMATION
        Public hProcess As IntPtr
        Public hThread As IntPtr
        Public dwProcessId As UInteger
        Public dwThreadId As UInteger
    End Structure
    <StructLayout(LayoutKind.Sequential)> _
    Public Structure SECURITY_ATTRIBUTES
        Public Length As Integer
        Public lpSecurityDescriptor As IntPtr
        Public bInheritHandle As Boolean
    End Structure
    <DllImport("kernel32.dll", EntryPoint:="CloseHandle", SetLastError:=True, CharSet:=CharSet.Auto, CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function CloseHandle(ByVal handle As IntPtr) As Boolean
    End Function
    '<System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError:=True, CallingConvention:=Runtime.InteropServices.CallingConvention.Winapi, CharSet:=Runtime.InteropServices.CharSet.Auto)> _
    'Private Shared Function CloseHandle(ByVal hObject As System.IntPtr) As <System.Runtime.InteropServices.MarshalAs(Runtime.InteropServices.UnmanagedType.Bool)> System.Boolean
    'End Function
    <DllImport("advapi32.dll", EntryPoint:="CreateProcessAsUser", SetLastError:=True, CharSet:=CharSet.Ansi, CallingConvention:=CallingConvention.StdCall)> _
    Public Shared Function CreateProcessAsUser(ByVal hToken As IntPtr, ByVal lpApplicationName As [String], ByVal lpCommandLine As [String], ByRef lpProcessAttributes As SECURITY_ATTRIBUTES, ByRef lpThreadAttributes As SECURITY_ATTRIBUTES, ByVal bInheritHandle As Boolean, _
    ByVal dwCreationFlags As Integer, ByVal lpEnvironment As IntPtr, ByVal lpCurrentDirectory As [String], ByRef lpStartupInfo As STARTUPINFO, ByRef lpProcessInformation As PROCESS_INFORMATION) As Boolean
    End Function
    <DllImport("advapi32.dll", EntryPoint:="DuplicateTokenEx")> _
    Public Shared Function DuplicateTokenEx(ByVal ExistingTokenHandle As IntPtr, ByVal dwDesiredAccess As UInteger, ByRef lpThreadAttributes As SECURITY_ATTRIBUTES, ByVal TokenType As Integer, ByVal ImpersonationLevel As Integer, ByRef DuplicateTokenHandle As IntPtr) As Boolean
    End Function
#End Region

#Region " コンストラクタ "
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub New()
        DirectCast(My.Application.ApplicationContext.MainForm, FormMain).RecordingReservationManagerInstance = Me
        Me.Entity = New EntityRecordingReservation
        Dim reservationTbl As SchemaRecordingReservation.T_RESERVATIONDataTable = Entity.GetStartReservation()
        For Each reservationRow As SchemaRecordingReservation.T_RESERVATIONRow In reservationTbl
            Me.StartRecordingThread(reservationRow)
        Next
        '★終了した予約を予約済みとする。
        Entity.BeginTran()
        Try
            Entity.UpdatePastReservationRecorded()
        Finally
            Entity.Commit()
        End Try
    End Sub
#End Region

#Region " 外部メソッド "
    ''' <summary>
    ''' 設定情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function GetSetting() As SchemaRecordingReservation.M_SETTINGDataTable Implements IRecordingReservationManager.GetSetting
        Return Me.Entity.GetSetting()
    End Function
    ''' <summary>
    ''' 設定情報取得
    ''' </summary>
    ''' <param name="settingTable"></param>
    ''' <param name="terrestrialDevice"></param>
    ''' <param name="bS110CSDevice"></param>
    ''' <remarks></remarks>
    Public Overridable Sub GetSettings(ByRef settingTable As M_SETTINGDataTable, _
                           ByRef terrestrialDevice As M_DEVICEDataTable, _
                           ByRef bS110CSDevice As M_DEVICEDataTable) Implements IRecordingReservationManager.GetSettings
        settingTable = Me.Entity.GetSetting()
        If settingTable.Count <= 0 Then
            settingTable.AddM_SETTINGRow(settingTable.NewM_SETTINGRow)
        End If
        terrestrialDevice = Me.Entity.GetDevice(BROADCAST_TYPE.TERRESTRIAL)
        bS110CSDevice = Me.Entity.GetDevice(BROADCAST_TYPE.BS110CS)
    End Sub
    ''' <summary>
    ''' 予約情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function GetReservation() As SchemaRecordingReservation.T_RESERVATIONDataTable Implements IRecordingReservationManager.GetReservation
        Return Me.Entity.GetReservation()
    End Function
    ''' <summary>
    ''' 録画一覧取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function GetRecordedList() As SchemaRecordingReservation.T_RESERVATIONDataTable Implements IRecordingReservationManager.GetRecordedList
        Return Me.Entity.GetRecordedList()
    End Function
    ''' <summary>
    ''' 録画一覧
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function GetRecordedEnumerableList() As IEnumerable(Of T_RESERVATIONRowSerializer) Implements IRecordingReservationManager.GetRecordedEnumerableList
        Return Me.GenerateReservationEnumerableList(Me.Entity.GetRecordedList())
    End Function
    ''' <summary>
    '''ログ取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function GetLog(ByVal reservationRowSerializer As T_RESERVATIONRowSerializer) As SchemaRecordingReservation.T_LOGDataTable Implements IRecordingReservationManager.GetLog
        Return Me.Entity.GetLog(reservationRowSerializer.ReservationRow)
    End Function
    ''' <summary>
    ''' 録画予約
    ''' </summary>
    ''' <param name="reservationRowSerializer"></param>
    ''' <remarks></remarks>
    Public Overridable Function SetRecordingReservation(ByVal reservationRowSerializer As T_RESERVATIONRowSerializer, ByVal force As Boolean) As Result Implements IRecordingReservationManager.SetRecordingReservation
        'WriteEventLog("System.Environment.UserName = " & System.Environment.UserDomainName & "\" & System.Environment.UserName)
        'WriteEventLog("ServiceSecurityContext.Current.WindowsIdentity.Name = " & ServiceSecurityContext.Current.WindowsIdentity.Name)
        Dim result As Result
        If Not force Then
            result = Me.CheckRecordingReservation(reservationRowSerializer.ReservationRow)
            If result.ResultValue <> RESULT_VALUE.SUCCESS Then
                Return result
            End If
        End If
        result = Me.SetChannel(reservationRowSerializer.ReservationRow)
        If result.ResultValue <> RESULT_VALUE.SUCCESS Then
            Return result
        End If
        Me.SetChannelType(reservationRowSerializer.ReservationRow)
        result = Me.SetDevice(reservationRowSerializer.ReservationRow)
        If result.ResultValue <> RESULT_VALUE.SUCCESS Then
            Return result
        End If
        With Entity
            .BeginTran()
            Try
                reservationRowSerializer.ReservationRow.ID = .GetID()
                .InsertReservation(reservationRowSerializer.ReservationRow)
            Finally
                .Commit()
            End Try
        End With
        '★録画スレッド開始
        Me.StartRecordingThread(reservationRowSerializer.ReservationRow)
        Return New Result(RESULT_VALUE.SUCCESS, "下記の番組予約を登録しました。" & System.Environment.NewLine & _
                                      reservationRowSerializer.ReservationRow.PROGRAM_TITLE & " " & _
                                      DateTime.ParseExact(reservationRowSerializer.ReservationRow.START_YYYYMMDDHHMM, DATE_FORMAT, Nothing).ToString(DATE_DISP_FORMAT) & "  -  " & _
                                      DateTime.ParseExact(reservationRowSerializer.ReservationRow.END_YYYYMMDDHHMM, DATE_FORMAT, Nothing).ToString(DATE_DISP_FORMAT) & " ")
    End Function
    ''' <summary>
    ''' 録画予約更新
    ''' </summary>
    ''' <param name="reservationRowSerializer"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function UpdateRecordingReservation(ByVal reservationRowSerializer As T_RESERVATIONRowSerializer) As Result Implements IRecordingReservationManager.UpdateRecordingReservation
        Me.SetChannelType(reservationRowSerializer.ReservationRow)
        With Entity
            .BeginTran()
            Try
                .UpdateReservation(reservationRowSerializer.ReservationRow)
            Finally
                .Commit()
            End Try
        End With
        If (2L).Equals(reservationRowSerializer.ReservationRow.RECORDED) Then
            '★録画中の場合はデータも更新する。
            SyncLock LockRecordingReservationDictionary
                For colIdx As System.Int32 = 0 To reservationRowSerializer.ReservationRow.Table.Columns.Count - 1
                    RecordingReservationManager.RecordingReservationDictionary(reservationRowSerializer.ReservationRow.ID)(colIdx) = reservationRowSerializer.ReservationRow(colIdx)
                Next
            End SyncLock
        Else
            '★録画スレッドキャンセル
            Me.CancelRecordingThread(reservationRowSerializer.ReservationRow)
            '★録画スレッド開始
            Me.StartRecordingThread(reservationRowSerializer.ReservationRow)
        End If
        Return New Result(RESULT_VALUE.SUCCESS, "下記の番組予約を修正しました。" & System.Environment.NewLine & _
                          reservationRowSerializer.ReservationRow.PROGRAM_TITLE & " " & _
                          DateTime.ParseExact(reservationRowSerializer.ReservationRow.START_YYYYMMDDHHMM, DATE_FORMAT, Nothing).ToString(DATE_DISP_FORMAT) & "  -  " & _
                          DateTime.ParseExact(reservationRowSerializer.ReservationRow.END_YYYYMMDDHHMM, DATE_FORMAT, Nothing).ToString(DATE_DISP_FORMAT) & " ")
    End Function
    ''' <summary>
    ''' 録画予約削除
    ''' </summary>
    ''' <param name="reservationTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function DeleteRecordingReservation(ByVal reservationTable As T_RESERVATIONDataTable) As Result Implements IRecordingReservationManager.DeleteRecordingReservation
        '★録画予約削除プロセススタート
        With Entity
            .BeginTran()
            Try
                For Each reservationRow As T_RESERVATIONRow In reservationTable
                    .DeleteReservation(reservationRow.ID)
                    .DeleteLog(reservationRow.ID)
                    If reservationRow.RECORDED = 1L Then
                        '★ファイルを削除
                        Try
                            System.IO.File.Delete(reservationRow.PATH)
                            System.IO.File.Delete(reservationRow.PATH & ".err")
                            System.IO.File.Delete(reservationRow.PATH & ".txt")
                        Catch ex As Exception
                            '//NOP
                        End Try
                    Else
                        '★録画スレッドキャンセル
                        Me.CancelRecordingThread(reservationRow)
                    End If
                Next
            Finally
                .Commit()
            End Try
        End With
        Return New Result(RESULT_VALUE.SUCCESS, "削除しました。")
    End Function
    ''' <summary>
    ''' ログ削除
    ''' </summary>
    ''' <param name="id"></param>
    ''' <remarks></remarks>
    Public Overridable Function DeleteLog(ByVal id As System.Int64) As Result Implements IRecordingReservationManager.DeleteLog
        With Entity
            .BeginTran()
            Try
                .DeleteLog(id)
            Finally
                .Commit()
            End Try
        End With
        Return New Result(RESULT_VALUE.SUCCESS, "ログを削除しました。")
    End Function
    ''' <summary>
    ''' 放送局情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function GetStationList() As SchemaRecordingReservation.M_STATIONDataTable Implements IRecordingReservationManager.GetStationList
        Return Me.Entity.GetStationList()
    End Function
    ''' <summary>
    ''' チャンネル情報更新
    ''' </summary>
    ''' <param name="stationRowSerializer"></param>
    ''' <remarks></remarks>
    Public Overridable Sub InsertStation(ByVal stationRowSerializer As M_STATIONRowSerializer) Implements IRecordingReservationManager.InsertStation
        With Entity
            .BeginTran()
            Try
                .InsertStation(stationRowSerializer.StationRow)
            Finally
                .Commit()
            End Try
        End With
    End Sub
    ''' <summary>
    ''' チャンネル情報更新
    ''' </summary>
    ''' <param name="stationList"></param>
    ''' <remarks></remarks>
    Public Overridable Sub UpdateStation(ByVal stationList As SchemaRecordingReservation.M_STATIONDataTable) Implements IRecordingReservationManager.UpdateStation
        With Me.Entity
            .BeginTran()
            Try
                .DeleteStation()
                For Each stationRow As SchemaRecordingReservation.M_STATIONRow In stationList
                    .InsertStation(stationRow)
                Next
            Finally
                .Commit()
            End Try
        End With
    End Sub
    ''' <summary>
    ''' 設定情報更新
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function UpdateSetting(ByVal settingRowSerializer As M_SETTINGRowSerializer, _
                                  ByVal terrestrialDevice As M_DEVICEDataTable, _
                                  ByVal bS110CSDevice As M_DEVICEDataTable) As Result Implements IRecordingReservationManager.UpdateSetting
        With Entity
            .BeginTran()
            Try
                .DeleteSetting()
                .InsertSetting(settingRowSerializer.SettingRow)
                .DeleteDevice()
                For Each terrestrialDeviceRow As SchemaRecordingReservation.M_DEVICERow In terrestrialDevice
                    .InsertDevice(BROADCAST_TYPE.TERRESTRIAL, terrestrialDeviceRow.DLL_NAME, terrestrialDeviceRow.PRIORITY)
                Next
                For Each bs110csDeviceRow As SchemaRecordingReservation.M_DEVICERow In bS110CSDevice
                    .InsertDevice(BROADCAST_TYPE.BS110CS, bs110csDeviceRow.DLL_NAME, bs110csDeviceRow.PRIORITY)
                Next
            Finally
                .Commit()
            End Try
        End With
        Return New Result(RESULT_VALUE.SUCCESS, "")
    End Function
    ''' <summary>
    ''' DLL名取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function GetDllNames(ByVal tvTestFileFullPath As String) As String() Implements IRecordingReservationManager.GetDllNames
        Return Me.Entity.GetDllFiles(tvTestFileFullPath)
    End Function
    ''' <summary>
    ''' 設定情報ダイアログを起動
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub ShowSettingDialog()
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
                settingProcess.EnableRaisingEvents = True
                AddHandler settingProcess.Exited, AddressOf Me.GetSetting
            End If
        Catch ex As System.ComponentModel.Win32Exception
            '//NOP
        Catch
            Throw
        End Try
    End Sub
    ''' <summary>
    ''' 管理者権限かどうかを取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Overridable Function IsAdmin() As Boolean
        Dim usrId As WindowsIdentity = WindowsIdentity.GetCurrent()
        Dim principal As WindowsPrincipal = New WindowsPrincipal(usrId)
        Return principal.IsInRole("BUILTIN\Administrators")
    End Function
    ''' <summary>
    ''' 終了処理
    ''' </summary>
    ''' <remarks></remarks>
    Friend Sub ExitApplication()
        Me.Entity.BeginTran()
        Try
            Me.Entity.InsertLog()
            Me.Entity.UpdateReservationRecorded()
        Finally
            Me.Entity.Commit()
        End Try
        System.Windows.Forms.Application.Exit()
    End Sub
#End Region

#Region " 内部メソッド "
    ''' <summary>
    ''' ログ削除
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overridable Function DeleteLog(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow) As Boolean
        With Entity
            .BeginTran()
            Try
                .DeleteLog(reservationRow.ID)
            Finally
                .Commit()
            End Try
        End With
        Return True
    End Function
    ''' <summary>
    ''' チャンネル情報更新
    ''' </summary>
    ''' <param name="stationRow"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub InsertStation(ByVal stationRow As SchemaRecordingReservation.M_STATIONRow)
        With Entity
            .BeginTran()
            Try
                .InsertStation(stationRow)
            Finally
                .Commit()
            End Try
        End With
    End Sub
    ''' <summary>
    ''' ログ出力処理
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <param name="message"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub WriteLog(ByVal pEntity As EntityRecordingReservation, ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow, ByVal message As String)
        pEntity.InsertLog(reservationRow, reservationRow.PROGRAM_TITLE & " " & DateTime.Now.ToString(DATE_DISP_FORMAT) & ": " & message)
    End Sub
    ''' <summary>
    ''' 予約チェック
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overridable Function CheckRecordingReservation(ByVal reservationRow As T_RESERVATIONRow) As Result
        Dim resultValue As Result
        '★過去の予約でないかチェック
        resultValue = Me.CheckIsPastReservation(reservationRow)
        If resultValue.ResultValue <> Result.RESULT_VALUE.SUCCESS Then
            Return resultValue
        End If
        '★チャンネル登録チェック
        resultValue = Me.CheckRegistChannel(reservationRow)
        If resultValue.ResultValue <> Result.RESULT_VALUE.SUCCESS Then
            Return resultValue
        End If
        '★重複予約がないかチェック
        resultValue = Me.CheckDuplicateReservation(reservationRow)
        If resultValue.ResultValue <> Result.RESULT_VALUE.SUCCESS Then
            Return resultValue
        End If
        Return New Result(Result.RESULT_VALUE.SUCCESS, String.Empty)
    End Function
    ''' <summary>
    ''' 過去の予約かチェック
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overridable Function CheckIsPastReservation(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow) As Result
        If DateTime.Now > DateTime.ParseExact(reservationRow.END_YYYYMMDDHHMM, DATE_FORMAT, Nothing) Then
            Return New Result(RESULT_VALUE.ERROR, "終了した番組です。予約できません。")
        End If
        Return New Result(RESULT_VALUE.SUCCESS, String.Empty)
    End Function
    ''' <summary>
    ''' チャンネル登録チェック
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overridable Function CheckRegistChannel(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow) As Result
        Dim stationTable As SchemaRecordingReservation.M_STATIONDataTable = Me.Entity.GetChannel(reservationRow)
        If stationTable.Count <= 0 Then
            Return New Result(RESULT_VALUE.UNREGIST_CHANNEL_ERROR, "[" & reservationRow.STATION & ":" & reservationRow.STATION_NAME & "]はチャンネルマスタに登録されていません。" _
                            & System.Environment.NewLine & "チャンネルを設定してください。")
        End If
        Return New Result(RESULT_VALUE.SUCCESS, String.Empty)
    End Function
    ''' <summary>
    ''' チャンネル取得
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <remarks></remarks>
    Protected Overridable Function SetChannel(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow) As Result
        Dim stationTable As SchemaRecordingReservation.M_STATIONDataTable = Me.Entity.GetChannel(reservationRow)
        If stationTable.Count <= 0 Then
            Return New Result(RESULT_VALUE.UNREGIST_CHANNEL_ERROR, "[" & reservationRow.STATION & ":" & reservationRow.STATION_NAME & "]はチャンネルマスタに登録されていません。" _
                            & System.Environment.NewLine & "チャンネルを設定してください。")
        End If
        reservationRow.CHANNEL = stationTable(0).CHANNEL
        If stationTable(0).IsSERVICE_IDNull Then reservationRow.SetSERVICE_IDNull() Else reservationRow.SERVICE_ID = stationTable(0).SERVICE_ID
        If reservationRow.SERVICE_ID < 1000 Then
            reservationRow.BROADCAST_TYPE = BROADCAST_TYPE.BS110CS
        Else
            reservationRow.BROADCAST_TYPE = BROADCAST_TYPE.TERRESTRIAL
        End If
        Return New Result(RESULT_VALUE.SUCCESS, String.Empty)
    End Function
    ''' <summary>
    ''' 重複予約チェック
    ''' </summary>
    ''' <param name="ReservationRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overridable Function CheckDuplicateReservation(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow) As Result
        Me.SetChannel(reservationRow)
        Dim duplicateReservationTbl As SchemaRecordingReservation.T_RESERVATIONDataTable = Entity.GetDuplicateReservation(reservationRow)
        Dim deviceList As SchemaRecordingReservation.M_DEVICEDataTable = Me.GetDevice(reservationRow)
        If Me.IsDuplicateReservation(duplicateReservationTbl, deviceList) Then
            Dim ErrMsgBuilder As New System.Text.StringBuilder
            With ErrMsgBuilder
                .AppendLine("予約が重複しています。")
                .AppendLine("　重複している予約")
                For Each duplicateReservationRow As SchemaRecordingReservation.T_RESERVATIONRow In duplicateReservationTbl
                    Dim startDate As DateTime = DateTime.ParseExact(duplicateReservationRow.START_YYYYMMDDHHMM, DATE_FORMAT, Nothing)
                    Dim endDate As DateTime = DateTime.ParseExact(duplicateReservationRow.END_YYYYMMDDHHMM, DATE_FORMAT, Nothing)
                    .Append("　　")
                    .Append(startDate.ToString(DATE_DISP_FORMAT))
                    .Append(" - ")
                    .Append(endDate.ToString("HH:mm"))
                    .Append(" ")
                    .AppendLine(duplicateReservationRow.PROGRAM_TITLE)
                Next
                .AppendLine("予約しますか？")
            End With
            Return New Result(RESULT_VALUE.DUPLICATE_PROGRAM_WARNING, ErrMsgBuilder.ToString())
        Else
            For Each duplicateReservationRow As SchemaRecordingReservation.T_RESERVATIONRow In duplicateReservationTbl
                If reservationRow.CHANNEL.Equals(duplicateReservationRow.CHANNEL) _
                AndAlso reservationRow.START_YYYYMMDDHHMM.Equals(duplicateReservationRow.START_YYYYMMDDHHMM) _
                AndAlso reservationRow.END_YYYYMMDDHHMM.Equals(duplicateReservationRow.END_YYYYMMDDHHMM) Then
                    Return New Result(RESULT_VALUE.ALREADY_REGIST_RESERVATION, "既に予約されています。")
                End If
            Next
            Dim recordingDeviceTable As SchemaRecordingReservation.M_DEVICEDataTable = Me.Entity.GetRecordingDevice(reservationRow)
            If recordingDeviceTable.Count <= 0 Then
                Return New Result(RESULT_VALUE.NOT_EXISTS_DEVICE_ERROR, "録画するデバイスが取得できません。")
            End If
        End If
        Return New Result(RESULT_VALUE.SUCCESS, String.Empty)
    End Function
    ''' <summary>
    ''' デバイスの取得
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overridable Function GetDevice(ByVal reservationRow As T_RESERVATIONRow) As M_DEVICEDataTable
        Select Case reservationRow.BROADCAST_TYPE
            Case BROADCAST_TYPE.TERRESTRIAL
                Return Me.Entity.GetDevice(BROADCAST_TYPE.TERRESTRIAL)
            Case BROADCAST_TYPE.BS110CS
                Return Me.Entity.GetDevice(BROADCAST_TYPE.BS110CS)
            Case Else
                Return New SchemaRecordingReservation.M_DEVICEDataTable
        End Select
    End Function
    ''' <summary>
    ''' 予約重複チェック
    ''' </summary>
    ''' <param name="duplicateReservationTbl"></param>
    ''' <param name="deviceList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overridable Function IsDuplicateReservation(ByVal duplicateReservationTbl As T_RESERVATIONDataTable, ByVal deviceList As M_DEVICEDataTable) As Boolean
        If duplicateReservationTbl.Count >= deviceList.Count Then
            '時間がかぶっている予約すべてに対して、重複している予約数とデバイスの数を確認
            For Each duplicateReservationRow As SchemaRecordingReservation.T_RESERVATIONRow In duplicateReservationTbl
                If Entity.GetDuplicateReservation(duplicateReservationRow).Count >= deviceList.Count Then
                    Return True
                End If
            Next
        End If
        Return False
    End Function

    ''' <summary>
    ''' デバイスの設定
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overridable Function SetDevice(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow) As Result
        Dim duplicateReservationTbl As SchemaRecordingReservation.T_RESERVATIONDataTable = Entity.GetDuplicateReservation(reservationRow)
        Dim deviceList As SchemaRecordingReservation.M_DEVICEDataTable = Me.GetDevice(reservationRow)
        If Me.IsDuplicateReservation(duplicateReservationTbl, deviceList) Then
            reservationRow.DLL_NAME = deviceList(0).DLL_NAME
        Else
            Dim recordingDeviceTable As SchemaRecordingReservation.M_DEVICEDataTable = Me.Entity.GetRecordingDevice(reservationRow)
            If recordingDeviceTable.Count <= 0 Then
                Return New Result(RESULT_VALUE.NOT_EXISTS_DEVICE_ERROR, "録画するデバイスが取得できません。")
            Else
                reservationRow.DLL_NAME = recordingDeviceTable(0).DLL_NAME
            End If
        End If
        Return New Result(RESULT_VALUE.SUCCESS, String.Empty)
    End Function
    ''' <summary>
    ''' チャンネルタイプを設定
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub SetChannelType(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow)
        Dim stationRow As SchemaRecordingReservation.M_STATIONRow = Me.Entity.GetChannel(reservationRow)(0)
        reservationRow.CHANNEL = stationRow.CHANNEL
        If stationRow.IsSERVICE_IDNull Then reservationRow.SetSERVICE_IDNull() Else reservationRow.SERVICE_ID = stationRow.SERVICE_ID
        If reservationRow.SERVICE_ID < 1000 Then
            reservationRow.BROADCAST_TYPE = BROADCAST_TYPE.BS110CS
        Else
            reservationRow.BROADCAST_TYPE = BROADCAST_TYPE.TERRESTRIAL
        End If
    End Sub
    ''' <summary>
    ''' 設定情報取得
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub GetSetting(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim settingProcess As System.Diagnostics.Process = DirectCast(sender, System.Diagnostics.Process)
        RemoveHandler settingProcess.Exited, AddressOf Me.GetSetting
        If settingProcess IsNot Nothing Then
            settingProcess.Dispose()
        End If
    End Sub
    '''' <summary>
    '''' 録画中またはこれから録画する予約が存在しないかチェック
    '''' </summary>
    '''' <returns>存在する場合：False,存在しない場合：True</returns>
    '''' <remarks></remarks>
    'Protected Overridable Function CheckNotExistRecordingReservation() As Boolean
    '    '★録画プロセスが存在しない場合
    '    Dim settingRow As SchemaRecordingReservation.M_SETTINGRow = Me.GetSetting()(0)
    '    If (0).Equals(Me.GetProcesses(settingRow.FRIIOVIEW_FULL_PATH).Length) _
    '    AndAlso (0).Equals(Me.GetProcesses(settingRow.TVTEST_FULL_PATH).Length) Then
    '        '★５分以内に開始番組が存在しない場合
    '        If (0).Equals(Me.Entity.GetStartReservation(settingRow.CHECK_START_INTERVAL_MINUTES).Count) Then
    '            Return True
    '        End If
    '    End If
    '    Return False
    'End Function
    ''' <summary>
    ''' 録画スレッド開始処理
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub StartRecordingThread(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow)
        Dim recordingThread As New RecordingDelegate(AddressOf Recording)
        recordingThread.BeginInvoke(reservationRow, AddressOf EndRecordingThread, recordingThread)
    End Sub
    ''' <summary>
    ''' 録画処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Function Recording(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow) As SchemaRecordingReservation.T_RESERVATIONRow
        Dim TimerHandle As System.IntPtr = System.IntPtr.Zero
        Using pEntity As New EntityRecordingReservation
            Try
                Dim settingRow As SchemaRecordingReservation.M_SETTINGRow = pEntity.GetSetting()(0)
                Me.WriteLog(pEntity, reservationRow, "録画予約スレッド開始")
                '★録画スレッドディクショナリにスレッドを登録
                SyncLock LockRecordingThreadDictionary
                    '★録画スレッドディクショナリに同じＩＤが存在する場合は無くなるまで待機
                    Do Until Not RecordingReservationManager.RecordingThreadDictionary.ContainsKey(reservationRow.ID) : Loop
                    RecordingReservationManager.RecordingThreadDictionary.Add(reservationRow.ID, Thread.CurrentThread)
                End SyncLock
                SyncLock LockRecordingReservationDictionary
                    Do Until Not RecordingReservationManager.RecordingReservationDictionary.ContainsKey(reservationRow.ID) : Loop
                    RecordingReservationManager.RecordingReservationDictionary.Add(reservationRow.ID, reservationRow)
                End SyncLock
                Dim endDate As DateTime = DateTime.ParseExact(reservationRow.END_YYYYMMDDHHMM, DATE_FORMAT, Nothing)
                If DateTime.Now > endDate Then
                    Me.WriteLog(pEntity, reservationRow, "番組が終了していた為録画できませんでした。")
                End If
                Dim startDate As DateTime = DateTime.ParseExact(reservationRow.START_YYYYMMDDHHMM, DATE_FORMAT, Nothing)
                'Dim execDate As DateTime = startDate _
                '    .AddSeconds(-1 * settingRow.POWER_ON_INTERVAL_SECONDS) _
                '    .AddSeconds(-1 * settingRow.EXE_BOOT_INTERVAL_SECONDS) _
                '    .AddSeconds(-1 * settingRow.START_INTERVAL_SECONDS)
                Dim nowDate As DateTime = DateTime.Now
                Dim execDate As DateTime = nowDate.AddMilliseconds(startDate.Subtract(nowDate).TotalMilliseconds _
                            - ((settingRow.START_INTERVAL_SECONDS + settingRow.EXE_BOOT_INTERVAL_SECONDS + 30) * 1000))
                If DateTime.Now >= execDate Then
                    '★開始時間を既に過ぎている場合は録画処理開始
                    Me.ExecuteRecorder(pEntity, reservationRow, settingRow, True)
                Else
                    '★タイマーを作成
                    TimerHandle = CreateWaitableTimer(IntPtr.Zero, False, Guid.NewGuid().ToString())
                    '★録画開始までのタイマーをセット
                    Me.WriteLog(pEntity, reservationRow, "録画予約待機開始")
                    SetWaitableTimer(TimerHandle, execDate.ToFileTimeUtc, 0, Nothing, IntPtr.Zero, True)
                    '★録画開始まで待機
                    Do Until (0).Equals(WaitForSingleObject(TimerHandle, 50)) OrElse DateTime.Now >= execDate : Loop
                    Me.WriteLog(pEntity, reservationRow, "録画予約待機終了")
                    '★録画処理
                    Me.ExecuteRecorder(pEntity, reservationRow, settingRow, False)
                End If
                Return reservationRow
            Catch taex As System.Threading.ThreadAbortException
                '★録画開始時間なのでタイマークリア
                If Not IntPtr.Zero.Equals(TimerHandle) Then
                    CancelWaitableTimer(TimerHandle)
                End If
                '★ログ出力
                Me.WriteLog(pEntity, reservationRow, "録画予約スレッドキャンセル")
                Throw taex
            Finally
                '★録画スレッドディクショナリからスレッドを削除
                SyncLock LockRecordingThreadDictionary
                    If RecordingReservationManager.RecordingThreadDictionary.ContainsKey(reservationRow.ID) Then
                        RecordingReservationManager.RecordingThreadDictionary.Remove(reservationRow.ID)
                    End If
                End SyncLock
                SyncLock LockRecordingReservationDictionary
                    If RecordingReservationManager.RecordingReservationDictionary.ContainsKey(reservationRow.ID) Then
                        RecordingReservationManager.RecordingReservationDictionary.Remove(reservationRow.ID)
                    End If
                End SyncLock
                If Not IntPtr.Zero.Equals(TimerHandle) Then
                    CloseHandle(TimerHandle)
                End If
            End Try
        End Using
    End Function
    ''' <summary>
    ''' スレッド終了後処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Sub EndRecordingThread(ByVal ar As System.IAsyncResult)
        Try
            '★スレッドで実行されていたデリゲートの取得
            Dim pRecordingDelegate As RecordingDelegate = DirectCast(ar.AsyncState, RecordingDelegate)
            '★デリゲートの返却値の取得
            Dim reservationRow As SchemaRecordingReservation.T_RESERVATIONRow = pRecordingDelegate.EndInvoke(ar)
            '★ログ出力
            Using pEntity As New EntityRecordingReservation
                Me.WriteLog(pEntity, reservationRow, "録画予約スレッド終了")
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    ''' <summary>
    ''' レコーディングスレッドキャンセル
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub CancelRecordingThread(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow)
        SyncLock LockRecordingThreadDictionary
            If RecordingReservationManager.RecordingThreadDictionary.ContainsKey(reservationRow.ID) Then
                RecordingReservationManager.RecordingThreadDictionary(reservationRow.ID).Abort()
                RecordingReservationManager.RecordingThreadDictionary.Remove(reservationRow.ID)
            End If
        End SyncLock
        SyncLock LockRecordingReservationDictionary
            If RecordingReservationManager.RecordingReservationDictionary.ContainsKey(reservationRow.ID) Then
                RecordingReservationManager.RecordingReservationDictionary.Remove(reservationRow.ID)
            End If
        End SyncLock
    End Sub
    ''' <summary>
    ''' 録画処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Sub ExecuteRecorder(ByVal pEntity As EntityRecordingReservation, ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow, _
                                              ByVal settingRow As SchemaRecordingReservation.M_SETTINGRow, ByVal isAsSoonAsPossible As Boolean)
        Dim fileName As String = String.Empty
        Dim recordingReservationEventArgs As New RecordingReservationEventArgs(reservationRow)
        Try
            '★録画即時開始でない場合
            If Not isAsSoonAsPossible Then
                '★ディスプレイがオンになるようにする
                SetThreadExecutionState(ES_SYSTEM_REQUIRED Or ES_DISPLAY_REQUIRED Or ES_CONTINUOUS)
                System.Threading.Thread.Sleep(Convert.ToInt32(settingRow.EXE_BOOT_INTERVAL_SECONDS * 1000L))
            End If
            '★同じデバイスを使用しているプロセスが存在するか取得
            Dim sameDeviceProcess As Process = Me.GetProcess(reservationRow.DLL_NAME)
            Dim startDateTime As DateTime = DateTime.ParseExact(reservationRow.START_YYYYMMDDHHMM, DATE_FORMAT, Nothing)
            '★プロセス実行後待機時間
            Dim delaySeconds As System.Int64 = 0L
            If isAsSoonAsPossible OrElse sameDeviceProcess IsNot Nothing Then
                '★プロセス実行後待機時間の算出
                delaySeconds = DELAY_SECONDS
                '★即時開始または同じデバイスを使用しているプロセスが存在する場合、録画開始22秒前までスリープ
                Dim waitTime As System.Int32 = Convert.ToInt32(startDateTime.AddSeconds(-1 * delaySeconds) _
                                                    .AddSeconds(-8).Subtract(DateTime.Now).TotalMilliseconds)
                If waitTime > 0 Then
                    System.Threading.Thread.Sleep(waitTime)
                End If
                '★同じデバイスを使用しているプロセスをクローズ（直前でもう一度取り直す）
                Me.CloseProcess(Me.GetProcess(reservationRow.DLL_NAME))
                Me.RemoveProcessID(reservationRow.DLL_NAME)
                '★プロセスをクローズ後、2秒間停止
                System.Threading.Thread.Sleep(2 * 1000)
            Else
                '★プロセス実行後待機時間の算出
                delaySeconds = settingRow.START_INTERVAL_SECONDS
                '★録画開始までスリープ
                Dim waitTime As System.Int32 = Convert.ToInt32(startDateTime.AddSeconds(-1 * delaySeconds) _
                                                    .AddSeconds(-8).Subtract(DateTime.Now).TotalMilliseconds)
                If waitTime > 0 Then
                    System.Threading.Thread.Sleep(waitTime)
                End If
            End If
            '★ファイル名を生成
            fileName = Me.MakeFileName(reservationRow)
            '★このEXEで管理されていないプロセスが存在する場合はプロセスをクローズする。
            '★中止　理由：プロセス開始後、ディクショナリ登録前の場合にプロセスを停止してしまう可能性がある為。
            'Me.CloseProcess(settingRow)
            '★同名のＴＳファイルが存在する場合、リネームする。
            fileName = Me.RenameFile(System.IO.Path.Combine(settingRow.SAVE_PATH, fileName))
            '★録画実行デバイスの取得
            Dim deviceInfo As SchemaRecordingReservation.M_DEVICERow = pEntity.GetDevice(reservationRow.DLL_NAME)
            '★プロセスの実行
            Me.WriteLog(pEntity, reservationRow, "録画開始")
            'WriteEventLog(Me.GetExecuteFileName(deviceInfo, settingRow))
            Using Process As System.Diagnostics.Process _
            = System.Diagnostics.Process.Start(settingRow.TVTEST_FULL_PATH, Me.MakeParam(deviceInfo, reservationRow, settingRow, fileName, delaySeconds))
                'WriteEventLog("System.Environment.UserName = " & System.Environment.UserDomainName & "\" & System.Environment.UserName)
                'WriteEventLog("Threading.Thread.CurrentPrincipal.Identity.Name = " & Threading.Thread.CurrentPrincipal.Identity.Name)
                'WriteEventLog("WindowsIdentity.GetCurrent(True).Name = " & WindowsIdentity.GetCurrent(True).Name)
                'WriteEventLog("WindowsIdentity.GetCurrent(False).Name = " & WindowsIdentity.GetCurrent(False).Name)
                'WriteEventLog("Process.StartInfo.UserName = " & Process.StartInfo.UserName)
                'Dim dupedToken As IntPtr
                'Dim processInfo As PROCESS_INFORMATION _
                '= Me.Execute(dupedToken, settingRow.TVTEST_FULL_PATH, _
                '             Me.MakeParam(deviceInfo, reservationRow, settingRow, fileName, delaySeconds))
                '★プロセスＩＤリストに現在のプロセスＩＤを設定
                Me.SetProcessID(reservationRow.DLL_NAME, CType(Process.Id, Integer))
                '★録画保存パスの取得および更新
                reservationRow.PATH = System.IO.Path.Combine(settingRow.SAVE_PATH, fileName)
                pEntity.BeginTran()
                Try
                    pEntity.UpdateReservationRecording(reservationRow)
                Finally
                    pEntity.Commit()
                End Try
                '★バルーンの表示
                RaiseEvent StartRecording(Me, recordingReservationEventArgs)
                '★ダミーファイルの作成および録画開始の失敗がある場合の強制実行の為に録画開始時間後３秒までスリープ
                System.Threading.Thread.Sleep(Convert.ToInt32(delaySeconds) * 1000 + 3000)
                '★ダミーファイルの削除と作成
                'System.IO.File.Delete(System.IO.Path.Combine(settingRow.SAVE_PATH, "RecordingReservationManager.dmy"))
                'System.IO.File.WriteAllText(System.IO.Path.Combine(settingRow.SAVE_PATH, "RecordingReservationManager.dmy"), DateTime.Now.ToString(DATE_DISP_FORMAT))
                '★プロセス終了まで待機
                Process.WaitForExit()
                'WaitForSingleObject(processInfo.hProcess, INFINITE)
                'Dim ret As Boolean
                'ret = CloseHandle(processInfo.hProcess)
                'If (Not ret) Then
                '    Throw New Exception("プロセスハンドルのクローズに失敗しました。")
                'End If
                'ret = CloseHandle(processInfo.hThread)
                'If (Not ret) Then
                '    Throw New Exception("スレッドハンドルのクローズに失敗しました。")
                'End If
                'ret = CloseHandle(dupedToken)
                'If (Not ret) Then
                '    Throw New Exception("トークンハンドルのクローズに失敗しました。")
                'End If
                '★プロセスＩＤリストから現在のプロセスＩＤを消去
                Me.RemoveProcessID(reservationRow.DLL_NAME)
            End Using
        Catch ex As System.IO.FileNotFoundException
            '★ログ出力
            Me.WriteLog(pEntity, reservationRow, "保存先フォルダが存在しません。録画に失敗しました。")
        Catch ex As Exception
            '★ログ出力
            Me.WriteLog(pEntity, reservationRow, ex.Message & System.Environment.NewLine & "録画に失敗しました。")
            'WriteEventLog(ex.Message & System.Environment.NewLine & ex.StackTrace & System.Environment.NewLine & ex.Source)
        Finally
            RaiseEvent EndRecording(Me, recordingReservationEventArgs)
            '★予約情報更新処理
            With pEntity
                .BeginTran()
                Try
                    .UpdateReservationRecorded(reservationRow)
                Finally
                    .Commit()
                End Try
            End With
            Me.WriteLog(pEntity, reservationRow, "録画終了")
        End Try
    End Sub
    ''' <summary>
    ''' 保存ファイル名称生成
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overridable Function MakeFileName(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow) As String
        Dim fileNameBuilder As New System.Text.StringBuilder
        With fileNameBuilder
            .Append(reservationRow.PROGRAM_TITLE)
            If Not String.IsNullOrEmpty(reservationRow.PROGRAM_SUBTITLE) Then .Append("：") : .Append(reservationRow.PROGRAM_SUBTITLE)
            .Append("：")
            .Append(reservationRow.START_YYYYMMDDHHMM)
            .Append("～")
            .Append(reservationRow.END_YYYYMMDDHHMM)
            .Append(".ts")
        End With
        Return fileNameBuilder.ToString
    End Function
    ''' <summary>
    ''' プロセス起動パラメータ作成
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <param name="fileName"></param>
    ''' <param name="delaySeconds"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overridable Function MakeParam(ByVal deviceInfo As SchemaRecordingReservation.M_DEVICERow, ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow, _
                                                ByVal settingRow As SchemaRecordingReservation.M_SETTINGRow, ByVal fileName As String, ByVal delaySeconds As System.Int64) As String
        '★パラメータの作成を行う。
        Dim recordingMinutes As String = String.Empty
        Dim paramBuilder As New System.Text.StringBuilder
        With paramBuilder
            recordingMinutes = Convert.ToInt32(ToRoundUp(DateTime.ParseExact(reservationRow.END_YYYYMMDDHHMM, DATE_FORMAT, Nothing).Subtract(DateTime.Now.AddSeconds(delaySeconds)).TotalSeconds, 0)).ToString()
            .Append(" /tsid ") : .Append(reservationRow.CHANNEL.ToString)
            .Append(" /sid ") : .Append(reservationRow.SERVICE_ID.ToString)
            'If Not reservationRow.IsSERVICE_IDNull Then .Append(" /sid ") : .Append(reservationRow.SERVICE_ID.ToString)
            .Append(" /log")
            If (0L).Equals(reservationRow.PREVIEW) Then .Append(" /min /nodshow /noview")
            .Append(" /rec")
            .Append(" /reccurservice ")
            .Append(" /recdelay ") : .Append(delaySeconds)
            .Append(" /recduration ") : .Append(recordingMinutes)
            If (1L).Equals(reservationRow.EXIT_APPLICATION_AFTER_RECORDING) Then .Append(" /recexit")
            .Append(" /recfile """) : .Append(System.IO.Path.Combine(settingRow.SAVE_PATH, fileName)) : .Append("""")
            .Append(" /d ") : .Append(deviceInfo.DLL_NAME)
            If (1L).Equals(reservationRow.OUTPUT_ERRORLOG) Then .Append(" /log")
        End With
        Return paramBuilder.ToString
    End Function

    Protected Function Execute(ByRef dupedToken As IntPtr, ByVal exeFullPath As String, ByVal parameter As String) As PROCESS_INFORMATION
        Dim Token As New IntPtr(0)
        dupedToken = New IntPtr(0)
        Dim ret As Boolean
        Dim sa As New SECURITY_ATTRIBUTES()
        sa.bInheritHandle = False
        sa.Length = Marshal.SizeOf(sa)
        sa.lpSecurityDescriptor = New IntPtr(0)
        Token = WindowsIdentity.GetCurrent(True).Token
        Const GENERIC_ALL As UInteger = &H10000000
        Const SecurityImpersonation As Integer = 2
        Const TokenType As Integer = 1
        ret = DuplicateTokenEx(Token, GENERIC_ALL, sa, SecurityImpersonation, TokenType, dupedToken)
        If (Not ret) Then
            Throw New Exception("トークンの取得に失敗しました。")
        End If
        Dim si As New STARTUPINFO()
        si.cb = Marshal.SizeOf(si)
        si.lpDesktop = ""
        Dim pi As New PROCESS_INFORMATION()
        ret = CreateProcessAsUser(dupedToken, Nothing, exeFullPath & " " & parameter, sa, sa, False, _
         0, New IntPtr(0), System.IO.Path.GetDirectoryName(exeFullPath), si, pi)
        If (Not ret) Then
            Throw New Exception("プロセスの実行に失敗しました。")
        End If
        Return pi
    End Function
    ''' <summary>
    ''' プロセス存在チェック
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Function GetProcesses(ByVal fullPath As String) As System.Diagnostics.Process()
        Return System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(fullPath))
    End Function
    ''' <summary>
    ''' プロセス存在チェック
    ''' </summary>
    ''' <remarks></remarks>
    Protected Function GetProcess(ByVal device As String) As System.Diagnostics.Process
        SyncLock LockProcessIDDictionary
            If RecordingReservationManager.ProcessIDDictionary.ContainsKey(device) Then
                Try
                    Dim deviceProcess As System.Diagnostics.Process = System.Diagnostics.Process.GetProcessById(RecordingReservationManager.ProcessIDDictionary(device))
                    Return deviceProcess
                Catch ex As Exception
                    Return Nothing
                End Try
            Else
                Return Nothing
            End If
        End SyncLock
    End Function
    ''' <summary>
    ''' プロセスクローズ
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Sub CloseProcess(ByVal process As System.Diagnostics.Process)
        'If (process IsNot Nothing) _
        'AndAlso (Not IntPtr.Zero.Equals(process.MainWindowHandle)) Then
        If (process IsNot Nothing) Then
            ''★FriioViewの録画停止ボタンのハンドルをキャプションで取得
            'Dim buttonHandle As IntPtr = FindWindowEx(process.MainWindowHandle, 0, Nothing, "&Stop")
            ''★録画が停止されている場合は録画ボタンのキャプションが"●REC(&R)"に変更される為、ボタンのハンドルが取得できない。
            'If Not IntPtr.Zero.Equals(buttonHandle) Then
            '    '★録画が停止されていない場合、録画停止ボタンを押下するメッセージを送信
            '    PostMessage(process.MainWindowHandle, WM_COMMAND, BN_CLICKED + (GetDlgCtrlID(buttonHandle) And &HFFFF), buttonHandle.ToInt32)
            'End If
            'process.Kill()
            process.CloseMainWindow()
            process.Close()
            process.Dispose()
        End If
    End Sub
    ''' <summary>
    ''' プロセスクローズ
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overridable Sub CloseProcess(ByVal settingRow As SchemaRecordingReservation.M_SETTINGRow)
        SyncLock LockProcessIDDictionary
            For Each process As System.Diagnostics.Process In System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(settingRow.TVTEST_FULL_PATH))
                If (Not RecordingReservationManager.ProcessIDDictionary.ContainsValue(process.Id)) AndAlso (Not IntPtr.Zero.Equals(process.MainWindowHandle)) _
                AndAlso process.Modules(0).FileName = settingRow.TVTEST_FULL_PATH Then
                    Me.CloseProcess(process)
                End If
            Next
        End SyncLock
    End Sub
    ''' <summary>
    ''' プロセスＩＤの取得
    ''' </summary>
    ''' <param name="device"></param>
    ''' <param name="processID"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub SetProcessID(ByVal device As String, ByVal processID As Integer)
        SyncLock LockProcessIDDictionary
            If Not RecordingReservationManager.ProcessIDDictionary.ContainsKey(device) Then
                RecordingReservationManager.ProcessIDDictionary.Add(device, processID)
            End If
        End SyncLock
    End Sub
    ''' <summary>
    ''' プロセスＩＤの除去
    ''' </summary>
    ''' <param name="device"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub RemoveProcessID(ByVal device As String)
        SyncLock LockProcessIDDictionary
            If RecordingReservationManager.ProcessIDDictionary.ContainsKey(device) Then
                RecordingReservationManager.ProcessIDDictionary.Remove(device)
            End If
        End SyncLock
    End Sub
    ''' <summary>
    ''' ファイルが存在する場合はこれから録画するファイルのリネームを行う。
    ''' </summary>
    ''' <param name="fullPath">リネームしたいファイルのフルパス</param>
    ''' <remarks></remarks>
    Protected Overridable Function RenameFile(ByVal fullPath As String) As String
        If System.IO.File.Exists(fullPath) Then
            Dim i As System.Int32 = 1
            Do While System.IO.File.Exists(fullPath.Replace(".ts", "_" & i.ToString & ".ts"))
                i += 1
            Loop
            Return System.IO.Path.GetFileName(fullPath.Replace(".ts", "_" & i.ToString & ".ts"))
        Else
            Return System.IO.Path.GetFileName(fullPath)
        End If
    End Function

    Protected Overloads Function GenerateReservationEnumerableList(ByVal reservationTable As T_RESERVATIONDataTable) As IEnumerable(Of T_RESERVATIONRowSerializer)
        Dim reservationEnumerableList As List(Of T_RESERVATIONRowSerializer) = New List(Of T_RESERVATIONRowSerializer)()
        For Each reservationRow As T_RESERVATIONRow In reservationTable
            reservationEnumerableList.Add(New T_RESERVATIONRowSerializer(reservationRow))
        Next
        Return reservationEnumerableList
    End Function

    '''' <summary>
    '''' ウィンドウメッセージ
    '''' </summary>
    '''' <param name="m"></param>
    '''' <remarks></remarks>
    'Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
    '    If WM_POWERBROADCAST.Equals(m.Msg) Then
    '        Select Case m.WParam
    '            Case PBT_APMQUERYSUSPEND
    '                '★サスペンドに移行する場合
    '                If Not CheckNotExistRecordingReservation() Then
    '                    '★録画中の予約または５分以内に予約が存在する場合
    '                    m.Result = New IntPtr(BROADCAST_QUERY_DENY)
    '                    MessageBox.Show("番組予約があるため電源を切ることができません。" _
    '                                  , MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1, 0, False)
    '                    Return
    '                End If
    '            Case Else
    '        End Select
    '    End If
    '    MyBase.WndProc(m)
    'End Sub
#End Region

End Class
