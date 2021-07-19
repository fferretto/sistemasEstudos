using PagNet.Domain.Entities;


namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_Config_Regra_PagRepository : IRepositoryBase<PAGNET_CONFIG_REGRA_PAG>
    {
        int GetMaxKey();
    }
}
