using PagNet.Bld.AntecipPGTO.Domain.Entities;
using PagNet.Bld.AntecipPGTO.Domain.Interface.Repository.Common;

namespace PagNet.Bld.AntecipPGTO.Domain.Interface.Repository
{
    public interface IPagNet_Taxas_TitulosRepository : IRepositoryBase<PAGNET_TAXAS_TITULOS>
    {
        int GetMaxKey();
    }
}
