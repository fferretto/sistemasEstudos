using PagNet.Bld.PGTO.Sicoob.Abstraction.Interface.Model;
using PagNet.Bld.PGTO.Sicoob.Abstraction.Model;

namespace PagNet.Bld.PGTO.Sicoob.Abstraction.Interface
{
    public interface IPagamentoApp
    {
        ResultadoTransmissaoArquivo GeraArquivoRemessa(IFiltroTransmissaoBancoVM model);
    }
}
