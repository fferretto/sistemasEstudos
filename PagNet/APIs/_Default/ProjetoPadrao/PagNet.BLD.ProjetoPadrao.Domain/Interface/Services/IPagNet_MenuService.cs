using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using System.Collections.Generic;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Services
{
    public interface IPagNet_MenuService : IServiceBase<PAGNET_MENU>
    {
        List<PAGNET_MENU> ListaMenusAtivos();
    }
}
