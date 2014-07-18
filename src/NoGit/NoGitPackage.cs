using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using Microsoft.Win32;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;

namespace Zudio.NoGit
{
    using System.ComponentModel.Composition;
    using System.Text;
    using EnvDTE;
    using Microsoft.VisualStudio.Settings;
    using Microsoft.VisualStudio.Shell.Settings;
    using IServiceProvider = System.IServiceProvider;

    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    [ProvideAutoLoad(UIContextGuids80.NoSolution)]
    [Guid(GuidList.guidNoGitPkgString)]
    public sealed class NoGitPackage : Package, IVsSolutionEvents
    {
        private IVsSolution _vsSolution;
        private uint _cookie;
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public NoGitPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }



        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation
        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            _vsSolution = (IVsSolution)GetService(typeof (SVsSolution));
            _vsSolution.AdviseSolutionEvents(this, out _cookie);
        }
        #endregion

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _cookie != 0)
            {
                _vsSolution.UnadviseSolutionEvents(_cookie);
            }
            base.Dispose(disposing);
        }

        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            SetScciProviderInactive();
            return VSConstants.S_OK;
        }

        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            SetScciProviderInactive();
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            SetScciProviderInactive();
            return VSConstants.S_OK;
        }

        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            SetScciProviderInactive();
            return VSConstants.S_OK;
        }

        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            SetScciProviderInactive();
            return VSConstants.S_OK;
        }

        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            SetScciProviderInactive();
            return VSConstants.S_OK;
        }

        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            SetScciProviderInactive();
            return VSConstants.S_OK;
        }

        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            SetScciProviderInactive();
            return VSConstants.S_OK;
        }

        public int OnBeforeCloseSolution(object pUnkReserved)
        {
            SetScciProviderInactive();
            return VSConstants.S_OK;
        }

        public int OnAfterCloseSolution(object pUnkReserved)
        {
            SetScciProviderInactive();
            return VSConstants.S_OK;
        }

        private void SetScciProviderInactive()
        {
            var getProvider = GetService( typeof( IVsRegisterScciProvider ) ) as IVsGetScciProviderInterface;
            if ( getProvider != null )
            {
                var provider = getProvider.GetProvider();
                if ( provider != null )
                {
                    provider.SetInactive();
                }
            }
        }
    }

    internal static class VsGetScciProviderExtensions
    {
        public static IVsSccProvider GetProvider(this IVsGetScciProviderInterface getScciProvider)
        {
            IVsSccProvider provider = null;
            IntPtr pUnk = IntPtr.Zero;
            try
            {
                pUnk = getScciProvider.GetInterface(typeof(IVsSccProvider).GUID);
                if (pUnk != IntPtr.Zero)
                    provider = Marshal.GetObjectForIUnknown(pUnk) as IVsSccProvider;
            }
            finally
            {
                if (pUnk != IntPtr.Zero)
                {
                    Marshal.Release(pUnk);
                }
            }
            return provider;
        }

        private static IntPtr GetInterface(this IVsGetScciProviderInterface getScciProvider, Guid guid)
        {
            IntPtr pvv;
            getScciProvider.GetSourceControlProviderInterface(ref guid, out pvv);
            return pvv;
        }
    }
}
