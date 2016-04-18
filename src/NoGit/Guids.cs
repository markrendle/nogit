// Guids.cs
// MUST match guids.h
using System;

namespace RendleLabs.NoGit
{
    static class GuidList
    {
        public const string guidNoGitPkgString = "c1d06f67-e7cb-4df1-b93d-2ee8e1b5193b";
        public const string guidNoGitCmdSetString = "f0c6d24f-7ffe-458d-9975-46544ed8d9cc";

        public static readonly Guid guidNoGitCmdSet = new Guid(guidNoGitCmdSetString);
    };
}