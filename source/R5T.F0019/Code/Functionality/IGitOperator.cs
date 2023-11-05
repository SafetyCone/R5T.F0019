using System;
using System.Collections.Generic;
using System.Linq;

using LibGit2Sharp;
using LibGit2Sharp.Handlers;

using R5T.N0000;

using R5T.T0046;
using R5T.T0132;
using R5T.T0144;

using Glossary = R5T.Y0004.Glossary;


namespace R5T.F0019
{
    [FunctionalityMarker]
    public interface IGitOperator : IFunctionalityMarker
    {
        /// <summary>
        /// Non-idempotently clones a remote Git repository to a local directory path.
        /// An error will be thrown if the local directory is not empty, if it exists.
        /// </summary>
        /// <returns>
        /// The local repository directory path.
        /// </returns>
        public string Clone_NonIdempotent(
            string sourceUrl,
            string localRepositoryDirectoryPath,
            Authentication authentication)
        {
            var options = new CloneOptions
            {
                CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials()
                    {
                        Username = authentication.Username,
                        Password = authentication.Password,
                    }),
            };

            // Safety check that local repository directory path is empty (if exists), it performed here.
            // LibGit2Sharp.NameConflictException, '{directory path}' exists and is not an empty directory
            var repositoryDirectoryPath = Repository.Clone(sourceUrl, localRepositoryDirectoryPath, options);
            return repositoryDirectoryPath;
        }

        public void Commit(
            string localRepositoryDirectoryPath,
            string commitMessage,
            Author author)
        {
            var authorSignature = new Signature(author.Name, author.EmailAddress, DateTime.Now);
            var committerSignature = authorSignature;

            using var repository = new Repository(localRepositoryDirectoryPath);

            var anyToCommit = repository.Index.Where(x => x.StageLevel == StageLevel.Staged).Any();
            if (anyToCommit)
            {
                repository.Commit(
                    commitMessage,
                    authorSignature,
                    committerSignature);
            }
        }

        /// <summary>
        /// Returns the <inheritdoc cref="Glossary.ForDirectories.RepositoryGitDirectory" path="/name"/> path if the provided path is part of a repository, or the <see cref="L0001.Z000.IValues.RepositoryDiscoverNotFoundResult"/> if not.
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
        /// <para>A quality-of-life overload for <see cref="GetRepository(string)"/>.</para>
        /// <inheritdoc cref="GetRepository(string)"/>
        /// </summary>
        public string GetRepositoryDirectoryPath(string path)
        {
            var repositoryDirectoryPath = this.GetRepository(path);
            return repositoryDirectoryPath;
        }

        public string GetRepositoryRemoteUrl(string path)
        {
            var repositoryDirectory = this.GetRepository(path);

            using var repository = new Repository(repositoryDirectory);

            var remoteUrl = Instances.RepositoryOperator.GetRemoteUrl(repository);
            return remoteUrl;
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

        // Prior work in R5T.D0038.L0001.
        // No logging or result infrastructure to allow this method to just be simple functionality.
        public bool HasUnpushedLocalChanges(string repositoryDirectoryPath)
        {
            using var repository = new Repository(repositoryDirectoryPath);

            // Assume no unpushed changes.
            var hasUnPushedChanges = false;

            // Are there any untracked files? (Other than ignored files.)
            // => I think the below takes care of this.

            // Are there any unstaged or uncommitted changes?
            var treeChanges = repository.Diff.Compare<TreeChanges>(
                repository.Head.Tip.Tree,
                DiffTargets.Index | DiffTargets.WorkingDirectory);

            hasUnPushedChanges = treeChanges.Count > 0;
            if (hasUnPushedChanges)
            {
                return hasUnPushedChanges;
            }

            // Get the current branch.
            var currentBranch = repository.Head;

            // Is the current branch untracked? This indicates that it has not been pushed to the remote!
            var isUntracked = !currentBranch.IsTracking;

            hasUnPushedChanges = isUntracked;
            if (hasUnPushedChanges)
            {
                return hasUnPushedChanges;
            }

            // Is the current branch ahead its remote tracking branch?
            var currentBranchLocalIsAheadOfRemote = currentBranch.TrackingDetails.AheadBy > 0;

            hasUnPushedChanges = currentBranchLocalIsAheadOfRemote;
            if (hasUnPushedChanges)
            {
                return hasUnPushedChanges;
            }

            // Finally, return the originally assumed value, that there are no unpushed changes.
            return hasUnPushedChanges;
        }

        /// <summary>
        /// Returns the <inheritdoc cref="Glossary.ForDirectories.RepositoryDirectory" path="/name"/> path given a file or directory path from within the repository.
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

        public string[] ListAllUnstagedPaths(string localRepositoryDirectoryPath)
        {
            using var repository = new Repository(localRepositoryDirectoryPath);

            var unstagedPaths = repository.Diff.Compare<TreeChanges>(repository.Head.Tip.Tree, DiffTargets.WorkingDirectory)
                  .Select(xChange => xChange.Path)
                  .ToArray();

            return unstagedPaths;
        }

        /// <summary>
        /// Push the HEAD branch.
        /// </summary>
        public void Push(
            string localRepositoryDirectoryPath,
            Authentication authentication)
        {
            using var repository = new Repository(localRepositoryDirectoryPath);

            var pushOptions = new PushOptions
            {
                CredentialsProvider = new CredentialsHandler((url, usernameFromUrl, types) =>
                    new UsernamePasswordCredentials()
                    {
                        Username = authentication.Username,
                        Password = authentication.Password,
                    })
            };

            repository.Network.Push(repository.Head, pushOptions);
        }

        /// <summary>
        /// Evaluates the result of a <see cref="Repository.Discover(string)"/> call to determine whether a repository was discovered.
        /// </summary>
        public bool RepositoryWasDiscovered(string repositoryDiscoveryResult)
        {
            var wasNotDiscovered = Instances.Values_ForLibGit2Sharp.RepositoryDiscoverNotFoundResult == repositoryDiscoveryResult;

            var wasDiscovered = !wasNotDiscovered;
            return wasDiscovered;
        }

        public void Stage(
            string localRepositoryDirectoryPath,
            IEnumerable<string> filePaths)
        {
            using var repository = new Repository(localRepositoryDirectoryPath);

            // Stage paths, if any.
            if (filePaths.Any())
            {
                Commands.Stage(repository, filePaths);
            }
        }

        /// <summary>
        /// Stages changes and returns the number of unstaged paths.
        /// </summary>
        public int StageAllUnstagedPaths(string localRepositoryDirectoryPath)
        {
            var unstagedPaths = this.ListAllUnstagedPaths(localRepositoryDirectoryPath);

            this.Stage(localRepositoryDirectoryPath,
                unstagedPaths);

            var unstagedPathsCount = unstagedPaths.Length;
            return unstagedPathsCount;
        }
    }
}
