using System.Runtime.InteropServices;

public class SimpleVersion
{
    public ushort Major { get; set; }
    public ushort Minor { get; set; }
    public ushort Build { get; set; }
    public ushort Revision { get; set; }
}

public class PInvoke
{
    // Extracted from CsWin32.
    internal partial struct PACKAGE_VERSION
    {
        internal _Anonymous_e__Union Anonymous;

        [StructLayout(LayoutKind.Explicit, Pack = 4)]
        [global::System.CodeDom.Compiler.GeneratedCode("Microsoft.Windows.CsWin32", "0.3.183+73e6125f79.RR")]
        internal partial struct _Anonymous_e__Union
        {
            [FieldOffset(0)]
            internal ulong Version;

            [FieldOffset(0)]
            internal _Anonymous_e__Struct Anonymous;

            [global::System.CodeDom.Compiler.GeneratedCode("Microsoft.Windows.CsWin32", "0.3.183+73e6125f79.RR")]
            internal partial struct _Anonymous_e__Struct
            {
                internal ushort Revision;

                internal ushort Build;

                internal ushort Minor;

                internal ushort Major;
            }
        }
    }

    public enum PackageDependencyProcessorArchitectures
    {
        /// <summary>No processor architecture is specified.</summary>
        PackageDependencyProcessorArchitectures_None = 0x00000000,
        /// <summary>Specifies the neutral architecture.</summary>
        PackageDependencyProcessorArchitectures_Neutral = 0x00000001,
        /// <summary>Specifies the x86 architecture.</summary>
        PackageDependencyProcessorArchitectures_X86 = 0x00000002,
        /// <summary>Specifies the x64 architecture.</summary>
        PackageDependencyProcessorArchitectures_X64 = 0x00000004,
        /// <summary>Specifies the ARM architecture.</summary>
        PackageDependencyProcessorArchitectures_Arm = 0x00000008,
        /// <summary>Specifies the ARM64 architecture.</summary>
        PackageDependencyProcessorArchitectures_Arm64 = 0x00000010,
        /// <summary>Specifies the x86/A64 architecture.</summary>
        PackageDependencyProcessorArchitectures_X86A64 = 0x00000020,
    }

    public enum PackageDependencyLifetimeKind
    {
        /// <summary>The current process is the lifetime artifact. The package dependency is implicitly deleted when the process terminates.</summary>
        PackageDependencyLifetimeKind_Process = 0,
        /// <summary>The lifetime artifact is an absolute filename or path. The package dependency is implicitly deleted when this is deleted.</summary>
        PackageDependencyLifetimeKind_FilePath = 1,
        /// <summary>The lifetime artifact is a registry key in the format *root*\\*subkey*, where *root* is one of the following: HKEY_LOCAL_MACHINE, HKEY_CURRENT_USER, HKEY_CLASSES_ROOT, or HKEY_USERS. The package dependency is implicitly deleted when this is deleted.</summary>
        PackageDependencyLifetimeKind_RegistryKey = 2,
    }

    public enum CreatePackageDependencyOptions
    {
        /// <summary>No options are applied.</summary>
        CreatePackageDependencyOptions_None = 0x00000000,
        /// <summary>Disables dependency resolution when pinning a package dependency. This is useful for installers running as user contexts other than the target user (for example, installers running as LocalSystem).</summary>
        CreatePackageDependencyOptions_DoNotVerifyDependencyResolution = 0x00000001,
        /// <summary>Defines the package dependency for the system, accessible to all users (by default, the package dependency is defined for a specific user). This option requires the caller has administrative privileges.</summary>
        CreatePackageDependencyOptions_ScopeIsSystem = 0x00000002,
    }

    [DllImport("KERNELBASE.dll", ExactSpelling = true), DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
    internal static extern unsafe int TryCreatePackageDependency(
        nint user,
        char* packageFamilyName,
        PACKAGE_VERSION minVersion,
        PackageDependencyProcessorArchitectures packageDependencyProcessorArchitectures,
        PackageDependencyLifetimeKind lifetimeKind,
        char* lifetimeArtifact,
        CreatePackageDependencyOptions options,
        char** packageDependencyId);

    internal static PACKAGE_VERSION CreatePackageVersion(SimpleVersion version)
    {
        return new PACKAGE_VERSION
        {
            Anonymous = new PACKAGE_VERSION._Anonymous_e__Union
            {
                Anonymous = new PACKAGE_VERSION._Anonymous_e__Union._Anonymous_e__Struct
                {
                    Major = version.Major,
                    Minor = version.Minor,
                    Build = version.Build,
                    Revision = version.Revision
                }
            }
        };
    }

    public static string TryCreatePackageDependency(
        string packageFamilyName,
        SimpleVersion minVersion,
        PackageDependencyProcessorArchitectures packageDependencyProcessorArchitectures,
        PackageDependencyLifetimeKind lifetimeKind,
        string lifetimeArtifact,
        CreatePackageDependencyOptions options)
    {
        var packageVersion = CreatePackageVersion(minVersion);

        unsafe
        {
            fixed (char* pPackageFamilyName = packageFamilyName)
            fixed (char* pLifetimeArtifact = lifetimeArtifact)
            {
                char* pPackageDependencyId;
                int result = TryCreatePackageDependency(
                    nint.Zero, // Use current user
                    pPackageFamilyName,
                    packageVersion,
                    packageDependencyProcessorArchitectures,
                    lifetimeKind,
                    pLifetimeArtifact,
                    options,
                    &pPackageDependencyId);

                if (result < 0)
                {
                    throw new System.ComponentModel.Win32Exception(result);
                }

                return new string(pPackageDependencyId);
            }
        }
    }
}
