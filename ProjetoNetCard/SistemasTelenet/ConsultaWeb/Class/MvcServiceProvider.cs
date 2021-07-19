#pragma warning disable 1591

using System;
using Telenet.Core.DependencyInjection;

namespace NetCardConsulta.Class
{
    public class MvcServiceProvider : ServiceProviderBase
    {
        public const string ScopedServicesSessionKey = "__SCOPED_SERVICES__";

        public override IServiceFactory CreateScopedFactory<TService>(Func<TService> factory)
        {
            return new ScopedFactory<TService>(factory);
        }
    }
}

#pragma warning restore 1591
