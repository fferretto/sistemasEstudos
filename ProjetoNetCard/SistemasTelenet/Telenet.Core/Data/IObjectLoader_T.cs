using System.Collections.Generic;
using System.Data;

namespace Telenet.Core.Data
{
    /// <summary>
    /// Define o contrato de implementação de objeto de carga de dados para um tipo específico.
    /// </summary>
    /// <typeparam name="TRecord">O tipo do objeto que será preenchido com os dados.</typeparam>
    public interface IObjectLoader<TRecord> where TRecord : new()
    {
        /// <summary>
        /// Carrega a instância do objeto com os dados do registro corrente do reader.
        /// </summary>
        IEnumerable<TRecord> Load(IDataReader reader);
    }
}
