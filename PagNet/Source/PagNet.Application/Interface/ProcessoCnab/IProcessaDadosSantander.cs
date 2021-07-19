using PagNet.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Application.Interface.ProcessoCnab
{
    public interface IProcessaDadosSantander
    {
        Task<string> GeraArquivoSantander(BorderoPagVM model, int codArquivo);
        Task<List<BaixaPagamentoVM>> ProcessaArquivoRetorno(string CaminhoArquivo);
    }
}
