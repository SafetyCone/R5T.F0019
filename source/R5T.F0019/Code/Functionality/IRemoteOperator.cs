using System;

using LibGit2Sharp;

using R5T.T0132;


namespace R5T.F0019
{
	/// <summary>
	/// Operator for <see cref="Remote"/>-related functionality.
	/// </summary>
	/// <remarks>
	/// See new work in R5T.L0083.F001.
	/// </remarks>
	[FunctionalityMarker]
	public partial interface IRemoteOperator : IFunctionalityMarker
	{
		// See R5T.L0082.F001.IRemoteOperator.Get_Url().
		public string GetUrl(Remote remote)
        {
			var url = remote.Url;
			return url;
        }
	}
}