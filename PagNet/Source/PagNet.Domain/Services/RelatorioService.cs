using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Domain.Interface.Services;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace PagNet.Domain.Services
{
    public class PagNet_RelatorioService : ServiceBase<PAGNET_RELATORIO>, IPagNet_RelatorioService
    {
        private readonly IPagNet_RelatorioRepository _rep;

        public PagNet_RelatorioService(IPagNet_RelatorioRepository rep)
            : base(rep)
        {
            _rep = rep;
        }
        public List<PAGNET_RELATORIO> GetAllRelatorios()
        {
            return _rep.GetAll().ToList();
        }
        public PAGNET_RELATORIO GetRelatorioByID(int codRel)
        {
            var dados = _rep.Get(x => x.ID_REL == codRel, "PAGNET_PARAMETRO_REL").FirstOrDefault();

            return dados;
        }

        public List<dynamic> GetDadosRelatorio(string _query, Dictionary<string, object> Parameters)
        {
            var MyList = _rep.ExecQueryDinamica(_query, Parameters).ToList();

            return MyList;
        }    
        public DataTable ListaDadosRelDataTable(string _query, Dictionary<string, object> Parameters)
        {
            var MyList = _rep.ListaDadosRelDataTable(_query, Parameters);

            return MyList;
        }
    }
}
