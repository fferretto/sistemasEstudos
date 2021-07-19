namespace Telenet.Core.Data
{
    public class JobNotifyLevel
    {
        public JobNotifyLevel()
        {
            Eventlog = NotifyLevelOptions.Always;
            SendEmail = NotifyLevelOptions.Never;
            Netsend = NotifyLevelOptions.Never;
            Delete = NotifyLevelOptions.OnSuccess;
        }

        public NotifyLevelOptions Eventlog { get; set; }
        public NotifyLevelOptions SendEmail { get; set; }
        public NotifyLevelOptions Netsend { get; set; }
        public NotifyLevelOptions Delete { get; set; }
    }
}