using System.Collections.Generic;

namespace Telenet.Core.DependencyInjection
{
    /// <summary>
    /// Define o contrato de implementação de uma coleção de configurações de serviços resolvidos via DI na aplicação.
    /// </summary>
    public interface IServiceCollection : IList<IServiceFactory>
    { }
}
