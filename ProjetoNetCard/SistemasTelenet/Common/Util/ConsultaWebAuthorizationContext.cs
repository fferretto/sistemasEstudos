using System;
using System.Configuration;
using Telenet.Core.Authorization;

namespace NetCard.Common.Util
{
    public class ConsultaWebAuthorizationContext : AuthorizationContextBase
    {
        public ConsultaWebAuthorizationContext(Uri serverAddress)
            : base(ConfigurationManager.AppSettings["ConsultaWebApplicationKey"], serverAddress)
        { }
    }
}