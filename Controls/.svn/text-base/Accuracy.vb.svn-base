Imports System.ComponentModel
Imports System.Drawing.Design

'/// <summary>数値精度をOracleライクに表現するクラス</summary>
<TypeConverter(GetType(AccuracyConverter)), Serializable()> _
Public Class Accuracy

#Region " メンバ変数・定数・イベント "
    Private mLength As Int16
    Private mDecimal As Int16
#End Region

#Region " プロパティ "
    '/// <value>精度：全体の桁数</value>
    <Category("あつらえ"), _
    RefreshPropertiesAttribute(RefreshProperties.All), _
    Description("精度：全体の桁数"), Bindable(True)> _
    Public Property Length() As Int16
        Get
            Return mLength
        End Get
        Set(ByVal Value As Int16)
            CheckValue(Value, mDecimal)
            mLength = Value
        End Set
    End Property
    '/// <value>精度：小数部の桁数</value>
    <Category("あつらえ"), _
    RefreshPropertiesAttribute(RefreshProperties.All), _
    Description("精度：小数部の桁数"), Bindable(True)> _
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

#Region " コンストラクタ "
    Public Sub New()
        Me.New(7, 0)
    End Sub
    '/// <summary>コンストラクタ（オーバーロード）</summary>
    '/// <param name="L">全体の桁数</param>
    '/// <param name="D">小数部の桁数</param>
    '/// <returns></returns>
    Public Sub New(ByVal L As Int16, ByVal D As Int16)
        MyBase.New()
        Length = L
        DecimalPart = D
    End Sub
#End Region

#Region " 外部メソッド "
    Public Overrides Function ToString() As String
        Return mLength.ToString & ", " & mDecimal.ToString
    End Function
#End Region

#Region " 内部メソッド "
    '/// <summary>プロパティの整合性チェック</summary>
    '/// <param name="len">全体の桁数</param>
    '/// <param name="dec">小数部の桁数</param>
    '/// <returns></returns>
    Private Sub CheckValue(ByVal len As Int16, ByVal dec As Int16)
        Dim Msg As String = "Length<" & len.ToString & ">, DecimalPart<" & dec.ToString & ">" & System.Environment.NewLine
        If len < 0 OrElse dec < 0 Then Throw New ArgumentOutOfRangeException(Msg & "正の整数を指定してください。")
        If len <= dec Then Throw New ArgumentOutOfRangeException(Msg & "小数部が桁数と等しいか、桁数より大きいです。")
    End Sub
#End Region

End Class

#Region " カスタムタイプコンバータ "
'/// <summary>数値精度をOracleライクに表現するクラスのタイプコンバータ</summary>
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
                Throw New ArgumentException("ConvertFromメソッドで失敗しました。<" & ex.Message & ">")
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
            '/// <description>デザイン時の自動生成コードにおいて、コンストラクタによってインスタンスが表わされる</description>
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
