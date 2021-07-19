using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.Domain.Interface.Services.Procedures;
using PagNet.Bld.Titulos.Abstraction.Interface;
using PagNet.Bld.Titulos.Abstraction.Interface.Model;
using PagNet.Bld.Titulos.Abstraction.Model;
using PagNet.Bld.Titulos.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Telenet.BusinessLogicModel;

namespace PagNet.Bld.Titulos.Application
{
    public class PagamentoApp : Service<IContextoApp>, IPagamentoApp
    {
        private readonly IOPERADORAService _ope;
        private readonly IPAGNET_CONTACORRENTEService _conta;
        private readonly IPAGNET_TRANSMISSAOARQUIVOService _transmissaoArquivo;
        private readonly IParametrosApp _user;
        private readonly IPAGNET_CADFAVORECIDOService _favorecido;
        private readonly IPAGNET_EMISSAO_TITULOSService _emissaoTitulos;
        private readonly IPAGNET_TITULOS_PAGOSService _pagamento;
        private readonly IPAGNET_ARQUIVOService _arquivo;
        private readonly IPAGNET_CADEMPRESAService _cadEmpresa;
        private readonly IPAGNET_BORDERO_PAGAMENTOService _bordero;
        private readonly IPAGNET_TAXAS_TITULOSService _taxaTitulo;
        private readonly IPAGNET_OCORRENCIARETPAGService _ocorrencia;
        private readonly IProceduresService _proc;

        public PagamentoApp(IContextoApp contexto,
                                IParametrosApp user,
                                IOPERADORAService ope,
                                IPAGNET_CONTACORRENTEService conta,
                                IPAGNET_TRANSMISSAOARQUIVOService transmissaoArquivo,
                                IPAGNET_CADFAVORECIDOService favorecido,
                                IPAGNET_EMISSAO_TITULOSService emissaoTitulos,
                                IPAGNET_TITULOS_PAGOSService pagamento,
                                IPAGNET_ARQUIVOService arquivo,
                                IProceduresService proc,
                                IPAGNET_CADEMPRESAService cadEmpresa,
                                IPAGNET_BORDERO_PAGAMENTOService bordero,
                                IPAGNET_TAXAS_TITULOSService taxaTitulo,
                                IPAGNET_OCORRENCIARETPAGService ocorrencia)
            : base(contexto)
        {
            _user = user;
            _ope = ope;
            _conta = conta;
            _transmissaoArquivo = transmissaoArquivo;
            _favorecido = favorecido;
            _emissaoTitulos = emissaoTitulos;
            _arquivo = arquivo;
            _cadEmpresa = cadEmpresa;
            _bordero = bordero;
            _taxaTitulo = taxaTitulo;
            _pagamento = pagamento;
            _proc = proc;
            _ocorrencia = ocorrencia;
        }

        public APIRetornoModal IncluirNovoTituloPGTO(IAPIDadosTituloModel model)
        {
            APIRetornoModal DadosRetorno = new APIRetornoModal();

            PAGNET_EMISSAO_TITULOS Titulos;


            if (model.codigoTitulo == 0)
            {
                Titulos = new PAGNET_EMISSAO_TITULOS();
            }
            else
            {
                Titulos = _emissaoTitulos.BuscaTituloByID(model.codigoTitulo).Result;
            }

            Titulos.STATUS = "EM_ABERTO";
            Titulos.CODEMPRESA = Convert.ToInt32(model.codigoEmpresa);
            Titulos.CODFAVORECIDO = Convert.ToInt32(model.codigoFavorecido);
            Titulos.DATEMISSAO = DateTime.Now;

            if (model.tipoTitulo == "BOLETO")
            {
                Titulos.DATPGTO = Convert.ToDateTime(model.dataPagamento);
                Titulos.DATREALPGTO = Convert.ToDateTime(model.dataRealPagamento);
                Titulos.VALBRUTO = Convert.ToDecimal(model.valorBruto.Replace("R$", "").Replace(".", ""));
                Titulos.VALLIQ = Convert.ToDecimal(model.valorLiquido.Replace("R$", "").Replace(".", ""));
                Titulos.VALTOTAL = Convert.ToDecimal(model.valorLiquido.Replace("R$", "").Replace(".", ""));
            }
            else
            {
                Titulos.DATPGTO = Convert.ToDateTime(model.dataPagamento);
                Titulos.DATREALPGTO = Convert.ToDateTime(model.dataRealPagamento);
                Titulos.VALBRUTO = Convert.ToDecimal(model.valorBruto.Replace("R$", "").Replace(".", ""));
                Titulos.VALLIQ = Convert.ToDecimal(model.valorLiquido.Replace("R$", "").Replace(".", ""));
                Titulos.VALTOTAL = Convert.ToDecimal(model.valorLiquido.Replace("R$", "").Replace(".", ""));
            }

            Titulos.TIPOTITULO = model.tipoTitulo;
            Titulos.ORIGEM = "PN";
            Titulos.SISTEMA = 0;
            Titulos.LINHADIGITAVEL = model.linhaDigitavel;
            Titulos.CODBORDERO = null;
            Titulos.SEUNUMERO = "";
            Titulos.CODPLANOCONTAS = Convert.ToInt32(model.codigoPlanoContas);

            try
            {
                if (model.codigoTitulo == 0)
                {
                    _emissaoTitulos.IncluiTitulo(Titulos);
                    _emissaoTitulos.IncluiLog(Titulos, _user.cod_usu, "Título incluído manualmente via PagNet");

                    DadosRetorno.sucesso = false;
                    DadosRetorno.msgRetorno = "Título incluído com sucesso";
                }
                else
                {
                    _emissaoTitulos.AtualizaTitulo(Titulos);
                    _emissaoTitulos.IncluiLog(Titulos, _user.cod_usu, "Título atualizado manualmente via PagNet");

                    DadosRetorno.sucesso = false;
                    DadosRetorno.msgRetorno = "Título atualizado com sucesso";
                }

            }
            catch (Exception ex)
            {
                DadosRetorno.sucesso = false;
                DadosRetorno.msgRetorno = ex.Message;
            }

            return DadosRetorno;
        }
        public List<TaxasCobradasPGTOModel> ListarTaxasTitulo(int codigoTitulo)
        {
            List<TaxasCobradasPGTOModel> listaRetorno = new List<TaxasCobradasPGTOModel>();
            try
            {
                var taxas = _taxaTitulo.buscaTodasTaxasbyCodTitulo(codigoTitulo).Result;
                TaxasCobradasPGTOModel taxa = new TaxasCobradasPGTOModel();
                foreach (var x in taxas)
                {
                    taxa.CodigoTaxa = x.CODTAXATITULO;
                    taxa.CodigoTitulo = x.CODTITULO;
                    taxa.Descrição = x.DESCRICAO;
                    taxa.ValorTotal_aux = x.VALOR;
                    taxa.Valor = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Math.Abs(x.VALOR));
                    taxa.DataInclusao = x.DTINCLUSAO.ToShortDateString();
                    taxa.nmUsuario = x.PAGNET_USUARIO.NMUSUARIO;
                    listaRetorno.Add(taxa);
                }

                var ValorTotal = listaRetorno.Select(x => x.ValorTotal_aux).Sum();

                for (int i = 0; i < listaRetorno.Count; i++)
                {
                    listaRetorno[i].ValorTotal = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Math.Abs(ValorTotal));
                }

                return listaRetorno;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public APIRetornoModal PagamentoManual(IAPIFiltroBorderoModel model)
        {
            APIRetornoModal DadosRetorno = new APIRetornoModal();
            try
            {
                DadosRetorno.sucesso = true;
                DadosRetorno.msgRetorno = "Baixa Registrado com Sucesso.";

                foreach (var Lista in model.ListaFechamento)
                {
                    var ListaTitulos = _emissaoTitulos.BuscaTitulosByFavorecidoDatPGTO(Convert.ToInt32(model.codEmpresa), Lista.codigoFavorecido, Convert.ToDateTime(Lista.dataPagamento)).Result;
                    foreach (var titulo in ListaTitulos)
                    {
                        var Titulo = _emissaoTitulos.GetById(titulo.CODTITULO);
                        Titulo.STATUS = "BAIXADO_MANUALMENTE";
                        Titulo.CODCONTACORRENTE = model.codigoContaCorrente;

                        _emissaoTitulos.AtualizaTitulo(Titulo);


                        //INSERI LOG DE ATUALIZAÇÃO DO TÍTULO
                        _emissaoTitulos.IncluiLog(Titulo, model.codUsuario, "Baixa manual com o motivo: " + model.descJustificativa);

                        //ATUALIZA SALDO DA CONTACA CORRENTE
                        AtualizaSaldoContaCorrente(model.codigoContaCorrente, titulo.CODTITULO);
                    }
                }


            }
            catch (Exception ex)
            {
                DadosRetorno.sucesso = false;
                DadosRetorno.msgRetorno = "Ocorreu uma falha durante o processo. favor contactar o suporte técnico";
            }
            return DadosRetorno;
        }
        public void AtualizaSaldoContaCorrente(int codcontacorrente, int codTitulo)
        {
            try
            {
                var dadosContaCorrente = _conta.GetContaCorrenteById(codcontacorrente).Result;
                var dadosTitulo = _emissaoTitulos.BuscaTituloByID(codTitulo).Result;
                var dadosTaxas = _taxaTitulo.buscaTodasTaxasbyCodTitulo(codTitulo).Result;

                decimal valorPago = 0;
                decimal valorTaxas = 0;
                decimal saldoContaCorrente = 0;

                valorPago = dadosTitulo.VALTOTAL;
                saldoContaCorrente = _conta.RetornaSaldoAtual(codcontacorrente);

                if (dadosTaxas != null)
                {
                    valorTaxas = dadosTaxas.Select(x => x.VALOR).Sum();
                }

                decimal CalculoSaldoAtual = saldoContaCorrente - (valorPago + valorTaxas);

                _conta.InseriNovoSaldo(codcontacorrente, dadosContaCorrente.CODEMPRESA, CalculoSaldoAtual);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public APIRetornoModal SalvaEdicaoTitulo(IAPIDadosTituloModel model)
        {
            APIRetornoModal DadosRetorno = new APIRetornoModal();
            try
            {

                var Titulo = _emissaoTitulos.BuscaTituloByID(model.codigoTitulo).Result;


                Titulo.DATREALPGTO = Convert.ToDateTime(model.dataRealPagamento);
                Titulo.CODPLANOCONTAS = Convert.ToInt32(model.codigoPlanoContas);

                _emissaoTitulos.AtualizaTitulo(Titulo);
                _emissaoTitulos.IncluiLog(Titulo, _user.cod_usu, "Edição de título com a justificativa: " + model.descricaoJustificativa);
                DadosRetorno.sucesso = true;
                DadosRetorno.msgRetorno = "Título alterado com sucesso!";

                return DadosRetorno;

            }
            catch (ArgumentException ex)
            {
                DadosRetorno.sucesso = false;
                DadosRetorno.msgRetorno = "Falha no processo. Favor contactar o suporte!";
            }
            return DadosRetorno;
        }
        public APIRetornoModal SalvarAjusteValorTitulo(IAPIDadosTituloModel model)
        {
            APIRetornoModal DadosRetorno = new APIRetornoModal();
            try
            {

                var Titulo = _emissaoTitulos.BuscaTituloByID(model.codigoTitulo).Result;
                var valorInformado = Convert.ToDecimal(model.valorConcedido);
                var justificativa = "";

                if (model.Desconto)
                {
                    justificativa = "Desconto de R$ " + valorInformado + " via sistema PagNet com a justificativa: " + model.descricaoJustificativa;
                    valorInformado = -1 * valorInformado;
                }
                else
                {
                    justificativa = "Acréscimo de R$" + valorInformado + " via sistema PagNet com a justificativa: " + model.descricaoJustificativa;
                }

                Titulo.VALTOTAL = Titulo.VALTOTAL + valorInformado;

                _emissaoTitulos.AtualizaTitulo(Titulo);
                _emissaoTitulos.IncluiLog(Titulo, _user.cod_usu, justificativa);

                PAGNET_TAXAS_TITULOS tx = new PAGNET_TAXAS_TITULOS();
                tx.CODTITULO = model.codigoTitulo;
                tx.DESCRICAO = model.descricaoTaxa;
                tx.VALOR = valorInformado;
                tx.ORIGEM = "PN";
                tx.DTINCLUSAO = DateTime.Now;
                tx.CODUSUARIO = _user.cod_usu;

                _taxaTitulo.IncluiTaxa(tx);

                DadosRetorno.sucesso = true;
                DadosRetorno.msgRetorno = "Título alterado com sucesso!";


            }
            catch (ArgumentException ex)
            {
                DadosRetorno.sucesso = false;
                DadosRetorno.msgRetorno = "Falha no processo. Favor contactar o suporte!";
            }
            return DadosRetorno;
        }
        public APIRetornoModal BaixaManualByID(IAPIFiltroBorderoModel model)
        {
            APIRetornoModal DadosRetorno = new APIRetornoModal();
            try
            {
                DadosRetorno.sucesso = true;
                DadosRetorno.msgRetorno = "Baixa Registrado com Sucesso.";

                foreach (var tt in model.ListaFechamento)
                {
                    var Titulo = _emissaoTitulos.BuscaTituloByID(Convert.ToInt32(tt.codigoTitulo)).Result;

                    Titulo.STATUS = "BAIXADO_MANUALMENTE";
                    Titulo.CODCONTACORRENTE = model.codigoContaCorrente;

                    _emissaoTitulos.AtualizaTitulo(Titulo);

                    //INSERI LOG DE ATUALIZAÇÃO DO TÍTULO
                    _emissaoTitulos.IncluiLog(Titulo, model.codUsuario, "Baixa manual com o motivo: " + model.descJustificativa);

                    //ATUALIZA SALDO DA CONTACA CORRENTE
                    AtualizaSaldoContaCorrente(model.codigoContaCorrente, Convert.ToInt32(tt.codigoTitulo));

                }

            }
            catch (Exception ex)
            {
                DadosRetorno.sucesso = false;
                DadosRetorno.msgRetorno = "Ocorreu uma falha durante o processo. favor contactar o suporte técnico";
            }
            return DadosRetorno;
        }
        public APIRetornoModal DesvinculaTitulo(IAPIFiltroBorderoModel model)
        {
            APIRetornoModal DadosRetorno = new APIRetornoModal();
            try
            {
                DadosRetorno.sucesso = true;
                DadosRetorno.msgRetorno = "Título desvinculado do arquivo de remessa com sucesso.";

                foreach (var tt in model.ListaFechamento)
                {
                    var Titulo = _emissaoTitulos.BuscaTituloByID(Convert.ToInt32(tt.codigoTitulo)).Result;

                    var listaTitulos = _emissaoTitulos.BuscaTituloBySeuNumero(Titulo.SEUNUMERO).Result;

                    var TituloPago = _pagamento.GetTransacaoBySeuNumero(Titulo.SEUNUMERO).Result;
                    TituloPago.STATUS = "CANCELADO";
                    var ret = _pagamento.AtualizaTransacao(TituloPago);

                    foreach (var tit in listaTitulos)
                    {
                        tit.STATUS = "EM_ABERTO";
                        tit.SEUNUMERO = "";
                        tit.CODBORDERO = null;
                        tit.TIPOTITULO = "TEDDOC";

                        //REMOVE A TAXA DE TRANSFERÊNCIA
                        var listaTaxa = _taxaTitulo.buscaTodasTaxasbyCodTitulo(tit.CODTITULO).Result;
                        var taxaTED = listaTaxa.Where(x => x.DESCRICAO == "TAXA DE TRANSFERÊNCIA").FirstOrDefault();
                        if (taxaTED != null)
                        {
                            tit.VALTOTAL = tit.VALTOTAL + Math.Abs(taxaTED.VALOR);
                            _taxaTitulo.RemoveTaxa(taxaTED.CODTAXATITULO);
                        }

                        _emissaoTitulos.AtualizaTitulo(tit);

                        //INSERI LOG DE ATUALIZAÇÃO DO TÍTULO
                        _emissaoTitulos.IncluiLog(tit, model.codUsuario, "Título desvinculado do arquivo de remessa com a justificativa: " + model.descJustificativa);

                    }


                }

            }
            catch (Exception ex)
            {
                DadosRetorno.sucesso = false;
                DadosRetorno.msgRetorno = "Falha ao desvincular os títulos.";
            }
            return DadosRetorno;

        }
        public APIRetornoModal CancelarTitulo(IAPIFiltroBorderoModel model)
        {
            APIRetornoModal DadosRetorno = new APIRetornoModal();
            DadosRetorno.sucesso = true;
            DadosRetorno.msgRetorno = "Baixa Registrado com Sucesso.";


            try
            {
                foreach (var linha in model.ListaFechamento)
                {
                    var Titulo = _emissaoTitulos.GetById(linha.codigoTitulo);

                    if (Titulo.SEUNUMERO != null)
                    {
                        var tt = _pagamento.GetTransacaoBySeuNumero(Titulo.SEUNUMERO).Result;
                        if (tt != null)
                        {
                            var result = _pagamento.AtualizaStatus("CANCELADO", tt.CODTITULOPAGO);
                        }
                    }
                    Titulo.STATUS = "CANCELADO";
                    Titulo.SEUNUMERO = null;
                    Titulo.CODBORDERO = null;

                    _emissaoTitulos.Update(Titulo);

                    //INSERI LOG DE ATUALIZAÇÃO DO TÍTULO
                    _emissaoTitulos.IncluiLog(Titulo, model.codUsuario, "Título cancelado com a justificativa:" + model.descJustificativa);


                }

            }
            catch (ArgumentException ex)
            {
                DadosRetorno.sucesso = false;
                DadosRetorno.msgRetorno = "Falha ao cancelar um ou mais títulos.";
            }
            return DadosRetorno;
        }
        public APIRetornoModal AlteraDataPGTOEmMassa(IGridTitulosModel model)
        {
            APIRetornoModal DadosRetorno = new APIRetornoModal();
            DadosRetorno.sucesso = true;
            DadosRetorno.msgRetorno = "Títulos atualizados com sucesso";


            var resultado = new Dictionary<string, string>();
            DateTime novaDataPGTO = Convert.ToDateTime(model.dtTransferencia);


            try
            {
                foreach (var linha in model.ListaTitulos)
                {
                    var Titulo = _emissaoTitulos.GetById(linha.codigoTitulo);
                    Titulo.DATREALPGTO = novaDataPGTO;

                    _emissaoTitulos.Update(Titulo);

                    //INSERI LOG DE ATUALIZAÇÃO DO TÍTULO
                    _emissaoTitulos.IncluiLog(Titulo, _user.cod_usu, "Alteração da data de Pagamento com a justificativa:" + model.Justificativa);


                }

            }
            catch (ArgumentException ex)
            {
                DadosRetorno.sucesso = false;
                DadosRetorno.msgRetorno = "Falha ao atualizar os títulos.";
            }
            return DadosRetorno;
        }
        public GridTitulosModel GetAllTitulosVencidos(IAPIFiltroTitulosPagamentoModel vm)
        {
            try
            {
                GridTitulosModel Titulos = new GridTitulosModel();
                APIDadosTituloModel titVencido = new APIDadosTituloModel();

                Titulos.dtTransferencia = DateTime.Now.ToShortDateString();

                int codEmpresa = Convert.ToInt32(vm.codEmpresa);
                DateTime dtInicio = Convert.ToDateTime(vm.dtInicio);
                DateTime dtFim = Convert.ToDateTime(vm.dtFim);

                var ListaTitulos = _proc.RetornaTitulosVencidos(codEmpresa, dtInicio, dtFim);
                Titulos.dtTransferencia = DateTime.Now.ToShortDateString();

                foreach(var item in ListaTitulos)
                {
                    titVencido = new APIDadosTituloModel();
                    titVencido.codigoTitulo = item.CODTITULO;
                    titVencido.codigoFavorecido = item.CODFAVORECIDO;
                    titVencido.nomeFavorecido = Helper.FormataTexto(item.NMFAVORECIDO, 30);
                    titVencido.CNPJ = Helper.FormataCPFCnPj(item.CPFCNPJ);
                    titVencido.dataRealPagamento = item.DATREALPGTO.ToString("dd/MM/yyyy");
                    titVencido.valorLiquido = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", item.VALTOTAL);
                    titVencido.codigoBanco = item.BANCO;
                    titVencido.Agencia = item.AGENCIA;
                    titVencido.contaCorrente = item.CONTACORRENTE;
                    Titulos.ListaTitulos.Add(titVencido);
                }


                return Titulos;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private List<APIDadosTituloModel> ToViewTitVencidos(List<PROC_PAGNET_CONS_TITULOS_VENCIDOS> listatit)
        {
            List<APIDadosTituloModel> lista = new List<APIDadosTituloModel>();
            var item = new APIDadosTituloModel();
            foreach (var x in listatit)
            {
                item = new APIDadosTituloModel();
                item.codigoTitulo = x.CODTITULO;
                item.codigoFavorecido = x.CODFAVORECIDO;
                item.nomeFavorecido = Helper.FormataTexto(x.NMFAVORECIDO, 30);
                item.CNPJ = Helper.FormataCPFCnPj(x.CPFCNPJ);
                item.dataPagamento = x.DATREALPGTO.ToString("dd/MM/yyyy");
                item.dataRealPagamento = x.DATREALPGTO.ToString("dd/MM/yyyy");
                item.valorLiquido = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALTOTAL);
                item.codigoBanco = x.BANCO;
                item.Agencia = x.AGENCIA;
                item.contaCorrente = x.CONTACORRENTE;

                lista.Add(item);
            }
            return lista;

        }

        public APIFiltroBorderoModel CarregaGridTitulos(IAPIFiltrotituloModel model)
        {
            try
            {
                APIFiltroBorderoModel retorno;

                model.codigoBanco = model.codigoBanco ?? "";

                if (model.codigoBanco != "")
                {
                    model.codigoBanco = (Convert.ToInt32(model.codigoBanco)).ToString();
                    while (model.codigoBanco.Length < 3)
                    {
                        model.codigoBanco = "0" + model.codigoBanco;
                    }
                }

                var Cred = _proc.BuscaListaTitulosEmAberto(Convert.ToDateTime(model.dataInicio), Convert.ToDateTime(model.dataFim),
                    model.codigoFavorecido, Convert.ToInt32(model.codigoEmpresa), Convert.ToInt32(model.codigoContaCorrente), model.codigoBanco).ToList();


                retorno = FiltroBorderoPagVM.ToView(Cred, Convert.ToDateTime(model.dataInicio), Convert.ToDateTime(model.dataFim));

                retorno.qtTitulosSelecionados = Cred.Count.ToString();
                retorno.ValorBordero = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Cred.Sum(x => x.VALORPREVISTOPAGAMENTO)).Replace("R$", "");

                return retorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
