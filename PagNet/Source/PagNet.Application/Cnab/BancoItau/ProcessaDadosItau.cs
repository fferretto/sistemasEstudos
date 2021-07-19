using PagNet.Application.Helpers;
using PagNet.Application.Models;
using PagNet.Domain.Interface.Services;
using System;
using System.Linq;
using System.IO;
using PagNet.Application.Interface.ProcessoCnab;
using System.Threading.Tasks;
using PagNet.Application.Cnab.Comun;
using System.Collections.Generic;
using System.Globalization;
using PagNet.Domain.Entities;

namespace PagNet.Application.Cnab.BancoItau
{
    public class ProcessaDadosItau : IProcessaDadosItau
    {
        private readonly IPagNet_ContaCorrenteService _conta;
        private readonly IPagNet_ArquivoService _arquivo;
        private readonly IPagNet_OcorrenciaRetPagService _ocorrencia;
        private readonly IPagNet_Titulos_PagosService _pag;
        private readonly IPagNet_CadFavorecidoService _favorito;
        private readonly IPagNet_CadEmpresaService _empresa;

        public ProcessaDadosItau(IPagNet_OcorrenciaRetPagService ocorrencia,
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

        public async Task<string> GeraArquivoItau(BorderoPagVM model, int codArquivo)
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
                int NumTotalRegistro = 0;//Total de Registro

                CnabItau240 GA = new CnabItau240();
                StreamWriter arquivoRemessa = new StreamWriter(arquivo, true);

                // Header do Arquivo
                NumTotalRegistro = 1;
                header = GA.HeaderArquivo(cedente) + Environment.NewLine;

                var TitulosPagamento = _pag.GetPagamentosByCodArquivo(codArquivo);

                var ListaTitulosTED = TitulosPagamento.Where(x => x.PAGNET_CADFAVORECIDO.BANCO != "341" && x.TIPOTITULO == "TEDDOC").ToList();
                //var ListaTitulosDOC = listaFechamento.Where(x => x.FormaPGTO == "DOC" && x.CODBANCO != "0237").ToList();
                var ListaTitulosCC = TitulosPagamento.Where(x => x.PAGNET_CADFAVORECIDO.BANCO == "341" && x.TIPOTITULO == "TEDDOC").ToList();

                var ListaTitulosBoleto = TitulosPagamento.Where(x => x.TIPOTITULO == "BOLETO").ToList();

                //MONTA LOTE PARA PAGAMENTO DE BOLETOS BANCÁRIOS
                if (ListaTitulosBoleto.Count > 0)
                {
                    NumLot += 1;
                    Lote += GeraTrailerLoteBoleto(cedente, model, ListaTitulosTED, "BOLETO", NumLot, out totalRegistroInserido);

                    if (totalRegistroInserido > 0)
                    {
                        NumTotalRegistro += totalRegistroInserido;
                    }
                }

                //MONTA LOTE PARA PAGAMENTO COMO CRÉDITO EM CONTA
                if (ListaTitulosCC.Count > 0)
                {
                    NumLot += 1;
                    Lote += GeraTrailerLote(cedente, model, ListaTitulosCC, "CC", NumLot, out totalRegistroInserido);

                    if (totalRegistroInserido > 0)
                    {
                        NumTotalRegistro += totalRegistroInserido;
                    }
                }
                //MONTA LOTE PARA PAGAMENTO COMO TED
                if (ListaTitulosTED.Count > 0)
                {
                    NumLot += 1;
                    Lote += GeraTrailerLote(cedente, model, ListaTitulosTED, "TED", NumLot, out totalRegistroInserido);

                    if (totalRegistroInserido > 0)
                    {
                        NumTotalRegistro += totalRegistroInserido;
                    }
                }

                NumTotalRegistro += 1;
                Trailerheader = GA.TrailerArquivo(NumLot, NumTotalRegistro);


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
        private string GeraTrailerLote(mdCedente cedente, BorderoPagVM model, IList<PAGNET_TITULOS_PAGOS> ListaPagamentos, string FormaPGTO, int NumLot, out int totalRegistroInserido)
        {

            string Lote = "";

            int NumSeq = 0;//numero da linha

            int codCamaraCentralizadora = 0;
            decimal TotalValorPagar = 0;

            CnabItau240 GA = new CnabItau240();


            foreach (var Titulo in ListaPagamentos)
            {
                //Caso a forma de pagamento seja crédito em conta mas o banco é diferente.
                if (FormaPGTO == "CC" && Titulo.PAGNET_CADFAVORECIDO.BANCO != "341")
                {
                    totalRegistroInserido = 0;
                    return "";
                }

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
                    Trans.FinalidadeTed = "";
                    Trans.FinalidadeDoc = "";

                }
                else
                {
                    Trans.Valor = Titulo.VALOR;

                    if (Titulo.CODFORMALANCAMENTO == 3)
                    {
                        codCamaraCentralizadora = 700;
                        Trans.FinalidadeTed = "";
                    }
                    else
                    {
                        codCamaraCentralizadora = 018;
                        Trans.FinalidadeDoc = "";
                    }
                }

                if (Lote == "")
                {
                    Lote = GA.HeaderLote(cedente, Trans, NumLot) + Environment.NewLine;
                }

                TotalValorPagar += Trans.Valor;

                NumSeq += 1; //numero da linha
                Lote += GA.DetalheLoteSegmento_A(cedente, Trans, codCamaraCentralizadora, NumLot, NumSeq) + Environment.NewLine;
                NumSeq += 1;//numero da linha
                Lote += GA.DetalheLoteSegmento_B(cedente, Trans, NumLot, NumSeq) + Environment.NewLine;

            }
            NumSeq += 2; //Inclusão do cabecalho e do rodape do lote
            Lote += GA.TrailerLote(NumLot, TotalValorPagar, NumSeq) + Environment.NewLine;

            totalRegistroInserido = NumSeq;

            return Lote;

        }
        private string GeraTrailerLoteBoleto(mdCedente cedente, BorderoPagVM model, IList<PAGNET_TITULOS_PAGOS> ListaPagamentos, string FormaPGTO, int NumLot, out int totalRegistroInserido)
        {

            string Lote = "";

            int NumSeq = 0;//numero da linha

            decimal TotalValorPagar = 0;

            CnabItau240 GA = new CnabItau240();


            foreach (var Titulo in ListaPagamentos)
            {
                var Trans = TransacoesPagamento.ToEntity(Titulo);


                if (Lote == "")
                {
                    Lote = GA.HeaderLotePagBoleto(cedente, Trans, NumLot) + Environment.NewLine;
                }

                TotalValorPagar += Trans.Valor;

                NumSeq += 1; //numero da linha
                Lote += GA.DetalheLoteSegmento_J(cedente, Trans, NumLot, NumSeq) + Environment.NewLine;
            }

            NumSeq += 2; //Inclusão do cabecalho e do rodape do lote
            Lote += GA.TrailerLote(NumLot, TotalValorPagar, NumSeq) + Environment.NewLine;

            totalRegistroInserido = NumSeq;

            return Lote;

        }

    }
}
