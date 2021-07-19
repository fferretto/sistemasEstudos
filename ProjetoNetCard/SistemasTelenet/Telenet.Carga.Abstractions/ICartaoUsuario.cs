
namespace Telenet.Carga.Abstractions
{
    /// <summary>
    /// Define o contrato de implementação de um objeto que representa um cartão de usuário.
    /// </summary>
    public interface ICartaoUsuario
    {
        /// <summary>
        /// Obtém o número do CPF do usuário.
        /// </summary>
        string Cpf { get; }

        /// <summary>
        /// Obtém o número do CPF original quando for uma operação de troca de CPF de usuário.
        /// </summary>
        string CpfOrigem { get; }

        /// <summary>
        /// Obtém o código da filial associada ao cartão.
        /// </summary>
        short Filial { get; }

        /// <summary>
        /// Obtém a matrícula do usuário.
        /// </summary>
        string Matricula { get; }

        /// <summary>
        /// Obtém o número do cartão.
        /// </summary>
        string Numero { get; }

        /// <summary>
        /// Obtém o nome do usuário do cartão.
        /// </summary>
        string Nome { get; }

        /// <summary>
        /// Obtém o setor associado ao cartão.
        /// </summary>
        string Setor { get; }
    }
}
