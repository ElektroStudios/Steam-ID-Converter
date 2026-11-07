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

#End Region

#Region " PresetParser "

' ReSharper disable once CheckNamespace

Namespace SteamIDConverter.Helpers

    ''' <summary>
    ''' Provides parsing of the Steam ID preset lines stored in the user configuration file,
    ''' where each line has the format <c>"Name|Value"</c>; For example, <b>"Codex (CDX)|1638"</b>.
    ''' </summary>
    Public NotInheritable Class PresetParser

#Region " Constants "

        ''' <summary>
        ''' The character that separates the preset name from the preset value.
        ''' </summary>
        Public Const Separator As Char = "|"c

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Prevents a default instance of the <see cref="PresetParser"/> class from being created.
        ''' </summary>
        <DebuggerNonUserCode>
        Private Sub New()
        End Sub

#End Region

#Region " Public Methods "

        ''' <summary>
        ''' Tries to parse a single preset line with the format <c>"Name|Value"</c>.
        ''' </summary>
        '''
        ''' <param name="line">
        ''' The preset line to parse.
        ''' <para></para>
        ''' Only the first <see cref="PresetParser.Separator"/> character is considered,
        ''' so the value part may contain additional separator characters.
        ''' </param>
        '''
        ''' <param name="preset">
        ''' When this method returns <see langword="True"/>, contains the preset name as the key and
        ''' the preset value as the value; otherwise, contains the default value of <see cref="KeyValuePair(Of String, String)"/>.
        ''' </param>
        '''
        ''' <returns>
        ''' <see langword="True"/> if <paramref name="line"/> was parsed successfully;
        ''' otherwise, <see langword="False"/> if the line is <see langword="Nothing"/>, empty,
        ''' or does not contain the <see cref="PresetParser.Separator"/> character.
        ''' </returns>
        <DebuggerStepThrough>
        Public Shared Function TryParsePresetLine(line As String, ByRef preset As KeyValuePair(Of String, String)) As Boolean

            preset = Nothing

            If String.IsNullOrEmpty(line) Then
                Return False
            End If

            Dim separatorIndex As Integer = line.IndexOf(PresetParser.Separator)
            If separatorIndex < 0 Then
                Return False
            End If

            Dim presetName As String = line.Substring(0, separatorIndex)
            Dim presetValue As String = line.Substring(separatorIndex + 1)

            preset = New KeyValuePair(Of String, String)(presetName, presetValue)
            Return True
        End Function

        ''' <summary>
        ''' Parses a sequence of preset lines with the format <c>"Name|Value"</c>,
        ''' skipping any line that cannot be parsed by <see cref="PresetParser.TryParsePresetLine"/>.
        ''' </summary>
        '''
        ''' <param name="lines">
        ''' The preset lines to parse.
        ''' </param>
        '''
        ''' <returns>
        ''' A read-only list containing the successfully parsed presets, in the same order as they appear in <paramref name="lines"/>.
        ''' </returns>
        '''
        ''' <exception cref="ArgumentNullException">
        ''' Thrown when <paramref name="lines"/> is <see langword="Nothing"/>.
        ''' </exception>
        <DebuggerStepThrough>
        Public Shared Function ParsePresetLines(lines As IEnumerable(Of String)) As ReadOnlyCollection(Of KeyValuePair(Of String, String))

            If lines Is Nothing Then
                Throw New ArgumentNullException(NameOf(lines))
            End If

            Dim presets As New List(Of KeyValuePair(Of String, String))

            For Each line As String In lines
                Dim preset As KeyValuePair(Of String, String) = Nothing
                If PresetParser.TryParsePresetLine(line, preset) Then
                    presets.Add(preset)
                End If
            Next

            Return presets.AsReadOnly()
        End Function

#End Region

    End Class

End Namespace

#End Region
