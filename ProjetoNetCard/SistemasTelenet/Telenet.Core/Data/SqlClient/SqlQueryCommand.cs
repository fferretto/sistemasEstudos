#pragma warning disable 1591

using System.Data.Common;
using System.Data.SqlClient;

namespace Telenet.Core.Data.SqlClient
{
    internal class SqlQueryCommand : DbQueryCommandBase
    {
        public SqlQueryCommand(SqlCommand commandBase)
            : base(commandBase, Helper.ParameterPrefixPattern)
        { }

        protected override void ConfigureParameter(DbParameter parameter, string type, int size, byte scale, byte precision)
        {
            Helper.ConfigureType(parameter, type, size, scale, precision);
        }
    }
}

#pragma warning restore 1591
