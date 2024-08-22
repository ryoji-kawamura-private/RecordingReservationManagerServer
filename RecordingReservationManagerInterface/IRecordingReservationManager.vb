Imports System.ServiceModel
Imports RK.SchemaRecordingReservation

<ServiceContract()> _
Public Interface IRecordingReservationManager

    ''' <summary>
    ''' 設定情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()> _
    Function GetSetting() As M_SETTINGDataTable

    ''' <summary>
    ''' 設定情報取得
    ''' </summary>
    ''' <param name="settingTable"></param>
    ''' <param name="terrestrialDevice"></param>
    ''' <param name="bS110CSDevice"></param>
    ''' <remarks></remarks>
    <OperationContract()> _
    Sub GetSettings(ByRef settingTable As M_SETTINGDataTable, _
                    ByRef terrestrialDevice As M_DEVICEDataTable, _
                    ByRef bS110CSDevice As M_DEVICEDataTable)

    ''' <summary>
    ''' 予約情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()> _
    Function GetReservation() As T_RESERVATIONDataTable

    ''' <summary>
    ''' 録画一覧取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()> _
    Function GetRecordedList() As T_RESERVATIONDataTable

    ''' <summary>
    ''' 録画一覧取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()> _
    Function GetRecordedEnumerableList() As IEnumerable(Of T_RESERVATIONRowSerializer)

    ''' <summary>
    '''ログ取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()> _
    Function GetLog(ByVal reservationRowSerializer As T_RESERVATIONRowSerializer) As T_LOGDataTable

    ''' <summary>
    ''' 録画予約
    ''' </summary>
    ''' <param name="reservationRowSerializer"></param>
    ''' <remarks></remarks>
    <OperationContract()> _
    Function SetRecordingReservation(ByVal reservationRowSerializer As T_RESERVATIONRowSerializer, ByVal force As Boolean) As Result

    ''' <summary>
    ''' 録画予約更新
    ''' </summary>
    ''' <param name="reservationRowSerializer"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()> _
    Function UpdateRecordingReservation(ByVal reservationRowSerializer As T_RESERVATIONRowSerializer) As Result

    ''' <summary>
    ''' 録画予約削除
    ''' </summary>
    ''' <param name="reservationTable"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()> _
    Function DeleteRecordingReservation(ByVal reservationTable As T_RESERVATIONDataTable) As Result

    ''' <summary>
    ''' ログ削除
    ''' </summary>
    ''' <param name="id"></param>
    ''' <remarks></remarks>
    <OperationContract()> _
    Function DeleteLog(ByVal id As System.Int64) As Result

    ''' <summary>
    ''' 放送局情報取得
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()> _
    Function GetStationList() As M_STATIONDataTable

    ''' <summary>
    ''' チャンネル情報更新
    ''' </summary>
    ''' <param name="stationRowSerializer"></param>
    ''' <remarks></remarks>
    <OperationContract()> _
    Sub InsertStation(ByVal stationRowSerializer As M_STATIONRowSerializer)

    ''' <summary>
    ''' チャンネル情報更新
    ''' </summary>
    ''' <param name="stationList"></param>
    ''' <remarks></remarks>
    <OperationContract()> _
    Sub UpdateStation(ByVal stationList As M_STATIONDataTable)

    ''' <summary>
    ''' 設定情報更新
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()> _
    Function UpdateSetting(ByVal settingRowSerializer As M_SETTINGRowSerializer, _
                           ByVal terrestrialDevice As M_DEVICEDataTable, _
                           ByVal bS110CSDevice As M_DEVICEDataTable) As Result

    ''' <summary>
    ''' Dll名取得
    ''' </summary>
    ''' <param name="tvTestFileFullPath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    <OperationContract()> _
    Function GetDllNames(ByVal tvTestFileFullPath As String) As String()

End Interface
