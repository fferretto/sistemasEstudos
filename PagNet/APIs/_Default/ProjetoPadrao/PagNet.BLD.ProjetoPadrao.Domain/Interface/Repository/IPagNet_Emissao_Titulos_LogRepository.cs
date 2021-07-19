using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository
{
    public interface IPagNet_Emissao_Titulos_LogRepository : IRepositoryBase<PAGNET_EMISSAO_TITULOS_LOG>
    {
        int GetMaxKey();
    }
}
