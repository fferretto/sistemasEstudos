using PagNet.Domain.Entities;


namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_Emissao_TitulosRepository : IRepositoryBase<PAGNET_EMISSAO_TITULOS>
    {
        int GetMaxKey();
    }
}
