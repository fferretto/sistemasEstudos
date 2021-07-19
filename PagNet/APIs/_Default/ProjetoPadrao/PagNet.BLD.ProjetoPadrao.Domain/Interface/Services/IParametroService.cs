using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using System.Collections.Generic;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Services
{
    public interface IPagNet_Parametro_RelService : IServiceBase<PAGNET_PARAMETRO_REL>
    {
        List<PAGNET_PARAMETRO_REL> GetAllParametrosByRel(int codRel);
    }
}
