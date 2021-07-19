
namespace Telenet.Core.Data
{
    /// <summary>
    /// Define os possíveis staus de um job no banco de dados.
    /// </summary>
    public enum JobStatus
    {
        /// <summary>
        /// Job foi executado e finalizado com erro.
        /// </summary>
        Failed = 0,

        /// <summary>
        /// Job foi executado e finalizado com sucesso.
        /// </summary>
        Success = 1,
        
        /// <summary>
        /// Job foi executado mas solicita uma nova execução.
        /// </summary>
        Retry = 2,
        
        /// <summary>
        /// Execução do job foi cancelado.
        /// </summary>
        Cancelled = 3,
        
        /// <summary>
        /// Job extá em execução.
        /// </summary>
        Running = 4,
        
        /// <summary>
        /// Joi foi criado mas a execução ainda não foi iniciada.
        /// </summary>
        Created = 5,
        
        /// <summary>
        /// Job não existe.
        /// </summary>
        NotExists = 6
    }
}
