
namespace Telenet.Carga.Abstractions
{
    /// <summary>
    /// Define o filtro para pesquisa de cartão.
    /// </summary>
    public class FiltroCartao
    {
        /// <summary>
        /// Obtém ou define o número do CPF do usuário do cartão a ser pesquisado.
        /// </summary>
        public string Cpf { get; set; }

        /// <summary>
        /// Obtém ou define o nome do usuário do cartão a ser pesquisado.
        /// </summary>
        public string NomeUsuario { get; set; }

        /// <summary>
        /// Obtém ou define o número do cartão a ser pesquisado.
        /// </summary>
        public string Numero { get; set; }
    }
}
