Imports System.ServiceModel
Imports RK.EntityRecordingReservation
Imports RK.SchemaRecordingReservation
Imports RK.RecordingReservationManager
Imports RK.Result

''' <summary>
''' メインフォーム
''' </summary>
''' <remarks></remarks>
Public Class FormMain

#Region " 変数・定数 "
    ''' <summary>サービスホスト</summary>
    Protected RecordingServiceManagerServer As ServiceHost
    ''' <summary></summary>
    Private WithEvents _RecordingReservationManagerInstance As RecordingReservationManager
#End Region

#Region " プロパティ "
    Friend WriteOnly Property RecordingReservationManagerInstance() As RecordingReservationManager
        Set(ByVal value As RecordingReservationManager)
            Me._RecordingReservationManagerInstance = value
        End Set
    End Property
#End Region

#Region " コンストラクタ "
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        ' この呼び出しは、Windows フォーム デザイナで必要です。
        InitializeComponent()
        ' InitializeComponent() 呼び出しの後で初期化を追加します。
        Me.Visible = False
        'Me.RecordingReservationManagerInstance = RecordingReservationManager.Instance
    End Sub
#End Region

#Region "イベント"

#Region "フォーム"
    '''' <summary>
    '''' オンロード
    '''' </summary>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        Me.Visible = False
        MyBase.OnLoad(e)
        Me.RecordingServiceManagerServer = New ServiceHost(GetType(RecordingReservationManager))
        Me.RecordingServiceManagerServer.Open()
        If Me._RecordingReservationManagerInstance.IsAdmin Then
            Me.SettingToolStripMenuItem.Image = Nothing
        Else
            Me.SettingToolStripMenuItem.Image = System.Drawing.SystemIcons.Shield.ToBitmap()
        End If
    End Sub
#End Region

#Region "タスクトレイアイコン"
    ''' <summary>
    ''' 設定ダイアログ表示
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub SettingToolStripMenuItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SettingToolStripMenuItem.Click
        Dim reservationContextMenuStrip As System.Windows.Forms.ContextMenuStrip = ClearContextMenuStrip()
        Try
            _RecordingReservationManagerInstance.ShowSettingDialog()
        Finally
            Me.AddReservationContextMenu(reservationContextMenuStrip)
        End Try
    End Sub
    ''' <summary>
    ''' アプリケーションを終了
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub FinishToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FinishToolStripMenuItem.Click
        If System.Windows.Forms.DialogResult.OK.Equals(MessageBox.Show("録画予約マネージャを終了しますか？", MESSAGEBOX_TITLE, MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2, 0, False)) Then
            _RecordingReservationManagerInstance.ExitApplication()
        End If
    End Sub
#End Region

#End Region

#Region " 内部メソッド "
    ''' <summary>
    ''' トゥールチップを消す。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function ClearContextMenuStrip() As System.Windows.Forms.ContextMenuStrip
        Dim reservationContextMenuStrip As ContextMenuStrip = Me.ReservationIcon.ContextMenuStrip
        Me.ReservationIcon.ContextMenuStrip = Nothing
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
        End If
    End Sub
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
    ''' バルーン表示
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub ShowBalloon(ByVal sender As Object, ByVal e As RecordingReservationEventArgs) Handles _RecordingReservationManagerInstance.StartRecording
        Me.ReservationIcon.Visible = True
        Me.ReservationIcon.BalloonTipTitle = "録画中"
        Me.ReservationIcon.BalloonTipText = e.ReservationRow.PROGRAM_TITLE & ":" & e.ReservationRow.PROGRAM_SUBTITLE & System.Environment.NewLine _
                                          & DateTime.ParseExact(e.ReservationRow.START_YYYYMMDDHHMM, DATE_FORMAT, Nothing).ToString(DATE_DISP_FORMAT) _
                                          & " - " & DateTime.ParseExact(e.ReservationRow.END_YYYYMMDDHHMM, DATE_FORMAT, Nothing).ToString("HH:mm")
        Me.ReservationIcon.ShowBalloonTip(Convert.ToInt32(DateTime.ParseExact(e.ReservationRow.END_YYYYMMDDHHMM, DATE_FORMAT, Nothing).Subtract(DateTime.Now).TotalMilliseconds))
    End Sub
    ''' <summary>
    ''' バルーン非表示
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearBallon(ByVal sender As Object, ByVal e As RecordingReservationEventArgs) Handles _RecordingReservationManagerInstance.EndRecording
        Me.ReservationIcon.Visible = False
        Me.ReservationIcon.Visible = True
    End Sub
#End Region

End Class