using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_API_PGTOService : IServiceBase<PAGNET_API_PGTO>
    {
        string RetornaCaminhoAPI(string codigoBanco);
    }
}
