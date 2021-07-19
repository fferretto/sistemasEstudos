using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Telenet.Core.Data
{
    /// <summary>
    /// Representa os valores de saída de um comando SQL.
    /// </summary>
    public class OutputValues
    {
        public OutputValues(DbParameterCollection  parameters)
        {
            _output = parameters
                .Cast<DbParameter>()
                .Where(p => p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Output)
                .ToDictionary(p => p.ParameterName, p => p.Value);
        }

        public OutputValues(IDictionary<string, object> parameters)
        {
            _output = parameters;
        }

        private IDictionary<string, object> _output;

        /// <summary>
        /// Verifica se um determinado valor existe nos valores retornados.
        /// </summary>
        public bool HasValue(string name)
        {
            return _output.ContainsKey(name);
        }

        /// <summary>
        /// Obtém um valor de saída.
        /// </summary>
        public object Get(string name)
        {
            if (!HasValue(name))
            {
                return null;
            }

            return _output[name];
        }

        /// <summary>
        /// Obtém os valores de saída.
        /// </summary>
        public IDictionary<string, object> Get()
        {
            return _output;
        }

        /// <summary>
        /// Obtém um valor de saída tipado.
        /// </summary>
        public TValue Get<TValue>(string name)
        {
            return Get(name, default(TValue));
        }

        /// <summary>
        /// Obtém um valor de saída tipado. Caso o valor seja nulo, retorna o valor default passado como parâmetro.
        /// </summary>
        public TValue Get<TValue>(string name, TValue defaultValue)
        {
            if (!HasValue(name))
            {
                return defaultValue;
            }

            var value = _output[name]; 
            return value == null || value == DBNull.Value ? defaultValue : (TValue)value;
        }
    }
}
