using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_CadClienteRepository : IRepositoryBase<PAGNET_CADCLIENTE>
    {
        int GetMaxKey();
    }

    public interface IPagNet_CadCliente_LogRepository : IRepositoryBase<PAGNET_CADCLIENTE_LOG>
    {
        int GetMaxKey();
    }
}
