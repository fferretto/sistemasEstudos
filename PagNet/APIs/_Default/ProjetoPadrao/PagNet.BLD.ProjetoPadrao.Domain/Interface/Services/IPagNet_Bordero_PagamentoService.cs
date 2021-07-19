using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Services
{
    public interface IPagNet_Bordero_PagamentoService : IServiceBase<PAGNET_BORDERO_PAGAMENTO>
    {
        List<PAGNET_BORDERO_PAGAMENTO> BuscaBordero(string status, string codBanco, int codSubRede, string codFormaPGTO, int codBordero);
        Task<List<PROC_PAGNET_CONS_BORDERO>> Proc_ConsultaBorderoPagamento(int codEmpresa, int codBordero, int codContaCorrente, string Status);

        PAGNET_BORDERO_PAGAMENTO LocalizaBordero(int codBordero);
        PAGNET_BORDERO_PAGAMENTO InseriBordero(PAGNET_BORDERO_PAGAMENTO b);
        void AtualizaStatusBordero(int codBordero, string NovoStatus);
        void AtualizaBordero(PAGNET_BORDERO_PAGAMENTO b);
    }
}
