using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_FORMAS_FATURAMENTOService : IServiceBase<PAGNET_FORMAS_FATURAMENTO>
    {
        object[][] GetHashFormasFaturamento();
        string GetFormaFaturamentoById(int codigo);
    }
}
