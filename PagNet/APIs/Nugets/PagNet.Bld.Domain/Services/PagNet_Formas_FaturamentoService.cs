using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System.Linq;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_FORMAS_FATURAMENTOService : ServiceBase<PAGNET_FORMAS_FATURAMENTO>, IPAGNET_FORMAS_FATURAMENTOService
    {
        private readonly IPAGNET_FORMAS_FATURAMENTORepository _rep;

        public PAGNET_FORMAS_FATURAMENTOService(IPAGNET_FORMAS_FATURAMENTORepository rep)
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
