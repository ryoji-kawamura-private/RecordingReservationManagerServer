Imports RK

Namespace My

    ' 次のイベントは MyApplication に対して利用できます:
    ' 
    ' Startup: アプリケーションが開始されたとき、スタートアップ フォームが作成される前に発生します。
    ' Shutdown: アプリケーション フォームがすべて閉じられた後に発生します。このイベントは、通常の終了以外の方法でアプリケーションが終了されたときには発生しません。
    ' UnhandledException: ハンドルされていない例外がアプリケーションで発生したときに発生するイベントです。
    ' StartupNextInstance: 単一インスタンス アプリケーションが起動され、それが既にアクティブであるときに発生します。 
    ' NetworkAvailabilityChanged: ネットワーク接続が接続されたとき、または切断されたときに発生します。
    Partial Friend Class MyApplication
        ''' <summary>
        ''' 開始時
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub MyApplication_Startup(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            Me.SetRecordingReservation(e.CommandLine)
        End Sub
        ''' <summary>
        ''' ２重起動時処理
        ''' </summary>
        ''' <param name="sender"></param>
        ''' <param name="e"></param>
        ''' <remarks></remarks>
        Private Sub MyApplication_StartupNextInstance(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupNextInstanceEventArgs) Handles Me.StartupNextInstance
            Me.SetRecordingReservation(e.CommandLine)
        End Sub
        ''' <summary>
        ''' 録画予約処理
        ''' </summary>
        ''' <param name="commandLines"></param>
        ''' <remarks></remarks>
        Private Sub SetRecordingReservation(ByVal commandLines As System.Collections.ObjectModel.ReadOnlyCollection(Of String))
            If commandLines.Count > 0 Then
                If Me.MainForm Is Nothing Then
                    Me.MainForm = New FormMain
                End If
                Dim mainForm As FormMain = DirectCast(Me.MainForm, FormMain)
                For Each commandLine As String In commandLines
                    If Not String.IsNullOrEmpty(commandLine) Then
                        mainForm.SetRecordingReservation(commandLine)
                    End If
                Next
            End If
        End Sub

        Private Sub MyApplication_UnhandledException(ByVal sender As Object, ByVal e As Microsoft.VisualBasic.ApplicationServices.UnhandledExceptionEventArgs) Handles Me.UnhandledException
            Dim message As String = String.Empty
            Dim stackTrace As String = String.Empty
            Me.GetExceptionInfomation(e.Exception, message, stackTrace)
            MessageBox.Show(message + System.Environment.NewLine + stackTrace, "エラー発生")
            e.ExitApplication = False
        End Sub

        Private Sub GetExceptionInfomation(ByVal ex As Exception, ByRef message As String, ByRef stackTrace As String)
            message += ex.Message + System.Environment.NewLine
            stackTrace += ex.StackTrace + System.Environment.NewLine
            If ex.InnerException IsNot Nothing Then
                Me.GetExceptionInfomation(ex.InnerException, message, stackTrace)
            End If
            Return
        End Sub
    End Class

End Namespace

