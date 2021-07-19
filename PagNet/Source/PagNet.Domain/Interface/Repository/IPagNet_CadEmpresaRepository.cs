using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_CadEmpresaRepository : IRepositoryBase<PAGNET_CADEMPRESA>
    {
        int GetMaxKey();
    }
}
