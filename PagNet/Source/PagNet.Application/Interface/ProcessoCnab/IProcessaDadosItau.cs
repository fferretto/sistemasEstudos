using PagNet.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Application.Interface.ProcessoCnab
{
    public interface IProcessaDadosItau
    {
        Task<string> GeraArquivoItau(BorderoPagVM model, int codArquivo);
        Task<List<BaixaPagamentoVM>> ProcessaArquivoRetorno(string CaminhoArquivo);
    }
}
