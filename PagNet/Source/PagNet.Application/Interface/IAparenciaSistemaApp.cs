using PagNet.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Application.Interface
{
    public interface IAparenciaSistemaApp
    {
        AparenciaSistemaVm CarregaLayoutAtual(int codOper, int codEmpresa);
        Task<IDictionary<string, string>> SalvarLayout(AparenciaSistemaVm vm);
        Task<IDictionary<string, string>> LayoutDefaultPagNet(AparenciaSistemaVm model);
    }
}
