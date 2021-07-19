
namespace NetCardConsulta.Class
{
    /// <summary>
    /// Define a classe controller base para hospedagem de serviços com valores pré definidos de configuração.
    /// </summary>
    public abstract class ConfigurableServiceControllerBase<TService, TConfig> : ConfigurableControllerBase<TConfig>
    {
        /// <summary>
        /// Cria uma nova instância do controller.
        /// </summary>
        protected ConfigurableServiceControllerBase()
            : base()
        {
            Service = GetService<TService>();
        }

        /// <summary>
        /// Obtém a instância do serviço hospedado pelo controller.
        /// </summary>
        protected TService Service { get; private set; }
    }
}