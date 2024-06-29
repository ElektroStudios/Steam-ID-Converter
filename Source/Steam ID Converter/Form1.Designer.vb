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
        Me.TextBox_InputValue = New TextBox()
        Me.Label_InputValue = New Label()
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
        Me.GroupBox_SteamId64.SuspendLayout()
        Me.GroupBox_SteamId32.SuspendLayout()
        CType(Me.ErrorProvider1, ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        ' 
        ' TextBox_InputValue
        ' 
        Me.TextBox_InputValue.BackColor = SystemColors.Window
        Me.TextBox_InputValue.Location = New Point(109, 12)
        Me.TextBox_InputValue.Name = "TextBox_InputValue"
        Me.TextBox_InputValue.Size = New Size(175, 29)
        Me.TextBox_InputValue.TabIndex = 0
        ' 
        ' Label_InputValue
        ' 
        Me.Label_InputValue.AutoSize = True
        Me.Label_InputValue.Location = New Point(12, 15)
        Me.Label_InputValue.Name = "Label_InputValue"
        Me.Label_InputValue.Size = New Size(91, 21)
        Me.Label_InputValue.TabIndex = 1
        Me.Label_InputValue.Text = "Input Value:"
        ' 
        ' TextBox_SteamId32Num
        ' 
        Me.TextBox_SteamId32Num.Enabled = False
        Me.TextBox_SteamId32Num.Location = New Point(60, 22)
        Me.TextBox_SteamId32Num.Name = "TextBox_SteamId32Num"
        Me.TextBox_SteamId32Num.ReadOnly = True
        Me.TextBox_SteamId32Num.Size = New Size(212, 29)
        Me.TextBox_SteamId32Num.TabIndex = 2
        ' 
        ' Label_SteamId32Num
        ' 
        Me.Label_SteamId32Num.AutoSize = True
        Me.Label_SteamId32Num.Location = New Point(6, 25)
        Me.Label_SteamId32Num.Name = "Label_SteamId32Num"
        Me.Label_SteamId32Num.Size = New Size(48, 21)
        Me.Label_SteamId32Num.TabIndex = 9
        Me.Label_SteamId32Num.Text = "Num:"
        ' 
        ' Label_SteamId32Hex
        ' 
        Me.Label_SteamId32Hex.AutoSize = True
        Me.Label_SteamId32Hex.Location = New Point(6, 59)
        Me.Label_SteamId32Hex.Name = "Label_SteamId32Hex"
        Me.Label_SteamId32Hex.Size = New Size(39, 21)
        Me.Label_SteamId32Hex.TabIndex = 11
        Me.Label_SteamId32Hex.Text = "Hex:"
        ' 
        ' TextBox_SteamId32Hex
        ' 
        Me.TextBox_SteamId32Hex.Enabled = False
        Me.TextBox_SteamId32Hex.Location = New Point(60, 56)
        Me.TextBox_SteamId32Hex.Name = "TextBox_SteamId32Hex"
        Me.TextBox_SteamId32Hex.ReadOnly = True
        Me.TextBox_SteamId32Hex.Size = New Size(212, 29)
        Me.TextBox_SteamId32Hex.TabIndex = 10
        ' 
        ' Label_SteamId64Hex
        ' 
        Me.Label_SteamId64Hex.AutoSize = True
        Me.Label_SteamId64Hex.Location = New Point(6, 59)
        Me.Label_SteamId64Hex.Name = "Label_SteamId64Hex"
        Me.Label_SteamId64Hex.Size = New Size(39, 21)
        Me.Label_SteamId64Hex.TabIndex = 16
        Me.Label_SteamId64Hex.Text = "Hex:"
        ' 
        ' TextBox_SteamId64Hex
        ' 
        Me.TextBox_SteamId64Hex.Enabled = False
        Me.TextBox_SteamId64Hex.Location = New Point(60, 56)
        Me.TextBox_SteamId64Hex.Name = "TextBox_SteamId64Hex"
        Me.TextBox_SteamId64Hex.ReadOnly = True
        Me.TextBox_SteamId64Hex.Size = New Size(212, 29)
        Me.TextBox_SteamId64Hex.TabIndex = 15
        ' 
        ' Label_SteamId64Num
        ' 
        Me.Label_SteamId64Num.AutoSize = True
        Me.Label_SteamId64Num.Location = New Point(6, 25)
        Me.Label_SteamId64Num.Name = "Label_SteamId64Num"
        Me.Label_SteamId64Num.Size = New Size(48, 21)
        Me.Label_SteamId64Num.TabIndex = 14
        Me.Label_SteamId64Num.Text = "Num:"
        ' 
        ' TextBox_SteamId64Num
        ' 
        Me.TextBox_SteamId64Num.Enabled = False
        Me.TextBox_SteamId64Num.Location = New Point(60, 22)
        Me.TextBox_SteamId64Num.Name = "TextBox_SteamId64Num"
        Me.TextBox_SteamId64Num.ReadOnly = True
        Me.TextBox_SteamId64Num.Size = New Size(212, 29)
        Me.TextBox_SteamId64Num.TabIndex = 12
        ' 
        ' GroupBox_SteamId64
        ' 
        Me.GroupBox_SteamId64.Controls.Add(Me.Label_SteamId64Num)
        Me.GroupBox_SteamId64.Controls.Add(Me.Label_SteamId64Hex)
        Me.GroupBox_SteamId64.Controls.Add(Me.TextBox_SteamId64Num)
        Me.GroupBox_SteamId64.Controls.Add(Me.TextBox_SteamId64Hex)
        Me.GroupBox_SteamId64.Location = New Point(12, 157)
        Me.GroupBox_SteamId64.Name = "GroupBox_SteamId64"
        Me.GroupBox_SteamId64.Size = New Size(280, 96)
        Me.GroupBox_SteamId64.TabIndex = 17
        Me.GroupBox_SteamId64.TabStop = False
        Me.GroupBox_SteamId64.Text = "Steam ID 64"
        ' 
        ' GroupBox_SteamId32
        ' 
        Me.GroupBox_SteamId32.Controls.Add(Me.Label_SteamId32Num)
        Me.GroupBox_SteamId32.Controls.Add(Me.TextBox_SteamId32Num)
        Me.GroupBox_SteamId32.Controls.Add(Me.TextBox_SteamId32Hex)
        Me.GroupBox_SteamId32.Controls.Add(Me.Label_SteamId32Hex)
        Me.GroupBox_SteamId32.Location = New Point(12, 51)
        Me.GroupBox_SteamId32.Name = "GroupBox_SteamId32"
        Me.GroupBox_SteamId32.Size = New Size(280, 96)
        Me.GroupBox_SteamId32.TabIndex = 18
        Me.GroupBox_SteamId32.TabStop = False
        Me.GroupBox_SteamId32.Text = "Steam ID 32"
        ' 
        ' LinkLabel_GitHub
        ' 
        Me.LinkLabel_GitHub.AutoSize = True
        Me.LinkLabel_GitHub.Location = New Point(12, 277)
        Me.LinkLabel_GitHub.Name = "LinkLabel_GitHub"
        Me.LinkLabel_GitHub.Size = New Size(173, 21)
        Me.LinkLabel_GitHub.TabIndex = 20
        Me.LinkLabel_GitHub.TabStop = True
        Me.LinkLabel_GitHub.Text = "Visit the official website"
        ' 
        ' Label_Author
        ' 
        Me.Label_Author.AutoSize = True
        Me.Label_Author.Location = New Point(12, 256)
        Me.Label_Author.Name = "Label_Author"
        Me.Label_Author.Size = New Size(173, 21)
        Me.Label_Author.TabIndex = 21
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
        Me.Button_TooggleDarkTheme.FlatStyle = FlatStyle.Flat
        Me.Button_TooggleDarkTheme.Location = New Point(260, 262)
        Me.Button_TooggleDarkTheme.Name = "Button_TooggleDarkTheme"
        Me.Button_TooggleDarkTheme.Size = New Size(32, 32)
        Me.Button_TooggleDarkTheme.TabIndex = 22
        Me.Button_TooggleDarkTheme.UseVisualStyleBackColor = False
        ' 
        ' Form1
        ' 
        Me.AutoScaleDimensions = New SizeF(9F, 21F)
        Me.AutoScaleMode = AutoScaleMode.Font
        Me.BackColor = SystemColors.Control
        Me.ClientSize = New Size(304, 306)
        Me.Controls.Add(Me.Button_TooggleDarkTheme)
        Me.Controls.Add(Me.Label_Author)
        Me.Controls.Add(Me.LinkLabel_GitHub)
        Me.Controls.Add(Me.GroupBox_SteamId32)
        Me.Controls.Add(Me.GroupBox_SteamId64)
        Me.Controls.Add(Me.Label_InputValue)
        Me.Controls.Add(Me.TextBox_InputValue)
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
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub

    Friend WithEvents TextBox_InputValue As TextBox
    Friend WithEvents Label_InputValue As Label
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

End Class
