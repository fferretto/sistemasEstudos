using PagNet.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PagNet.Domain.Interface.Services
{
    public interface IPagNet_InstrucaoEmailService: IServiceBase<PAGNET_INSTRUCAOEMAIL>
    {
        Task<PAGNET_INSTRUCAOEMAIL> IncluirEmail(PAGNET_INSTRUCAOEMAIL conta);
        Task<PAGNET_INSTRUCAOEMAIL> GetInstrucaoById(int id);
        void AtualizaEmail(PAGNET_INSTRUCAOEMAIL conta);
        Task<List<PAGNET_INSTRUCAOEMAIL>> ConsultaInstrucaoById(int codEmpresa);
    }
}
