using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;


namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository
{
    public interface IPagNet_Emissao_TitulosRepository : IRepositoryBase<PAGNET_EMISSAO_TITULOS>
    {
        int GetMaxKey();
    }
}
