using System;
using System.Collections.Generic;

namespace Telenet.Carga.Abstractions
{
    /// <summary>
    /// Define o contrato do serviço de solicitação de carga de cartões via arquivo. Esta operação realiza a carga em massa de valor 
    /// limite de utilização para um conjunto de cartões.
    /// </summary>
    public interface ISolicitacaoCargaArquivo : IControleCarga
    {
        /// <summary>
        /// Cancela o processo atual de solicitação de carga.
        /// </summary>
        /// <exception cref="System.ApplicationException">Não existe um processo de carga em andamento [NAO_EXISTE_CARGA_EM_ANDAMENTO].</exception>
        void CancelarCarga();

        /// <summary>
        /// Finaliza uma solicitação efetuada.
        /// </summary>
        /// <exception cref="System.ApplicationException">Não existe um processo de carga em andamento [NAO_EXISTE_CARGA_EM_ANDAMENTO].</exception>
        /// <exception cref="System.ApplicationException">O estado atual do processo não permite esta operação [STATUS_NAO_PERMITE_OPERACAO].</exception>
        void FinalizarCarga();

        /// <summary>
        /// Consulta o status de um processo de solicitação de carga.
        /// </summary>
        IStatusCarga ObterStatusCarga();

        /// <summary>
        /// Efetiva a atualização dos limites de utilização dos cartões para uma carga verificada.
        /// </summary>
        /// <exception cref="System.ApplicationException">Não existe um processo de carga em andamento [NAO_EXISTE_CARGA_EM_ANDAMENTO].</exception>
        /// <exception cref="System.ApplicationException">O estado atual do processo não permite esta operação [STATUS_NAO_PERMITE_OPERACAO].</exception>
        void SolicitarCarga();

        /// <summary>
        /// Valida um arquivo de solicitação de carga. 
        /// </summary>
        /// <exception cref="System.ApplicationException">Já existe um processo de carga em andamento [JA_EXISTE_PROCESSO].</exception>
        /// <exception cref="System.IO.FileNotFoundException">Arquivo de carga não foi encontrado [ARQUIVO_CARGA_NAO_ENCONTRADO].</exception>
        void ValidarArquivo(DateTime dataProgramacao, string nomeCompletoArquivoCarga, string nomeOriginalArquivo, bool validarCpf);
    }
}
