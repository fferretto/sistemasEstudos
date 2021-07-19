using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Api.Service.Interface
{
    public interface IAPITransmissaoCobrancaBancaria
    {
        //Métodos da tela de Inclusão de Arquivo Remessa
        string GeraArquivoRemessa(IAPIBorderoBolVM filtro);
        List<APISolicitacaoBoletoVM> CarregaDadosArquivoRetorno(string CaminhoArquivo);
        void GeraBoletoPDF(IAPIFiltroCobranca filtro);
    }
}
