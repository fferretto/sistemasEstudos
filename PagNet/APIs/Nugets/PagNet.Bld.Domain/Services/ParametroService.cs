using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System.Collections.Generic;
using System.Linq;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_PARAMETRO_RELService : ServiceBase<PAGNET_PARAMETRO_REL>, IPAGNET_PARAMETRO_RELService
    {
        private readonly IPAGNET_PARAMETRO_RELRepository _rep;

        public PAGNET_PARAMETRO_RELService(IPAGNET_PARAMETRO_RELRepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public List<PAGNET_PARAMETRO_REL> GetAllParametrosByRel(int codRel)
        {
            return _rep.Get( x => x.ID_REL == codRel).OrderBy(y => y.ORDEM_TELA).ToList();
        }
    }
}
