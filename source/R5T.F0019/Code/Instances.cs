using System;

using R5T.L0001.Z000;
using R5T.T0041;
using R5T.Z0005;


namespace R5T.F0019
{
    public static class Instances
    {
        public static IDirectoryNames DirectoryNames { get; } = Z0005.DirectoryNames.Instance;
        public static IPathOperator PathOperator { get; } = T0041.PathOperator.Instance;
        public static IValues Values_ForLibGit2Sharp { get; } = L0001.Z000.Values.Instance;
    }
}
