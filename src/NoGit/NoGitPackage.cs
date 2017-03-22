﻿namespace RendleLabs.NoGit
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;

    /// <summary>
    ///     This is the class that implements the package exposed by this assembly.
    ///     The minimum requirement for a class to be considered a valid package for Visual Studio
    ///     is to implement the IVsPackage interface and register itself with the shell.
    ///     This package uses the helper classes defined inside the Managed Package Framework (MPF)
    ///     to do it: it derives from the Package class that provides the implementation of the
    ///     IVsPackage interface and uses the registration attributes defined in the framework to
    ///     register itself and its components with the shell.
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
        private uint _cookie;
        private IVsSolution _vsSolution;

        /// <summary>
        ///     Default constructor of the package.
        ///     Inside this method you can place any initialization code that does not require
        ///     any Visual Studio service because at this point the package object is created but
        ///     not sited yet inside Visual Studio environment. The place to do all the other
        ///     initialization is the Initialize method.
        /// </summary>
        public NoGitPackage()
        {
            Debug.WriteLine("Entering constructor for: {0}", ToString());
        }

        #region Package Members

        /// <summary>
        ///     Initialization of the package; this method is called right after the package is sited, so this is the place
        ///     where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine("Entering Initialize() of: {0}", ToString());
            base.Initialize();

            _vsSolution = (IVsSolution) GetService(typeof (SVsSolution));
            _vsSolution.AdviseSolutionEvents(this, out _cookie);
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation

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

        protected override void Dispose(bool disposing)
        {
            if (disposing && _cookie != 0)
            {
                _vsSolution.UnadviseSolutionEvents(_cookie);
            }
            base.Dispose(disposing);
        }

        private void SetScciProviderInactive()
        {
            var getProvider = GetService(typeof (IVsRegisterScciProvider)) as IVsGetScciProviderInterface;
            if (getProvider != null)
            {
                IVsSccProvider provider = getProvider.GetProvider();
                if (provider != null)
                {
                    provider.SetInactive();
                }
            }
        }
    }
}
