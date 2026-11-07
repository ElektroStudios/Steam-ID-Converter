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

Imports SteamIDConverter.Helpers

#End Region

#Region " SteamUtilTests "

Namespace SteamIDConverter.Tests.Helpers

    ''' <summary>
    ''' Unit tests for the <see cref="SteamUtil"/> class.
    ''' </summary>
    <TestClass>
    Public NotInheritable Class SteamUtilTests

#Region " Constants "

        ''' <summary>
        ''' Tests that <see cref="SteamUtil.SteamID64Base"/> matches the documented value
        ''' for an individual account in the public universe (<c>0x0110000100000000</c>).
        ''' </summary>
        <TestMethod>
        Public Sub SteamID64Base_MatchesDocumentedValue()

            Assert.AreEqual(&H110000100000000UL, SteamUtil.SteamID64Base)
            Assert.AreEqual(76561197960265728UL, SteamUtil.SteamID64Base)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="SteamUtil.MaxSteamID64"/> is the sum of
        ''' <see cref="SteamUtil.SteamID64Base"/> and <see cref="UInteger.MaxValue"/>.
        ''' </summary>
        <TestMethod>
        Public Sub MaxSteamID64_IsBasePlusMaxAccountNumber()

            Assert.AreEqual(76561202255233023UL, SteamUtil.MaxSteamID64)
        End Sub

#End Region

#Region " ConvertSteamID32ToSteamID64 "

        ''' <summary>
        ''' Tests that <see cref="SteamUtil.ConvertSteamID32ToSteamID64"/> returns the expected SteamID64 for known values.
        ''' </summary>
        <TestMethod>
        <DataRow(872980358UI, 76561198833246086UL, DisplayName:="Documented example")>
        <DataRow(1638UI, 76561197960267366UL, DisplayName:="Codex (CDX) preset")>
        <DataRow(22202UI, 76561197960287930UL, DisplayName:="DenuvOwO Repacks preset")>
        <DataRow(4919UI, 76561197960270647UL, DisplayName:="Reloaded (RLD) preset")>
        <DataRow(4660UI, 76561197960270388UL, DisplayName:="Tenoke preset")>
        <DataRow(1UI, 76561197960265729UL, DisplayName:="Lowest valid account number")>
        Public Sub ConvertSteamID32ToSteamID64_KnownValues_ReturnsExpected(steamID32 As UInteger, expectedSteamID64 As ULong)

            Dim actual As ULong = SteamUtil.ConvertSteamID32ToSteamID64(steamID32)

            Assert.AreEqual(expectedSteamID64, actual)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="SteamUtil.ConvertSteamID32ToSteamID64"/> maps zero to <see cref="SteamUtil.SteamID64Base"/>.
        ''' </summary>
        <TestMethod>
        Public Sub ConvertSteamID32ToSteamID64_Zero_ReturnsBase()

            Assert.AreEqual(SteamUtil.SteamID64Base, SteamUtil.ConvertSteamID32ToSteamID64(0UI))
        End Sub

        ''' <summary>
        ''' Tests that <see cref="SteamUtil.ConvertSteamID32ToSteamID64"/> maps
        ''' <see cref="UInteger.MaxValue"/> to <see cref="SteamUtil.MaxSteamID64"/> without overflowing.
        ''' </summary>
        <TestMethod>
        Public Sub ConvertSteamID32ToSteamID64_MaxValue_ReturnsMaxSteamID64()

            Assert.AreEqual(SteamUtil.MaxSteamID64, SteamUtil.ConvertSteamID32ToSteamID64(UInteger.MaxValue))
        End Sub

#End Region

#Region " ConvertSteamID64ToSteamID32 "

        ''' <summary>
        ''' Tests that <see cref="SteamUtil.ConvertSteamID64ToSteamID32"/> returns the expected SteamID32 for known values.
        ''' </summary>
        <TestMethod>
        <DataRow(76561198833246086UL, 872980358UI, DisplayName:="Documented example")>
        <DataRow(76561197960267366UL, 1638UI, DisplayName:="Codex (CDX) preset")>
        <DataRow(76561197960287930UL, 22202UI, DisplayName:="DenuvOwO Repacks preset")>
        <DataRow(76561197960270647UL, 4919UI, DisplayName:="Reloaded (RLD) preset")>
        <DataRow(76561197960270388UL, 4660UI, DisplayName:="Tenoke preset")>
        <DataRow(76561197960265729UL, 1UI, DisplayName:="Lowest valid account number")>
        Public Sub ConvertSteamID64ToSteamID32_KnownValues_ReturnsExpected(steamID64 As ULong, expectedSteamID32 As UInteger)

            Dim actual As UInteger = SteamUtil.ConvertSteamID64ToSteamID32(steamID64)

            Assert.AreEqual(expectedSteamID32, actual)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="SteamUtil.ConvertSteamID64ToSteamID32"/> maps <see cref="SteamUtil.SteamID64Base"/> to zero.
        ''' </summary>
        <TestMethod>
        Public Sub ConvertSteamID64ToSteamID32_Base_ReturnsZero()

            Assert.AreEqual(0UI, SteamUtil.ConvertSteamID64ToSteamID32(SteamUtil.SteamID64Base))
        End Sub

        ''' <summary>
        ''' Tests that <see cref="SteamUtil.ConvertSteamID64ToSteamID32"/> maps
        ''' <see cref="SteamUtil.MaxSteamID64"/> to <see cref="UInteger.MaxValue"/>.
        ''' </summary>
        <TestMethod>
        Public Sub ConvertSteamID64ToSteamID32_MaxSteamID64_ReturnsMaxValue()

            Assert.AreEqual(UInteger.MaxValue, SteamUtil.ConvertSteamID64ToSteamID32(SteamUtil.MaxSteamID64))
        End Sub

        ''' <summary>
        ''' Tests that <see cref="SteamUtil.ConvertSteamID64ToSteamID32"/> throws
        ''' <see cref="ArgumentOutOfRangeException"/> for values outside the convertible range.
        ''' </summary>
        <TestMethod>
        <DataRow(0UL, DisplayName:="Zero")>
        <DataRow(1UL, DisplayName:="One")>
        <DataRow(4294967295UL, DisplayName:="UInteger.MaxValue")>
        <DataRow(76561197960265727UL, DisplayName:="Base minus one")>
        <DataRow(76561202255233024UL, DisplayName:="MaxSteamID64 plus one")>
        <DataRow(18446744073709551615UL, DisplayName:="ULong.MaxValue")>
        Public Sub ConvertSteamID64ToSteamID32_OutOfRange_ThrowsArgumentOutOfRangeException(steamID64 As ULong)

            Dim ex As ArgumentOutOfRangeException =
                Assert.ThrowsExactly(Of ArgumentOutOfRangeException)(Sub() SteamUtil.ConvertSteamID64ToSteamID32(steamID64))

            Assert.AreEqual("steamID64", ex.ParamName)
            Assert.AreEqual(CObj(steamID64), ex.ActualValue)
        End Sub

#End Region

#Region " Round-trip "

        ''' <summary>
        ''' Tests that converting a SteamID32 to SteamID64 and back returns the original value.
        ''' </summary>
        <TestMethod>
        <DataRow(0UI)>
        <DataRow(1UI)>
        <DataRow(1638UI)>
        <DataRow(872980358UI)>
        <DataRow(2147483647UI)>
        <DataRow(2147483648UI)>
        <DataRow(4294967294UI)>
        <DataRow(4294967295UI)>
        Public Sub RoundTrip_SteamID32_ReturnsOriginalValue(steamID32 As UInteger)

            Dim steamID64 As ULong = SteamUtil.ConvertSteamID32ToSteamID64(steamID32)
            Dim roundTrip As UInteger = SteamUtil.ConvertSteamID64ToSteamID32(steamID64)

            Assert.AreEqual(steamID32, roundTrip)
        End Sub

        ''' <summary>
        ''' Tests that converting a SteamID64 to SteamID32 and back returns the original value.
        ''' </summary>
        <TestMethod>
        <DataRow(76561197960265728UL)>
        <DataRow(76561198833246086UL)>
        <DataRow(76561200107749375UL)>
        <DataRow(76561202255233023UL)>
        Public Sub RoundTrip_SteamID64_ReturnsOriginalValue(steamID64 As ULong)

            Dim steamID32 As UInteger = SteamUtil.ConvertSteamID64ToSteamID32(steamID64)
            Dim roundTrip As ULong = SteamUtil.ConvertSteamID32ToSteamID64(steamID32)

            Assert.AreEqual(steamID64, roundTrip)
        End Sub

#End Region

    End Class

End Namespace

#End Region
