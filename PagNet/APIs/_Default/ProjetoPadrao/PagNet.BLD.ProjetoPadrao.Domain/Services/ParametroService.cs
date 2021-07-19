using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Domain.Services.Common;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services;
using System.Collections.Generic;
using System.Linq;
namespace PagNet.BLD.ProjetoPadrao.Domain.Services
{
    public class PagNet_Parametro_RelService : ServiceBase<PAGNET_PARAMETRO_REL>, IPagNet_Parametro_RelService
    {
        private readonly IPagNet_Parametro_RelRepository _rep;

        public PagNet_Parametro_RelService(IPagNet_Parametro_RelRepository rep)
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
