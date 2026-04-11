' ***********************************************************************
' Author   : ElektroStudios
' Modified : 08-April-2026
' ***********************************************************************

#Region " Option Statements "

Option Explicit On
Option Strict On
Option Infer Off

#End Region

#Region " Imports "

Imports SteamIDConverter.Helpers

#End Region

Namespace My

    <Global.System.Configuration.SettingsProvider(GetType(FlexibleSettingsProvider))>
    Partial Friend NotInheritable Class MySettings

        Public Sub New()
            FlexibleSettingsProvider.BaseDirectoryPath = ".\"
            FlexibleSettingsProvider.DirectoryName = Nothing
            FlexibleSettingsProvider.DirectoryNameFlags = SettingsDirectoryNameFlags.None
            FlexibleSettingsProvider.FileName = "user.config"

            Debug.WriteLine($"Effective config file path: {FlexibleSettingsProvider.EffectiveConfigFilePath}")
        End Sub

    End Class
End Namespace