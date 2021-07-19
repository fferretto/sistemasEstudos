using PagNet.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Application.Interface.ProcessoCnab
{
    public interface IProcessaDadosBancoBrasil
    {
        Task<string> GeraArquivoBancoBrasil(BorderoPagVM model, int codArquivo);
        Task<List<BaixaPagamentoVM>> ProcessaArquivoRetorno(string CaminhoArquivo);
    }
}
