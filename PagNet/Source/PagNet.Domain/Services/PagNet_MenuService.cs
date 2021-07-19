using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Repository;
using PagNet.Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PagNet.Domain.Services
{
    public class PagNet_MenuService : ServiceBase<PAGNET_MENU>, IPagNet_MenuService
    {
        private readonly IPagNet_MenuRepository _rep;

        public PagNet_MenuService(IPagNet_MenuRepository rep)
            : base(rep)
        {
            _rep = rep;
        }
        public List<PAGNET_MENU> ListaMenusAtivos()
        {
            try
            {
                var menus = _rep.Get(x => x.Ativo == "S").OrderBy(t => t.Ordem).ToList();

                return menus;
            }
            catch(Exception ex)
            {
                throw new Exception($"Message: {ex.Message}\nConnectionString: {_rep.Connectionstring}", ex);
            }
        }
    }
}
