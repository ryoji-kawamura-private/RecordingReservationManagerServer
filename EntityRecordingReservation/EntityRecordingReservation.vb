Imports System.Data.SQLite
Imports System.Text
Imports RK.SchemaRecordingReservation

''' <summary>
''' エンティティ
''' </summary>
''' <remarks></remarks>
Public Class EntityRecordingReservation : Implements IDisposable

#Region " 定数・変数 "
    ''' <summary>disposeメソッドの重複する呼び出しを検出する</summary>
    Protected disposedValue As Boolean = False
    ''' <summary>Ｓｑｌｉｔｅコネクション</summary>
    Protected sqliteConn As SQLiteConnection
    ''' <summary>Ｓｑｌｉｔｅコマンド</summary>
    Protected sqliteCmd As SQLiteCommand
#End Region

#Region "コンストラクタ"
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        sqliteConn = New SQLiteConnection("Version=3;Data Source=" & System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "DB\RESERVATION.db") & ";New=False;Compress=True;")
        sqliteConn.Open()
        sqliteCmd = sqliteConn.CreateCommand
    End Sub
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New(ByVal dbPath As String)
        sqliteConn = New SQLiteConnection("Version=3;Data Source=" & dbPath & ";New=False;Compress=True;")
        sqliteConn.Open()
        sqliteCmd = sqliteConn.CreateCommand
    End Sub
#End Region

#Region "デストラクタ"
    ''' <summary>
    ''' Finalizeメソッド
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub Finalize()
        Me.Dispose(False)
        MyBase.Finalize()
    End Sub

    ''' <summary>
    ''' Disposeメソッド
    ''' </summary>
    ''' <param name="disposing"></param>
    ''' <remarks></remarks>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                ' TODO: 明示的に呼び出されたときにマネージ リソースを解放します
            End If
            ' TODO: 共有のアンマネージ リソースを解放します
            If sqliteCmd IsNot Nothing Then
                sqliteCmd.Dispose()
            End If
            If sqliteConn IsNot Nothing Then
                If ConnectionState.Open.Equals(sqliteConn.State) Then
                    sqliteConn.Close()
                End If
                sqliteConn.Dispose()
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' このコードは、破棄可能なパターンを正しく実装できるように Visual Basic によって追加されました。
    ''' <summary>
    ''' Diposeメソッド
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Dispose() Implements IDisposable.Dispose
        ' このコードを変更しないでください。クリーンアップ コードを上の Dispose(ByVal disposing As Boolean) に記述します。
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

#Region "外部メソッド"

#Region "トランザクション開始"
    ''' <summary>
    ''' トランザクション開始
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BeginTran()
        Me.sqliteCmd.Transaction = Me.sqliteConn.BeginTransaction()
    End Sub
#End Region

#Region "トランザクション終了"
    ''' <summary>
    ''' トランザクション終了
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Commit()
        Me.sqliteCmd.Transaction.Commit()
    End Sub
#End Region

#Region "設定値取得"
    ''' <summary>
    ''' 設定値取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetSetting() As SchemaRecordingReservation.M_SETTINGDataTable
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("SELECT ")
            .AppendLine("     START_INTERVAL_SECONDS ")
            .AppendLine("    ,POWER_ON_INTERVAL_SECONDS ")
            .AppendLine("    ,EXE_BOOT_INTERVAL_SECONDS ")
            .AppendLine("    ,CHECK_START_INTERVAL_MINUTES ")
            .AppendLine("    ,OUTPUT_ERRORLOG ")
            .AppendLine("    ,EXIT_APPLICATION_AFTER_RECORDING ")
            .AppendLine("    ,PREVIEW ")
            .AppendLine("    ,SUSPEND_AFTER_RECORDING ")
            .AppendLine("    ,TVTEST_FULL_PATH ")
            .AppendLine("    ,SAVE_PATH ")
            .AppendLine("FROM ")
            .AppendLine("     M_SETTING ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        Dim settingTbl As New SchemaRecordingReservation.M_SETTINGDataTable
        Using sqliteDtAdpt As SQLiteDataAdapter = New SQLiteDataAdapter(Me.sqliteCmd)
            sqliteDtAdpt.Fill(settingTbl)
            Return settingTbl
        End Using
    End Function
#End Region

#Region "デバイス情報取得"
    ''' <summary>
    ''' デバイス情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDevice(ByVal broadcastType As BROADCAST_TYPE) As SchemaRecordingReservation.M_DEVICEDataTable
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("SELECT ")
            .AppendLine("     DLL_NAME ")
            .AppendLine("    ,PRIORITY ")
            .AppendLine("FROM ")
            .AppendLine("     M_DEVICE ")
            .AppendLine("WHERE ")
            .AppendLine("     BROADCAST_TYPE = :BROADCAST_TYPE ")
            .AppendLine("ORDER BY ")
            .AppendLine("     PRIORITY DESC ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        With sqliteCmd.Parameters
            .Add(New SQLiteParameter(":BROADCAST_TYPE", broadcastType))
        End With
        Dim deviceTbl As New SchemaRecordingReservation.M_DEVICEDataTable
        Using sqliteDtAdpt As SQLiteDataAdapter = New SQLiteDataAdapter(Me.sqliteCmd)
            sqliteDtAdpt.Fill(deviceTbl)
            Return deviceTbl
        End Using
    End Function
#End Region

#Region "デバイス情報取得"
    ''' <summary>
    ''' デバイス情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDevice(ByVal dllName As String) As SchemaRecordingReservation.M_DEVICERow
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("SELECT ")
            .AppendLine("     DLL_NAME ")
            .AppendLine("    ,PRIORITY ")
            .AppendLine("FROM ")
            .AppendLine("     M_DEVICE ")
            .AppendLine("WHERE ")
            .AppendLine("     DLL_NAME = :DLL_NAME ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        With sqliteCmd.Parameters
            .Add(New SQLiteParameter(":DLL_NAME", dllName))
        End With
        Dim deviceTbl As New SchemaRecordingReservation.M_DEVICEDataTable
        Using sqliteDtAdpt As SQLiteDataAdapter = New SQLiteDataAdapter(Me.sqliteCmd)
            sqliteDtAdpt.Fill(deviceTbl)
            If deviceTbl.Rows.Count > 0 Then
                Return deviceTbl(0)
            Else
                Return Nothing
            End If
        End Using
    End Function
#End Region

#Region "チャンネル取得"
    ''' <summary>
    ''' チャンネル取得
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetChannel(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow) As SchemaRecordingReservation.M_STATIONDataTable
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("SELECT ")
            .AppendLine("     CHANNEL ")
            .AppendLine("    ,SERVICE_ID ")
            .AppendLine("FROM ")
            .AppendLine("     M_STATION ")
            .AppendLine("WHERE ")
            .AppendLine("     STATION = :STATION ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        With sqliteCmd.Parameters
            .Add(New SQLiteParameter(":STATION", reservationRow.STATION))
        End With
        Dim stationTbl As New SchemaRecordingReservation.M_STATIONDataTable
        Using sqliteDtAdpt As SQLiteDataAdapter = New SQLiteDataAdapter(Me.sqliteCmd)
            sqliteDtAdpt.Fill(stationTbl)
            If stationTbl.Count > 0 Then
                Return stationTbl
            Else
                With sqliteCmd.Parameters
                    .Clear()
                    .Add(New SQLiteParameter(":STATION", reservationRow.STATION_NAME))
                End With
                sqliteDtAdpt.Fill(stationTbl)
                Return stationTbl
            End If
        End Using
    End Function
#End Region

#Region "放送局情報取得"
    ''' <summary>
    ''' チャンネル取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStationList() As SchemaRecordingReservation.M_STATIONDataTable
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("SELECT ")
            .AppendLine("     STATION ")
            .AppendLine("    ,CHANNEL ")
            .AppendLine("    ,SERVICE_ID ")
            .AppendLine("FROM ")
            .AppendLine("     M_STATION ")
            .AppendLine("ORDER BY ")
            .AppendLine("     CHANNEL ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        Dim stationListTbl As New SchemaRecordingReservation.M_STATIONDataTable
        Using sqliteDtAdpt As SQLiteDataAdapter = New SQLiteDataAdapter(Me.sqliteCmd)
            sqliteDtAdpt.Fill(stationListTbl)
            Return stationListTbl
        End Using
    End Function
#End Region

#Region "予約一覧取得"
    ''' <summary>
    ''' 予約一覧取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetReservation() As SchemaRecordingReservation.T_RESERVATIONDataTable
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("SELECT ")
            .AppendLine("     ID ")
            .AppendLine("    ,DLL_NAME ")
            .AppendLine("    ,BROADCAST_TYPE ")
            .AppendLine("    ,STATION ")
            .AppendLine("    ,STATION_NAME ")
            .AppendLine("    ,CHANNEL ")
            .AppendLine("    ,SERVICE_ID ")
            .AppendLine("    ,START_YYYYMMDDHHMM ")
            .AppendLine("    ,END_YYYYMMDDHHMM ")
            .AppendLine("    ,PROGRAM_TITLE ")
            .AppendLine("    ,PROGRAM_SUBTITLE ")
            .AppendLine("    ,EXTEND ")
            .AppendLine("    ,PERFORMER ")
            .AppendLine("    ,GENRE ")
            .AppendLine("    ,SUBGENRE")
            .AppendLine("    ,NOTE")
            .AppendLine("    ,OUTPUT_ERRORLOG ")
            .AppendLine("    ,EXIT_APPLICATION_AFTER_RECORDING ")
            .AppendLine("    ,PREVIEW ")
            .AppendLine("    ,SUSPEND_AFTER_RECORDING ")
            .AppendLine("    ,RECORDED")
            .AppendLine("    ,PATH")
            .AppendLine("    ,CLIENT_PC_NAME")
            .AppendLine("FROM ")
            .AppendLine("     T_RESERVATION ")
            .AppendLine("WHERE ")
            .AppendLine("     RECORDED IN (0,2) ")
            .AppendLine("ORDER BY ")
            .AppendLine("     START_YYYYMMDDHHMM ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        Dim reservationTbl As New SchemaRecordingReservation.T_RESERVATIONDataTable
        Using sqliteDtAdpt As SQLiteDataAdapter = New SQLiteDataAdapter(sqliteCmd)
            sqliteDtAdpt.Fill(reservationTbl)
            Return reservationTbl
        End Using
    End Function
#End Region

#Region "録画一覧取得"
    ''' <summary>
    ''' 録画一覧取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRecordedList() As SchemaRecordingReservation.T_RESERVATIONDataTable
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("SELECT ")
            .AppendLine("     ID ")
            .AppendLine("    ,DLL_NAME ")
            .AppendLine("    ,BROADCAST_TYPE ")
            .AppendLine("    ,STATION ")
            .AppendLine("    ,STATION_NAME ")
            .AppendLine("    ,CHANNEL ")
            .AppendLine("    ,SERVICE_ID ")
            .AppendLine("    ,START_YYYYMMDDHHMM ")
            .AppendLine("    ,END_YYYYMMDDHHMM ")
            .AppendLine("    ,PROGRAM_TITLE ")
            .AppendLine("    ,PROGRAM_SUBTITLE ")
            .AppendLine("    ,EXTEND ")
            .AppendLine("    ,PERFORMER ")
            .AppendLine("    ,GENRE ")
            .AppendLine("    ,SUBGENRE")
            .AppendLine("    ,NOTE")
            .AppendLine("    ,OUTPUT_ERRORLOG ")
            .AppendLine("    ,EXIT_APPLICATION_AFTER_RECORDING ")
            .AppendLine("    ,PREVIEW ")
            .AppendLine("    ,SUSPEND_AFTER_RECORDING ")
            .AppendLine("    ,RECORDED")
            .AppendLine("    ,PATH")
            .AppendLine("    ,CLIENT_PC_NAME")
            .AppendLine("FROM ")
            .AppendLine("     T_RESERVATION ")
            .AppendLine("WHERE ")
            .AppendLine("     RECORDED = 1 ")
            .AppendLine("ORDER BY ")
            .AppendLine("     START_YYYYMMDDHHMM ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        Dim reservationTbl As New SchemaRecordingReservation.T_RESERVATIONDataTable
        Using sqliteDtAdpt As SQLiteDataAdapter = New SQLiteDataAdapter(sqliteCmd)
            sqliteDtAdpt.Fill(reservationTbl)
            Return reservationTbl
        End Using
    End Function
#End Region

#Region "重複予約取得"
    ''' <summary>
    ''' 重複予約取得
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDuplicateReservation(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow) As SchemaRecordingReservation.T_RESERVATIONDataTable
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("SELECT ")
            .AppendLine("     ID ")
            .AppendLine("    ,DLL_NAME ")
            .AppendLine("    ,BROADCAST_TYPE ")
            .AppendLine("    ,STATION ")
            .AppendLine("    ,STATION_NAME ")
            .AppendLine("    ,CHANNEL ")
            .AppendLine("    ,SERVICE_ID ")
            .AppendLine("    ,START_YYYYMMDDHHMM ")
            .AppendLine("    ,END_YYYYMMDDHHMM ")
            .AppendLine("    ,PROGRAM_TITLE ")
            .AppendLine("    ,PROGRAM_SUBTITLE ")
            .AppendLine("    ,EXTEND ")
            .AppendLine("    ,PERFORMER ")
            .AppendLine("    ,GENRE ")
            .AppendLine("    ,SUBGENRE")
            .AppendLine("    ,NOTE")
            .AppendLine("    ,OUTPUT_ERRORLOG ")
            .AppendLine("    ,EXIT_APPLICATION_AFTER_RECORDING ")
            .AppendLine("    ,PREVIEW ")
            .AppendLine("    ,SUSPEND_AFTER_RECORDING ")
            .AppendLine("    ,RECORDED")
            .AppendLine("    ,PATH")
            .AppendLine("    ,CLIENT_PC_NAME")
            .AppendLine("FROM ")
            .AppendLine("     T_RESERVATION ")
            .AppendLine("WHERE ")
            .AppendLine("     ((START_YYYYMMDDHHMM >= :START_YYYYMMDDHHMM AND START_YYYYMMDDHHMM < :END_YYYYMMDDHHMM) ")
            .AppendLine("   OR (END_YYYYMMDDHHMM > :START_YYYYMMDDHHMM AND END_YYYYMMDDHHMM <= :END_YYYYMMDDHHMM) ")
            .AppendLine("   OR (START_YYYYMMDDHHMM < :START_YYYYMMDDHHMM AND END_YYYYMMDDHHMM > :END_YYYYMMDDHHMM)) ")
            .AppendLine(" AND RECORDED IN (0,2) ")
            .AppendLine(" AND BROADCAST_TYPE = :BROADCAST_TYPE ")
            .AppendLine(" AND ID <> :ID ")
            .AppendLine("ORDER BY ")
            .AppendLine("     START_YYYYMMDDHHMM ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        With sqliteCmd.Parameters
            .Clear()
            .Add(New SQLiteParameter(":START_YYYYMMDDHHMM", reservationRow.START_YYYYMMDDHHMM))
            .Add(New SQLiteParameter(":END_YYYYMMDDHHMM", reservationRow.END_YYYYMMDDHHMM))
            .Add(New SQLiteParameter(":BROADCAST_TYPE", reservationRow.BROADCAST_TYPE))
            .Add(New SQLiteParameter(":ID", reservationRow.ID))
        End With
        Dim duplicateReservationTbl As New SchemaRecordingReservation.T_RESERVATIONDataTable
        Using sqliteDtAdpt As SQLiteDataAdapter = New SQLiteDataAdapter(sqliteCmd)
            sqliteDtAdpt.Fill(duplicateReservationTbl)
            Return duplicateReservationTbl
        End Using
    End Function
#End Region

#Region "録画デバイス取得"
    ''' <summary>
    ''' 録画デバイス取得
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRecordingDevice(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow) As SchemaRecordingReservation.M_DEVICEDataTable
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .Append("SELECT ")
            .Append("     DLL_NAME ")
            .Append("    ,PRIORITY ")
            .Append("FROM ")
            .Append("     M_DEVICE ")
            .Append("WHERE ")
            .Append("     BROADCAST_TYPE = :BROADCAST_TYPE ")
            .Append(" AND DLL_NAME NOT IN (SELECT ")
            .Append("                         CASE WHEN DLL_NAME IS NULL THEN '' ELSE DLL_NAME END ")
            .Append("                    FROM ")
            .Append("                         T_RESERVATION ")
            .Append("                    WHERE ")
            .Append("                         ((START_YYYYMMDDHHMM >= :START_YYYYMMDDHHMM AND START_YYYYMMDDHHMM < :END_YYYYMMDDHHMM) ")
            .Append("                       OR (END_YYYYMMDDHHMM > :START_YYYYMMDDHHMM AND END_YYYYMMDDHHMM <= :END_YYYYMMDDHHMM) ")
            .Append("                       OR (START_YYYYMMDDHHMM < :START_YYYYMMDDHHMM AND END_YYYYMMDDHHMM > :END_YYYYMMDDHHMM)) ")
            .Append("                     AND RECORDED IN (0,2) ")
            .Append("                     AND BROADCAST_TYPE = :BROADCAST_TYPE ")
            .Append("                     AND ID <> :ID) ")
            .Append("ORDER BY ")
            .Append("     PRIORITY ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        With sqliteCmd.Parameters
            .Clear()
            .Add(New SQLiteParameter(":START_YYYYMMDDHHMM", reservationRow.START_YYYYMMDDHHMM))
            .Add(New SQLiteParameter(":END_YYYYMMDDHHMM", reservationRow.END_YYYYMMDDHHMM))
            .Add(New SQLiteParameter(":BROADCAST_TYPE", reservationRow.BROADCAST_TYPE))
            .Add(New SQLiteParameter(":ID", reservationRow.ID))
        End With
        Dim deviceList As New SchemaRecordingReservation.M_DEVICEDataTable
        Using sqliteDtAdpt As SQLiteDataAdapter = New SQLiteDataAdapter(sqliteCmd)
            sqliteDtAdpt.Fill(deviceList)
            Return deviceList
        End Using
    End Function
#End Region

#Region "現在からインターバル時間以内に開始する番組情報を取得"
    ''' <summary>
    ''' 番組情報を取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStartReservation(ByVal interval As System.Int64) As SchemaRecordingReservation.T_RESERVATIONDataTable
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("SELECT ")
            .AppendLine("     ID ")
            .AppendLine("    ,DLL_NAME ")
            .AppendLine("    ,BROADCAST_TYPE ")
            .AppendLine("    ,STATION ")
            .AppendLine("    ,STATION_NAME ")
            .AppendLine("    ,CHANNEL ")
            .AppendLine("    ,SERVICE_ID ")
            .AppendLine("    ,START_YYYYMMDDHHMM ")
            .AppendLine("    ,END_YYYYMMDDHHMM ")
            .AppendLine("    ,PROGRAM_TITLE ")
            .AppendLine("    ,PROGRAM_SUBTITLE ")
            .AppendLine("    ,EXTEND ")
            .AppendLine("    ,PERFORMER ")
            .AppendLine("    ,GENRE ")
            .AppendLine("    ,SUBGENRE")
            .AppendLine("    ,NOTE")
            .AppendLine("    ,OUTPUT_ERRORLOG ")
            .AppendLine("    ,EXIT_APPLICATION_AFTER_RECORDING ")
            .AppendLine("    ,PREVIEW ")
            .AppendLine("    ,SUSPEND_AFTER_RECORDING ")
            .AppendLine("    ,RECORDED")
            .AppendLine("    ,PATH")
            .AppendLine("    ,CLIENT_PC_NAME")
            .AppendLine("FROM ")
            .AppendLine("     T_RESERVATION ")
            .AppendLine("WHERE ")
            .AppendLine("     :NOW_YYYYMMDDHHMM <= START_YYYYMMDDHHMM ")
            .AppendLine(" AND START_YYYYMMDDHHMM <= :START_INTERVAL_MINUTES_LATER_YYYYMMDDHHMM ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        With sqliteCmd.Parameters
            .Clear()
            .Add(New SQLiteParameter(":NOW_YYYYMMDDHHMM", DateTime.Now.ToString(DATE_FORMAT)))
            .Add(New SQLiteParameter(":START_INTERVAL_MINUTES_LATER_YYYYMMDDHHMM", DateTime.Now.AddMinutes(interval).ToString(DATE_FORMAT)))
        End With
        Dim startProgramTbl As New SchemaRecordingReservation.T_RESERVATIONDataTable
        Using sqliteDtAdpt As SQLiteDataAdapter = New SQLiteDataAdapter(sqliteCmd)
            sqliteDtAdpt.Fill(startProgramTbl)
            Return startProgramTbl
        End Using
    End Function
#End Region

#Region "これから録画する番組情報を取得"
    ''' <summary>
    ''' これから録画する番組情報を取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetStartReservation() As SchemaRecordingReservation.T_RESERVATIONDataTable
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("SELECT ")
            .AppendLine("     ID ")
            .AppendLine("    ,DLL_NAME ")
            .AppendLine("    ,BROADCAST_TYPE ")
            .AppendLine("    ,STATION ")
            .AppendLine("    ,STATION_NAME ")
            .AppendLine("    ,CHANNEL ")
            .AppendLine("    ,SERVICE_ID ")
            .AppendLine("    ,START_YYYYMMDDHHMM ")
            .AppendLine("    ,END_YYYYMMDDHHMM ")
            .AppendLine("    ,PROGRAM_TITLE ")
            .AppendLine("    ,PROGRAM_SUBTITLE ")
            .AppendLine("    ,EXTEND ")
            .AppendLine("    ,PERFORMER ")
            .AppendLine("    ,GENRE ")
            .AppendLine("    ,SUBGENRE")
            .AppendLine("    ,NOTE")
            .AppendLine("    ,OUTPUT_ERRORLOG ")
            .AppendLine("    ,EXIT_APPLICATION_AFTER_RECORDING ")
            .AppendLine("    ,PREVIEW ")
            .AppendLine("    ,SUSPEND_AFTER_RECORDING ")
            .AppendLine("    ,RECORDED")
            .AppendLine("    ,PATH")
            .AppendLine("    ,CLIENT_PC_NAME")
            .AppendLine("FROM ")
            .AppendLine("     T_RESERVATION ")
            .AppendLine("WHERE ")
            .AppendLine("     END_YYYYMMDDHHMM > :NOW_YYYYMMDDHHMM ")
            .AppendLine(" AND RECORDED IN (0,2) ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        With sqliteCmd.Parameters
            .Clear()
            .Add(New SQLiteParameter(":NOW_YYYYMMDDHHMM", DateTime.Now.ToString(DATE_FORMAT)))
        End With
        Dim startProgramTbl As New SchemaRecordingReservation.T_RESERVATIONDataTable
        Using sqliteDtAdpt As SQLiteDataAdapter = New SQLiteDataAdapter(sqliteCmd)
            sqliteDtAdpt.Fill(startProgramTbl)
            Return startProgramTbl
        End Using
    End Function
#End Region

#Region "IDで予約取得"
    ''' <summary>
    ''' 予約一覧取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetReservation(ByVal id As Int64) As SchemaRecordingReservation.T_RESERVATIONDataTable
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("SELECT ")
            .AppendLine("     ID ")
            .AppendLine("    ,DLL_NAME ")
            .AppendLine("    ,BROADCAST_TYPE ")
            .AppendLine("    ,STATION ")
            .AppendLine("    ,STATION_NAME ")
            .AppendLine("    ,CHANNEL ")
            .AppendLine("    ,SERVICE_ID ")
            .AppendLine("    ,START_YYYYMMDDHHMM ")
            .AppendLine("    ,END_YYYYMMDDHHMM ")
            .AppendLine("    ,PROGRAM_TITLE ")
            .AppendLine("    ,PROGRAM_SUBTITLE ")
            .AppendLine("    ,EXTEND ")
            .AppendLine("    ,PERFORMER ")
            .AppendLine("    ,GENRE ")
            .AppendLine("    ,SUBGENRE")
            .AppendLine("    ,NOTE")
            .AppendLine("    ,OUTPUT_ERRORLOG ")
            .AppendLine("    ,EXIT_APPLICATION_AFTER_RECORDING ")
            .AppendLine("    ,PREVIEW ")
            .AppendLine("    ,SUSPEND_AFTER_RECORDING ")
            .AppendLine("    ,RECORDED")
            .AppendLine("    ,PATH")
            .AppendLine("    ,CLIENT_PC_NAME")
            .AppendLine("FROM ")
            .AppendLine("     T_RESERVATION ")
            .AppendLine("WHERE ")
            .AppendLine("     ID = :ID ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        With Me.sqliteCmd.Parameters
            .Clear()
            .Add(New SQLiteParameter(":ID", id))
        End With
        Dim reservationTbl As New SchemaRecordingReservation.T_RESERVATIONDataTable
        Using sqliteDtAdpt As SQLiteDataAdapter = New SQLiteDataAdapter(sqliteCmd)
            sqliteDtAdpt.Fill(reservationTbl)
            Return reservationTbl
        End Using
    End Function
#End Region

#Region "ＩＤ取得"
    ''' <summary>
    ''' ＩＤ取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetID() As System.Int64
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("SELECT ")
            .AppendLine("     MAX(ID) ")
            .AppendLine("FROM ")
            .AppendLine("     T_RESERVATION ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        Dim maxIDTbl As New DataTable
        Using sqliteDtAdpt As SQLiteDataAdapter = New SQLiteDataAdapter(sqliteCmd)
            sqliteDtAdpt.Fill(maxIDTbl)
            If maxIDTbl.Rows(0)(0) Is System.Convert.DBNull Then
                Return 1L
            Else
                Return Convert.ToInt64(maxIDTbl.Rows(0)(0)) + 1L
            End If
        End Using
    End Function
#End Region

#Region "ログ取得"
    ''' <summary>
    ''' ログ取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetLog(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow) As SchemaRecordingReservation.T_LOGDataTable
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("SELECT ")
            .AppendLine("     MESSAGE ")
            .AppendLine("FROM ")
            .AppendLine("     T_LOG ")
            .AppendLine("WHERE ")
            .AppendLine("     (ID = :ID) OR (:ID IS NULL) ")
            .AppendLine("ORDER BY ")
            .AppendLine("     SEQ ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        With sqliteCmd.Parameters
            .Clear()
            .Add(New SQLiteParameter(":ID", reservationRow.ID))
        End With
        Dim logTable As New SchemaRecordingReservation.T_LOGDataTable
        Using sqliteDtAdpt As SQLiteDataAdapter = New SQLiteDataAdapter(sqliteCmd)
            sqliteDtAdpt.Fill(logTable)
            Return logTable
        End Using
    End Function
#End Region

#Region "録画予約情報登録"
    ''' <summary>
    ''' 録画予約情報登録
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <remarks></remarks>
    Public Sub InsertReservation(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow)
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("INSERT INTO ")
            .AppendLine("     T_RESERVATION ")
            .AppendLine("    (ID ")
            .AppendLine("    ,DLL_NAME ")
            .AppendLine("    ,BROADCAST_TYPE ")
            .AppendLine("    ,STATION ")
            .AppendLine("    ,STATION_NAME ")
            .AppendLine("    ,CHANNEL ")
            .AppendLine("    ,SERVICE_ID ")
            .AppendLine("    ,START_YYYYMMDDHHMM ")
            .AppendLine("    ,END_YYYYMMDDHHMM ")
            .AppendLine("    ,PROGRAM_TITLE ")
            .AppendLine("    ,PROGRAM_SUBTITLE ")
            .AppendLine("    ,EXTEND ")
            .AppendLine("    ,PERFORMER ")
            .AppendLine("    ,GENRE ")
            .AppendLine("    ,SUBGENRE ")
            .AppendLine("    ,NOTE ")
            .AppendLine("    ,OUTPUT_ERRORLOG ")
            .AppendLine("    ,EXIT_APPLICATION_AFTER_RECORDING ")
            .AppendLine("    ,PREVIEW ")
            .AppendLine("    ,SUSPEND_AFTER_RECORDING ")
            .AppendLine("    ,RECORDED ")
            .AppendLine("    ,PATH ")
            .AppendLine("    ,CLIENT_PC_NAME) ")
            .AppendLine("VALUES ")
            .AppendLine("    (:ID ")
            .AppendLine("    ,:DLL_NAME ")
            .AppendLine("    ,:BROADCAST_TYPE ")
            .AppendLine("    ,:STATION ")
            .AppendLine("    ,:STATION_NAME ")
            .AppendLine("    ,:CHANNEL ")
            .AppendLine("    ,:SERVICE_ID ")
            .AppendLine("    ,:START_YYYYMMDDHHMM ")
            .AppendLine("    ,:END_YYYYMMDDHHMM ")
            .AppendLine("    ,:PROGRAM_TITLE ")
            .AppendLine("    ,:PROGRAM_SUBTITLE ")
            .AppendLine("    ,:EXTEND ")
            .AppendLine("    ,:PERFORMER ")
            .AppendLine("    ,:GENRE ")
            .AppendLine("    ,:SUBGENRE ")
            .AppendLine("    ,:NOTE ")
            .AppendLine("    ,:OUTPUT_ERRORLOG ")
            .AppendLine("    ,:EXIT_APPLICATION_AFTER_RECORDING ")
            .AppendLine("    ,:PREVIEW ")
            .AppendLine("    ,:SUSPEND_AFTER_RECORDING ")
            .AppendLine("    ,:RECORDED ")
            .AppendLine("    ,:PATH ")
            .AppendLine("    ,:CLIENT_PC_NAME) ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        With Me.sqliteCmd.Parameters
            .Clear()
            .Add(New SQLiteParameter(":ID", reservationRow.ID))
            .Add(New SQLiteParameter(":DLL_NAME", reservationRow.DLL_NAME))
            .Add(New SQLiteParameter(":BROADCAST_TYPE", reservationRow.BROADCAST_TYPE))
            .Add(New SQLiteParameter(":STATION", reservationRow.STATION))
            .Add(New SQLiteParameter(":STATION_NAME", reservationRow.STATION_NAME))
            .Add(New SQLiteParameter(":CHANNEL", reservationRow.CHANNEL))
            .Add(New SQLiteParameter(":SERVICE_ID", NVL(reservationRow.SERVICE_ID, reservationRow.IsSERVICE_IDNull, RETURN_TYPE.OBJECT)))
            .Add(New SQLiteParameter(":START_YYYYMMDDHHMM", reservationRow.START_YYYYMMDDHHMM))
            .Add(New SQLiteParameter(":END_YYYYMMDDHHMM", reservationRow.END_YYYYMMDDHHMM))
            .Add(New SQLiteParameter(":PROGRAM_TITLE", reservationRow.PROGRAM_TITLE))
            .Add(New SQLiteParameter(":PROGRAM_SUBTITLE", reservationRow.PROGRAM_SUBTITLE))
            .Add(New SQLiteParameter(":EXTEND", reservationRow.EXTEND))
            .Add(New SQLiteParameter(":PERFORMER", reservationRow.PERFORMER))
            .Add(New SQLiteParameter(":GENRE", reservationRow.GENRE))
            .Add(New SQLiteParameter(":SUBGENRE", reservationRow.SUBGENRE))
            .Add(New SQLiteParameter(":NOTE", reservationRow.NOTE))
            .Add(New SQLiteParameter(":OUTPUT_ERRORLOG", reservationRow.OUTPUT_ERRORLOG))
            .Add(New SQLiteParameter(":EXIT_APPLICATION_AFTER_RECORDING", reservationRow.EXIT_APPLICATION_AFTER_RECORDING))
            .Add(New SQLiteParameter(":PREVIEW", reservationRow.PREVIEW))
            .Add(New SQLiteParameter(":SUSPEND_AFTER_RECORDING", reservationRow.SUSPEND_AFTER_RECORDING))
            .Add(New SQLiteParameter(":RECORDED", reservationRow.RECORDED))
            .Add(New SQLiteParameter(":PATH", reservationRow.PATH))
            .Add(New SQLiteParameter(":CLIENT_PC_NAME", reservationRow.CLIENT_PC_NAME))
        End With
        'SQLを実行
        sqliteCmd.ExecuteNonQuery()
    End Sub
#End Region

#Region "予約情報更新"
    ''' <summary>
    ''' 予約情報更新
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <remarks></remarks>
    Public Sub UpdateReservation(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow)
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("UPDATE")
            .AppendLine("     T_RESERVATION")
            .AppendLine("SET")
            .AppendLine("     DLL_NAME = :DLL_NAME")
            .AppendLine("    ,BROADCAST_TYPE = :BROADCAST_TYPE")
            .AppendLine("    ,STATION = :STATION")
            .AppendLine("    ,STATION_NAME = :STATION_NAME")
            .AppendLine("    ,CHANNEL = :CHANNEL")
            .AppendLine("    ,SERVICE_ID = :SERVICE_ID")
            .AppendLine("    ,START_YYYYMMDDHHMM = :START_YYYYMMDDHHMM")
            .AppendLine("    ,END_YYYYMMDDHHMM = :END_YYYYMMDDHHMM")
            .AppendLine("    ,PROGRAM_TITLE = :PROGRAM_TITLE")
            .AppendLine("    ,PROGRAM_SUBTITLE = :PROGRAM_SUBTITLE")
            .AppendLine("    ,EXTEND = :EXTEND")
            .AppendLine("    ,PERFORMER = :PERFORMER")
            .AppendLine("    ,GENRE = :GENRE")
            .AppendLine("    ,SUBGENRE = :SUBGENRE")
            .AppendLine("    ,NOTE = :NOTE")
            .AppendLine("    ,OUTPUT_ERRORLOG = :OUTPUT_ERRORLOG")
            .AppendLine("    ,EXIT_APPLICATION_AFTER_RECORDING = :EXIT_APPLICATION_AFTER_RECORDING")
            .AppendLine("    ,PREVIEW = :PREVIEW")
            .AppendLine("    ,SUSPEND_AFTER_RECORDING = :SUSPEND_AFTER_RECORDING")
            .AppendLine("    ,RECORDED = :RECORDED")
            .AppendLine("    ,PATH = :PATH")
            .AppendLine("    ,CLIENT_PC_NAME = :CLIENT_PC_NAME ")
            .AppendLine("WHERE")
            .AppendLine("     ID = :ID")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        With Me.sqliteCmd.Parameters
            .Clear()
            .Add(New SQLiteParameter(":DLL_NAME", reservationRow.DLL_NAME))
            .Add(New SQLiteParameter(":BROADCAST_TYPE", reservationRow.BROADCAST_TYPE))
            .Add(New SQLiteParameter(":STATION", reservationRow.STATION))
            .Add(New SQLiteParameter(":STATION_NAME", reservationRow.STATION_NAME))
            .Add(New SQLiteParameter(":CHANNEL", reservationRow.CHANNEL))
            .Add(New SQLiteParameter(":SERVICE_ID", NVL(reservationRow.SERVICE_ID, reservationRow.IsSERVICE_IDNull, RETURN_TYPE.OBJECT)))
            .Add(New SQLiteParameter(":START_YYYYMMDDHHMM", reservationRow.START_YYYYMMDDHHMM))
            .Add(New SQLiteParameter(":END_YYYYMMDDHHMM", reservationRow.END_YYYYMMDDHHMM))
            .Add(New SQLiteParameter(":PROGRAM_TITLE", reservationRow.PROGRAM_TITLE))
            .Add(New SQLiteParameter(":PROGRAM_SUBTITLE", reservationRow.PROGRAM_SUBTITLE))
            .Add(New SQLiteParameter(":EXTEND", reservationRow.EXTEND))
            .Add(New SQLiteParameter(":PERFORMER", reservationRow.PERFORMER))
            .Add(New SQLiteParameter(":GENRE", reservationRow.GENRE))
            .Add(New SQLiteParameter(":SUBGENRE", reservationRow.SUBGENRE))
            .Add(New SQLiteParameter(":NOTE", reservationRow.NOTE))
            .Add(New SQLiteParameter(":OUTPUT_ERRORLOG", reservationRow.OUTPUT_ERRORLOG))
            .Add(New SQLiteParameter(":EXIT_APPLICATION_AFTER_RECORDING", reservationRow.EXIT_APPLICATION_AFTER_RECORDING))
            .Add(New SQLiteParameter(":PREVIEW", reservationRow.PREVIEW))
            .Add(New SQLiteParameter(":SUSPEND_AFTER_RECORDING", reservationRow.SUSPEND_AFTER_RECORDING))
            .Add(New SQLiteParameter(":RECORDED", reservationRow.RECORDED))
            .Add(New SQLiteParameter(":PATH", reservationRow.PATH))
            .Add(New SQLiteParameter(":CLIENT_PC_NAME", reservationRow.CLIENT_PC_NAME))
            .Add(New SQLiteParameter(":ID", reservationRow.ID))
        End With
        'SQLを実行
        sqliteCmd.ExecuteNonQuery()
    End Sub
#End Region

#Region "録画中更新処理"
    ''' <summary>
    ''' 録画中更新処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateReservationRecording(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow)
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("UPDATE ")
            .AppendLine("     T_RESERVATION ")
            .AppendLine("SET ")
            .AppendLine("     RECORDED = 2 ")
            .AppendLine("    ,PATH = :PATH ")
            .AppendLine("WHERE ")
            .AppendLine("     ID = :ID ")
        End With
        With Me.sqliteCmd
            .CommandText = sqlBuilder.ToString
            With .Parameters
                .Clear()
                .Add(New SQLiteParameter(":PATH", reservationRow.PATH))
                .Add(New SQLiteParameter(":ID", reservationRow.ID))
            End With
            .ExecuteNonQuery()
        End With
    End Sub
#End Region

#Region "録画予約済更新処理"
    ''' <summary>
    ''' 録画予約済更新処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateReservationRecorded(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow)
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("UPDATE ")
            .AppendLine("     T_RESERVATION ")
            .AppendLine("SET ")
            .AppendLine("     RECORDED = 1 ")
            .AppendLine("WHERE ")
            .AppendLine("     ID = :ID ")
        End With
        With Me.sqliteCmd
            .CommandText = sqlBuilder.ToString
            With .Parameters
                .Clear()
                .Add(New SQLiteParameter(":ID", reservationRow.ID))
            End With
            .ExecuteNonQuery()
        End With
    End Sub
#End Region

#Region "録画中予約情報録画済更新"
    ''' <summary>
    ''' 録画中予約情報録画済更新
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdateReservationRecorded()
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("UPDATE ")
            .AppendLine("     T_RESERVATION ")
            .AppendLine("SET ")
            .AppendLine("     RECORDED = 1 ")
            .AppendLine("WHERE ")
            .AppendLine("     RECORDED = 2 ")
        End With
        With Me.sqliteCmd
            .CommandText = sqlBuilder.ToString
            .ExecuteNonQuery()
        End With
    End Sub
#End Region

#Region "録画予約済更新処理"
    ''' <summary>
    ''' 録画予約済更新処理
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub UpdatePastReservationRecorded()
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("UPDATE ")
            .AppendLine("     T_RESERVATION ")
            .AppendLine("SET ")
            .AppendLine("     RECORDED = 1 ")
            .AppendLine("WHERE ")
            .AppendLine("     RECORDED <> 1 ")
            .AppendLine(" AND END_YYYYMMDDHHMM < :NOW_YYYYMMDDHHMM ")
        End With
        With Me.sqliteCmd
            .CommandText = sqlBuilder.ToString
            With .Parameters
                .Clear()
                .Add(New SQLiteParameter(":NOW_YYYYMMDDHHMM", DateTime.Now.ToString(DATE_FORMAT)))
            End With
            .ExecuteNonQuery()
        End With
    End Sub
#End Region

#Region "番組情報削除"
    ''' <summary>
    ''' 番組情報削除
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub DeleteReservation(ByVal id As System.Int64)
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("DELETE ")
            .AppendLine("FROM ")
            .AppendLine("     T_RESERVATION ")
            .AppendLine("WHERE ")
            .AppendLine("     ID = :ID ")
        End With
        With Me.sqliteCmd
            .CommandText = sqlBuilder.ToString
            With .Parameters
                .Clear()
                .Add(New SQLiteParameter(":ID", id))
            End With
            .ExecuteNonQuery()
        End With
    End Sub
#End Region

#Region "ログ情報削除"
    ''' <summary>
    ''' ログ情報削除
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub DeleteLog(ByVal id As System.Int64)
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("DELETE ")
            .AppendLine("FROM ")
            .AppendLine("     T_LOG ")
            .AppendLine("WHERE ")
            .AppendLine("     ID = :ID ")
        End With
        With Me.sqliteCmd
            .CommandText = sqlBuilder.ToString
            With .Parameters
                .Clear()
                .Add(New SQLiteParameter(":ID", id))
            End With
            .ExecuteNonQuery()
        End With
    End Sub
#End Region

#Region "設定情報削除"
    ''' <summary>
    ''' 設定情報削除
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub DeleteSetting()
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("DELETE ")
            .AppendLine("FROM ")
            .AppendLine("     M_SETTING ")
        End With
        With Me.sqliteCmd
            .CommandText = sqlBuilder.ToString
            .ExecuteNonQuery()
        End With
    End Sub
#End Region

#Region "設定情報情報登録"
    ''' <summary>
    ''' 設定情報登録
    ''' </summary>
    ''' <param name="settingRow"></param>
    ''' <remarks></remarks>
    Public Sub InsertSetting(ByVal settingRow As SchemaRecordingReservation.M_SETTINGRow)
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("INSERT INTO ")
            .AppendLine("     M_SETTING ")
            .AppendLine("    (POWER_ON_INTERVAL_SECONDS ")
            .AppendLine("    ,EXE_BOOT_INTERVAL_SECONDS ")
            .AppendLine("    ,START_INTERVAL_SECONDS ")
            .AppendLine("    ,CHECK_START_INTERVAL_MINUTES ")
            .AppendLine("    ,OUTPUT_ERRORLOG ")
            .AppendLine("    ,EXIT_APPLICATION_AFTER_RECORDING ")
            .AppendLine("    ,PREVIEW ")
            .AppendLine("    ,SUSPEND_AFTER_RECORDING ")
            .AppendLine("    ,TVTEST_FULL_PATH ")
            .AppendLine("    ,SAVE_PATH) ")
            .AppendLine("VALUES ")
            .AppendLine("    (:POWER_ON_INTERVAL_SECONDS ")
            .AppendLine("    ,:EXE_BOOT_INTERVAL_SECONDS ")
            .AppendLine("    ,:START_INTERVAL_SECONDS ")
            .AppendLine("    ,:CHECK_START_INTERVAL_MINUTES ")
            .AppendLine("    ,:OUTPUT_ERRORLOG ")
            .AppendLine("    ,:EXIT_APPLICATION_AFTER_RECORDING ")
            .AppendLine("    ,:PREVIEW ")
            .AppendLine("    ,:SUSPEND_AFTER_RECORDING ")
            .AppendLine("    ,:TVTEST_FULL_PATH ")
            .AppendLine("    ,:SAVE_PATH) ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        With Me.sqliteCmd.Parameters
            .Clear()
            .Add(New SQLiteParameter(":POWER_ON_INTERVAL_SECONDS", settingRow.POWER_ON_INTERVAL_SECONDS))
            .Add(New SQLiteParameter(":EXE_BOOT_INTERVAL_SECONDS", settingRow.EXE_BOOT_INTERVAL_SECONDS))
            .Add(New SQLiteParameter(":START_INTERVAL_SECONDS", settingRow.START_INTERVAL_SECONDS))
            .Add(New SQLiteParameter(":CHECK_START_INTERVAL_MINUTES", settingRow.CHECK_START_INTERVAL_MINUTES))
            .Add(New SQLiteParameter(":OUTPUT_ERRORLOG", settingRow.OUTPUT_ERRORLOG))
            .Add(New SQLiteParameter(":EXIT_APPLICATION_AFTER_RECORDING", settingRow.EXIT_APPLICATION_AFTER_RECORDING))
            .Add(New SQLiteParameter(":PREVIEW", settingRow.PREVIEW))
            .Add(New SQLiteParameter(":SUSPEND_AFTER_RECORDING", settingRow.SUSPEND_AFTER_RECORDING))
            .Add(New SQLiteParameter(":TVTEST_FULL_PATH", settingRow.TVTEST_FULL_PATH))
            .Add(New SQLiteParameter(":SAVE_PATH", settingRow.SAVE_PATH))
        End With
        'SQLを実行
        sqliteCmd.ExecuteNonQuery()
    End Sub
#End Region

#Region "チャンネル情報登録"
    ''' <summary>
    ''' 設定情報登録
    ''' </summary>
    ''' <param name="stationRow"></param>
    ''' <remarks></remarks>
    Public Sub InsertStation(ByVal stationRow As SchemaRecordingReservation.M_STATIONRow)
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("INSERT INTO ")
            .AppendLine("     M_STATION ")
            .AppendLine("    (STATION ")
            .AppendLine("    ,CHANNEL ")
            .AppendLine("    ,SERVICE_ID) ")
            .AppendLine("VALUES ")
            .AppendLine("    (:STATION ")
            .AppendLine("    ,:CHANNEL ")
            .AppendLine("    ,:SERVICE_ID) ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        With Me.sqliteCmd.Parameters
            .Clear()
            .Add(New SQLiteParameter(":STATION", stationRow.STATION))
            .Add(New SQLiteParameter(":CHANNEL", stationRow.CHANNEL))
            .Add(New SQLiteParameter(":SERVICE_ID", NVL(stationRow.SERVICE_ID, stationRow.IsSERVICE_IDNull, RETURN_TYPE.OBJECT)))
        End With
        'SQLを実行
        sqliteCmd.ExecuteNonQuery()
    End Sub
#End Region

#Region "チャンネル情報削除"
    ''' <summary>
    ''' 設定情報登録
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub DeleteStation()
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("DELETE FROM ")
            .AppendLine("     M_STATION ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        'SQLを実行
        sqliteCmd.ExecuteNonQuery()
    End Sub
#End Region

#Region "デバイス情報登録"
    ''' <summary>
    ''' デバイス情報登録
    ''' </summary>
    ''' <param name="broadcastType"></param>
    ''' <param name="dllName"></param>
    ''' <remarks></remarks>
    Public Sub InsertDevice(ByVal broadcastType As BROADCAST_TYPE, ByVal dllName As String, ByVal priority As Int64)
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("INSERT INTO ")
            .AppendLine("     M_DEVICE ")
            .AppendLine("    (DLL_NAME ")
            .AppendLine("    ,BROADCAST_TYPE ")
            .AppendLine("    ,PRIORITY) ")
            .AppendLine("VALUES ")
            .AppendLine("    (:DLL_NAME ")
            .AppendLine("    ,:BROADCAST_TYPE ")
            .AppendLine("    ,:PRIORITY) ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        With Me.sqliteCmd.Parameters
            .Clear()
            .Add(New SQLiteParameter(":DLL_NAME", dllName))
            .Add(New SQLiteParameter(":BROADCAST_TYPE", broadcastType))
            .Add(New SQLiteParameter(":PRIORITY", priority))
        End With
        'SQLを実行
        sqliteCmd.ExecuteNonQuery()
    End Sub
#End Region

#Region "デバイス情報削除"
    ''' <summary>
    ''' デバイス情報削除
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub DeleteDevice()
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("DELETE FROM ")
            .AppendLine("     M_DEVICE ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        'SQLを実行
        sqliteCmd.ExecuteNonQuery()
    End Sub
#End Region

#Region "ログ出力処理"
    ''' <summary>
    ''' ログ出力処理
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <param name="message"></param>
    ''' <remarks></remarks>
    Public Sub InsertLog(ByVal reservationRow As SchemaRecordingReservation.T_RESERVATIONRow, ByVal message As String)
        Me.BeginTran()
        Try
            Dim sqlBuilder As New StringBuilder
            With sqlBuilder
                .AppendLine("INSERT INTO ")
                .AppendLine("     T_LOG ")
                .AppendLine("    (ID ")
                .AppendLine("    ,MESSAGE) ")
                .AppendLine("VALUES ")
                .AppendLine("    (:ID ")
                .AppendLine("    ,:MESSAGE) ")
            End With
            Me.sqliteCmd.CommandText = sqlBuilder.ToString
            With Me.sqliteCmd.Parameters
                .Clear()
                .Add(New SQLiteParameter(":ID", reservationRow.ID))
                .Add(New SQLiteParameter(":MESSAGE", message))
            End With
            'SQLを実行
            sqliteCmd.ExecuteNonQuery()
        Finally
            Me.Commit()
        End Try
    End Sub
#End Region

#Region "ログ出力処理（録画処理中に録画予約マネージャを終了）"
    ''' <summary>
    ''' ログ出力処理（録画処理中に録画予約マネージャを終了）
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub InsertLog()
        Dim sqlBuilder As New StringBuilder
        With sqlBuilder
            .AppendLine("INSERT INTO ")
            .AppendLine("     T_LOG ")
            .AppendLine("    (ID ")
            .AppendLine("    ,MESSAGE) ")
            .AppendLine("SELECT ")
            .AppendLine("     ID ")
            .AppendLine("    ,PROGRAM_TITLE || ' ' || :NOW_YYYYMMDDHHMM || ': 録画処理中に録画予約マネージャが終了しました。' ")
            .AppendLine("FROM ")
            .AppendLine("     T_RESERVATION ")
            .AppendLine("WHERE ")
            .AppendLine("     RECORDED = 2 ")
        End With
        Me.sqliteCmd.CommandText = sqlBuilder.ToString
        With Me.sqliteCmd.Parameters
            .Clear()
            .Add(New SQLiteParameter(":NOW_YYYYMMDDHHMM", DateTime.Now.ToString(DATE_DISP_FORMAT)))
        End With
        'SQLを実行
        sqliteCmd.ExecuteNonQuery()
    End Sub
#End Region

#Region "拡張子関連付け"
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

#Region "拡張子関連付け削除"
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

#Region "BonDriver*.dllの取得"
    ''' <summary>
    ''' DriverDllの取得
    ''' </summary>
    ''' <param name="tvTestFileFullPath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDllFiles(ByVal tvTestFileFullPath As String) As String()
        Dim dllNameList As New List(Of String)
        If System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(tvTestFileFullPath)) Then
            For Each fileName As String In System.IO.Directory.GetFiles( _
            System.IO.Path.GetDirectoryName(tvTestFileFullPath), "BonDriver*.dll")
                Dim dllName As String = System.IO.Path.GetFileName(fileName)
                dllNameList.Add(dllName)
            Next
        End If
        Return dllNameList.ToArray()
    End Function
#End Region

#End Region

#Region "内部メソッド"

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

#End Region

End Class
