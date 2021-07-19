
namespace Telenet.Core.DependencyInjection
{
    /// <summary>
    /// Tipos de ciclo de vida de serviços configuráveis.
    /// </summary>
    public enum ServiceLifetime
    {
        /// <summary>
        /// A instância do serviço resolvida via ID é única para toda a aplicação e todas as threads.
        /// </summary>
        Singleton = 0,

        /// <summary>
        /// A instância do serviço resolvida via ID é única para o escopo da chamada.
        /// </summary>
        Scoped = 1,

        /// <summary>
        /// A instância do serviço resolvida via ID é sempre um novo objeto para cada chamada.
        /// </summary>
        Transient = 2
    }
}
