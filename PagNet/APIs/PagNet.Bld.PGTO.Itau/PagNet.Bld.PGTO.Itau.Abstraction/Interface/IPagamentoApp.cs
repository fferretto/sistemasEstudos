using PagNet.Bld.PGTO.Itau.Abstraction.Interface.Model;
using PagNet.Bld.PGTO.Itau.Abstraction.Model;
using System.Collections.Generic;

namespace PagNet.Bld.PGTO.Itau.Abstraction.Interface
{
    public interface IPagamentoApp
    {
        ResultadoTransmissaoArquivo GeraArquivoRemessa(IFiltroTransmissaoBancoVM model);
    }
}
