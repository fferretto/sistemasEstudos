using PagNet.Bld.PGTO.ABCBrasil.Abstraction.Interface.Model;
using PagNet.Bld.PGTO.ABCBrasil.Abstraction.Model;

namespace PagNet.Bld.PGTO.ABCBrasil.Abstraction.Interface
{
    public interface IPagamentoApp
    {
        ResultadoTransmissaoArquivo TransmiteArquivoBanco(IFiltroTransmissaoBancoVM filtro);
        RetornoArquivoBancoVM ProcessaArquivoRetorno(string caminhoArquivo);
    }
}
