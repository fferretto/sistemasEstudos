#pragma warning disable 1591

using System;

namespace SIL
{
    public class UnauthorizedUserAgentException : Exception
    {
        public UnauthorizedUserAgentException()
            : base()
        { }

        public UnauthorizedUserAgentException(string message)
            : base(message)
        { }

        public UnauthorizedUserAgentException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

#pragma warning restore 1591