using System;

using R5T.T0131;


namespace R5T.F0019.V000
{
    [ValuesMarker]
    public interface IDirectoryPaths : IValuesMarker
    {
        public string FileInRepositoryDirectory => @"C:\Code\DEV\Git\GitHub\SafetyCone\R5T.S0027\source\R5T.S0027.sln";
        public string NotARepositoryDirectory => @"C:\Temp\";
        public string RepositoryDirectory => @"C:\Code\DEV\Git\GitHub\SafetyCone\R5T.S0027\";
        public string RepositoryGitDirectory => @"C:\Code\DEV\Git\GitHub\SafetyCone\R5T.S0027\.git\";
    }
}
