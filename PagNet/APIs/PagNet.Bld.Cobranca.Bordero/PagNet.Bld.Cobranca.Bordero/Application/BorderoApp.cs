using Microsoft.AspNetCore.Http;
using PagNet.Bld.Cobranca.Bordero.Abstraction.Interface;
using PagNet.Bld.Domain.Interface;
using Telenet.BusinessLogicModel;
using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Cobranca.Bordero.Abstraction.Interface.Model;
using PagNet.Bld.Cobranca.Bordero.Abstraction.Model;
using System.Collections.Generic;
using System.Linq;
using System;
using PagNet.Bld.Domain.Interface.Services.Procedures;

namespace PagNet.Bld.Cobranca.Bordero.Application
{
    public class BorderoApp : Service<IContextoApp>, IApplication
    {
        protected IHttpContextAccessor _ContextAccessor { get; }
        private readonly IParametrosApp _user;
        private readonly IPAGNET_BORDERO_BOLETOService _bordero;
        private readonly IPAGNET_EMISSAOBOLETOService _fatura;
        private readonly IPAGNET_CADCLIENTEService _cliente;
        private readonly IPAGNET_CONTACORRENTEService _contaCorrente;
        private readonly IProceduresService _proc;

        public BorderoApp(IContextoApp contexto,
                            IHttpContextAccessor contextAccessor,
                            IParametrosApp user,
                            IPAGNET_BORDERO_BOLETOService bordero,
                            IPAGNET_EMISSAOBOLETOService fatura,
                            IPAGNET_CADCLIENTEService cliente,
                            IPAGNET_CONTACORRENTEService contaCorrente,
                            IProceduresService proc
                            ) : base(contexto)
        {
            _user = user;
            _ContextAccessor = contextAccessor;
            _bordero = bordero;
            _fatura = fatura;
            _cliente = cliente;
            _contaCorrente = contaCorrente;
            _proc = proc;
        }

        public DadosBoletoModel BuscaFaturas(IFiltroBorderoModel filtro)
        {
            DadosBoletoModel dadosRet = new DadosBoletoModel();
            dadosRet.codigoContaCorrente = filtro.codigoContaCorrente;
            dadosRet.codigoEmpresa = filtro.codigoEmpresa;

            var listaFaturas = _proc.ExecutarConsFaturaBordero(Convert.ToDateTime(filtro.DataInicial), Convert.ToDateTime(filtro.DataFinal),
                                                                        filtro.codigoCliente, filtro.codigoEmpresa, filtro.codigoContaCorrente);
            var itemFatura = new SolicitacaoBoletoModel();
            foreach (var fatura in listaFaturas)
            {
                itemFatura = new SolicitacaoBoletoModel();
                itemFatura.codigoFatura = fatura.CODFATURA;
                itemFatura.codigoCliente = fatura.CODCLIENTE;
                itemFatura.nomeCliente = fatura.NMCLIENTE;
                itemFatura.cnpj = fatura.CPFCNPJ;
                itemFatura.dataVencimento = fatura.DATVENCIMENTO;
                itemFatura.Valor = fatura.CODCLIENTE;
                itemFatura.QuantidadeFatura = fatura.CODCLIENTE;
                dadosRet.ListaBoletos.Add(itemFatura);
            }
            dadosRet.qtFaturasSelecionados = listaFaturas.Count;
            dadosRet.ValorBordero = listaFaturas.Select(x => x.VALOR).Sum();

            return dadosRet;
        }

        public List<DadosBorderoModel> ListaBorderos(IFiltroBorderoModel filtro)
        {
            List<DadosBorderoModel> bordero = new List<DadosBorderoModel>();

            List<PAGNET_BORDERO_BOLETO> ListaBordero = new List<PAGNET_BORDERO_BOLETO>();

            ListaBordero = _bordero.BuscaBordero(filtro.status, filtro.codigoEmpresa, filtro.codigoBordero);
            DadosBorderoModel itemBordero = new DadosBorderoModel();
            foreach (var x in ListaBordero)
            {
                itemBordero = new DadosBorderoModel();
                itemBordero.codigoBordero = x.CODBORDERO;
                itemBordero.Status = x.STATUS.Replace("_", " ");
                itemBordero.quantidadeFatura = x.QUANTFATURAS.ToString();
                itemBordero.ValorBordero = x.VLBORDERO;
                itemBordero.dataEmissao = Convert.ToDateTime(x.DTBORDERO);
                bordero.Add(itemBordero);
            }

            return bordero;
        }

        public RetornoModel Salvar(IDadosBoletoModel model)
        {
            RetornoModel ModelRetorno = new RetornoModel();
            bool AgruparFaturasDia = false;
            try
            {
                var vlBordero = model.ListaBoletos.Sum(X => X.Valor);
                var quantidadeFaturas = model.ListaBoletos.Count();

                PAGNET_BORDERO_BOLETO bordero = new PAGNET_BORDERO_BOLETO();

                bordero.STATUS = "EM_BORDERO";
                bordero.CODUSUARIO = _user.cod_usu;
                bordero.QUANTFATURAS = quantidadeFaturas;
                bordero.VLBORDERO = vlBordero;
                bordero.CODEMPRESA = model.codigoEmpresa;
                bordero.DTBORDERO = DateTime.Now;

                bordero = _bordero.InseriBordero(bordero);

                foreach (var item in model.ListaBoletos)
                {
                    AgruparFaturasDia = false;
                    var dadosCli = _cliente.BuscaClienteByID(item.codigoCliente).Result;
                    if(dadosCli.COBRANCADIFERENCIADA == "S")
                    {
                        AgruparFaturasDia = (dadosCli.AGRUPARFATURAMENTOSDIA == "S");
                    }
                    else
                    {
                        var DadosContaCorrente = _contaCorrente.GetContaCorrenteById(model.codigoContaCorrente).Result;
                        AgruparFaturasDia = (DadosContaCorrente.AGRUPARFATURAMENTOSDIA == "S");
                    }
                    if(AgruparFaturasDia)
                    {
                        var listaFaturas = _fatura.BuscaFaturas(item.codigoCliente, item.dataVencimento);

                        foreach(var fatura in listaFaturas)
                        {
                            var dadosFatura = _fatura.BuscaFaturamentoByID(fatura.CODEMISSAOFATURAMENTO);

                            dadosFatura.STATUS = bordero.STATUS;
                            dadosFatura.CODBORDERO = bordero.CODBORDERO;

                            _fatura.AtualizaFaturamento(dadosFatura);
                            _fatura.IncluiLog(dadosFatura, _user.cod_usu, "Fatura incluído em um borderô");
                        }
                    }
                    else
                    {
                        var DadosEmissaoBoleto = _fatura.BuscaFaturamentoByID(item.codigoFatura);

                        DadosEmissaoBoleto.STATUS = bordero.STATUS;
                        DadosEmissaoBoleto.CODBORDERO = bordero.CODBORDERO;

                        _fatura.AtualizaFaturamento(DadosEmissaoBoleto);
                        _fatura.IncluiLog(DadosEmissaoBoleto, _user.cod_usu, "Boleto incluído em um borderô");
                    }

                }
                ModelRetorno.Sucesso = true;
                ModelRetorno.msgResultado = "Borderô salvo com sucesso. O número dele é: " + bordero.CODBORDERO;
                
            }
            catch (Exception ex)
            {
                ModelRetorno.Sucesso = false;
                ModelRetorno.msgResultado = ex.Message;
            }
            return ModelRetorno;
        }

        public RetornoModel Cancelar(int codigoBordero)
        {
            RetornoModel dadosRet = new RetornoModel();
            _bordero.AtualizaStatusBordero(codigoBordero, "CANCELADO");
            _fatura.IncluiLogByBordero(codigoBordero, "EM_ABERTO", _user.cod_usu, "Borderô cancelado");
            _fatura.AtualizaStatusBycodBordero(codigoBordero, "EM_ABERTO");
            dadosRet.Sucesso = true;
            dadosRet.msgResultado = "Borderô cancelado com sucesso.";

            return dadosRet;
        }
    }
}
