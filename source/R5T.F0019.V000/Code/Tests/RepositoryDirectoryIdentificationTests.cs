using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace R5T.F0019.V000
{
    [TestClass]
    public partial class RepositoryDirectoryIdentificationTests
    {
        /// <summary>
        /// Test that a repository directory path *is* a repository directory path.
        /// </summary>
        [TestMethod]
        public void RepositoryDirectoryIsRepositoryDirectory()
        {
            var actual = Instances.GitOperator.IsRepository(
                Instances.DirectoryPaths.RepositoryDirectory);

            var expected = true;

            Instances.Assertion.AreEqual(
                actual,
                expected);
        }

        /// <summary>
        /// Test that a directory path that is not a repository directory path is *not* a repository directory path.
        /// </summary>
        [TestMethod]
        public void NotARepositoryDirectoryIsNotARepositoryDirectory()
        {
            var actual = Instances.GitOperator.IsRepository(
                Instances.DirectoryPaths.NotARepositoryDirectory);

            var expected = false;

            Instances.Assertion.AreEqual(
                actual,
                expected);
        }

        /// <summary>
        /// Test that a directory path in a repository directory *is* in a repository directory.
        /// </summary>
        [TestMethod]
        public void InRepositoryDirectoryIsInRepositoryDirectory()
        {
            var actual = Instances.GitOperator.IsInRepository(
                Instances.DirectoryPaths.FileInRepositoryDirectory);

            var expected = true;

            Instances.Assertion.AreEqual(
                actual,
                expected);
        }

        /// <summary>
        /// Test that a repository directory path *is* in a repository directory.
        /// </summary>
        [TestMethod]
        public void RepositoryDirectoryIsInRepositoryDirectory()
        {
            var actual = Instances.GitOperator.IsInRepository(
                Instances.DirectoryPaths.RepositoryDirectory);

            var expected = true;

            Instances.Assertion.AreEqual(
                actual,
                expected);
        }

        /// <summary>
        /// Test that a repository git directory path is not a repository directory.
        /// </summary>
        [TestMethod]
        public void RepositoryGitDirectoryIsNotRepositoryDirectory()
        {
            var actual = Instances.GitOperator.IsRepositoryDirectory(
                Instances.DirectoryPaths.RepositoryGitDirectory);

            var expected = false;

            Instances.Assertion.AreEqual(
                actual,
                expected);
        }
    }
}
