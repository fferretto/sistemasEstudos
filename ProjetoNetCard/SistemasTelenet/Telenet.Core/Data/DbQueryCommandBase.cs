#pragma warning disable 1591

using System.Collections.Generic;
using System.Data.Common;

namespace Telenet.Core.Data
{
    public abstract class DbQueryCommandBase : DbSpecializedCommandBase, IQueryCommand
    {
        protected DbQueryCommandBase(DbCommand commandBase, string parameterPrefixPattern)
            : base(commandBase, parameterPrefixPattern)
        { }

        public TValue GetScalar<TValue>(params object[] values)
        {
            return GetScalar(default(TValue), values);
        }

        public TValue GetScalar<TValue>(TValue defaultValue, params object[] values)
        {
            using (new EnsureOpen(CommandBase.Connection))
            {
                BindParameters(values);
                var value = CommandBase.ExecuteScalar();
                return value == null ? defaultValue : (TValue)value;
            }
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
    }
}

#pragma warning restore 1591