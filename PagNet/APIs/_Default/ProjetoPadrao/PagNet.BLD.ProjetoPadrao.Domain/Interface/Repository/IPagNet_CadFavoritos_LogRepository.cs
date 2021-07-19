using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository
{
    public interface IPagNet_CadFavorecido_LogRepository : IRepositoryBase<PAGNET_CADFAVORECIDO_LOG>
    {
        int GetMaxKey();
    }
}
