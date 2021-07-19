using PagNet.BLD.ProjetoPadrao.Domain.Entities;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository;
using PagNet.BLD.ProjetoPadrao.Domain.Services.Common;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.BLD.ProjetoPadrao.Domain.Services
{
    public class PagNet_Emissao_TitulosService : ServiceBase<PAGNET_EMISSAO_TITULOS>, IPagNet_Emissao_TitulosService
    {
        private static object _syncIncluiTitulo = new object();
        private static object _syncIncluiTituloLog = new object();

        private readonly IPagNet_Emissao_TitulosRepository _rep;
        private readonly IPagNet_Emissao_Titulos_LogRepository _log;


        public PagNet_Emissao_TitulosService(IPagNet_Emissao_TitulosRepository rep,
                                       IPagNet_Emissao_Titulos_LogRepository log)
            : base(rep)
        {
            _rep = rep;
            _log = log;
        }

        public void AtualizaPlanoContasTitulosNaoBaixados(int codEmpresa, int CodPlanoContas, int CodUsuario)
        {
            var ListaTitulos = _rep.Get(x => x.CODEMPRESA == codEmpresa && x.STATUS == "EM_ABERTO").ToList();
            foreach(var tit in ListaTitulos)
            {
                tit.CODPLANOCONTAS = CodPlanoContas;

                _rep.Update(tit);
                IncluiLog(tit, CodUsuario, "Alteração do plano de contas padrão.");
            }
        }

        public void AtualizaStatusTituloBycodBordero(int codBordero, string NovoStatus)
        {
            var Titulos = BuscaTitulosByBordero(codBordero).Result;

            foreach (var lista in Titulos)
            {
                if (NovoStatus == "EM_ABERTO")
                    lista.CODBORDERO = null;
                if (NovoStatus == "EM_BORDERO")
                    lista.SEUNUMERO = "";

                lista.STATUS = NovoStatus;
                _rep.Update(lista);
            }
        }

        public void AtualizaTitulo(PAGNET_EMISSAO_TITULOS Titulo)
        {
            _rep.Update(Titulo);
        }

        public async Task<List<PAGNET_EMISSAO_TITULOS_LOG>> BuscaLog(int CODTITULO)
        {
            var dados = _log.Get(x => x.CODTITULO == CODTITULO, "PAGNET_CADFAVORECIDO,PAGNET_CADEMPRESA,USUARIO_NETCARD")
                            .OrderBy(y => y.DATINCLOG).ToList();
            return dados;             
        }

        public async Task<int> BuscaProximoCodTitulo()
        {
            return _rep.GetMaxKey();
        }

        public async Task<PAGNET_EMISSAO_TITULOS> BuscaTituloByID(int CodTitulo)
        {
            return _rep.Get(x => x.CODTITULO == CodTitulo, "PAGNET_CADFAVORECIDO,PAGNET_CADEMPRESA,PAGNET_CADPLANOCONTAS").FirstOrDefault();
        }

        public async Task<List<PAGNET_EMISSAO_TITULOS>> BuscaTituloByIDPai(int codTituloPai, int CodTitulo)
        {
            if (codTituloPai > 0)
            {
                return _rep.Get(x => x.CODTITULOPAI == codTituloPai).ToList();
            }
            else
            {
                codTituloPai = Convert.ToInt32(_rep.Get(x => x.CODTITULO == CodTitulo).FirstOrDefault().CODTITULOPAI);
                return _rep.Get(x => x.CODTITULOPAI == codTituloPai).ToList();
            }
        }

        public async Task<List<PAGNET_EMISSAO_TITULOS>> BuscaTituloBySeuNumero(string SeuNumero)
        {
            var titulos = _rep.Get(x => x.SEUNUMERO == SeuNumero, "PAGNET_CADFAVORECIDO,PAGNET_CADEMPRESA").ToList();

            return titulos;
        }

        public async Task<List<PAGNET_EMISSAO_TITULOS>> BuscaTitulosByBordero(int CodBordero)
        {
            var titulos = _rep.Get(x => x.CODBORDERO == CodBordero, "PAGNET_CADFAVORECIDO,PAGNET_CADEMPRESA,PAGNET_BORDERO_PAGAMENTO").ToList();

            return titulos;
        }

        public async Task<List<PAGNET_EMISSAO_TITULOS>> BuscaTitulosByData(DateTime dtRealPGTO, int codContaCorrente)
        {
            var titulos = _rep.Get(x => x.DATREALPGTO == dtRealPGTO &&
                                    (codContaCorrente == 0 || x.CODCONTACORRENTE == codContaCorrente)).ToList();

            return titulos;
        }

        public async Task<List<PAGNET_EMISSAO_TITULOS>> BuscaTitulosByFavorecidoDatPGTO(int codEmpresa, int codFavorecido, DateTime datPGTO)
        {
            var statusAceito = new[] { "EM_ABERTO", "RECUSADO"};
            var ListaTitulos = _rep.Get(x => x.CODEMPRESA == codEmpresa &&
                                             x.CODFAVORECIDO == codFavorecido &&
                                             statusAceito.Contains(x.STATUS) &&
                                             x.DATREALPGTO == datPGTO, "PAGNET_CADFAVORECIDO,PAGNET_CADEMPRESA").ToList();

            return ListaTitulos;
        }

        public async Task<PAGNET_EMISSAO_TITULOS> BuscaTitulosByLinhaDigitavel(string LinhaDigitavel)
        {
            var ListaTitulos = _rep.Get(x => x.LINHADIGITAVEL == LinhaDigitavel).FirstOrDefault();

            return ListaTitulos;
        }

        public async Task<List<PAGNET_EMISSAO_TITULOS>> BuscaTitulosNaoConciliados(int codigoEmpresa, int codContaCorrente, DateTime dataInicio, DateTime dataFim)
        {
            var listaTransacao = _rep.Get(x => x.CODEMPRESA == codigoEmpresa &&
                                                x.CODCONTACORRENTE == codContaCorrente &&
                                                x.DATREALPGTO >= dataInicio &&
                                                x.DATREALPGTO <= dataFim &&
                                                (x.STATUS == "BAIXADO" || x.STATUS == "BAIXADO_MANUALMENTE"), "PAGNET_CADFAVORECIDO");

            return listaTransacao.ToList();


        }

        public async Task<List<PAGNET_EMISSAO_TITULOS>> BuscaTransacaoAConsolidar(int codigoEmpresa, int codContaCorrente, int mes, int ano)
        {
            var listaTransacao = _rep.Get(x => x.CODEMPRESA == codigoEmpresa &&
                                               x.CODCONTACORRENTE == codContaCorrente &&
                                               x.STATUS == "PENDENTE_COSOLIDACAO" &&
                                               x.DATREALPGTO.Month == mes &&
                                               x.DATREALPGTO.Year == ano
                                               ).ToList();
            return listaTransacao;
        }

        public async Task<List<PAGNET_EMISSAO_TITULOS>> BustaTitulosByStatus(int codEmpresa, string status)
        {
            var titulos = _rep.Get(x => x.STATUS == status 
                                     && x.CODEMPRESA == codEmpresa, "PAGNET_CADFAVORECIDO,PAGNET_CADEMPRESA").ToList();

            return titulos;
        }

        public object[][] DDLTitulosAbertosByCodFavorecido(int CodigoFavorecido)
        {
            return _rep.Get(x => x.STATUS == "EM_ABERTO" && x.CODFAVORECIDO == CodigoFavorecido)
                    .Select(x => new object[] { x.CODTITULO, "Valor: " + string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALTOTAL) +
                    " Data: " + x.DATREALPGTO }).ToArray();
        }

        public void IncluiLog(PAGNET_EMISSAO_TITULOS Titulo, int codUsuario, string Justificativa)
        {
                int id = _log.GetMaxKey();

                PAGNET_EMISSAO_TITULOS_LOG log = new PAGNET_EMISSAO_TITULOS_LOG();
                log.CODTITULO_LOG = id;
                log.CODTITULO = Titulo.CODTITULO;
                log.CODTITULOPAI = Titulo.CODTITULOPAI;
                log.STATUS = Titulo.STATUS;
                log.CODEMPRESA = Titulo.CODEMPRESA;
                log.CODFAVORECIDO = Titulo.CODFAVORECIDO;
                log.DATEMISSAO = Titulo.DATEMISSAO;
                log.DATPGTO = Titulo.DATPGTO;
                log.DATREALPGTO = Titulo.DATREALPGTO;
                log.VALBRUTO = Titulo.VALBRUTO;
                log.VALLIQ = Titulo.VALLIQ;
                log.TIPOTITULO = Titulo.TIPOTITULO;
                log.ORIGEM = Titulo.ORIGEM;
                log.SISTEMA = Titulo.SISTEMA;
                log.LINHADIGITAVEL = Titulo.LINHADIGITAVEL;
                log.CODBORDERO = Titulo.CODBORDERO;
                log.VALTOTAL = Titulo.VALTOTAL;
                log.SEUNUMERO = Titulo.SEUNUMERO;
                log.CODUSUARIO = codUsuario;
                log.DATINCLOG = DateTime.Now;
                log.DESCLOG = Justificativa;
                log.CODCONTACORRENTE = Titulo.CODCONTACORRENTE;

                _log.Add(log);
        }

        public void IncluiLogByBordero(int codBordero, string status, int codUsuario, string Justificativa)
        {
            var ListaTitulos = BuscaTitulosByBordero(codBordero).Result;
            PAGNET_EMISSAO_TITULOS_LOG log = null;

            foreach (var Titulo in ListaTitulos)
            {
                int id = _log.GetMaxKey();
                log = new PAGNET_EMISSAO_TITULOS_LOG();

                log.CODTITULO_LOG = id;
                log.CODTITULO = Titulo.CODTITULO;
                log.CODTITULOPAI = Titulo.CODTITULOPAI;
                log.STATUS = status;
                log.CODEMPRESA = Titulo.CODEMPRESA;
                log.CODFAVORECIDO = Titulo.CODFAVORECIDO;
                log.DATEMISSAO = Titulo.DATEMISSAO;
                log.DATPGTO = Titulo.DATPGTO;
                log.DATREALPGTO = Titulo.DATREALPGTO;
                log.VALBRUTO = Titulo.VALBRUTO;
                log.VALLIQ = Titulo.VALLIQ;
                log.TIPOTITULO = Titulo.TIPOTITULO;
                log.ORIGEM = Titulo.ORIGEM;
                log.SISTEMA = Titulo.SISTEMA;
                log.LINHADIGITAVEL = Titulo.LINHADIGITAVEL;
                log.CODBORDERO = Titulo.CODBORDERO;
                log.VALTOTAL = Titulo.VALTOTAL;
                log.SEUNUMERO = Titulo.SEUNUMERO;
                log.CODUSUARIO = codUsuario;
                log.DATINCLOG = DateTime.Now;
                log.DESCLOG = Justificativa;
                log.CODCONTACORRENTE = Titulo.CODCONTACORRENTE;

                _log.Add(log);
            }
        }

        public void IncluiTitulo(PAGNET_EMISSAO_TITULOS Titulo)
        {
                var id = _rep.GetMaxKey();
                Titulo.CODTITULO = id;

                if (Titulo.CODTITULOPAI == null || Titulo.CODTITULOPAI == 0)
                {
                    Titulo.CODTITULOPAI = id;
                }
                _rep.Add(Titulo);
        }

        public async Task<List<PAGNET_EMISSAO_TITULOS>> ListaTitulosEmAberto(int codEmpresa, int codFavorecido, DateTime datInicio, DateTime datFim)
        {
            var statusAceito = new[] { "EM_ABERTO", "RECUSADO" };
            var ListaTitulos = _rep.Get(x => x.CODEMPRESA == codEmpresa &&
                                             ((codFavorecido == 0) || x.CODFAVORECIDO == codFavorecido) &&
                                             statusAceito.Contains(x.STATUS) &&
                                             x.DATREALPGTO >= datInicio &&
                                             x.DATREALPGTO <= datFim, "PAGNET_CADFAVORECIDO,PAGNET_CADEMPRESA").ToList();

            return ListaTitulos;
        }

        public async Task<List<PAGNET_EMISSAO_TITULOS>> ListaTitulosNaoLiquidado(int codEmpresa)
        {

            DateTime dataAtual = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            var titulos = _rep.Get(x => x.STATUS != "BAIXADO" && 
                                        x.STATUS != "BAIXADO_MANUALMENTE" && 
                                        x.STATUS != "CANCELADO" &&
                                        x.STATUS != "PENDENTE_COSOLIDACAO" &&
                                        x.STATUS != "CONSOLIDADO" &&
                                        x.CODEMPRESA == codEmpresa &&
                                        x.DATREALPGTO < dataAtual, "PAGNET_CADFAVORECIDO,PAGNET_CADEMPRESA").ToList();

            return titulos;
        }
        public async Task<int> RetornaquantidadeParcelasFuturas(int codigo)
        {
            try
            {
                var codigoPai = _rep.Get(x => x.CODTITULO == codigo).FirstOrDefault().CODTITULOPAI;
                var qtTransacoes = _rep.Get(x => x.CODTITULOPAI == codigoPai && x.CODTITULO > codigo && x.STATUS == "PENDENTE_COSOLIDACAO").ToList().Count();

                return qtTransacoes;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
