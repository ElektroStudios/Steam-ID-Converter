' ***********************************************************************
' Author   : ElektroStudios
' Modified : 07-November-2026
' ***********************************************************************

#Region " Option Statements "

Option Strict On
Option Explicit On
Option Infer Off

#End Region

#Region " Imports "

Imports Microsoft.VisualStudio.TestTools.UnitTesting

#End Region

#Region " Assembly Attributes "

' Some tests modify the shared (static) configuration properties of FlexibleSettingsProvider class,
' so tests must not run in parallel to keep them deterministic.
<Assembly: DoNotParallelize>

#End Region
