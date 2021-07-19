using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Collections.Generic;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_PARAMETRO_RELService : IServiceBase<PAGNET_PARAMETRO_REL>
    {
        List<PAGNET_PARAMETRO_REL> GetAllParametrosByRel(int codRel);
    }
}
