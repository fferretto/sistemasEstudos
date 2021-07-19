using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System.Linq;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_CODIGOOCORRENCIAService : ServiceBase<PAGNET_CODIGOOCORRENCIA>, IPAGNET_CODIGOOCORRENCIAService
    {
        private readonly IPAGNET_CODIGOOCORRENCIARepository _rep;

        public PAGNET_CODIGOOCORRENCIAService(IPAGNET_CODIGOOCORRENCIARepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public object[][] GetTiposOcorrencias()
        {
            return _rep.Get(x => x.ATIVO == "S")
                .Select(x => new object[] { x.codOcorrencia, x.nmOcorrencia }).ToArray();
        }
    }
}
