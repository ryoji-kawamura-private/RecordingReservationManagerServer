Imports System
Imports System.Drawing
Imports System.Windows.Forms

#Region "数値編集セル"

''' <summary>
''' NumericTextBoxで編集できるテキスト情報を
''' DataGridViewコントロールに表示します。
''' </summary>
Public Class NumericTextBoxCell : Inherits BaseTextBoxCell

#Region "メンバ変数"
    ''' <summary>テキストに入力可能な最小値</summary>
    Protected mMinValue As Int64
    ''' <summary>テキストに入力可能な最大値</summary>
    Protected mMaxValue As Int64
    ''' <summary>テキストに入力可能な数値の精度</summary>
    Protected mAccuracy As Accuracy
    ''' <summary>テキストの区切り文字</summary>
    Protected mDelimiter As Char = ","c
    ''' <summary>マイナス値の入力可否設定</summary>
    Protected mInputMinus As Boolean = False
    ''' <summary>ゼロ値の表示可否設定</summary>
    Protected mZeroToBlank As Boolean = False
#End Region

#Region "プロパティ"
    ''' <summary>入力可能な最小の数値です。</summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MinValue() As Int64
        Get
            Return mMinValue
        End Get
        Set(ByVal Value As Int64)
            mMinValue = Value
        End Set
    End Property
    ''' <summary>入力可能な最大の数値です。</summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MaxValue() As Int64
        Get
            Return mMaxValue
        End Get
        Set(ByVal Value As Int64)
            mMaxValue = Value
        End Set
    End Property
    ''' <summary>数値の精度です。(全体の桁数, 小数部の桁数)</summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NumericAccuracy() As Accuracy
        Get
            Return mAccuracy
        End Get
        Set(ByVal Value As Accuracy)
            mAccuracy = Value
        End Set
    End Property
    ''' <summary>区切り文字です。</summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Delimiter() As Char
        Get
            Return mDelimiter
        End Get
        Set(ByVal Value As Char)
            mDelimiter = Value
        End Set
    End Property
    ''' <summary>マイナス値の入力可否設定です。</summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InputMinus() As Boolean
        Get
            Return mInputMinus
        End Get
        Set(ByVal Value As Boolean)
            mInputMinus = Value
        End Set
    End Property
    ''' <summary>ゼロ値の表示可否設定です。</summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ZeroToBlank() As Boolean
        Get
            Return mZeroToBlank
        End Get
        Set(ByVal Value As Boolean)
            mZeroToBlank = Value
        End Set
    End Property
#End Region

#Region "コンストラクタ"
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

    End Sub
#End Region

#Region "オーバーライドメソッド"
    ''' <summary>
    ''' 編集コントロールを初期化する
    ''' 編集コントロールは別のセルや列でも使いまわされるため、初期化の必要がある
    ''' </summary>
    ''' <param name="rowIndex"></param>
    ''' <param name="initialFormattedValue"></param>
    ''' <param name="dataGridViewCellStyle"></param>
    ''' <remarks></remarks>
    Public Overrides Sub InitializeEditingControl(ByVal rowIndex As Integer, _
                                                  ByVal initialFormattedValue As Object, _
                                                  ByVal dataGridViewCellStyle As DataGridViewCellStyle)
        MyBase.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle)
        '編集コントロールの取得
        Dim NumericTextBox As NumericTextBoxEditingControl = DirectCast(Me.DataGridView.EditingControl, NumericTextBoxEditingControl)
        If NumericTextBox IsNot Nothing Then
            With NumericTextBox
                Try
                    If Me.Value Is Nothing Then
                        .Text = String.Empty
                    Else
                        .Text = Me.Value.ToString
                    End If
                Catch ex As System.ArgumentOutOfRangeException
                    '//NOP
                End Try
                Dim cellTemp As NumericTextBoxCell = DirectCast(Me.DataGridView.Columns(Me.ColumnIndex).CellTemplate, NumericTextBoxCell)
                .SelectionBackColor = cellTemp.SelectionBackColor
                .CanSearch = cellTemp.CanSearch
                .SearchBackColor = cellTemp.SearchBackColor
                .MaxLengthByte = cellTemp.MaxLengthByte
                .InputAlpha = cellTemp.InputAlpha
                .InputNumeric = cellTemp.InputNumeric
                .InputKana = cellTemp.InputKana
                .InputEtc = cellTemp.InputEtc
                .InputZenkaku = cellTemp.InputZenkaku
                .InputOk = cellTemp.InputOk
                .InputNg = cellTemp.InputNg
                .AutoResize = cellTemp.AutoResize
                .AutoFormat = cellTemp.AutoFormat
                .MinValue = cellTemp.MinValue
                .MaxValue = cellTemp.MaxValue
                .NumericAccuracy = cellTemp.NumericAccuracy
                .Delimiter = cellTemp.Delimiter
                .InputMinus = cellTemp.InputMinus
                .ZeroToBlank = cellTemp.ZeroToBlank
            End With
        End If
    End Sub
    ''' <summary>
    ''' 編集コントロールの型を指定する
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides ReadOnly Property EditType() As Type
        Get
            Return GetType(NumericTextBoxEditingControl)
        End Get
    End Property
    ''' <summary>
    ''' 新しいレコード行のセルの既定値を指定する
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides ReadOnly Property DefaultNewRowValue() As Object
        Get
            Return MyBase.DefaultNewRowValue
        End Get
    End Property
#End Region

End Class

#End Region

#Region "数値データグリッド編集コントロール"

''' <summary>
''' NumericTextBoxCellでホストされる
''' NumericTextBoxコントロールを表します。
''' </summary>
Public Class NumericTextBoxEditingControl : Inherits NumericTextBox : Implements IDataGridViewEditingControl

#Region "メンバ変数"
    ''' <summary>編集コントロールが表示されているDataGridView</summary>
    Protected dataGridView As DataGridView
    ''' <summary>編集コントロールが表示されている行</summary>
    Protected rowIndex As Integer
    ''' <summary>編集コントロールの値とセルの値が違うかどうか</summary>
    Protected valueChanged As Boolean
#End Region

#Region "コンストラクタ"
    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
        Me.TabStop = False
    End Sub
#End Region

#Region "インターフェース実装"
    ''' <summary>
    ''' 編集コントロールで変更されたセルの値
    ''' </summary>
    ''' <param name="context"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetEditingControlFormattedValue(ByVal context As DataGridViewDataErrorContexts) As Object _
        Implements IDataGridViewEditingControl.GetEditingControlFormattedValue
        Return Me.Text
    End Function
    ''' <summary>
    ''' 編集コントロールで変更されたセルの値
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EditingControlFormattedValue() As Object _
        Implements IDataGridViewEditingControl.EditingControlFormattedValue
        Get
            Return Me.GetEditingControlFormattedValue(DataGridViewDataErrorContexts.Formatting)
        End Get
        Set(ByVal value As Object)
            Try
                Me.Text = Convert.ToString(value)
            Catch ex As Exception
                Me.Text = String.Empty
            End Try
        End Set
    End Property
    ''' <summary>
    ''' セルスタイルを編集コントロールに適用する編集コントロールの前景色、背景色、フォントなどをセルスタイルに合わせる
    ''' </summary>
    ''' <param name="dataGridViewCellStyle"></param>
    ''' <remarks></remarks>
    Public Sub ApplyCellStyleToEditingControl(ByVal dataGridViewCellStyle As DataGridViewCellStyle) _
        Implements IDataGridViewEditingControl.ApplyCellStyleToEditingControl

        Me.Font = dataGridViewCellStyle.Font
        Me.ForeColor = dataGridViewCellStyle.ForeColor
        Me.BackColor = dataGridViewCellStyle.BackColor
        Select Case dataGridViewCellStyle.Alignment
            Case DataGridViewContentAlignment.BottomCenter, _
                    DataGridViewContentAlignment.MiddleCenter, _
                    DataGridViewContentAlignment.TopCenter
                Me.TextAlign = HorizontalAlignment.Center
            Case DataGridViewContentAlignment.BottomRight, _
                    DataGridViewContentAlignment.MiddleRight, _
                    DataGridViewContentAlignment.TopRight
                Me.TextAlign = HorizontalAlignment.Right
            Case Else
                Me.TextAlign = HorizontalAlignment.Left
        End Select
    End Sub
    ''' <summary>
    ''' '編集するセルがあるDataGridView
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EditingControlDataGridView() As DataGridView _
        Implements IDataGridViewEditingControl.EditingControlDataGridView
        Get
            Return Me.dataGridView
        End Get
        Set(ByVal value As DataGridView)
            Me.dataGridView = value
        End Set
    End Property
    ''' <summary>
    ''' 編集している行のインデックス
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EditingControlRowIndex() As Integer _
        Implements IDataGridViewEditingControl.EditingControlRowIndex
        Get
            Return Me.rowIndex
        End Get
        Set(ByVal value As Integer)
            Me.rowIndex = value
        End Set
    End Property
    ''' <summary>
    ''' 値が変更されたかどうか編集コントロールの値とセルの値が違うかどうか
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property EditingControlValueChanged() As Boolean _
        Implements IDataGridViewEditingControl.EditingControlValueChanged
        Get
            Return Me.valueChanged
        End Get
        Set(ByVal value As Boolean)
            Me.valueChanged = value
        End Set
    End Property
    ''' <summary>
    ''' 指定されたキーをDataGridViewが処理するか、編集コントロールが処理するか
    ''' Trueを返すと、編集コントロールが処理する
    ''' dataGridViewWantsInputKeyがTrueの時は、DataGridViewが処理できる
    ''' </summary>
    ''' <param name="keyData"></param>
    ''' <param name="dataGridViewWantsInputKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EditingControlWantsInputKey(ByVal keyData As Keys, _
        ByVal dataGridViewWantsInputKey As Boolean) As Boolean _
        Implements IDataGridViewEditingControl.EditingControlWantsInputKey
        'Keys.Left、Right、Home、Endの時は、Trueを返す
        'このようにしないと、これらのキーで別のセルにフォーカスが移ってしまう
        Select Case keyData And Keys.KeyCode
            Case Keys.Right, Keys.End, Keys.Left, Keys.Home
                Return True
            Case Else
                Return False
        End Select
    End Function
    ''' <summary>
    ''' マウスカーソルがEditingPanel上にあるときのカーソルを指定する
    ''' EditingPanelは編集コントロールをホストするパネルで、
    ''' 編集コントロールがセルより小さいとコントロール以外の部分がパネルとなる
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property EditingPanelCursor() As Cursor _
        Implements IDataGridViewEditingControl.EditingPanelCursor
        Get
            Return MyBase.Cursor
        End Get
    End Property
    ''' <summary>
    ''' コントロールで編集する準備をする
    ''' テキストを選択状態にしたり、挿入ポインタを末尾にしたりする
    ''' </summary>
    ''' <param name="selectAll"></param>
    ''' <remarks></remarks>
    Public Sub PrepareEditingControlForEdit(ByVal selectAll As Boolean) _
        Implements IDataGridViewEditingControl.PrepareEditingControlForEdit

        If selectAll Then
            '選択状態にする
            Me.SelectAll()
        Else
            '挿入ポインタを末尾にする
            Me.SelectionStart = Me.TextLength
        End If
    End Sub
    ''' <summary>
    ''' 値が変更した時に、セルの位置を変更するかどうか
    ''' 値が変更された時に編集コントロールの大きさが変更される時はTrue
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property RepositionEditingControlOnValueChange() _
        As Boolean _
        Implements IDataGridViewEditingControl.RepositionEditingControlOnValueChange
        Get
            Return False
        End Get
    End Property

#End Region

#Region "オーバーライドメソッド"
    ''' <summary>
    ''' 値が変更された時
    ''' </summary>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Overrides Sub OnTextChanged(ByVal e As EventArgs)
        MyBase.OnTextChanged(e)
        '値が変更されたことをDataGridViewに通知する
        Me.valueChanged = True
        Me.dataGridView.NotifyCurrentCellDirty(True)
    End Sub

#End Region

End Class

#End Region

