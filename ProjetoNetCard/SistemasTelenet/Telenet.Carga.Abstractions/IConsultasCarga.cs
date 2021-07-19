using System.Collections.Generic;

namespace Telenet.Carga.Abstractions
{
    /// <summary>
    /// Define o contrato de implementação do serviço de consultas do dómínio Carga.
    /// </summary>
    public interface IConsultasCarga
    {
        /// <summary>
        /// Obtém a lista de cargas solicitadas no sistema para o cliente atual.
        /// </summary>
        IEnumerable<ICargaSolicitada> CargasSolicitadas();

        /// <summary>
        /// Obtém um cartão a partir de um filtro.
        /// </summary>
        ICartaoUsuario ObterCartao(FiltroCartao filtro);

        /// <summary>
        /// Verifica se um determinado número de CPF é válido em uma operação de troca de CPF.
        /// </summary>
        bool CpfValidoParaTroca(string cpf, out int erro, out string mensagem);

        /// <summary>
        /// Verifica se um determinado número de CPF temporário é válido em uma operação de troca de CPF.
        /// </summary>
        bool CpfTemporarioValidoParaTroca(string cpf, out int erro, out string mensagem);

        /// <summary>
        /// Verifica se o cliente atual pode fazer transferência de CPF de cartões.
        /// </summary>
        bool ClientePodeTrocarCpf();

        /// <summary>
        /// Obtém uma lista de cartões a partir de um filtro.
        /// </summary>
        IEnumerable<string> LookupPara(FiltroCartao filtro);
    }
}
