using Telenet.Core.Data;

namespace Telenet.Carga
{
    /// <summary>
    /// Define o contrato de implementação do objeto que encapsula os clientes para acesso aos bancos de dados das aplicações Telenet.
    /// </summary>
    public interface IAcessoDados
    {
        /// <summary>
        /// Obtém o usuário do banco de dados.
        /// </summary>
        string DbUser { get; }

        /// <summary>
        /// Obtém um objeto cliente para acesso ao banco do Concentrador.
        /// </summary>
        IDbClient Concentrador { get; }

        /// <summary>
        /// Obtém um objeto cliente para acesso ao banco do NetCard.
        /// </summary>
        IDbClient NetCard { get; }
    }
}
