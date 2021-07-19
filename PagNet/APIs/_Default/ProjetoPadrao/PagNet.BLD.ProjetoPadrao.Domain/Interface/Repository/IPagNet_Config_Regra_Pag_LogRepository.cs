using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;


namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository
{
    public interface IPagNet_Config_Regra_Pag_LogRepository : IRepositoryBase<PAGNET_CONFIG_REGRA_PAG_LOG>
    {
        int GetMaxKey();
    }
}
