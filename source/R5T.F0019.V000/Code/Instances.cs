using System;

using R5T.T0119;


namespace R5T.F0019.V000
{
    public static class Instances
    {
        public static IAssertion Assertion { get; } = T0119.Assertion.Instance;
        public static IDirectoryPaths DirectoryPaths { get; } = V000.DirectoryPaths.Instance;
        public static IGitOperator GitOperator { get; } = F0019.GitOperator.Instance;
    }
}
