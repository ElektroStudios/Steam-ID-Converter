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

        <DllImport("user32.dll")>
        Friend Shared Function GetWindowDC(hWnd As IntPtr) As IntPtr
        End Function

        <DllImport("user32.dll")>
        Friend Shared Function ReleaseDC(hWnd As IntPtr, hDC As IntPtr) As Integer
        End Function

        <DllImport("dwmapi.dll", PreserveSig:=True)>
        Friend Shared Function DwmSetWindowAttribute(
            hWnd As IntPtr,
            attribute As Integer,
            ByRef refAttributeValue As Integer,
            attributeSize As Integer) As Integer
        End Function

    End Class

End Namespace
