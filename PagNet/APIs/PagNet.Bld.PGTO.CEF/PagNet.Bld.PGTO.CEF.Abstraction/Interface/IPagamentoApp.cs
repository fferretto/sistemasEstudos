using PagNet.Bld.PGTO.CEF.Abstraction.Interface.Model;
using PagNet.Bld.PGTO.CEF.Abstraction.Model;
using System.Collections.Generic;

namespace PagNet.Bld.PGTO.CEF.Abstraction.Interface
{
    public interface IPagamentoApp
    {
        ResultadoTransmissaoArquivo GeraArquivoRemessa(IFiltroTransmissaoBancoVM model);
    }
}
