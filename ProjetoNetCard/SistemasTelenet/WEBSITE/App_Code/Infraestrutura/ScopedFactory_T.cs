#pragma warning disable 1591

using System;
using Telenet.Core.DependencyInjection;
using Context = System.Web.HttpContext;

/// <summary>
/// Summary description for ScopedFactory_T
/// </summary>
internal class ScopedFactory<TService> : TransientFactory<TService>
{
    public ScopedFactory(Func<TService> factory)
        : base(factory)
    { }

    [ThreadStatic]
    private static TService _instance;

    public override object GetService()
    {
        if (_instance == null)
        {
            _instance = Factory();
        }

        return _instance;
    }
}

#pragma warning restore 1591
