using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Services.Common;
using PagNet.Bld.Domain.Interface.Repository;
using System.Linq;
using System.Collections.Generic;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_TRANSMISSAOARQUIVOService : ServiceBase<PAGNET_TRANSMISSAOARQUIVO>, IPAGNET_TRANSMISSAOARQUIVOService
    {
        private readonly IPAGNET_TRANSMISSAOARQUIVORepository _rep;

        public PAGNET_TRANSMISSAOARQUIVOService(IPAGNET_TRANSMISSAOARQUIVORepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public void AtualizaFormaTransmissao(PAGNET_TRANSMISSAOARQUIVO transmissao)
        {
            _rep.Update(transmissao);
        }

        public PAGNET_TRANSMISSAOARQUIVO BuscaFormaTransmissao(int codigoContaCorrente, string TipoArquivo)
        {
            var dados = _rep.Get(x => x.CODCONTACORRENTE == codigoContaCorrente && x.TIPOARQUIVO == TipoArquivo).FirstOrDefault();

            return dados;
        }
        public List<PAGNET_TRANSMISSAOARQUIVO> BuscaTodasFormaTransmissao(int codigoContaCorrente)
        {
            var dados = _rep.Get(x => x.CODCONTACORRENTE == codigoContaCorrente).ToList();

            return dados;
        }

        public void IncluiFormaTransmissao(PAGNET_TRANSMISSAOARQUIVO transmissao)
        {
            int id = _rep.BuscaProximoID();
            transmissao.CODTRANSMISSAOARQUIVO = id;
            _rep.Add(transmissao);
        }

        public void RemoveTodasFormasTransmissao(int codigoContaCorrente)
        {
            var dados = _rep.Get(x => x.CODCONTACORRENTE == codigoContaCorrente).ToList();
            if(dados != null)
            {
                _rep.RemoveRanger(dados);
            }
        }
    }
}
