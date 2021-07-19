using System;

namespace Telenet.Core.Authorization
{
    public interface IAuthorizationContext
    {
        string ApplicationKey { get; }
        Uri ServerAddress { get; }
    }
}
