using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Api.Service.Interface
{
    public interface IAPITrasmissaoPagamento
    {
        List<APIRetornoArquivoBancoVM> ProcessaArquivoRetorno(string CaminhoArquivo);
        Dictionary<string, string> TransmiteArquivoBanco(IAPITransmissaoArquivoModal model);
        RetornoModel CancelaArquivoRemessa(int codigoArquivo);
    }
}
