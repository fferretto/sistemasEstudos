using Microsoft.EntityFrameworkCore;
using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Context;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;

namespace PagNet.Infra.Data.Repositories
{
    public class PagNet_RelatorioRepository : RepositoryBase<PAGNET_RELATORIO>, IPagNet_RelatorioRepository
    {
        public PagNet_RelatorioRepository(ContextNetCard context)
            : base(context)
        {
            _context = context;
        }

        private readonly ContextNetCard _context;

        public List<dynamic> ExecQueryDinamica(string _query, Dictionary<string, object> Parameters)
        {
            var MyList = CollectionFromSql(_context, _query, Parameters).ToList();

            return MyList;
        }
        private IEnumerable<dynamic> CollectionFromSql(ContextNetCard dbContext, string Sql, Dictionary<string, object> Parameters)
        {
            using (var cmd = dbContext.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = Sql;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                using (var dataReader = cmd.ExecuteReader())
                {

                    while (dataReader.Read())
                    {
                        var dataRow = GetDataRow(dataReader);
                        yield return dataRow;

                    }
                }
            }
        }
        public DataTable ListaDadosRelDataTable(string _query, Dictionary<string, object> Parameters)
        {
            using (var cmd = _context.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = _query;
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();

                foreach (KeyValuePair<string, object> param in Parameters)
                {
                    DbParameter dbParameter = cmd.CreateParameter();
                    dbParameter.ParameterName = param.Key;
                    dbParameter.Value = param.Value;
                    cmd.Parameters.Add(dbParameter);
                }

                DataTable dt = new DataTable();
                //var retObject = new List<dynamic>();
                using (var dataReader = cmd.ExecuteReader())
                {
                    dt.Load(dataReader);
                }

                return dt;
            }
        }

        private dynamic GetDataRow(DbDataReader dataReader)
        {
            var dataRow = new ExpandoObject() as IDictionary<string, object>;
            for (var fieldCount = 0; fieldCount < dataReader.FieldCount; fieldCount++)
                dataRow.Add(dataReader.GetName(fieldCount), dataReader[fieldCount]);
            return dataRow;
        }
    }
}

