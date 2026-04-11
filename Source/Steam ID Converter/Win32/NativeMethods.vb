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

Imports System.Runtime.InteropServices
Imports System.Security

#End Region

Namespace SteamIDConverter.Win32

    ''' <summary>
    ''' Provides P/Invoke declarations for unmanaged Windows API functions.
    ''' </summary>
    <SuppressUnmanagedCodeSecurity>
    Friend NotInheritable Class NativeMethods

#Region " gdi32.dll "

        <DllImport("gdi32.dll")>
        Friend Shared Function BitBlt(hdcDest As IntPtr, nXDest As Integer, nYDest As Integer,
                                      nWidth As Integer, nHeight As Integer,
                                      hdcSrc As IntPtr, nXSrc As Integer, nYSrc As Integer,
                                      dwRop As Integer
        ) As Boolean
        End Function

        <DllImport("gdi32.dll")>
        Friend Shared Function CreateCompatibleDC(hdc As IntPtr
        ) As IntPtr
        End Function

        <DllImport("gdi32.dll")>
        Friend Shared Function CreateCompatibleBitmap(hdc As IntPtr,
                                                      nWidth As Integer,
                                                      nHeight As Integer
        ) As IntPtr
        End Function

        <DllImport("gdi32.dll")>
        Friend Shared Function DeleteDC(hdc As IntPtr
        ) As Boolean
        End Function

        <DllImport("gdi32.dll")>
        Friend Shared Function DeleteObject(hObject As IntPtr
        ) As Boolean
        End Function

        <DllImport("gdi32.dll")>
        Friend Shared Function ExcludeClipRect(hdc As IntPtr,
                                               nLeftRect As Integer,
                                               nTopRect As Integer,
                                               nRightRect As Integer,
                                               nBottomRect As Integer
        ) As Integer
        End Function

        <DllImport("gdi32.dll")>
        Friend Shared Function SelectObject(hdc As IntPtr,
                                            hgdiobj As IntPtr
        ) As IntPtr
        End Function

#End Region

#Region " dwmapi.dll "

        <DllImport("dwmapi.dll", PreserveSig:=True)>
        Friend Shared Function DwmSetWindowAttribute(hWnd As IntPtr,
                                                     attribute As Integer,
                                               ByRef refAttributeValue As Integer,
                                                     attributeSize As Integer
        ) As Integer
        End Function

#End Region

#Region " user32.dll "

        <DllImport("user32.dll", EntryPoint:="GetWindowLongPtrW")>
        Friend Shared Function GetWindowLongPtr(hWnd As IntPtr,
                                                nIndex As Integer) As IntPtr
        End Function

        <DllImport("user32.dll", EntryPoint:="SetWindowLongPtrW")>
        Friend Shared Function SetWindowLongPtr(hWnd As IntPtr,
                                                nIndex As Integer,
                                                dwNewLong As IntPtr) As IntPtr
        End Function

        <DllImport("user32.dll")>
        Friend Shared Function GetWindowDC(hWnd As IntPtr
        ) As IntPtr
        End Function

        <DllImport("user32.dll")>
        Friend Shared Function ReleaseDC(hWnd As IntPtr,
                                         hDC As IntPtr
        ) As Integer
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Auto)>
        Friend Shared Function SendMessage(hWnd As IntPtr,
                                           msg As Integer,
                                           wParam As IntPtr,
                                           lParam As IntPtr
        ) As IntPtr
        End Function

        <DllImport("user32.dll")>
        Friend Shared Function ValidateRect(hWnd As IntPtr,
                                            lpRect As IntPtr
        ) As Boolean
        End Function

#End Region

#Region " uxtheme.dll "

        <DllImport("uxtheme.dll", CharSet:=CharSet.Unicode)>
        Friend Shared Function SetWindowTheme(hWnd As IntPtr,
                                              subAppName As String,
                                              subIdList As String
        ) As Integer
        End Function

#End Region

    End Class

End Namespace
