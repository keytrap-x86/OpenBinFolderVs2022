using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenBinFolderVs2022
{
    using System;

    /// <summary>
    ///     Helper class that exposes all GUIDs used across VS Package.
    /// </summary>
    internal sealed partial class PackageGuids
    {
        public const string PackageGuidString = "645d738e-fdcc-4a60-826c-421b06cf1d71";
        public static Guid OpenBinFolderPackageGuid = new Guid(PackageGuidString);

        public const string PackageCommandSetGuidString = "9a55a2b4-3e29-4359-882b-fa5f51c09300";
        public static Guid OpenBinFolderCommandSetGuid = new Guid(PackageCommandSetGuidString);
    }

    /// <summary>
    ///     Helper class that encapsulates all CommandIDs uses across VS Package.
    /// </summary>
    internal sealed partial class PackageIds
    {
        public const int CmdOpenBinFolder = 0xACAB;
    }
}
