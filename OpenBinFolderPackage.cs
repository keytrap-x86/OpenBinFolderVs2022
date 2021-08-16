
using EnvDTE;

using EnvDTE80;

using Microsoft;
using Microsoft.VisualStudio.Shell;

using System;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

namespace OpenBinFolderVs2022
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the
    /// IVsPackage interface and uses the registration attributes defined in the framework to
    /// register itself and its components with the shell. These attributes tell the pkgdef creation
    /// utility what data to put into .pkgdef file.
    /// </para>
    /// <para>
    /// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
    /// </para>
    /// </remarks>
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(PackageGuids.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class OpenBinFolderPackage : AsyncPackage
    {
        /// <summary>
        /// OpenBinFolderVs2022Package GUID string.
        /// </summary>


        public static DTE2 _dte;

        #region Package Members

        protected async override System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            _dte = await GetServiceAsync(typeof(DTE)) as DTE2;

            Assumes.Present(_dte);

            if (await GetServiceAsync(typeof(IMenuCommandService)) is OleMenuCommandService mcs)
            {
                CommandID menuCommandID = new CommandID(PackageGuids.OpenBinFolderCommandSetGuid, PackageIds.CmdOpenBinFolder);
                OleMenuCommand menuItem = new OleMenuCommand(OpenBinFolderWithFileExplorer, menuCommandID);
                mcs.AddCommand(menuItem);
            }

        }

        #endregion


        private void OpenBinFolderWithFileExplorer(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            // Find active projects
            var activeProjects = (Array)_dte.ActiveSolutionProjects;

            foreach (var (currentProjectPath, currentProjectBinPath) in
            from Project activeProject in activeProjects
            let currentProjectPath = Path.GetDirectoryName(activeProject.FullName)
            let currentProjectOutputPath = activeProject.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value.ToString()
            let currentProjectBinPath = Path.Combine(currentProjectPath, currentProjectOutputPath)
            select (currentProjectPath, currentProjectBinPath))
            {
                if (Directory.Exists(currentProjectBinPath))
                    System.Diagnostics.Process.Start(currentProjectBinPath);
                else
                    System.Diagnostics.Process.Start(currentProjectPath);
            }
        }
    }
}
