using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository
{
    public interface IPagNet_Taxas_TitulosRepository : IRepositoryBase<PAGNET_TAXAS_TITULOS>
    {
        int GetMaxKey();
    }
}
