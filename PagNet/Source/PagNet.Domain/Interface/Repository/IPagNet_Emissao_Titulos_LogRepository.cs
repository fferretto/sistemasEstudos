using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_Emissao_Titulos_LogRepository : IRepositoryBase<PAGNET_EMISSAO_TITULOS_LOG>
    {
        int GetMaxKey();
    }
}
