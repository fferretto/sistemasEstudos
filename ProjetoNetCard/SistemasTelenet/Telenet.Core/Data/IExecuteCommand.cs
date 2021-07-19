
namespace Telenet.Core.Data
{
    /// <summary>
    /// Define o contrato de implementação de um comando especializado em execução de comandos DML.
    /// </summary>
    public interface IExecuteCommand
    {
        /// <summary>
        /// Executa o comando e retorna o número de linhas afetadas pela execução.
        /// </summary>
        int Execute(params object[] values);
    }
}
