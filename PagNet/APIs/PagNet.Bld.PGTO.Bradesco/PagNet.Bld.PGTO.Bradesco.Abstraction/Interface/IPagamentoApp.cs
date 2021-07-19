using PagNet.Bld.PGTO.Bradesco.Abstraction.Interface.Model;
using PagNet.Bld.PGTO.Bradesco.Abstraction.Model;

namespace PagNet.Bld.PGTO.Bradesco.Abstraction.Interface
{
    public interface IPagamentoApp
    {
        ResultadoTransmissaoArquivo GeraArquivoRemessa(IFiltroTransmissaoBancoVM model);
    }
}
