using System;
using System.Collections.Generic;

namespace Telenet.Carga.Abstractions
{
    /// <summary>
    /// Define o contrato do serviço de solicitação de carga manual de cartões. Esta operação realiza a carga de valor limite de utilização 
    /// de um cartão ou de um pequeno grupo de cartões.
    /// </summary>
    public interface ISolicitacaoCargaManual : IControleCarga
    {
        /// <summary>
        /// Finaliza uma solicitação efetuada.
        /// </summary>
        /// <exception cref="System.ApplicationException">Não existe um processo de carga em andamento [NAO_EXISTE_CARGA_EM_ANDAMENTO].</exception>
        /// <exception cref="System.ApplicationException">O estado atual do processo não permite esta operação [STATUS_NAO_PERMITE_OPERACAO].</exception>
        void FinalizarCarga();

        /// <summary>
        /// Solicita a carga para um conjunto de cartões.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">O total de arquivos solicitados excede o máximo permitido para carga manual de cartões [MAXIMO_CARTOES_CARGA_EXCEDIDO].</exception>
        void SolicitarCarga(DateTime dataProgramacao, IEnumerable<IDadosCargaCartao> cartoes, out int erro, out string mensagem);
    }
}
