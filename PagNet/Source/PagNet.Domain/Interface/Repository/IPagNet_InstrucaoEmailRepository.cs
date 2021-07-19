using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Repository
{
    public interface IPagNet_InstrucaoEmailRepository : IRepositoryBase<PAGNET_INSTRUCAOEMAIL>
    {
        int GetMaxKey();
    }
}
