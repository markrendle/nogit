namespace Zudio.NoGit
{
    using System;
    using System.Runtime.InteropServices;
    using Microsoft.VisualStudio.Shell.Interop;

    internal static class VsGetScciProviderExtensions
    {
        public static IVsSccProvider GetProvider(this IVsGetScciProviderInterface getScciProvider)
        {
            IVsSccProvider provider = null;
            IntPtr pUnk = IntPtr.Zero;
            try
            {
                pUnk = getScciProvider.GetInterface(typeof (IVsSccProvider).GUID);
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