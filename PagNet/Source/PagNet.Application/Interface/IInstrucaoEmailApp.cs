using PagNet.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Application.Interface
{
    public interface IInstrucaoEmailApp
    {
        Task<IDictionary<string, string>> SalvaInstrucao(InstrucaoEmailVm _email);
        InstrucaoEmailVm GetInstrucaoEmailById(int? id, int codEmpresa);

    }
}
