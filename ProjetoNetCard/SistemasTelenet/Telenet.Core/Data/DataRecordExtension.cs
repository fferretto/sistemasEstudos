using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Telenet.Core.Data
{
    /// <summary>
    /// Extensões funcionais para o tipo System.Data.Common.DbDataRecord.
    /// </summary>
    public static class DataRecordExtension
    {
        private const string DefaultDateFormat = "dd/MM/yyyy";

        private static TValue GetValue<TValue>(object value, TValue defaultValue)
        {
            if (typeof(TValue).IsEnum)
            {
                return (TValue)Enum.Parse(typeof(TValue), Convert.ToString(value), true);
            }

            switch (Type.GetTypeCode(typeof(TValue)))
            {
                case TypeCode.DBNull:
                case TypeCode.Empty:
                    return defaultValue;
                case TypeCode.Boolean:
                case TypeCode.DateTime:
                case TypeCode.Object:
                    throw new InvalidCastException();
                case TypeCode.Byte:
                case TypeCode.Char:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.String:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                default:
                    var val = Convert.ToString(value);
                    return string.IsNullOrEmpty(val) ? defaultValue : (TValue)Convert.ChangeType(val, typeof(TValue));
            }
        }

        /// <summary>
        /// Lê um valor específico no registro e o converte para o tipo apropriado.
        /// </summary>
        public static TValue GetValue<TValue>(this IDataReader reader, string fieldname)
        {
            return reader.GetValue<TValue>(fieldname, default(TValue));
        }

        /// <summary>
        /// Lê um valor específico no registro e o converte para o tipo apropriado.
        /// </summary>
        public static TValue GetValue<TValue>(this IDataReader reader, string fieldname, TValue defaultValue)
        {
            if (string.IsNullOrEmpty(fieldname))
            {
                return defaultValue;
            }

            var value = reader[fieldname];

            if (value == DBNull.Value)
            {
                return defaultValue;
            }

            return (TValue)GetValue<TValue>(value, defaultValue);
        }

        /// <summary>
        /// Lê um valor específico no registro que representa uma data.
        /// </summary>
        public static DateTime GetDTValue(this IDataReader reader, string fieldname, string format = DefaultDateFormat)
        {
            return reader.GetDTValue(fieldname, DateTime.MinValue, format);
        }

        /// <summary>
        /// Lê um valor específico no registro que representa uma data.
        /// </summary>
        public static DateTime GetDTValue(this IDataReader reader, string fieldname, DateTime defaultValue, string format = DefaultDateFormat)
        {
            var value = reader[fieldname];

            if (value == DBNull.Value)
            {
                return defaultValue;
            }

            if (string.IsNullOrEmpty(format))
            {
                format = DefaultDateFormat;
            }

            return DateTime.ParseExact(Convert.ToString(value), format, null);
        }

        /// <summary>
        /// Lê um valor específico no registro e o converte para o tipo nullable apropriado.
        /// </summary>
        public static Nullable<TValue> GetNValue<TValue>(this IDataReader reader, string fieldname)
            where TValue : struct
        {
            var value = reader[fieldname];

            if (value == null || value == DBNull.Value)
            {
                return new Nullable<TValue>();
            }

            return new Nullable<TValue>(GetValue<TValue>(value, default(TValue)));
        }

        /// <summary>
        /// Verifica se o reader tem algum dos campos passados como parâmetro.
        /// </summary>
        public static bool HasField(this IDataReader reader, string fieldname)
        {
            return reader
                .GetSchemaTable()
                .Rows
                .Cast<DataRow>()
                .Select(r => Convert.ToString(r[0]))
                .Any(f => f.Equals(fieldname, StringComparison.InvariantCultureIgnoreCase));
        }

        /// <summary>
        /// Cria uma lista de retistros a partir de um IDataReader.
        /// </summary>
        public static IEnumerable<TRecord> ToEnumerable<TRecord>(this IDataReader reader)
            where TRecord : ILoadableObject, new()
        {
            var resultset = new List<TRecord>();

            while (reader.Read())
            {
                var instance = new TRecord();
                instance.LoadFrom(reader);
                resultset.Add(instance);
            }

            return resultset;
        }

        /// <summary>
        /// Cria uma lista de retistros a partir de um IDataReader e um loader especializado.
        /// </summary>
        public static IEnumerable<TRecord> ToEnumerable<TRecord, TLoader>(this IDataReader reader)
            where TRecord : new()
            where TLoader : IObjectLoader<TRecord>, new()
        {
            return new TLoader().Load(reader);
        }
    }
}
