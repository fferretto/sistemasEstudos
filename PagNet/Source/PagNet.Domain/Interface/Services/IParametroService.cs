using PagNet.Domain.Entities;
using System.Collections.Generic;

namespace PagNet.Domain.Interface.Services
{
    public interface IPagNet_Parametro_RelService : IServiceBase<PAGNET_PARAMETRO_REL>
    {
        List<PAGNET_PARAMETRO_REL> GetAllParametrosByRel(int codRel);
    }
}
