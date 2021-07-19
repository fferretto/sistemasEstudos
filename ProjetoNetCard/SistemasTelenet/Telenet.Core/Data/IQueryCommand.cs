
namespace Telenet.Core.Data
{
    /// <summary>
    /// Define o contrato de implementação de um comando especializado em executar queries no banco de dados.
    /// </summary>
    public interface IQueryCommand : IQueryData
    {
        /// <summary>
        /// Recupera um valor escalar retornado pela consulta.
        /// </summary>
        TValue GetScalar<TValue>(params object[] values);

        /// <summary>
        /// Recupera um valor escalar retornado pela consulta.
        /// </summary>
        TValue GetScalar<TValue>(TValue defaultValue, params object[] values);
    }
}
