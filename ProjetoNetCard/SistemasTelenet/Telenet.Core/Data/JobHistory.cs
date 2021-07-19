using System;

namespace Telenet.Core.Data
{
    /// <summary>
    /// Representa um entrada do histórico da execução de um job no banco de dados.
    /// </summary>
    public struct JobHistory
    {
        internal JobHistory(int instanceId, string jobId, int stepId, string stepName, int messageId, int severity, string message,
            JobStatus status, DateTime dateTime, int duration, string server)
        {
            _dateTime = dateTime;
            _duration = duration;
            _instanceId = instanceId;
            _jobId = jobId;
            _message = message;
            _messageId = messageId;
            _server = server;
            _severity = severity;
            _status = status;
            _stepName = stepName;
            _stepId = stepId;
        }

        private DateTime _dateTime;
        private int _duration;
        private int _instanceId;
        private string _jobId;
        private string _message;
        private int _messageId;
        private string _server;
        private int _severity;
        private JobStatus _status;
        private string _stepName;
        private int _stepId;

        /// <summary>
        /// Obtém a data e hora da execução do passo do job.
        /// </summary>
        public DateTime DateTime { get { return _dateTime; } }

        /// <summary>
        /// Obtém a duração da execução do passo do job.
        /// </summary>
        public int Duration { get { return _duration; } }

        /// <summary>
        /// Obtém o identificador da instância da execução do job.
        /// </summary>
        public int InstanceId { get { return _instanceId; } }

        /// <summary>
        /// Obtém o identificador do job.
        /// </summary>
        public string JobId { get { return _jobId; } }

        /// <summary>
        /// Obtém a mensagem retornada pela execução.
        /// </summary>
        public string Message { get { return _message; } }

        /// <summary>
        /// Obtém o identificador da mensagem retornada pela execução.
        /// </summary>
        public int MessageId { get { return _messageId; } }

        /// <summary>
        /// Obtém o nome do servidor onde o job foi executado.
        /// </summary>
        public string Server { get { return _server; } }

        /// <summary>
        /// Obtém a severidade da mensagem para a execução.
        /// </summary>
        public int Severity { get { return _severity; } }

        /// <summary>
        /// Obtém o status da execução.
        /// </summary>
        public JobStatus Status { get { return _status; } }

        /// <summary>
        /// Obtém o nome do passo executado.
        /// </summary>
        public string StepName { get { return _stepName; } }

        /// <summary>
        /// Obtém o identificador do passo execudado.
        /// </summary>
        public int StepId { get { return _stepId; } }
    }
}
