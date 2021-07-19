using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository.Common;


namespace PagNet.Bld.Domain.Interface.Repository
{
    public interface IPAGNET_CONFIG_REGRA_PAGRepository : IRepositoryBase<PAGNET_CONFIG_REGRA_PAG>
    {
        int BuscaProximoID();
    }
    public interface IPAGNET_CONFIG_REGRA_PAG_LOGRepository : IRepositoryBase<PAGNET_CONFIG_REGRA_PAG_LOG>
    {
        int BuscaProximoID();
    }
}
