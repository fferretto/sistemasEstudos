using System;

namespace PagNet.Application.Application.Common
{
    public class PagNetException : ApplicationException
    {
        public PagNetException()
            : base()
        { }

        public PagNetException(string message)
            : base(message)
        { }

        public PagNetException(bool tratada, string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
