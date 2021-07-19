using System;
using System.Linq;
using System.Reflection;

namespace Telenet.Core.DependencyInjection
{
    /// <summary>
    /// Extensões funcionais para o tipo SIL.DI.IServiceCollection
    /// </summary>
    public static class ServiceConfiguration
    {
        private static object _providerSync = new object();
        private static object _servicesSync = new object();
        private static IServiceProvider _provider;

        internal static Func<TService> ConfigureFactory<TService, TImplementation>(IServiceCollection services)
            where TImplementation : class, TService
        {
            var type = typeof(TImplementation);
            var constructorInfo = type.GetConstructors(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault();

            if (constructorInfo == null)
            {
                throw new MissingMethodException(type.Name);
            }

            var constructorParameters = constructorInfo.GetParameters();

            if (constructorParameters.Length == 0)
            {
                return () => Activator.CreateInstance<TImplementation>();
            }

            return () =>
            {
                var parameters = new object[constructorParameters.Length];

                for (int i = 0; i < constructorParameters.Length; i++)
                {
                    var parameter = constructorParameters[i];
                    var service = _provider.GetService(parameter.ParameterType);

                    if (service == null)
                    {
                        throw new ArgumentNullException(string.Format("Service {0} not configures.", parameter.ParameterType));
                    }

                    parameters[i] = service;
                }

                return (TService)Activator.CreateInstance(type, parameters);
            };
        }

        /// <summary>
        /// Obtém o provider de serviços.
        /// </summary>
        public static IServiceProvider ServiceProvider { get { return _provider; } }

        /// <summary>
        /// Define o provider a ser utilizado pelo configurador de serviços.
        /// </summary>
        public static IServiceCollection Provider(IServiceProvider serviceProvider)
        {
            lock (_providerSync)
            {
                _provider = serviceProvider;
            }

            return _provider.Services;
        }

        /// <summary>
        /// Adiciona a configuração de um serviço através de uma fábrica específica.
        /// </summary>
        public static void AddService(this IServiceCollection services, IServiceFactory factory)
        {
            lock (_servicesSync)
            {
                if (services.Any(sf => sf.ServiceType == factory.ServiceType))
                {
                    throw new ArgumentException("Service factory already configured.");
                }

                services.Add(factory);
            }
        }

        /// <summary>
        /// Adiciona a configuração de um serviço do tipo Scoped a ser criado com a instância do objeto do tipo definido.
        /// </summary>
        public static IServiceCollection AddScoped<TService, TImplementation>(this IServiceCollection services)
            where TImplementation : class, TService
        {
            services.AddService(_provider.CreateScopedFactory<TService>(ConfigureFactory<TService, TImplementation>(services)));
            return services;
        }

        /// <summary>
        /// Adiciona a configuração de um serviço do tipo Scoped a ser criado por uma factory informada.
        /// </summary>
        public static IServiceCollection AddScoped<TService>(this IServiceCollection services, Func<IServiceCollection, TService> factory)
        {
            services.AddService(_provider.CreateScopedFactory<TService>(() => factory(services)));
            return services;
        }

        /// <summary>
        /// Adiciona a configuração de um serviço do tipo Singleton a ser criado com a instância do objeto do tipo definido.
        /// </summary>
        public static IServiceCollection AddSingleton<TService, TImplementation>(this IServiceCollection services)
            where TImplementation : class, TService
        {
            services.AddService(_provider.CreateSingletonFactory<TService>(ConfigureFactory<TService, TImplementation>(services)));
            return services;
        }

        /// <summary>
        /// Adiciona a configuração de um serviço do tipo Singleton criado com a instância informada.
        /// </summary>
        public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, TService instance)
        {
            services.AddService(_provider.CreateSingletonFactory<TService>(() => instance));
            return services;
        }

        /// <summary>
        /// Adiciona a configuração de um serviço do tipo Singleton a ser criado por uma factory informada.
        /// </summary>
        public static IServiceCollection AddSingleton<TService>(this IServiceCollection services, Func<IServiceCollection, TService> factory)
        {
            services.AddService(_provider.CreateSingletonFactory<TService>(() => factory(services)));
            return services;
        }

        /// <summary>
        /// Adiciona a configuração de um serviço do tipo Transient a ser criado com a instância do objeto do tipo definido.
        /// </summary>
        public static IServiceCollection AddTransient<TService, TImplementation>(this IServiceCollection services)
            where TImplementation : class, TService
        {
            services.AddService(_provider.CreateTransientFactory<TService>(ConfigureFactory<TService, TImplementation>(services)));
            return services;
        }

        /// <summary>
        /// Adiciona a configuração de um serviço do tipo Singleton a ser criado por uma factory informada.
        /// </summary>
        public static IServiceCollection AddTransient<TService>(this IServiceCollection services, Func<IServiceCollection, TService> factory)
        {
            services.AddService(_provider.CreateTransientFactory<TService>(() => factory(services)));
            return services;
        }
    }
}
