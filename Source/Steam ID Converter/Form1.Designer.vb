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
        Me.TextBox_Manual = New TextBox()
        Me.Label_Maunal = New Label()
        Me.TextBox_SteamId32Num = New TextBox()
        Me.Label_SteamId32Num = New Label()
        Me.Label_SteamId32Hex = New Label()
        Me.TextBox_SteamId32Hex = New TextBox()
        Me.Label_SteamId64Hex = New Label()
        Me.TextBox_SteamId64Hex = New TextBox()
        Me.Label_SteamId64Num = New Label()
        Me.TextBox_SteamId64Num = New TextBox()
        Me.GroupBox_SteamId64 = New GroupBox()
        Me.GroupBox_SteamId32 = New GroupBox()
        Me.LinkLabel_GitHub = New LinkLabel()
        Me.Label_Author = New Label()
        Me.ErrorProvider1 = New ErrorProvider(Me.components)
        Me.Button_TooggleDarkTheme = New Button()
        Me.ComboBox_Predefined = New ComboBox()
        Me.GroupBox_InputValue = New GroupBox()
        Me.Label_Predefined = New Label()
        Me.GroupBox_SteamId64.SuspendLayout()
        Me.GroupBox_SteamId32.SuspendLayout()
        CType(Me.ErrorProvider1, ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_InputValue.SuspendLayout()
        Me.SuspendLayout()
        ' 
        ' TextBox_Manual
        ' 
        Me.TextBox_Manual.BackColor = SystemColors.Window
        Me.TextBox_Manual.Location = New Point(100, 22)
        Me.TextBox_Manual.Name = "TextBox_Manual"
        Me.TextBox_Manual.Size = New Size(172, 29)
        Me.TextBox_Manual.TabIndex = 1
        ' 
        ' Label_Maunal
        ' 
        Me.Label_Maunal.AutoSize = True
        Me.Label_Maunal.Location = New Point(6, 25)
        Me.Label_Maunal.Name = "Label_Maunal"
        Me.Label_Maunal.Size = New Size(65, 21)
        Me.Label_Maunal.TabIndex = 0
        Me.Label_Maunal.Text = "Manual:"
        ' 
        ' TextBox_SteamId32Num
        ' 
        Me.TextBox_SteamId32Num.Enabled = False
        Me.TextBox_SteamId32Num.Location = New Point(60, 22)
        Me.TextBox_SteamId32Num.Name = "TextBox_SteamId32Num"
        Me.TextBox_SteamId32Num.ReadOnly = True
        Me.TextBox_SteamId32Num.Size = New Size(228, 29)
        Me.TextBox_SteamId32Num.TabIndex = 1
        ' 
        ' Label_SteamId32Num
        ' 
        Me.Label_SteamId32Num.AutoSize = True
        Me.Label_SteamId32Num.Location = New Point(6, 25)
        Me.Label_SteamId32Num.Name = "Label_SteamId32Num"
        Me.Label_SteamId32Num.Size = New Size(48, 21)
        Me.Label_SteamId32Num.TabIndex = 0
        Me.Label_SteamId32Num.Text = "Num:"
        ' 
        ' Label_SteamId32Hex
        ' 
        Me.Label_SteamId32Hex.AutoSize = True
        Me.Label_SteamId32Hex.Location = New Point(6, 59)
        Me.Label_SteamId32Hex.Name = "Label_SteamId32Hex"
        Me.Label_SteamId32Hex.Size = New Size(39, 21)
        Me.Label_SteamId32Hex.TabIndex = 2
        Me.Label_SteamId32Hex.Text = "Hex:"
        ' 
        ' TextBox_SteamId32Hex
        ' 
        Me.TextBox_SteamId32Hex.Enabled = False
        Me.TextBox_SteamId32Hex.Location = New Point(60, 56)
        Me.TextBox_SteamId32Hex.Name = "TextBox_SteamId32Hex"
        Me.TextBox_SteamId32Hex.ReadOnly = True
        Me.TextBox_SteamId32Hex.Size = New Size(228, 29)
        Me.TextBox_SteamId32Hex.TabIndex = 3
        ' 
        ' Label_SteamId64Hex
        ' 
        Me.Label_SteamId64Hex.AutoSize = True
        Me.Label_SteamId64Hex.Location = New Point(6, 59)
        Me.Label_SteamId64Hex.Name = "Label_SteamId64Hex"
        Me.Label_SteamId64Hex.Size = New Size(39, 21)
        Me.Label_SteamId64Hex.TabIndex = 2
        Me.Label_SteamId64Hex.Text = "Hex:"
        ' 
        ' TextBox_SteamId64Hex
        ' 
        Me.TextBox_SteamId64Hex.Enabled = False
        Me.TextBox_SteamId64Hex.Location = New Point(60, 56)
        Me.TextBox_SteamId64Hex.Name = "TextBox_SteamId64Hex"
        Me.TextBox_SteamId64Hex.ReadOnly = True
        Me.TextBox_SteamId64Hex.Size = New Size(228, 29)
        Me.TextBox_SteamId64Hex.TabIndex = 3
        ' 
        ' Label_SteamId64Num
        ' 
        Me.Label_SteamId64Num.AutoSize = True
        Me.Label_SteamId64Num.Location = New Point(6, 25)
        Me.Label_SteamId64Num.Name = "Label_SteamId64Num"
        Me.Label_SteamId64Num.Size = New Size(48, 21)
        Me.Label_SteamId64Num.TabIndex = 0
        Me.Label_SteamId64Num.Text = "Num:"
        ' 
        ' TextBox_SteamId64Num
        ' 
        Me.TextBox_SteamId64Num.Enabled = False
        Me.TextBox_SteamId64Num.Location = New Point(60, 22)
        Me.TextBox_SteamId64Num.Name = "TextBox_SteamId64Num"
        Me.TextBox_SteamId64Num.ReadOnly = True
        Me.TextBox_SteamId64Num.Size = New Size(228, 29)
        Me.TextBox_SteamId64Num.TabIndex = 1
        ' 
        ' GroupBox_SteamId64
        ' 
        Me.GroupBox_SteamId64.Controls.Add(Me.Label_SteamId64Num)
        Me.GroupBox_SteamId64.Controls.Add(Me.Label_SteamId64Hex)
        Me.GroupBox_SteamId64.Controls.Add(Me.TextBox_SteamId64Num)
        Me.GroupBox_SteamId64.Controls.Add(Me.TextBox_SteamId64Hex)
        Me.GroupBox_SteamId64.Location = New Point(12, 220)
        Me.GroupBox_SteamId64.Name = "GroupBox_SteamId64"
        Me.GroupBox_SteamId64.Size = New Size(295, 96)
        Me.GroupBox_SteamId64.TabIndex = 2
        Me.GroupBox_SteamId64.TabStop = False
        Me.GroupBox_SteamId64.Text = "Steam ID 64"
        ' 
        ' GroupBox_SteamId32
        ' 
        Me.GroupBox_SteamId32.Controls.Add(Me.Label_SteamId32Num)
        Me.GroupBox_SteamId32.Controls.Add(Me.TextBox_SteamId32Num)
        Me.GroupBox_SteamId32.Controls.Add(Me.TextBox_SteamId32Hex)
        Me.GroupBox_SteamId32.Controls.Add(Me.Label_SteamId32Hex)
        Me.GroupBox_SteamId32.Location = New Point(12, 114)
        Me.GroupBox_SteamId32.Name = "GroupBox_SteamId32"
        Me.GroupBox_SteamId32.Size = New Size(295, 96)
        Me.GroupBox_SteamId32.TabIndex = 1
        Me.GroupBox_SteamId32.TabStop = False
        Me.GroupBox_SteamId32.Text = "Steam ID 32"
        ' 
        ' LinkLabel_GitHub
        ' 
        Me.LinkLabel_GitHub.AutoSize = True
        Me.LinkLabel_GitHub.Location = New Point(12, 347)
        Me.LinkLabel_GitHub.Name = "LinkLabel_GitHub"
        Me.LinkLabel_GitHub.Size = New Size(173, 21)
        Me.LinkLabel_GitHub.TabIndex = 4
        Me.LinkLabel_GitHub.TabStop = True
        Me.LinkLabel_GitHub.Text = "Visit the official website"
        ' 
        ' Label_Author
        ' 
        Me.Label_Author.AutoSize = True
        Me.Label_Author.Location = New Point(12, 326)
        Me.Label_Author.Name = "Label_Author"
        Me.Label_Author.Size = New Size(173, 21)
        Me.Label_Author.TabIndex = 3
        Me.Label_Author.Text = "Made by ElektroStudios"
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
        Me.Button_TooggleDarkTheme.Location = New Point(275, 332)
        Me.Button_TooggleDarkTheme.Name = "Button_TooggleDarkTheme"
        Me.Button_TooggleDarkTheme.Size = New Size(32, 32)
        Me.Button_TooggleDarkTheme.TabIndex = 5
        Me.Button_TooggleDarkTheme.UseVisualStyleBackColor = False
        ' 
        ' ComboBox_Predefined
        ' 
        Me.ComboBox_Predefined.DropDownStyle = ComboBoxStyle.DropDownList
        Me.ComboBox_Predefined.FormattingEnabled = True
        Me.ComboBox_Predefined.Items.AddRange(New Object() {"NONE", "12345", "12345678", "CODEX (CDX)", "RELOADED (RLD)"})
        Me.ComboBox_Predefined.Location = New Point(100, 57)
        Me.ComboBox_Predefined.Name = "ComboBox_Predefined"
        Me.ComboBox_Predefined.Size = New Size(172, 29)
        Me.ComboBox_Predefined.TabIndex = 3
        ' 
        ' GroupBox_InputValue
        ' 
        Me.GroupBox_InputValue.Controls.Add(Me.Label_Predefined)
        Me.GroupBox_InputValue.Controls.Add(Me.Label_Maunal)
        Me.GroupBox_InputValue.Controls.Add(Me.ComboBox_Predefined)
        Me.GroupBox_InputValue.Controls.Add(Me.TextBox_Manual)
        Me.GroupBox_InputValue.Location = New Point(12, 12)
        Me.GroupBox_InputValue.Name = "GroupBox_InputValue"
        Me.GroupBox_InputValue.Size = New Size(295, 96)
        Me.GroupBox_InputValue.TabIndex = 0
        Me.GroupBox_InputValue.TabStop = False
        Me.GroupBox_InputValue.Text = "Input Value"
        ' 
        ' Label_Predefined
        ' 
        Me.Label_Predefined.AutoSize = True
        Me.Label_Predefined.Location = New Point(6, 60)
        Me.Label_Predefined.Name = "Label_Predefined"
        Me.Label_Predefined.Size = New Size(88, 21)
        Me.Label_Predefined.TabIndex = 2
        Me.Label_Predefined.Text = "Predefined:"
        ' 
        ' Form1
        ' 
        Me.AutoScaleDimensions = New SizeF(9F, 21F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.BackColor = SystemColors.Control
        Me.ClientSize = New Size(318, 376)
        Me.Controls.Add(Me.GroupBox_InputValue)
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
        Me.GroupBox_InputValue.ResumeLayout(False)
        Me.GroupBox_InputValue.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents TextBox_Manual As TextBox
    Friend WithEvents Label_Maunal As Label
    Friend WithEvents TextBox_SteamId32Num As TextBox
    Friend WithEvents Label_SteamId32Num As Label
    Friend WithEvents Label_SteamId32Hex As Label
    Friend WithEvents TextBox_SteamId32Hex As TextBox
    Friend WithEvents Label_SteamId64Hex As Label
    Friend WithEvents TextBox_SteamId64Hex As TextBox
    Friend WithEvents Label_SteamId64Num As Label
    Friend WithEvents TextBox_SteamId64Num As TextBox
    Friend WithEvents GroupBox_SteamId64 As GroupBox
    Friend WithEvents GroupBox_SteamId32 As GroupBox
    Friend WithEvents LinkLabel_GitHub As LinkLabel
    Friend WithEvents Label_Author As Label
    Friend WithEvents ErrorProvider1 As ErrorProvider
    Friend WithEvents Button_TooggleDarkTheme As Button
    Friend WithEvents GroupBox_InputValue As GroupBox
    Friend WithEvents ComboBox_Predefined As ComboBox
    Friend WithEvents Label_Predefined As Label

End Class
