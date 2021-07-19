#pragma warning disable 1591

using System.Data;
using System.Data.SqlClient;

namespace Telenet.Core.Data.SqlClient
{
    public class SqlDbClient : DbClient
    {
        public SqlDbClient(string connectionString)
            : base(connectionString)
        { }

        private SqlConnection _connection;

        private SqlCommand CreateSqlCommand(string sql)
        {
            var command = _connection.CreateCommand();
            command.CommandText = sql;
            return command;
        }

        protected override void CreateConnection(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public override IExecuteCommand Command(string sql)
        {
            return new SqlExecuteCommand(CreateSqlCommand(sql));
        }

        public override IDbCommand CreateCommand(string sql)
        {
            return CreateSqlCommand(sql);
        }

        public override void FinishAsyncResult(string key)
        {
            Helper.FinishAsyncResult(key);
        }

        public override ICommandAsyncResult GetAsyncResult(string key)
        {
            return Helper.GetAsyncResult(key);
        }

        public override IJob Job(string name, string description = null)
        {
            return new SqlJob(name, description, _connection);
        }

        public override IQueryCommand Query(string sql)
        {
            return new SqlQueryCommand(CreateSqlCommand(sql));
        }

        public override void RemoveAsyncResult(string key)
        {
            Helper.RemoveAsyncResult(key);
        }

        public override IStoredProcedureCommand StoredProcedure(string sql)
        {
            return new SqlStoredProcedureCommand(CreateSqlCommand(sql));
        }
    }
}

#pragma warning restore 1591
