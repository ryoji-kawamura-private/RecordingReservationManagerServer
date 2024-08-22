Imports RK.SchemaRecordingReservation

''' <summary>
''' チャンネル登録フォーム
''' </summary>
''' <remarks></remarks>
Public Class formSettingChannel

#Region "定数・変数"
    ''' <summary>起動タイプ</summary>
    Protected Enum BOOT_TYPE As Byte
        ''' <summary>予約中登録</summary>
        REGIST_CHANNEL_WHILE_YOYAKU
        ''' <summary>チャンネル設定</summary>
        SETTING_CHANNEL
    End Enum
    ''' <summary>登録タイプ</summary>
    Protected Enum REGIST_TYPE As Byte
        ''' <summary>追加登録</summary>
        INSERT_CHANNEL
        ''' <summary>更新登録</summary>
        UPDATE_CHANNEL
    End Enum
    ''' <summary>追加ボタンキャプション</summary>
    Protected Const INSERT_CAPTION As String = "▼　追加　▼"
    ''' <summary>更新ボタンキャプション</summary>
    Protected Const UPDATE_CAPTION As String = "▼　選択行のチャンネル更新　▼"
    ''' <summary>起動タイプ</summary>
    Protected BootType As BOOT_TYPE
    ''' <summary>登録タイプ</summary>
    Protected RegistType As REGIST_TYPE
    ''' <summary>メインフォーム</summary>
    Protected MainForm As FormMain
    ''' <summary>データグリッドにバインドするデータ</summary>
    Protected StationList As SchemaRecordingReservation.M_STATIONDataTable
    ''' <summary>予約情報</summary>
    Protected ReservationRow As SchemaRecordingReservation.T_RESERVATIONRow
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
        Me.Initilize()
    End Sub
#End Region

#Region "ボタン"
    ''' <summary>
    ''' チャンネル追加ボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnReflectChannel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReflectChannel.Click
        Me.ReflectStation()
    End Sub
    ''' <summary>
    ''' キャンセルボタン
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.CancelEdit()
    End Sub
    ''' <summary>
    ''' エクスポート
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click
        Me.ExportStationList()
    End Sub
    ''' <summary>
    ''' インポート
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Me.ImportStationList()
    End Sub
    ''' <summary>
    ''' 編集ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnEditChannel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEditChannel.Click
        Me.SetControlState(REGIST_TYPE.UPDATE_CHANNEL)
    End Sub
    ''' <summary>
    ''' 削除ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnDeleteChannel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteChannel.Click
        Me.DeleteStation()
    End Sub
    ''' <summary>
    ''' ＯＫボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.MainForm.UpdateStation(Me.StationList)
    End Sub
    ''' <summary>
    ''' キャンセルボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnAllCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllCancel.Click
    End Sub
    ''' <summary>
    ''' 適用ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnApply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnApply.Click
        Me.MainForm.UpdateStation(Me.StationList)
    End Sub
#End Region

#Region "テキストボックス"
    ''' <summary>
    ''' 放送局名キーダウンイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtStationName_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtStationName.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter
                '★エンターキー押下時
                Me.ReflectStation()
        End Select
    End Sub
#End Region

#Region "データグリッド"
    ''' <summary>
    ''' データバインド
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvStation_DataBindingComplete(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewBindingCompleteEventArgs) Handles dgvStation.DataBindingComplete
        If System.ComponentModel.ListChangedType.Reset.Equals(e.ListChangedType) Then
            For Each dgvColumn As DataGridViewColumn In Me.dgvStation.Columns
                Dim stationCol As DataColumn = StationList.Columns(dgvColumn.Index)
                Dim columnProperty() As String = stationCol.Caption.Split(";"c)
                dgvColumn.HeaderText = columnProperty(0)
                dgvColumn.Visible = Convert.ToBoolean(columnProperty(1))
                Select Case stationCol.ColumnName
                    Case StationList.CHANNELColumn.ColumnName
                        dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                        '    Dim numericTextCell As New NumericTextBoxCell
                        '    With numericTextCell
                        '        .AutoFormat = False
                        '        .AutoResize = False
                        '        .Delimiter = Global.Microsoft.VisualBasic.ChrW(0)
                        '        .InputAlpha = False
                        '        .InputEtc = False
                        '        .InputKana = False
                        '        .InputZenkaku = False
                        '        .MaxLengthByte = CType(19, Short)
                        '        .MaxValue = CType(9223372036854775806, Long)
                        '        .MinValue = CType(0, Long)
                        '        .NumericAccuracy = New RK.Accuracy(CType(19, Short), CType(0, Short))
                        '        .SearchBackColor = System.Drawing.Color.LightPink
                        '        .SelectionBackColor = System.Drawing.Color.LightSteelBlue
                        '    End With
                        '    dgvColumn.CellTemplate = numericTextCell
                    Case StationList.SERVICE_IDColumn.ColumnName
                        dgvColumn.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight
                End Select
            Next
        End If
    End Sub
    ''' <summary>
    ''' キーダウンイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvStation_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles dgvStation.KeyDown
        Select Case e.KeyCode
            Case Keys.Enter, Keys.Space
                Me.SetControlState(REGIST_TYPE.UPDATE_CHANNEL)
                e.Handled = True
        End Select
    End Sub
    ''' <summary>
    ''' データグリッドダブルクリック
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub dgvStation_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles dgvStation.MouseDoubleClick
        Select Case Me.BootType
            Case BOOT_TYPE.SETTING_CHANNEL
                Dim hti As System.Windows.Forms.DataGridView.HitTestInfo = Me.dgvStation.HitTest(e.X, e.Y)
                Select Case hti.Type
                    Case DataGridViewHitTestType.RowHeader, DataGridViewHitTestType.Cell
                        Me.SetControlState(REGIST_TYPE.UPDATE_CHANNEL)
                End Select
        End Select
    End Sub
#End Region

#Region "編集メニュー"
    ''' <summary>
    ''' 編集メニュー
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub EditToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles editToolStripMenuItem.Click
        Me.SetControlState(REGIST_TYPE.UPDATE_CHANNEL)
    End Sub
#End Region

#Region "削除メニュー"
    ''' <summary>
    ''' 削除メニュー
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles deleteToolStripMenuItem.Click
        Me.DeleteStation()
    End Sub
#End Region

#End Region

#Region "外部メソッド"
    ''' <summary>
    ''' モーダルダイアログ表示
    ''' </summary>
    ''' <param name="pStationList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overloads Function ShowDialog(ByVal parentForm As Form, ByVal pStationList As SchemaRecordingReservation.M_STATIONDataTable, ByVal pReservationRow As SchemaRecordingReservation.T_RESERVATIONRow) As Windows.Forms.DialogResult
        Me.StationList = pStationList
        Me.ReservationRow = pReservationRow
        Return Me.ShowDialog(parentForm)
    End Function
#End Region

#Region "内部メソッド"
    ''' <summary>
    ''' 初期処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub Initilize()
        Dim ownerForm As Form = Me.Owner
        Do Until Me.MainForm IsNot Nothing OrElse ownerForm Is Nothing
            If TypeOf ownerForm Is FormMain Then
                Me.MainForm = DirectCast(ownerForm, FormMain)
            End If
            ownerForm = ownerForm.Owner
        Loop
        If Me.ReservationRow Is Nothing Then
            Me.SetStationData(BOOT_TYPE.SETTING_CHANNEL)
        Else
            Me.SetStationData(BOOT_TYPE.REGIST_CHANNEL_WHILE_YOYAKU)
        End If
    End Sub
    ''' <summary>
    ''' 画面に設定
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetStationData(ByVal pBootType As BOOT_TYPE)
        Me.BootType = pBootType
        Me.SetStationList()
        Select Case Me.BootType
            Case BOOT_TYPE.SETTING_CHANNEL
                Me.btnCancel.Enabled = False
            Case BOOT_TYPE.REGIST_CHANNEL_WHILE_YOYAKU
                If String.IsNullOrEmpty(Me.ReservationRow.STATION_NAME) Then
                    Me.txtStationName.Text = Me.ReservationRow.STATION
                Else
                    Me.txtStationName.Text = Me.ReservationRow.STATION_NAME
                End If
                Me.txtStationName.Enabled = False
                Me.btnEditChannel.Enabled = False
                Me.btnDeleteChannel.Enabled = False
                Me.btnExport.Enabled = False
                Me.btnImport.Enabled = False
                Me.btnOK.Enabled = False
                Me.btnAllCancel.Enabled = False
                Me.btnApply.Enabled = False
                Me.dgvStation.ContextMenuStrip = Nothing
        End Select
    End Sub
    ''' <summary>
    ''' グリッド設定
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetStationList()
        With Me.dgvStation
            .DataSource = StationList
            Me.StationList.DefaultView.Sort = Me.StationList.CHANNELColumn.ColumnName & "," _
                                            & Me.StationList.SERVICE_IDColumn.ColumnName & "," _
                                            & Me.StationList.STATIONColumn.ColumnName _
                                            & " ASC"
            .Columns(0).HeaderCell.SortGlyphDirection = SortOrder.Ascending
        End With
    End Sub
    ''' <summary>
    ''' 反映前チェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function CheckBeforeReflectChannel() As Boolean
        '★チャンネルチェック
        If Not CheckChannel() Then
            Return False
        End If
        Return True
    End Function
    ''' <summary>
    ''' チャンネルチェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function CheckChannel() As Boolean
        '★必須入力チェック
        If String.IsNullOrEmpty(Me.txtChannel.Text.Trim) Then
            MessageBox.Show("チャンネルを入力してください。", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
            SelectControl(Me.txtChannel)
            Return False
        End If
        '★重複チェック
        Select Case Me.RegistType
            Case REGIST_TYPE.INSERT_CHANNEL
                If Not Me.StationList.All(Function(stationListRow As SchemaRecordingReservation.M_STATIONRow) Not stationListRow.STATION.Equals(Me.txtStationName.Text.Trim)) Then
                    MessageBox.Show("その放送局名は既に登録されています。", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
                    SelectControl(Me.txtStationName)
                    Return False
                End If
            Case REGIST_TYPE.UPDATE_CHANNEL
        End Select
        Return True
    End Function
    ''' <summary>
    ''' サブチャンネルチェック
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function CheckServiceID() As Boolean
        '★重複チェック
        Select Case Me.RegistType
            Case REGIST_TYPE.INSERT_CHANNEL
                If Not Me.StationList.All(Function(stationListRow As SchemaRecordingReservation.M_STATIONRow) Not (stationListRow.STATION.Equals(Me.txtStationName.Text.Trim) AndAlso NVL(stationListRow.SERVICE_ID, stationListRow.IsSERVICE_IDNull, RETURN_TYPE.STRING).Equals(Me.txtServiceID.Text.Trim))) Then
                    MessageBox.Show("そのサブチャンネルは既に登録されています。", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
                    SelectControl(Me.txtStationName)
                    Return False
                End If
            Case REGIST_TYPE.UPDATE_CHANNEL
        End Select
        Return True
    End Function
    ''' <summary>
    ''' インポートファイルのチェック
    ''' </summary>
    ''' <param name="stationListText"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function CheckImportFile(ByVal stationListText() As String) As Boolean
        Dim returnValue As Boolean = True
        Dim errMsgBuilder As New System.Text.StringBuilder
        Using checkStationList As New SchemaRecordingReservation.M_STATIONDataTable
            For i As Integer = 0 To stationListText.Length - 1
                Dim stationText() As String = stationListText(i).Split(New String() {"=="}, StringSplitOptions.None)
                '★"=="の数をチェック
                If stationText.Length < 3 Then
                    With errMsgBuilder
                        .Append((i + 1).ToString) : .AppendLine("行目：""==""が入力されていません。")
                    End With
                    returnValue = False
                    Continue For
                End If
                If stationText.Length > 3 Then
                    With errMsgBuilder
                        .Append((i + 1).ToString) : .AppendLine("行目：""==""が複数入力されています。")
                    End With
                    returnValue = False
                End If
                Dim channel As String = stationText(0)
                Dim serviceID As String = stationText(1)
                Dim stationName As String = stationText(2)
                '★必須入力チェック
                If String.IsNullOrEmpty(channel) Then
                    With errMsgBuilder
                        .Append((i + 1).ToString) : .AppendLine("行目：チャンネルを入力されていません。")
                    End With
                    returnValue = False
                End If
                '★数値チェック
                If Not isInt64(channel) Then
                    With errMsgBuilder
                        .Append((i + 1).ToString) : .AppendLine("行目：入力されたチャンネルが数値として認識できません。")
                    End With
                    returnValue = False
                End If
                '★放送局名重複チェック
                If Not checkStationList.All(Function(stationListRow As SchemaRecordingReservation.M_STATIONRow) Not stationListRow.STATION.Equals(stationName)) Then
                    With errMsgBuilder
                        .Append((i + 1).ToString) : .AppendLine("行目：放送局名が重複して入力されています。")
                    End With
                    returnValue = False
                End If
                '★サブチャンネル重複チェック
                If Not checkStationList.All(Function(stationListRow As SchemaRecordingReservation.M_STATIONRow) Not (stationListRow.STATION.Equals(stationName) AndAlso NVL(stationListRow.SERVICE_ID, stationListRow.IsSERVICE_IDNull, RETURN_TYPE.STRING).Equals(serviceID))) Then
                    With errMsgBuilder
                        .Append((i + 1).ToString) : .AppendLine("行目：サブチャンネルが重複して入力されています。")
                    End With
                    returnValue = False
                    Return False
                End If
                checkStationList.AddM_STATIONRow(Convert.ToInt64(channel), Convert.ToInt64(serviceID), stationName)
            Next
            If Not returnValue Then
                MessageBox.Show("インポートファイルが不正です。" & System.Environment.NewLine & errMsgBuilder.ToString, MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 0, False)
            Else
                Me.StationList.Clear()
                Me.StationList.Dispose()
                Me.StationList = checkStationList
            End If
        End Using
        Return returnValue
    End Function
    ''' <summary>
    ''' 放送局反映処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ReflectStation()
        Dim stationRow As SchemaRecordingReservation.M_STATIONRow = Nothing
        '★チェック
        If Not CheckBeforeReflectChannel() Then
            Return
        End If
        Select Case RegistType
            Case REGIST_TYPE.INSERT_CHANNEL
                '★行追加
                stationRow = Me.StationList.NewM_STATIONRow()
            Case REGIST_TYPE.UPDATE_CHANNEL
                '★選択行編集
                stationRow = Me.GetCurrentStationRow
        End Select
        stationRow.CHANNEL = Convert.ToInt64(Me.txtChannel.Text.Trim)
        If String.IsNullOrEmpty(Me.txtServiceID.Text.Trim) Then stationRow.SetSERVICE_IDNull() Else stationRow.SERVICE_ID = Convert.ToInt64(Me.txtServiceID.Text.Trim)
        stationRow.STATION = Me.txtStationName.Text.Trim
        '★画面反映
        Me.SetStationList()
        Select Case Me.BootType
            Case BOOT_TYPE.REGIST_CHANNEL_WHILE_YOYAKU
                '★登録処理
                Me.MainForm.InsertStation(stationRow)
                Me.DialogResult = Windows.Forms.DialogResult.OK
            Case BOOT_TYPE.SETTING_CHANNEL
                '★コントロール状態設定
                Me.SetControlState(REGIST_TYPE.INSERT_CHANNEL)
        End Select
    End Sub
    ''' <summary>
    ''' キャンセル処理
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub CancelEdit()
        Select Case Me.BootType
            Case BOOT_TYPE.REGIST_CHANNEL_WHILE_YOYAKU
                Me.DialogResult = Windows.Forms.DialogResult.Cancel
                Me.Close()
            Case BOOT_TYPE.SETTING_CHANNEL
                Me.SetControlState(REGIST_TYPE.INSERT_CHANNEL)
        End Select
    End Sub
    ''' <summary>
    ''' 放送局情報削除
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub DeleteStation()
        Dim stationRow As SchemaRecordingReservation.M_STATIONRow = Me.GetCurrentStationRow()
        If stationRow IsNot Nothing Then
            Me.StationList.RemoveM_STATIONRow(stationRow)
            Me.SetStationList()
        End If
        Me.Activate()
    End Sub
    ''' <summary>
    ''' 放送局リスト出力
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ExportStationList()
        Using fileDialog As New System.Windows.Forms.SaveFileDialog
            With fileDialog
                .AddExtension = True
                .Filter = "放送局リスト（*.txt）|*.txt"
                .DefaultExt = ".txt"
                .SupportMultiDottedExtensions = True
                .CheckFileExists = False
                .CheckPathExists = False
                .DereferenceLinks = False
                .InitialDirectory = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath)
                .ShowHelp = False
                .OverwritePrompt = True
                .ValidateNames = True
                .FileName = "ExportStation.txt"
            End With
            If System.Windows.Forms.DialogResult.OK.Equals(fileDialog.ShowDialog) Then
                Dim stationListBuilder As New System.Text.StringBuilder
                With stationListBuilder
                    For Each stationRow As SchemaRecordingReservation.M_STATIONRow In Me.StationList.OrderBy(Function(pStationRow As SchemaRecordingReservation.M_STATIONRow) pStationRow.CHANNEL)
                        .Append(stationRow.CHANNEL) : .Append("==") : .Append(NVL(stationRow.SERVICE_ID, stationRow.IsSERVICE_IDNull, RETURN_TYPE.STRING)) : .Append("==") : .AppendLine(stationRow.STATION)
                    Next
                End With
                '★文字コード(ここでは、Shift JIS)
                Dim enc As System.Text.Encoding = System.Text.Encoding.GetEncoding(932)
                System.IO.File.WriteAllText(fileDialog.FileName(), stationListBuilder.ToString, enc)
                MessageBox.Show(fileDialog.FileName & " にエクスポートしました。", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0, False)
            End If
        End Using
    End Sub
    ''' <summary>
    ''' 放送局リスト入力
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ImportStationList()
        Try
            Using fileDialog As New System.Windows.Forms.OpenFileDialog
                With fileDialog
                    .AddExtension = True
                    .Filter = "放送局リスト（*.txt）|*.txt"
                    .DefaultExt = ".txt"
                    .SupportMultiDottedExtensions = True
                    .CheckFileExists = True
                    .DereferenceLinks = False
                    .InitialDirectory = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath)
                    .Multiselect = False
                    .ShowHelp = False
                    .FileName = String.Empty
                End With
                If System.Windows.Forms.DialogResult.OK.Equals(fileDialog.ShowDialog) Then
                    '★文字コード(ここでは、Shift JIS)
                    Dim enc As System.Text.Encoding = System.Text.Encoding.GetEncoding(932)
                    Dim stationList() As String = System.IO.File.ReadAllLines(fileDialog.FileName(), enc)
                    If CheckImportFile(stationList) Then
                        Me.SetStationList()
                        MessageBox.Show(fileDialog.FileName & " をインポートしました。", MESSAGEBOX_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 0, False)
                    End If
                End If
            End Using
        Catch ex As Exception

        End Try
    End Sub
    ''' <summary>
    ''' コントロール状態設定
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetControlState(ByVal pRegistState As REGIST_TYPE)
        Me.RegistType = pRegistState
        Select Case Me.RegistType
            Case REGIST_TYPE.INSERT_CHANNEL
                Me.txtChannel.Text = String.Empty
                Me.txtStationName.Text = String.Empty
                Me.txtStationName.Enabled = True
                Me.btnReflectChannel.Text = INSERT_CAPTION
                Me.btnCancel.Enabled = False
                Me.btnEditChannel.Enabled = True
                Me.btnDeleteChannel.Enabled = True
                Me.btnExport.Enabled = True
                Me.btnImport.Enabled = True
                Me.btnOK.Enabled = True
                Me.btnAllCancel.Enabled = True
                Me.btnApply.Enabled = True
                Me.dgvStation.ContextMenuStrip = Me.stationMenuStrip
                Me.dgvStation.Enabled = True
            Case REGIST_TYPE.UPDATE_CHANNEL
                Dim stationRow As SchemaRecordingReservation.M_STATIONRow = Me.GetCurrentStationRow
                If stationRow IsNot Nothing Then
                    Me.txtChannel.Text = stationRow.CHANNEL.ToString
                    Me.txtServiceID.Text = NVL(stationRow.SERVICE_ID, stationRow.IsSERVICE_IDNull, RETURN_TYPE.STRING).ToString
                    Me.txtStationName.Text = stationRow.STATION
                    Me.txtStationName.Enabled = False
                    Me.btnReflectChannel.Text = UPDATE_CAPTION
                    Me.btnCancel.Enabled = True
                    Me.btnEditChannel.Enabled = False
                    Me.btnDeleteChannel.Enabled = False
                    Me.btnExport.Enabled = False
                    Me.btnImport.Enabled = False
                    Me.btnOK.Enabled = False
                    Me.btnAllCancel.Enabled = False
                    Me.btnApply.Enabled = False
                    Me.dgvStation.ContextMenuStrip = Nothing
                    Me.dgvStation.Enabled = False
                Else
                    Me.RegistType = REGIST_TYPE.INSERT_CHANNEL
                End If
        End Select
        Me.txtChannel.Select()
        Me.txtChannel.SelectAll()
    End Sub
    ''' <summary>
    ''' カレント行にバインドされているデータ行を取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function GetCurrentStationRow() As SchemaRecordingReservation.M_STATIONRow
        If Me.dgvStation.CurrentRow IsNot Nothing Then
            Return DirectCast(DirectCast(Me.dgvStation.CurrentRow.DataBoundItem, DataRowView).Row, SchemaRecordingReservation.M_STATIONRow)
        Else
            Return Nothing
        End If
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
    ''' <summary>
    ''' 数値チェック
    ''' </summary>
    ''' <param name="expression"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Function isInt64(ByVal expression As Object) As Boolean
        If expression IsNot Nothing _
        AndAlso expression IsNot System.Convert.DBNull _
        AndAlso System.Text.RegularExpressions.Regex.IsMatch(expression.ToString, "^([1-9][0-9]*)?[0-9]$") _
        AndAlso System.Int64.TryParse(expression.ToString, New System.Int64) Then
            Return True
        Else
            Return False
        End If
    End Function
#End Region

End Class
