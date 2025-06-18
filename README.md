# PackageDependency

Trying to encapsulate [TryCreatePackageDependency](https://learn.microsoft.com/en-us/windows/win32/api/appmodel/nf-appmodel-trycreatepackagedependency) for use from PowerShell.

THIS DOESN'T WORK. **FAILS** with `E_INVALIDARG`:

```
Creating package dependency for:
Package Family Name: Microsoft.WindowsTerminal_8wekyb3d8bbwe
Package Version:     1.22.11141.0
Registry Key:        HKEY_CURRENT_USER\Software\Microsoft\UfScripts\Microsoft.WindowsTerminal_8wekyb3d8bbwe_1.22.11141.0

Registry Key Created: HKEY_CURRENT_USER\Software\Microsoft\UfScripts\Microsoft.WindowsTerminal_8wekyb3d8bbwe_1.22.11141.0
Unhandled exception. System.ComponentModel.Win32Exception (0x80070057): The parameter is incorrect.
   at PInvoke.TryCreatePackageDependency(String packageFamilyName, SimpleVersion minVersion, PackageDependencyProcessorArchitectures packageDependencyProcessorArchitectures, PackageDependencyLifetimeKind lifetimeKind, String lifetimeArtifact, CreatePackageDependencyOptions options) in C:\Users\ben\Projects\PackageDependency\PInvoke.cs:line 151
   at Program.<Main>$(String[] args) in C:\Users\ben\Projects\PackageDependency\Program.cs:line 24
```

## Loading from PWSH

```pwsh
add-type -Path "C:\Users\ben\Projects\PackageDependency\PInvoke.cs" -CompilerOptions '-unsafe'
```

https://github.com/PowerShell/PowerShell/issues/13594