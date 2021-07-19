#pragma warning disable 1591

using System;
using Telenet.Core.DependencyInjection;

public class WebFormsServiceProvider : ServiceProviderBase
{
    public override IServiceFactory CreateScopedFactory<TService>(Func<TService> factory)
    {
        return new ScopedFactory<TService>(factory);
    }
}

#pragma warning restore 1591
