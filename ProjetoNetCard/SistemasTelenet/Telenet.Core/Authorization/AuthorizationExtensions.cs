using System;
using Telenet.Core.DependencyInjection;

namespace Telenet.Core.Authorization
{
    public static class AuthorizationExtensions
    {
        public static IServiceCollection AddAuthorizarion(this IServiceCollection services, Action<AuthorizationOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException("configure");
            }

            var options = new AuthorizationOptions();
            configure(options);

            foreach (var authorizer in options.Factories)
            {
                services.AddService(authorizer);
            }

            return services;
        }
    }
}
