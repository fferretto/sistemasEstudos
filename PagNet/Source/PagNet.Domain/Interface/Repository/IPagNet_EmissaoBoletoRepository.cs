using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_EmissaoBoletoRepository : IRepositoryBase<PAGNET_EMISSAOBOLETO>
    {
        int GetMaxKey();
    }
}
