using PagNet.Bld.PGTO.ArquivoRemessa.Abrstraction.Interface.Model;
using PagNet.Bld.PGTO.ArquivoRemessa.Abrstraction.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.PGTO.ArquivoRemessa.Abrstraction.Interface
{
    public interface IArquivoRemessa
    {
        ResultadoGeracaoArquivo TransmiteArquivoBanco(FiltroTransmissaoBancoVM filtro);
        List<RetornoLeituraArquivoModel> ProcessaArquivoRetorno(string caminhoArquivo);
        RetornoModel CancelaArquivoRemessaByID(int codigoArquivo);
    }
}
