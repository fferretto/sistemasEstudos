using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IOPERADORAService : IServiceBase<OPERADORA>
    {
        IEnumerable<OPERADORA> GetAllOperadora();
        Task<OPERADORA> GetOperadoraById(int id);
        object[][] GetHashOperadora();

    }
}
