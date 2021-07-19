#pragma warning disable 1591

using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace Telenet.Core.Data.SqlClient
{
    internal class SqlStoredProcedureCommand : DbStoredProcedureCommand
    {
        public SqlStoredProcedureCommand(SqlCommand commandBase)
            : base(commandBase, Helper.ParameterPrefixPattern)
        { }

        private void ExecuteAsync(string key, Action<int, OutputValues> callback, params object[] values)
        {
            CommandBase.Connection = Helper.CreateAsynchronousConnection(CommandBase.Connection);
            BindParameters(values);

            var asyncControl = new AsyncControl(string.IsNullOrEmpty(key)
                ? Guid.NewGuid().ToString() : key,
                callback, CommandBase as SqlCommand);
            Helper.SaveAyncResult(asyncControl.Key, asyncControl);

            new EnsureOpen(CommandBase.Connection);
            (CommandBase as SqlCommand).BeginExecuteNonQuery(Helper.CallbackHandler, asyncControl);
        }

        protected override void ConfigureParameter(DbParameter parameter, string type, int size, byte scale, byte precision)
        {
            Helper.ConfigureType(parameter, type, size, scale, precision);
        }

        public override void ExecuteAsync(Action<int, OutputValues> callback, params object[] values)
        {
            ExecuteAsync(Guid.NewGuid().ToString(), callback, values);
        }

        public override void ExecuteAsync(string key, params object[] values)
        {
            ExecuteAsync(key, null, values);
        }
    }
}

#pragma warning restore 1591
