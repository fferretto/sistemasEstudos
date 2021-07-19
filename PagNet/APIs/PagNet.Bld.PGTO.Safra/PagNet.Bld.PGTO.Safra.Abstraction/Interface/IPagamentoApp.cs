using PagNet.Bld.PGTO.Safra.Abstraction.Interface.Model;
using PagNet.Bld.PGTO.Safra.Abstraction.Model;

namespace PagNet.Bld.PGTO.Safra.Abstraction.Interface
{
    public interface IPagamentoApp
    {
        ResultadoTransmissaoArquivo GeraArquivoRemessa(IFiltroTransmissaoBancoVM model);
    }
}
