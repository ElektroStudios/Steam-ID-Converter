' ***********************************************************************
' Author   : ElektroStudios
' Modified : 08-April-2026
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

Namespace SteamIDConverter.Win32

    Friend NotInheritable Class Constants

#Region " DWM Window Attributes "

        Friend Const DWMWA_USE_IMMERSIVE_DARK_MODE As Integer = 20

#End Region

#Region " Raster Operation Codes "

        Friend Const SRCCOPY As Integer = &HCC0020

#End Region

#Region " Window Messages "

        Friend Const WM_ERASEBKGND As Integer = &H14
        Friend Const WM_NCPAINT As Integer = &H85
        Friend Const WM_PAINT As Integer = &HF
        Friend Const WM_PRINT As Integer = &H317

        Friend Const WM_KILLFOCUS As Integer = &H8
        Friend Const WM_SETFOCUS As Integer = &H7

#End Region

#Region " Window Styles "

        Friend Const GWL_STYLE As Integer = -16
        Friend Const WS_CLIPCHILDREN As Integer = &H2000000

#End Region

#Region " WM_PRINT Flags "

        Friend Const PRF_CLIENT As Integer = &H4

        Friend Const PRF_NONCLIENT As Integer = &H2

        Friend Const PRF_CHILDREN As Integer = &H10

        Friend Const PRF_ERASEBKGND As Integer = &H8

#End Region

    End Class

End Namespace
