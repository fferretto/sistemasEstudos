#pragma warning disable 1591

using System;

namespace SIL
{
    public class UserWarningException : Exception
    {
        public UserWarningException()
            : base()
        { }

        public UserWarningException(string message)
            : base(message)
        { }

        public UserWarningException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}

#pragma warning restore 1591