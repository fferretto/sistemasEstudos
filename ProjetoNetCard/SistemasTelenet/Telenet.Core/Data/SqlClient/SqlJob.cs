#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Telenet.Core.Properties;

namespace Telenet.Core.Data.SqlClient
{
    internal class SqlJob : IJob
    {
        public SqlJob(string name, string description, SqlConnection connection)
        {
            _connection = connection;
            _description = description;
            _name = name;

            var connStrBuilder = new SqlConnectionStringBuilder(_connection.ConnectionString);
            _login = Convert.ToString(connStrBuilder["User ID"] ?? null);
        }

        private SqlConnection _connection;
        private string _description;
        private string _name;
        private string _login;

        private string CreateScript(JobParameters parameters, string script, string login)
        {
            var agendado = parameters.ProcessExecution.ExecuteAt > DateTime.MinValue;

            var template = agendado
                ? Resources.ScheduledJobTemplate
                : Resources.JobTemplate; // Job sem agendamento

            script = template
                .Replace("_JOB_NAME_", _name)
                .Replace("_CREATION_DATE_", DateTime.Now.ToString())
                .Replace("_JOB_DESCRIPTION_", _description)
                .Replace("_OWNER_LOGIN_", login)
                .Replace("_JOB_SCRIPT", script)

                // Parameters
                .Replace("_ENABLED_", (parameters.Enabled ? 1 : 0).ToString())
                .Replace("_EVENT_LOG_", ((int)parameters.NotifyLevel.Eventlog).ToString())
                .Replace("_SEND_EMAIL_", ((int)parameters.NotifyLevel.SendEmail).ToString())
                .Replace("_NET_SEND_", ((int)parameters.NotifyLevel.Netsend).ToString())
                .Replace("_DELETE_LEVEL_", ((int)parameters.NotifyLevel.Delete).ToString())
                .Replace("_ON_SUCCESS_ACTION_", ((int)parameters.ProcessExecution.OnSuccess).ToString())
                .Replace("_ON_FAIL_ACTION_", ((int)parameters.ProcessExecution.OnFail).ToString());

            if (agendado)
            {
                script = script
                    .Replace("_START_DATE_", parameters.ProcessExecution.ExecuteAt.ToString("yyyyMMdd"))
                    .Replace("_START_TIME_", parameters.ProcessExecution.ExecuteAt.ToString("HHmmss"));
            }

            return script;
        }

        public IJob Create(string script)
        {
            return Create(script, new JobParameters());
        }

        public IJob Create(string script, JobParameters parameters)
        {
            script = CreateScript(parameters, script, _login);

            using (new EnsureOpen(_connection))
            using (var command = new SqlCommand(script, _connection))
            {
                command.ExecuteNonQuery();
            }

            return this;
        }

        public void Delete()
        {
            using (new EnsureOpen(_connection))
            using (var command = new SqlCommand("msdb.dbo.sp_delete_job", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@job_name", _name));
                command.ExecuteNonQuery();
            }
        }

        public void DeleteIfExists()
        {
            if (!Exists())
            {
                return;
            }

            Delete();
        }

        public bool Exists()
        {
            return Status() != JobStatus.NotExists;
        }

        public IEnumerable<JobHistory> History()
        {
            var history = new List<JobHistory>();
            var sql = @"select jobhistory.instance_id, jobhistory.job_id, jobhistory.step_id, jobhistory.step_name, jobhistory.sql_message_id, 
jobhistory.sql_severity, jobhistory.message, jobhistory.run_status, jobhistory.run_date, jobhistory.run_time, jobhistory.run_duration, jobhistory.server
from msdb.dbo.sysjobs job
left join msdb.dbo.sysjobhistory jobhistory on (job.job_id = jobhistory.job_id)
where job.name = @name
and jobhistory.step_id > 0
order by jobhistory.step_id";

            using (new EnsureOpen(_connection))
            using (var command = new SqlCommand(sql, _connection))
            {
                command.Parameters.Add(new SqlParameter("@name", _name));

                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var date = Convert.ToString(reader["run_date"]);
                            var time = Convert.ToString(reader["run_time"]).PadLeft(6, '0');
                            var dateTime = new DateTime(Convert.ToInt32(date.Substring(0, 4)),
                                Convert.ToInt32(date.Substring(4, 2)),
                                Convert.ToInt32(date.Substring(6)),
                                Convert.ToInt32(time.Substring(0, 2)),
                                Convert.ToInt32(time.Substring(2, 2)),
                                Convert.ToInt32(time.Substring(4, 2)));

                            history.Add(new JobHistory(
                                Convert.ToInt32(reader["instance_id"]),
                                Convert.ToString(reader["job_id"]),
                                Convert.ToInt32(reader["step_id"]),
                                Convert.ToString(reader["step_name"]),
                                Convert.ToInt32(reader["sql_message_id"]),
                                Convert.ToInt32(reader["sql_severity"]),
                                Convert.ToString(reader["message"]),
                                (JobStatus)Convert.ToInt32(reader["run_status"]),
                                dateTime,
                                Convert.ToInt32(reader["run_duration"]),
                                Convert.ToString(reader["server"])));
                        }
                    }
                }
            }

            return history;
        }

        public JobStatus Status()
        {
            var sql = @"select isnull(jobhistory.run_status, 5) run_status
from msdb.dbo.sysjobs job
left join msdb.dbo.sysjobhistory jobhistory on (job.job_id = jobhistory.job_id)
where job.name = @name
group by job.name, jobhistory.run_status
";

            using (new EnsureOpen(_connection))
            using (var command = new SqlCommand(sql, _connection))
            {
                command.Parameters.Add(new SqlParameter("@name", _name));
                var status = command.ExecuteScalar();
                return status == null ? JobStatus.NotExists : (JobStatus)Convert.ToInt32(status);
            }
        }

        public IJob Start()
        {
            using (new EnsureOpen(_connection))
            using (var command = new SqlCommand("msdb.dbo.sp_start_job", _connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.Add(new SqlParameter("@job_name", _name));
                command.ExecuteNonQuery();
            }

            return this;
        }

        public IJob Start(string login)
        {
            if (login != _login)
            {
                using (new EnsureOpen(_connection))
                using (var command = new SqlCommand("msdb.dbo.sp_update_job", _connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@job_name", _name));
                    command.Parameters.Add(new SqlParameter("@owner_login_name", login));
                    command.ExecuteNonQuery();
                    _login = login;
                }
            }

            return Start();
        }
    }
}

#pragma warning restore 1591
