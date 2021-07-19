using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services.Common;

namespace PagNet.BLD.ProjetoPadrao.Domain.Interface.Services
{
    public interface IPagNet_InstrucaoCobrancaService : IServiceBase<PAGNET_INSTRUCAOCOBRANCA>
    {
        object[][] GetHashInstrucaoCobranca();
        string GetInstrucaoCobrancaById(int codInstrucaoCobranca);
    }
}
