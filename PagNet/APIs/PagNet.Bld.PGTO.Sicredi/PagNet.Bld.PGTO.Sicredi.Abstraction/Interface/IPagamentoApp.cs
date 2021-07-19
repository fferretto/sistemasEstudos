using PagNet.Bld.PGTO.Sicredi.Abstraction.Interface.Model;
using PagNet.Bld.PGTO.Sicredi.Abstraction.Model;

namespace PagNet.Bld.PGTO.Sicredi.Abstraction.Interface
{
    public interface IPagamentoApp
    {
        ResultadoTransmissaoArquivo GeraArquivoRemessa(IFiltroTransmissaoBancoVM model);
    }
}
