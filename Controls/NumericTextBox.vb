Imports System.ComponentModel
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Drawing

'/// <summary>数値専用テキストボックスコンポーネント。</summary>
Public Class NumericTextBox : Inherits BaseTextBox

#Region " コントロールのデザイナ上の表示をカスタマイズする "
    '/// <summary>コントロールのデザイナ上の表示をカスタマイズするための入れ子クラス。</summary>
    Friend Class NumericTextDesigner : Inherits System.Windows.Forms.Design.ControlDesigner
        Protected Overrides Sub PostFilterProperties(ByVal Properties As IDictionary)
            '/// <description>次のプロパティはデザイナでは非表示にする。</description>
            Properties.Remove("InputAlpha")
            Properties.Remove("InputNumeric")
            Properties.Remove("InputKana")
            Properties.Remove("InputEtc")
            Properties.Remove("InputZenkaku")
            Properties.Remove("InputOk")
            Properties.Remove("InputNg")
            Properties.Remove("MaxLengthByte")
            Properties.Remove("AutoResize")
        End Sub
    End Class
#End Region

#Region " メンバ変数・定数・イベント "
    '/// <description>イベント定義</description>
    <Description("Accuracy プロパティが変更された時に発生します。")> _
    Public Event AccuracyChanged(ByVal sender As Object, ByVal ev As EventArgs)
    Private evhAccuracyChanged As EventHandler
    <Description("コントロール内部で Text プロパティが変更された時に発生します。")> _
    Public Event InnerTextChanged(ByVal sender As Object, ByVal ev As EventArgs)
    Private evhInnerTextChanged As EventHandler

    '/// <description>変数定義</description>
    Private mMinValue As Int64                          '/// <summary>テキストに入力可能な最小値</summary>
    Private mMaxValue As Int64                          '/// <summary>テキストに入力可能な最大値</summary>
    Private mAccuracy As Accuracy                       '/// <summary>テキストに入力可能な数値の精度</summary>
    Private mDelimiter As Char = ","c                   '/// <summary>テキストの区切り文字</summary>

    Private mblnInputMinus As Boolean = False           '/// <summary>マイナス値の入力可否設定</summary>
    Private mblnZeroToBlank As Boolean = False          '/// <summary>ゼロ値の表示可否設定</summary>

    Private _SkipTextChangedEvent As Boolean = False    '/// <summary>TextChangedイベントをスキップ</summary>
#End Region

#Region " コンポーネント デザイナで生成されたコード "

    Public Sub New()
        MyBase.New()

        ' この呼び出しは、コンポーネント デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後に初期化を追加します。

        '/// <description>デフォルト設定を反映します。</description>
        MyBase.ImeMode = Windows.Forms.ImeMode.Disable
        MyBase.TextAlign = Windows.Forms.HorizontalAlignment.Right
        MyBase.InputAlpha = False
        MyBase.InputNumeric = True
        MyBase.InputKana = False
        MyBase.InputEtc = False
        MyBase.InputZenkaku = False
        MyBase.InputOk = ""
        MyBase.InputNg = ""
        MyBase.AutoResize = False
        MyBase.MaxLengthByte = 0
        MyBase.AutoFormat = True
        MyBase.Text = String.Empty

        Me.mAccuracy = New Accuracy
    End Sub

    'Control は、コンポーネント一覧に後処理を実行するために、dispose をオーバーライドします。
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'コントロール デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    ' メモ : 以下のプロシージャはコンポーネント デザイナで必要です。
    ' コンポーネント デザイナを使って変更できます。
    ' コード エディタを使って変更しないでください。  
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
    End Sub

#End Region

#Region " プロパティ "
    '/// <value>数値の精度です。
    '(全体の桁数, 小数部の桁数)</value>
    <Category("あつらえ"), _
    Description("数値の精度です。" & vbNewLine & "(全体の桁数, 小数部の桁数)")> _
    Public Property NumericAccuracy() As Accuracy
        Get
            Return mAccuracy
        End Get
        Set(ByVal Value As Accuracy)
            mAccuracy = Value
            OnAccuracyChanged(EventArgs.Empty)

            MyBase.InputOk = ""
            If mblnInputMinus = True Then
                MyBase.InputOk = "-"
            End If
            If mAccuracy.DecimalPart > 0 Then
                MyBase.InputOk = MyBase.InputOk & "."
            End If
        End Set
    End Property
    '/// <value>区切り文字です。</value>
    <Category("あつらえ"), _
    DefaultValue(","c), _
    Description("区切り文字です。"), Bindable(True)> _
    Public Property Delimiter() As Char
        Get
            Return mDelimiter
        End Get
        Set(ByVal Value As Char)
            mDelimiter = Value
        End Set
    End Property
    '/// <value>入力可能な最小の数値です。</value>
    <Category("あつらえ"), _
    DefaultValue(0), _
    Description("入力可能な最小の数値です。")> _
    Public Property MinValue() As Int64
        Get
            Return mMinValue
        End Get
        Set(ByVal Value As Int64)
            mMinValue = Value
        End Set
    End Property
    '/// <value>入力可能な最大の数値です。</value>
    <Category("あつらえ"), _
    DefaultValue(9999999), _
    Description("入力可能な最大の数値です。")> _
    Public Property MaxValue() As Int64
        Get
            Return mMaxValue
        End Get
        Set(ByVal Value As Int64)
            mMaxValue = Value
        End Set
    End Property

    '/// <value>マイナス値の入力可否設定です。</value>
    <Category("あつらえ"), _
    DefaultValue(False), _
    Description("マイナス値の入力可否設定です。")> _
    Public Property InputMinus() As Boolean
        Get
            Return mblnInputMinus
        End Get
        Set(ByVal Value As Boolean)
            mblnInputMinus = Value
            MyBase.InputOk = ""
            If mblnInputMinus = True Then
                MyBase.InputOk = "-"
            End If
            If mAccuracy.DecimalPart > 0 Then
                MyBase.InputOk = MyBase.InputOk & "."
            End If
        End Set
    End Property

    '/// <value>ゼロ値の表示可否設定です。</value>
    <Category("あつらえ"), _
    DefaultValue(False), _
    Description("ゼロ値の表示可否設定です。")> _
    Public Property ZeroToBlank() As Boolean
        Get
            Return mblnZeroToBlank
        End Get
        Set(ByVal Value As Boolean)
            mblnZeroToBlank = Value
        End Set
    End Property
#End Region

#Region " イベントハンドラ "
    Protected Overrides Sub OnKeyPress(ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Try
            '制御文字チェック
            If Char.IsControl(e.KeyChar) = True Then
                'ベースのメッセージ処理に任せる
                e.Handled = False
                Return
            End If

            '入力完了後の書式でマスクチェックを行なう
            Dim strInputAfterValue As String
            Dim r As Regex = New Regex(MakeRegularPattern())
            strInputAfterValue = Me.Text.Substring(0, Me.SelectionStart) & _
                                 e.KeyChar & _
                                 Me.Text.Substring(Me.SelectionStart + Me.SelectionLength)
            If r.IsMatch(strInputAfterValue) Then
                e.Handled = False
            Else
                e.Handled = True
            End If

            If String.IsNullOrEmpty(Me.Text) Then
                Return
            End If

            Dim ControlText As Decimal = Convert.ToDecimal(strInputAfterValue.Replace(mDelimiter.ToString, String.Empty))
            If mMinValue <= ControlText _
            AndAlso ControlText <= mMaxValue Then
                e.Handled = False
            Else
                e.Handled = True
            End If

            Return
        Catch ex As Exception
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
    End Sub

    Protected Overrides Sub OnEnter(ByVal e As System.EventArgs)
        Try
            MyBase.OnEnter(e)

            '// ↓↓↓↓↓ 内部での変更においては、TextChangedイベントを発生させない。 ↓↓↓↓↓
            _SkipTextChangedEvent = True
            Me.Text = Me.Text.Replace(mDelimiter.ToString, String.Empty)
            _SkipTextChangedEvent = False
            '// ↑↑↑↑↑ この間にReturn命令等で制御の流れの変更禁止 !!!  ↑↑↑↑↑↑↑↑↑↑↑

            Return
        Catch ex As Exception
            _SkipTextChangedEvent = False
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
    End Sub

    Protected Overrides Function ProcessDialogKey(ByVal keyData As System.Windows.Forms.Keys) As Boolean
        Try
            If keyData = Keys.Enter OrElse keyData = (Keys.Enter Or Keys.Shift) Then

                ErrorCode = ""

                If Me.Text.TrimEnd = String.Empty Then
                    Return MyBase.ProcessDialogKey(keyData)
                End If
            End If

            Return MyBase.ProcessDialogKey(keyData)

        Catch ex As Exception
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
    End Function

    Protected Overrides Sub OnLeave(ByVal e As System.EventArgs)
        Try
            MyBase.OnLeave(e)

            If Me.Text.TrimEnd = String.Empty Then
                Return
            End If

            '// ↓↓↓↓↓ 内部での変更においては、TextChangedイベントを発生させない。 ↓↓↓↓↓
            _SkipTextChangedEvent = True
            Me.Text = Me.Text.Replace(mDelimiter.ToString, String.Empty)
            If Convert.ToDecimal(Me.Text) <> 0 Then
                If AutoFormat Then
                    If mAccuracy.DecimalPart > 0 Then
                        Me.Text = Convert.ToDecimal(Me.Text).ToString(MakeFormatStyle() & "." & New String("0"c, mAccuracy.DecimalPart))
                    Else
                        Me.Text = Convert.ToInt64(Me.Text).ToString(MakeFormatStyle())
                    End If
                End If
            Else
                'ゼロ値を表示しない設定の場合、空文字に置換する
                If mblnZeroToBlank Then
                    Me.Text = String.Empty
                Else
                    If mAccuracy.DecimalPart > 0 Then
                        Me.Text = Convert.ToDecimal(Me.Text).ToString(MakeFormatStyle() & "." & New String("0"c, mAccuracy.DecimalPart))
                    Else
                        Me.Text = Convert.ToInt64(Me.Text).ToString(MakeFormatStyle())
                    End If
                End If
            End If
            _SkipTextChangedEvent = False
            '// ↑↑↑↑↑ この間にReturn命令等で制御の流れの変更禁止 !!!  ↑↑↑↑↑↑↑↑↑↑↑

            '// 右詰で表示したいため。
            Me.SelectionStart = Me.Text.Length
            Return

        Catch ex As Exception
            _SkipTextChangedEvent = False
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Try
            Const WM_PASTE As Integer = &H302

            Select Case m.Msg
                Case WM_PASTE
                    Dim strPasteBeforeString As String = Me.Text

                    'ベースのメッセージ処理に任せる
                    MyBase.WndProc(m)

                    Dim strPasteAfterString As String = Me.Text
                    Dim r As Regex = New Regex(MakeRegularPattern())
                    If r.IsMatch(strPasteAfterString) = False Then
                        '// ↓↓↓↓↓ 内部での変更においては、TextChangedイベントを発生させない。 ↓↓↓↓↓
                        _SkipTextChangedEvent = True
                        Me.Text = strPasteBeforeString
                        _SkipTextChangedEvent = False
                        '// ↑↑↑↑↑ この間にReturn命令等で制御の流れの変更禁止 !!!  ↑↑↑↑↑↑↑↑↑↑↑
                    End If

                    Return

            End Select

            MyBase.WndProc(m)

            Return
        Catch ex As Exception
            _SkipTextChangedEvent = False
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
    End Sub

    Protected Overrides Sub OnTextChanged(ByVal e As System.EventArgs)
        Try
            '// 当プログラム内部でテキストを変更した際は、テキストチェンジイベントを発生させない。
            If _SkipTextChangedEvent Then
                Me.OnInnerTextChanged(e)
            Else
                MyBase.OnTextChanged(e)
            End If
        Catch ex As Exception
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
    End Sub

    Protected Overridable Sub OnAccuracyChanged(ByVal e As EventArgs)
        Try
            Me.Invalidate()
            ''If Not (evhAccuracyChanged Is Nothing) Then evhAccuracyChanged.Invoke(Me, e)
            RaiseEvent AccuracyChanged(Me, EventArgs.Empty)

            Return
        Catch ex As Exception
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
    End Sub

    Protected Overridable Sub OnInnerTextChanged(ByVal e As EventArgs)
        Try
            Me.Invalidate()
            ''If Not (evhInnerTextChanged Is Nothing) Then evhInnerTextChanged.Invoke(Me, e)
            RaiseEvent InnerTextChanged(Me, EventArgs.Empty)

            Return
        Catch ex As Exception
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
    End Sub
#End Region

#Region " 内部メソッド "
    '/// <summary>フォーマットスタイル作成</summary>
    '/// <param name=""></param>
    '/// <returns>フォーマット用スタイル文字列</returns>
    Private Function MakeFormatStyle() As String
        Dim s As System.Text.StringBuilder = New System.Text.StringBuilder(mAccuracy.Length + 1)
        Try
            For i As Int16 = mAccuracy.Length To 2 Step -1
                s.Append("#")
                If (i Mod 3 = 1) And Not (mDelimiter = Nothing) Then
                    s.Append(mDelimiter.ToString)
                End If
            Next
            s.Append("0")
        Catch ex As Exception
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
        Return s.ToString
    End Function
    '/// <summary>正規表現パターン作成</summary>
    '/// <param name=""></param>
    '/// <returns>正規表現パターン文字列</returns>
    Private Function MakeRegularPattern() As String
        Dim s As System.Text.StringBuilder = New System.Text.StringBuilder
        Try
            Dim AccuIntPart As Int16 = mAccuracy.Length - mAccuracy.DecimalPart
            s.Append("^[+-]$|^[+-]?\d{1," & AccuIntPart & "}$")
            If mAccuracy.DecimalPart > 0 Then
                s.Append("|^[+-]?\d{1," & AccuIntPart & "}\.\d{0," & mAccuracy.DecimalPart & "}$")
                s.Append("|^[+-]?\.\d{0," & mAccuracy.DecimalPart & "}$")
            End If
        Catch ex As Exception
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
        Return s.ToString
    End Function
#End Region

End Class
