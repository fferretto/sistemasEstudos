using PagNet.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Application.Interface
{
    public interface IAjudaApp
    {
        Task<IDictionary<bool, string>> EnviarEmail(ContatoViaEmailVM modal);
    }
}
