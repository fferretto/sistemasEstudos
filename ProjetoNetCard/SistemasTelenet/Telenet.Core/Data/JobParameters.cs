namespace Telenet.Core.Data
{
    public class JobParameters
    {
        public JobParameters()
            : this(true, new JobNotifyLevel(), new JobProcessExecution())
        { }

        public JobParameters(bool enabled, JobNotifyLevel notifyLevel, JobProcessExecution processExecution)
        {
            Enabled = enabled;
            NotifyLevel = notifyLevel;
            ProcessExecution = processExecution;
        }

        public bool Enabled { get; set; }
        public JobNotifyLevel NotifyLevel { get; set; }
        public JobProcessExecution ProcessExecution { get; set; }
    }
}
