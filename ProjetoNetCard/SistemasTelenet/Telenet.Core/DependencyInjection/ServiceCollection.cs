#pragma warning disable 1591

using System.Collections.Generic;

namespace Telenet.Core.DependencyInjection
{
    internal class ServiceCollection : List<IServiceFactory>, IServiceCollection
    { }
}

#pragma warning restore 1591
