' ***********************************************************************
' Author   : ElektroStudios
' Modified : 29-June-2024
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " SteamUtil "

' ReSharper disable once CheckNamespace

Namespace DevCase.ThirdParty.Steam

    ''' <summary>
    ''' Contains interoperability features for Steam client.
    ''' </summary>
    '''
    ''' <remarks>
    ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
    ''' <para></para>
    ''' <see href="https://store.steampowered.com/about/">Steam</see>
    ''' </remarks>
    Public NotInheritable Class SteamUtil

#Region " Constructors "

        ''' <summary>
        ''' Prevents a default instance of the <see cref="SteamUtil"/> class from being created.
        ''' </summary>
        '''
        ''' <remarks>
        ''' Note: Some functionalities of this assembly may require to install one or all of the listed applications:
        ''' <para></para>
        ''' <see href="https://store.steampowered.com/about/">Steam</see>
        ''' </remarks>
        <DebuggerNonUserCode>
        Private Sub New()
        End Sub

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Converts a SteamID64 identifier to a SteamID32 (account number) identifier.
        ''' </summary>
        ''' 
        ''' <example> This is a code example.
        ''' <code language="VB.NET">
        ''' Dim steamID64 As ULong = 76561198833246086
        ''' Dim steamID32 As UInteger = ConvertSteamID64ToSteamID32(steamID64) 'Expected Result: 872980358
        ''' Console.WriteLine($"SteamID64: {steamID64}")
        ''' Console.WriteLine($"SteamID32: {steamID32}")
        ''' </code>
        ''' </example>
        '''
        ''' <param name="steamID64">
        ''' The SteamID64 identifier to convert.
        ''' </param>
        ''' 
        ''' <returns>
        ''' The resulting SteamID32 (account number) identifier.
        ''' </returns>
        ''' 
        ''' <remarks>
        ''' Steam IDs are unique identifiers used by the Steam gaming platform to identify users and game content.
        ''' <para></para>
        ''' Steam64 IDs are 64-bit identifiers typically used in programming interfaces, while Steam32 IDs are shorter, 
        ''' human-readable versions often seen in game-related activities.
        ''' <para></para>
        ''' For more information about Steam IDs, visit: <see href="https://developer.valvesoftware.com/wiki/SteamID"/>
        ''' </remarks>
        <DebuggerStepThrough>
        Public Shared Function ConvertSteamID64ToSteamID32(steamID64 As ULong) As UInteger

            Return CUInt(steamID64 - 76561197960265728L)

        End Function

        ''' <summary>
        ''' Converts a SteamID32 (account number) identifier to a SteamID64 identifier.
        ''' </summary>
        ''' 
        ''' <example> This is a code example.
        ''' <code language="VB.NET">
        ''' Dim steamID32 As UInteger = 872980358
        ''' Dim steamID64 As ULong = ConvertSteamID32ToSteamID64(steamID32) 'Expected Result: 76561198833246086
        ''' Console.WriteLine($"SteamID32: {steamID32}")
        ''' Console.WriteLine($"SteamID64: {steamID64}")
        ''' </code>
        ''' </example>
        '''
        ''' <param name="steamID32">
        ''' The SteamID32 (account number) identifier to convert.
        ''' </param>
        ''' 
        ''' <returns>
        ''' The resulting SteamID64 identifier.
        ''' </returns>
        ''' 
        ''' <remarks>
        ''' Steam IDs are unique identifiers used by the Steam gaming platform to identify users and game content.
        ''' <para></para>
        ''' Steam64 IDs are 64-bit identifiers typically used in programming interfaces, while Steam32 IDs are shorter, 
        ''' human-readable versions often seen in game-related activities.
        ''' <para></para>
        ''' For more information about Steam IDs, visit: <see href="https://developer.valvesoftware.com/wiki/SteamID"/>
        ''' </remarks>
        <DebuggerStepThrough>
        Public Shared Function ConvertSteamID32ToSteamID64(steamID32 As UInteger) As ULong

            Return 76561197960265728UL + steamID32

        End Function

#End Region

    End Class

End Namespace

#End Region
