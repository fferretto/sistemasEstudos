using PagNet.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Application.Interface.ProcessoCnab
{
    public interface IProcessaDadosBradesco
    {
        Task<string> GeraArquivoBradesco(BorderoPagVM model, int codArquivo);
        Task<List<BaixaPagamentoVM>> ProcessaArquivoRetorno(string CaminhoArquivo);
    }
}
