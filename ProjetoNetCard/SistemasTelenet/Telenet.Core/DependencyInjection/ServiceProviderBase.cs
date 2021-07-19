#pragma warning disable 1591

using System;
using System.Linq;

namespace Telenet.Core.DependencyInjection
{
    public abstract class ServiceProviderBase : IServiceProvider
    {
        private IServiceCollection _services = new ServiceCollection();

        public IServiceCollection Services { get { return _services; } }

        public virtual IServiceFactory CreateSingletonFactory<TService>(Func<TService> factory)
        {
            return new SingletonFactory<TService>(factory);
        }

        public abstract IServiceFactory CreateScopedFactory<TService>(Func<TService> factory);

        public virtual IServiceFactory CreateTransientFactory<TService>(Func<TService> factory)
        {
            return new TransientFactory<TService>(factory);
        }

        public object GetService(Type serviceType)
        {
            var factory = _services.FirstOrDefault(s => s.ServiceType == serviceType);
            return factory == null ? null : factory.GetService();
        }

        public TService GetService<TService>()
        {
            return (TService)GetService(typeof(TService));
        }
    }
}

#pragma warning restore 1591
