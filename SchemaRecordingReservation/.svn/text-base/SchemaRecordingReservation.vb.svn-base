Imports System.IO
Imports System.Runtime.Serialization
Imports System.Runtime.Serialization.Formatters.Binary

Partial Public Class SchemaRecordingReservation

#Region " 定数 "
    ''' <summary>日付フォーマット定数</summary>
    Public Const DATE_FORMAT As String = "yyyyMMddHHmm"
    ''' <summary>日付表示フォーマット定数</summary>
    Public Const DATE_DISP_FORMAT As String = "yyyy/MM/dd HH:mm"
    ''' <summary>メッセージボックスタイトル</summary>
    Public Const MESSAGEBOX_TITLE As String = "録画予約"
    ''' <summary>録画実行ファイル引数</summary>
    Public Const [SET] As String = "SET"
    ''' <summary>録画実行ファイル引数</summary>
    Public Const UPDATE As String = "UPDATE"
    ''' <summary>録画実行ファイル引数</summary>
    Public Const DELETE As String = "DELETE"

    ''' <summary>ブロードキャストタイプ</summary>
    Public Enum BROADCAST_TYPE As Byte
        TERRESTRIAL
        BS110CS
    End Enum
    ''' <summary>デバイスタイプ</summary>
    Public Class DEVICE_TYPE
        Public Const FRIIO As String = "Friio"
        Public Const MONSTER_TV As String = "MonsterTV"
    End Class
    ''' <summary>NVL返却型</summary>
    Public Enum RETURN_TYPE As Byte
        [STRING]
        [INT32]
        [INT64]
        [OBJECT]
    End Enum
#End Region

#Region " NVL "
    ''' <summary>
    ''' NVL
    ''' </summary>
    ''' <param name="expression"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function NVL(ByVal expression As Object, ByVal isNull As Boolean, ByVal returnType As RETURN_TYPE) As Object
        Select Case isNull
            Case True
                Select Case returnType
                    Case RETURN_TYPE.STRING
                        Return String.Empty
                    Case RETURN_TYPE.INT32
                        Return 0I
                    Case RETURN_TYPE.INT64
                        Return 0L
                    Case RETURN_TYPE.OBJECT
                        Return System.Convert.DBNull
                    Case Else
                        Return Nothing
                End Select
            Case Else
                Return expression
        End Select
    End Function
#End Region

#Region " ToRoundUp "
    ''' <summary>
    ''' 切上
    ''' </summary>
    ''' <param name="dValue"></param>
    ''' <param name="iDigits"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ToRoundUp(ByVal dValue As Double, ByVal iDigits As Integer) As Double
        Dim dCoef As Double = System.Math.Pow(10, iDigits)

        If dValue > 0 Then
            Return System.Math.Ceiling(dValue * dCoef) / dCoef
        Else
            Return System.Math.Floor(dValue * dCoef) / dCoef
        End If
    End Function
#End Region

End Class
