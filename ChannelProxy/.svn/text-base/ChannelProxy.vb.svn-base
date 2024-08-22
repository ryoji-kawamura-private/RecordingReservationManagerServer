Imports System.Net
Imports System.ServiceModel

Public Class ChannelProxy(Of T) : Implements IDisposable
    ''' <summary>チャンネルファクトリ</summary>
    Protected channelFactory As ChannelFactory(Of T)
    ''' <summary>Webサービス</summary>
    Protected _Channel As T
    Public ReadOnly Property Channel() As T
        Get
            Return _Channel
        End Get
    End Property
    Public Sub New(ByVal serviceName As String)
        Me.channelFactory = New ChannelFactory(Of T)(serviceName)
        'Me.channelFactory.Credentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation
        'Me.channelFactory.Credentials.Windows.AllowNtlm = False
        Me.channelFactory.Credentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials
        Me._Channel = channelFactory.CreateChannel()
    End Sub
    Public Sub Dispose() Implements IDisposable.Dispose
        Dim channel As ICommunicationObject
        Try
            If Me._Channel IsNot Nothing Then
                channel = DirectCast(Me._Channel, ICommunicationObject)
                If channel.State = CommunicationState.Faulted Then
                    channel.Abort()
                Else
                    channel.Close()
                End If
            End If
        Catch
            If Me._Channel IsNot Nothing Then
                channel = DirectCast(Me._Channel, ICommunicationObject)
                channel.Abort()
            End If
            Throw
        Finally
            Try
                If Me.channelFactory IsNot Nothing Then
                    If channelFactory.State = CommunicationState.Faulted Then
                        channelFactory.Abort()
                    Else
                        channelFactory.Close()
                    End If

                End If
            Catch
                If Me.channelFactory IsNot Nothing Then
                    channelFactory.Abort()
                End If
                Throw
            End Try
        End Try
    End Sub
End Class
