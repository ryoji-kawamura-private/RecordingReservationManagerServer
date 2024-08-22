Imports System.ComponentModel
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms

'/// <summary>テキストボックスのベースとなるコンポーネント。</summary>
<ToolboxBitmap(GetType(System.Windows.Forms.TextBox)), _
Designer(GetType(BaseTextBox.TextDesigner))> _
Public Class BaseTextBox : Inherits System.Windows.Forms.TextBox

#Region " コントロールのデザイナ上の表示をカスタマイズする "
    '/// <summary>コントロールのデザイナ上の表示をカスタマイズするための入れ子クラス。</summary>
    Friend Class TextDesigner : Inherits System.Windows.Forms.Design.ControlDesigner
        Protected Overrides Sub PostFilterProperties(ByVal Properties As IDictionary)
            '/// <description>次のプロパティはデザイナでは非表示にする。</description>
            Properties.Remove("AutoSize")
            Properties.Remove("BackgroundImage")
            Properties.Remove("Image")
            Properties.Remove("ImageAlign")
            Properties.Remove("ImageIndex")
            Properties.Remove("ImageList")
        End Sub
    End Class
#End Region

#Region " メンバ変数・定数・イベント "

    '/// <description>イベント定義</description>
    <Description("SelectionBackColor プロパティが変更された時に発生します。")> _
    Public Event SelectionBackColorChanged(ByVal sender As Object, ByVal ev As EventArgs)
    Private evhSelectionBackColorChanged As EventHandler
    <Description("CanSearch プロパティが変更された時に発生します。")> _
    Public Event CanSearchChanged(ByVal sender As Object, ByVal ev As EventArgs)
    Private evhCanSearchChanged As EventHandler
    <Description("SearchBackColor プロパティが変更された時に発生します。")> _
    Public Event SearchBackColorChanged(ByVal sender As Object, ByVal ev As EventArgs)
    Private evhSearchBackColorChanged As EventHandler

    '/// <description>変数定義</description>
    Private mSelectionBackColor As Color = Color.LightSteelBlue     '/// <summary>選択色</summary>
    Private mCanSearch As Boolean = False                           '/// <summary>テキストボックスが検索フィールドの際は True をセット。</summary>
    Private mSearchBackColor As Color = Color.LightPink             '/// <summary>検索色</summary>
    Private mLenB As Int16 = 0                                      '/// <summary>テキストのバイト長</summary>
    Private mMaxLenB As Int16 = 20                                  '/// <summary>テキストに入力可能な最大バイト長</summary>

    Private mBaseBackColor As Color                                 '/// <summary>バックカラーの待避</summary>

    Private mblnInputAlpha As Boolean = True                        '/// <summary>入力可能文字種設定（半角英字）</summary>
    Private mblnInputNumeric As Boolean = True                      '/// <summary>入力可能文字種設定（半角数字）</summary>
    Private mblnInputKana As Boolean = True                         '/// <summary>入力可能文字種設定（半角カナ）</summary>
    Private mblnInputEtc As Boolean = True                          '/// <summary>入力可能文字種設定（半角その他記号等）</summary>
    Private mblnInputZenkaku As Boolean = True                      '/// <summary>入力可能文字種設定（全角文字）</summary>
    Private mstrInputOk As String = ""                              '/// <summary>入力可能文字設定</summary>
    Private mstrInputNg As String = ""                              '/// <summary>入力不可文字設定</summary>
    Private mstrErrorCode As String = ""                            '/// <summary>入力エラーコード</summary>
    Private mblnAutoResize As Boolean = True                        '/// <summary>自動幅調整</summary>
    Private mblnAutoFormat As Boolean = True                        '/// <summary>自動書式設定</summary>
    Private isCreated As Boolean = False                            '/// <summary>OnCreateControlが呼ばれたかどうかのフラグ。</summary>
#End Region

#Region " コンポーネント デザイナで生成されたコード "

    Public Sub New()
        MyBase.New()

        ' この呼び出しは、コンポーネント デザイナで必要です。
        InitializeComponent()

        ' InitializeComponent() 呼び出しの後に初期化を追加します。

        '/// <description>デフォルト設定を反映します。</description>
        MyBase.Font = New Font("ＭＳ ゴシック", 9.75!)
        MyBase.Height = 19
        MyBase.Text = String.Empty

    End Sub

    'Component は、コンポーネント一覧に後処理を実行するために dispose をオーバーライドします。
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    ' コンポーネント デザイナで必要です。
    Private components As System.ComponentModel.IContainer

    ' メモ : 以下のプロシージャはコンポーネント デザイナで必要です。
    ' コンポーネント デザイナを使って変更してください。
    ' コード エディタを使って変更しないでください。
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
    End Sub

#End Region

#Region " プロパティ "
    '/// <value>コントロール選択時の背景色です。</value>
    <Category("あつらえ"), _
    Description("コントロール選択時の背景色です。"), Bindable(True)> _
    Public Property SelectionBackColor() As Color
        Get
            Return mSelectionBackColor
        End Get
        Set(ByVal Value As Color)
            mSelectionBackColor = Value
            OnSelectionBackColorChanged(EventArgs.Empty)
        End Set
    End Property
    '/// <value>コントロールが検索対象フィールドかどうかを表します。</value>
    <Category("あつらえ"), _
    DefaultValue(False), _
    Description("コントロールが検索対象フィールドかどうかを表します。"), Bindable(True)> _
    Public Property CanSearch() As Boolean
        Get
            Return mCanSearch
        End Get
        Set(ByVal Value As Boolean)
            mCanSearch = Value
            OnCanSearchChanged(EventArgs.Empty)
        End Set
    End Property
    '/// <value>コントロール選択時の検索対象フィールド背景色です。</value>
    <Category("あつらえ"), _
    Description("コントロール選択時の検索対象フィールド背景色です。"), Bindable(True)> _
    Public Property SearchBackColor() As Color
        Get
            Return mSearchBackColor
        End Get
        Set(ByVal Value As Color)
            mSearchBackColor = Value
            OnSearchBackColorChanged(EventArgs.Empty)
        End Set
    End Property

    '/// <value>テキストのバイト長です。</value>
    <Category("あつらえ"), _
    Browsable(False), _
    DefaultValue(0), _
    Description("テキストのバイト長です。"), Bindable(True)> _
    Public ReadOnly Property LengthByte() As Int16
        Get
            Dim SJIS As Encoding = Encoding.GetEncoding("Shift_JIS")
            mLenB = Convert.ToInt16(SJIS.GetByteCount(Me.Text))
            Return mLenB
        End Get
        'Set(ByVal Value As Int16)
        '    mLenB = Value
        'End Set
    End Property

    '/// <value>テキストの最大バイト長です。</value>
    <Category("あつらえ"), _
    DefaultValue(20), _
    Description("テキストの最大バイト長です。"), Bindable(True)> _
    Public Property MaxLengthByte() As Int16
        Get
            Return mMaxLenB
        End Get
        Set(ByVal Value As Int16)
            mMaxLenB = Value
            If mblnAutoResize = True Then
                Me.Width = 5 + (mMaxLenB * 7) + 5
            End If
        End Set
    End Property

    '/// <value>半角英字の入力可否を設定します。</value>
    <Category("あつらえ"), _
    DefaultValue(True), _
    Description("半角英字の入力可否を設定します。"), Bindable(True)> _
    Public Property InputAlpha() As Boolean
        Get
            Return mblnInputAlpha
        End Get
        Set(ByVal Value As Boolean)
            mblnInputAlpha = Value
        End Set
    End Property

    '/// <value>半角数字の入力可否を設定します。</value>
    <Category("あつらえ"), _
    DefaultValue(True), _
    Description("半角数字の入力可否を設定します。"), Bindable(True)> _
    Public Property InputNumeric() As Boolean
        Get
            Return mblnInputNumeric
        End Get
        Set(ByVal Value As Boolean)
            mblnInputNumeric = Value
        End Set
    End Property

    '/// <value>半角カナの入力可否を設定します。</value>
    <Category("あつらえ"), _
    DefaultValue(True), _
    Description("半角カナの入力可否を設定します。"), Bindable(True)> _
    Public Property InputKana() As Boolean
        Get
            Return mblnInputKana
        End Get
        Set(ByVal Value As Boolean)
            mblnInputKana = Value
        End Set
    End Property

    '/// <value>半角その他記号等の入力可否を設定します。</value>
    <Category("あつらえ"), _
    DefaultValue(True), _
    Description("半角その他記号等の入力可否を設定します。"), Bindable(True)> _
    Public Property InputEtc() As Boolean
        Get
            Return mblnInputEtc
        End Get
        Set(ByVal Value As Boolean)
            mblnInputEtc = Value
        End Set
    End Property

    '/// <value>全角文字の入力可否を設定します。</value>
    <Category("あつらえ"), _
    DefaultValue(True), _
    Description("全角文字の入力可否を設定します。"), Bindable(True)> _
    Public Property InputZenkaku() As Boolean
        Get
            Return mblnInputZenkaku
        End Get
        Set(ByVal Value As Boolean)
            mblnInputZenkaku = Value
        End Set
    End Property

    '/// <value>入力可能文字を設定します。</value>
    <Category("あつらえ"), _
    DefaultValue(""), _
    Description("入力可能文字を設定します。"), Bindable(True)> _
    Public Property InputOk() As String
        Get
            Return mstrInputOk
        End Get
        Set(ByVal Value As String)
            mstrInputOk = Value
        End Set
    End Property

    '/// <value>入力不可文字を設定します。</value>
    <Category("あつらえ"), _
    DefaultValue(""), _
    Description("入力不可文字を設定します。"), Bindable(True)> _
    Public Property InputNg() As String
        Get
            Return mstrInputNg
        End Get
        Set(ByVal Value As String)
            mstrInputNg = Value
        End Set
    End Property

    '/// <value>入力可能なバイト長に合わせて自動で幅調整を行なうかを設定します。</value>
    <Category("あつらえ"), _
    DefaultValue(True), _
    Description("入力可能なバイト長に合わせて自動で幅調整を行なうかを設定します。"), Bindable(True)> _
    Public Property AutoResize() As Boolean
        Get
            Return mblnAutoResize
        End Get
        Set(ByVal Value As Boolean)
            mblnAutoResize = Value
        End Set
    End Property

    '/// <value>入力可能なバイト長に合わせて自動で幅調整を行なうかを設定します。</value>
    <Category("あつらえ"), _
    DefaultValue(True), _
    Description("入力完了後に書式および補完を行なうかを設定します。"), Bindable(True)> _
    Public Property AutoFormat() As Boolean
        Get
            Return mblnAutoFormat
        End Get
        Set(ByVal Value As Boolean)
            mblnAutoFormat = Value
        End Set
    End Property

    '/// <value>入力エラー時のステータスです。</value>
    <Category("あつらえ"), _
    Description("入力エラーコードです。")> _
    Public Property ErrorCode() As String
        Get
            Return mstrErrorCode
        End Get
        Set(ByVal Value As String)
            mstrErrorCode = Value
        End Set
    End Property

#End Region

#Region " イベントハンドラ "
    Protected Overrides Sub OnPaint(ByVal e As System.Windows.Forms.PaintEventArgs)
        Dim TextPoint As New PointF             '/// <summary>文字列を表示する座標</summary>
        Dim TextSize As SizeF                   '/// <summary>表示する文字列の幅</summary>
        Dim XAdjust As Integer                  '/// <summary>位置調整</summary>
        Dim YAdjust As Integer                  '/// <summary>位置調整</summary>

        Try
            MyBase.OnPaint(e)

            '/// <description>BorderStyleによって位置調整</description>
            Select Case Me.BorderStyle
                Case BorderStyle.Fixed3D
                    XAdjust = -1
                    YAdjust = 6
                Case BorderStyle.FixedSingle
                    XAdjust = 0
                    YAdjust = 4
                Case BorderStyle.None
                    XAdjust = -2
                    YAdjust = 1
                Case Else
                    XAdjust = 0
                    YAdjust = 0
            End Select

            '/// <description>表示する文字列幅の算出</description>
            TextSize = e.Graphics.MeasureString(Me.Text, Me.Font)

            '/// <description>テキスト配置に応じて表示する座標を計算</description>
            Select Case Me.TextAlign
                Case HorizontalAlignment.Left
                    TextPoint.X = XAdjust
                    TextPoint.Y = (Me.Height - Me.Font.GetHeight(e.Graphics) - YAdjust) / 2
                Case HorizontalAlignment.Center
                    TextPoint.X = (Me.Width - TextSize.Width) / 2 + (XAdjust * 3)
                    TextPoint.Y = (Me.Height - Me.Font.GetHeight(e.Graphics) - YAdjust) / 2
                Case HorizontalAlignment.Right
                    TextPoint.X = Me.Width - TextSize.Width + (XAdjust * 4)
                    TextPoint.Y = (Me.Height - Me.Font.GetHeight(e.Graphics) - YAdjust) / 2
            End Select

            e.Graphics.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
            '/// <description>文字列を表示</description>
            e.Graphics.DrawString(Me.Text, Me.Font, New SolidBrush(Me.ForeColor), TextPoint)

        Catch ex As Exception
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
    End Sub

    Protected Overrides Sub OnEnter(ByVal e As System.EventArgs)
        Try
            MyBase.OnEnter(e)

            If Me.Enabled Then
                '// OnLeaveメソッド内で元に戻す現在の背景色を待避。
                If Me.ReadOnly Then
                    mBaseBackColor = SystemColors.Control
                Else
                    mBaseBackColor = Me.BackColor
                End If

                '// 背景色を設定
                If mCanSearch Then
                    Me.BackColor = mSearchBackColor
                Else
                    Me.BackColor = mSelectionBackColor
                End If

                '// テキストを選択状態に
                Me.SelectAll()
            Else
                '// 使用不可の場合は、背景色を固定で処理終了。
                Me.BackColor = SystemColors.Control
            End If

            Return
        Catch ex As Exception
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
    End Sub

    Protected Overrides Sub OnLeave(ByVal e As System.EventArgs)
        Try
            MyBase.OnLeave(e)

            If Me.Enabled Then
                '// 使用可能の場合に限り、OnEnterメソッドで待避した背景色に戻す。
                Me.BackColor = mBaseBackColor
            End If

            Return
        Catch ex As Exception
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
    End Sub

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        Try
            Const WM_PASTE As Integer = &H302
            Const WM_CHAR As Integer = &H102
            Const WM_IME_CHAR As Integer = &H286
            'Const WM_IME_COMPOSITION As Integer = &H10F
            'Const WM_IME_COMPOSITIONFULL As Integer = &H284
            'Const WM_IME_CONTROL As Integer = &H283

            Select Case m.Msg
                Case WM_PASTE
                    '// 読み取り状態でペーストを防ぐ      at 2005/06/24 by TJ R.Nishila
                    If Me.ReadOnly Then Exit Select

                    'クリップボードより入力可能文字のみ（ペースト対象文字列）を取得する
                    Dim iData As IDataObject = Clipboard.GetDataObject()
                    Dim strClipboardString As String = String.Empty
                    If iData.GetDataPresent(DataFormats.Text) Then
                        strClipboardString = DirectCast(iData.GetData(DataFormats.Text), String)
                    End If

                    Dim intLoopCnt As Integer
                    Dim strChar As String = strClipboardString.Substring(intLoopCnt, 1)
                    Dim strCheckAfterString As String = ""
                    For intLoopCnt = 0 To strClipboardString.Length - 1
                        strChar = strClipboardString.Substring(intLoopCnt, 1)
                        '入力可能文字チェック（１文字単位）
                        If CheckInputCode(AscW(strChar)) = True Then
                            strCheckAfterString = strCheckAfterString & strChar
                        End If
                    Next intLoopCnt

                    '入力済文字数＋ペースト対象文字数が最大入力可能バイト数より大きい場合
                    'ペースト対象文字列の末尾をカットする
                    If mMaxLenB > 0 Then
                        Dim SJIS As Encoding = Encoding.GetEncoding("Shift_JIS")
                        Dim intInputByte As Integer = SJIS.GetByteCount(Me.Text)
                        Dim intSelectByte As Integer = SJIS.GetByteCount(Me.SelectedText)
                        Dim intSjisByte As Integer = SJIS.GetByteCount(strCheckAfterString)
                        Do Until mMaxLenB >= (intInputByte - intSelectByte) + intSjisByte
                            intLoopCnt = strCheckAfterString.Length - 1
                            strCheckAfterString = strCheckAfterString.Substring(0, intLoopCnt)
                            intSjisByte = SJIS.GetByteCount(strCheckAfterString)
                        Loop
                    End If
                    'カレントカーソル位置にペーストする
                    Dim intPasteAfterStart As Integer = 0
                    intPasteAfterStart = Me.SelectionStart + strCheckAfterString.Length
                    Me.Text = Me.Text.Substring(0, Me.SelectionStart) & _
                            strCheckAfterString & _
                            Me.Text.Substring(Me.SelectionStart + Me.SelectionLength)
                    Me.SelectionStart = intPasteAfterStart

                    m.Result = IntPtr.Zero
                    Return

                Case WM_CHAR, WM_IME_CHAR
                    '制御文字チェック
                    If Char.IsControl(ChrW(m.WParam.ToInt32)) = True Then
                        'ベースのメッセージ処理に任せる
                        MyBase.WndProc(m)
                        Return
                    End If
                    '入力チェック
                    If CheckInputCode(m.WParam.ToInt32) = False Then
                        m.Result = IntPtr.Zero
                        Return
                    End If
                    '最大入力可能バイト数チェック
                    If mMaxLenB > 0 Then
                        Dim SJIS As Encoding = Encoding.GetEncoding("Shift_JIS")
                        Dim intInputByte As Integer = SJIS.GetByteCount(Me.Text)
                        Dim intSelectByte As Integer = SJIS.GetByteCount(Me.SelectedText)
                        Dim intSjisByte As Integer = SJIS.GetByteCount(ChrW(m.WParam.ToInt32))
                        If mMaxLenB < (intInputByte - intSelectByte) + intSjisByte Then
                            m.Result = IntPtr.Zero
                            Return
                        End If
                    End If

            End Select

            MyBase.WndProc(m)

            Return
        Catch ex As Exception
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
    End Sub

    Protected Overrides Sub OnEnabledChanged(ByVal e As System.EventArgs)
        Try
            MyBase.OnEnabledChanged(e)

            If (isCreated) Then
                '// 既にOnCreateControlが呼ばれてる場合のみ、Styleの変更を行なう。
                If Me.Enabled Then
                    Me.SetStyle(Windows.Forms.ControlStyles.UserPaint Or _
                                    Windows.Forms.ControlStyles.AllPaintingInWmPaint Or _
                                    Windows.Forms.ControlStyles.DoubleBuffer, False)
                    Me.UpdateStyles()
                    Me.BackColor = Me.mBaseBackColor
                Else
                    Me.SetStyle(Windows.Forms.ControlStyles.UserPaint Or _
                                     Windows.Forms.ControlStyles.AllPaintingInWmPaint Or _
                                     Windows.Forms.ControlStyles.DoubleBuffer, True)
                    Me.UpdateStyles()
                    Me.BackColor = DefaultBackColor
                End If

                Me.Invalidate()
            End If

            Return
        Catch ex As Exception
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
    End Sub

    Protected Overrides Sub OnCreateControl()
        Try
            MyBase.OnCreateControl()
            isCreated = True
            If Not (Me.Enabled) Then
                '// デザイン時にEnabled=Falseとされた場合は、遅延してSetStyleを行なう。
                Me.SetStyle(Windows.Forms.ControlStyles.UserPaint Or _
                                 Windows.Forms.ControlStyles.AllPaintingInWmPaint Or _
                                 Windows.Forms.ControlStyles.DoubleBuffer, True)
                Me.UpdateStyles()
                Me.BackColor = DefaultBackColor
            End If
        Catch ex As Exception
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
    End Sub

    Protected Overridable Sub OnSelectionBackColorChanged(ByVal E As EventArgs)
        Try
            Invalidate()
            ''If Not (evhSelectionBackColorChanged Is Nothing) Then evhSelectionBackColorChanged.Invoke(Me, E)
            RaiseEvent SelectionBackColorChanged(Me, EventArgs.Empty)

            Return
        Catch ex As Exception
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
    End Sub

    Protected Overridable Sub OnCanSearchChanged(ByVal E As EventArgs)
        Try
            Invalidate()
            ''If Not (evhCanSearchChanged Is Nothing) Then evhCanSearchChanged.Invoke(Me, E)
            RaiseEvent CanSearchChanged(Me, EventArgs.Empty)

            Return
        Catch ex As Exception
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
    End Sub

    Protected Overridable Sub OnSearchBackColorChanged(ByVal E As EventArgs)
        Try
            Invalidate()
            ''If Not (evhSearchBackColorChanged Is Nothing) Then evhSearchBackColorChanged.Invoke(Me, E)
            RaiseEvent SearchBackColorChanged(Me, EventArgs.Empty)

            Return
        Catch ex As Exception
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
    End Sub

#End Region

#Region " 内部メソッド "
    Private Function CheckInputCode(ByVal intCharCode As Integer) As Boolean
        Try
            Dim SJIS As Encoding = Encoding.GetEncoding("Shift_JIS")
            Dim cCharacter As Char = ChrW(intCharCode)
            Dim intSjisByte As Integer = SJIS.GetByteCount(cCharacter)

            '制御文字チェック
            If Char.IsControl(ChrW(intCharCode)) = True Then
                Return (False)
            End If

            '入力可能指定文字チェック
            If mstrInputOk <> "" Then
                If InStr(mstrInputOk, cCharacter, CompareMethod.Binary) > 0 Then
                    'ベースのメッセージ処理に任せる
                    Return (True)
                End If
            End If

            '入力不可指定文字チェック
            If mstrInputNg <> "" Then
                If InStr(mstrInputNg, cCharacter, CompareMethod.Binary) > 0 Then
                    Return (False)
                End If
            End If

            '半角英文字チェック
            If mblnInputAlpha = False Then
                '英大文字
                If 65 <= intCharCode And intCharCode <= 90 Then
                    Return (False)
                End If

                '英小文字
                If 97 <= intCharCode And intCharCode <= 122 Then
                    Return (False)
                End If
            End If

            '半角数字チェック
            If mblnInputNumeric = False Then
                If 48 <= intCharCode And intCharCode <= 57 Then
                    Return (False)
                End If
            End If

            '半角カナ文字チェック
            If mblnInputKana = False Then
                If 65377 <= intCharCode And intCharCode <= 65439 Then
                    Return (False)
                End If
            End If

            '半角その他記号等チェック
            If mblnInputEtc = False Then
                If 32 <= intCharCode And intCharCode <= 47 Then
                    Return (False)
                End If

                If 58 <= intCharCode And intCharCode <= 64 Then
                    Return (False)
                End If

                If 91 <= intCharCode And intCharCode <= 96 Then
                    Return (False)
                End If

                If 123 <= intCharCode And intCharCode <= 126 Then
                    Return (False)
                End If
            End If

            '全角文字チェック
            If mblnInputZenkaku = False And intSjisByte = 2 Then
                Return (False)
            End If

            'ベースのメッセージ処理に任せる
            Return (True)

        Catch ex As Exception
            Debug.WriteLine(ex.Source & "||" & ex.Message)
        End Try
    End Function
#End Region

End Class

