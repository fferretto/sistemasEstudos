
namespace Telenet.Carga
{
    /// <summary>
    /// Define o contrato de implementação de um objeto de configuração de serviços de carga manual.
    /// </summary>
    public interface IContextoCargaManual : IContextoCarga
    {
        /// <summary>
        /// Obtém o número máximo de cartões permitidos para cada solicitação manual de carga.
        /// </summary>
        int MaximoCartoesPorSolicitacao { get; }
    }
}
