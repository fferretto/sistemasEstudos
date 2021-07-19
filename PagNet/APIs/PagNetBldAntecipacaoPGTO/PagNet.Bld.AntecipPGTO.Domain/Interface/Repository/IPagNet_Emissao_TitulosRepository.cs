using PagNet.Bld.AntecipPGTO.Domain.Entities;
using PagNet.Bld.AntecipPGTO.Domain.Interface.Repository.Common;

namespace PagNet.Bld.AntecipPGTO.Domain.Interface.Repository
{
    public interface IPagNet_Emissao_TitulosRepository : IRepositoryBase<PAGNET_EMISSAO_TITULOS>
    {
        int GetMaxKey();
    }
    public interface IPagNet_Emissao_Titulos_LogRepository : IRepositoryBase<PAGNET_EMISSAO_TITULOS_LOG>
    {
        int GetMaxKey();
    }
}
