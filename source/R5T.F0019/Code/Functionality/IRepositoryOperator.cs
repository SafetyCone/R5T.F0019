using System;

using LibGit2Sharp;

using R5T.T0132;


namespace R5T.F0019
{
	[FunctionalityMarker]
	public partial interface IRepositoryOperator : IFunctionalityMarker
	{
		public Remote GetRemote(Repository repository, string remoteRepositoryName)
        {
			var remote = repository.Network.Remotes[remoteRepositoryName];
			return remote;
		}

		public Remote GetRemote_Origin(Repository repository)
		{
			var remote = repository.Network.Remotes[Instances.RemoteRepositoryNames.Origin];
			return remote;
		}

		/// <summary>
		/// A quality-of-life overload for <see cref="GetRemote_Origin(Repository)"/>.
		/// </summary>
		public Remote GetRemote(Repository repository)
        {
			var remote = this.GetRemote_Origin(repository);
			return remote;
        }

		public string GetRemoteUrl(Repository repository, string remoteRepositoryName)
        {
			var remote = this.GetRemote(repository, remoteRepositoryName);

			var remoteUrl = Instances.RemoteOperator.GetUrl(remote);
			return remoteUrl;
		}

		public string GetRemoteUrl(Repository repository)
        {
			var remote = this.GetRemote(repository);

			var remoteUrl = Instances.RemoteOperator.GetUrl(remote);
			return remoteUrl;
		}
	}
}