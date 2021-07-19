using System;

namespace Telenet.Core.DependencyInjection
{
    /// <summary>
    /// Define o contrato de implementação de uma fábrica de serviços.
    /// </summary>
    public interface IServiceFactory
    {
        /// <summary>
        /// Obtém o tipo do serviço criado.
        /// </summary>
        Type ServiceType { get; }

        /// <summary>
        /// Obtém a instância do serviço.
        /// </summary>
        object GetService();
    }
}
