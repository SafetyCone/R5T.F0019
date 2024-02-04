using System;


namespace R5T.F0019
{
	public class RemoteOperator : IRemoteOperator
	{
		#region Infrastructure

	    public static IRemoteOperator Instance { get; } = new RemoteOperator();

	    private RemoteOperator()
	    {
        }

	    #endregion
	}
}