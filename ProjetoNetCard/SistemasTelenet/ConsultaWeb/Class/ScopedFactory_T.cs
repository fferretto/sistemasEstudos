#pragma warning disable 1591

using System;
using System.Collections.Generic;
using Telenet.Core.DependencyInjection;
using Context = System.Web.HttpContext;

namespace NetCardConsulta.Class
{
    public class ScopedFactory<TService> : TransientFactory<TService>
    {
        public ScopedFactory(Func<TService> factory)
            : base(factory)
        { }

        private IDictionary<Type, object> ScopedServices
        {
            get
            {
                var scopedServices = Context.Current.Session[MvcServiceProvider.ScopedServicesSessionKey] as IDictionary<Type, object>;

                if (scopedServices == null)
                {
                    scopedServices = new Dictionary<Type, object>();
                    Context.Current.Session[MvcServiceProvider.ScopedServicesSessionKey] = scopedServices;
                }

                return scopedServices;
            }
        }

        private object GetScopedService()
        {
            var serviceType = typeof(TService);

            if (!ScopedServices.ContainsKey(serviceType))
            {
                ScopedServices.Add(serviceType, Factory());
            }

            return ScopedServices[serviceType];
        }

        public override object GetService()
        {
            return GetScopedService();
        }
    }
}

#pragma warning restore 1591
