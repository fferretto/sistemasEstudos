using PagNet.Bld.AntecipPGTO.Abstraction.Interface;
using PagNet.Bld.AntecipPGTO.Abstraction.Interface.Model;
using PagNet.Bld.AntecipPGTO.Abstraction.Models;
using System;
using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Interface;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Telenet.BusinessLogicModel;

namespace PagNet.Bld.AntecipPGTO.Application
{
    public class AntecipacaoApp : Service<IContextoAntecipacaoApp>, IAntecipacaoApp
    {
        private readonly IPAGNET_TAXAS_TITULOSService _taxasPag;
        private readonly IPAGNET_CONFIG_REGRAService _configPag;
        private readonly IPAGNET_EMISSAO_TITULOSService _emissaoTitulos;
        private readonly IParametrosApp _user;


        public AntecipacaoApp(IContextoAntecipacaoApp contexto,
                              IParametrosApp user,
                              IPAGNET_TAXAS_TITULOSService taxasPag,
                              IPAGNET_CONFIG_REGRAService configPag,
                              IPAGNET_EMISSAO_TITULOSService emissaoTitulos)
            : base(contexto)
        {
            _taxasPag = taxasPag;
            _configPag = configPag;
            _emissaoTitulos = emissaoTitulos;
            _user = user;
        }

        public RegraNegocioPagamentoVm BuscaRegraAtivaPagamento()
        {
            RegraNegocioPagamentoVm RegrasNegocio = new RegraNegocioPagamentoVm();

            var regras = _configPag.BuscaRegraAtivaPag();

            return RegrasNegocio;
        }

        public RegraNegocioPagamentoVm BuscaRegraPagamentoByID(int codRegra)
        {
            throw new NotImplementedException();
        }

        public AntecipacaoPGTOModel CalculaTaxaAntecipacaoPGTO(IFiltroAntecipacaoModel filtro)
        {
            var dataAtual = Convert.ToDateTime(DateTime.Now.ToShortDateString());

            AssertionValidator
                .AssertNow(filtro != null, Constants.CodigosErro.IdSistemaInvalido)
                .AssertNow(filtro.NovaDataPGTO != null, Constants.CodigosErro.DataAntecipacaoNaoInformada)
                .Assert(filtro.NovaDataPGTO >= dataAtual, Constants.CodigosErro.DataMenorDataAtual)
                .Assert(filtro.codigoTitulo > 0, Constants.CodigosErro.CodigoTituloNaoInformado)
                .Validate();

            var Titulo = _emissaoTitulos.BuscaTituloByID(filtro.codigoTitulo).Result;

            AntecipacaoPGTOModel Dados;
            Dados = new AntecipacaoPGTOModel();
            Dados = CalculaValorAntecipacao(Titulo, (DateTime)filtro.NovaDataPGTO);

            return Dados;
        }

        public List<AntecipacaoPGTOModel> ListaTitulosValidosAntecipacao(IFiltroAntecipacaoModel filtro)
        {
            if (Convert.ToDateTime(filtro.NovaDataPGTO) < Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                filtro.NovaDataPGTO = Convert.ToDateTime(DateTime.Now.ToShortDateString());
            
            AssertionValidator
                .AssertNow(filtro != null, Constants.CodigosErro.IdSistemaInvalido)
                .Assert(filtro.codigoFavorecido > 0, Constants.CodigosErro.CodigoFavorecidoNaoInformado)
                .Validate();

            var listaTitulos = _emissaoTitulos.BustaTitulosAVencer(_user.cod_empresa, (int)filtro.codigoFavorecido, (DateTime)filtro.NovaDataPGTO);

            List<AntecipacaoPGTOModel> ListaAntecipacao = new List<AntecipacaoPGTOModel>();
            AntecipacaoPGTOModel Dados;
            foreach (var item in listaTitulos)
            {
                Dados = new AntecipacaoPGTOModel();
                Dados = CalculaValorAntecipacao(item, (DateTime)filtro.NovaDataPGTO);
                ListaAntecipacao.Add(Dados);
            }

            return ListaAntecipacao;

        }
        private AntecipacaoPGTOModel CalculaValorAntecipacao(PAGNET_EMISSAO_TITULOS item, DateTime NovaDataPGTO)
        {
            string valTaxa = "R$ 0";
            decimal taxaAntecipacao = 0;
            AntecipacaoPGTOModel Dados = new AntecipacaoPGTOModel();
            var regraPGTO = _configPag.BuscaRegraAtivaPag().Result;

            decimal valorPagar = item.VALTOTAL;
            var dataPGTO = item.DATREALPGTO;
            var qtDiasAntecipacao = ((TimeSpan)(dataPGTO - NovaDataPGTO)).Days;
            taxaAntecipacao = CalculaTaxaAntecipacao(valorPagar, qtDiasAntecipacao);

            Dados.CodigoTitulo = item.CODTITULO;
            Dados.CodigoFavorecido = (int)item.CODFAVORECIDO;
            Dados.DataEmissao = item.DATEMISSAO;
            Dados.DataRealPGTO = item.DATREALPGTO;
            Dados.NomeFavorecido = item.PAGNET_CADFAVORECIDO.NMFAVORECIDO;
            Dados.CNPJ = item.PAGNET_CADFAVORECIDO.CPFCNPJ;
            Dados.CodigoBanco = item.PAGNET_CADFAVORECIDO.BANCO;
            Dados.Agencia = item.PAGNET_CADFAVORECIDO.AGENCIA + "-" + item.PAGNET_CADFAVORECIDO.DVAGENCIA;
            Dados.ContaCorrente = item.PAGNET_CADFAVORECIDO.CONTACORRENTE + "-" + item.PAGNET_CADFAVORECIDO.DVCONTACORRENTE;
            Dados.ValorAtual = item.VALTOTAL;
            Dados.CodigoEmpresa = item.CODEMPRESA;
            Dados.TipoCartao = (item.SISTEMA == 0) ? "Pós Pago" : "Pré Pago";

            if (regraPGTO != null)
            {
                if (regraPGTO.COBRATAXAANTECIPACAO == "S")
                {
                    if (regraPGTO.VLTAXAANTECIPACAO > 0)
                    {
                        valTaxa = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (decimal)regraPGTO.VLTAXAANTECIPACAO);
                    }
                    else if (regraPGTO.PERCTAXAANTECIPACAO > 0)
                    {
                        valTaxa = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (decimal)regraPGTO.PERCTAXAANTECIPACAO).Replace("R$ ", "") + " %";
                    }
                }
            }
            Dados.ValorTaxa = valTaxa;
            Dados.ValorTaxaProRata = taxaAntecipacao;
            Dados.ValorTotalComTaxa = item.VALTOTAL - taxaAntecipacao;


            return Dados;
        }
        private decimal CalculaTaxaAntecipacao(decimal valorPagar, int qtDiasAntecipacao)
        {
            decimal taxaAntecipacao = 0;
            var regraPGTO = _configPag.BuscaRegraAtivaPag().Result;
            if (regraPGTO != null)
            {
                if (regraPGTO.COBRATAXAANTECIPACAO == "S")
                {
                    //verifica o valor da taxa a ser aplica
                    if (regraPGTO.VLTAXAANTECIPACAO > 0)
                    {
                        taxaAntecipacao = (decimal)regraPGTO.VLTAXAANTECIPACAO;
                    }
                    else if (regraPGTO.PERCTAXAANTECIPACAO > 0)
                    {
                        taxaAntecipacao = ((decimal)regraPGTO.PERCTAXAANTECIPACAO / 100) * valorPagar;
                    }

                    if (regraPGTO.FORMACOMPENSACAO == "M")
                    {
                        taxaAntecipacao = (taxaAntecipacao / 30) * qtDiasAntecipacao;
                    }
                    else if (regraPGTO.FORMACOMPENSACAO == "D")
                    {
                        taxaAntecipacao = (taxaAntecipacao) * qtDiasAntecipacao;
                    }
                }
            }


            return taxaAntecipacao;
        }

        public bool SalvarAntecipacaoPGTO(IFiltroAntecipacaoModel filtro)
        {
            bool sucesso = true;

            AssertionValidator
                .AssertNow(filtro != null, Constants.CodigosErro.IdSistemaInvalido)
                .Assert(filtro.codigoTitulo > 0, Constants.CodigosErro.CodigoTituloNaoInformado)
                .Assert(filtro.NovaDataPGTO != null, Constants.CodigosErro.DataAntecipacaoNaoInformada)
                .Validate();

            PAGNET_TAXAS_TITULOS Taxa = new PAGNET_TAXAS_TITULOS();

            var Tit = _emissaoTitulos.BuscaTituloByID(filtro.codigoTitulo).Result;
            var Antecipacao = CalculaValorAntecipacao(Tit, (DateTime)filtro.NovaDataPGTO);

            Taxa.CODTITULO = Tit.CODTITULO;
            Taxa.DESCRICAO = "TAXA DE ANTECIPAÇÃO";
            Taxa.VALOR = Convert.ToDecimal(Antecipacao.ValorTaxaProRata - (Antecipacao.ValorTaxaProRata * 2)); //Converter valor para negativo
            Taxa.DTINCLUSAO = DateTime.Now;
            Taxa.ORIGEM = "PN";
            Taxa.CODUSUARIO = _user.cod_usu;

            //inseri a taxa
            _taxasPag.IncluiTaxa(Taxa);

            //Atualiza o valor total do título
            Tit.VALTOTAL = Tit.VALTOTAL - Antecipacao.ValorTaxaProRata;
            Tit.DATREALPGTO = (DateTime)filtro.NovaDataPGTO;
            _emissaoTitulos.AtualizaTitulo(Tit);

            //INCLUI LOG DE INCLUSÃO DE NOVA TAXA
            _emissaoTitulos.IncluiLog(Tit, _user.cod_usu, "Inclusão de taxa de antecipação de pagamento");



            return sucesso;
        }
    }
}
