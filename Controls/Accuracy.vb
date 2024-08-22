Imports System.ComponentModel
Imports System.Drawing.Design

'/// <summary>���l���x��Oracle���C�N�ɕ\������N���X</summary>
<TypeConverter(GetType(AccuracyConverter)), Serializable()> _
Public Class Accuracy

#Region " �����o�ϐ��E�萔�E�C�x���g "
    Private mLength As Int16
    Private mDecimal As Int16
#End Region

#Region " �v���p�e�B "
    '/// <value>���x�F�S�̂̌���</value>
    <Category("���炦"), _
    RefreshPropertiesAttribute(RefreshProperties.All), _
    Description("���x�F�S�̂̌���"), Bindable(True)> _
    Public Property Length() As Int16
        Get
            Return mLength
        End Get
        Set(ByVal Value As Int16)
            CheckValue(Value, mDecimal)
            mLength = Value
        End Set
    End Property
    '/// <value>���x�F�������̌���</value>
    <Category("���炦"), _
    RefreshPropertiesAttribute(RefreshProperties.All), _
    Description("���x�F�������̌���"), Bindable(True)> _
    Public Property DecimalPart() As Int16
        Get
            Return mDecimal
        End Get
        Set(ByVal Value As Int16)
            CheckValue(mLength, Value)
            mDecimal = Value
        End Set
    End Property
#End Region

#Region " �R���X�g���N�^ "
    Public Sub New()
        Me.New(7, 0)
    End Sub
    '/// <summary>�R���X�g���N�^�i�I�[�o�[���[�h�j</summary>
    '/// <param name="L">�S�̂̌���</param>
    '/// <param name="D">�������̌���</param>
    '/// <returns></returns>
    Public Sub New(ByVal L As Int16, ByVal D As Int16)
        MyBase.New()
        Length = L
        DecimalPart = D
    End Sub
#End Region

#Region " �O�����\�b�h "
    Public Overrides Function ToString() As String
        Return mLength.ToString & ", " & mDecimal.ToString
    End Function
#End Region

#Region " �������\�b�h "
    '/// <summary>�v���p�e�B�̐������`�F�b�N</summary>
    '/// <param name="len">�S�̂̌���</param>
    '/// <param name="dec">�������̌���</param>
    '/// <returns></returns>
    Private Sub CheckValue(ByVal len As Int16, ByVal dec As Int16)
        Dim Msg As String = "Length<" & len.ToString & ">, DecimalPart<" & dec.ToString & ">" & System.Environment.NewLine
        If len < 0 OrElse dec < 0 Then Throw New ArgumentOutOfRangeException(Msg & "���̐������w�肵�Ă��������B")
        If len <= dec Then Throw New ArgumentOutOfRangeException(Msg & "�������������Ɠ��������A�������傫���ł��B")
    End Sub
#End Region

End Class

#Region " �J�X�^���^�C�v�R���o�[�^ "
'/// <summary>���l���x��Oracle���C�N�ɕ\������N���X�̃^�C�v�R���o�[�^</summary>
Public Class AccuracyConverter : Inherits ExpandableObjectConverter

    Public Overloads Overrides Function CanConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal sourceType As System.Type) As Boolean
        If sourceType Is GetType(System.String) Then
            Return True
        End If
        Return MyBase.CanConvertFrom(context, sourceType)
    End Function

    Public Overloads Overrides Function ConvertFrom(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object) As Object
        If TypeOf value Is System.String Then
            Try
                Dim s As String = DirectCast(value, String).Trim
                Dim v() As String = s.Split(New String() {", "}, StringSplitOptions.None)
                Return New Accuracy(Int16.Parse(v(0).Trim), Int16.Parse(v(1).Trim))
            Catch ex As Exception
                Throw New ArgumentException("ConvertFrom���\�b�h�Ŏ��s���܂����B<" & ex.Message & ">")
            End Try
        End If
        Return MyBase.ConvertFrom(context, culture, value)
    End Function

    Public Overloads Overrides Function CanConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal destinationType As System.Type) As Boolean
        If destinationType Is GetType(Design.Serialization.InstanceDescriptor) Then
            Return True
        End If
        If destinationType Is GetType(System.String) Then
            Return True
        End If
        Return MyBase.CanConvertTo(context, destinationType)
    End Function

    Public Overloads Overrides Function ConvertTo(ByVal context As System.ComponentModel.ITypeDescriptorContext, ByVal culture As System.Globalization.CultureInfo, ByVal value As Object, ByVal destinationType As System.Type) As Object
        If destinationType Is GetType(Design.Serialization.InstanceDescriptor) And TypeOf value Is Accuracy Then
            '/// <description>�f�U�C�����̎��������R�[�h�ɂ����āA�R���X�g���N�^�ɂ���ăC���X�^���X���\�킳���</description>
            Dim ac As Accuracy = CType(value, Accuracy)
            Dim ctor As System.Reflection.ConstructorInfo = GetType(Accuracy).GetConstructor(New System.Type() {GetType(Int16), GetType(Int16)})
            If Not ctor Is Nothing Then
                Return New Design.Serialization.InstanceDescriptor(ctor, New Object() {ac.Length, ac.DecimalPart})
            End If
        End If

        If destinationType Is GetType(System.String) Then
            Return (DirectCast(value, Accuracy).Length & ", " & DirectCast(value, Accuracy).DecimalPart)
        End If
        Return MyBase.ConvertTo(context, culture, value, destinationType)
    End Function

End Class
#End Region
