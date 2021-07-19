using PagNet.Domain.Entities;

namespace PagNet.Domain.Interface.Services
{
    public interface IPagNet_InstrucaoCobrancaService : IServiceBase<PAGNET_INSTRUCAOCOBRANCA>
    {
        object[][] GetHashInstrucaoCobranca();
        string GetInstrucaoCobrancaById(int codInstrucaoCobranca);
    }
}
