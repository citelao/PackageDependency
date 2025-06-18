using static PInvoke;

// var packageFamilyName = "MicrosoftWindows.UfPsPackage_kzq9xh7cr2gpm";
var packageFamilyName = "Microsoft.WindowsTerminal_8wekyb3d8bbwe";
var packageVersion = new SimpleVersion(1, 22, 11141, 0);
var registryKey = $"Software\\Microsoft\\UfScripts\\{packageFamilyName}_{packageVersion}";

Console.WriteLine($"Creating package dependency for:");
Console.WriteLine($"Package Family Name: {packageFamilyName}");
Console.WriteLine($"Package Version:     {packageVersion}");
Console.WriteLine($"Registry Key:        HKEY_CURRENT_USER\\{registryKey}");
Console.WriteLine();

// Create the registry key

#pragma warning disable CA1416
var registryKeyResult = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
    registryKey,
    Microsoft.Win32.RegistryKeyPermissionCheck.ReadWriteSubTree
);
Console.WriteLine($"Registry Key Created: {registryKeyResult.Name}");
#pragma warning restore CA1416

var result = TryCreatePackageDependency(
    packageFamilyName,
    packageVersion,
    PackageDependencyProcessorArchitectures.PackageDependencyProcessorArchitectures_Neutral,
    PackageDependencyLifetimeKind.PackageDependencyLifetimeKind_RegistryKey,
    $"HKEY_CURRENT_USER\\{registryKey}",
    CreatePackageDependencyOptions.CreatePackageDependencyOptions_None
);

Console.WriteLine($"=> Package Dependency ID: {result}");