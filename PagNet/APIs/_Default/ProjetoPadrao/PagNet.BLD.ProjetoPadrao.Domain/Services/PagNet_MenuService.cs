using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Domain.Services.Common;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PagNet.BLD.ProjetoPadrao.Domain.Services
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
