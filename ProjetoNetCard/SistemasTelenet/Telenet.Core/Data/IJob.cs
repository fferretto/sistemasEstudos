using System.Collections.Generic;

namespace Telenet.Core.Data
{
    /// <summary>
    /// Define a interface de implementação de um agente de controle de jobs no banco.
    /// </summary>
    public interface IJob
    {
        /// <summary>
        /// Cria o job no banco de dados.
        /// </summary>
        IJob Create(string script);

        /// <summary>
        /// Cria o job no banco de dados.
        /// </summary>
        IJob Create(string script, JobParameters parameters);

        /// <summary>
        /// Excluit o job no banco de dados.
        /// </summary>
        void Delete();

        /// <summary>
        /// Excluit o job no banco de dados.
        /// </summary>
        void DeleteIfExists();

        /// <summary>
        /// Verifica se o job existe no banco de dados.
        /// </summary>
        bool Exists();

        /// <summary>
        /// Obtém o histórico da execução do job no banco de dados.
        /// </summary>
        IEnumerable<JobHistory> History();

        /// <summary>
        /// Verifica se um job está sendo executado no banco de dados.
        /// </summary>
        JobStatus Status();

        /// <summary>
        /// Inicia a execução de um job no banco de dados.
        /// </summary>
        IJob Start();

        /// <summary>
        /// Inicia a execução de um job no banco de dados.
        /// </summary>
        IJob Start(string login);
    }
}
