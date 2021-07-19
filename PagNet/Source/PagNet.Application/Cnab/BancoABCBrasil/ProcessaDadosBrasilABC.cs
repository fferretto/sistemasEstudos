using PagNet.Application.Helpers;
using PagNet.Application.Models;
using PagNet.Domain.Interface.Services;
using System;
using System.Linq;
using System.IO;
using PagNet.Application.Interface.ProcessoCnab;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Globalization;
using PagNet.Application.Cnab.Comun;
using PagNet.Domain.Entities;


namespace PagNet.Application.Cnab.BancoABCBrasil
{
    public class ProcessaDadosBrasilABC : IProcessaDadosBrasilABC
    {
        private readonly IPagNet_ContaCorrenteService _conta;
        private readonly IPagNet_ArquivoService _arquivo;
        private readonly IPagNet_OcorrenciaRetPagService _ocorrencia;
        private readonly IPagNet_Titulos_PagosService _pag;
        private readonly IPagNet_CadFavorecidoService _favorito;
        private readonly IPagNet_CadEmpresaService _empresa;

        public ProcessaDadosBrasilABC(IPagNet_OcorrenciaRetPagService ocorrencia,
                                     IPagNet_ContaCorrenteService conta,
                                     IPagNet_ArquivoService arquivo,
                                     IPagNet_Titulos_PagosService pag,
                                     IPagNet_CadFavorecidoService favorito,
                                     IPagNet_CadEmpresaService empresa)
        {
            _conta = conta;
            _arquivo = arquivo;
            _pag = pag;
            _ocorrencia = ocorrencia;
            _favorito = favorito;
            _empresa = empresa;
        }

        public async Task<string> GeraArquivoBradesco(BorderoPagVM model, int codArquivo)
        {
            try
            {
                string nmArquivo;
                string parth = "";

                if (model.ListaBordero.Count > 0)
                {
                    MetodosGeraisdd _MetodosGeraisdd = new MetodosGeraisdd(_arquivo);
                    var DadosArquivo = await _arquivo.ReturnFileById(codArquivo);

                    var contaCorrente = await _conta.GetContaCorrenteById(Convert.ToInt32(model.codContaCorrente));

                    string CaminhoArquivo = DadosArquivo.CAMINHOARQUIVO;
                    nmArquivo = DadosArquivo.NMARQUIVO;

                    var vm = mdCedente.ToView(contaCorrente);
                    vm.NumSeq = DadosArquivo.CODARQUIVO;
                    GerarArquivoRemessa(codArquivo, model, vm, CaminhoArquivo + "\\" + nmArquivo);

                    parth = nmArquivo;
                }

                return parth;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void GerarArquivoRemessa(int codArquivo, BorderoPagVM model, mdCedente cedente, string arquivo)
        {
            try
            {
                string Arquivo = "";
                string Lote = "";
                string header;
                string Trailerheader = "";


                int totalRegistroInserido = 0;
                int NumLot = 0;//numero do lote
                int NumLotRetorno = 0;//numero do lote
                int NumTotalRegistro = 0;//Total de Registro
                string TipoTratamento = "2";

                CNABBrasilABC240 GA = new CNABBrasilABC240();
                StreamWriter arquivoRemessa = new StreamWriter(arquivo, true);


                var TitulosPagamento = _pag.GetPagamentosByCodArquivo(codArquivo);
                var TitulosTransferencias = TitulosPagamento.Where(x => x.TIPOTITULO == "TEDDOC").ToList();

                var ListaTitulosTED = TitulosTransferencias.Where(x => x.PAGNET_CADFAVORECIDO.BANCO != "246" && x.TIPOTITULO == "TEDDOC").ToList();
                var ListaTitulosCC = TitulosTransferencias.Where(x => x.PAGNET_CADFAVORECIDO.BANCO == "246" && x.TIPOTITULO == "TEDDOC").ToList();
                var ListaTitulosBoleto = TitulosPagamento.Where(x => x.TIPOTITULO == "BOLETO").ToList();
                
                if (ListaTitulosBoleto.Count > 0)
                {
                    TipoTratamento = "1";
                }
                // Header do Arquivo
                NumTotalRegistro = 1;
                header = GA.HeaderArquivo(cedente, TipoTratamento) + Environment.NewLine;

                //MONTA LOTE PARA PAGAMENTO DE BOLETOS BANCÁRIOS
                if (ListaTitulosBoleto.Count > 0)
                {
                    //Lote += GeraTrailerLoteBoleto(cedente, model, ListaTitulosTED, "BOLETO", NumLot, out NumLotRetorno, out totalRegistroInserido);
                    //NumLot = NumLotRetorno;
                    //if (totalRegistroInserido > 0)
                    //{
                    //    NumTotalRegistro += totalRegistroInserido;
                    //}
                }

                //MONTA LOTE PARA PAGAMENTO COMO CRÉDITO EM CONTA
                if (ListaTitulosCC.Count > 0)
                {
                    Lote += GeraTrailerLote(cedente, model, ListaTitulosCC, "CC", NumLot, out NumLotRetorno, out totalRegistroInserido);
                    NumLot = NumLotRetorno;
                    if (totalRegistroInserido > 0)
                    {
                        NumTotalRegistro += totalRegistroInserido;
                    }
                }
                //MONTA LOTE PARA PAGAMENTO COMO TED
                if (ListaTitulosTED.Count > 0)
                {
                    Lote += GeraTrailerLote(cedente, model, ListaTitulosTED, "TED", NumLot, out NumLotRetorno, out totalRegistroInserido);
                    NumLot = NumLotRetorno;
                    if (totalRegistroInserido > 0)
                    {
                        NumTotalRegistro += totalRegistroInserido;
                    }
                }

                NumTotalRegistro += 1;
                Trailerheader = GA.TrailerArquivo(NumLot, NumTotalRegistro, 0);


                Arquivo = header + Lote;

                Arquivo += Trailerheader;

                arquivoRemessa.WriteLine(Arquivo);

                arquivoRemessa.Close();
                arquivoRemessa.Dispose();
                arquivoRemessa = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        private string GeraTrailerLote(mdCedente cedente, BorderoPagVM model, IList<PAGNET_TITULOS_PAGOS> ListaPagamentos, string FormaPGTO, int NumLot, out int NumLotRetorno, out int totalRegistroInserido)
        {

            string Lote = "";

            int NumSeq = 0;//numero da linha

            int codCamaraCentralizadora = 0;
            decimal TotalValorPagar = 0;

            CNABBrasilABC240 GA = new CNABBrasilABC240();

            var AgrupamentoPorData = (from reg in ListaPagamentos
                                   group reg by new
                                   {
                                       reg.DTREALPAGAMENTO
                                   } into g
                                   select new
                                   {
                                       g.Key.DTREALPAGAMENTO
                                   }).ToList();

            foreach (var DataAgrupada in AgrupamentoPorData)
            {
                NumLot += 1;
                var ListaTitulosAgrupadoData = ListaPagamentos.Where(x => x.DTREALPAGAMENTO == DataAgrupada.DTREALPAGAMENTO).ToList();

                foreach (var Titulo in ListaTitulosAgrupadoData)
                {
                    var Trans = TransacaoTransferencia.ToEntity(Titulo);
                    Trans.EmissaoAvisoFavorecido = "0";

                    Trans.ContaCorrenteFavorecido = Geral.FormataInteiro(Trans.AgenciaFavorecido, 5) +
                                                     Geral.FormataTexto("", 1) +
                                                     Geral.FormataInteiro(Trans.ContaCorrenteFavorecido, 12) +
                                                     Geral.FormataTexto("", 1) +
                                                     Geral.FormataTexto(Trans.DigitoContaFavorecido, 1);

                    if (Titulo.CODFORMALANCAMENTO == 1)
                    {
                        codCamaraCentralizadora = 0;
                        Trans.FinalidadeDoc = "";
                        Trans.FinalidadeTed = "";
                    }
                    else
                    {
                        Trans.Valor = Titulo.VALOR;
                        codCamaraCentralizadora = 018; //ted
                        Trans.FinalidadeDoc = "";
                    }


                    if (Lote == "")
                    {
                        Lote = GA.HeaderLote(cedente, Trans, NumLot, DataAgrupada.DTREALPAGAMENTO) + Environment.NewLine;
                    }

                    TotalValorPagar += Trans.Valor;

                    NumSeq += 1; //numero da linha
                    Lote += GA.DetalheLoteSegmento_A(cedente, Trans, codCamaraCentralizadora, NumLot, NumSeq) + Environment.NewLine;

                }
            }
            NumSeq += 2; //Inclusão do cabecalho e do rodape do lote
            Lote += GA.TrailerLote(NumLot, TotalValorPagar, NumSeq) + Environment.NewLine;

            totalRegistroInserido = NumSeq;
            NumLotRetorno = NumLot;

            return Lote;

        }
        private string GeraTrailerLoteBoleto(mdCedente cedente, BorderoPagVM model, IList<PAGNET_TITULOS_PAGOS> ListaPagamentos, string FormaPGTO, int NumLot, out int NumLotRetorno, out int totalRegistroInserido)
        {

            string Lote = "";

            int NumSeq = 0;//numero da linha

            decimal TotalValorPagar = 0;

            CNABBrasilABC240 GA = new CNABBrasilABC240();

            var AgrupamentoPorData = (from reg in ListaPagamentos
                                      group reg by new
                                      {
                                          reg.DTREALPAGAMENTO
                                      } into g
                                      select new
                                      {
                                          g.Key.DTREALPAGAMENTO
                                      }).ToList();

            foreach (var DataAgrupada in AgrupamentoPorData)
            {
                NumLot += 1;
                var ListaTitulosAgrupadoData = ListaPagamentos.Where(x => x.DTREALPAGAMENTO == DataAgrupada.DTREALPAGAMENTO).ToList();


                foreach (var Titulo in ListaTitulosAgrupadoData)
                {
                    var Trans = TransacoesPagamento.ToEntity(Titulo);


                    if (Lote == "")
                    {
                        //Lote = GA.HeaderLote(cedente, Trans, NumLot, DataAgrupada.DTREALPAGAMENTO) + Environment.NewLine;
                    }

                    TotalValorPagar += Trans.Valor;

                    NumSeq += 1; //numero da linha
                    Lote += GA.DetalheLoteSegmento_J(cedente, Trans, NumLot, NumSeq) + Environment.NewLine;
                }
            }

            NumSeq += 2; //Inclusão do cabecalho e do rodape do lote
            Lote += GA.TrailerLote(NumLot, TotalValorPagar, NumSeq) + Environment.NewLine;

            totalRegistroInserido = NumSeq;
            NumLotRetorno = NumLot;

            return Lote;

        }

        private void ProcessaArquivoExtratoBancario(string caminhoArquivo)
        {
            try
            {

                List<DadosArquivoConciliacaoVM> DadosArquivo = new List<DadosArquivoConciliacaoVM>();
                MetodosGeraisdd _MetodosGeraisdd = new MetodosGeraisdd(_arquivo);
                DadosArquivoConciliacaoVM arq = new DadosArquivoConciliacaoVM();

                string[] lines = File.ReadAllLines(caminhoArquivo);

                foreach (string line in lines)
                {
                    if (line.Substring(13, 1) == "E")
                    {
                        arq = new DadosArquivoConciliacaoVM();

                        arq.ValorConciliacao = TrataValor(line.Substring(150, 18));
                        arq.DataConciliacao = DateTime.ParseExact(line.Substring(142, 18), "yyyyMMdd", null);
                        arq.DescricaoConciliacao = line.Substring(176, 25);
                        arq.TipoConciliacao = line.Substring(168, 4);
                        arq.CodigoTransacaoConciliacao = line.Substring(172, 18);
                        DadosArquivo.Add(arq);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public async Task<List<BaixaPagamentoVM>> ProcessaArquivoRetorno(string CaminhoArquivo)
        {
            try
            {
                string seuNumero = "";
                string msgRet = "";
                bool ArqRetorno = true;
                decimal valorTotal = 0;
                int qtRegistro = 0;
                int qtRegistroOK = 0;
                int qtRegistroFalha = 0;


                List<BaixaPagamentoVM> DadosRet = new List<BaixaPagamentoVM>();
                MetodosGeraisdd _MetodosGeraisdd = new MetodosGeraisdd(_arquivo);

                string[] lines = File.ReadAllLines(CaminhoArquivo);

                foreach (string line in lines)
                {
                    if (line.Substring(3, 4) == "0000" && line.Substring(142, 1) != "2")
                    {
                        ArqRetorno = false;
                        break;
                    }
                    if (line.Substring(13, 1) == "A")
                    {
                        seuNumero = line.Substring(73, 20).Trim();

                        var pagamento = _pag.GetTransacaoBySeuNumero(seuNumero).Result;

                        if (pagamento != null)
                        {
                            qtRegistro += 1;
                            valorTotal += pagamento.VALOR;
                            if (line.Length > 231)
                            {
                                if (line.Substring(230, 2).Trim() != "")
                                {
                                    msgRet = _ocorrencia.ReturnOcorrencia(line.Substring(230, 2).Trim()).Trim() + "; ";
                                }
                                if (line.Length > 233)
                                {
                                    if (line.Substring(232, 2).Trim() != "")
                                    {
                                        msgRet += _ocorrencia.ReturnOcorrencia(line.Substring(232, 2).Trim()).Trim() + "; ";
                                    }
                                    if (line.Length > 235)
                                    {
                                        if (line.Substring(234, 2).Trim() != "")
                                        {
                                            msgRet += _ocorrencia.ReturnOcorrencia(line.Substring(234, 2).Trim()).Trim() + "; ";
                                        }
                                        if (line.Length > 237)
                                        {
                                            if (line.Substring(236, 2).Trim() != "")
                                            {
                                                msgRet += _ocorrencia.ReturnOcorrencia(line.Substring(236, 2).Trim()).Trim() + "; ";
                                            }
                                            if (line.Length > 239)
                                            {
                                                if (line.Substring(238, 2).Trim() != "")
                                                {
                                                    msgRet += _ocorrencia.ReturnOcorrencia(line.Substring(238, 2).Trim()).Trim() + "; ";
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            if (line.Substring(230, 2).Trim() == "00")
                                qtRegistroOK += 1;
                            else if (line.Substring(230, 2).Trim() == "BD")
                                qtRegistroOK += 1;
                            else
                                qtRegistroFalha += 1;


                            DadosRet.Add(new BaixaPagamentoVM()
                            {
                                SeuNumero = pagamento.SEUNUMERO,
                                CodRetorno = line.Substring(230, line.Length - 230).Trim(),
                                Status = (string.IsNullOrWhiteSpace(msgRet)) ? "EM BORDERO" : _MetodosGeraisdd.validaMsgRetorno(line.Substring(230, line.Length - 230).Trim(), (int)pagamento.CODARQUIVO),
                                RAZSOC = Geral.FormataTexto(pagamento.PAGNET_CADFAVORECIDO.NMFAVORECIDO, 30),
                                CNPJ = Geral.FormataCPFCnPj(pagamento.PAGNET_CADFAVORECIDO.CPFCNPJ),
                                DATPGTO = pagamento.DTREALPAGAMENTO.ToShortDateString(),
                                VALLIQ = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", pagamento.VALOR),
                                MsgRetBanco = msgRet.Trim(),
                                MsgRetorno = ""
                            });

                        }
                    }
                }
                string ResumoArquivo = "Valor Total do Arquivo: " + string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", valorTotal) + ". "
                                     + "Quantidade de registros no arquivo: " + qtRegistro + ". "
                                     + "Total liquidados: " + qtRegistroOK + ". "
                                     + "Total Recusados: " + qtRegistroFalha + ".";
                if (!ArqRetorno)
                {
                    DadosRet = new List<BaixaPagamentoVM>();
                    DadosRet.Add(new BaixaPagamentoVM()
                    {
                        MsgRetorno = "Tipo de arquivo inválido. O arquivo não é um arquivo de retorno."
                    });
                }
                foreach (var linha in DadosRet)
                {
                    linha.Resumo = ResumoArquivo;
                    linha.vlTotalArquivo = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", valorTotal);
                    linha.qtRegistroArquivo = qtRegistro;
                    linha.qtRegistroOK = qtRegistroOK;
                    linha.qtRegistroFalha = qtRegistroFalha;
                    break;
                }
                return DadosRet;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private static decimal TrataValor(string valor)
        {
            valor = Geral.RemoveCaracteres(valor);
            string ValFinal = "";
            if (valor.Length >= 3)
            {
                string Val1 = valor.Substring(0, valor.Length - 2);
                string Val2 = valor.Substring(valor.Length - 2, 2);
                ValFinal = Val1 + "." + Val2;
            }
            else if (valor.Length == 2)
            {
                string Val2 = valor.Substring(0, 2);
                ValFinal = "0." + Val2;
            }
            else
            {
                ValFinal = "0.0" + valor;
            }

            return Convert.ToDecimal(ValFinal);
        }
    }
}
