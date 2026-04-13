' ***********************************************************************
' Author   : ElektroStudios
' Modified : 13-April-2026
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Globalization
Imports System.Runtime.InteropServices

Imports SteamIDConverter.Helpers
Imports SteamIDConverter.Win32

#End Region

#Region " Form1 "

#Disable Warning CA1050 ' Declare types in namespaces
Public NotInheritable Class Form1 : Inherits Form
#Enable Warning CA1050 ' Declare types in namespaces

#Region " Fields "

    ''' <summary>
    ''' The URL that points to the GitHub repository of this application.
    ''' </summary>
    Private Const GitHubUrl As String = "https://github.com/ElektroStudios/Steam-ID-Converter"

    ''' <summary>
    ''' A binding list for preset Steam ID values.
    ''' </summary>
    Private ReadOnly Presets As New BindingList(Of KeyValuePair(Of String, String)) From {
        New KeyValuePair(Of String, String)("None", "")
    }

    ''' <summary>
    ''' A dictionary that maps each relevant control with a <see cref="ControlBorderPainter"/> instance
    ''' responsible for rendering its custom border color.
    ''' </summary>
    Private ControlBorderPainterControls As New Dictionary(Of Control, ControlBorderPainter)

    ''' <summary>
    ''' A flag to indicate whether the form load process has completed.
    ''' Used to prevent certain actions from being performed before the form is fully initialized.
    ''' </summary>
    Private formLoadCompleted As Boolean = False

#End Region

#Region " Event Handlers "

    ''' <summary>
    ''' Handles the Load event of the Form1 control.
    ''' </summary>
    ''' 
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.Text = $"{My.Application.Info.Title} v{My.Application.Info.Version.Major}.{My.Application.Info.Version.Minor}.{My.Application.Info.Version.Build}"

        Me.LoadPresetsFromSettings()

        Me.ComboBox_Preset.DataSource = Me.Presets
        Me.ComboBox_Preset.DisplayMember = "Key"
        Me.ComboBox_Preset.ValueMember = "Value"
        Me.ComboBox_Preset.SelectedIndex = 0

        Me.ControlBorderPainterControls = New Dictionary(Of Control, ControlBorderPainter) From {
            {Me.GroupBox_SteamIdInput, Nothing},
            {Me.GroupBox_SteamId32, Nothing},
            {Me.GroupBox_SteamId64, Nothing},
            {Me.Button_TooggleDarkTheme, Nothing},
            {Me.ComboBox_Preset, Nothing},
            {Me.TextBox_CustomID, Nothing},
            {Me.TextBox_SteamId32Dec, Nothing},
            {Me.TextBox_SteamId32Hex, Nothing},
            {Me.TextBox_SteamId64Dec, Nothing},
            {Me.TextBox_SteamId64Hex, Nothing}
        }

        Me.SetVisualTheme()

        Me.BeginInvoke(Sub()
                           ' Toogling button visibility forces proper border color update.
                           Me.Button_TooggleDarkTheme.Visible = False
                           Me.Button_TooggleDarkTheme.Visible = True
                       End Sub)
    End Sub

    ''' <summary>
    ''' Handles the Shown event of the Form1 control.
    ''' </summary>
    ''' 
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Private Sub Form1_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown

        With Me.ErrorProvider1
            .SetIconAlignment(Me.TextBox_CustomID, ErrorIconAlignment.MiddleLeft)
            .SetIconPadding(Me.TextBox_CustomID, 3)

            .SetIconAlignment(Me.LinkLabel_GitHub, ErrorIconAlignment.BottomRight)
            .SetIconPadding(Me.LinkLabel_GitHub, 2)
        End With

        Me.formLoadCompleted = True
    End Sub

    ''' <summary>
    ''' Handles the TextChanged event of the TextBox_InputValue control.
    ''' </summary>
    ''' 
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Private Sub TextBox_InputValue_TextChanged(sender As Object, e As EventArgs) _
    Handles TextBox_CustomID.TextChanged

        Dim tb As TextBox = DirectCast(sender, TextBox)

        Dim success As Boolean = Me.ParseInputSteamID(tb.Text)
        If success Then
            Me.ErrorProvider1.SetError(tb, String.Empty)
        End If
    End Sub

    ''' <summary>
    ''' Handles the TextChanged event of the TextBox_SteamId32Dec, TextBox_SteamId32Hex, 
    ''' TextBox_SteamId64Dec and TextBox_SteamId64Hex controls.
    ''' </summary>
    ''' 
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Private Sub TextBox_SteamId_TextChanged(sender As Object, e As EventArgs) _
    Handles TextBox_SteamId32Dec.TextChanged, TextBox_SteamId32Hex.TextChanged,
            TextBox_SteamId64Dec.TextChanged, TextBox_SteamId64Hex.TextChanged

        Dim tb As TextBox = DirectCast(sender, TextBox)
        tb.Enabled = Not String.IsNullOrEmpty(tb.Text)
    End Sub

    ''' <summary>
    ''' Handles the Click event of the Button_TooggleDarkTheme control.
    ''' </summary>
    ''' 
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Private Sub Button_TooggleDarkTheme_Click(sender As Object, e As EventArgs) Handles Button_TooggleDarkTheme.Click

        My.Settings.UseDarkTheme = Not My.Settings.UseDarkTheme
        My.Settings.Save()

        Me.SetVisualTheme()
    End Sub

    ''' <summary>
    ''' Handles the LinkClicked event of the LinkLabel_GitHub control.
    ''' </summary>
    ''' 
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="LinkLabelLinkClickedEventArgs"/> instance containing the event data.</param>
    Private Sub LinkLabel_GitHub_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel_GitHub.LinkClicked

        Dim lnkLbl As LinkLabel = DirectCast(sender, LinkLabel)
        Me.ErrorProvider1.SetError(lnkLbl, String.Empty)

        Try
            Using pr As New Process
                pr.StartInfo.FileName = Form1.GitHubUrl
                pr.StartInfo.UseShellExecute = True
                pr.Start()
            End Using

        Catch ex As Exception
            Me.ErrorProvider1.SetError(lnkLbl, ex.Message)

        End Try

    End Sub

    ''' <summary>
    ''' Handles the SelectedIndexChanged event of the ComboBox_Preset control.
    ''' </summary>
    ''' 
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Private Sub ComboBox_Preset_SelectedIndexChanged(sender As Object, e As EventArgs) _
    Handles ComboBox_Preset.SelectedIndexChanged

        If Not formLoadCompleted Then
            Return
        End If

        Dim cb As ComboBox = DirectCast(sender, ComboBox)
        Dim selectedPair As KeyValuePair(Of String, String) = Me.Presets.Item(cb.SelectedIndex)

        Dim valueToParse As String = selectedPair.Value
        Dim isValueEmpty As Boolean = String.IsNullOrEmpty(valueToParse)

        Me.Label_CustomID.Visible = isValueEmpty
        Me.TextBox_CustomID.Visible = isValueEmpty

        Me.TextBox_CustomID.Text = valueToParse
        Me.TextBox_CustomID.Enabled = String.IsNullOrEmpty(valueToParse)

        If String.IsNullOrWhiteSpace(valueToParse) Then
            Me.TextBox_CustomID.Focus()
        End If
    End Sub

#End Region

#Region " Private Methods "

    ''' <summary>
    ''' Parses a user-provided Steam ID value in decimal or hexadecimal format,
    ''' determines whether it represents a 32-bit or 64-bit Steam ID, and updates
    ''' the corresponding UI textboxes with both decimal and hexadecimal forms.
    ''' </summary>
    ''' 
    ''' <param name="value">
    ''' The input string containing the Steam ID to parse.
    ''' The value may be:
    ''' <list type="bullet">
    ''' <item>
    ''' <description>A decimal SteamID32 value.</description>
    ''' </item>
    ''' <item>
    ''' <description>A decimal SteamID64 value.</description>
    ''' </item>
    ''' <item>
    ''' <description>A hexadecimal value prefixed with "0x".</description>
    ''' </item>
    ''' <item>
    ''' <description>An empty or whitespace string, which clears all output fields.</description>
    ''' </item>
    ''' </list>
    ''' </param>
    ''' 
    ''' <returns>
    ''' <c>True</c> if the value was successfully parsed or the input was empty;
    ''' otherwise, <c>False</c> if the input format was invalid or conversion failed.
    ''' </returns>
    ''' 
    ''' <remarks>
    ''' <para>
    ''' If the input value is empty or consists only of whitespace, all related
    ''' output textboxes are cleared and the method returns <c>True</c>.
    ''' </para>
    ''' <para>
    ''' If the value starts with the prefix <c>"0x"</c>, it is interpreted as a
    ''' hexadecimal number; otherwise, it is parsed as a decimal number.
    ''' </para>
    ''' <para>
    ''' The method automatically determines whether the parsed value represents
    ''' a SteamID32 or SteamID64 based on its numeric range.
    ''' </para>
    ''' <para>
    ''' On successful parsing, the following UI fields are updated:
    ''' <list type="bullet">
    ''' <item>
    ''' <description>SteamID32 (decimal)</description>
    ''' </item>
    ''' <item>
    ''' <description>SteamID64 (decimal)</description>
    ''' </item>
    ''' <item>
    ''' <description>SteamID32 (hexadecimal)</description>
    ''' </item>
    ''' <item>
    ''' <description>SteamID64 (hexadecimal)</description>
    ''' </item>
    ''' </list>
    ''' </para>
    ''' <para>
    ''' If parsing or conversion fails, an error message is displayed using
    ''' <see cref="ErrorProvider"/>, all output fields are cleared, and
    ''' the method returns <c>False</c>.
    ''' </para>
    ''' </remarks>
    Private Function ParseInputSteamID(value As String) As Boolean

        value = value?.Trim()
        If String.IsNullOrEmpty(value) Then
            Me.ClearTextboxes()
            Return True
        End If

        Dim steamId32Dec As UInteger
        Dim steamId64Dec As ULong

        Dim numericValue As ULong
        Dim numericParseResult As Boolean

        Dim isHexadecimal As Boolean = value.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
        If isHexadecimal Then
            value = value.Substring(2)
            numericParseResult = ULong.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, numericValue)
        Else
            numericParseResult = ULong.TryParse(value, CultureInfo.InvariantCulture, numericValue)
        End If

        If Not numericParseResult OrElse numericValue = 0 Then
            Me.ErrorProvider1.SetError(Me.TextBox_CustomID, "Invalid Steam ID format.")
            Me.ClearTextboxes()
            Return False

        Else
            Dim isSteam32IdLength As Boolean = numericValue < UInteger.MaxValue
            Try
                If isSteam32IdLength Then
                    steamId32Dec = CUInt(numericValue)
                    steamId64Dec = SteamUtil.ConvertSteamID32ToSteamID64(steamId32Dec)
                Else
                    steamId32Dec = SteamUtil.ConvertSteamID64ToSteamID32(numericValue)
                    steamId64Dec = numericValue
                End If
                Me.TextBox_SteamId32Dec.Text = CStr(steamId32Dec)
                Me.TextBox_SteamId64Dec.Text = CStr(steamId64Dec)
                Me.TextBox_SteamId32Hex.Text = $"0x{steamId32Dec:X}"
                Me.TextBox_SteamId64Hex.Text = $"0x{steamId64Dec:X}"

            Catch ex As Exception
                Me.ErrorProvider1.SetError(Me.TextBox_CustomID, "Invalid Steam ID format.")
                Me.ClearTextboxes()
                Return False

            End Try

        End If

        Return True
    End Function

    ''' <summary>
    ''' Clears the Steam ID textboxes.
    ''' </summary>
    Private Sub ClearTextboxes()
        Me.TextBox_SteamId32Dec.Clear()
        Me.TextBox_SteamId32Hex.Clear()

        Me.TextBox_SteamId64Dec.Clear()
        Me.TextBox_SteamId64Hex.Clear()
    End Sub

    ''' <summary>
    ''' Sets the visual theme for the application.
    ''' </summary>
    Private Sub SetVisualTheme()

        If My.Settings.UseDarkTheme Then
            Dim formBackColor As Color = Color.FromArgb(255, 34, 39, 44)
            Dim textboxBackColor As Color = Color.FromArgb(255, 40, 46, 56)
            Dim formForeColor As Color = Color.FromArgb(255, 230, 230, 235)
            Dim groupBoxForeColor As Color = Color.FromArgb(255, 230, 230, 235)
            Dim textboxForeColor As Color = Color.FromArgb(255, 240, 240, 245)
            Dim linkLabelForeColor As Color = Color.FromArgb(255, 86, 156, 214)

            Me.BackColor = formBackColor
            Me.ForeColor = formForeColor

            Me.GroupBox_SteamIdInput.ForeColor = groupBoxForeColor
            Me.GroupBox_SteamId64.ForeColor = groupBoxForeColor
            Me.GroupBox_SteamId32.ForeColor = groupBoxForeColor

            Me.ComboBox_Preset.BackColor = textboxBackColor
            Me.ComboBox_Preset.ForeColor = textboxForeColor
            Me.ComboBox_Preset.FlatStyle = FlatStyle.Flat
            ' Toogling DropDownStyle value forces to recreate the ComboBox handle
            ' to properly reflect the new BackColor.
            Me.ComboBox_Preset.DropDownStyle = ComboBoxStyle.Simple
            Me.ComboBox_Preset.DropDownStyle = ComboBoxStyle.DropDownList

            Me.LinkLabel_GitHub.LinkColor = linkLabelForeColor

            Me.Button_TooggleDarkTheme.BackColor = Color.Transparent
            Me.Button_TooggleDarkTheme.FlatStyle = FlatStyle.Flat
            Me.Button_TooggleDarkTheme.FlatAppearance.BorderSize = 1
            Me.Button_TooggleDarkTheme.FlatAppearance.BorderColor = SystemColors.ControlDarkDark
            Me.Button_TooggleDarkTheme.FlatAppearance.MouseOverBackColor = SystemColors.ControlDark
            ' Toogling button visibility forces proper border color update.
            If Me.formLoadCompleted Then
                Me.Button_TooggleDarkTheme.Visible = False
                Me.Button_TooggleDarkTheme.Visible = True
            End If

            For Each tb As TextBox In {
                Me.TextBox_CustomID,
                Me.TextBox_SteamId32Hex,
                Me.TextBox_SteamId32Dec,
                Me.TextBox_SteamId64Hex,
                Me.TextBox_SteamId64Dec
            }
                tb.BackColor = textboxBackColor
                tb.ForeColor = textboxForeColor
                tb.BorderStyle = BorderStyle.FixedSingle
            Next

            For Each ctrl As Control In Me.ControlBorderPainterControls.Keys
                Dim painter As ControlBorderPainter = Me.ControlBorderPainterControls(ctrl)
                painter?.Dispose()

                Me.ControlBorderPainterControls(ctrl) = New ControlBorderPainter(ctrl, Color.FromArgb(65, 65, 65), borderThickness:=1)
            Next

            Me.SetDarkTitleBar(enabled:=True)
        Else
            Me.BackColor = Form.DefaultBackColor
            Me.ForeColor = Form.DefaultForeColor

            Me.GroupBox_SteamIdInput.ForeColor = GroupBox.DefaultForeColor
            Me.GroupBox_SteamId64.ForeColor = GroupBox.DefaultForeColor
            Me.GroupBox_SteamId32.ForeColor = GroupBox.DefaultForeColor

            Me.ComboBox_Preset.BackColor = ComboBox.DefaultBackColor
            Me.ComboBox_Preset.ForeColor = ComboBox.DefaultForeColor
            Me.ComboBox_Preset.FlatStyle = FlatStyle.Standard

            Me.LinkLabel_GitHub.LinkColor = Color.FromArgb(255, 0, 0, 255)

            Me.Button_TooggleDarkTheme.BackColor = Button.DefaultBackColor
            Me.Button_TooggleDarkTheme.FlatStyle = FlatStyle.Flat
            Me.Button_TooggleDarkTheme.FlatAppearance.BorderSize = 1
            Me.Button_TooggleDarkTheme.FlatAppearance.BorderColor = Button.DefaultBackColor
            Me.Button_TooggleDarkTheme.FlatAppearance.MouseOverBackColor = SystemColors.ControlDarkDark

            For Each tb As TextBox In {
                Me.TextBox_CustomID,
                Me.TextBox_SteamId32Hex,
                Me.TextBox_SteamId32Dec,
                Me.TextBox_SteamId64Hex,
                Me.TextBox_SteamId64Dec
            }
                tb.BackColor = TextBox.DefaultBackColor
                tb.ForeColor = TextBox.DefaultForeColor
                tb.BorderStyle = BorderStyle.Fixed3D
            Next

            For Each ctrl As Control In Me.ControlBorderPainterControls.Keys
                Dim painter As ControlBorderPainter = Me.ControlBorderPainterControls(ctrl)
                painter?.Dispose()
            Next

            Me.SetDarkTitleBar(enabled:=False)
        End If

    End Sub

    ''' <summary>
    ''' Enables or disables the dark mode title bar on this form.
    ''' <para></para>
    ''' Silently does nothing on unsupported Windows versions.
    ''' </summary>
    ''' 
    ''' <param name="enabled">
    ''' <c>True</c> to enable dark title bar; <c>False</c> to disable.
    ''' </param>
    ''' 
    ''' <returns>
    ''' <c>True</c> if the attribute was applied successfully; otherwise, <c>False</c>.
    ''' </returns>
    Private Function SetDarkTitleBar(enabled As Boolean) As Boolean

        Dim flagValue As Integer = If(enabled, 1, 0)
        Dim flagSize As Integer = Marshal.SizeOf(GetType(Integer))

        Dim hResult As Integer =
            NativeMethods.DwmSetWindowAttribute(Me.Handle, Constants.DWMWA_USE_IMMERSIVE_DARK_MODE, flagValue, flagSize)

        If Me.WindowState = FormWindowState.Normal Then
            Dim originalSize As Size = Me.Size
            Me.Size = New Size(originalSize.Width, originalSize.Height + 1)
            Me.Size = originalSize
        End If

        Return hResult = 0
    End Function

    ''' <summary>
    ''' Loads the preset values from <see cref="My.MySettings.PresetValues"/>
    ''' into the <see cref="Form1.Presets"/> binding list.
    ''' </summary>
    Private Sub LoadPresetsFromSettings()

        Dim storedPresets As Specialized.StringCollection = My.Settings.Presets

        If storedPresets Is Nothing Then
            Return
        End If

        For Each rawLine As String In storedPresets

            If String.IsNullOrEmpty(rawLine) Then
                Continue For
            End If

            Dim separatorIndex As Integer = rawLine.IndexOf("|"c)
            If separatorIndex < 0 Then
                Continue For
            End If

            Dim presetName As String = rawLine.Substring(0, separatorIndex)
            Dim presetValue As String = rawLine.Substring(separatorIndex + 1)

            Me.Presets.Add(New KeyValuePair(Of String, String)(presetName, presetValue))
        Next

    End Sub

#End Region

End Class

#End Region
