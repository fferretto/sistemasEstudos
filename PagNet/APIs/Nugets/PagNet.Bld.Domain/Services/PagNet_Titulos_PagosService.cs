using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Repository;
using PagNet.Bld.Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagNet.Bld.Domain.Services.Common;

namespace PagNet.Bld.Domain.Services
{
    public class PAGNET_TITULOS_PAGOSService : ServiceBase<PAGNET_TITULOS_PAGOS>, IPAGNET_TITULOS_PAGOSService
    {
        private readonly IPAGNET_TITULOS_PAGOSRepository _rep;

        public PAGNET_TITULOS_PAGOSService(IPAGNET_TITULOS_PAGOSRepository rep)
            : base(rep)
        {
            _rep = rep;
        }

        public async Task<bool> AtualizaStatus(string status, int CodTransacao)
        {
            try
            {
                var transacao = GetTransacaoById(CodTransacao).Result;
                transacao.STATUS = status;

                _rep.Update(transacao);

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
}

        public async Task<bool> AtualizaTransacao(PAGNET_TITULOS_PAGOS pag)
        {
            try
            {
                _rep.Update(pag);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PAGNET_TITULOS_PAGOS> GetPagamentosAtivosByCodArquivo(int codCarquivo)
        {

            var retorno = _rep.Get
                    (
                        x => x.CODARQUIVO == codCarquivo &&
                             x.PAGNET_ARQUIVO.STATUS != "CANCELADO"
                        , "PAGNET_CADFAVORECIDO,PAGNET_ARQUIVO"
                    );

            return retorno.ToList();
        }

        public List<PAGNET_TITULOS_PAGOS> GetPagamentosByCodArquivo(int codCarquivo)
        {
            var retorno = _rep.Get(x => x.CODARQUIVO == codCarquivo, "PAGNET_CADFAVORECIDO");

            return retorno.ToList();
        }

        public async Task<PAGNET_TITULOS_PAGOS> GetTransacaoById(int id)
        {
            return _rep.Get(x => x.CODTITULOPAGO == id, "PAGNET_CADFAVORECIDO").FirstOrDefault();
        }

        public async Task<PAGNET_TITULOS_PAGOS> GetTransacaoBySeuNumero(string seuNumero)
        {
            return _rep.Get(x => x.SEUNUMERO == seuNumero, "PAGNET_CADFAVORECIDO").FirstOrDefault();
        }

        public List<PAGNET_TITULOS_PAGOS> getTransacaoPagamento(DateTime dtInicio, DateTime dtFim, int codCred, int codEmpresa)
        {
            var retorno = _rep.Get
                    (
                        x => x.DTREALPAGAMENTO >= dtInicio &&
                              x.DTREALPAGAMENTO <= dtFim &&
                              ((codEmpresa == 0) || x.PAGNET_CONTACORRENTE.CODEMPRESA == codEmpresa) &&
                              x.STATUS != "CANCELADO"
                              , "PAGNET_CADFAVORECIDO,PAGNET_ARQUIVO"
                    );

            return retorno.ToList();
        }

        public List<PAGNET_TITULOS_PAGOS> getTransacaoPagamentoByBordero(int codBordero)
        {
            return _rep.Get(x => x.CODBORDERO == codBordero, "PAGNET_CADFAVORECIDO").ToList();
        }

        public PAGNET_TITULOS_PAGOS IncluiTransacao(PAGNET_TITULOS_PAGOS pag)
        {
            _rep.Add(pag);

            return pag;
        }

        public int RetornaProximoNumeroTitulosPagos()
        {
            int codpag = _rep.BuscaProximoID();
            return codpag;
        }


        public async Task<List<PAGNET_TITULOS_PAGOS>> BuscaTitulosByData(DateTime dtRealPGTO, int codContaCorrente)
        {
            var titulos = _rep.Get(x => x.DTREALPAGAMENTO == dtRealPGTO &&
                                    (codContaCorrente == 0 || x.CODCONTACORRENTE == codContaCorrente)).ToList();

            return titulos;
        }
    }
}
