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

#Region " SteamIdParseResult "

' ReSharper disable once CheckNamespace

Namespace SteamIDConverter.Helpers

    ''' <summary>
    ''' Represents the result of a successful <see cref="SteamIdParser.TryParse"/> operation,
    ''' exposing a Steam ID in both SteamID32 (account number) and SteamID64 formats.
    ''' </summary>
    <DebuggerDisplay("SteamID32 = {SteamID32}, SteamID64 = {SteamID64}")>
    Public Structure SteamIdParseResult : Implements IEquatable(Of SteamIdParseResult)

#Region " Properties "

        ''' <summary>
        ''' Gets the SteamID32 (account number) identifier.
        ''' </summary>
        Public ReadOnly Property SteamID32 As UInteger

        ''' <summary>
        ''' Gets the SteamID64 identifier.
        ''' </summary>
        Public ReadOnly Property SteamID64 As ULong

        ''' <summary>
        ''' Gets the SteamID32 (account number) identifier formatted as a decimal string;
        ''' For example, <b>"872980358"</b>.
        ''' </summary>
        Public ReadOnly Property SteamID32Decimal As String
            <DebuggerStepThrough>
            Get
                Return Me.SteamID32.ToString(CultureInfo.InvariantCulture)
            End Get
        End Property

        ''' <summary>
        ''' Gets the SteamID64 identifier formatted as a decimal string;
        ''' For example, <b>"76561198833246086"</b>.
        ''' </summary>
        Public ReadOnly Property SteamID64Decimal As String
            <DebuggerStepThrough>
            Get
                Return Me.SteamID64.ToString(CultureInfo.InvariantCulture)
            End Get
        End Property

        ''' <summary>
        ''' Gets the SteamID32 (account number) identifier formatted as an
        ''' uppercase hexadecimal string prefixed with <c>"0x"</c>; For example, <b>"0x34089F86"</b>.
        ''' </summary>
        Public ReadOnly Property SteamID32Hexadecimal As String
            <DebuggerStepThrough>
            Get
                Return $"{SteamIdParser.HexadecimalPrefix}{Me.SteamID32.ToString("X", CultureInfo.InvariantCulture)}"
            End Get
        End Property

        ''' <summary>
        ''' Gets the SteamID64 identifier formatted as an
        ''' uppercase hexadecimal string prefixed with <c>"0x"</c>; For example, <b>"0x110000134089F86"</b>.
        ''' </summary>
        Public ReadOnly Property SteamID64Hexadecimal As String
            <DebuggerStepThrough>
            Get
                Return $"{SteamIdParser.HexadecimalPrefix}{Me.SteamID64.ToString("X", CultureInfo.InvariantCulture)}"
            End Get
        End Property

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="SteamIdParseResult"/> structure.
        ''' </summary>
        '''
        ''' <param name="steamID32">
        ''' The SteamID32 (account number) identifier.
        ''' </param>
        '''
        ''' <param name="steamID64">
        ''' The SteamID64 identifier.
        ''' </param>
        <DebuggerStepThrough>
        Public Sub New(steamID32 As UInteger, steamID64 As ULong)

            Me.SteamID32 = steamID32
            Me.SteamID64 = steamID64
        End Sub

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Determines whether this instance is equal to another <see cref="SteamIdParseResult"/> instance.
        ''' </summary>
        '''
        ''' <param name="other">
        ''' The other <see cref="SteamIdParseResult"/> to compare with this instance.
        ''' </param>
        '''
        ''' <returns>
        ''' <see langword="True"/> if both instances hold the same identifiers; otherwise, <see langword="False"/>.
        ''' </returns>
        <DebuggerStepThrough>
        Public Overloads Function Equals(other As SteamIdParseResult) As Boolean _
        Implements IEquatable(Of SteamIdParseResult).Equals

            Return Me.SteamID32 = other.SteamID32 AndAlso Me.SteamID64 = other.SteamID64
        End Function

        ''' <summary>
        ''' Determines whether this instance is equal to the specified object.
        ''' </summary>
        '''
        ''' <param name="obj">
        ''' The object to compare with this instance.
        ''' </param>
        '''
        ''' <returns>
        ''' <see langword="True"/> if <paramref name="obj"/> is a <see cref="SteamIdParseResult"/>
        ''' that holds the same identifiers; otherwise, <see langword="False"/>.
        ''' </returns>
        <DebuggerStepThrough>
        Public Overrides Function Equals(obj As Object) As Boolean

            Return TypeOf obj Is SteamIdParseResult AndAlso Me.Equals(DirectCast(obj, SteamIdParseResult))
        End Function

        ''' <summary>
        ''' Returns the hash code for this instance.
        ''' </summary>
        '''
        ''' <returns>
        ''' A 32-bit signed integer hash code.
        ''' </returns>
        <DebuggerStepThrough>
        Public Overrides Function GetHashCode() As Integer

            Return HashCode.Combine(Me.SteamID32, Me.SteamID64)
        End Function

        ''' <summary>
        ''' Returns a string that represents this instance.
        ''' </summary>
        '''
        ''' <returns>
        ''' A string that represents this instance; For example, <b>"SteamID32: 872980358, SteamID64: 76561198833246086"</b>.
        ''' </returns>
        <DebuggerStepThrough>
        Public Overrides Function ToString() As String

            Return $"SteamID32: {Me.SteamID32Decimal}, SteamID64: {Me.SteamID64Decimal}"
        End Function

#End Region

#Region " Operators "

        ''' <summary>
        ''' Determines whether two <see cref="SteamIdParseResult"/> instances are equal.
        ''' </summary>
        '''
        ''' <param name="left">The first instance to compare.</param>
        ''' <param name="right">The second instance to compare.</param>
        '''
        ''' <returns>
        ''' <see langword="True"/> if both instances hold the same identifiers; otherwise, <see langword="False"/>.
        ''' </returns>
        Public Shared Operator =(left As SteamIdParseResult, right As SteamIdParseResult) As Boolean
            Return left.Equals(right)
        End Operator

        ''' <summary>
        ''' Determines whether two <see cref="SteamIdParseResult"/> instances are not equal.
        ''' </summary>
        '''
        ''' <param name="left">The first instance to compare.</param>
        ''' <param name="right">The second instance to compare.</param>
        '''
        ''' <returns>
        ''' <see langword="True"/> if the instances hold different identifiers; otherwise, <see langword="False"/>.
        ''' </returns>
        Public Shared Operator <>(left As SteamIdParseResult, right As SteamIdParseResult) As Boolean
            Return Not left.Equals(right)
        End Operator

#End Region

    End Structure

End Namespace

#End Region
