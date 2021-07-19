#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Telenet.Core.Data
{
    public abstract class DbStoredProcedureCommand : DbSpecializedCommandBase, IStoredProcedureCommand
    {
        protected DbStoredProcedureCommand(DbCommand commandBase, string parameterPrefixPattern)
            : base(commandBase, parameterPrefixPattern)
        { }

        protected override void CreateParameters()
        {
            base.CreateParameters();
            var indexOf = CommandBase.CommandText.IndexOf(" ");
            var stdProcName = indexOf > -1 ? CommandBase.CommandText.Substring(0, CommandBase.CommandText.IndexOf(" ")) : CommandBase.CommandText;
            CommandBase.CommandText = stdProcName;
            CommandBase.CommandType = System.Data.CommandType.StoredProcedure;
        }

        public int Execute(params object[] values)
        {
            using (new EnsureOpen(CommandBase.Connection))
            {
                BindParameters(values);
                return CommandBase.ExecuteNonQuery();
            }
        }

        public abstract void ExecuteAsync(Action<int, OutputValues> callback, params object[] values);

        public abstract void ExecuteAsync(string key, params object[] values);

        public TValue Execute<TValue>(params object[] values)
        {
            using (new EnsureOpen(CommandBase.Connection))
            {
                BindParameters(values);
                CommandBase.ExecuteNonQuery();
            }

            return GetReturnValue<TValue>();
        }

        public IEnumerable<TRecord> GetData<TRecord>(params object[] values)
            where TRecord : ILoadableObject, new()
        {
            using (new EnsureOpen(CommandBase.Connection))
            {
                BindParameters(values);
                return CommandBase.ExecuteReader().ToEnumerable<TRecord>();
            }
        }

        public IEnumerable<TRecord> GetData<TRecord, TLoader>(params object[] values)
            where TRecord : new()
            where TLoader : IObjectLoader<TRecord>, new()
        {
            using (new EnsureOpen(CommandBase.Connection))
            {
                BindParameters(values);
                return CommandBase.ExecuteReader().ToEnumerable<TRecord, TLoader>();
            }
        }

        public OutputValues GetOutputs()
        {
            return new OutputValues(CommandBase.Parameters);
        }
    }
}

#pragma warning restore 1591