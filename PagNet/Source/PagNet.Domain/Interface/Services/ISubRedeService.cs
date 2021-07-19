using PagNet.Domain.Entities;
using System.Threading.Tasks;

namespace PagNet.Domain.Interface.Services
{
    public interface ISubRedeService : IServiceBase<SUBREDE>
    {
        object[][] GetSubRede();
        object[][] BuscaSubRedeByID(int codSubRede);

        
        Task<SUBREDE> GetSubRedeByID(int codSubRede);
    }
}
