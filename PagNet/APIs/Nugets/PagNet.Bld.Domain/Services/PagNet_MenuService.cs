using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_MENUService : ServiceBase<PAGNET_MENU>, IPAGNET_MENUService
    {
        private readonly IPAGNET_MENURepository _rep;

        public PAGNET_MENUService(IPAGNET_MENURepository rep)
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
