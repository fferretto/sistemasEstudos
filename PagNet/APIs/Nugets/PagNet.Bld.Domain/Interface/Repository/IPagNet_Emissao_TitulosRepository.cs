using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository.Common;


namespace PagNet.Bld.Domain.Interface.Repository
{
    public interface IPAGNET_EMISSAO_TITULOSRepository : IRepositoryBase<PAGNET_EMISSAO_TITULOS>
    {
        int BuscaProximoID();
    }
    public interface IPAGNET_EMISSAO_TITULOS_LOGRepository : IRepositoryBase<PAGNET_EMISSAO_TITULOS_LOG>
    {
        int BuscaProximoID();
    }
}
