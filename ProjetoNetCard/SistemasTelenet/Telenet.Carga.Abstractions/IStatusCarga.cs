
namespace Telenet.Carga.Abstractions
{
    /// <summary>
    /// Define o contrato de implementação de um objeto que representa o status do processamento da carga.
    /// </summary>
    public interface IStatusCarga
    {
        /// <summary>
        /// Obtém o código de erro da etapa.
        /// </summary>
        int Erro { get; }

        /// <summary>
        /// Obtém a etapa do processo.
        /// </summary>
        EtapaCarga EtapaCarga { get; }

        /// <summary>
        /// Obtém o identificador único do processo.
        /// </summary>
        string IdProcesso { get; }

        /// <summary>
        /// Obtém a mensagem do status.
        /// </summary>
        string Mensagem { get; }

        /// <summary>
        /// Obtém o nível do processamento.
        /// </summary>
        int Nivel { get; }
    }
}
