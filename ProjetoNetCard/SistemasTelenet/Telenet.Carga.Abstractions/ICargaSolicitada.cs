using System;

namespace Telenet.Carga.Abstractions
{
    /// <summary>
    /// Define o contrato de implementação de um objeto que representa uma carga solicitada para um cliente.
    /// </summary>
    public interface ICargaSolicitada
    {
        /// <summary>
        /// Obtém o código do cliente da carga.
        /// </summary>
        int CodigoCliente { get; }

        /// <summary>
        /// Obtém o número da carga.
        /// </summary>
        int NumeroCarga { get; }

        /// <summary>
        /// Obtém a data de solicitação da carga.
        /// </summary>
        DateTime DataSolicitacao { get; }

        /// <summary>
        /// Obtém a data de programação da carga.
        /// </summary>
        DateTime DataProgramacao { get; }

        /// <summary>
        /// Obtém o valor da carga.
        /// </summary>
        decimal Valor { get; }

        /// <summary>
        /// Obtém a quantidade de cartões que compõe a solicitação de carga.
        /// </summary>
        int Quantidade { get; }

        /// <summary>
        /// Obtém a situação da carga no sistema.
        /// </summary>
        string Situacao { get; }
    }
}
