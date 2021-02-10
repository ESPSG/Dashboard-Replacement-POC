using System;

namespace AMT.Data.Common
{
    /// <summary>
    ///     Use this exception when the application cannot proceed.  This is not typical.
    /// </summary>
    public class FatalException : Exception
    {
        private const int DefaultExitCode = -1;

        public FatalException(Exception innerException, string message, int exitCode = DefaultExitCode)
            : base(message, innerException)
        {
            ExitCode = exitCode;
        }

        public FatalException(string message, int exitCode = DefaultExitCode)
            : this(null, message, exitCode) { }

        public FatalException(string message, params object[] args)
            : this(null, message)
        {
            Args = args;
        }

        public object[] Args { get; private set; }
        public int ExitCode { get; private set; }
    }
}