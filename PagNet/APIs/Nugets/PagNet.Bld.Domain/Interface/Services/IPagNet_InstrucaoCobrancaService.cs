using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_INSTRUCAOCOBRANCAService : IServiceBase<PAGNET_INSTRUCAOCOBRANCA>
    {
        object[][] GetHashInstrucaoCobranca();
        string GetInstrucaoCobrancaById(int codInstrucaoCobranca);
    }
}
