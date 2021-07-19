#pragma warning disable 1591

using System.Collections.Generic;
using System.Data;

namespace Telenet.Core.Data
{
    public abstract class ObjectLoader<TRecord> : IObjectLoader<TRecord> where TRecord : new()
    {
        protected abstract void LoadInstance(IDataRecord record, TRecord instance);

        public virtual IEnumerable<TRecord> Load(IDataReader reader)
        {
            var resultset = new List<TRecord>();

                while (reader.Read())
                {
                    var instance = new TRecord();
                    LoadInstance(reader, instance);
                    resultset.Add(instance);
                }

            return resultset;
        }
    }
}

#pragma warning restore 1591
