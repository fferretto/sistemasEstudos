// -----------------------------------------------------------------------------
// Telenet Tecnologia e Serviços em Rede
// Autor: Alexandre Chestter
// Data: 16/09/2019
// -----------------------------------------------------------------------------

using System.Collections.Generic;

namespace Telenet.Core.Web
{
    /// <summary>
    /// Define o contrato de implementação de um objeto de dados dinâmicos para binding com fontes HTML marcadas com tags no formato @{NomeTag};
    /// </summary>
    public interface IViewBag : IDictionary<string, object>
    { }
}