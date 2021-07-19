using PagNet.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Application.Interface.ProcessoCnab
{
    public interface IProcessaDadosBancoBs2
    {
        Task<IDictionary<bool, string>> EnviaPagamentoBanco(BorderoPagVM model, int codArquivo);
    }
}
