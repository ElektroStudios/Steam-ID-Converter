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

Imports SteamIDConverter.Win32

#End Region

#Region " ComboBoxBorderPainter "

''' <summary>
''' Attaches to an existing <see cref="ComboBox"/> at runtime and repaints
''' its border with a custom color by intercepting WM_PAINT.
''' 
''' This class does not require a designer-registered custom control.
''' It subclasses the combo's window at runtime via <see cref="NativeWindow"/>.
''' </summary>
Namespace SteamIDConverter

    Public NotInheritable Class ComboBoxBorderPainter : Inherits NativeWindow

#Region " Fields "

        ''' <summary>
        ''' The target combo being painted.
        ''' </summary>
        Private ReadOnly targetCombo As ComboBox

        ''' <summary>
        ''' Backing field for <see cref="BorderColor"/>.
        ''' </summary>
        Private borderColorValue As Color = Color.Gray

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or sets the border color applied to the target combo.
        ''' </summary>
        Public Property BorderColor As Color
            Get
                Return Me.borderColorValue
            End Get
            Set(value As Color)
                Me.borderColorValue = value
                Me.targetCombo.Invalidate()
            End Set
        End Property

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ComboBoxBorderPainter"/> class
        ''' and attaches it to the given <see cref="ComboBox"/>.
        ''' </summary>
        ''' 
        ''' <param name="combo">The combo box whose border will be repainted.</param>
        ''' <param name="initialColor">The initial border color.</param>
        Public Sub New(combo As ComboBox, initialColor As Color)
            MyBase.New()
            Me.targetCombo = combo
            Me.borderColorValue = initialColor

            If Me.targetCombo.IsHandleCreated Then
                Me.AssignHandle(Me.targetCombo.Handle)
            Else
                AddHandler Me.targetCombo.HandleCreated, AddressOf Me.OnTargetHandleCreated
            End If

            AddHandler Me.targetCombo.HandleDestroyed, AddressOf Me.OnTargetHandleDestroyed
        End Sub

#End Region

#Region " Handle Management "

        ''' <summary>
        ''' Handles the <see cref="Control.HandleCreated"/> event of the target combo.
        ''' </summary>
        ''' 
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        Private Sub OnTargetHandleCreated(sender As Object, e As EventArgs)

            Me.AssignHandle(Me.targetCombo.Handle)
        End Sub

        ''' <summary>
        ''' Handles the <see cref="Control.HandleDestroyed"/> event of the target combo.
        ''' </summary>
        ''' 
        ''' <param name="sender">The source of the event.</param>
        ''' <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        Private Sub OnTargetHandleDestroyed(sender As Object, e As EventArgs)

            Me.ReleaseHandle()
        End Sub

#End Region

#Region " WndProc Override "

        ''' <summary>
        ''' Intercepts window messages sent to the target combo.
        ''' After the system paints the control, draws a custom border on top.
        ''' </summary>
        ''' 
        ''' <param name="m">The Windows <see cref="Message"/>.</param>
        Protected Overrides Sub WndProc(ByRef m As Message)

            MyBase.WndProc(m)

            If m.Msg = Constants.WM_PAINT Then
                Me.DrawCustomBorder()
            End If
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Draws the custom border over the combo box using the window DC.
        ''' </summary>
        Private Sub DrawCustomBorder()

            Dim hDC As IntPtr = NativeMethods.GetWindowDC(Me.targetCombo.Handle)
            If hDC = IntPtr.Zero Then
                Return
            End If

            Try
                Using g As Graphics = Graphics.FromHdc(hDC)

                    Dim borderRect As New Rectangle(0, 0, Me.targetCombo.Width - 1, Me.targetCombo.Height - 1)

                    Using borderPen As New Pen(Me.borderColorValue, 1.0F)
                        g.DrawRectangle(borderPen, borderRect)
                    End Using
                End Using

            Finally
                Dim result As Integer = NativeMethods.ReleaseDC(Me.targetCombo.Handle, hDC)

            End Try
        End Sub

#End Region

    End Class

End Namespace

#End Region