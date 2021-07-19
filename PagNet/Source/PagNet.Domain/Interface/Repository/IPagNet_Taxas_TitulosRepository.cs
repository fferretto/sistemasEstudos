using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_Taxas_TitulosRepository : IRepositoryBase<PAGNET_TAXAS_TITULOS>
    {
        int GetMaxKey();
    }
}
