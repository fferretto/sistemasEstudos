
namespace Telenet.Carga
{
    /// <summary>
    /// Define o contrato de implementação de um objeto de configuração de serviços de carga.
    /// </summary>
    public interface IContextoCarga
    {
        /// <summary>
        /// Obtém o número da inscrição no CNPJ do cliente.
        /// </summary>
        string CnpjCliente { get; }

        /// <summary>
        /// Obtém o código da operadora do usuário.
        /// </summary>
        int CodigoOperadora { get; }

        /// <summary>
        /// Obtém o código do cliente.
        /// </summary>
        int CodigoCliente { get; }

        /// <summary>
        /// Obtém o identificador único do operador que efetuou a carga.
        /// </summary>
        int IdOperador { get; }

        /// <summary>
        /// Obtém o login do usuário da carga.
        /// </summary>
        string Login { get; }

        /// <summary>
        /// Obtém o nome da operadora.
        /// </summary>
        string NomeOperadora { get; }

        /// <summary>
        /// Obtém a pasta de armazenamento dos arquivos de carga.
        /// </summary>
        string PastaArquivosImportacao { get; }

        /// <summary>
        /// Obtém o flag que define o sistema de origem da carga.
        /// </summary>
        string SistemaOrigem { get; }

        /// <summary>
        /// Obtém o IP da máquina solicitante.
        /// </summary>
        string IpMaquinaSolicitante { get; }
    }
}
