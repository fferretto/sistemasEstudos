
namespace Telenet.Carga.Abstractions
{
    /// <summary>
    /// Define o contrato de implementação de um objeto que representa os dados para carga manual de um cartão de usuário.
    /// </summary>
    public interface IDadosCargaCartao : ICartaoUsuario
    {
        /// <summary>
        /// Obtém o código do centro de custo para o qual o valor da carga será associado.
        /// </summary>
        string CentroCusto { get; }

        /// <summary>
        /// Obtém o valor da carga.
        /// </summary>
        decimal Valor { get; }
    }
}
