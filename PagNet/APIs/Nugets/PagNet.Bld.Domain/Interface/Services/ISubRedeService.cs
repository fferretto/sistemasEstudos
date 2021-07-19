using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Threading.Tasks;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface ISUBREDEService : IServiceBase<SUBREDE>
    {
        object[][] GetSubRede();
        object[][] BuscaSubRedeByID(int codSubRede);

        
        Task<SUBREDE> GetSubRedeByID(int codSubRede);
    }
}
