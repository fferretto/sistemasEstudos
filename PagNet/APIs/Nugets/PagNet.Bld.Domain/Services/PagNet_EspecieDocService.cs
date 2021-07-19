using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_ESPECIEDOCService : ServiceBase<PAGNET_ESPECIEDOC>, IPAGNET_ESPECIEDOCService
    {
        private readonly IPAGNET_ESPECIEDOCRepository _rep;

        public PAGNET_ESPECIEDOCService(IPAGNET_ESPECIEDOCRepository rep)
            : base(rep)
        {
            _rep = rep;
        }

    }
}
