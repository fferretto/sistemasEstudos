// -----------------------------------------------------------------------------
// Telenet Tecnologia e Serviços em Rede
// Autor: Alexandre Chestter
// Data: 16/09/2019
// -----------------------------------------------------------------------------

namespace Telenet.Core.Web
{
    /// <summary>
    /// Define o contrato de implementação de um binder para templates HTML.
    /// </summary>
    public interface IHtmlFileBinder
    {
        /// <summary>
        /// Obtém o nome do binder.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Efetua o bind de valores com o template, devolvendo o HTML final.
        /// </summary>
        string Bind(IViewBag viewBag);

        /// <summary>
        /// Efetua o bind de valores com o template, devolvendo o HTML final.
        /// </summary>
        string Bind<TModel>(TModel model) where TModel : class, new();
    }
}
