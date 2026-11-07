' ***********************************************************************
' Author   : ElektroStudios
' Modified : 07-November-2026
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " SteamUtil "

' ReSharper disable once CheckNamespace

Namespace SteamIDConverter.Helpers

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

#Region " Constants "

        ''' <summary>
        ''' The SteamID64 value that corresponds to the SteamID32 (account number) zero 
        ''' for an individual account in the public universe (<c>0x0110000100000000</c>).
        ''' <para></para>
        ''' Any SteamID64 of an individual account is the sum of this value and its SteamID32 (account number).
        ''' </summary>
        Public Const SteamID64Base As ULong = 76561197960265728UL

        ''' <summary>
        ''' The highest SteamID64 value that can be converted to a SteamID32 (account number);
        ''' it is the sum of <see cref="SteamUtil.SteamID64Base"/> and <see cref="UInteger.MaxValue"/>.
        ''' </summary>
        Public Const MaxSteamID64 As ULong = SteamUtil.SteamID64Base + UInteger.MaxValue

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
        ''' 
        ''' <exception cref="ArgumentOutOfRangeException">
        ''' Thrown when <paramref name="steamID64"/> is lower than <see cref="SteamUtil.SteamID64Base"/> 
        ''' or greater than <see cref="SteamUtil.MaxSteamID64"/>.
        ''' </exception>
        <DebuggerStepThrough>
        Public Shared Function ConvertSteamID64ToSteamID32(steamID64 As ULong) As UInteger

            If steamID64 < SteamUtil.SteamID64Base OrElse steamID64 > SteamUtil.MaxSteamID64 Then
                Throw New ArgumentOutOfRangeException(NameOf(steamID64), steamID64,
                    $"The SteamID64 value must be in range {SteamUtil.SteamID64Base} to {SteamUtil.MaxSteamID64}.")
            End If

            Return CUInt(steamID64 - SteamUtil.SteamID64Base)
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

            Return SteamUtil.SteamID64Base + steamID32
        End Function

#End Region

    End Class

End Namespace

#End Region
