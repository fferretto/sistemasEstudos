using PagNet.Bld.PGTO.CobrancaBancaria.Abstraction.Interface.Model;
using PagNet.Bld.PGTO.CobrancaBancaria.Abstraction.Model;
using System.Collections.Generic;

namespace PagNet.Bld.PGTO.CobrancaBancaria.Abstraction.Interface
{
    public interface ICobrancaBancariaApp
    {
        ResultadoTransmissaoArquivo GeraArquivoRemessa(IBorderoBolVM filtro);
        List<SolicitacaoBoletoVM> CarregaDadosArquivoRetorno(IFiltroCobranca filtro);
        void GeraBoletoPDF(IFiltroCobranca filtro);

    }
}