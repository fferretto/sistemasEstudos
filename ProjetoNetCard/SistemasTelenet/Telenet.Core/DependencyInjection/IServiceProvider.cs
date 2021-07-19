using System;

namespace Telenet.Core.DependencyInjection
{
    /// <summary>
    /// Define o contrato de implementação de um provider de serviços.
    /// </summary>
    public interface IServiceProvider
    {
        /// <summary>
        /// Obtém a coleção de serviços configurados.
        /// </summary>
        IServiceCollection Services { get; }

        /// <summary>
        /// Cria uma nova fábrica para um serviço tipo singleton.
        /// </summary>
        IServiceFactory CreateSingletonFactory<TService>(Func<TService> factory);

        /// <summary>
        /// Cria uma nova fábrica para um serviço de escopo.
        /// </summary>
        IServiceFactory CreateScopedFactory<TService>(Func<TService> factory);

        /// <summary>
        /// Cria uma nova fábrica para um serviço trensiente.
        /// </summary>
        IServiceFactory CreateTransientFactory<TService>(Func<TService> factory);

        /// <summary>
        /// Resolve a instância de um serviço.
        /// </summary>
        object GetService(Type serviceType);

        /// <summary>
        /// Resolve a instância de um serviço.
        /// </summary>
        TService GetService<TService>();
    }
}
