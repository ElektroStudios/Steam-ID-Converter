' ***********************************************************************
' Author   : ElektroStudios
' Modified : 11-April-2026
' ***********************************************************************

#Region " Usage Examples "

' Private _borderPainter As ControlBorderPainter
' 
' Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
'     
'     Dim ctrl As Control = Me.TextBox1
'     Dim clr As Color = Color.DodgerBlue
'     
'     _borderPainter = New ControlBorderPainter(ctrl, clr)
' End Sub
'
' Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
'     _borderPainter?.Dispose()
' End Sub

#End Region

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports SteamIDConverter.Win32

#End Region

#Region " Control Border Painter "

Namespace SteamIDConverter.Helpers

    ''' <summary>
    ''' Paints a custom-colored border on any <see cref="System.Windows.Forms.Control"/> at runtime
    ''' by subclassing its underlying <see cref="NativeWindow"/> and intercepting
    ''' <c>WM_PAINT</c> messages.
    ''' </summary>
    ''' 
    ''' <example> This is a code example.
    ''' <code language="VB.NET">
    ''' Private _borderPainter As ControlBorderPainter
    ''' 
    ''' Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
    '''     
    '''     Dim ctrl As Control = Me.TextBox1
    '''     Dim clr As Color = Color.DodgerBlue
    '''     
    '''     _borderPainter = New ControlBorderPainter(ctrl, clr)
    ''' End Sub
    '''
    ''' Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
    '''     _borderPainter?.Dispose()
    ''' End Sub
    ''' </code>
    ''' </example>
    Public NotInheritable Class ControlBorderPainter : Inherits NativeWindow : Implements IDisposable

#Region " Inner Classes "

        ''' <summary>
        ''' Subclasses the parent control to exclude the target child control's
        ''' rectangle from the parent's <c>WM_ERASEBKGND</c> and <c>WM_PAINT</c> message processing,
        ''' preventing the parent from erasing/overwriting the child's custom border causing a flickering effect.
        ''' </summary>
        Private NotInheritable Class ParentHook : Inherits NativeWindow

            ''' <summary>
            ''' List of child controls whose bounds must be excluded
            ''' from the parent's erase/paint operations.
            ''' </summary>
            Private ReadOnly children As List(Of Control)

            ''' <summary>
            ''' Initializes a new instance of the <see cref="ParentHook"/> class.
            ''' </summary>
            ''' 
            ''' <param name="parentHandle">
            ''' The window handle of the parent control to subclass.
            ''' </param>
            Friend Sub New(parentHandle As IntPtr)

                Me.children = New List(Of Control)()
                Me.AssignHandle(parentHandle)
            End Sub

            ''' <summary>
            ''' Registers a child control whose bounds will be excluded
            ''' from the parent's background erase.
            ''' </summary>
            ''' 
            ''' <param name="child">
            ''' The child control to protect.
            ''' </param>
            Friend Sub AddChild(child As Control)

                If Not Me.children.Contains(child) Then
                    Me.children.Add(child)
                End If
            End Sub

            ''' <summary>
            ''' Unregisters a child control so its bounds are no longer excluded.
            ''' </summary>
            ''' 
            ''' <param name="child">
            ''' The child control to stop protecting.
            ''' </param>
            Friend Sub RemoveChild(child As Control)

                Me.children.Remove(child)
            End Sub

            ''' <summary>
            ''' Gets the number of child controls currently registered.
            ''' </summary>
            Friend ReadOnly Property ChildCount As Integer
                Get
                    Return Me.children.Count
                End Get
            End Property

            ''' <summary>
            ''' Invokes the default window procedure associated with this window,
            ''' excluding registered child rectangles from <c>WM_ERASEBKGND</c>.
            ''' </summary>
            ''' 
            ''' <param name="m">
            ''' A <see cref="System.Windows.Forms.Message"/> that is 
            ''' associated with the current Windows message.
            ''' </param>
            Protected Overrides Sub WndProc(ByRef m As Message)

                If m.Msg = Constants.WM_ERASEBKGND AndAlso Me.children.Count > 0 Then
                    Dim hdc As IntPtr = m.WParam
                    For Each child As Control In Me.children
                        If child IsNot Nothing AndAlso
                           child.IsHandleCreated AndAlso
                           child.Visible AndAlso
                           child.Size <> Size.Empty Then

                            Dim r As Rectangle = child.Bounds
                            ' Inflate by 1 pixel to cover the border area
                            ' that sits just outside the control's client rect.
                            r.Inflate(1, 1)
                            Dim result As Integer = NativeMethods.ExcludeClipRect(hdc, r.Left, r.Top, r.Right, r.Bottom)
                        End If
                    Next
                End If

                MyBase.WndProc(m)
            End Sub

            ''' <summary>
            ''' Releases the handle and detaches from the parent window.
            ''' </summary>
            Public Sub Unhook()

                Me.children.Clear()
                Me.ReleaseHandle()
            End Sub

        End Class

#End Region

#Region " Fields "

        ''' <summary>
        ''' Indicates whether this instance has been disposed.
        ''' </summary>
        Private disposed As Boolean = False

        ''' <summary>
        ''' Shared dictionary that maps parent control handles to their
        ''' <see cref="ParentHook"/> instances, allowing multiple 
        ''' <see cref="ControlBorderPainter"/> instances on sibling controls
        ''' to share a single parent hook.
        ''' </summary>
        Private Shared ReadOnly parentHooks As New Dictionary(Of IntPtr, ParentHook)()

        ''' <summary>
        ''' Stores the parent handle that this instance registered with,
        ''' so it can be properly unregistered on dispose.
        ''' </summary>
        Private hookedParentHandle As IntPtr = IntPtr.Zero

        ''' <summary>
        ''' Reentrancy guard for <see cref="PaintComboBoxBuffered"/>.
        ''' Prevents <c>WM_PRINT</c> → <c>WM_PAINT</c> recursion.
        ''' </summary>
        Private isBufferedPainting As Boolean = False

#End Region

#Region " Properties "

        ''' <summary>
        ''' Gets or sets target control being painted.
        ''' </summary>
        Public ReadOnly Property Control As Control

        ''' <summary>
        ''' Gets or sets the border color applied to the target control 
        ''' specified in <see cref="ControlBorderPainter.Control"/> property.
        ''' </summary>
        Public Property BorderColor As Color
            Get
                Return Me.borderColorValue
            End Get

            Set(value As Color)
                Me.borderColorValue = value
                If Not Me.disposed Then
                    Dim t As Integer = CInt(System.Math.Ceiling(Me.borderThicknessValue))
                    Dim w As Integer = Me.Control.Width
                    Dim h As Integer = Me.Control.Height

                    Using borderRegion As New Region(New Rectangle(0, 0, w, t)) ' top
                        borderRegion.Union(New Rectangle(0, h - t, w, t))       ' bottom
                        borderRegion.Union(New Rectangle(0, 0, t, h))           ' left
                        borderRegion.Union(New Rectangle(w - t, 0, t, h))       ' right

                        Me.Control.Invalidate(borderRegion)
                    End Using
                End If
            End Set
        End Property

        ''' <summary>
        ''' Backing field for <see cref="ControlBorderPainter.BorderColor"/> property.
        ''' </summary>
        Private borderColorValue As Color ' = SystemColors.ControlDark

        ''' <summary>
        ''' Gets or sets the border thickness, in pixels.
        ''' </summary>
        Public Property BorderThickness As Single
            Get
                Return Me.borderThicknessValue
            End Get

            Set(value As Single)
                Me.borderThicknessValue = value
                If Not Me.disposed Then
                    Me.Control.Invalidate()
                End If
            End Set
        End Property

        ''' <summary>
        ''' Backing field for <see cref="ControlBorderPainter.BorderThickness"/> property.
        ''' </summary>
        Private borderThicknessValue As Single = 1.0F

#End Region

#Region " Constructors "

        ''' <summary>
        ''' Prevents a default instance of the <see cref="ControlBorderPainter"/> class from being created.
        ''' </summary>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="ControlBorderPainter"/> class
        ''' and attaches it to the given <see cref="ControlBorderPainter.Control"/>.
        ''' </summary>
        ''' 
        ''' <param name="control">
        ''' The control whose border will be repainted.
        ''' </param>
        ''' 
        ''' <param name="borderColor">
        ''' The initial border color.
        ''' </param>
        ''' 
        ''' <param name="borderThickness">The initial border thickness. Default is 1.</param>
        Public Sub New(control As Control, borderColor As Color,
                       Optional borderThickness As Single = 1.0F)

            MyBase.New()

            Me.Control = control
            Me.borderColorValue = borderColor
            Me.borderThicknessValue = borderThickness

            AddHandler Me.Control.HandleCreated, AddressOf Me.Control_HandleCreated
            AddHandler Me.Control.HandleDestroyed, AddressOf Me.Control_HandleDestroyed

            ' GroupBox: subscribe to the Paint event so the custom border
            ' is drawn as part of the control's own paint cycle, using the
            ' managed Graphics object from PaintEventArgs instead of a raw HDC.
            If TypeOf Me.Control Is GroupBox Then
                AddHandler Me.Control.Paint, AddressOf Me.GroupBox_Paint
            End If

            If Me.Control.IsHandleCreated Then
                Me.AssignHandle(Me.Control.Handle)

                If TypeOf Me.Control Is ComboBox Then
                    Dim cb As ComboBox = DirectCast(Me.Control, ComboBox)
                    If cb.DropDownStyle <> ComboBoxStyle.Simple Then
                        ' Disables Visual Styles (UxTheme) rendering on the control.
                        Dim result As Integer = NativeMethods.SetWindowTheme(Me.Control.Handle, String.Empty, String.Empty)
                    End If
                End If

                If TypeOf Me.Control IsNot GroupBox AndAlso
                   TypeOf Me.Control IsNot Panel AndAlso
                   TypeOf Me.Control IsNot TabControl Then

                    Me.HookParent()
                    Me.EnsureParentClipsChildren()
                End If

                ' Force an initial repaint so the custom border appears
                ' immediately. This is necessary when the painter is attached
                ' to a control that is already visible and fully painted — the
                ' control would otherwise keep showing its native border until
                ' some external event (resize, minimize, etc.) invalidates it.
                Me.Control.Invalidate(invalidateChildren:=True)
            End If
        End Sub

#End Region

#Region " Event Handlers "

        ''' <summary>
        ''' Handles the <see cref="System.Windows.Forms.Control.HandleCreated"/> event of the target control
        ''' specified in <see cref="ControlBorderPainter.Control"/> property.
        ''' </summary>
        ''' 
        ''' <param name="sender">
        ''' The source of the event.
        ''' </param>
        ''' 
        ''' <param name="e">
        ''' The <see cref="EventArgs"/> instance containing the event data.
        ''' </param>
        Private Sub Control_HandleCreated(sender As Object, e As EventArgs)

            Me.AssignHandle(Me.Control.Handle)

            If TypeOf Me.Control Is ComboBox Then
                Dim cb As ComboBox = DirectCast(Me.Control, ComboBox)
                If cb.DropDownStyle <> ComboBoxStyle.Simple Then
                    ' Disables Visual Styles (UxTheme) rendering on the control.
                    Dim result As Integer = NativeMethods.SetWindowTheme(Me.Control.Handle, String.Empty, String.Empty)
                End If
            End If

            Me.HookParent()
            Me.EnsureParentClipsChildren()
        End Sub

        ''' <summary>
        ''' Handles the <see cref="System.Windows.Forms.Control.HandleDestroyed"/> event of the target control
        ''' specified in <see cref="ControlBorderPainter.Control"/> property.
        ''' </summary>
        ''' 
        ''' <param name="sender">
        ''' The source of the event.
        ''' </param>
        ''' 
        ''' <param name="e">
        ''' The <see cref="EventArgs"/> instance containing the event data.
        ''' </param>
        Private Sub Control_HandleDestroyed(sender As Object, e As EventArgs)

            Me.ReleaseHandle()
        End Sub

        ''' <summary>
        ''' Handles the <see cref="System.Windows.Forms.Control.Paint"/> event 
        ''' of a <see cref="GroupBox"/> control to draw a custom border using the
        ''' managed <see cref="Graphics"/> object from <see cref="PaintEventArgs"/>.
        ''' <para></para>
        ''' The title text area is excluded from the clip region so the border
        ''' rectangle never overwrites the title, avoiding any flickering effect.
        ''' </summary>
        ''' 
        ''' <param name="sender">
        ''' The <see cref="GroupBox"/> being painted.
        ''' </param>
        ''' 
        ''' <param name="e">
        ''' The <see cref="PaintEventArgs"/> instance containing the event data.
        ''' </param>
        Private Sub GroupBox_Paint(sender As Object, e As PaintEventArgs)

            Dim gb As GroupBox = DirectCast(sender, GroupBox)
            Dim g As Graphics = e.Graphics

            Dim titleText As String = If(gb.Text, String.Empty)
            Dim titleFont As Font = gb.Font
            Dim titleSize As SizeF = g.MeasureString(titleText, titleFont, Integer.MaxValue, StringFormat.GenericTypographic)

            Dim titleHeight As Integer = CInt(System.Math.Ceiling(titleSize.Height))
            Dim titleWidth As Integer = TextRenderer.MeasureText(titleText, titleFont, New Size(Integer.MaxValue, Integer.MaxValue), TextFormatFlags.NoPadding).Width
            Dim titleHalfHeight As Integer = titleHeight \ 2

            ' Exclude the title text area so DrawRectangle method never touches it.
            ' This strategy avoids flickering effect in the title text.
            If Not String.IsNullOrEmpty(titleText) Then
                g.ExcludeClip(New Rectangle(8, 0, titleWidth + 4, titleHeight))
            End If

            Dim borderRect As New Rectangle(0, titleHalfHeight, gb.Width - 1, gb.Height - titleHalfHeight - 2)
            Using borderPen As New Pen(Me.borderColorValue, Me.borderThicknessValue)
                g.DrawRectangle(borderPen, borderRect)
            End Using

            g.ResetClip()
        End Sub

#End Region

#Region " WndProc Override "

        ''' <summary>
        ''' Invokes the default window procedure associated with this window.
        ''' </summary>
        ''' 
        ''' <param name="m">
        ''' A <see cref="System.Windows.Forms.Message"/> that is 
        ''' associated with the current Windows message.
        ''' </param>
        Protected Overrides Sub WndProc(ByRef m As Message)

            Dim msgId As Integer = m.Msg
            Dim isComboBox As Boolean = TypeOf Me.Control Is ComboBox
            Dim isListBox As Boolean = TypeOf Me.Control Is ListBox

            ' ── ComboBox: double-buffered WM_PAINT ────────────────────────
            ' Renders the entire ComboBox to an off-screen bitmap via
            ' WM_PRINT, overwrites the border on the bitmap, then BitBlt's
            ' the result to screen atomically. Zero flicker.
            If msgId = Constants.WM_PAINT AndAlso isComboBox AndAlso Not Me.isBufferedPainting Then
                Me.PaintComboBoxBuffered()
                Return
            End If

            ' ── WM_ERASEBKGND ─────────────────────────────────────────────
            ' ComboBox / ListBox: suppress the background erase entirely.
            ' When an ErrorProvider (or any sibling) invalidates the parent,
            ' Windows propagates the invalidation to child controls, which
            ' then receive WM_ERASEBKGND before WM_PAINT. The default handler
            ' fills the client area with the background color, blanking out
            ' the custom border for one frame (visible as a white flash).
            ' Returning 1 tells Windows "I already erased it" so no erase
            ' happens. The WM_PAINT handler repaints the full content anyway.
            '
            ' GroupBox: also suppress because the Paint event handler
            ' overwrites the entire surface, making the native erase redundant.
            If msgId = Constants.WM_ERASEBKGND AndAlso
               (isComboBox OrElse isListBox OrElse TypeOf Me.Control Is GroupBox) Then
                m.Result = New IntPtr(1)
                Return
            End If

            ' ── WM_NCPAINT ────────────────────────────────────────────────
            ' ComboBox: suppress native WM_NCPAINT entirely.
            ' The ComboBox NC area is only the border (the dropdown button
            ' is client area), so skipping the base handler is safe.
            ' This prevents the native border from being painted for one frame
            ' before our custom border overwrites it.
            If (msgId = Constants.WM_NCPAINT) AndAlso isComboBox Then
                m.Result = IntPtr.Zero
                Me.DrawCustomControlBorder()
                Return
            End If

            ' ── ListBox: let base handle WM_NCPAINT first ────────────────
            ' Scrollbars live in the NC area and need the base handler,
            ' then overwrite the border.
            If (msgId = Constants.WM_NCPAINT) AndAlso isListBox Then
                MyBase.WndProc(m)
                Me.DrawCustomControlBorder()
                Return
            End If

            MyBase.WndProc(m)

            ' ── WM_PAINT ─────────────────────────────────────────────────
            ' GroupBox border is handled by the GroupBox_Paint event handler.
            If msgId = Constants.WM_PAINT Then
                If TypeOf Me.Control Is GroupBox Then
                    ' Handled by the Paint event subscriber (GroupBox_Paint).
                    ' No action needed here.

                ElseIf TypeOf Me.Control Is TabControl Then
                    Me.DrawCustomTabControlBorder()

                Else
                    Me.DrawCustomControlBorder()
                End If

                Return
            End If

            ' ── Focus transitions ────────────────────────────────────────
            ' The native control redraws its border on focus changes;
            ' repaint our custom border immediately after.
            If (isComboBox OrElse isListBox) AndAlso
               (msgId = Constants.WM_SETFOCUS OrElse
                msgId = Constants.WM_KILLFOCUS) Then

                Me.DrawCustomControlBorder()
            End If
        End Sub

#End Region

#Region " Private Methods "

        ''' <summary>
        ''' Hooks the parent control of the target control to intercept
        ''' <c>WM_ERASEBKGND</c> and exclude this control's rectangle,
        ''' preventing the parent from erasing the custom border area.
        ''' </summary>
        Private Sub HookParent()

            Me.UnhookParent()

            Dim parent As Control = Me.Control.Parent
            If parent Is Nothing OrElse Not parent.IsHandleCreated Then
                Return
            End If

            Dim parentHandle As IntPtr = parent.Handle
            Dim hook As ParentHook = Nothing

            If Not ControlBorderPainter.parentHooks.TryGetValue(parentHandle, hook) Then
                hook = New ParentHook(parentHandle)
                ControlBorderPainter.parentHooks(parentHandle) = hook
            End If

            hook.AddChild(Me.Control)
            Me.hookedParentHandle = parentHandle
        End Sub

        ''' <summary>
        ''' Unhooks this control from its parent's <see cref="ParentHook"/>.
        ''' If no more children are registered, the hook is removed entirely.
        ''' </summary>
        Private Sub UnhookParent()

            If Me.hookedParentHandle = IntPtr.Zero Then
                Return
            End If

            Dim hook As ParentHook = Nothing
            If ControlBorderPainter.parentHooks.TryGetValue(Me.hookedParentHandle, hook) Then
                hook.RemoveChild(Me.Control)
                If hook.ChildCount = 0 Then
                    hook.Unhook()
                    ControlBorderPainter.parentHooks.Remove(Me.hookedParentHandle)
                End If
            End If

            Me.hookedParentHandle = IntPtr.Zero
        End Sub

        ''' <summary>
        ''' Ensures the parent of the target control has the <c>WS_CLIPCHILDREN</c>
        ''' style so that when the parent repaints (via <c>WM_PAINT</c>) it does not 
        ''' overwrite child areas, preventing a one-frame flash of the parent 
        ''' background over the custom border.
        ''' </summary>
        Private Sub EnsureParentClipsChildren()
            Dim parent As Control = Me.Control.Parent
            If parent Is Nothing OrElse Not parent.IsHandleCreated Then
                Return
            End If

            Dim style As Long = NativeMethods.GetWindowLongPtr(parent.Handle, Constants.GWL_STYLE).ToInt64()
            If (style And CLng(Constants.WS_CLIPCHILDREN)) = 0L Then
                Dim result As IntPtr = NativeMethods.SetWindowLongPtr(parent.Handle, Constants.GWL_STYLE,
                                                                      New IntPtr(style Or CLng(Constants.WS_CLIPCHILDREN)))
            End If
        End Sub

        ''' <summary>
        ''' Draws a custom border over a <see cref="System.Windows.Forms.Control"/>.
        ''' </summary>
        Private Sub DrawCustomControlBorder()

            Dim hDC As IntPtr = NativeMethods.GetWindowDC(Me.Control.Handle)
            If hDC = IntPtr.Zero Then
                Return
            End If

            Try
                Using g As Graphics = Graphics.FromHdc(hDC)

                    Dim borderRect As New Rectangle(0, 0, Me.Control.Width - 1, Me.Control.Height - 1)

                    Using borderPen As New Pen(Me.borderColorValue, Me.borderThicknessValue)
                        g.DrawRectangle(borderPen, borderRect)
                    End Using
                End Using

            Finally
                Dim result As Integer = NativeMethods.ReleaseDC(Me.Control.Handle, hDC)

            End Try
        End Sub

        ''' <summary>
        ''' Draws a custom border over a <see cref="System.Windows.Forms.TabControl"/>.
        ''' </summary>
        Private Sub DrawCustomTabControlBorder()

            Dim hDC As IntPtr = NativeMethods.GetWindowDC(Me.Control.Handle)
            If hDC = IntPtr.Zero Then
                Return
            End If

            Try
                Using g As Graphics = Graphics.FromHdc(hDC)

                    Dim tc As TabControl = DirectCast(Me.Control, TabControl)

                    Dim dr As Rectangle = tc.DisplayRectangle
                    Dim w As Integer = tc.Width
                    Dim h As Integer = tc.Height

                    ' The native 3-D border occupies exactly the margins between the
                    ' control bounds and DisplayRectangle on left, right and bottom,
                    ' and between GetTabRect(0).Bottom and DisplayRectangle.Y on top.
                    Dim marginLeft As Integer = dr.X
                    Dim marginRight As Integer = w - dr.Right
                    Dim marginBottom As Integer = h - dr.Bottom
                    Dim tabsBottom As Integer = tc.GetTabRect(0).Bottom
                    Dim contentY As Integer = tabsBottom

                    Using backBrush As New SolidBrush(tc.Parent.BackColor)
                        g.FillRectangle(backBrush, New Rectangle(0, contentY, marginLeft, h - contentY))                ' left
                        g.FillRectangle(backBrush, New Rectangle(w - marginRight, contentY, marginRight, h - contentY)) ' right
                        g.FillRectangle(backBrush, New Rectangle(0, h - marginBottom, w, marginBottom))                 ' bottom
                        g.FillRectangle(backBrush, New Rectangle(0, contentY, w, dr.Y - contentY))                      ' top strip below tabs
                    End Using

                    Dim borderRect As New Rectangle(0, contentY, w - 1, h - contentY - 1)

                    Using borderPen As New Pen(Me.borderColorValue, Me.borderThicknessValue)
                        g.DrawRectangle(borderPen, borderRect)
                    End Using

                End Using

            Finally
                Dim result As Integer = NativeMethods.ReleaseDC(Me.Control.Handle, hDC)

            End Try

        End Sub

        ''' <summary>
        ''' Performs a fully double-buffered repaint of the <see cref="ComboBox"/>:
        ''' renders the control to an off-screen bitmap via <c>WM_PRINT</c>,
        ''' draws the custom border on the bitmap, and then <c>BitBlt</c>'s the
        ''' final result to the screen in a single, flicker-free operation.
        ''' </summary>
        Private Sub PaintComboBoxBuffered()

            Dim hWnd As IntPtr = Me.Control.Handle
            Dim w As Integer = Me.Control.Width
            Dim h As Integer = Me.Control.Height

            ' Validate the entire window so the default WM_PAINT handler
            ' won't fire when we return; we handle all painting ourselves.
            Dim result1 As Boolean = NativeMethods.ValidateRect(hWnd, IntPtr.Zero)

            ' Acquire the window DC (covers the full window including NC border area).
            Dim screenDC As IntPtr = NativeMethods.GetWindowDC(hWnd)
            If screenDC = IntPtr.Zero Then
                Return
            End If

            Try
                ' Create the back buffer (memory DC + compatible bitmap).
                Dim memDC As IntPtr = NativeMethods.CreateCompatibleDC(screenDC)
                If memDC = IntPtr.Zero Then
                    Return
                End If

                Try
                    Dim memBmp As IntPtr = NativeMethods.CreateCompatibleBitmap(screenDC, w, h)
                    If memBmp = IntPtr.Zero Then
                        Return
                    End If

                    Dim oldBmp As IntPtr = NativeMethods.SelectObject(memDC, memBmp)

                    Try
                        ' Set reentrancy guard.
                        Me.isBufferedPainting = True

                        ' Render the entire control (client + non-client + children)
                        ' to the memory DC via WM_PRINT.
                        Dim printFlags As Integer = Constants.PRF_CLIENT Or
                                                    Constants.PRF_NONCLIENT Or
                                                    Constants.PRF_CHILDREN Or
                                                    Constants.PRF_ERASEBKGND

                        Dim result2 As IntPtr = NativeMethods.SendMessage(
                            hWnd, Constants.WM_PRINT, memDC, New IntPtr(printFlags))

                        Me.isBufferedPainting = False

                        ' Overwrite the native border on the back buffer.
                        Using g As Graphics = Graphics.FromHdc(memDC)
                            Dim borderRect As New Rectangle(0, 0, w - 1, h - 1)
                            Using borderPen As New Pen(Me.borderColorValue, Me.borderThicknessValue)
                                g.DrawRectangle(borderPen, borderRect)
                            End Using
                        End Using

                        ' Blit the final composited image to the screen atomically.
                        Dim result3 As Boolean = NativeMethods.BitBlt(
                            screenDC, 0, 0, w, h,
                            memDC, 0, 0,
                            Constants.SRCCOPY)

                    Finally
                        Me.isBufferedPainting = False
                        Dim result4 As IntPtr = NativeMethods.SelectObject(memDC, oldBmp)
                        Dim result5 As Boolean = NativeMethods.DeleteObject(memBmp)
                    End Try

                Finally
                    Dim result6 As Boolean = NativeMethods.DeleteDC(memDC)
                End Try

            Finally
                Dim result7 As Integer = NativeMethods.ReleaseDC(hWnd, screenDC)
            End Try
        End Sub

#End Region

#Region " IDisposable "

        ''' <summary>
        ''' Releases all resources used by this <see cref="ControlBorderPainter"/> instance.
        ''' </summary>
        Public Sub Dispose() Implements IDisposable.Dispose

            Me.Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        ''' <summary>
        ''' Releases unmanaged and - optionally - managed resources.
        ''' </summary>
        ''' 
        ''' <param name="disposing">
        ''' <see langword="True"/> to release both managed and unmanaged resources; 
        ''' <see langword="False"/> to release only unmanaged resources.
        ''' </param>
        Private Sub Dispose(disposing As Boolean)

            If Me.disposed Then
                Return
            End If
            Me.disposed = True

            ' Managed resources.
            If disposing Then
                RemoveHandler Me.Control.HandleCreated, AddressOf Me.Control_HandleCreated
                RemoveHandler Me.Control.HandleDestroyed, AddressOf Me.Control_HandleDestroyed

                If TypeOf Me.Control Is GroupBox Then
                    RemoveHandler Me.Control.Paint, AddressOf Me.GroupBox_Paint
                End If

                If TypeOf Me.Control Is ComboBox Then
                    ' Restore visual styles (UxTheme) rendering on the control.
                    Dim result As Integer = NativeMethods.SetWindowTheme(Me.Control.Handle, Nothing, Nothing)
                End If

                Me.UnhookParent()

                Me.borderColorValue = Color.Empty
                Me.borderThicknessValue = Single.NaN

            End If

            ' Unmanaged resources.
            Me.ReleaseHandle()
        End Sub

        ''' <summary>
        ''' Finalizer. Acts as a safety net if <see cref="ControlBorderPainter.Dispose()"/> was not called.
        ''' </summary>
        Protected Overrides Sub Finalize()

            Me.Dispose(disposing:=False)
            MyBase.Finalize()
        End Sub

#End Region

    End Class

End Namespace

#End Region