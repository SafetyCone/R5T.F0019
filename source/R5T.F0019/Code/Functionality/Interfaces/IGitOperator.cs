using System;

using LibGit2Sharp;

using R5T.Magyar;

using R5T.T0132;

using Glossary = R5T.Y0004.Glossary;


namespace R5T.F0019
{
    [FunctionalityMarker]
    public interface IGitOperator : IFunctionalityMarker
    {
        /// <summary>
        /// Returns the <inheritdoc cref="Glossary.ForDirectories.RepositoryGitDirectory" path="/name"/> path if the provided path is part of a repository, or the <see cref="L0001.Z000.IValues.RepositoryDiscoveryNotFoundResult"/> if not.
        /// </summary>
        /// <param name="path">A directory or file path.</param>
        public string DiscoverRepositoryGitDirectory(string path)
        {
            var repositoryGitDirectoryPathOrNotFound = Repository.Discover(path);
            return repositoryGitDirectoryPathOrNotFound;
        }

        /// <inheritdoc cref="HasRepository_GitDirectory(string)"/>
        public string GetRepository_GitDirectory(string path)
        {
            var wasFound = this.HasRepository_GitDirectory(path);
            if(!wasFound)
            {
                throw new Exception($"Not git repository was found for path:\n{path}");
            }

            return wasFound.Result;
        }

        /// <inheritdoc cref="HasRepository(string)"/>
        public string GetRepository(string path)
        {
            var wasFound = this.HasRepository(path);
            if (!wasFound)
            {
                throw new Exception($"Not git repository was found for path:\n{path}");
            }

            return wasFound.Result;
        }

        /// <summary>
        /// Returns the <inheritdoc cref="Glossary.ForDirectories.RepositoryGitDirectory" path="/name"/> path.
        /// </summary>
        public WasFound<string> HasRepository_GitDirectory(string path)
        {
            var repositoryGitDirectoryPathOrNotFound = this.DiscoverRepositoryGitDirectory(path);

            var exists = this.RepositoryWasDiscovered(repositoryGitDirectoryPathOrNotFound);

            var output = WasFound.From(
                exists,
                repositoryGitDirectoryPathOrNotFound);

            return output;
        }

        /// <summary>
        /// Returns the <inheritdoc cref="Glossary.ForDirectories.RepositoryDirectory" path="/name"/> path.
        /// </summary>
        public WasFound<string> HasRepository(string path)
        {
            var wasFound_GitDirectory = this.HasRepository_GitDirectory(path);

            var output = wasFound_GitDirectory.Convert(
                gitDirectoryPath => Instances.PathOperator.GetParentDirectoryPath(gitDirectoryPath));

            return output;
        }

        public WasFound<string> IsInRepository(string path)
        {
            var output = this.HasRepository(path);
            return output;
        }

        public bool IsGitHiddenDirectory(string directoryName)
        {
            var isGitDirectory = Instances.DirectoryNames.GitHiddenDirectory == directoryName;
            return isGitDirectory;
        }

        public bool IsRepository(string directoryPath)
        {
            var output = Repository.IsValid(directoryPath);
            return output;
        }

        /// <summary>
        /// Returns whether the directory path is the path of a hidden <inheritdoc cref="Glossary.ForDirectories.RepositoryGitDirectory" path="/name"/>.
        /// </summary>
        public bool IsRepositoryGitDirectory(string directoryPath)
        {
            var isRepository = this.IsRepository(directoryPath);

            // If the directory path is not a repository at all, return false.
            if(!isRepository)
            {
                return false;
            }

            var directoryName = Instances.PathOperator.GetDirectoryNameOfDirectoryPath(directoryPath);

            var isGitDirectory = this.IsGitHiddenDirectory(directoryName);

            // If the directory path is not a git directory, return false.
            if (!isGitDirectory)
            {
                return false;
            }

            // Else
            return true;
        }

        /// <summary>
        /// Returns whether the directory path is the path of the <inheritdoc cref="Glossary.ForDirectories.RepositoryDirectory" path="/name"/> containing a hidden <inheritdoc cref="Glossary.ForDirectories.RepositoryGitDirectory" path="/name"/>.
        /// </summary>
        public bool IsRepositoryDirectory(string directoryPath)
        {
            var isRepository = this.IsRepository(directoryPath);

            // If the directory path is not a repository at all, return false.
            if (!isRepository)
            {
                return false;
            }

            var directoryName = Instances.PathOperator.GetDirectoryNameOfDirectoryPath(directoryPath);

            var isGitDirectory = this.IsGitHiddenDirectory(directoryName);

            // If the directory path *is* not a git directory, return false.
            if (isGitDirectory)
            {
                return false;
            }

            // Else
            return true;
        }

        /// <summary>
        /// Evaluates the result of a <see cref="Repository.Discover(string)"/> call to determine whether a repository was discovered.
        /// </summary>
        public bool RepositoryWasDiscovered(string repositoryDiscoveryResult)
        {
            var wasNotDiscovered = Instances.Values_ForLibGit2Sharp.RepositoryDiscoveryNotFoundResult == repositoryDiscoveryResult;

            var wasDiscovered = !wasNotDiscovered;
            return wasDiscovered;
        }
    }
}
