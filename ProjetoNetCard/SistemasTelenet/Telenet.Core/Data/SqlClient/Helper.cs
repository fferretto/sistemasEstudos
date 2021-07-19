#pragma warning disable 1591

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;

namespace Telenet.Core.Data.SqlClient
{
    internal static class Helper
    {
        private const string AsynchronousProcessingFlag = "Asynchronous Processing=true";
        private static object _sync = new object();
        private static IDictionary<string, AsyncControl> _cache = new Dictionary<string, AsyncControl>();

        public const string ParameterPrefixPattern = "@\\w+";

        public static void CallbackHandler(IAsyncResult asyncResult)
        {
            var asyncControl = asyncResult.AsyncState as AsyncControl;
            var rowsAffected = 0;

            var command = asyncControl.Command;
            rowsAffected = command.EndExecuteNonQuery(asyncResult);

            if (command.Connection.State == ConnectionState.Open)
            {
                command.Connection.Close();
                command.Connection.Dispose();
            }

            asyncControl.Finished = true;

            if (asyncControl.Callback != null)
            {
                asyncControl.Callback(rowsAffected, new OutputValues(asyncControl.Command.Parameters));
                RemoveAsyncResult(asyncControl.Key);
            }
        }

        public static void ConfigureType(DbParameter parameter, string type, int size, byte scale, byte precision)
        {
            var sqlParameter = (parameter as SqlParameter);
            sqlParameter.Size = size;
            sqlParameter.Scale = scale;
            sqlParameter.Precision = precision;

            sqlParameter.SqlDbType = (type == "NUMERIC")
                ? SqlDbType.Decimal
                : (SqlDbType)Enum.Parse(typeof(SqlDbType), type, true);
        }

        public static SqlConnection CreateAsynchronousConnection(DbConnection connection)
        {
            return new SqlConnection(string.Concat(connection.ConnectionString,
                connection.ConnectionString.EndsWith(";")
                    ? AsynchronousProcessingFlag
                    : string.Concat(";", AsynchronousProcessingFlag)));
        }

        public static void FinishAsyncResult(string key)
        {
            var asyncResult = Helper.GetAsyncResult(key);

            if (asyncResult == null)
            {
                return;
            }

            (asyncResult as AsyncControl).Finished = true;
        }

        public static ICommandAsyncResult GetAsyncResult(string key)
        {
            key = key.ToUpper();

            if (!_cache.ContainsKey(key))
            {
                return null;
            }

            var result = _cache[key] as AsyncControl;
            return new SqlCommandAsyncResult(key, result.Command, result.Finished);
        }

        public static void RemoveAsyncResult(string key)
        {
            var result = GetAsyncResult(key);

            if (result == null)
            {
                return;
            }

            if (!result.Finished)
            {
                throw new InvalidOperationException("The async command provided are still running.");
            }

            lock (_sync)
            {
                _cache.Remove(key);
            }
        }

        public static void SaveAyncResult(string key, AsyncControl control)
        {
            key = key.ToUpper();

            if (_cache.ContainsKey(key))
            {
                if (!_cache[key].Finished)
                {
                    throw new InvalidOperationException("Attempt to insert a new value with a key for a unfinished async command.");
                }

                _cache.Remove(key);
            }

            lock (_sync)
            {
                _cache.Add(key, control);
            }
        }
    }
}

#pragma warning restore 1591
