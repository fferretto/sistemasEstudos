using System.Data;

namespace Telenet.Core.Data
{
    /// <summary>
    /// Define o contrato de implementação de um objeto especializado na execução de comandos contra um banco de dados.
    /// </summary>
    public interface IDbClient
    {
        /// <summary>
        /// Executa um comando DML no banco.
        /// </summary>
        IExecuteCommand Command(string sql);

        /// <summary>
        /// Cria um comando puro, para execuções customizadas.
        /// </summary>
        IDbCommand CreateCommand(string sql);

        /// <summary>
        /// Força a atualização do flag de informação de finalização de um comando executado assíncronamente no banco.
        /// </summary>
        void FinishAsyncResult(string key);

        /// <summary>
        /// Obtém um agente para manipulação de jobs no servidor.
        /// </summary>
        IJob Job(string name, string description = null);

        /// <summary>
        /// Recupera o resultado de um comando executado assíncronamente no banco.
        /// </summary>
        ICommandAsyncResult GetAsyncResult(string key);

        /// <summary>
        /// Executa uma consulta no banco.
        /// </summary>
        IQueryCommand Query(string sql);

        /// <summary>
        /// Remove um resultado de um comando executado assíncronamente no banco.
        /// </summary>
        void RemoveAsyncResult(string key);

        /// <summary>
        /// Executa uma stored procedure no banco.
        /// </summary>
        IStoredProcedureCommand StoredProcedure(string sql);
    }
}
