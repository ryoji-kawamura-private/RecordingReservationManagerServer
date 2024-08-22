Imports System
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Text

#Region "ベースセル"

''' <summary>
''' BaseTextBoxで編集できるテキスト情報を
''' DataGridViewコントロールに表示します。
''' </summary>
Public MustInherit Class BaseTextBoxCell : Inherits DataGridViewTextBoxCell

#Region "メンバ変数"
    ''' <summary>選択時背景色</summary>
    Protected mSelectionBackColor As Color = Color.LightSteelBlue
    ''' <summary>検索対象項目是非</summary>
    Protected mCanSearch As Boolean = False
    ''' <summary>選択時背景色（検索対象項目）</summary>
    Protected mSearchBackColor As Color = Color.LightPink
    ''' <summary>入力可能最大長</summary>
    Protected mMaxLenB As Int16 = 20
    ''' <summary>背景色</summary>
    Protected mBaseBackColor As Color
    ''' <summary>半角英字入力可否</summary>
    Protected mInputAlpha As Boolean = True
    ''' <summary>半角数字入力可否</summary>
    Protected mInputNumeric As Boolean = True
    ''' <summary>半角カナ入力可否</summary>
    Protected mInputKana As Boolean = True
    ''' <summary>半角その他入力可否</summary>
    Protected mInputEtc As Boolean = True
    ''' <summary>全角入力可否</summary>
    Protected mInputZenkaku As Boolean = True
    ''' <summary>入力可能文字</summary>
    Protected mInputOk As String = ""
    ''' <summary>入力不可文字</summary>
    Protected mInputNg As String = ""
    ''' <summary>自動サイズ調整</summary>
    Protected mAutoResize As Boolean = True
    ''' <summary>自動フォーマット</summary>
    Protected mAutoFormat As Boolean = True
#End Region

#Region "プロパティ"
    ''' <summary>
    ''' 選択時背景色
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SelectionBackColor() As Color
        Get
            Return mSelectionBackColor
        End Get
        Set(ByVal Value As Color)
            mSelectionBackColor = Value
        End Set
    End Property
    ''' <summary>
    ''' 検索対象項目是非
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CanSearch() As Boolean
        Get
            Return mCanSearch
        End Get
        Set(ByVal Value As Boolean)
            mCanSearch = Value
        End Set
    End Property
    ''' <summary>
    ''' 選択時背景色（検索対象項目）
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SearchBackColor() As Color
        Get
            Return mSearchBackColor
        End Get
        Set(ByVal Value As Color)
            mSearchBackColor = Value
        End Set
    End Property
    ''' <summary>
    ''' テキスト最大バイト長
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MaxLengthByte() As Int16
        Get
            Return mMaxLenB
        End Get
        Set(ByVal Value As Int16)
            mMaxLenB = Value
        End Set
    End Property
    ''' <summary>
    ''' 半角英字入力可否
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InputAlpha() As Boolean
        Get
            Return mInputAlpha
        End Get
        Set(ByVal Value As Boolean)
            mInputAlpha = Value
        End Set
    End Property
    ''' <summary>
    ''' 半角数字入力可否
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InputNumeric() As Boolean
        Get
            Return mInputNumeric
        End Get
        Set(ByVal Value As Boolean)
            mInputNumeric = Value
        End Set
    End Property
    ''' <summary>
    ''' 半角カナ入力可否
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InputKana() As Boolean
        Get
            Return mInputKana
        End Get
        Set(ByVal Value As Boolean)
            mInputKana = Value
        End Set
    End Property
    ''' <summary>
    ''' 半角その他記号等入力可否
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InputEtc() As Boolean
        Get
            Return mInputEtc
        End Get
        Set(ByVal Value As Boolean)
            mInputEtc = Value
        End Set
    End Property
    ''' <summary>
    ''' 全角文字入力可否
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InputZenkaku() As Boolean
        Get
            Return mInputZenkaku
        End Get
        Set(ByVal Value As Boolean)
            mInputZenkaku = Value
        End Set
    End Property
    ''' <summary>
    ''' 入力可能文字
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InputOk() As String
        Get
            Return mInputOk
        End Get
        Set(ByVal Value As String)
            mInputOk = Value
        End Set
    End Property
    ''' <summary>
    ''' 入力不可文字
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InputNg() As String
        Get
            Return mInputNg
        End Get
        Set(ByVal Value As String)
            mInputNg = Value
        End Set
    End Property
    ''' <summary>
    ''' 入力可能なバイト長に合わせて自動で幅調整を行なうかを設定します。
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AutoResize() As Boolean
        Get
            Return mAutoResize
        End Get
        Set(ByVal Value As Boolean)
            mAutoResize = Value
        End Set
    End Property
    ''' <summary>
    ''' 入力可能なバイト長に合わせて自動で幅調整を行なうかを設定します。
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AutoFormat() As Boolean
        Get
            Return mAutoFormat
        End Get
        Set(ByVal Value As Boolean)
            mAutoFormat = Value
        End Set
    End Property
#End Region

End Class

#End Region
