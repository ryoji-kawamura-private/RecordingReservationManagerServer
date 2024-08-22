Imports RK.SchemaRecordingReservation

''' <summary>
''' 予約登録フォーム
''' </summary>
''' <remarks></remarks>
Public Class FormRegistRecordingReservation

#Region "定数・変数"
    ''' <summary>予約情報</summary>
    Protected ReservationRow As SchemaRecordingReservation.T_RESERVATIONRow
    ''' <summary>メインフォーム</summary>
    Protected MainForm As FormMain
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
        '★メインフォーム
        Dim ownerForm As Form = Me.Owner
        Do Until Me.MainForm IsNot Nothing OrElse ownerForm Is Nothing
            If TypeOf ownerForm Is FormMain Then
                Me.MainForm = DirectCast(ownerForm, FormMain)
            End If
            ownerForm = ownerForm.Owner
        Loop
        '★年コンボ
        Dim yyyyDictionary As New ArrayList
        For year As System.Int32 = DateTime.Now.Year To DateTime.Now.Year + 10
            yyyyDictionary.Add(New DictionaryEntry(year.ToString.PadLeft(4), year))
        Next
        SetComboList(Me.cboYYYY, yyyyDictionary)
        '★月コンボ
        Dim mmDictionary As New ArrayList
        For month As System.Int32 = 1 To 12
            mmDictionary.Add(New DictionaryEntry(month.ToString.PadLeft(2), month))
        Next
        SetComboList(Me.cboMM, mmDictionary)
        '★日コンボ
        Dim ddDictionary As New ArrayList
        For day As System.Int32 = 1 To 31
            ddDictionary.Add(New DictionaryEntry(day.ToString.PadLeft(2), day))
        Next
        SetComboList(Me.cboDD, ddDictionary)
        '★時コンボ
        Dim sHHDictionary As New ArrayList
        Dim eHHDictionary As New ArrayList
        For hour As System.Int32 = 0 To 23
            sHHDictionary.Add(New DictionaryEntry(hour.ToString.PadLeft(2), hour))
            eHHDictionary.Add(New DictionaryEntry(hour.ToString.PadLeft(2), hour))
        Next
        SetComboList(Me.cboStartHH, sHHDictionary)
        SetComboList(Me.cboEndHH, eHHDictionary)
        '★分コンボ
        Dim sMMDictionary As New ArrayList
        Dim eMMDictionary As New ArrayList
        For minute As System.Int32 = 0 To 59
            sMMDictionary.Add(New DictionaryEntry(minute.ToString.PadLeft(2), minute))
            eMMDictionary.Add(New DictionaryEntry(minute.ToString.PadLeft(2), minute))
        Next
        SetComboList(Me.cboStartMM, sMMDictionary)
        SetComboList(Me.cboEndMM, eMMDictionary)
        '★放送局コンボ
        With Me.cboStation
            Dim stationList As SchemaRecordingReservation.M_STATIONDataTable = Me.MainForm.GetStationList()
            .ValueMember = stationList.CHANNELColumn.ColumnName
            .DisplayMember = stationList.STATIONColumn.ColumnName
            .DataSource = stationList
        End With
        Me.SetReservationData()
        Me.SetControlEnabled()
    End Sub
#End Region

#Region "ボタン"
    ''' <summary>
    ''' 予約ボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub BtnReservation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnReservation.Click
        Me.GetReservationData()
        With MainForm
            If TypeOf Me.Owner Is FormRecordingReservationList Then
                If Not .UpdateRecordingReservation(Me.ReservationRow) Then
                    Return
                End If
            Else
                If Not .SetRecordingReservation(Me.ReservationRow, False) Then
                    Return
                End If
            End If
        End With
        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    ''' <summary>
    ''' キャンセルボタン押下処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub BtnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
#End Region

#End Region

#Region "外部メソッド"
    ''' <summary>
    ''' ダイアログとして表示
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

#Region "内部メソッド"
    ''' <summary>
    ''' コンボセッティング
    ''' </summary>
    ''' <param name="combo"></param>
    ''' <param name="listItem"></param>
    ''' <remarks></remarks>
    Protected Sub SetComboList(ByVal combo As ComboBox, ByVal listItem As ArrayList)
        combo.Items.Clear()
        combo.DisplayMember = "Key"
        combo.ValueMember = "Value"
        combo.DataSource = listItem
    End Sub
    ''' <summary>
    ''' 画面に設定
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetReservationData()
        With Me.ReservationRow
            Me.txtTitle.Text = .PROGRAM_TITLE
            If String.IsNullOrEmpty(.STATION_NAME) Then
                Me.cboStation.Text = .STATION
            Else
                Me.cboStation.Text = .STATION_NAME
            End If
            Try
                Dim startDate As DateTime = DateTime.ParseExact(.START_YYYYMMDDHHMM, DATE_FORMAT, Nothing)
                Me.cboYYYY.SelectedValue = startDate.Year
                Me.cboMM.SelectedValue = startDate.Month
                Me.cboDD.SelectedValue = startDate.Day
                Me.cboStartHH.SelectedValue = startDate.Hour
                Me.cboStartMM.SelectedValue = startDate.Minute
            Catch ex As Exception
                Me.cboYYYY.SelectedValue = DateTime.Now.Year
                Me.cboMM.SelectedValue = DateTime.Now.Month
                Me.cboDD.SelectedValue = DateTime.Now.Day
                Me.cboStartHH.SelectedValue = DateTime.Now.Hour
                Me.cboStartMM.SelectedValue = DateTime.Now.Minute
            End Try
            Try
                Dim endDate As DateTime = DateTime.ParseExact(.END_YYYYMMDDHHMM, DATE_FORMAT, Nothing)
                Me.cboEndHH.SelectedValue = endDate.Hour
                Me.cboEndMM.SelectedValue = endDate.Minute
            Catch ex As Exception
                Me.cboEndHH.SelectedValue = DateTime.Now.Hour
                Me.cboEndMM.SelectedValue = DateTime.Now.Minute
            End Try
            Me.chkSuspendAfterRecording.Checked = (1L).Equals(.SUSPEND_AFTER_RECORDING)
            Me.chkOutputErrorLog.Checked = (1L).Equals(.OUTPUT_ERRORLOG)
            Me.chkExitApplicationAfterRecording.Checked = (1L).Equals(.EXIT_APPLICATION_AFTER_RECORDING)
            Me.chkPreview.Checked = (1L).Equals(.PREVIEW)
        End With
        Me.TopMost = True
        Me.Activate()
        Me.TopMost = False
    End Sub
    ''' <summary>
    ''' コントロールの活性制御
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetControlEnabled()
        If (2L).Equals(ReservationRow.RECORDED) Then
            Me.cboStation.Enabled = False
            Me.cboYYYY.Enabled = False
            Me.cboMM.Enabled = False
            Me.cboDD.Enabled = False
            Me.cboStartHH.Enabled = False
            Me.cboStartMM.Enabled = False
            Me.cboEndHH.Enabled = False
            Me.cboEndMM.Enabled = False
            Me.chkOutputErrorLog.Enabled = False
            Me.chkExitApplicationAfterRecording.Enabled = False
            Me.chkPreview.Enabled = False
        End If
    End Sub
    ''' <summary>
    ''' 画面データを取得
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub GetReservationData()
        With Me.ReservationRow
            .PROGRAM_TITLE = Me.txtTitle.Text
            .STATION = Me.cboStation.Text
            .STATION_NAME = Me.cboStation.Text
            Dim startHHMM As String = Me.cboStartHH.SelectedValue.ToString.PadLeft(2, "0"c) & Me.cboStartMM.SelectedValue.ToString.PadLeft(2, "0"c)
            Dim endHHMM As String = Me.cboEndHH.SelectedValue.ToString.PadLeft(2, "0"c) & Me.cboEndMM.SelectedValue.ToString.PadLeft(2, "0"c)
            .START_YYYYMMDDHHMM = Me.cboYYYY.SelectedValue.ToString & Me.cboMM.SelectedValue.ToString.PadLeft(2, "0"c) & Me.cboDD.SelectedValue.ToString.PadLeft(2, "0"c) & startHHMM
            Dim endYYYYMMDDHHMM As String = Me.cboYYYY.SelectedValue.ToString & Me.cboMM.SelectedValue.ToString.PadLeft(2, "0"c) & Me.cboDD.SelectedValue.ToString.PadLeft(2, "0"c) & endHHMM
            If startHHMM > endHHMM Then
                .END_YYYYMMDDHHMM = DateTime.ParseExact(endYYYYMMDDHHMM, DATE_FORMAT, Nothing).AddDays(1).ToString(DATE_FORMAT)
            Else
                .END_YYYYMMDDHHMM = Me.cboYYYY.SelectedValue.ToString & Me.cboMM.SelectedValue.ToString.PadLeft(2, "0"c) & Me.cboDD.SelectedValue.ToString.PadLeft(2, "0"c) & endHHMM
            End If
            If Me.chkSuspendAfterRecording.Checked Then .SUSPEND_AFTER_RECORDING = 1L Else .SUSPEND_AFTER_RECORDING = 0L
            If Me.chkOutputErrorLog.Checked Then .OUTPUT_ERRORLOG = 1L Else .OUTPUT_ERRORLOG = 0L
            If Me.chkExitApplicationAfterRecording.Checked Then .EXIT_APPLICATION_AFTER_RECORDING = 1L Else .EXIT_APPLICATION_AFTER_RECORDING = 0L
            If Me.chkPreview.Checked Then .PREVIEW = 1L Else .PREVIEW = 0L
        End With
    End Sub
#End Region

End Class
