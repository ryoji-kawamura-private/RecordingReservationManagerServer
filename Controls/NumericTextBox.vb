Imports System.ComponentModel
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports System.Drawing

'/// <summary>���l��p�e�L�X�g�{�b�N�X�R���|�[�l���g�B</summary>
Public Class NumericTextBox : Inherits BaseTextBox

#Region " �R���g���[���̃f�U�C�i��̕\�����J�X�^�}�C�Y���� "
    '/// <summary>�R���g���[���̃f�U�C�i��̕\�����J�X�^�}�C�Y���邽�߂̓���q�N���X�B</summary>
    Friend Class NumericTextDesigner : Inherits System.Windows.Forms.Design.ControlDesigner
        Protected Overrides Sub PostFilterProperties(ByVal Properties As IDictionary)
            '/// <description>���̃v���p�e�B�̓f�U�C�i�ł͔�\���ɂ���B</description>
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

#Region " �����o�ϐ��E�萔�E�C�x���g "
    '/// <description>�C�x���g��`</description>
    <Description("Accuracy �v���p�e�B���ύX���ꂽ���ɔ������܂��B")> _
    Public Event AccuracyChanged(ByVal sender As Object, ByVal ev As EventArgs)
    Private evhAccuracyChanged As EventHandler
    <Description("�R���g���[�������� Text �v���p�e�B���ύX���ꂽ���ɔ������܂��B")> _
    Public Event InnerTextChanged(ByVal sender As Object, ByVal ev As EventArgs)
    Private evhInnerTextChanged As EventHandler

    '/// <description>�ϐ���`</description>
    Private mMinValue As Int64                          '/// <summary>�e�L�X�g�ɓ��͉\�ȍŏ��l</summary>
    Private mMaxValue As Int64                          '/// <summary>�e�L�X�g�ɓ��͉\�ȍő�l</summary>
    Private mAccuracy As Accuracy                       '/// <summary>�e�L�X�g�ɓ��͉\�Ȑ��l�̐��x</summary>
    Private mDelimiter As Char = ","c                   '/// <summary>�e�L�X�g�̋�؂蕶��</summary>

    Private mblnInputMinus As Boolean = False           '/// <summary>�}�C�i�X�l�̓��͉ېݒ�</summary>
    Private mblnZeroToBlank As Boolean = False          '/// <summary>�[���l�̕\���ېݒ�</summary>

    Private _SkipTextChangedEvent As Boolean = False    '/// <summary>TextChanged�C�x���g���X�L�b�v</summary>
#End Region

#Region " �R���|�[�l���g �f�U�C�i�Ő������ꂽ�R�[�h "

    Public Sub New()
        MyBase.New()

        ' ���̌Ăяo���́A�R���|�[�l���g �f�U�C�i�ŕK�v�ł��B
        InitializeComponent()

        ' InitializeComponent() �Ăяo���̌�ɏ�������ǉ����܂��B

        '/// <description>�f�t�H���g�ݒ�𔽉f���܂��B</description>
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

    'Control �́A�R���|�[�l���g�ꗗ�Ɍ㏈�������s���邽�߂ɁAdispose ���I�[�o�[���C�h���܂��B
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    '�R���g���[�� �f�U�C�i�ŕK�v�ł��B
    Private components As System.ComponentModel.IContainer

    ' ���� : �ȉ��̃v���V�[�W���̓R���|�[�l���g �f�U�C�i�ŕK�v�ł��B
    ' �R���|�[�l���g �f�U�C�i���g���ĕύX�ł��܂��B
    ' �R�[�h �G�f�B�^���g���ĕύX���Ȃ��ł��������B  
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        components = New System.ComponentModel.Container
    End Sub

#End Region

#Region " �v���p�e�B "
    '/// <value>���l�̐��x�ł��B
    '(�S�̂̌���, �������̌���)</value>
    <Category("���炦"), _
    Description("���l�̐��x�ł��B" & vbNewLine & "(�S�̂̌���, �������̌���)")> _
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
    '/// <value>��؂蕶���ł��B</value>
    <Category("���炦"), _
    DefaultValue(","c), _
    Description("��؂蕶���ł��B"), Bindable(True)> _
    Public Property Delimiter() As Char
        Get
            Return mDelimiter
        End Get
        Set(ByVal Value As Char)
            mDelimiter = Value
        End Set
    End Property
    '/// <value>���͉\�ȍŏ��̐��l�ł��B</value>
    <Category("���炦"), _
    DefaultValue(0), _
    Description("���͉\�ȍŏ��̐��l�ł��B")> _
    Public Property MinValue() As Int64
        Get
            Return mMinValue
        End Get
        Set(ByVal Value As Int64)
            mMinValue = Value
        End Set
    End Property
    '/// <value>���͉\�ȍő�̐��l�ł��B</value>
    <Category("���炦"), _
    DefaultValue(9999999), _
    Description("���͉\�ȍő�̐��l�ł��B")> _
    Public Property MaxValue() As Int64
        Get
            Return mMaxValue
        End Get
        Set(ByVal Value As Int64)
            mMaxValue = Value
        End Set
    End Property

    '/// <value>�}�C�i�X�l�̓��͉ېݒ�ł��B</value>
    <Category("���炦"), _
    DefaultValue(False), _
    Description("�}�C�i�X�l�̓��͉ېݒ�ł��B")> _
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

    '/// <value>�[���l�̕\���ېݒ�ł��B</value>
    <Category("���炦"), _
    DefaultValue(False), _
    Description("�[���l�̕\���ېݒ�ł��B")> _
    Public Property ZeroToBlank() As Boolean
        Get
            Return mblnZeroToBlank
        End Get
        Set(ByVal Value As Boolean)
            mblnZeroToBlank = Value
        End Set
    End Property
#End Region

#Region " �C�x���g�n���h�� "
    Protected Overrides Sub OnKeyPress(ByVal e As System.Windows.Forms.KeyPressEventArgs)
        Try
            '���䕶���`�F�b�N
            If Char.IsControl(e.KeyChar) = True Then
                '�x�[�X�̃��b�Z�[�W�����ɔC����
                e.Handled = False
                Return
            End If

            '���͊�����̏����Ń}�X�N�`�F�b�N���s�Ȃ�
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

            '// ���������� �����ł̕ύX�ɂ����ẮATextChanged�C�x���g�𔭐������Ȃ��B ����������
            _SkipTextChangedEvent = True
            Me.Text = Me.Text.Replace(mDelimiter.ToString, String.Empty)
            _SkipTextChangedEvent = False
            '// ���������� ���̊Ԃ�Return���ߓ��Ő���̗���̕ύX�֎~ !!!  ����������������������

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

            '// ���������� �����ł̕ύX�ɂ����ẮATextChanged�C�x���g�𔭐������Ȃ��B ����������
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
                '�[���l��\�����Ȃ��ݒ�̏ꍇ�A�󕶎��ɒu������
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
            '// ���������� ���̊Ԃ�Return���ߓ��Ő���̗���̕ύX�֎~ !!!  ����������������������

            '// �E�l�ŕ\�����������߁B
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

                    '�x�[�X�̃��b�Z�[�W�����ɔC����
                    MyBase.WndProc(m)

                    Dim strPasteAfterString As String = Me.Text
                    Dim r As Regex = New Regex(MakeRegularPattern())
                    If r.IsMatch(strPasteAfterString) = False Then
                        '// ���������� �����ł̕ύX�ɂ����ẮATextChanged�C�x���g�𔭐������Ȃ��B ����������
                        _SkipTextChangedEvent = True
                        Me.Text = strPasteBeforeString
                        _SkipTextChangedEvent = False
                        '// ���������� ���̊Ԃ�Return���ߓ��Ő���̗���̕ύX�֎~ !!!  ����������������������
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
            '// ���v���O���������Ńe�L�X�g��ύX�����ۂ́A�e�L�X�g�`�F���W�C�x���g�𔭐������Ȃ��B
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

#Region " �������\�b�h "
    '/// <summary>�t�H�[�}�b�g�X�^�C���쐬</summary>
    '/// <param name=""></param>
    '/// <returns>�t�H�[�}�b�g�p�X�^�C��������</returns>
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
    '/// <summary>���K�\���p�^�[���쐬</summary>
    '/// <param name=""></param>
    '/// <returns>���K�\���p�^�[��������</returns>
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
