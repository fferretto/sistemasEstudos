using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository
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
