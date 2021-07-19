#pragma warning disable 1591

using System;
using System.Data;

namespace Telenet.Core.Data
{
    public abstract class DbClient : IDbClient
    {
        protected DbClient(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            CreateConnection(connectionString);
        }

        protected abstract void CreateConnection(string connectionString);

        public abstract IExecuteCommand Command(string sql);

        public abstract IDbCommand CreateCommand(string sql);

        public abstract void FinishAsyncResult(string key);

        public abstract ICommandAsyncResult GetAsyncResult(string key);

        public abstract IJob Job(string name, string description = null);
        
        public abstract IQueryCommand Query(string sql);

        public abstract void RemoveAsyncResult(string key);

        public abstract IStoredProcedureCommand StoredProcedure(string sql);
    }
}

#pragma warning restore 1591