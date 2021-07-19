using Boleto2Net;
using PagNet.Application.Models;

namespace PagNet.Application.Interface.ProcessoCnab
{
    public interface IArquivoRemessaBoleto
    {
        void GeraRemessaBoleto(BorderoReceitaVm Boletos, IBanco banco, TipoArquivo tipoArquivo, string nmArquivo, string CaminhoArquivo);
    }
}
