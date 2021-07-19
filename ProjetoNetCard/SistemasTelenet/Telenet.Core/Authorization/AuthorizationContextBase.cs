using System;

namespace Telenet.Core.Authorization
{
    public abstract class AuthorizationContextBase : IAuthorizationContext
    {
        public AuthorizationContextBase(string applicationKey, Uri serverAddress)
        {
            ApplicationKey = applicationKey;
            ServerAddress = serverAddress;
        }

        public string ApplicationKey { get; }

        public Uri ServerAddress { get; }
    }
}
