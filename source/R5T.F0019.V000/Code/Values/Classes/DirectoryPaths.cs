using System;


namespace R5T.F0019.V000
{
    public class DirectoryPaths : IDirectoryPaths
    {
        #region Infrastructure

        public static DirectoryPaths Instance { get; } = new();

        private DirectoryPaths()
        {
        }

        #endregion
    }
}
