using PagNet.Domain.Entities;
using System.Collections.Generic;

namespace PagNet.Domain.Interface.Services
{
    public interface IPagNet_MenuService : IServiceBase<PAGNET_MENU>
    {
        List<PAGNET_MENU> ListaMenusAtivos();
    }
}
