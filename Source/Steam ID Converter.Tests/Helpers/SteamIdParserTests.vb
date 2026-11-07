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
Imports System.Threading

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports SteamIDConverter.Helpers

#End Region

#Region " SteamIdParserTests "

Namespace SteamIDConverter.Tests.Helpers

    ''' <summary>
    ''' Unit tests for the <see cref="SteamIdParser"/> class.
    ''' </summary>
    <TestClass>
    Public NotInheritable Class SteamIdParserTests

#Region " Argument validation "

        ''' <summary>
        ''' Tests that <see cref="SteamIdParser.TryParse"/> throws <see cref="ArgumentNullException"/>
        ''' when the input value is <see langword="Nothing"/>.
        ''' </summary>
        <TestMethod>
        Public Sub TryParse_NullValue_ThrowsArgumentNullException()

            Dim result As SteamIdParseResult
            Dim ex As ArgumentNullException =
                Assert.ThrowsExactly(Of ArgumentNullException)(Sub() SteamIdParser.TryParse(Nothing, result))

            Assert.AreEqual("value", ex.ParamName)
        End Sub

#End Region

#Region " Valid SteamID32 input "

        ''' <summary>
        ''' Tests that <see cref="SteamIdParser.TryParse"/> parses SteamID32 values written in decimal notation.
        ''' </summary>
        <TestMethod>
        <DataRow("872980358", 872980358UI, 76561198833246086UL, DisplayName:="Documented example")>
        <DataRow("1638", 1638UI, 76561197960267366UL, DisplayName:="Codex (CDX) preset")>
        <DataRow("22202", 22202UI, 76561197960287930UL, DisplayName:="DenuvOwO Repacks preset")>
        <DataRow("4919", 4919UI, 76561197960270647UL, DisplayName:="Reloaded (RLD) preset")>
        <DataRow("4660", 4660UI, 76561197960270388UL, DisplayName:="Tenoke preset")>
        <DataRow("1", 1UI, 76561197960265729UL, DisplayName:="Lowest account number")>
        <DataRow("4294967294", 4294967294UI, 76561202255233022UL, DisplayName:="UInteger.MaxValue minus one")>
        <DataRow("0001638", 1638UI, 76561197960267366UL, DisplayName:="Leading zeros")>
        <DataRow("+1638", 1638UI, 76561197960267366UL, DisplayName:="Leading plus sign")>
        Public Sub TryParse_DecimalSteamID32_ReturnsTrueAndExpectedResult(input As String, expectedSteamID32 As UInteger, expectedSteamID64 As ULong)

            Dim result As SteamIdParseResult
            Dim success As Boolean = SteamIdParser.TryParse(input, result)

            Assert.IsTrue(success)
            Assert.AreEqual(expectedSteamID32, result.SteamID32)
            Assert.AreEqual(expectedSteamID64, result.SteamID64)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="SteamIdParser.TryParse"/> accepts the highest SteamID32 value (<see cref="UInteger.MaxValue"/>).
        ''' <para></para>
        ''' Regression test: this value was previously rejected as invalid,
        ''' although its SteamID64 equivalent was accepted.
        ''' </summary>
        <TestMethod>
        <DataRow("4294967295", DisplayName:="Decimal")>
        <DataRow("0xFFFFFFFF", DisplayName:="Hexadecimal")>
        Public Sub TryParse_MaxSteamID32_ReturnsTrue(input As String)

            Dim result As SteamIdParseResult
            Dim success As Boolean = SteamIdParser.TryParse(input, result)

            Assert.IsTrue(success)
            Assert.AreEqual(UInteger.MaxValue, result.SteamID32)
            Assert.AreEqual(SteamUtil.MaxSteamID64, result.SteamID64)
        End Sub

#End Region

#Region " Valid SteamID64 input "

        ''' <summary>
        ''' Tests that <see cref="SteamIdParser.TryParse"/> parses SteamID64 values written in decimal notation.
        ''' </summary>
        <TestMethod>
        <DataRow("76561198833246086", 872980358UI, 76561198833246086UL, DisplayName:="Documented example")>
        <DataRow("76561197960267366", 1638UI, 76561197960267366UL, DisplayName:="Codex (CDX) preset")>
        <DataRow("76561197960265729", 1UI, 76561197960265729UL, DisplayName:="Lowest SteamID64 (Base plus one)")>
        <DataRow("76561202255233023", 4294967295UI, 76561202255233023UL, DisplayName:="Highest SteamID64")>
        Public Sub TryParse_DecimalSteamID64_ReturnsTrueAndExpectedResult(input As String, expectedSteamID32 As UInteger, expectedSteamID64 As ULong)

            Dim result As SteamIdParseResult
            Dim success As Boolean = SteamIdParser.TryParse(input, result)

            Assert.IsTrue(success)
            Assert.AreEqual(expectedSteamID32, result.SteamID32)
            Assert.AreEqual(expectedSteamID64, result.SteamID64)
        End Sub

#End Region

#Region " Valid hexadecimal input "

        ''' <summary>
        ''' Tests that <see cref="SteamIdParser.TryParse"/> parses values written in hexadecimal notation,
        ''' regardless of the letter case of the prefix and digits.
        ''' </summary>
        <TestMethod>
        <DataRow("0x34089F86", 872980358UI, 76561198833246086UL, DisplayName:="SteamID32, uppercase digits")>
        <DataRow("0x34089f86", 872980358UI, 76561198833246086UL, DisplayName:="SteamID32, lowercase digits")>
        <DataRow("0X34089F86", 872980358UI, 76561198833246086UL, DisplayName:="SteamID32, uppercase prefix")>
        <DataRow("0x666", 1638UI, 76561197960267366UL, DisplayName:="SteamID32, Codex (CDX) preset")>
        <DataRow("0x1", 1UI, 76561197960265729UL, DisplayName:="SteamID32, lowest value")>
        <DataRow("0x00000666", 1638UI, 76561197960267366UL, DisplayName:="SteamID32, leading zeros")>
        <DataRow("0x110000134089F86", 872980358UI, 76561198833246086UL, DisplayName:="SteamID64")>
        <DataRow("0x0110000134089F86", 872980358UI, 76561198833246086UL, DisplayName:="SteamID64, leading zero")>
        <DataRow("0x1100001FFFFFFFF", 4294967295UI, 76561202255233023UL, DisplayName:="SteamID64, highest value")>
        Public Sub TryParse_Hexadecimal_ReturnsTrueAndExpectedResult(input As String, expectedSteamID32 As UInteger, expectedSteamID64 As ULong)

            Dim result As SteamIdParseResult
            Dim success As Boolean = SteamIdParser.TryParse(input, result)

            Assert.IsTrue(success)
            Assert.AreEqual(expectedSteamID32, result.SteamID32)
            Assert.AreEqual(expectedSteamID64, result.SteamID64)
        End Sub

#End Region

#Region " White-space handling "

        ''' <summary>
        ''' Tests that <see cref="SteamIdParser.TryParse"/> ignores leading and trailing white-spaces.
        ''' </summary>
        <TestMethod>
        <DataRow("  1638  ", DisplayName:="Spaces")>
        <DataRow(vbTab & "1638" & vbCrLf, DisplayName:="Tab and new line")>
        <DataRow(" 0x666 ", DisplayName:="Hexadecimal with spaces")>
        Public Sub TryParse_SurroundingWhiteSpace_ReturnsTrue(input As String)

            Dim result As SteamIdParseResult
            Dim success As Boolean = SteamIdParser.TryParse(input, result)

            Assert.IsTrue(success)
            Assert.AreEqual(1638UI, result.SteamID32)
        End Sub

#End Region

#Region " Invalid input "

        ''' <summary>
        ''' Tests that <see cref="SteamIdParser.TryParse"/> returns <see langword="False"/> and a default result
        ''' for invalid input values.
        ''' </summary>
        <TestMethod>
        <DataRow("", DisplayName:="Empty string")>
        <DataRow("   ", DisplayName:="White-spaces only")>
        <DataRow("abc", DisplayName:="Letters")>
        <DataRow("12a", DisplayName:="Digits followed by a letter")>
        <DataRow("1638 1", DisplayName:="Inner white-space")>
        <DataRow("1.5", DisplayName:="Decimal point")>
        <DataRow("1,638", DisplayName:="Thousands separator")>
        <DataRow("-1", DisplayName:="Negative number")>
        <DataRow("0", DisplayName:="Zero")>
        <DataRow("-0", DisplayName:="Negative zero")>
        <DataRow("0x", DisplayName:="Hexadecimal prefix only")>
        <DataRow("0x0", DisplayName:="Hexadecimal zero")>
        <DataRow("0xZZ", DisplayName:="Invalid hexadecimal digits")>
        <DataRow("0x-1", DisplayName:="Negative hexadecimal")>
        <DataRow("x666", DisplayName:="Incomplete hexadecimal prefix")>
        <DataRow("&H666", DisplayName:="VB hexadecimal notation")>
        <DataRow("4294967296", DisplayName:="UInteger.MaxValue plus one")>
        <DataRow("76561197960265727", DisplayName:="SteamID64Base minus one")>
        <DataRow("76561197960265728", DisplayName:="SteamID64Base (account number zero)")>
        <DataRow("0x110000100000000", DisplayName:="SteamID64Base in hexadecimal (account number zero)")>
        <DataRow("76561202255233024", DisplayName:="MaxSteamID64 plus one")>
        <DataRow("18446744073709551615", DisplayName:="ULong.MaxValue")>
        <DataRow("18446744073709551616", DisplayName:="ULong overflow")>
        <DataRow("0x10000000000000000", DisplayName:="Hexadecimal ULong overflow")>
        Public Sub TryParse_InvalidValue_ReturnsFalseAndDefaultResult(input As String)

            Dim result As New SteamIdParseResult(123UI, 456UL)
            Dim success As Boolean = SteamIdParser.TryParse(input, result)

            Assert.IsFalse(success)
            Assert.AreEqual(Nothing, result)
        End Sub

#End Region

#Region " Consistency "

        ''' <summary>
        ''' Tests that the SteamID32 and SteamID64 representations of the same account,
        ''' in decimal and hexadecimal notation, produce the same result.
        ''' </summary>
        <TestMethod>
        <DataRow(1UI)>
        <DataRow(1638UI)>
        <DataRow(872980358UI)>
        <DataRow(4294967295UI)>
        Public Sub TryParse_AllNotationsOfSameAccount_ProduceSameResult(steamID32 As UInteger)

            Dim steamID64 As ULong = SteamUtil.ConvertSteamID32ToSteamID64(steamID32)

            Dim inputs As String() = {
                steamID32.ToString(CultureInfo.InvariantCulture),
                steamID64.ToString(CultureInfo.InvariantCulture),
                $"0x{steamID32.ToString("X", CultureInfo.InvariantCulture)}",
                $"0x{steamID64.ToString("X", CultureInfo.InvariantCulture)}"
            }

            Dim expected As New SteamIdParseResult(steamID32, steamID64)

            For Each input As String In inputs
                Dim result As SteamIdParseResult
                Assert.IsTrue(SteamIdParser.TryParse(input, result), $"Input: {input}")
                Assert.AreEqual(expected, result, $"Input: {input}")
            Next
        End Sub

        ''' <summary>
        ''' Tests that the formatted output of a parse result can be parsed back to the same result.
        ''' </summary>
        <TestMethod>
        <DataRow("1638")>
        <DataRow("76561198833246086")>
        <DataRow("0xFFFFFFFF")>
        Public Sub TryParse_FormattedOutput_RoundTrips(input As String)

            Dim original As SteamIdParseResult
            Assert.IsTrue(SteamIdParser.TryParse(input, original))

            For Each formatted As String In {original.SteamID32Decimal, original.SteamID64Decimal,
                                             original.SteamID32Hexadecimal, original.SteamID64Hexadecimal}
                Dim reparsed As SteamIdParseResult
                Assert.IsTrue(SteamIdParser.TryParse(formatted, reparsed), $"Formatted: {formatted}")
                Assert.AreEqual(original, reparsed, $"Formatted: {formatted}")
            Next
        End Sub

        ''' <summary>
        ''' Tests that <see cref="SteamIdParser.TryParse"/> does not depend on the current thread culture.
        ''' </summary>
        <TestMethod>
        <DataRow("es-ES")>
        <DataRow("ar-SA")>
        <DataRow("fa-IR")>
        <DataRow("tr-TR")>
        Public Sub TryParse_DifferentCultures_ProduceSameResult(cultureName As String)

            Dim originalCulture As CultureInfo = Thread.CurrentThread.CurrentCulture
            Try
                Thread.CurrentThread.CurrentCulture = New CultureInfo(cultureName)

                Dim result As SteamIdParseResult
                Assert.IsTrue(SteamIdParser.TryParse("0x34089F86", result))
                Assert.AreEqual("872980358", result.SteamID32Decimal)
                Assert.AreEqual("76561198833246086", result.SteamID64Decimal)

            Finally
                Thread.CurrentThread.CurrentCulture = originalCulture

            End Try
        End Sub

#End Region

    End Class

End Namespace

#End Region
