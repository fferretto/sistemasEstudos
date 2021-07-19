using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Domain.Services.Common;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services;
using System.Linq;

namespace PagNet.BLD.ProjetoPadrao.Domain.Services
{
    public class PagNet_Formas_FaturamentoService : ServiceBase<PAGNET_FORMAS_FATURAMENTO>, IPagNet_Formas_FaturamentoService
    {
        private readonly IPAGNET_FORMAS_FATURAMENTORepository _rep;

        public PagNet_Formas_FaturamentoService(IPAGNET_FORMAS_FATURAMENTORepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public string GetFormaFaturamentoById(int codigo)
        {
            var dados = _rep.Get(x => x.CODFORMAFATURAMENTO == codigo).FirstOrDefault();
            return dados.NMFORMAFATURAMENTO.Trim();
        }

        public object[][] GetHashFormasFaturamento()
        {
            return _rep.Get(x => x.ATIVO == "S")
                .Select(x => new object[] { x.CODFORMAFATURAMENTO, x.NMFORMAFATURAMENTO }).ToArray();
        }
    }
}
