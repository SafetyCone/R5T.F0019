using System;


namespace R5T.F0019
{
    public class GitOperator : IGitOperator
    {
        #region Infrastructure

        public static GitOperator Instance { get; } = new();

        private GitOperator()
        {
        }

        #endregion
    }
}
