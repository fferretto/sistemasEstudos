﻿using PagNet.Bld.Domain.Entities;
using PagNet.Bld.Domain.Interface;
using PagNet.Bld.Domain.Interface.Services;
using PagNet.Bld.PGTO.Bradesco.Abstraction.Interface;
using PagNet.Bld.PGTO.Bradesco.Abstraction.Interface.Model;
using PagNet.Bld.PGTO.Bradesco.Abstraction.Model;
using PagNet.Bld.PGTO.Bradesco.CNAB;
using PagNet.Bld.PGTO.Bradesco.ModelAuxiliar;
using PagNet.Bld.PGTO.Bradesco.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Telenet.BusinessLogicModel;
using static PagNet.Bld.PGTO.Bradesco.Constants;

namespace PagNet.Bld.PGTO.Bradesco.Application
{
    public class PagamentoApp : Service<IContextoApp>, IPagamentoApp
    {
        private readonly IParametrosApp _user;
        private readonly IPAGNET_TITULOS_PAGOSService _pagamento;

        public PagamentoApp(IContextoApp contexto,
                            IParametrosApp user,
                            IPAGNET_TITULOS_PAGOSService pagamento)
            : base(contexto)
        {
            _user = user;
            _pagamento = pagamento;
        }
        public ResultadoTransmissaoArquivo GeraArquivoRemessa(IFiltroTransmissaoBancoVM model)
        {
            try
            {
                ResultadoTransmissaoArquivo dadosRetorno = new ResultadoTransmissaoArquivo();

                dadosRetorno.Resultado = true;
                if (model.ListaBorderosPGTO.Count > 0)
                {
                    var ResultadoGeracaoArquivo = GerarArquivoRemessa(model);

                    dadosRetorno.Resultado = ResultadoGeracaoArquivo.FirstOrDefault().Key;
                    dadosRetorno.msgResultado = ResultadoGeracaoArquivo.FirstOrDefault().Value;
                }

                return dadosRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public IDictionary<bool, string> GerarArquivoRemessa(IFiltroTransmissaoBancoVM model)
        {
            var ResultadoProcessamento = new Dictionary<bool, string>();
            try
            {
                string Arquivo = "";
                string Lote = "";
                string header;
                string Trailerheader = "";


                int totalRegistroInserido = 0;
                int NumLot = 0;//numero do lote
                int NumTotalRegistro = 0;//Total de Registro

                CnabBradesco240 GA = new CnabBradesco240();
                StreamWriter arquivoRemessa = new StreamWriter(model.CaminhoArquivo, true);

                // Header do Arquivo
                NumTotalRegistro = 1;
                model.cedente.NumSeq = 1;
                header = GA.HeaderArquivo(model.cedente) + Environment.NewLine;

                var TitulosPagamento = _pagamento.GetPagamentosByCodArquivo(model.codigoArquivo);
                var TitulosTransferencias = TitulosPagamento.Where(x => x.TIPOTITULO == "TEDDOC").ToList();

                var ListaTitulosTED = TitulosTransferencias.Where(x => x.PAGNET_CADFAVORECIDO.BANCO != "237" && x.TIPOTITULO == "TEDDOC").ToList();
                //var ListaTitulosDOC = TitulosTransferencias.Where(x => x.FormaPGTO == "DOC" && x.CODBANCO != "0237" && x.TIPOTITULO == "TEDDOC").ToList();
                var ListaTitulosCC = TitulosTransferencias.Where(x => x.PAGNET_CADFAVORECIDO.BANCO == "237" && x.TIPOTITULO == "TEDDOC").ToList();
                var ListaTitulosBoleto = TitulosPagamento.Where(x => x.TIPOTITULO == "BOLETO").ToList();

                //MONTA LOTE PARA PAGAMENTO DE BOLETOS BANCÁRIOS
                if (ListaTitulosBoleto.Count > 0)
                {
                    NumLot += 1;
                    Lote += GeraTrailerLoteBoleto(model.cedente, ListaTitulosBoleto, "BOLETO", NumLot, out totalRegistroInserido);

                    if (totalRegistroInserido > 0)
                    {
                        NumTotalRegistro += totalRegistroInserido;
                    }
                }
                //MONTA LOTE PARA PAGAMENTO COMO CRÉDITO EM CONTA
                if (ListaTitulosCC.Count > 0)
                {
                    NumLot += 1;
                    Lote += GeraTrailerLote(model.cedente, ListaTitulosCC, "CC", NumLot, out totalRegistroInserido);

                    if (totalRegistroInserido > 0)
                    {
                        NumTotalRegistro += totalRegistroInserido;
                    }
                }
                //MONTA LOTE PARA PAGAMENTO COMO TED
                if (ListaTitulosTED.Count > 0)
                {
                    NumLot += 1;
                    Lote += GeraTrailerLote(model.cedente, ListaTitulosTED, "TED", NumLot, out totalRegistroInserido);

                    if (totalRegistroInserido > 0)
                    {
                        NumTotalRegistro += totalRegistroInserido;
                    }
                }

                NumTotalRegistro += 1;
                Trailerheader = GA.TrailerArquivo(NumLot, NumTotalRegistro, 0);


                Arquivo = header + Lote;

                Arquivo += Trailerheader;

                arquivoRemessa.Write(Arquivo);

                arquivoRemessa.Close();
                arquivoRemessa.Dispose();
                arquivoRemessa = null;

                ResultadoProcessamento.Add(true, "Arquivo gerado com sucesso");
            }
            catch (Exception ex)
            {
                ResultadoProcessamento.Add(false, "Falha durante o processo de criação do arquivo de remessa. Favor contactar o suporte.");
                throw ex;
            }
            return ResultadoProcessamento;

        }
        private string GeraTrailerLote(mdCedente cedente, List<PAGNET_TITULOS_PAGOS> ListaPagamentos, string FormaPGTO, int NumLot, out int totalRegistroInserido)
        {

            string Lote = "";

            int NumSeq = 0;//numero da linha

            int codCamaraCentralizadora = 0;
            decimal TotalValorPagar = 0;

            CnabBradesco240 GA = new CnabBradesco240();


            foreach (var Titulo in ListaPagamentos)
            {
                //Caso a forma de pagamento seja crédito em conta mas o banco é diferente.
                if (FormaPGTO == "CC" && Titulo.PAGNET_CADFAVORECIDO.BANCO != "237")
                {
                    totalRegistroInserido = 0;
                    return "";
                }

                var Trans = TransacaoTransferencia.ToEntity(Titulo);
                Trans.EmissaoAvisoFavorecido = "0";

                if (Titulo.CODFORMALANCAMENTO == 1)
                {
                    codCamaraCentralizadora = 0;
                    Trans.FinalidadeDoc = "";
                    Trans.FinalidadeTed = "";
                }
                else
                {
                    Trans.Valor = Titulo.VALOR;

                    //if (Titulo.CODFORMALANCAMENTO == 3)
                    //{
                    //    codCamaraCentralizadora = 700; //doc
                    //    Trans.FinalidadeTed = "";
                    //}
                    //else 
                    //{
                    codCamaraCentralizadora = 018; //ted
                    Trans.FinalidadeDoc = "";
                    //}
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
            //var Valor_aux = ListaPagamentos.Select(x => x.VALOR).Sum();
            //var aux = Valor_aux - TotalValorPagar;
            NumSeq += 2; //Inclusão do cabecalho e do rodape do lote
            Lote += GA.TrailerLote(NumLot, TotalValorPagar, NumSeq) + Environment.NewLine;

            totalRegistroInserido = NumSeq;

            return Lote;

        }
        private string GeraTrailerLoteBoleto(mdCedente cedente, IList<PAGNET_TITULOS_PAGOS> ListaPagamentos, string FormaPGTO, int NumLot, out int totalRegistroInserido)
        {

            string Lote = "";

            int NumSeq = 0;//numero da linha

            decimal TotalValorPagar = 0;

            CnabBradesco240 GA = new CnabBradesco240();

            foreach (var Titulo in ListaPagamentos)
            {
                var Trans = TransacoesPagamento.ToEntity(Titulo);


                if (Lote == "")
                {
                    Lote = GA.HeaderLotePagBoleto(cedente, Trans, NumLot) + Environment.NewLine;
                }

                TotalValorPagar += Trans.Valor;

                NumSeq += 1; //numero da linha
                if (Trans.codFormaLancamento == 11)//boleto de arrecadação (agua, luz, telefone, etc...)
                {
                    Lote += GA.DetalheLoteSegmento_O(cedente, Trans, NumLot, NumSeq) + Environment.NewLine;
                }
                else//boleto de cobrança normal
                {
                    Lote += GA.DetalheLoteSegmento_J(cedente, Trans, NumLot, NumSeq) + Environment.NewLine;
                }
            }

            NumSeq += 2; //Inclusão do cabecalho e do rodape do lote
            Lote += GA.TrailerLote(NumLot, TotalValorPagar, NumSeq) + Environment.NewLine;

            totalRegistroInserido = NumSeq;

            return Lote;

        }
    }
}
