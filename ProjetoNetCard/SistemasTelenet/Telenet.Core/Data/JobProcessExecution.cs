using System;

namespace Telenet.Core.Data
{
    public class JobProcessExecution
    {
        public JobProcessExecution()
        {
            ExecuteAt = DateTime.MinValue;
            OnFail = FinishOptions.ExitOnFail;
            OnSuccess = FinishOptions.ExitOnSuccess;
        }

        public DateTime ExecuteAt { get; set; }
        public FinishOptions OnFail { get; set; }
        public FinishOptions OnSuccess { get; set; }
    }
}