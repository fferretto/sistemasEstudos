using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Collections.Generic;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_MENUService : IServiceBase<PAGNET_MENU>
    {
        List<PAGNET_MENU> ListaMenusAtivos();
    }
}
