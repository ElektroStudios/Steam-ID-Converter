
#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports System.ComponentModel
Imports System.Globalization

Imports DevCase.ThirdParty.Steam

#End Region

#Region " Form1 "

Public Class Form1

#Region " Fields "

    ''' <summary>
    ''' The URL that points to the GitHub repository of this application.
    ''' </summary>
    Private Const GitHubUrl As String = "https://github.com/ElektroStudios/Steam-ID-Converter"

    ''' <summary>
    ''' A list of predefined Steam ID values.
    ''' </summary>
    Private PredefinedValues As New BindingList(Of KeyValuePair(Of String, String)) From {
        New KeyValuePair(Of String, String)("NONE", ""),
        New KeyValuePair(Of String, String)("CODEX (CDX)", "1638"),
        New KeyValuePair(Of String, String)("DARKSiDERS (DS)", "3256440348"),
        New KeyValuePair(Of String, String)("RELOADED (RLD)", "4919"),
        New KeyValuePair(Of String, String)("TENOKE", "4660")
    }

#End Region

#Region " Event Handlers "

    ''' <summary>
    ''' Handles the Load event of the Form1 control.
    ''' </summary>
    ''' 
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Text = $"{My.Application.Info.Title} v{My.Application.Info.Version.Major}.{My.Application.Info.Version.Minor}"

        Me.ComboBox_Predefined.DataSource = Me.PredefinedValues
        Me.ComboBox_Predefined.DisplayMember = "Key"
        Me.ComboBox_Predefined.ValueMember = "Value"
        Me.ComboBox_Predefined.SelectedIndex = 0

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
            .SetIconAlignment(Me.TextBox_Manual, ErrorIconAlignment.MiddleRight)
            .SetIconPadding(Me.TextBox_Manual, 3)

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
    Handles TextBox_Manual.TextChanged

        Dim tb As TextBox = DirectCast(sender, TextBox)
        Me.ErrorProvider1.SetError(tb, Nothing)
        Me.ParseInputValue(tb.Text)
    End Sub

    ''' <summary>
    ''' Handles the TextChanged event of the TextBox_SteamId32Num, TextBox_SteamId32Hex, 
    ''' TextBox_SteamId64Num and TextBox_SteamId64Hex controls.
    ''' </summary>
    ''' 
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Private Sub TextBox_SteamId_TextChanged(sender As Object, e As EventArgs) _
    Handles TextBox_SteamId32Num.TextChanged, TextBox_SteamId32Hex.TextChanged,
            TextBox_SteamId64Num.TextChanged, TextBox_SteamId64Hex.TextChanged

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
        Me.ErrorProvider1.SetError(lnkLbl, Nothing)

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
    ''' Handles the SelectedIndexChanged event of the ComboBox_Predefined control.
    ''' </summary>
    ''' 
    ''' <param name="sender">The source of the event.</param>
    ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
    Private Sub ComboBox_Predefined_SelectedIndexChanged(sender As Object, e As EventArgs) _
    Handles ComboBox_Predefined.SelectedIndexChanged

        Dim cb As ComboBox = DirectCast(sender, ComboBox)
        Dim selectedPair As KeyValuePair(Of String, String) = Me.PredefinedValues.Item(cb.SelectedIndex)

        Me.TextBox_Manual.Text = selectedPair.Value
        Me.TextBox_Manual.Enabled = String.IsNullOrEmpty(selectedPair.Value)

    End Sub

#End Region

#Region " Private Methods "

    ''' <summary>
    ''' Parses the input value.
    ''' </summary>
    ''' 
    ''' <param name="value">The value.</param>
    Private Sub ParseInputValue(value As String)

        value = value.Trim()
        If String.IsNullOrEmpty(value) Then
            Me.ClearTextboxes()
            Exit Sub
        End If

        Dim steamId32Num As UInteger
        Dim steamId64Num As ULong

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
            Me.ErrorProvider1.SetError(Me.TextBox_Manual, "Cannot parse input value.")
            Me.ClearTextboxes()

        Else
            Dim isSteam32IdLength As Boolean = numericValue < UInteger.MaxValue
            Try
                If isSteam32IdLength Then
                    steamId32Num = CUInt(numericValue)
                    steamId64Num = SteamUtil.ConvertSteamID32ToSteamID64(steamId32Num)
                Else
                    steamId32Num = SteamUtil.ConvertSteamID64ToSteamID32(numericValue)
                    steamId64Num = numericValue
                End If
                Me.TextBox_SteamId32Num.Text = CStr(steamId32Num)
                Me.TextBox_SteamId64Num.Text = CStr(steamId64Num)
                Me.TextBox_SteamId32Hex.Text = $"0x{steamId32Num:X2}"
                Me.TextBox_SteamId64Hex.Text = $"0x{steamId64Num:X2}"

            Catch ex As Exception
                Me.ErrorProvider1.SetError(Me.TextBox_Manual, "Cannot parse input value.")
                Me.ClearTextboxes()

            End Try

        End If

    End Sub

    ''' <summary>
    ''' Clears the Steam ID textboxes.
    ''' </summary>
    Private Sub ClearTextboxes()
        Me.TextBox_SteamId32Num.Clear()
        Me.TextBox_SteamId32Hex.Clear()

        Me.TextBox_SteamId64Num.Clear()
        Me.TextBox_SteamId64Hex.Clear()
    End Sub

    ''' <summary>
    ''' Sets the visual theme for the application.
    ''' </summary>
    Private Sub SetVisualTheme()

        If My.Settings.UseDarkTheme Then
            Dim formBackColor As Color = Color.FromArgb(255, 30, 30, 30)
            Dim textboxBackColor As Color = Color.FromArgb(255, 37, 37, 38)
            Dim formForeColor As Color = Color.WhiteSmoke
            Dim groupBoxForeColor As Color = Color.WhiteSmoke
            Dim textboxForeColor As Color = Color.WhiteSmoke
            Dim linkLabelForeColor As Color = Color.DodgerBlue

            Me.BackColor = formBackColor
            Me.ForeColor = formForeColor

            Me.GroupBox_InputValue.ForeColor = groupBoxForeColor
            Me.GroupBox_SteamId64.ForeColor = groupBoxForeColor
            Me.GroupBox_SteamId32.ForeColor = groupBoxForeColor

            Me.ComboBox_Predefined.BackColor = Color.FromArgb(255, 37, 37, 38)
            Me.ComboBox_Predefined.ForeColor = textboxForeColor
            Me.ComboBox_Predefined.FlatStyle = FlatStyle.Flat

            Me.LinkLabel_GitHub.LinkColor = linkLabelForeColor

            Me.Button_TooggleDarkTheme.FlatStyle = FlatStyle.Flat
            Me.Button_TooggleDarkTheme.FlatAppearance.BorderSize = 0
            Me.Button_TooggleDarkTheme.FlatAppearance.MouseOverBackColor = SystemColors.ControlDarkDark

            Me.TextBox_Manual.BackColor = textboxBackColor
            Me.TextBox_Manual.ForeColor = textboxForeColor
            Me.TextBox_Manual.BorderStyle = BorderStyle.FixedSingle

            Me.TextBox_SteamId32Hex.BackColor = textboxBackColor
            Me.TextBox_SteamId32Hex.ForeColor = textboxForeColor
            Me.TextBox_SteamId32Hex.BorderStyle = BorderStyle.FixedSingle

            Me.TextBox_SteamId32Num.BackColor = textboxBackColor
            Me.TextBox_SteamId32Num.ForeColor = textboxForeColor
            Me.TextBox_SteamId32Num.BorderStyle = BorderStyle.FixedSingle

            Me.TextBox_SteamId64Hex.BackColor = textboxBackColor
            Me.TextBox_SteamId64Hex.ForeColor = textboxForeColor
            Me.TextBox_SteamId64Hex.BorderStyle = BorderStyle.FixedSingle

            Me.TextBox_SteamId64Num.BackColor = textboxBackColor
            Me.TextBox_SteamId64Num.ForeColor = textboxForeColor
            Me.TextBox_SteamId64Num.BorderStyle = BorderStyle.FixedSingle

        Else
            Me.BackColor = Form.DefaultBackColor
            Me.ForeColor = Form.DefaultForeColor

            Me.GroupBox_InputValue.ForeColor = GroupBox.DefaultForeColor
            Me.GroupBox_SteamId64.ForeColor = GroupBox.DefaultForeColor
            Me.GroupBox_SteamId32.ForeColor = GroupBox.DefaultForeColor

            Me.ComboBox_Predefined.BackColor = ComboBox.DefaultBackColor
            Me.ComboBox_Predefined.ForeColor = ComboBox.DefaultForeColor
            Me.ComboBox_Predefined.FlatStyle = FlatStyle.Standard

            Me.LinkLabel_GitHub.LinkColor = Color.FromArgb(255, 0, 0, 255)

            Me.Button_TooggleDarkTheme.FlatStyle = FlatStyle.Standard

            Me.TextBox_Manual.BackColor = TextBox.DefaultBackColor
            Me.TextBox_Manual.ForeColor = TextBox.DefaultForeColor
            Me.TextBox_Manual.BorderStyle = BorderStyle.Fixed3D

            Me.TextBox_SteamId32Hex.BackColor = TextBox.DefaultBackColor
            Me.TextBox_SteamId32Hex.ForeColor = TextBox.DefaultForeColor
            Me.TextBox_SteamId32Hex.BorderStyle = BorderStyle.Fixed3D

            Me.TextBox_SteamId32Num.BackColor = TextBox.DefaultBackColor
            Me.TextBox_SteamId32Num.ForeColor = TextBox.DefaultForeColor
            Me.TextBox_SteamId32Num.BorderStyle = BorderStyle.Fixed3D

            Me.TextBox_SteamId64Hex.BackColor = TextBox.DefaultBackColor
            Me.TextBox_SteamId64Hex.ForeColor = TextBox.DefaultForeColor
            Me.TextBox_SteamId64Hex.BorderStyle = BorderStyle.Fixed3D

            Me.TextBox_SteamId64Num.BackColor = TextBox.DefaultBackColor
            Me.TextBox_SteamId64Num.ForeColor = TextBox.DefaultForeColor
            Me.TextBox_SteamId64Num.BorderStyle = BorderStyle.Fixed3D

        End If

    End Sub

#End Region

End Class

#End Region
