using System.ComponentModel;

namespace Telenet.Carga.Abstractions
{
    /// <summary>
    /// Etapas do processo de carga assíncrona via arquivo.
    /// </summary>
    public enum EtapaCarga
    {
        /// <summary>
        /// Não existe um processo de carga sendo executado.
        /// </summary>
        [Description("Não existe carga de limite de cartões sendo executada")]
        SemCargaEmExecucao = -1,

        /// <summary>
        /// Erro na etapa de validação do layout do arquivo de carga.
        /// </summary>
        [Description("Validação do layout do arquivo")]
        ValidacaoLayoutErro = 0,

        /// <summary>
        /// Etapa de validação do layout do arquivo de carga finalizada com sucesso.
        /// </summary>
        [Description("Validação do layout do arquivo")]
        ValidacaoLayoutOk = 1,

        /// <summary>
        /// Erro na etapa de verificação dos dados do arquivo de carga.
        /// </summary>
        [Description("Verificação dos dados do arquivo")]
        VerificacaoDadosErro = 2,

        /// <summary>
        /// Etapa de verificação dos dados do arquivo de carga finalizada com sucesso.
        /// </summary>
        [Description("Verificação dos dados do arquivo")]
        VerificacaoDadosOk = 3,

        /// <summary>
        /// Erro na etapa de solicitação da carga dos cartões.
        /// </summary>
        [Description("Solicitação da carga")]
        SolicitacaoCargaErro = 4,

        /// <summary>
        /// Etapa da solicitação de carga dos cartões finalizada com sucesso.
        /// </summary>
        [Description("Solicitação da carga")]
        SolicitacaoCargaOk = 5,

        /// <summary>
        /// Carga finalizada e log do processo baixado.
        /// </summary>
        [Description("Solicitação de carga finalizada")]
        CargaFinalizada = 6
    }
}
