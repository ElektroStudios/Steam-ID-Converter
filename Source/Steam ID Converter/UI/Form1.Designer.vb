<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.TextBox_CustomID = New TextBox()
        Me.Label_CustomID = New Label()
        Me.TextBox_SteamId32Dec = New TextBox()
        Me.Label_SteamId32Dec = New Label()
        Me.Label_SteamId32Hex = New Label()
        Me.TextBox_SteamId32Hex = New TextBox()
        Me.Label_SteamId64Hex = New Label()
        Me.TextBox_SteamId64Hex = New TextBox()
        Me.Label_SteamId64Dec = New Label()
        Me.TextBox_SteamId64Dec = New TextBox()
        Me.GroupBox_SteamId64 = New GroupBox()
        Me.GroupBox_SteamId32 = New GroupBox()
        Me.LinkLabel_GitHub = New LinkLabel()
        Me.Label_Author = New Label()
        Me.ErrorProvider1 = New ErrorProvider(Me.components)
        Me.Button_TooggleDarkTheme = New Button()
        Me.ComboBox_Preset = New ComboBox()
        Me.GroupBox_SteamIdInput = New GroupBox()
        Me.Label_Preset = New Label()
        Me.GroupBox_SteamId64.SuspendLayout()
        Me.GroupBox_SteamId32.SuspendLayout()
        CType(Me.ErrorProvider1, ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_SteamIdInput.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' TextBox_CustomID
        ' 
        Me.TextBox_CustomID.BackColor = SystemColors.Window
        Me.TextBox_CustomID.Font = New Font("Segoe UI", 12F)
        Me.TextBox_CustomID.Location = New Point(110, 24)
        Me.TextBox_CustomID.Name = "TextBox_CustomID"
        Me.TextBox_CustomID.Size = New Size(178, 29)
        Me.TextBox_CustomID.TabIndex = 1
        ' 
        ' Label_CustomID
        ' 
        Me.Label_CustomID.AutoSize = True
        Me.Label_CustomID.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Me.Label_CustomID.Location = New Point(6, 27)
        Me.Label_CustomID.Name = "Label_CustomID"
        Me.Label_CustomID.Size = New Size(86, 21)
        Me.Label_CustomID.TabIndex = 0
        Me.Label_CustomID.Text = "Custom ID:"
        ' 
        ' TextBox_SteamId32Dec
        ' 
        Me.TextBox_SteamId32Dec.Enabled = False
        Me.TextBox_SteamId32Dec.Font = New Font("Segoe UI", 12F)
        Me.TextBox_SteamId32Dec.Location = New Point(60, 25)
        Me.TextBox_SteamId32Dec.Name = "TextBox_SteamId32Dec"
        Me.TextBox_SteamId32Dec.ReadOnly = True
        Me.TextBox_SteamId32Dec.Size = New Size(228, 29)
        Me.TextBox_SteamId32Dec.TabIndex = 1
        ' 
        ' Label_SteamId32Dec
        ' 
        Me.Label_SteamId32Dec.AutoSize = True
        Me.Label_SteamId32Dec.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Me.Label_SteamId32Dec.Location = New Point(6, 28)
        Me.Label_SteamId32Dec.Name = "Label_SteamId32Dec"
        Me.Label_SteamId32Dec.Size = New Size(39, 21)
        Me.Label_SteamId32Dec.TabIndex = 0
        Me.Label_SteamId32Dec.Text = "Dec:"
        ' 
        ' Label_SteamId32Hex
        ' 
        Me.Label_SteamId32Hex.AutoSize = True
        Me.Label_SteamId32Hex.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Me.Label_SteamId32Hex.Location = New Point(6, 62)
        Me.Label_SteamId32Hex.Name = "Label_SteamId32Hex"
        Me.Label_SteamId32Hex.Size = New Size(39, 21)
        Me.Label_SteamId32Hex.TabIndex = 2
        Me.Label_SteamId32Hex.Text = "Hex:"
        ' 
        ' TextBox_SteamId32Hex
        ' 
        Me.TextBox_SteamId32Hex.Enabled = False
        Me.TextBox_SteamId32Hex.Font = New Font("Segoe UI", 12F)
        Me.TextBox_SteamId32Hex.Location = New Point(60, 59)
        Me.TextBox_SteamId32Hex.Name = "TextBox_SteamId32Hex"
        Me.TextBox_SteamId32Hex.ReadOnly = True
        Me.TextBox_SteamId32Hex.Size = New Size(228, 29)
        Me.TextBox_SteamId32Hex.TabIndex = 3
        ' 
        ' Label_SteamId64Hex
        ' 
        Me.Label_SteamId64Hex.AutoSize = True
        Me.Label_SteamId64Hex.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Me.Label_SteamId64Hex.Location = New Point(6, 62)
        Me.Label_SteamId64Hex.Name = "Label_SteamId64Hex"
        Me.Label_SteamId64Hex.Size = New Size(39, 21)
        Me.Label_SteamId64Hex.TabIndex = 2
        Me.Label_SteamId64Hex.Text = "Hex:"
        ' 
        ' TextBox_SteamId64Hex
        ' 
        Me.TextBox_SteamId64Hex.Enabled = False
        Me.TextBox_SteamId64Hex.Font = New Font("Segoe UI", 12F)
        Me.TextBox_SteamId64Hex.Location = New Point(60, 59)
        Me.TextBox_SteamId64Hex.Name = "TextBox_SteamId64Hex"
        Me.TextBox_SteamId64Hex.ReadOnly = True
        Me.TextBox_SteamId64Hex.Size = New Size(228, 29)
        Me.TextBox_SteamId64Hex.TabIndex = 3
        ' 
        ' Label_SteamId64Dec
        ' 
        Me.Label_SteamId64Dec.AutoSize = True
        Me.Label_SteamId64Dec.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Me.Label_SteamId64Dec.Location = New Point(6, 28)
        Me.Label_SteamId64Dec.Name = "Label_SteamId64Dec"
        Me.Label_SteamId64Dec.Size = New Size(39, 21)
        Me.Label_SteamId64Dec.TabIndex = 0
        Me.Label_SteamId64Dec.Text = "Dec:"
        ' 
        ' TextBox_SteamId64Dec
        ' 
        Me.TextBox_SteamId64Dec.Enabled = False
        Me.TextBox_SteamId64Dec.Font = New Font("Segoe UI", 12F)
        Me.TextBox_SteamId64Dec.Location = New Point(60, 25)
        Me.TextBox_SteamId64Dec.Name = "TextBox_SteamId64Dec"
        Me.TextBox_SteamId64Dec.ReadOnly = True
        Me.TextBox_SteamId64Dec.Size = New Size(228, 29)
        Me.TextBox_SteamId64Dec.TabIndex = 1
        ' 
        ' GroupBox_SteamId64
        ' 
        Me.GroupBox_SteamId64.Controls.Add(Me.Label_SteamId64Dec)
        Me.GroupBox_SteamId64.Controls.Add(Me.Label_SteamId64Hex)
        Me.GroupBox_SteamId64.Controls.Add(Me.TextBox_SteamId64Dec)
        Me.GroupBox_SteamId64.Controls.Add(Me.TextBox_SteamId64Hex)
        Me.GroupBox_SteamId64.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Me.GroupBox_SteamId64.Location = New Point(12, 220)
        Me.GroupBox_SteamId64.Name = "GroupBox_SteamId64"
        Me.GroupBox_SteamId64.Size = New Size(295, 96)
        Me.GroupBox_SteamId64.TabIndex = 2
        Me.GroupBox_SteamId64.TabStop = False
        Me.GroupBox_SteamId64.Text = "Steam ID (64-bit)"
        ' 
        ' GroupBox_SteamId32
        ' 
        Me.GroupBox_SteamId32.Controls.Add(Me.Label_SteamId32Dec)
        Me.GroupBox_SteamId32.Controls.Add(Me.TextBox_SteamId32Dec)
        Me.GroupBox_SteamId32.Controls.Add(Me.TextBox_SteamId32Hex)
        Me.GroupBox_SteamId32.Controls.Add(Me.Label_SteamId32Hex)
        Me.GroupBox_SteamId32.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Me.GroupBox_SteamId32.Location = New Point(12, 114)
        Me.GroupBox_SteamId32.Name = "GroupBox_SteamId32"
        Me.GroupBox_SteamId32.Size = New Size(295, 96)
        Me.GroupBox_SteamId32.TabIndex = 1
        Me.GroupBox_SteamId32.TabStop = False
        Me.GroupBox_SteamId32.Text = "Steam ID (32-bit)"
        ' 
        ' LinkLabel_GitHub
        ' 
        Me.LinkLabel_GitHub.AutoSize = True
        Me.LinkLabel_GitHub.Location = New Point(12, 349)
        Me.LinkLabel_GitHub.Name = "LinkLabel_GitHub"
        Me.LinkLabel_GitHub.Size = New Size(144, 21)
        Me.LinkLabel_GitHub.TabIndex = 4
        Me.LinkLabel_GitHub.TabStop = True
        Me.LinkLabel_GitHub.Text = "🌐 View on GitHub"
        ' 
        ' Label_Author
        ' 
        Me.Label_Author.AutoSize = True
        Me.Label_Author.Location = New Point(12, 326)
        Me.Label_Author.Name = "Label_Author"
        Me.Label_Author.Size = New Size(188, 21)
        Me.Label_Author.TabIndex = 3
        Me.Label_Author.Text = "Created by ElektroStudios"
        ' 
        ' ErrorProvider1
        ' 
        Me.ErrorProvider1.ContainerControl = Me
        ' 
        ' Button_TooggleDarkTheme
        ' 
        Me.Button_TooggleDarkTheme.BackColor = Color.Transparent
        Me.Button_TooggleDarkTheme.BackgroundImage = My.Resources.Resources.theme_switch
        Me.Button_TooggleDarkTheme.BackgroundImageLayout = ImageLayout.Zoom
        Me.Button_TooggleDarkTheme.FlatStyle = FlatStyle.Flat
        Me.Button_TooggleDarkTheme.Location = New Point(275, 338)
        Me.Button_TooggleDarkTheme.Name = "Button_TooggleDarkTheme"
        Me.Button_TooggleDarkTheme.Size = New Size(32, 32)
        Me.Button_TooggleDarkTheme.TabIndex = 5
        Me.Button_TooggleDarkTheme.UseVisualStyleBackColor = False
        ' 
        ' ComboBox_Preset
        ' 
        Me.ComboBox_Preset.DropDownStyle = ComboBoxStyle.DropDownList
        Me.ComboBox_Preset.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Me.ComboBox_Preset.FormattingEnabled = True
        Me.ComboBox_Preset.Items.AddRange(New Object() {"NONE"})
        Me.ComboBox_Preset.Location = New Point(110, 59)
        Me.ComboBox_Preset.Name = "ComboBox_Preset"
        Me.ComboBox_Preset.Size = New Size(178, 29)
        Me.ComboBox_Preset.TabIndex = 3
        ' 
        ' GroupBox_SteamIdInput
        ' 
        Me.GroupBox_SteamIdInput.Controls.Add(Me.Label_Preset)
        Me.GroupBox_SteamIdInput.Controls.Add(Me.Label_CustomID)
        Me.GroupBox_SteamIdInput.Controls.Add(Me.ComboBox_Preset)
        Me.GroupBox_SteamIdInput.Controls.Add(Me.TextBox_CustomID)
        Me.GroupBox_SteamIdInput.Font = New Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Me.GroupBox_SteamIdInput.Location = New Point(12, 12)
        Me.GroupBox_SteamIdInput.Name = "GroupBox_SteamIdInput"
        Me.GroupBox_SteamIdInput.Size = New Size(295, 96)
        Me.GroupBox_SteamIdInput.TabIndex = 0
        Me.GroupBox_SteamIdInput.TabStop = False
        Me.GroupBox_SteamIdInput.Text = "Steam ID Input"
        ' 
        ' Label_Preset
        ' 
        Me.Label_Preset.AutoSize = True
        Me.Label_Preset.Font = New Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Me.Label_Preset.Location = New Point(6, 62)
        Me.Label_Preset.Name = "Label_Preset"
        Me.Label_Preset.Size = New Size(75, 21)
        Me.Label_Preset.TabIndex = 2
        Me.Label_Preset.Text = "Preset ID:"
        ' 
        ' Form1
        ' 
        Me.AutoScaleDimensions = New SizeF(9F, 21F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.BackColor = SystemColors.Control
        Me.ClientSize = New Size(318, 376)
        Me.Controls.Add(Me.GroupBox_SteamIdInput)
        Me.Controls.Add(Me.Button_TooggleDarkTheme)
        Me.Controls.Add(Me.Label_Author)
        Me.Controls.Add(Me.LinkLabel_GitHub)
        Me.Controls.Add(Me.GroupBox_SteamId32)
        Me.Controls.Add(Me.GroupBox_SteamId64)
        Me.Font = New Font("Segoe UI", 12F)
        Me.FormBorderStyle = FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), Icon)
        Me.Margin = New Padding(4)
        Me.MaximizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = FormStartPosition.CenterScreen
        Me.Text = "Steam ID Converter"
        Me.GroupBox_SteamId64.ResumeLayout(False)
        Me.GroupBox_SteamId64.PerformLayout()
        Me.GroupBox_SteamId32.ResumeLayout(False)
        Me.GroupBox_SteamId32.PerformLayout()
        CType(Me.ErrorProvider1, ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_SteamIdInput.ResumeLayout(False)
        Me.GroupBox_SteamIdInput.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents TextBox_CustomID As TextBox
    Friend WithEvents Label_CustomID As Label
    Friend WithEvents TextBox_SteamId32Dec As TextBox
    Friend WithEvents Label_SteamId32Dec As Label
    Friend WithEvents Label_SteamId32Hex As Label
    Friend WithEvents TextBox_SteamId32Hex As TextBox
    Friend WithEvents Label_SteamId64Hex As Label
    Friend WithEvents TextBox_SteamId64Hex As TextBox
    Friend WithEvents Label_SteamId64Dec As Label
    Friend WithEvents TextBox_SteamId64Dec As TextBox
    Friend WithEvents GroupBox_SteamId64 As GroupBox
    Friend WithEvents GroupBox_SteamId32 As GroupBox
    Friend WithEvents LinkLabel_GitHub As LinkLabel
    Friend WithEvents Label_Author As Label
    Friend WithEvents ErrorProvider1 As ErrorProvider
    Friend WithEvents Button_TooggleDarkTheme As Button
    Friend WithEvents GroupBox_SteamIdInput As GroupBox
    Friend WithEvents ComboBox_Preset As ComboBox
    Friend WithEvents Label_Preset As Label

End Class
