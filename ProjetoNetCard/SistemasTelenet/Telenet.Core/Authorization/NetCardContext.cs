using System;

namespace Telenet.Core.Authorization
{
    public class NetCardContext : AuthorizationContextBase
    {
        public NetCardContext(Uri serverAddress)
            : base("ab6187df-c43b-40d1-8e45-17413584124f:45c065VV72@D26A#40E5a*13iAa-239F8DFc", serverAddress)
        { }
    }
}
