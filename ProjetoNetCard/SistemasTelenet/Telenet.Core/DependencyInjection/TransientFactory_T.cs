#pragma warning disable 1591

using System;

namespace Telenet.Core.DependencyInjection
{
    public class TransientFactory<TService> : IServiceFactory
    {
        public TransientFactory(Func<TService> factory)
        {
            if (factory == null)
            {
                throw new ArgumentNullException("factory");
            }

            Factory = factory;
        }

        protected readonly Func<TService> Factory;

        public Type ServiceType { get { return typeof(TService); } }

        public virtual object GetService()
        {
            return Factory();
        }
    }
}

#pragma warning restore 1591
