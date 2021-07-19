using System;
using System.Data;
using System.Data.SqlClient;

namespace SIL
{
    /// <summary>
    /// Summary description for DbConnectionExtensions
    /// </summary>
    public static class DbConnectionExtensions
    {
        private static SqlCommand CreateCommand(this SqlConnection self, string sql, params object[] parameters)
        {
            var command = self.CreateCommand();
            command.CommandText = sql;
            command.CommandType = CommandType.Text;

            for (int i = 0; i < parameters.Length; i++)
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = "@" + (i + 1).ToString();
                parameter.Value = parameters[i] == null ? DBNull.Value : parameters[i];
                command.Parameters.Add(parameter);
            }

            return command;
        }

        public static int Execute(this SqlConnection self, string sql, params object[] parameters)
        {
            using (var command = self.CreateCommand(sql, parameters))
            {
                self.Open();
                try
                {
                    return command.ExecuteNonQuery();
                }
                finally
                {
                    self.Close();
                }
            }
        }

        public static TValue ExecuteScalar<TValue>(this SqlConnection self, TValue defaultValue, string sql, params object[] parameters)
        {
            using (var command = self.CreateCommand(sql, parameters))
            {
                self.Open();
                try
                {
                    var returnValue = command.ExecuteScalar();
                    return returnValue == null ? defaultValue : (TValue)returnValue;
                }
                finally
                {
                    self.Close();
                }
            }
        }
    }
}
