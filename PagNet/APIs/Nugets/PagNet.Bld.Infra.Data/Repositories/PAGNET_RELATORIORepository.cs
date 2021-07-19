using Microsoft.EntityFrameworkCore;
using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Infra.Data.ContextDados;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using PagNet.Bld.Infra.Data.Repositories.Common;

namespace PagNet.Bld.Infra.Data.Repositories
{
    public class PAGNET_RELATORIORepository : RepositoryBase<PAGNET_RELATORIO>, IPAGNET_RELATORIORepository
    {
        public PAGNET_RELATORIORepository(ContextPagNet context)
            : base(context)
        {
            _context = context;
        }

        private readonly ContextPagNet _context;

        public List<dynamic> ExecQueryDinamica(string _query, Dictionary<string, object> Parameters)
        {
            var MyList = CollectionFromSql(_context, _query, Parameters).ToList();

            return MyList;
        }
        private IEnumerable<dynamic> CollectionFromSql(ContextPagNet dbContext, string Sql, Dictionary<string, object> Parameters)
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

    public class PAGNET_RELATORIO_RESULTADORepository : RepositoryBase<PAGNET_RELATORIO_RESULTADO>, IPAGNET_RELATORIO_RESULTADORepository
    {
        public PAGNET_RELATORIO_RESULTADORepository(ContextPagNet context)
            : base(context)
        { }
    }

    public class PAGNET_RELATORIO_PARAM_UTILIZADORepository : RepositoryBase<PAGNET_RELATORIO_PARAM_UTILIZADO>, IPAGNET_RELATORIO_PARAM_UTILIZADORepository
    {
        public PAGNET_RELATORIO_PARAM_UTILIZADORepository(ContextPagNet context)
            : base(context)
        { }

        protected override int GetInitialSeed()
        {
            return DbNetCard.PAGNET_RELATORIO_PARAM_UTILIZADO.Select(p => p.COD_PARAM_UTILIZADO).DefaultIfEmpty(0).Max();
        }
    }

    public class PAGNET_RELATORIO_STATUSRepository : RepositoryBase<PAGNET_RELATORIO_STATUS>, IPAGNET_RELATORIO_STATUSRepository
    {
        public PAGNET_RELATORIO_STATUSRepository(ContextPagNet context)
            : base(context)
        { }

    }
}

