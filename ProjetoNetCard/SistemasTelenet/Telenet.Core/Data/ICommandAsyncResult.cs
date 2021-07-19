using System;
using System.Data.Common;

namespace Telenet.Core.Data
{
    /// <summary>
    /// Define o contrato de implementação de um objeto que representa o resultado assíncrono de um comando.
    /// </summary>
    public interface ICommandAsyncResult : IDisposable
    {
        /// <summary>
        /// Obtém os valores de saída da execução do comando.
        /// </summary>
        OutputValues GetOutputs { get; }

        /// <summary>
        /// Obtém a chave de identificação única do comando executado.
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Obtém um flag que informa se o comando já terminou sua execução no banco.
        /// </summary>
        bool Finished { get; }
    }
}
