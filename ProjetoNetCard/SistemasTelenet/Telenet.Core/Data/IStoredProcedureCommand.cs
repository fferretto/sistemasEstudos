using System;

namespace Telenet.Core.Data
{
    /// <summary>
    /// Define o contrato de implementação de um comando especializado em executar stored procedures.
    /// </summary>
    public interface IStoredProcedureCommand : IQueryData
    {
        /// <summary>
        /// Executa uma stored procedure e retorna o número de linhas afetadas pela execução.
        /// </summary>
        int Execute(params object[] values);

        /// <summary>
        /// Executa uma stored procedure assíncronamente, executando a função de callback ao término.
        /// </summary>
        void ExecuteAsync(Action<int, OutputValues> callback, params object[] values);

        /// <summary>
        /// Executa uma stored procedure assíncronamente, associando uma chave para posterior recuperação do retorno.
        /// </summary>
        void ExecuteAsync(string key, params object[] values);

        /// <summary>
        /// Executa uma stored procedure e retorna o valor de retorno da execução.
        /// </summary>
        TValue Execute<TValue>(params object[] values);

        /// <summary>
        /// Obtém os valores de saída da procedure.
        /// </summary>
        OutputValues GetOutputs();
    }
}
