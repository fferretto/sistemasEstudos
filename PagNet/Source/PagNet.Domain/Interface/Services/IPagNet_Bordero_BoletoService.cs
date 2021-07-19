using PagNet.Domain.Entities;
using System.Collections.Generic;

namespace PagNet.Domain.Interface.Services
{
    public interface IPagNet_Bordero_BoletoService : IServiceBase<PAGNET_BORDERO_BOLETO>
    {
        List<PAGNET_BORDERO_BOLETO> BuscaBordero(string status, int codSubRede, int codBordero);
        List<PAGNET_BORDERO_BOLETO> BuscaBorderoByCodArquivo(int codCarquivo);

        PAGNET_BORDERO_BOLETO LocalizaBordero(int codBordero);
        PAGNET_BORDERO_BOLETO InseriBordero(PAGNET_BORDERO_BOLETO b);
        void AtualizaStatusBordero(int codBordero, string NovoStatus);
        void AtualizaBordero(PAGNET_BORDERO_BOLETO b);
    }
}
