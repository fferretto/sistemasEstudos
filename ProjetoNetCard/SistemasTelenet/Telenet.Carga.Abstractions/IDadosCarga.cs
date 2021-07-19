using System;

namespace Telenet.Carga.Abstractions
{
    /// <summary>
    /// Define o contrato de implementação de um objeto que representa os dados carga.
    /// </summary>
    public interface IDadosCarga
    {
        /// <summary>
        /// Obtém o flag que identifica se o log do processo da carga foi baixado pelo cliente.
        /// </summary>
        bool BaixouLog { get; }

        /// <summary>
        /// Obtém o CNPJ do cliente da carga.
        /// </summary>
        string Cnpj { get; }
        
        /// <summary>
        /// Obtém o código do cliente da carga.
        /// </summary>
        int CodigoCliente { get; }
        
        /// <summary>
        /// Obtém a data da programação da carga.
        /// </summary>
        DateTime? DataProgramacao { get; }
        
        /// <summary>
        /// Obtém o código do erro ocorrido na etapa atual do processo de carga.
        /// </summary>
        int Erro { get; }
        
        /// <summary>
        /// Obtém o antigo identificador único da carga.
        /// </summary>
        string IdCarga { get; }

        /// <summary>
        /// Obtém o identificador único da carga.
        /// </summary>
        string IdProcesso { get; }

        /// <summary>
        /// Obtém o nome do arquivo de carga.
        /// </summary>
        string NomeArquivoCarga { get; }

        /// <summary>
        /// Obtém o nome original do arquivo de carga.
        /// </summary>
        string NomeArquivoOriginal { get; }

        /// <summary>
        /// Obtém o nome da tabela temporária da carga.
        /// </summary>
        string NomeTabela { get; }

        /// <summary>
        /// Obtém o número da carga.
        /// </summary>
        int NumeroCarga { get; }

        /// <summary>
        /// Obtém a etapa atual do processo de carga.
        /// </summary>
        EtapaCarga EtapaCarga { get; }

        /// <summary>
        /// Obtém o total de registros da carga.
        /// </summary>
        int TotalRegistros { get; }

        /// <summary>
        /// Obtém o valor total da carga.
        /// </summary>
        decimal ValorCarga { get; }
    }
}
