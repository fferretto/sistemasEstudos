using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Services
{
    public interface IPagNet_Formas_FaturamentoService : IServiceBase<PAGNET_FORMAS_FATURAMENTO>
    {
        object[][] GetHashFormasFaturamento();
        string GetFormaFaturamentoById(int codigo);
    }
}
