
namespace NetCardConsulta.Class
{
    /// <summary>
    /// Define a classe controller base para hospedagem de serviços.
    /// </summary>
    public class ServiceControllerBase<TService> : TelenetControllerBase
    {
        /// <summary>
        /// Cria uma nova instância do controller.
        /// </summary>
        protected ServiceControllerBase()
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