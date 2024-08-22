Imports System.Configuration

Friend Class ConstClass
    Friend Const SAVE_PATH_CONFIG As String = "savePathConfig"
    Friend Const SOURCE As String = "source"
    Friend Const FILE_PATH As String = "filePath"
    Friend Const SAVE_PATH As String = "savePath"
End Class

''' <summary>
''' エレメント
''' </summary>
''' <remarks></remarks>
Friend Class SavePathSourceElement : Inherits ConfigurationElement
    <ConfigurationProperty(ConstClass.FILE_PATH, isRequired:=True, isKey:=True)> _
    Friend Property FilePath() As String
        Get
            Return DirectCast(Me.Item(ConstClass.FILE_PATH), String)
        End Get
        Set(ByVal value As String)
            Me.Item(ConstClass.FILE_PATH) = value
        End Set
    End Property
End Class

''' <summary>
''' セクション
''' </summary>
''' <remarks></remarks>
Friend Class SavePathConfigSection : Inherits ConfigurationSection
    <ConfigurationProperty(ConstClass.SOURCE)> _
    Friend ReadOnly Property SourceElement() As SavePathSourceElement
        Get
            Dim source As SavePathSourceElement = DirectCast(Me.Item(ConstClass.SOURCE), SavePathSourceElement)
            Return source
        End Get
    End Property
End Class

''' <summary>
''' 保存パスConfig
''' </summary>
''' <remarks></remarks>
Public Class SavePathConfiguration
    Private saveFileConfig As Configuration
    Public Sub New()
        Dim filePath As String = DirectCast(ConfigurationManager.GetSection(ConstClass.SAVE_PATH_CONFIG), savePathConfigSection).SourceElement.FilePath
        If IO.File.Exists(filePath) Then
            Dim fileMap As ExeConfigurationFileMap = New ExeConfigurationFileMap()
            fileMap.ExeConfigFilename = DirectCast(ConfigurationManager.GetSection(ConstClass.SAVE_PATH_CONFIG), savePathConfigSection).SourceElement.FilePath
            Me.saveFileConfig = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None)
        Else
            Throw New System.IO.FileNotFoundException
        End If
    End Sub
    Public Property SavePath() As String
        Get
            Return Me.saveFileConfig.AppSettings.Settings(ConstClass.SAVE_PATH).Value
        End Get
        Set(ByVal value As String)
            Me.saveFileConfig.AppSettings.Settings(ConstClass.SAVE_PATH).Value = value
            Me.saveFileConfig.Save()
        End Set
    End Property
End Class
