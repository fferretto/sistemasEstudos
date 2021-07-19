using PagNet.Bld.PGTO.Santander.Abstraction.Interface.Model;
using PagNet.Bld.PGTO.Santander.Abstraction.Model;

namespace PagNet.Bld.PGTO.Santander.Abstraction.Interface
{
    public interface IPagamentoApp
    {
        ResultadoTransmissaoArquivo GeraArquivoRemessa(IFiltroTransmissaoBancoVM model);
    }
}
