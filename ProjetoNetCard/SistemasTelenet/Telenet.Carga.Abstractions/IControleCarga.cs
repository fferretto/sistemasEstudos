using System.Collections.Generic;

namespace Telenet.Carga.Abstractions
{
    /// <summary>
    /// Define o contrato de implementação de um leitor do resumo de uma carga.
    /// </summary>
    public interface IControleCarga
    {
        /// <summary>
        /// Consulta os parâmetros definidos para uma solicitação de carga em andamento.
        /// </summary>
        /// <exception cref="System.ApplicationException">Não existe um processo de carga em andamento [NAO_EXISTE_CARGA_EM_ANDAMENTO].</exception>
        IDadosCarga ObterDadosCarga();

        /// <summary>
        /// Obtém o resumo do processo de carga atual.
        /// </summary>
        /// <exception cref="System.ApplicationException">Não existe um processo de carga em andamento [NAO_EXISTE_CARGA_EM_ANDAMENTO].</exception>
        IEnumerable<IResumoCarga> ObterResumoCarga();
    }
}
