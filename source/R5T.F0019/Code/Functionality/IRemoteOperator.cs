using System;

using LibGit2Sharp;

using R5T.T0132;


namespace R5T.F0019
{
	[FunctionalityMarker]
	public partial interface IRemoteOperator : IFunctionalityMarker
	{
		public string GetUrl(Remote remote)
        {
			var url = remote.Url;
			return url;
        }
	}
}