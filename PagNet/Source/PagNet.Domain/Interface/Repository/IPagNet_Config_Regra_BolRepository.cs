using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_Config_Regra_BolRepository : IRepositoryBase<PAGNET_CONFIG_REGRA_BOL>
    {
        int GetMaxKey();
    }
}
