
namespace Telenet.Carga
{
    /// <summary>
    /// Define o contrato de implementação de um objeto de configuração do serviço de consulta de cargas.
    /// </summary>
    public interface IContextoConsultaCarga
    {
        /// <summary>
        /// Obtém o código do cliente.
        /// </summary>
        int CodigoCliente { get; }
    }
}
