using PagNet.Bld.PGTO.Brasil.Abstraction.Interface.Model;
using PagNet.Bld.PGTO.Brasil.Abstraction.Model;
using System.Collections.Generic;

namespace PagNet.Bld.PGTO.Brasil.Abstraction.Interface
{
    public interface IPagamentoApp
    {
        ResultadoTransmissaoArquivo GeraArquivoRemessa(IFiltroTransmissaoBancoVM model);

    }
}
