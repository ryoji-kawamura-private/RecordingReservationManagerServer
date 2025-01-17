﻿Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary
Imports RK.SchemaRecordingReservation

#Region " コンスタント値クラス "

''' <summary>
''' コンスタント値クラス
''' </summary>
''' <remarks></remarks>
Public Class ConstClass
    ''' <summary>日付表示フォーマット定数</summary>
    Public Const RECORING_RESERVATION_MANAGER As String = "RecordingReservationManager"
End Class

#End Region

#Region " 返却用クラス"

''' <summary>
''' 返却用クラス
''' </summary>
''' <remarks></remarks>
<DataContract()> _
Public Class Result
    ''' <summary>
    ''' 返却値
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum RESULT_VALUE As Int32
        SUCCESS
        [ERROR]
        PAST_PROGRAM_ERROR
        UNREGIST_CHANNEL_ERROR
        ALREADY_REGIST_RESERVATION
        NOT_EXISTS_DEVICE_ERROR
        DUPLICATE_PROGRAM_WARNING
    End Enum
    ''' <summary>返却値</summary>
    Private _ResultValue As RESULT_VALUE
    ''' <summary>メッセージ</summary>
    Private _Message As String
    ''' <summary>返却値</summary>
    <DataMember()> _
    Public Property ResultValue() As RESULT_VALUE
        Get
            Return _ResultValue
        End Get
        Set(ByVal value As RESULT_VALUE)
            _ResultValue = value
        End Set
    End Property
    ''' <summary>メッセージ</summary>
    <DataMember()> _
    Public Property Message() As String
        Get
            Return _Message
        End Get
        Set(ByVal value As String)
            _Message = value
        End Set
    End Property
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()
    End Sub
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="resultValue"></param>
    ''' <param name="message"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal resultValue As RESULT_VALUE, ByVal message As String)
        Me.New()
        _ResultValue = resultValue
        _Message = message
    End Sub
End Class

#End Region

#Region " 設定データシリアライザ "

<DataContract()> _
Public Class M_SETTINGRowSerializer

#Region " 変数・定数 "
    ''' <summary>
    ''' 予約データ行
    ''' </summary>
    ''' <remarks></remarks>
    Private _SettingRow As M_SETTINGRow
#End Region

#Region " プロパティ "
    ''' <summary>
    ''' 予約データ行
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SettingRow() As M_SETTINGRow
        Get
            If Me._SettingRow Is Nothing Then
                Using settingTable As M_SETTINGDataTable = (New M_SETTINGDataTable)
                    _SettingRow = settingTable.NewM_SETTINGRow
                End Using
            End If
            Return Me._SettingRow
        End Get
    End Property
    <DataMember()> _
    Public Property POWER_ON_INTERVAL_SECONDS() As Long
        Get
            Return Me.SettingRow.POWER_ON_INTERVAL_SECONDS
        End Get
        Set(ByVal value As Long)
            Me.SettingRow.POWER_ON_INTERVAL_SECONDS = value
        End Set
    End Property
    <DataMember()> _
    Public Property EXE_BOOT_INTERVAL_SECONDS() As Long
        Get
            Return Me.SettingRow.EXE_BOOT_INTERVAL_SECONDS
        End Get
        Set(ByVal value As Long)
            Me.SettingRow.EXE_BOOT_INTERVAL_SECONDS = value
        End Set
    End Property
    <DataMember()> _
    Public Property START_INTERVAL_SECONDS() As Long
        Get
            Return Me.SettingRow.START_INTERVAL_SECONDS
        End Get
        Set(ByVal value As Long)
            Me.SettingRow.START_INTERVAL_SECONDS = value
        End Set
    End Property
    <DataMember()> _
    Public Property CHECK_START_INTERVAL_MINUTES() As Long
        Get
            Return Me.SettingRow.CHECK_START_INTERVAL_MINUTES
        End Get
        Set(ByVal value As Long)
            Me.SettingRow.CHECK_START_INTERVAL_MINUTES = value
        End Set
    End Property
    <DataMember()> _
    Public Property OUTPUT_ERRORLOG() As Long
        Get
            Return Me.SettingRow.OUTPUT_ERRORLOG
        End Get
        Set(ByVal value As Long)
            Me.SettingRow.OUTPUT_ERRORLOG = value
        End Set
    End Property
    <DataMember()> _
    Public Property EXIT_APPLICATION_AFTER_RECORDING() As Long
        Get
            Return Me.SettingRow.EXIT_APPLICATION_AFTER_RECORDING
        End Get
        Set(ByVal value As Long)
            Me.SettingRow.EXIT_APPLICATION_AFTER_RECORDING = value
        End Set
    End Property
    <DataMember()> _
    Public Property PREVIEW() As Long
        Get
            Return Me.SettingRow.PREVIEW
        End Get
        Set(ByVal value As Long)
            Me.SettingRow.PREVIEW = value
        End Set
    End Property
    <DataMember()> _
    Public Property SUSPEND_AFTER_RECORDING() As Long
        Get
            Return Me.SettingRow.SUSPEND_AFTER_RECORDING
        End Get
        Set(ByVal value As Long)
            Me.SettingRow.SUSPEND_AFTER_RECORDING = value
        End Set
    End Property
    <DataMember()> _
    Public Property TVTEST_FULL_PATH() As String
        Get
            Return Me.SettingRow.TVTEST_FULL_PATH
        End Get
        Set(ByVal value As String)
            Me.SettingRow.TVTEST_FULL_PATH = value
        End Set
    End Property
    <DataMember()> _
    Public Property SAVE_PATH() As String
        Get
            Return Me.SettingRow.SAVE_PATH
        End Get
        Set(ByVal value As String)
            Me.SettingRow.SAVE_PATH = value
        End Set
    End Property
#End Region

#Region " コンストラクタ "
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    Public Sub New()
        MyBase.New()
    End Sub
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="settingRow"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal settingRow As M_SETTINGRow)
        Me.New()
        _SettingRow = settingRow
    End Sub
#End Region

#Region " 外部メソッド "

#End Region

End Class

#End Region

#Region " 録画予約データシリアライザ "

<DataContract()> _
Public Class T_RESERVATIONRowSerializer

#Region " 変数・定数 "
    ''' <summary>
    ''' 予約データ行
    ''' </summary>
    ''' <remarks></remarks>
    Private _ReservationRow As T_RESERVATIONRow
#End Region

#Region " プロパティ "
    ''' <summary>
    ''' 予約データ行
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property ReservationRow() As T_RESERVATIONRow
        Get
            If Me._ReservationRow Is Nothing Then
                Using reservationTable As T_RESERVATIONDataTable = (New T_RESERVATIONDataTable)
                    _ReservationRow = reservationTable.NewT_RESERVATIONRow
                End Using
            End If
            Return Me._ReservationRow
        End Get
    End Property
    <DataMember()> _
    Public Property ID() As Int64
        Get
            Return Me.ReservationRow.ID
        End Get
        Set(ByVal value As Int64)
            Me.ReservationRow.ID = value
        End Set
    End Property
    <DataMember()> _
    Public Property DLL_NAME() As String
        Get
            Return Me.ReservationRow.DLL_NAME
        End Get
        Set(ByVal value As String)
            Me.ReservationRow.DLL_NAME = value
        End Set
    End Property
    <DataMember()> _
    Public Property BROADCAST_TYPE() As Int64
        Get
            Return Me.ReservationRow.BROADCAST_TYPE
        End Get
        Set(ByVal value As Int64)
            Me.ReservationRow.BROADCAST_TYPE = value
        End Set
    End Property
    <DataMember()> _
    Public Property STATION() As String
        Get
            Return Me.ReservationRow.STATION
        End Get
        Set(ByVal value As String)
            Me.ReservationRow.STATION = value
        End Set
    End Property
    <DataMember()> _
    Public Property STATION_NAME() As String
        Get
            Return Me.ReservationRow.STATION_NAME
        End Get
        Set(ByVal value As String)
            Me.ReservationRow.STATION_NAME = value
        End Set
    End Property
    <DataMember()> _
    Public Property CHANNEL() As Int64
        Get
            Return Me.ReservationRow.CHANNEL
        End Get
        Set(ByVal value As Int64)
            Me.ReservationRow.CHANNEL = value
        End Set
    End Property
    <DataMember()> _
    Public Property SERVICE_ID() As Int64
        Get
            Return Me.ReservationRow.SERVICE_ID
        End Get
        Set(ByVal value As Int64)
            Me.ReservationRow.SERVICE_ID = value
        End Set
    End Property
    <DataMember()> _
    Public Property START_YYYYMMDDHHMM() As String
        Get
            Return Me.ReservationRow.START_YYYYMMDDHHMM
        End Get
        Set(ByVal value As String)
            Me.ReservationRow.START_YYYYMMDDHHMM = value
        End Set
    End Property
    <DataMember()> _
    Public Property START_DISP() As String
        Get
            Return Me.ReservationRow.START_DISP
        End Get
        Set(ByVal value As String)
            Me.ReservationRow.START_DISP = value
        End Set
    End Property
    <DataMember()> _
    Public Property END_YYYYMMDDHHMM() As String
        Get
            Return Me.ReservationRow.END_YYYYMMDDHHMM
        End Get
        Set(ByVal value As String)
            Me.ReservationRow.END_YYYYMMDDHHMM = value
        End Set
    End Property
    <DataMember()> _
    Public Property END_DISP() As String
        Get
            Return Me.ReservationRow.END_DISP
        End Get
        Set(ByVal value As String)
            Me.ReservationRow.END_DISP = value
        End Set
    End Property
    <DataMember()> _
    Public Property PROGRAM_TITLE() As String
        Get
            Return Me.ReservationRow.PROGRAM_TITLE
        End Get
        Set(ByVal value As String)
            Me.ReservationRow.PROGRAM_TITLE = value
        End Set
    End Property
    <DataMember()> _
    Public Property PROGRAM_SUBTITLE() As String
        Get
            Return Me.ReservationRow.PROGRAM_SUBTITLE
        End Get
        Set(ByVal value As String)
            Me.ReservationRow.PROGRAM_SUBTITLE = value
        End Set
    End Property
    <DataMember()> _
    Public Property EXTEND() As Int64
        Get
            Return Me.ReservationRow.EXTEND
        End Get
        Set(ByVal value As Int64)
            Me.ReservationRow.EXTEND = value
        End Set
    End Property
    <DataMember()> _
    Public Property PERFORMER() As String
        Get
            Return Me.ReservationRow.PERFORMER
        End Get
        Set(ByVal value As String)
            Me.ReservationRow.PERFORMER = value
        End Set
    End Property
    <DataMember()> _
    Public Property GENRE() As Int64
        Get
            Return Me.ReservationRow.GENRE
        End Get
        Set(ByVal value As Int64)
            Me.ReservationRow.GENRE = value
        End Set
    End Property
    <DataMember()> _
    Public Property SUBGENRE() As Int64
        Get
            Return Me.ReservationRow.SUBGENRE
        End Get
        Set(ByVal value As Int64)
            Me.ReservationRow.SUBGENRE = value
        End Set
    End Property
    <DataMember()> _
    Public Property NOTE() As String
        Get
            Return Me.ReservationRow.NOTE
        End Get
        Set(ByVal value As String)
            Me.ReservationRow.NOTE = value
        End Set
    End Property
    <DataMember()> _
    Public Property RECORDED() As Int64
        Get
            Return Me.ReservationRow.RECORDED
        End Get
        Set(ByVal value As Int64)
            Me.ReservationRow.RECORDED = value
        End Set
    End Property
    <DataMember()> _
    Public Property OUTPUT_ERRORLOG() As Int64
        Get
            Return Me.ReservationRow.OUTPUT_ERRORLOG
        End Get
        Set(ByVal value As Int64)
            Me.ReservationRow.OUTPUT_ERRORLOG = value
        End Set
    End Property
    <DataMember()> _
    Public Property EXIT_APPLICATION_AFTER_RECORDING() As Int64
        Get
            Return Me.ReservationRow.EXIT_APPLICATION_AFTER_RECORDING
        End Get
        Set(ByVal value As Int64)
            Me.ReservationRow.EXIT_APPLICATION_AFTER_RECORDING = value
        End Set
    End Property
    <DataMember()> _
    Public Property PREVIEW() As Int64
        Get
            Return Me.ReservationRow.PREVIEW
        End Get
        Set(ByVal value As Int64)
            Me.ReservationRow.PREVIEW = value
        End Set
    End Property
    <DataMember()> _
    Public Property SUSPEND_AFTER_RECORDING() As Int64
        Get
            Return Me.ReservationRow.SUSPEND_AFTER_RECORDING
        End Get
        Set(ByVal value As Int64)
            Me.ReservationRow.SUSPEND_AFTER_RECORDING = value
        End Set
    End Property
    <DataMember()> _
    Public Property PATH() As String
        Get
            Return Me.ReservationRow.PATH
        End Get
        Set(ByVal value As String)
            Me.ReservationRow.PATH = value
        End Set
    End Property
    <DataMember()> _
    Public Property CLIENT_PC_NAME() As String
        Get
            Return Me.ReservationRow.CLIENT_PC_NAME
        End Get
        Set(ByVal value As String)
            Me.ReservationRow.CLIENT_PC_NAME = value
        End Set
    End Property
#End Region

#Region " コンストラクタ "
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>

    Public Sub New()
        MyBase.New()
    End Sub
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="reservationRow"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal reservationRow As T_RESERVATIONRow)
        Me.New()
        Me._ReservationRow = reservationRow
    End Sub
#End Region

End Class

#End Region

#Region " 放送局データシリアライザ "

<DataContract()> _
Public Class M_STATIONRowSerializer

#Region " 変数・定数 "
    ''' <summary>
    ''' 予約データ行
    ''' </summary>
    ''' <remarks></remarks>
    Private _StationRow As M_STATIONRow
#End Region

#Region " プロパティ "
    ''' <summary>
    ''' 予約データ行
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property StationRow() As M_STATIONRow
        Get
            Using stationTable As M_STATIONDataTable = (New M_STATIONDataTable)
                Me._StationRow = stationTable.NewM_STATIONRow
                Me._StationRow.CHANNEL = Me.CHANNEL
                Me._StationRow.SERVICE_ID = Me.SERVICE_ID
                Me._StationRow.STATION = Me.STATION
            End Using
            Return Me._StationRow
        End Get
    End Property
    Private _CHANNEL As Long
    <DataMember()> _
    Public Property CHANNEL() As Long
        Get
            Return Me._CHANNEL
        End Get
        Set(ByVal value As Long)
            Me._CHANNEL = value
        End Set
    End Property
    Private _SERVICE_ID As Long
    <DataMember()> _
    Public Property SERVICE_ID() As Long
        Get
            Return Me._SERVICE_ID
        End Get
        Set(ByVal value As Long)
            Me._SERVICE_ID = value
        End Set
    End Property
    Private _STATION As String
    <DataMember()> _
    Public Property STATION() As String
        Get
            Return Me._STATION
        End Get
        Set(ByVal value As String)
            Me._STATION = value
        End Set
    End Property
#End Region

#Region " コンストラクタ "
    ''' <summary>
    ''' コンストラクタ（シリアライズ）
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        MyBase.New()
    End Sub
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="stationRow"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal stationRow As M_STATIONRow)
        Me.New()
        Me._StationRow = stationRow
        Me.CHANNEL = Me._StationRow.CHANNEL
        Me.SERVICE_ID = Me._StationRow.SERVICE_ID
        Me.STATION = Me._StationRow.STATION
    End Sub
#End Region

End Class

#End Region
