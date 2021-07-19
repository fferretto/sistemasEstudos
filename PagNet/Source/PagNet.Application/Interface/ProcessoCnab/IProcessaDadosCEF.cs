using PagNet.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Application.Interface.ProcessoCnab
{
    public interface IProcessaDadosCEF
    {
        Task<string> GeraArquivoCEF(BorderoPagVM model, int codAquivo);
        Task<List<BaixaPagamentoVM>> ProcessaArquivoRetorno(string CaminhoArquivo);
    }
}
