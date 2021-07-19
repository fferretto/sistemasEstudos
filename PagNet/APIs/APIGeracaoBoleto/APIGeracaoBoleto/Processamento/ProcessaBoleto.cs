using APIGeracaoBoleto.Models;
using Boleto2Net;
using NReco.PdfGenerator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace APIGeracaoBoleto.Processamento
{
    public class ProcessaBoleto
    {
        private static int _contador = 1;
        private IBanco _banco;
        public Boletos Boletos { get; set; } = new Boletos();
        public string GeraArquivo(APIBoletoVM VMBol)
        {
            try
            {
                string msg = "";

                _banco = Banco.Instancia(Convert.ToInt32(VMBol.Cedente.contaBancaria.CodigoBanco));
                _banco.Cedente = GerarCedente(VMBol.Cedente);
                _banco.FormataCedente();

                if (File.Exists(VMBol.CaminhoArquivoRemessa))
                {
                    File.Delete(VMBol.CaminhoArquivoRemessa);
                }

                // Arquivo Remessa.
                try
                {
                    var _TipoArquivo = TipoArquivo.CNAB400;
                    if (VMBol.QuantidadePosicao == 240)
                    {
                        _TipoArquivo = TipoArquivo.CNAB240;
                    }

                    var DadosArquivoRemessa = GerarBoletos(VMBol, _banco);

                    if (VMBol.GeraArquivoRemessa)
                    {

                        var arquivoRemessa = new ArquivoRemessa(_banco, _TipoArquivo, VMBol.CodigoSequencialArquivo);
                        using (var fileStream = new FileStream(VMBol.CaminhoArquivoRemessa, FileMode.Create))
                            arquivoRemessa.GerarArquivoRemessa(DadosArquivoRemessa, fileStream);

                        if (!File.Exists(VMBol.CaminhoArquivoRemessa))
                            msg = ("Arquivo Remessa não encontrado: " + VMBol.CaminhoArquivoRemessa);
                    }
                    msg = "Sucesso";
                }
                catch (Exception e)
                {
                    if (File.Exists(VMBol.CaminhoArquivoRemessa))
                        File.Delete(VMBol.CaminhoArquivoRemessa);
                    msg = (e.InnerException.ToString());
                }

                return msg;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        public APIBoletoVM GetArquivoRetorno(APIDadosRetArquivo model)
        {
            try
            {
                string[] lines = File.ReadAllLines(model.caminhoArquivo);
                var codBanco = lines[0].Substring(0, 3);
                var len = lines[1].Length;

                _banco = Banco.Instancia(Convert.ToInt32(codBanco));


                var _TipoArquivo = TipoArquivo.CNAB400;
                if (len == 240)
                {
                    _TipoArquivo = TipoArquivo.CNAB240;
                }
                var arquivo = File.OpenRead(model.caminhoArquivo);

                Boletos boletos = new Boletos();
                var arquivoRetorno = new ArquivoRetorno(_banco, _TipoArquivo, true);

                boletos = arquivoRetorno.LerArquivoRetorno(arquivo);

                var dadosBol = LeDadosArquivoRetorno(boletos).Result;

                return dadosBol;
            }
            catch (Exception ex)
            {
                APIBoletoVM boletoVM = new APIBoletoVM();
                boletoVM.MsgRetorno = "Erro ao ler arquivo." + ex.Message;
                return boletoVM;
            }

        }

        internal static Boletos GerarBoletos(APIBoletoVM Boletos, IBanco banco)
        {
            int count = 0;
            var boletos = new Boletos
            {
                Banco = banco
            };

            foreach (var _linhaBoleto in Boletos.ListaBoletos)
            {
                var cnpj = _linhaBoleto.Sacado.CPFCNPJ.Length;
                count += 1;
                var bol = GerarBoleto(_linhaBoleto, banco, count, Boletos);

                string caminhoBoleto = Path.Combine(Boletos.CaminhoArquivoBoleto, _linhaBoleto.nmBoleto + ".pdf");

                if (Boletos.GeraBoleto)
                {
                    GeraPDFBoleto(bol, caminhoBoleto);
                }

                boletos.Add(bol);
            }
            return boletos;
        }
        internal static Boleto GerarBoleto(APIListaBoletoVM bol, IBanco banco, int i, APIBoletoVM Boletos)
        {
            try
            {
                if (bol.Aceite == "?")
                    bol.Aceite = _contador % 2 == 0 ? "N" : "A";
                
                if (banco.Codigo == 104)
                {
                    banco.Cedente.CodigoDV = GeraDVCedenteCaixa(bol.NumeroControleParticipante, banco.Cedente.Codigo);
                }

                var boleto = new Boleto(banco)
                {
                    CodigoOcorrencia = bol.CodigoOcorrencia, //Registrar remessa
                    DescricaoOcorrencia = bol.DescricaoOcorrencia,

                    //NumeroDocumento = "5",
                    //NumeroControleParticipante = "6",

                    Sacado = GerarSacado(bol.Sacado),

                    DataEmissao = bol.DataEmissao,
                    DataProcessamento = DateTime.Now,
                    DataVencimento = bol.DataVencimento,
                    ValorTitulo = bol.ValorTitulo,
                    NossoNumero = Util.RemoveCaracteres(bol.NossoNumero),
                    EspecieDocumento = TipoEspecieDocumento.DM,
                    Aceite = bol.Aceite,
                    CodigoInstrucao1 = bol.CodigoInstrucao1,
                    CodigoInstrucao2 = bol.CodigoInstrucao2,
                    //DataDesconto = DateTime.Today,
                    //ValorDesconto =0,
                    DataMulta = bol.DataMulta.AddDays(1),
                    PercentualMulta = bol.PercentualMulta,
                    ValorMulta = bol.ValorMulta,
                    DataJuros = bol.DataJuros.AddDays(1),
                    PercentualJurosDia = bol.PercentualJurosDia,
                    ValorJurosDia = bol.ValorJurosDia,
                    MensagemArquivoRemessa = bol.MensagemArquivoRemessa ?? "",
                    NumeroDocumento = bol.NumeroDocumento,
                    NumeroControleParticipante = bol.NumeroControleParticipante
                };

                // Mensagem - Instruções do Caixa
                StringBuilder msgCaixa = new StringBuilder();
                if (boleto.ValorDesconto > 0)
                    msgCaixa.AppendLine("Conceder desconto de " + boleto.ValorDesconto.ToString("R$ ##,##0.00") + " até " + boleto.DataDesconto.ToString("dd/MM/yyyy") + ". ");
                if (boleto.ValorMulta > 0)
                    msgCaixa.AppendLine("Cobrar multa de " + boleto.ValorMulta.ToString("R$ ##,##0.00") + " após o vencimento. ");
                if (boleto.ValorJurosDia > 0)
                    msgCaixa.AppendLine("Cobrar juros de " + boleto.ValorJurosDia.ToString("R$ ##,##0.00") + " por dia de atraso. ");
                if (boleto.PercentualJurosDia > 0)
                    msgCaixa.AppendLine("Cobrar juros de " + boleto.PercentualJurosDia.ToString() + "% por dia de atraso. ");
                if (boleto.PercentualMulta > 0)
                    msgCaixa.AppendLine("Cobrar multa de " + boleto.PercentualMulta.ToString() + "% após o vencimento. ");


                boleto.MensagemInstrucoesCaixa = msgCaixa.ToString();
                // Avalista
                if (_contador % 3 == 0)
                {
                    boleto.Avalista = GerarSacado(bol.Sacado);
                    boleto.Avalista.Nome = boleto.Avalista.Nome.Replace("Sacado", "Avalista");
                }

                boleto.ValidarDados();
                _contador++;
                return boleto;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GeraDVCedenteCaixa(string SeuNumero, string codigoCedente)
        {
            string dv = "0";

            string codigoUtilizandoparaCalculo = codigoCedente + SeuNumero;
            string resto = codigoUtilizandoparaCalculo;
            int valorParaCalculo = 0;
            int multiplicador = 1;
            int ResultadoEquacao = 0;
            while (resto.Length > 0)
            {
                multiplicador += 1;
                if (multiplicador > 9) multiplicador = 2;

                valorParaCalculo = Convert.ToInt32(resto.Substring(resto.Length - 1, 1));
                ResultadoEquacao = valorParaCalculo * multiplicador;

                resto = resto.Substring(0, resto.Length - 1);
            }

            if (ResultadoEquacao > 11)
            {
                ResultadoEquacao = ResultadoEquacao % 11;
            }
            ResultadoEquacao = 11 - ResultadoEquacao;
            if (ResultadoEquacao > 9) ResultadoEquacao = 0;


            dv = ResultadoEquacao.ToString();


            return dv;
        }
        private async Task<APIBoletoVM> LeDadosArquivoRetorno(Boletos ListaBoletos)
        {
            List<APIListaBoletoVM> ApiBol = new List<APIListaBoletoVM>();

            foreach (var bol in ListaBoletos)
            {
                APIListaBoletoVM api = new APIListaBoletoVM();

                api.Sacado = GerarSacadoRetorno(bol.Sacado);
                api.Avalista = GerarSacadoRetorno(bol.Avalista);
                api.CodigoOcorrencia = bol.CodigoOcorrencia;
                api.DescricaoOcorrencia = bol.DescricaoOcorrencia;
                api.CodOcorrenciaAuxiliar = bol.CodigoOcorrenciaAuxiliar;
                api.PercentualJurosDia = bol.PercentualJurosDia;
                api.DataJuros = bol.DataJuros;
                api.PercentualMulta = bol.PercentualMulta;
                api.DataMulta = bol.DataMulta;
                api.CodigoInstrucao1 = bol.CodigoInstrucao1;
                api.ComplementoInstrucao1 = bol.ComplementoInstrucao1;
                api.CodigoInstrucao2 = bol.CodigoInstrucao2;
                api.ComplementoInstrucao2 = bol.ComplementoInstrucao2;
                api.CodigoInstrucao3 = bol.CodigoInstrucao3;
                api.ComplementoInstrucao3 = bol.ComplementoInstrucao3;
                api.MensagemInstrucoesCaixa = bol.MensagemInstrucoesCaixa;
                api.MensagemArquivoRemessa = bol.MensagemArquivoRemessa;
                api.EspecieDocumento = bol.EspecieDocumento;
                api.NossoNumero = bol.NossoNumero;
                api.DataEmissao = bol.DataEmissao;
                api.DataVencimento = bol.DataVencimento;
                api.NumeroDocumento = bol.NumeroDocumento;
                api.NumeroControleParticipante = bol.NumeroControleParticipante;
                api.Aceite = bol.Aceite;
                api.ValorTitulo = bol.ValorTitulo;
                api.ValorJurosDia = bol.ValorJurosDia;
                api.ValorMulta = bol.ValorMulta;
                api.DataProcessamento = bol.DataProcessamento;


                ApiBol.Add(api);
            }

            APIBoletoVM DadosRetBol = new APIBoletoVM();
            DadosRetBol.ListaBoletos = ApiBol;

            return DadosRetBol;
        }
        internal Cedente GerarCedente(APIBoletoCedenteVM _cedente)
        {
            try
            {
                var CB = new ContaBancaria
                {
                    CodigoBancoCorrespondente = Convert.ToInt32(_cedente.contaBancaria.CodigoBanco),
                    Agencia = _cedente.contaBancaria.Agencia.Trim(),
                    DigitoAgencia = _cedente.contaBancaria.DigitoAgencia.Trim(),
                    Conta = _cedente.contaBancaria.Conta.Trim(),
                    OperacaoConta = _cedente.contaBancaria.OperacaoConta,
                    DigitoConta = _cedente.contaBancaria.DigitoConta.Trim(),
                    CarteiraPadrao = _cedente.contaBancaria.CarteiraPadrao,
                    TipoCarteiraPadrao = _cedente.contaBancaria.TipoCarteiraPadrao,
                    TipoFormaCadastramento = _cedente.contaBancaria.TipoFormaCadastramento,
                    TipoImpressaoBoleto = _cedente.contaBancaria.TipoImpressaoBoleto,
                    VariacaoCarteiraPadrao = _cedente.contaBancaria.VariacaoCarteiraPadrao
                };
                if (_cedente.contaBancaria.CodigoBanco == "756")
                {
                    CB.CodigoBancoCorrespondente = 0;
                }
                Cedente ced = new Cedente();

                ced.CPFCNPJ = _cedente.CPFCNPJ;
                ced.Nome = _cedente.Nome;
                ced.Codigo = _cedente.Codigo;
                ced.CodigoDV = _cedente.CodigoDV;
                ced.CodigoTransmissao = _cedente.CodigoTransmissao;
                ced.MostrarCNPJnoBoleto = _cedente.MostrarCNPJnoBoleto;
                ced.Endereco = new Endereco
                {
                    LogradouroEndereco = _cedente.Endereco.LogradouroEndereco,
                    LogradouroNumero = _cedente.Endereco.LogradouroNumero,
                    LogradouroComplemento = _cedente.Endereco.LogradouroComplemento,
                    Bairro = _cedente.Endereco.Bairro,
                    Cidade = _cedente.Endereco.Cidade,
                    UF = _cedente.Endereco.UF,
                    CEP = _cedente.Endereco.CEP,
                };
                ced.ContaBancaria = CB;

                return ced;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        internal static Sacado GerarSacado(APIBoletoSacadoVM dados)
        {
            return new Sacado
            {
                CPFCNPJ = dados.CPFCNPJ,
                Nome = dados.Nome,
                Observacoes = dados.Observacoes,
                Endereco = new Endereco
                {
                    LogradouroEndereco = dados.Endereco.LogradouroEndereco,
                    LogradouroNumero = (Convert.ToString(dados.Endereco.LogradouroNumero) == "") ? "0" : Convert.ToString(dados.Endereco.LogradouroNumero),
                    Bairro = dados.Endereco.Bairro,
                    Cidade = dados.Endereco.Cidade,
                    UF = dados.Endereco.UF,
                    CEP = dados.Endereco.CEP
                }
            };
        }
        internal static APIBoletoSacadoVM GerarSacadoRetorno(Sacado dados)
        {
            return new APIBoletoSacadoVM
            {
                CPFCNPJ = dados.CPFCNPJ,
                Nome = dados.Nome,
                Observacoes = dados.Observacoes,
                Endereco = new APIEnderecoVM
                {
                    LogradouroEndereco = dados.Endereco.LogradouroEndereco,
                    LogradouroNumero = (Convert.ToString(dados.Endereco.LogradouroNumero) == "") ? "0" : Convert.ToString(dados.Endereco.LogradouroNumero),
                    Bairro = dados.Endereco.Bairro,
                    Cidade = dados.Endereco.Cidade,
                    UF = dados.Endereco.UF,
                    CEP = dados.Endereco.CEP
                }
            };
        }
        private static void GeraPDFBoleto(Boleto _dadosBoleto, string caminhoBoleto)
        {
            // Gera arquivo PDF
            try
            {
                if (File.Exists(caminhoBoleto))
                {
                    File.Delete(caminhoBoleto);
                }

                var html = new StringBuilder();

                BoletoBancario bol = new BoletoBancario();

                bol.Boleto = _dadosBoleto;
                bol.OcultarInstrucoes = false;
                bol.MostrarComprovanteEntrega = false;
                bol.MostrarEnderecoCedente = true;
                bol.ExibirDemonstrativo = true;

                html.Append("<div style=\"page-break-after: always;\">");
                html.Append(bol.MontaHtml());
                html.Append("</div>");

                var pdf = new HtmlToPdfConverter().GeneratePdf(html.ToString());
                using (var fs = new FileStream(caminhoBoleto, FileMode.Create))
                    fs.Write(pdf, 0, pdf.Length);


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}