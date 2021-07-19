using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Services
{
    public interface IPagNet_Formas_FaturamentoService : IServiceBase<PAGNET_FORMAS_FATURAMENTO>
    {
        object[][] GetHashFormasFaturamento();
        string GetFormaFaturamentoById(int codigo);
    }
}
