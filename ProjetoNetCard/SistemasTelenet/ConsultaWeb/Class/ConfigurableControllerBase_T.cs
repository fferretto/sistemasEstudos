
namespace NetCardConsulta.Class
{
    /// <summary>
    /// Define a classe controller base da Telenet para um serviço que contém valores iniciais de configuração.
    /// </summary>
    public class ConfigurableControllerBase<TConfig> : TelenetControllerBase
    {
        /// <summary>
        /// Cria uma nova instância do controller.
        /// </summary>
        protected ConfigurableControllerBase()
            : base()
        {
            Config = GetService<TConfig>();
        }

        /// <summary>
        /// Obtém o objeto de configuração do serviço.
        /// </summary>
        protected TConfig Config { get; private set; }
    }
}