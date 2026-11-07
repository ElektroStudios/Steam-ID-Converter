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

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports SteamIDConverter.Helpers

#End Region

#Region " SteamIdParseResultTests "

Namespace SteamIDConverter.Tests.Helpers

    ''' <summary>
    ''' Unit tests for the <see cref="SteamIdParseResult"/> structure.
    ''' </summary>
    <TestClass>
    Public NotInheritable Class SteamIdParseResultTests

#Region " Constructor and properties "

        ''' <summary>
        ''' Tests that the constructor stores the specified identifiers.
        ''' </summary>
        <TestMethod>
        Public Sub Constructor_StoresIdentifiers()

            Dim result As New SteamIdParseResult(872980358UI, 76561198833246086UL)

            Assert.AreEqual(872980358UI, result.SteamID32)
            Assert.AreEqual(76561198833246086UL, result.SteamID64)
        End Sub

        ''' <summary>
        ''' Tests that the formatting properties return the expected decimal and hexadecimal strings.
        ''' </summary>
        <TestMethod>
        <DataRow(872980358UI, 76561198833246086UL, "872980358", "76561198833246086", "0x34089F86", "0x110000134089F86")>
        <DataRow(1638UI, 76561197960267366UL, "1638", "76561197960267366", "0x666", "0x110000100000666")>
        <DataRow(1UI, 76561197960265729UL, "1", "76561197960265729", "0x1", "0x110000100000001")>
        <DataRow(4294967295UI, 76561202255233023UL, "4294967295", "76561202255233023", "0xFFFFFFFF", "0x1100001FFFFFFFF")>
        Public Sub FormattingProperties_ReturnExpectedStrings(steamID32 As UInteger, steamID64 As ULong,
                                                              expected32Dec As String, expected64Dec As String,
                                                              expected32Hex As String, expected64Hex As String)

            Dim result As New SteamIdParseResult(steamID32, steamID64)

            Assert.AreEqual(expected32Dec, result.SteamID32Decimal)
            Assert.AreEqual(expected64Dec, result.SteamID64Decimal)
            Assert.AreEqual(expected32Hex, result.SteamID32Hexadecimal)
            Assert.AreEqual(expected64Hex, result.SteamID64Hexadecimal)
        End Sub

        ''' <summary>
        ''' Tests that the hexadecimal formatting properties start with <see cref="SteamIdParser.HexadecimalPrefix"/>
        ''' and do not contain lowercase hexadecimal digits.
        ''' </summary>
        <TestMethod>
        Public Sub HexadecimalProperties_UsePrefixAndUppercaseDigits()

            Dim result As New SteamIdParseResult(&HABCDEFUI, &H1100001ABCDEFUL)

            StringAssert.StartsWith(result.SteamID32Hexadecimal, SteamIdParser.HexadecimalPrefix, StringComparison.Ordinal)
            StringAssert.StartsWith(result.SteamID64Hexadecimal, SteamIdParser.HexadecimalPrefix, StringComparison.Ordinal)
            Assert.AreEqual("0xABCDEF", result.SteamID32Hexadecimal)
            Assert.AreEqual("0x1100001ABCDEF", result.SteamID64Hexadecimal)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="SteamIdParseResult.ToString"/> includes both identifiers.
        ''' </summary>
        <TestMethod>
        Public Sub ToString_ContainsBothIdentifiers()

            Dim result As New SteamIdParseResult(872980358UI, 76561198833246086UL)

            Assert.AreEqual("SteamID32: 872980358, SteamID64: 76561198833246086", result.ToString())
        End Sub

#End Region

#Region " Equality "

        ''' <summary>
        ''' Tests that two instances with the same identifiers are equal by every equality member.
        ''' </summary>
        <TestMethod>
        Public Sub Equality_SameIdentifiers_AreEqual()

            Dim a As New SteamIdParseResult(1638UI, 76561197960267366UL)
            Dim b As New SteamIdParseResult(1638UI, 76561197960267366UL)

            Assert.IsTrue(a.Equals(b))
            Assert.IsTrue(a.Equals(CObj(b)))
            Assert.IsTrue(a = b)
            Assert.IsFalse(a <> b)
            Assert.AreEqual(a.GetHashCode(), b.GetHashCode())
        End Sub

        ''' <summary>
        ''' Tests that two instances with different identifiers are not equal.
        ''' </summary>
        <TestMethod>
        <DataRow(1639UI, 76561197960267366UL, DisplayName:="Different SteamID32")>
        <DataRow(1638UI, 76561197960267367UL, DisplayName:="Different SteamID64")>
        <DataRow(0UI, 0UL, DisplayName:="Default values")>
        Public Sub Equality_DifferentIdentifiers_AreNotEqual(steamID32 As UInteger, steamID64 As ULong)

            Dim a As New SteamIdParseResult(1638UI, 76561197960267366UL)
            Dim b As New SteamIdParseResult(steamID32, steamID64)

            Assert.IsFalse(a.Equals(b))
            Assert.IsFalse(a = b)
            Assert.IsTrue(a <> b)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="SteamIdParseResult.Equals(Object)"/> returns <see langword="False"/>
        ''' for <see langword="Nothing"/> and for objects of a different type.
        ''' </summary>
        <TestMethod>
        Public Sub EqualsObject_NullOrDifferentType_ReturnsFalse()

            Dim result As New SteamIdParseResult(1638UI, 76561197960267366UL)

            Assert.IsFalse(result.Equals(CObj(Nothing)))
            Assert.IsFalse(result.Equals(CObj("1638")))
            Assert.IsFalse(result.Equals(CObj(1638UI)))
        End Sub

        ''' <summary>
        ''' Tests that the default instance holds zero identifiers.
        ''' </summary>
        <TestMethod>
        Public Sub DefaultInstance_HoldsZeroIdentifiers()

            Dim result As SteamIdParseResult = Nothing

            Assert.AreEqual(0UI, result.SteamID32)
            Assert.AreEqual(0UL, result.SteamID64)
            Assert.AreEqual(New SteamIdParseResult(0UI, 0UL), result)
        End Sub

#End Region

    End Class

End Namespace

#End Region
