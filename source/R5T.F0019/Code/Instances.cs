using System;

using R5T.F0002;
using R5T.L0001.Z000;


namespace R5T.F0019
{
    public static class Instances
    {
        public static Z0005.IDirectoryNames DirectoryNames { get; } = Z0005.DirectoryNames.Instance;
        public static IGitOperator GitOperator { get; } = F0019.GitOperator.Instance;
        public static IPathOperator PathOperator { get; } = F0002.PathOperator.Instance;
        public static IRemoteOperator RemoteOperator { get; } = F0019.RemoteOperator.Instance;
        public static IRemoteRepositoryNames RemoteRepositoryNames { get; } = F0019.RemoteRepositoryNames.Instance;
        public static IRepositoryOperator RepositoryOperator { get; } = F0019.RepositoryOperator.Instance;
        public static IValues Values_ForLibGit2Sharp { get; } = L0001.Z000.Values.Instance;
    }
}
