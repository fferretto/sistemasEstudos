using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services.Common;
using System.Collections.Generic;

namespace PagNet.Bld.Domain.Interface.Services
{
    public interface IPAGNET_TRANSMISSAOARQUIVOService : IServiceBase<PAGNET_TRANSMISSAOARQUIVO>
    {
        PAGNET_TRANSMISSAOARQUIVO BuscaFormaTransmissao(int codigoContaCorrente, string TipoArquivo);
        List<PAGNET_TRANSMISSAOARQUIVO> BuscaTodasFormaTransmissao(int codigoContaCorrente);

        void IncluiFormaTransmissao(PAGNET_TRANSMISSAOARQUIVO transmissao);
        void AtualizaFormaTransmissao(PAGNET_TRANSMISSAOARQUIVO transmissao);
        void RemoveTodasFormasTransmissao(int codigoContaCorrente);

    }
}
