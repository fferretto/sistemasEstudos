using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository
{
    public interface IPagNet_ContaEmailRepository : IRepositoryBase<PAGNET_CONTAEMAIL>
    {
        int GetMaxKey();
    }
}
