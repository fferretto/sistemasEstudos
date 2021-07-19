using Telenet.Core.Authorization;
using System;

namespace ConsoleApp4.Authorization
{
    public class ConsultaWebContext : AuthorizationContextBase
    {
        public ConsultaWebContext(Uri serverAddress) 
            : base("a8bfc2e2-1759-412d-94c3-fe65378da5a4:v@378u$40Ez265r4F#8BtC9FXaS289FF3D*5", serverAddress)
        { }
    }
}
