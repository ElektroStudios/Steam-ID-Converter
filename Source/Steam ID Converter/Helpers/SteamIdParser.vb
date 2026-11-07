' ***********************************************************************
' Author   : ElektroStudios
' Modified : 07-November-2026
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.Globalization

#End Region

#Region " SteamIdParser "

' ReSharper disable once CheckNamespace

Namespace SteamIDConverter.Helpers

    ''' <summary>
    ''' Provides parsing of user-provided Steam ID values written in decimal or hexadecimal notation,
    ''' automatically determining whether the value represents a SteamID32 (account number) or a SteamID64.
    ''' </summary>
    Public NotInheritable Class SteamIdParser

#Region " Constants "

        ''' <summary>
        ''' The prefix that identifies a hexadecimal value (case-insensitive).
        ''' </summary>
        Public Const HexadecimalPrefix As String = "0x"

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Prevents a default instance of the <see cref="SteamIdParser"/> class from being created.
        ''' </summary>
        <DebuggerNonUserCode>
        Private Sub New()
        End Sub

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Tries to parse a user-provided Steam ID value in decimal or hexadecimal format,
        ''' and determines whether it represents a SteamID32 (account number) or a SteamID64.
        ''' </summary>
        '''
        ''' <example> This is a code example.
        ''' <code language="VB.NET">
        ''' Dim result As SteamIdParseResult
        ''' If SteamIdParser.TryParse("0x34089F86", result) Then
        '''     Console.WriteLine(result.SteamID32Decimal) ' 872980358
        '''     Console.WriteLine(result.SteamID64Decimal) ' 76561198833246086
        ''' End If
        ''' </code>
        ''' </example>
        '''
        ''' <param name="value">
        ''' The input string containing the Steam ID to parse. Leading and trailing white-spaces are ignored.
        ''' <para></para>
        ''' The value may be a decimal SteamID32, a decimal SteamID64,
        ''' or any of them in hexadecimal notation prefixed with <c>"0x"</c> (case-insensitive).
        ''' </param>
        '''
        ''' <param name="result">
        ''' When this method returns <see langword="True"/>, contains the parsed Steam ID in both formats;
        ''' otherwise, contains the default value of <see cref="SteamIdParseResult"/>.
        ''' </param>
        '''
        ''' <returns>
        ''' <see langword="True"/> if <paramref name="value"/> was parsed successfully; otherwise, <see langword="False"/>.
        ''' <para></para>
        ''' The method returns <see langword="False"/> for empty or white-space strings, non-numeric strings,
        ''' a zero account number, and numeric values that are neither a valid SteamID32
        ''' nor a SteamID64 within the range
        ''' (<see cref="SteamUtil.SteamID64Base"/> + 1) to <see cref="SteamUtil.MaxSteamID64"/>.
        ''' </returns>
        '''
        ''' <remarks>
        ''' Any numeric value from 1 to <see cref="UInteger.MaxValue"/> (both inclusive) is treated as a SteamID32;
        ''' greater values are treated as a SteamID64.
        ''' </remarks>
        '''
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="value"/> is <see langword="Nothing"/>.
        ''' </exception>
        <DebuggerStepThrough>
        Public Shared Function TryParse(value As String, ByRef result As SteamIdParseResult) As Boolean

            If value Is Nothing Then
                Throw New ArgumentNullException(NameOf(value))
            End If

            result = Nothing

            Dim trimmedValue As String = value.Trim()
            If trimmedValue.Length = 0 Then
                Return False
            End If

            Dim numericValue As ULong
            Dim numericParseResult As Boolean

            If trimmedValue.StartsWith(SteamIdParser.HexadecimalPrefix, StringComparison.OrdinalIgnoreCase) Then
                Dim hexDigits As String = trimmedValue.Substring(SteamIdParser.HexadecimalPrefix.Length)
                numericParseResult = ULong.TryParse(hexDigits, NumberStyles.HexNumber, CultureInfo.InvariantCulture, numericValue)
            Else
                numericParseResult = ULong.TryParse(trimmedValue, NumberStyles.Integer, CultureInfo.InvariantCulture, numericValue)
            End If

            If Not numericParseResult OrElse numericValue = 0UL Then
                Return False
            End If

            Dim steamID32 As UInteger
            Dim steamID64 As ULong

            If numericValue <= UInteger.MaxValue Then
                steamID32 = CUInt(numericValue)
                steamID64 = SteamUtil.ConvertSteamID32ToSteamID64(steamID32)

            ElseIf numericValue > SteamUtil.SteamID64Base AndAlso numericValue <= SteamUtil.MaxSteamID64 Then
                steamID32 = SteamUtil.ConvertSteamID64ToSteamID32(numericValue)
                steamID64 = numericValue

            Else
                Return False

            End If

            result = New SteamIdParseResult(steamID32, steamID64)
            Return True
        End Function

#End Region

    End Class

End Namespace

#End Region
