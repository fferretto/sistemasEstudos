using PagNet.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Domain.Interface.Services
{
    public interface IOperadoraService : IServiceBase<OPERADORA>
    {
        IEnumerable<OPERADORA> GetAllOperadora();
        Task<OPERADORA> GetOperadoraById(int id);
        object[][] GetHashOperadora();

    }
}
