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

Imports System.Collections.Specialized
Imports System.Configuration
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text.RegularExpressions

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports SteamIDConverter.Helpers

#End Region

#Region " FlexibleSettingsProviderTests "

Namespace SteamIDConverter.Tests.Helpers

    ''' <summary>
    ''' Unit tests for the <see cref="FlexibleSettingsProvider"/> class.
    ''' </summary>
    '''
    ''' <remarks>
    ''' <see cref="FlexibleSettingsProvider"/> exposes its configuration through shared (static) properties,
    ''' so every test saves and restores them, and works inside its own temporary directory.
    ''' </remarks>
    <TestClass>
    Public NotInheritable Class FlexibleSettingsProviderTests

#Region " Fields "

        ''' <summary>
        ''' The temporary directory used by the current test.
        ''' </summary>
        Private testDirectoryPath As String

        ' Original values of the shared properties, restored after each test.
        Private originalBaseDirectoryPath As String
        Private originalDirectoryName As String
        Private originalDirectoryNameFlags As SettingsDirectoryNameFlags
        Private originalFileName As String
        Private originalHashAlgorithmType As Type
        Private originalHashLength As Integer

#End Region

#Region " Test Initialization / Cleanup "

        ''' <summary>
        ''' Saves the shared configuration of <see cref="FlexibleSettingsProvider"/>
        ''' and creates a new temporary directory for the current test.
        ''' </summary>
        <TestInitialize>
        Public Sub TestInitialize()

            Me.originalBaseDirectoryPath = FlexibleSettingsProvider.BaseDirectoryPath
            Me.originalDirectoryName = FlexibleSettingsProvider.DirectoryName
            Me.originalDirectoryNameFlags = FlexibleSettingsProvider.DirectoryNameFlags
            Me.originalFileName = FlexibleSettingsProvider.FileName
            Me.originalHashAlgorithmType = FlexibleSettingsProvider.HashAlgorithmType
            Me.originalHashLength = FlexibleSettingsProvider.HashLength

            Me.testDirectoryPath = Path.Combine(Path.GetTempPath(), "SteamIDConverter.Tests", Guid.NewGuid().ToString("N"))
            Directory.CreateDirectory(Me.testDirectoryPath)

            FlexibleSettingsProvider.BaseDirectoryPath = Me.testDirectoryPath
            FlexibleSettingsProvider.DirectoryName = "Settings"
            FlexibleSettingsProvider.DirectoryNameFlags = SettingsDirectoryNameFlags.None
            FlexibleSettingsProvider.FileName = "test.config"
            FlexibleSettingsProvider.HashAlgorithmType = GetType(MD5)
            FlexibleSettingsProvider.HashLength = 8
        End Sub

        ''' <summary>
        ''' Restores the shared configuration of <see cref="FlexibleSettingsProvider"/>
        ''' and deletes the temporary directory of the current test.
        ''' </summary>
        <TestCleanup>
        Public Sub TestCleanup()

            FlexibleSettingsProvider.BaseDirectoryPath = Me.originalBaseDirectoryPath
            FlexibleSettingsProvider.DirectoryName = Me.originalDirectoryName
            FlexibleSettingsProvider.DirectoryNameFlags = Me.originalDirectoryNameFlags
            FlexibleSettingsProvider.FileName = Me.originalFileName
            FlexibleSettingsProvider.HashAlgorithmType = Me.originalHashAlgorithmType
            FlexibleSettingsProvider.HashLength = Me.originalHashLength

            If Directory.Exists(Me.testDirectoryPath) Then
                Directory.Delete(Me.testDirectoryPath, recursive:=True)
            End If
        End Sub

#End Region

#Region " Effective paths "

        ''' <summary>
        ''' Tests that <see cref="FlexibleSettingsProvider.EffectiveConfigFilePath"/> combines
        ''' the base directory, the directory name and the file name.
        ''' </summary>
        <TestMethod>
        Public Sub EffectiveConfigFilePath_CombinesAllParts()

            Dim expected As String = Path.Combine(Me.testDirectoryPath, "Settings", "test.config")

            Assert.AreEqual(expected, FlexibleSettingsProvider.EffectiveConfigFilePath, ignoreCase:=True)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="FlexibleSettingsProvider.EffectiveConfigFilePath"/> places the file
        ''' directly in the base directory when no directory name is specified.
        ''' </summary>
        <TestMethod>
        <DataRow(Nothing, DisplayName:="Null")>
        <DataRow("", DisplayName:="Empty")>
        <DataRow("   ", DisplayName:="White-spaces")>
        Public Sub EffectiveConfigFilePath_NoDirectoryName_UsesBaseDirectory(directoryName As String)

            FlexibleSettingsProvider.DirectoryName = directoryName

            Dim expected As String = Path.Combine(Me.testDirectoryPath, "test.config")

            Assert.AreEqual(expected, FlexibleSettingsProvider.EffectiveConfigFilePath, ignoreCase:=True)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="FlexibleSettingsProvider.EffectiveConfigFilePath"/> falls back
        ''' to <b>"user.config"</b> when no file name is specified.
        ''' </summary>
        <TestMethod>
        <DataRow(Nothing, DisplayName:="Null")>
        <DataRow("", DisplayName:="Empty")>
        <DataRow("   ", DisplayName:="White-spaces")>
        Public Sub EffectiveConfigFilePath_NoFileName_UsesDefaultFileName(fileName As String)

            FlexibleSettingsProvider.FileName = fileName

            Assert.AreEqual("user.config", Path.GetFileName(FlexibleSettingsProvider.EffectiveConfigFilePath))
        End Sub

        ''' <summary>
        ''' Tests that <see cref="FlexibleSettingsProvider.EffectiveConfigFilePath"/> expands a relative base directory path.
        ''' </summary>
        <TestMethod>
        Public Sub EffectiveConfigFilePath_RelativeBaseDirectory_IsExpandedToFullPath()

            FlexibleSettingsProvider.BaseDirectoryPath = ".\"
            FlexibleSettingsProvider.DirectoryName = Nothing

            Dim expected As String = Path.Combine(Path.GetFullPath(".\"), "test.config")

            Assert.AreEqual(expected, FlexibleSettingsProvider.EffectiveConfigFilePath, ignoreCase:=True)
            Assert.IsTrue(Path.IsPathFullyQualified(FlexibleSettingsProvider.EffectiveConfigFilePath))
        End Sub

        ''' <summary>
        ''' Tests that <see cref="FlexibleSettingsProvider.EffectiveConfigFilePath"/> creates
        ''' the base directory when it does not exist.
        ''' </summary>
        <TestMethod>
        Public Sub EffectiveConfigFilePath_MissingBaseDirectory_IsCreated()

            Dim missingDirectoryPath As String = Path.Combine(Me.testDirectoryPath, "Missing", "Nested")
            FlexibleSettingsProvider.BaseDirectoryPath = missingDirectoryPath

            Dim effectivePath As String = FlexibleSettingsProvider.EffectiveConfigFilePath

            Assert.IsTrue(Directory.Exists(missingDirectoryPath))
            StringAssert.StartsWith(effectivePath, missingDirectoryPath, StringComparison.OrdinalIgnoreCase)
        End Sub

#End Region

#Region " Directory name flags "

        ''' <summary>
        ''' Tests that <see cref="SettingsDirectoryNameFlags.UserName"/> appends the current user name
        ''' to the directory name, separated by an underscore.
        ''' </summary>
        <TestMethod>
        Public Sub DirectoryNameFlags_UserName_AppendsUserName()

            FlexibleSettingsProvider.DirectoryNameFlags = SettingsDirectoryNameFlags.UserName

            Dim provider As New FlexibleSettingsProvider()

            Assert.AreEqual($"Settings_{Environment.UserName}", provider.ApplicationName)
        End Sub

        ''' <summary>
        ''' Tests that no leading underscore is added when a flag is used without a directory name.
        ''' </summary>
        <TestMethod>
        Public Sub DirectoryNameFlags_UserNameWithoutDirectoryName_HasNoLeadingUnderscore()

            FlexibleSettingsProvider.DirectoryName = Nothing
            FlexibleSettingsProvider.DirectoryNameFlags = SettingsDirectoryNameFlags.UserName

            Dim provider As New FlexibleSettingsProvider()

            Assert.AreEqual(Environment.UserName, provider.ApplicationName)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="SettingsDirectoryNameFlags.None"/> uses the directory name as is.
        ''' </summary>
        <TestMethod>
        Public Sub DirectoryNameFlags_None_UsesDirectoryNameAsIs()

            Dim provider As New FlexibleSettingsProvider()

            Assert.AreEqual("Settings", provider.ApplicationName)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="SettingsDirectoryNameFlags.Hash"/> appends a deterministic
        ''' uppercase hexadecimal hash with the requested length.
        ''' </summary>
        <TestMethod>
        <DataRow(8, DisplayName:="Even length")>
        <DataRow(5, DisplayName:="Odd length")>
        <DataRow(1, DisplayName:="Single character")>
        <DataRow(32, DisplayName:="Full MD5 length")>
        Public Sub DirectoryNameFlags_Hash_AppendsDeterministicHashWithRequestedLength(hashLength As Integer)

            FlexibleSettingsProvider.DirectoryNameFlags = SettingsDirectoryNameFlags.Hash
            FlexibleSettingsProvider.HashLength = hashLength

            Dim provider As New FlexibleSettingsProvider()
            Dim first As String = provider.ApplicationName
            Dim second As String = provider.ApplicationName

            Assert.AreEqual(first, second, "The hash must be deterministic.")
            Assert.IsTrue(Regex.IsMatch(first, $"^Settings_[0-9A-F]{{{hashLength}}}$"), $"Actual: {first}")
        End Sub

        ''' <summary>
        ''' Tests that <see cref="FlexibleSettingsProvider.HashLength"/> is truncated to the
        ''' maximum length supported by the hash algorithm.
        ''' </summary>
        <TestMethod>
        <DataRow(GetType(MD5), 32, DisplayName:="MD5")>
        <DataRow(GetType(SHA1), 40, DisplayName:="SHA1")>
        <DataRow(GetType(SHA256), 64, DisplayName:="SHA256")>
        Public Sub DirectoryNameFlags_HashLengthExceedingAlgorithm_IsTruncated(algorithmType As Type, maxLength As Integer)

            FlexibleSettingsProvider.DirectoryName = Nothing
            FlexibleSettingsProvider.DirectoryNameFlags = SettingsDirectoryNameFlags.Hash
            FlexibleSettingsProvider.HashAlgorithmType = algorithmType
            FlexibleSettingsProvider.HashLength = 1000

            Dim provider As New FlexibleSettingsProvider()

            Assert.AreEqual(maxLength, provider.ApplicationName.Length)
        End Sub

        ''' <summary>
        ''' Tests that the hash prefix is stable when the requested length grows.
        ''' </summary>
        <TestMethod>
        Public Sub DirectoryNameFlags_Hash_ShorterHashIsPrefixOfLongerHash()

            FlexibleSettingsProvider.DirectoryName = Nothing
            FlexibleSettingsProvider.DirectoryNameFlags = SettingsDirectoryNameFlags.Hash
            Dim provider As New FlexibleSettingsProvider()

            FlexibleSettingsProvider.HashLength = 7
            Dim shortHash As String = provider.ApplicationName

            FlexibleSettingsProvider.HashLength = 16
            Dim longHash As String = provider.ApplicationName

            StringAssert.StartsWith(longHash, shortHash, StringComparison.Ordinal)
        End Sub

        ''' <summary>
        ''' Tests that every <see cref="SettingsDirectoryNameFlags"/> value except 
        ''' <see cref="SettingsDirectoryNameFlags.None"/> is a distinct single-bit flag.
        ''' </summary>
        <TestMethod>
        Public Sub SettingsDirectoryNameFlags_AreDistinctSingleBitFlags()

            Dim accumulatedBits As Integer = 0

            For Each flag As SettingsDirectoryNameFlags In [Enum].GetValues(Of SettingsDirectoryNameFlags)()
                Dim bits As Integer = CInt(flag)

                If flag = SettingsDirectoryNameFlags.None Then
                    Assert.AreEqual(0, bits)
                    Continue For
                End If

                Assert.AreEqual(1, Integer.PopCount(bits), $"Flag {flag} must have exactly one bit set.")
                Assert.AreEqual(0, accumulatedBits And bits, $"Flag {flag} overlaps another flag.")
                accumulatedBits = accumulatedBits Or bits
            Next
        End Sub

#End Region

#Region " Provider members "

        ''' <summary>
        ''' Tests that setting <see cref="FlexibleSettingsProvider.ApplicationName"/> is intentionally ignored.
        ''' </summary>
        <TestMethod>
        Public Sub ApplicationName_Setter_IsIgnored()

            Dim provider As New FlexibleSettingsProvider With {
                .ApplicationName = "Something else"
            }

            Assert.AreEqual("Settings", provider.ApplicationName)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="FlexibleSettingsProvider.Initialize"/> uses the type name
        ''' as the provider name when no name is specified.
        ''' </summary>
        <TestMethod>
        <DataRow(Nothing, DisplayName:="Null")>
        <DataRow("", DisplayName:="Empty")>
        Public Sub Initialize_NoName_UsesTypeName(name As String)

            Dim provider As New FlexibleSettingsProvider()
            provider.Initialize(name, Nothing)

            Assert.AreEqual(NameOf(FlexibleSettingsProvider), provider.Name)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="FlexibleSettingsProvider.Initialize"/> keeps the specified provider name.
        ''' </summary>
        <TestMethod>
        Public Sub Initialize_WithName_KeepsName()

            Dim provider As New FlexibleSettingsProvider()
            provider.Initialize("CustomProvider", Nothing)

            Assert.AreEqual("CustomProvider", provider.Name)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="FlexibleSettingsProvider.Description"/> is not empty.
        ''' </summary>
        <TestMethod>
        Public Sub Description_IsNotEmpty()

            Dim provider As New FlexibleSettingsProvider()

            Assert.IsFalse(String.IsNullOrWhiteSpace(provider.Description))
        End Sub

#End Region

#Region " GetPropertyValues "

        ''' <summary>
        ''' Tests that <see cref="FlexibleSettingsProvider.GetPropertyValues"/> returns the default values
        ''' when the configuration file does not exist.
        ''' </summary>
        <TestMethod>
        Public Sub GetPropertyValues_MissingFile_ReturnsDefaultValues()

            Dim provider As New FlexibleSettingsProvider()
            Dim properties As SettingsPropertyCollection = FlexibleSettingsProviderTests.CreateTestProperties()

            Assert.IsFalse(File.Exists(FlexibleSettingsProvider.EffectiveConfigFilePath))

            Dim values As SettingsPropertyValueCollection = provider.GetPropertyValues(New SettingsContext(), properties)

            FlexibleSettingsProviderTests.AssertDefaultValues(values)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="FlexibleSettingsProvider.GetPropertyValues"/> returns the default values
        ''' when the configuration file is corrupt.
        ''' </summary>
        <TestMethod>
        <DataRow("this is not XML", DisplayName:="Not XML")>
        <DataRow("<settings><UseDarkTheme>True", DisplayName:="Truncated XML")>
        <DataRow("", DisplayName:="Empty file")>
        Public Sub GetPropertyValues_CorruptFile_ReturnsDefaultValues(fileContent As String)

            Dim configFilePath As String = FlexibleSettingsProviderTests.PrepareConfigFile(fileContent)
            Dim provider As New FlexibleSettingsProvider()

            Dim values As SettingsPropertyValueCollection =
                provider.GetPropertyValues(New SettingsContext(), FlexibleSettingsProviderTests.CreateTestProperties())

            Assert.IsTrue(File.Exists(configFilePath))
            FlexibleSettingsProviderTests.AssertDefaultValues(values)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="FlexibleSettingsProvider.GetPropertyValues"/> reads the values stored in the file,
        ''' and falls back to the default value for the properties that are not stored.
        ''' </summary>
        <TestMethod>
        Public Sub GetPropertyValues_PartialFile_ReadsStoredValuesAndDefaultsTheRest()

            FlexibleSettingsProviderTests.PrepareConfigFile(
                "<?xml version=""1.0"" encoding=""utf-8""?>" &
                "<settings>" &
                "  <UseDarkTheme>True</UseDarkTheme>" &
                "  <Presets><string>Stored|1</string></Presets>" &
                "</settings>")

            Dim provider As New FlexibleSettingsProvider()
            Dim values As SettingsPropertyValueCollection =
                provider.GetPropertyValues(New SettingsContext(), FlexibleSettingsProviderTests.CreateTestProperties())

            Assert.IsTrue(CBool(values("UseDarkTheme").PropertyValue))
            Assert.AreEqual(5, CInt(values("Counter").PropertyValue))
            Assert.AreEqual("Default text", CStr(values("Text").PropertyValue))

            Dim presets As StringCollection = DirectCast(values("Presets").PropertyValue, StringCollection)
            Assert.AreEqual(1, presets.Count)
            Assert.AreEqual("Stored|1", presets(0))
        End Sub

        ''' <summary>
        ''' Tests that <see cref="FlexibleSettingsProvider.GetPropertyValues"/> falls back to the default value
        ''' of a <see cref="StringCollection"/> property when its element has no <c>string</c> children.
        ''' </summary>
        <TestMethod>
        Public Sub GetPropertyValues_StringCollectionWithoutChildren_ReturnsDefaultValue()

            FlexibleSettingsProviderTests.PrepareConfigFile("<settings><Presets /></settings>")

            Dim provider As New FlexibleSettingsProvider()
            Dim values As SettingsPropertyValueCollection =
                provider.GetPropertyValues(New SettingsContext(), FlexibleSettingsProviderTests.CreateTestProperties())

            Dim presets As StringCollection = DirectCast(values("Presets").PropertyValue, StringCollection)
            CollectionAssert.AreEqual(FlexibleSettingsProviderTests.DefaultPresets, presets.Cast(Of String)().ToArray())
        End Sub

        ''' <summary>
        ''' Tests that <see cref="FlexibleSettingsProvider.GetPropertyValues"/> ignores unknown elements in the file.
        ''' </summary>
        <TestMethod>
        Public Sub GetPropertyValues_UnknownElements_AreIgnored()

            FlexibleSettingsProviderTests.PrepareConfigFile("<settings><Unknown>value</Unknown><Counter>42</Counter></settings>")

            Dim provider As New FlexibleSettingsProvider()
            Dim values As SettingsPropertyValueCollection =
                provider.GetPropertyValues(New SettingsContext(), FlexibleSettingsProviderTests.CreateTestProperties())

            Assert.AreEqual(4, values.Count)
            Assert.IsNull(values("Unknown"))
            Assert.AreEqual(42, CInt(values("Counter").PropertyValue))
        End Sub

#End Region

#Region " SetPropertyValues "

        ''' <summary>
        ''' Tests that values written by <see cref="FlexibleSettingsProvider.SetPropertyValues"/>
        ''' are read back by <see cref="FlexibleSettingsProvider.GetPropertyValues"/>.
        ''' </summary>
        <TestMethod>
        Public Sub SetPropertyValues_ThenGetPropertyValues_RoundTrips()

            Dim provider As New FlexibleSettingsProvider()
            Dim properties As SettingsPropertyCollection = FlexibleSettingsProviderTests.CreateTestProperties()

            Dim presets As New StringCollection()
            presets.AddRange({"First|1", "Second|0x2", "Third|76561198833246086"})

            Dim valuesToWrite As SettingsPropertyValueCollection = provider.GetPropertyValues(New SettingsContext(), properties)
            valuesToWrite("UseDarkTheme").PropertyValue = True
            valuesToWrite("Counter").PropertyValue = -123
            valuesToWrite("Text").PropertyValue = "Text with <XML> & special ""characters"" ñ"
            valuesToWrite("Presets").PropertyValue = presets

            provider.SetPropertyValues(New SettingsContext(), valuesToWrite)

            Dim readValues As SettingsPropertyValueCollection =
                New FlexibleSettingsProvider().GetPropertyValues(New SettingsContext(), FlexibleSettingsProviderTests.CreateTestProperties())

            Assert.IsTrue(CBool(readValues("UseDarkTheme").PropertyValue))
            Assert.AreEqual(-123, CInt(readValues("Counter").PropertyValue))
            Assert.AreEqual("Text with <XML> & special ""characters"" ñ", CStr(readValues("Text").PropertyValue))
            CollectionAssert.AreEqual(presets.Cast(Of String)().ToArray(),
                                      DirectCast(readValues("Presets").PropertyValue, StringCollection).Cast(Of String)().ToArray())
        End Sub

        ''' <summary>
        ''' Tests that <see cref="FlexibleSettingsProvider.SetPropertyValues"/> creates the settings directory
        ''' and writes the expected XML structure.
        ''' </summary>
        <TestMethod>
        Public Sub SetPropertyValues_WritesExpectedXmlStructure()

            Dim provider As New FlexibleSettingsProvider()
            Dim values As SettingsPropertyValueCollection =
                provider.GetPropertyValues(New SettingsContext(), FlexibleSettingsProviderTests.CreateTestProperties())
            values("UseDarkTheme").PropertyValue = True

            Assert.IsFalse(Directory.Exists(Path.Combine(Me.testDirectoryPath, "Settings")))

            provider.SetPropertyValues(New SettingsContext(), values)

            Dim configFilePath As String = FlexibleSettingsProvider.EffectiveConfigFilePath
            Assert.IsTrue(File.Exists(configFilePath))

            Dim doc As XDocument = XDocument.Load(configFilePath)
            Assert.AreEqual("settings", doc.Root.Name.LocalName)
            Assert.AreEqual("True", doc.Root.Element("UseDarkTheme").Value)
            Assert.AreEqual("5", doc.Root.Element("Counter").Value)
            Assert.AreEqual("Default text", doc.Root.Element("Text").Value)

            Dim presetElements As String() = doc.Root.Element("Presets").Elements("string").Select(Function(e As XElement) e.Value).ToArray()
            CollectionAssert.AreEqual(FlexibleSettingsProviderTests.DefaultPresets, presetElements)
        End Sub

        ''' <summary>
        ''' Tests that <see cref="FlexibleSettingsProvider.SetPropertyValues"/> writes an empty element for
        ''' a <see langword="Nothing"/> value, which is read back as an empty string.
        ''' </summary>
        <TestMethod>
        Public Sub SetPropertyValues_NullString_IsWrittenAsEmptyElement()

            Dim provider As New FlexibleSettingsProvider()
            Dim values As SettingsPropertyValueCollection =
                provider.GetPropertyValues(New SettingsContext(), FlexibleSettingsProviderTests.CreateTestProperties())
            values("Text").PropertyValue = Nothing

            provider.SetPropertyValues(New SettingsContext(), values)

            Dim doc As XDocument = XDocument.Load(FlexibleSettingsProvider.EffectiveConfigFilePath)
            Assert.IsTrue(doc.Root.Element("Text").IsEmpty)

            Dim readValues As SettingsPropertyValueCollection =
                provider.GetPropertyValues(New SettingsContext(), FlexibleSettingsProviderTests.CreateTestProperties())
            Assert.AreEqual(String.Empty, CStr(readValues("Text").PropertyValue))
        End Sub

        ''' <summary>
        ''' Tests that <see cref="FlexibleSettingsProvider.SetPropertyValues"/> overwrites an existing file
        ''' instead of appending to it.
        ''' </summary>
        <TestMethod>
        Public Sub SetPropertyValues_ExistingFile_IsOverwritten()

            Dim provider As New FlexibleSettingsProvider()

            For Each counter As Integer In {1, 2, 3}
                Dim values As SettingsPropertyValueCollection =
                    provider.GetPropertyValues(New SettingsContext(), FlexibleSettingsProviderTests.CreateTestProperties())
                values("Counter").PropertyValue = counter
                provider.SetPropertyValues(New SettingsContext(), values)
            Next

            Dim doc As XDocument = XDocument.Load(FlexibleSettingsProvider.EffectiveConfigFilePath)
            Assert.AreEqual(1, doc.Root.Elements("Counter").Count())
            Assert.AreEqual("3", doc.Root.Element("Counter").Value)
        End Sub

#End Region

#Region " Private Members "

        ''' <summary>
        ''' The default presets used by the test <see cref="StringCollection"/> property.
        ''' </summary>
        Private Shared ReadOnly DefaultPresets As String() = {"Default A|1", "Default B|2"}

        ''' <summary>
        ''' Creates a collection of settings properties that mimics the application settings.
        ''' </summary>
        '''
        ''' <returns>
        ''' The resulting <see cref="SettingsPropertyCollection"/>.
        ''' </returns>
        Private Shared Function CreateTestProperties() As SettingsPropertyCollection

            Dim defaultPresetsXml As String =
                "<?xml version=""1.0"" encoding=""utf-16""?>" &
                "<ArrayOfString xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">" &
                String.Concat(FlexibleSettingsProviderTests.DefaultPresets.Select(Function(p As String) $"<string>{p}</string>")) &
                "</ArrayOfString>"

            Dim properties As New SettingsPropertyCollection From {
                FlexibleSettingsProviderTests.CreateProperty("UseDarkTheme", GetType(Boolean), "False", SettingsSerializeAs.String),
                FlexibleSettingsProviderTests.CreateProperty("Counter", GetType(Integer), "5", SettingsSerializeAs.String),
                FlexibleSettingsProviderTests.CreateProperty("Text", GetType(String), "Default text", SettingsSerializeAs.String),
                FlexibleSettingsProviderTests.CreateProperty("Presets", GetType(StringCollection), defaultPresetsXml, SettingsSerializeAs.Xml)
            }

            Return properties
        End Function

        ''' <summary>
        ''' Creates a single settings property.
        ''' </summary>
        '''
        ''' <param name="name">The property name.</param>
        ''' <param name="propertyType">The property type.</param>
        ''' <param name="defaultValue">The serialized default value.</param>
        ''' <param name="serializeAs">The serialization mode.</param>
        '''
        ''' <returns>
        ''' The resulting <see cref="SettingsProperty"/>.
        ''' </returns>
        Private Shared Function CreateProperty(name As String, propertyType As Type,
                                               defaultValue As String, serializeAs As SettingsSerializeAs) As SettingsProperty

            Return New SettingsProperty(name) With {
                .PropertyType = propertyType,
                .DefaultValue = defaultValue,
                .SerializeAs = serializeAs,
                .IsReadOnly = False
            }
        End Function

        ''' <summary>
        ''' Writes the specified content to the effective configuration file, creating its directory if needed.
        ''' </summary>
        '''
        ''' <param name="content">The file content.</param>
        '''
        ''' <returns>
        ''' The full path of the configuration file.
        ''' </returns>
        Private Shared Function PrepareConfigFile(content As String) As String

            Dim configFilePath As String = FlexibleSettingsProvider.EffectiveConfigFilePath
            Directory.CreateDirectory(Path.GetDirectoryName(configFilePath))
            File.WriteAllText(configFilePath, content)
            Return configFilePath
        End Function

        ''' <summary>
        ''' Asserts that the specified values are the default values declared by <see cref="CreateTestProperties"/>.
        ''' </summary>
        '''
        ''' <param name="values">The values to check.</param>
        Private Shared Sub AssertDefaultValues(values As SettingsPropertyValueCollection)

            Assert.AreEqual(4, values.Count)
            Assert.IsFalse(CBool(values("UseDarkTheme").PropertyValue))
            Assert.AreEqual(5, CInt(values("Counter").PropertyValue))
            Assert.AreEqual("Default text", CStr(values("Text").PropertyValue))

            Dim presets As StringCollection = DirectCast(values("Presets").PropertyValue, StringCollection)
            CollectionAssert.AreEqual(FlexibleSettingsProviderTests.DefaultPresets, presets.Cast(Of String)().ToArray())
        End Sub

#End Region

    End Class

End Namespace

#End Region
