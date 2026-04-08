' ***********************************************************************
' Author   : ElektroStudios
' Modified : 08-April-2026
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

Imports SteamIDConverter
Imports SteamIDConverter.Win32

#End Region

#Region " Form1 "

Public NotInheritable Class Form1 : Inherits Form

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
    ''' Runtime border painter attached to <see cref="Form1.ComboBox_Preset"/>.
    ''' </summary>
    Private comboBoxPresetBorderPainter As ComboBoxBorderPainter

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

        For Each gb As GroupBox In {
            Me.GroupBox_SteamIdInput,
            Me.GroupBox_SteamId32,
            Me.GroupBox_SteamId64
        }
            AddHandler gb.Paint, AddressOf Me.GroupBox_PaintCustomBorder
        Next

        Me.SetVisualTheme()
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
        Me.ErrorProvider1.SetError(tb, String.Empty)
        Me.ParseInputSteamID(tb.Text)
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

        Dim cb As ComboBox = DirectCast(sender, ComboBox)
        Dim selectedPair As KeyValuePair(Of String, String) = Me.Presets.Item(cb.SelectedIndex)

        Dim valueToParse As String = selectedPair.Value
        Dim isValueEmpty As Boolean = String.IsNullOrEmpty(valueToParse)

        Me.Label_CustomID.Visible = isValueEmpty
        Me.TextBox_CustomID.Visible = isValueEmpty

        Me.TextBox_CustomID.Text = valueToParse
        Me.TextBox_CustomID.Enabled = String.IsNullOrEmpty(valueToParse)
    End Sub

    ''' <summary>
    ''' Custom paint handler that draws a <see cref="GroupBox"/> with a 
    ''' configurable border color, instead of the default system theme color.
    ''' </summary>
    ''' 
    ''' <param name="sender">
    ''' The <see cref="GroupBox"/> being painted.
    ''' </param>
    ''' 
    ''' <param name="e">
    ''' The <see cref="PaintEventArgs"/> instance containing the event data.
    ''' </param>
    Private Sub GroupBox_PaintCustomBorder(sender As Object, e As PaintEventArgs)

        Dim gb As GroupBox = DirectCast(sender, GroupBox)
        Dim g As Graphics = e.Graphics

        Dim borderColor As Color =
            If(My.Settings.UseDarkTheme,
               Color.FromArgb(255, 60, 68, 90),
               Color.Gray)

        Dim titleText As String = If(gb.Text, String.Empty)
        Dim titleFont As Font = gb.Font
        Dim titleSize As SizeF = g.MeasureString(titleText, titleFont)

        Dim titleHeightInt As Integer = CInt(Math.Ceiling(CDbl(titleSize.Height)))
        Dim titleWidthInt As Integer = CInt(Math.Ceiling(CDbl(titleSize.Width)))
        Dim titleHalfHeight As Integer = titleHeightInt \ 2

        ' Erase the entire control surface with its BackColor.
        Using backBrush As New SolidBrush(gb.BackColor)
            g.FillRectangle(backBrush, gb.ClientRectangle)
        End Using

        ' Draw the custom border rectangle.
        Dim borderRect As New Rectangle(0, titleHalfHeight, gb.Width - 1, gb.Height - titleHalfHeight - 1)

        Using borderPen As New Pen(borderColor, 1.0F)
            g.DrawRectangle(borderPen, borderRect)
        End Using

        ' Erase the line section behind the title and draw the title text.
        If Not String.IsNullOrEmpty(titleText) Then

            Dim titleBackRect As New Rectangle(8, 0, titleWidthInt, titleHeightInt)
            Using titleBackBrush As New SolidBrush(gb.BackColor)
                g.FillRectangle(titleBackBrush, titleBackRect)
            End Using

            Using titleBrush As New SolidBrush(gb.ForeColor)
                g.DrawString(titleText, titleFont, titleBrush, 8.0F, 0.0F)
            End Using

        End If

    End Sub

    ''' <summary>
    ''' Custom draw handler for <see cref="ComboBox_Preset"/> that paints each item
    ''' (including the selected value shown in the closed combo) with theme colors.
    ''' </summary>
    ''' 
    ''' <param name="sender">
    ''' The <see cref="ComboBox"/> being drawn.
    ''' </param>
    ''' 
    ''' <param name="e">
    ''' The <see cref="DrawItemEventArgs"/> instance containing the event data.
    ''' </param>
    Private Sub ComboBox_Preset_DrawItem(sender As Object, e As DrawItemEventArgs)

        Dim cb As ComboBox = DirectCast(sender, ComboBox)

        ' Decide colors based on current theme.
        Dim itemBackColor As Color
        Dim itemForeColor As Color
        Dim selectedBackColor As Color

        If My.Settings.UseDarkTheme Then
            itemBackColor = Color.FromArgb(255, 39, 44, 60)
            itemForeColor = Color.FromArgb(255, 220, 223, 235)
            selectedBackColor = Color.FromArgb(255, 60, 68, 90)
        Else
            itemBackColor = SystemColors.Window
            itemForeColor = SystemColors.ControlText
            selectedBackColor = SystemColors.Highlight
        End If

        ' Determine whether this item is the highlighted one in the dropdown list.
        Dim isSelected As Boolean = (e.State And DrawItemState.Selected) = DrawItemState.Selected
        Dim backColor As Color = If(isSelected, selectedBackColor, itemBackColor)

        ' Fill the item background.
        Using backBrush As New SolidBrush(backColor)
            e.Graphics.FillRectangle(backBrush, e.Bounds)
        End Using

        ' Resolve the display text for the item.
        Dim itemText As String = String.Empty
        If e.Index >= 0 AndAlso e.Index < cb.Items.Count Then
            Dim rawItem As Object = cb.Items.Item(e.Index)
            If TypeOf rawItem Is KeyValuePair(Of String, String) Then
                Dim pair As KeyValuePair(Of String, String) = DirectCast(rawItem, KeyValuePair(Of String, String))
                itemText = pair.Key
            Else
                itemText = If(rawItem?.ToString(), String.Empty)
            End If
        End If

        ' Draw the item text with a small left padding.
        Using foreBrush As New SolidBrush(itemForeColor)
            Dim textRect As New Rectangle(
            e.Bounds.X + 3,
            e.Bounds.Y + 1,
            e.Bounds.Width - 3,
            e.Bounds.Height - 1)
            e.Graphics.DrawString(itemText, cb.Font, foreBrush, textRect)
        End Using

        ' Draw the focus rectangle if needed.
        e.DrawFocusRectangle()

    End Sub

#End Region

#Region " Private Methods "

    ''' <summary>
    ''' Parses the input value.
    ''' </summary>
    ''' 
    ''' <param name="value">The value.</param>
    Private Sub ParseInputSteamID(value As String)

        value = value.Trim()
        If String.IsNullOrEmpty(value) Then
            Me.ClearTextboxes()
            Exit Sub
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

            End Try

        End If

    End Sub

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
            Me.ComboBox_Preset.DrawMode = DrawMode.OwnerDrawFixed
            Me.ComboBox_Preset.FlatStyle = FlatStyle.Flat
            Me.ComboBox_Preset.DropDownStyle = ComboBoxStyle.DropDownList
            RemoveHandler Me.ComboBox_Preset.DrawItem, AddressOf Me.ComboBox_Preset_DrawItem
            AddHandler Me.ComboBox_Preset.DrawItem, AddressOf Me.ComboBox_Preset_DrawItem

            Me.LinkLabel_GitHub.LinkColor = linkLabelForeColor

            Me.Button_TooggleDarkTheme.BackColor = Me.BackColor
            Me.Button_TooggleDarkTheme.FlatStyle = FlatStyle.Flat
            Me.Button_TooggleDarkTheme.FlatAppearance.BorderSize = 1
            Me.Button_TooggleDarkTheme.FlatAppearance.BorderColor = Me.BackColor
            Me.Button_TooggleDarkTheme.FlatAppearance.MouseOverBackColor = SystemColors.ControlDarkDark

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

            Me.comboBoxPresetBorderPainter = New ComboBoxBorderPainter(Me.ComboBox_Preset, SystemColors.ControlDarkDark)

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
            Me.ComboBox_Preset.DrawMode = DrawMode.Normal
            RemoveHandler Me.ComboBox_Preset.DrawItem, AddressOf Me.ComboBox_Preset_DrawItem

            Me.LinkLabel_GitHub.LinkColor = Color.FromArgb(255, 0, 0, 255)

            Me.Button_TooggleDarkTheme.BackColor = Me.BackColor
            Me.Button_TooggleDarkTheme.FlatStyle = FlatStyle.Flat
            Me.Button_TooggleDarkTheme.FlatAppearance.BorderSize = 1
            Me.Button_TooggleDarkTheme.FlatAppearance.BorderColor = Me.BackColor
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

            Me.comboBoxPresetBorderPainter = Nothing

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
