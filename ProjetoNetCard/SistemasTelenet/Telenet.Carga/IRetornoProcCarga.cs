
namespace Telenet.Carga
{
    /// <summary>
    /// Define o contrato de implementação de um retorno de execução de procedure.
    /// </summary>
    public interface IRetornoProcCarga
    {
        /// <summary>
        /// Obtém o código de erro retornado.
        /// </summary>
        int Erro { get; }

        /// <summary>
        /// Obtém a mensagem de erro retornada.
        /// </summary>
        string Mensagem { get; }
    }
}
