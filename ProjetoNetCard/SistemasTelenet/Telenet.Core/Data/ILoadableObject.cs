using System.Data;

namespace Telenet.Core.Data
{
    /// <summary>
    /// Define o contrato de implementação de método de carga de dados para um objeto.
    /// </summary>
    public interface ILoadableObject
    {
        /// <summary>
        /// Carrega o objeto com os dados do registro corrente do reader.
        /// </summary>
        void LoadFrom(IDataReader reader);
    }
}
