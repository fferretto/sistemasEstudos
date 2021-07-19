
namespace Telenet.Carga.Abstractions
{
    /// <summary>
    /// Dados do resumo de uma carga.
    /// </summary>
    public interface IResumoCarga
    {
        /// <summary>
        /// Obtém ou define o código do cliente de uma carga.
        /// </summary>
        int CodigoCliente { get; }

        /// <summary>
        /// Obtém ou define o identificador único da carga.
        /// </summary>
        string IdCarga { get; }

        /// <summary>
        /// Obtém ou define o número da carga.
        /// </summary>
        int NumeroCarga { get; }

        /// <summary>
        /// Obtém ou define o texto do log.
        /// </summary>
        string RegistroLog { get; }

        /// <summary>
        /// Obtém ou define o tipo de registro de log.
        /// </summary>
        char TipoRegistro { get; }
    }
}
