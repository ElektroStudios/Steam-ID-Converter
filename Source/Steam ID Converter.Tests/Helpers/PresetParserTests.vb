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

Imports System.Collections.ObjectModel
Imports System.Collections.Specialized

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports SteamIDConverter.Helpers

#End Region

#Region " PresetParserTests "

Namespace SteamIDConverter.Tests.Helpers

    ''' <summary>
    ''' Unit tests for the <see cref="PresetParser"/> class.
    ''' </summary>
    <TestClass>
    Public NotInheritable Class PresetParserTests

#Region " Constants "

        ''' <summary>
        ''' The default preset lines declared in the application settings (<c>Settings.settings</c>).
        ''' </summary>
        Private Shared ReadOnly DefaultPresetLines As String() = {
            "Codex (CDX)|1638",
            "DenuvOwO Repacks|22202",
            "Reloaded (RLD)|4919",
            "Tenoke|4660"
        }

#End Region

#Region " TryParsePresetLine "

        ''' <summary>
        ''' Tests that <see cref="PresetParser.TryParsePresetLine"/> splits valid lines into name and value.
        ''' </summary>
        <TestMethod>
        <DataRow("Codex (CDX)|1638", "Codex (CDX)", "1638", DisplayName:="Default preset")>
        <DataRow("Hex preset|0x666", "Hex preset", "0x666", DisplayName:="Hexadecimal value")>
        <DataRow("SteamID64|76561198833246086", "SteamID64", "76561198833246086", DisplayName:="SteamID64 value")>
        <DataRow("Name|Value|Extra", "Name", "Value|Extra", DisplayName:="Value containing separators")>
        <DataRow("|1638", "", "1638", DisplayName:="Empty name")>
        <DataRow("Name|", "Name", "", DisplayName:="Empty value")>
        <DataRow("|", "", "", DisplayName:="Separator only")>
        <DataRow(" Name | 1638 ", " Name ", " 1638 ", DisplayName:="White-spaces are preserved")>
        Public Sub TryParsePresetLine_ValidLine_ReturnsTrueAndExpectedPair(line As String, expectedName As String, expectedValue As String)

            Dim preset As KeyValuePair(Of String, String) = Nothing
            Dim success As Boolean = PresetParser.TryParsePresetLine(line, preset)

            Assert.IsTrue(success)
            Assert.AreEqual(expectedName, preset.Key)
            Assert.AreEqual(expectedValue, preset.Value)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="PresetParser.TryParsePresetLine"/> rejects lines that cannot be parsed.
        ''' </summary>
        <TestMethod>
        <DataRow(Nothing, DisplayName:="Null")>
        <DataRow("", DisplayName:="Empty")>
        <DataRow("Codex (CDX) 1638", DisplayName:="No separator")>
        <DataRow("Codex (CDX):1638", DisplayName:="Wrong separator")>
        Public Sub TryParsePresetLine_InvalidLine_ReturnsFalseAndDefaultPair(line As String)

            Dim preset As New KeyValuePair(Of String, String)("previous", "value")
            Dim success As Boolean = PresetParser.TryParsePresetLine(line, preset)

            Assert.IsFalse(success)
            Assert.IsNull(preset.Key)
            Assert.IsNull(preset.Value)
        End Sub

        ''' <summary>
        ''' Tests that every default preset declared in the application settings
        ''' is parsed and its value is a valid Steam ID.
        ''' </summary>
        <TestMethod>
        Public Sub TryParsePresetLine_DefaultPresets_HaveValidSteamIDs()

            For Each line As String In PresetParserTests.DefaultPresetLines
                Dim preset As KeyValuePair(Of String, String) = Nothing
                Assert.IsTrue(PresetParser.TryParsePresetLine(line, preset), $"Line: {line}")

                Dim result As SteamIdParseResult
                Assert.IsTrue(SteamIdParser.TryParse(preset.Value, result), $"Preset value: {preset.Value}")
            Next
        End Sub

#End Region

#Region " ParsePresetLines "

        ''' <summary>
        ''' Tests that <see cref="PresetParser.ParsePresetLines"/> throws <see cref="ArgumentNullException"/>
        ''' when the input sequence is <see langword="Nothing"/>.
        ''' </summary>
        <TestMethod>
        Public Sub ParsePresetLines_NullSequence_ThrowsArgumentNullException()

            Dim ex As ArgumentNullException =
                Assert.ThrowsExactly(Of ArgumentNullException)(Sub() PresetParser.ParsePresetLines(Nothing))

            Assert.AreEqual("lines", ex.ParamName)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="PresetParser.ParsePresetLines"/> returns an empty list for an empty sequence.
        ''' </summary>
        <TestMethod>
        Public Sub ParsePresetLines_EmptySequence_ReturnsEmptyList()

            Dim presets As ReadOnlyCollection(Of KeyValuePair(Of String, String)) =
                PresetParser.ParsePresetLines(Array.Empty(Of String)())

            Assert.AreEqual(0, presets.Count)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="PresetParser.ParsePresetLines"/> parses the default presets preserving their order.
        ''' </summary>
        <TestMethod>
        Public Sub ParsePresetLines_DefaultPresets_PreservesOrder()

            Dim presets As ReadOnlyCollection(Of KeyValuePair(Of String, String)) =
                PresetParser.ParsePresetLines(PresetParserTests.DefaultPresetLines)

            Assert.AreEqual(4, presets.Count)
            Assert.AreEqual(New KeyValuePair(Of String, String)("Codex (CDX)", "1638"), presets(0))
            Assert.AreEqual(New KeyValuePair(Of String, String)("DenuvOwO Repacks", "22202"), presets(1))
            Assert.AreEqual(New KeyValuePair(Of String, String)("Reloaded (RLD)", "4919"), presets(2))
            Assert.AreEqual(New KeyValuePair(Of String, String)("Tenoke", "4660"), presets(3))
        End Sub

        ''' <summary>
        ''' Tests that <see cref="PresetParser.ParsePresetLines"/> skips invalid lines and keeps the valid ones.
        ''' </summary>
        <TestMethod>
        Public Sub ParsePresetLines_MixedLines_SkipsInvalidLines()

            Dim lines As String() = {
                Nothing,
                "",
                "First|1",
                "no separator",
                "Second|2",
                "Third|"
            }

            Dim presets As ReadOnlyCollection(Of KeyValuePair(Of String, String)) = PresetParser.ParsePresetLines(lines)

            Assert.AreEqual(3, presets.Count)
            Assert.AreEqual("First", presets(0).Key)
            Assert.AreEqual("Second", presets(1).Key)
            Assert.AreEqual("Third", presets(2).Key)
            Assert.AreEqual("", presets(2).Value)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="PresetParser.ParsePresetLines"/> works with a <see cref="StringCollection"/>,
        ''' which is the type used to store the presets in the application settings.
        ''' </summary>
        <TestMethod>
        Public Sub ParsePresetLines_StringCollection_ParsesAllLines()

            Dim collection As New StringCollection()
            collection.AddRange(PresetParserTests.DefaultPresetLines)

            Dim presets As ReadOnlyCollection(Of KeyValuePair(Of String, String)) =
                PresetParser.ParsePresetLines(collection.Cast(Of String)())

            Assert.AreEqual(PresetParserTests.DefaultPresetLines.Length, presets.Count)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="PresetParser.ParsePresetLines"/> keeps duplicated presets,
        ''' as the user configuration file may intentionally contain them.
        ''' </summary>
        <TestMethod>
        Public Sub ParsePresetLines_DuplicatedLines_KeepsDuplicates()

            Dim presets As ReadOnlyCollection(Of KeyValuePair(Of String, String)) =
                PresetParser.ParsePresetLines({"Same|1", "Same|1"})

            Assert.AreEqual(2, presets.Count)
        End Sub

#End Region

    End Class

End Namespace

#End Region
