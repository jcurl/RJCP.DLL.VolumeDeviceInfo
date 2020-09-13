// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Minor Code Smell", "S101:Types should be named in PascalCase", Justification = "P/Invoke code", Scope = "namespaceanddescendants", Target = "N:VolumeTest.Native.Win32")]
[assembly: SuppressMessage("Major Code Smell", "S1144:Unused private types or members should be removed", Justification = "P/Invoke Code", Scope = "namespaceanddescendants", Target = "N:VolumeTest.Native.Win32")]
[assembly: SuppressMessage("Minor Code Smell", "S2344:Enumeration type names should not have \"Flags\" or \"Enum\" suffixes", Justification = "P/Invoke code", Scope = "namespaceanddescendants", Target = "N:VolumeTest.Native.Win32")]
[assembly: SuppressMessage("Major Code Smell", "S4016:Enumeration members should not be named \"Reserved\"", Justification = "P/Invoke code", Scope = "namespaceanddescendants", Target = "N:VolumeTest.Native.Win32")]
[assembly: SuppressMessage("Major Code Smell", "S4070:Non-flags enums should not be marked with \"FlagsAttribute\"", Justification = "P/Invoke code", Scope = "namespaceanddescendants", Target = "N:VolumeTest.Native.Win32")]
