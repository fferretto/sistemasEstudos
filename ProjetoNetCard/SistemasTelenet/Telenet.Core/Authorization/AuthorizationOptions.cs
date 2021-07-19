using System;
using System.Collections.Generic;
using Telenet.Core.DependencyInjection;
using Telenet.Core.Web;

namespace Telenet.Core.Authorization
{
    public class AuthorizationOptions
    {
        internal readonly HashSet<IServiceFactory> Factories = new HashSet<IServiceFactory>();
        internal Uri ServerAddress;

        private static IServiceFactory CreateAuthorizationFactory<TContext, TAuthorization, TImplementation>(Uri serverAddress)
            where TContext : IAuthorizationContext
            where TAuthorization : IAuthorization<TContext>
            where TImplementation : class, TAuthorization
        {
            return ServiceConfiguration.ServiceProvider.CreateScopedFactory(() =>
            {
                var sessionAccessor = ServiceConfiguration.ServiceProvider.GetService<ISessionAccessor>();
                var context = (TContext)Activator.CreateInstance(typeof(TContext), serverAddress);

                return (TAuthorization)Activator.CreateInstance(typeof(TImplementation), context, sessionAccessor);
            });
        }

        public AuthorizationOptions UseServerAddress(string address)
        {
            return UseServerAddress(new Uri(address));
        }

        public AuthorizationOptions UseServerAddress(Uri address)
        {
            ServerAddress = address;
            return this;
        }

        public AuthorizationOptions AddAppAuthorization<TContext>()
            where TContext : IAuthorizationContext
        {
            Factories.Add(CreateAuthorizationFactory<TContext, IAppAuthorization<TContext>, AppAuthorization<TContext>>(
                ServerAddress));
            return this;
        }

        public AuthorizationOptions AddUserAuthorization<TContext>()
            where TContext : IAuthorizationContext
        {
            Factories.Add(CreateAuthorizationFactory<TContext, IUserAuthorization<TContext>, UserAuthorization<TContext>>(
                ServerAddress));
            return this;
        }
    }
}
