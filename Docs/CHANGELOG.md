# Steam ID Converter Change Log 📋

## v1.3.3 *(current)* 🆕
#### 🛠️ Fixes:
- The highest possible SteamID32 value (`4294967295` / `0xFFFFFFFF`) was rejected as an invalid Steam ID, although its SteamID64 equivalent (`76561202255233023`) was accepted and converted to that same value.
- A SteamID64 that corresponds to the account number zero (`76561197960265728` / `0x110000100000000`) was accepted and displayed a SteamID32 of `0`, while entering `0` directly was rejected. Both are now consistently treated as invalid.

#### 🌟 Improvements:
- **Source-code:** The Steam ID parsing and preset loading logic has been moved out of the main form into reusable helper classes.
- **Source-code:** Added a unit test project (MSTest) covering Steam ID conversion, input parsing, preset parsing and the settings provider.

## v1.3.2 🔄
#### 🌟 Improvements:
- Hopefully fixed any remaining visual glitches related to control border rendering when the visual theme is set to dark.

## v1.3.1 🔄
#### 🌟 Improvements:
- Renamed the preset "Mr. Goldberg" to "DenuvOwO Repacks" for clarity and precision.
- The "Custom ID" textbox now automatically receives focus when the preset is set to "None".
- Reduced visual glitches related to control border rendering when the visual theme is set to dark.

## v1.3.0 🔄
#### 🌟 Improvements:
- **User Interface:** Repositioned controls, improved text descriptions, and fixed color inconsistencies.
- **Source-code:** Various refactors and micro-optimizations.
- Added the "Mr. Goldberg" preset (_A Steam-ID that appears in various repacks_).
- The program can now load user-defined presets from a `user.config` file located in the program folder.
   (note: If the file does not exist, it will be automatically generated when closing the program, populated with the default presets.

## v1.2.2 🔄
#### 🛠️ Fixes:
- Removed default Steam ID for DARKSiDERS piracy group, as it was not really a default Steam ID.

## v1.2.1 🔄
#### 🌟 Improvements:
- Added default Steam ID for DARKSiDERS piracy group. Note: It may be the same for all or some of the SKIDROW cracks.
- Removed useless default Steam ID "12345" and "12345678".

## v1.2 🔄
#### 🌟 Improvements:
- Added default Steam ID for TENOKE piracy group.

## v1.1 🔄
#### 🚀 New Features:
- Added a set of default Steam Id values, including those from CODEX and RELOADED piracy groups.

## v1.0 🔄
Initial Release.