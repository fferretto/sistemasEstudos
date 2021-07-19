using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Interface.Services.Procedures;
using PagNet.Bld.PGTO.ABCBrasil.Abstraction.Interface;
using PagNet.Bld.PGTO.ABCBrasil.Abstraction.Interface.Model;
using PagNet.Bld.PGTO.ABCBrasil.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Telenet.BusinessLogicModel;

namespace PagNet.Bld.PGTO.ABCBrasil.Application
{
    public class PagamentoApp : Service<IContextoApp>, IPagamentoApp
    {
        private readonly ITAB_GESTAO_DESCONTO_FOLHAService _descontoFolha;
        private readonly INETCARD_USUARIOPOSService _usuarioNetCard;
        private readonly IPAGNET_EMISSAOBOLETOService _emissaoBoleto;
        private readonly IProceduresService _proc;
        private readonly IParametrosApp _user;
        private readonly IPAGNET_ARQUIVO_CONCILIACAOService _arquivoConciliacao;
        private readonly IPAGNET_CADCLIENTEService _cliente;

        public PagamentoApp(IContextoApp contexto,
                                IParametrosApp user,
                                ITAB_GESTAO_DESCONTO_FOLHAService descontoFolha,
                                INETCARD_USUARIOPOSService usuarioNetCard,
                                IPAGNET_EMISSAOBOLETOService emissaoBoleto,
                                IProceduresService proc,
                                IPAGNET_CADCLIENTEService cliente,
                                IPAGNET_ARQUIVO_CONCILIACAOService arquivoConciliacao)
            : base(contexto)
        {
            _user = user;
            _descontoFolha = descontoFolha;
            _proc = proc;
            _usuarioNetCard = usuarioNetCard;
            _emissaoBoleto = emissaoBoleto;
            _arquivoConciliacao = arquivoConciliacao;
            _cliente = cliente;
        }

        public RetornoArquivoBancoVM ProcessaArquivoRetorno(string caminhoArquivo)
        {
            throw new NotImplementedException();
        }

        public ResultadoTransmissaoArquivo TransmiteArquivoBanco(IFiltroTransmissaoBancoVM filtro)
        {
            throw new NotImplementedException();
        }
    }
}
