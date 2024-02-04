using System;


namespace R5T.F0019
{
	public class RemoteRepositoryNames : IRemoteRepositoryNames
	{
		#region Infrastructure

	    public static IRemoteRepositoryNames Instance { get; } = new RemoteRepositoryNames();

	    private RemoteRepositoryNames()
	    {
        }

	    #endregion
	}
}