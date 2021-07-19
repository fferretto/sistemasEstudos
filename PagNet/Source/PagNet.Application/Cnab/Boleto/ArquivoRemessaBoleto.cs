using Boleto2Net;
using PagNet.Application.Interface.ProcessoCnab;
using PagNet.Application.Models;
using PagNet.Domain.Entities;
using PagNet.Domain.Interface.Services;
using System;
using System.IO;
using System.Text;

namespace PagNet.Application.Cnab.Boleto
{
    public class ArquivoRemessaBoleto : IArquivoRemessaBoleto
    {
        private static int _contador = 1;
        private static int _proximoNossoNumero = 1;

        private readonly IPagNet_ArquivoService _arquivo;
        private readonly IPagNet_EmissaoBoletoService _emissaoBoleto;
        private readonly IPagNet_CadClienteService _cliente;

        public ArquivoRemessaBoleto(IPagNet_ArquivoService arquivo,
                                    IPagNet_CadClienteService cliente,
                                    IPagNet_EmissaoBoletoService emissaoBoleto)
        {
            _arquivo = arquivo;
            _emissaoBoleto = emissaoBoleto;
            _cliente = cliente;
        }
        public void GeraRemessaBoleto(BorderoReceitaVm Boletos, IBanco banco, TipoArquivo tipoArquivo, string nmArquivo, string CaminhoArquivo)
        {
            var boletos = GerarBoletos(Boletos, banco, nmArquivo);

            string msg = "";
            // Define os nomes dos arquivos, cria pasta e apaga arquivos anteriores
            var nomeArquivoREM = Path.Combine(CaminhoArquivo, nmArquivo + ".REM");

            if (!Directory.Exists(Path.GetDirectoryName(nomeArquivoREM)))
                Directory.CreateDirectory(Path.GetDirectoryName(nomeArquivoREM));
            if (File.Exists(nomeArquivoREM))
            {
                File.Delete(nomeArquivoREM);
            }

            // Arquivo Remessa.
            try
            {
                var arquivoRemessa = new ArquivoRemessa(boletos.Banco, tipoArquivo, 1);
                using (var fileStream = new FileStream(nomeArquivoREM, FileMode.Create))
                    arquivoRemessa.GerarArquivoRemessa(boletos, fileStream);

                if (!File.Exists(nomeArquivoREM))
                    msg = ("Arquivo Remessa não encontrado: " + nomeArquivoREM);
            }
            catch (Exception e)
            {
                if (File.Exists(nomeArquivoREM))
                    File.Delete(nomeArquivoREM);
                msg = (e.InnerException.ToString());
            }
        }
        internal Boletos GerarBoletos(BorderoReceitaVm Boletos, IBanco banco, string nmArquivo)
        {
            int count = 0;
            var boletos = new Boletos
            {
                Banco = banco
            };

            foreach (var bol in Boletos.ListaFechamento)
            {
                count += 1;

                boletos.Add(GerarBoleto(bol, banco, count, Boletos, nmArquivo));
            }
            return boletos;
        }
        internal Boleto2Net.Boleto GerarBoleto(ListaFechClienteVM bol, IBanco banco, int i, BorderoReceitaVm FechCli, string nmArquivo)
        {
            //int codEmissaoBoleto = _emissaoBoleto.GetLastCodBoleto().Result;

            var boleto = new Boleto2Net.Boleto(banco)
            {
                Sacado = GerarSacado(bol),
                DataEmissao = DateTime.Now,
                DataProcessamento = DateTime.Now,
                DataVencimento = Convert.ToDateTime(FechCli.dtVencimento),
                ValorTitulo = Convert.ToDecimal(bol.TOTAL.Replace("R$", "").Trim()),
                //NossoNumero = Geral.GeraNossoNumero("0", codEmissaoBoleto),
                //NumeroDocumento = Geral.GeraNumeroControle(30, codEmissaoBoleto.ToString(),""),
                EspecieDocumento = TipoEspecieDocumento.DM,
                Aceite = "N",
                CodigoInstrucao1 = FechCli.codPrimeiraInstrucaoCobranca,
                CodigoInstrucao2 = FechCli.codSegundaInstrucaoCobranca,
                DataMulta = Convert.ToDateTime(FechCli.dtVencimento),
                PercentualMulta = Convert.ToDecimal(FechCli.PercMulta),
                ValorMulta = Convert.ToDecimal(FechCli.vlMulta),
                DataJuros = Convert.ToDateTime(FechCli.dtVencimento),
                PercentualJurosDia = Convert.ToDecimal(FechCli.PercJuros),
                ValorJurosDia = Convert.ToDecimal(FechCli.vlJuros),
                MensagemArquivoRemessa = FechCli.MensagemArquivoRemessa,
                //NumeroControleParticipante = Geral.GeraNumeroControle(30, codEmissaoBoleto.ToString(), ""),
            };


            int nroSeqArquivo = 5;// RetornaNroSeqArquivo();
            int codArquivo = InseriArquivo(FechCli.CaminhoArquivoRemessa, nmArquivo, nroSeqArquivo, FechCli.dtVencimento);

            FechCli.codArquivo = codArquivo;



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
                boleto.Avalista = GerarSacado(bol);
                boleto.Avalista.Nome = boleto.Avalista.Nome.Replace("Sacado", "Avalista");
            }

            boleto.ValidarDados();
            _contador++;
            _proximoNossoNumero++;

            var nomeArquivoPDF = Path.Combine(FechCli.CaminhoArquivoRemessa, boleto.NumeroControleParticipante + ".PDF");
            // Gera arquivo PDF
            try
            {
                var html = new StringBuilder();
                BoletoBancario boletobancario = new BoletoBancario();

                boletobancario.Boleto = boleto;
                boletobancario.OcultarInstrucoes = false;
                boletobancario.MostrarComprovanteEntrega = false;
                boletobancario.MostrarEnderecoCedente = true;
                boletobancario.ExibirDemonstrativo = true;



                //html.Append("<div style=\"page-break-after: always;\">");
                //html.Append(boletobancario.MontaHtml());         
                //html.Append("</div>");


                
                
            }
            catch (Exception e)
            {
                if (File.Exists(nomeArquivoPDF))
                    File.Delete(nomeArquivoPDF);
            }

            return boleto;
        }
        internal Sacado GerarSacado(ListaFechClienteVM bol)
        {
            var cliente = _cliente.BuscaClienteByID(bol.CODCLI).Result;

            string endereco = "";
            string numero = "";

            if (cliente.LOGRADOURO.Contains(","))
            {
                var end = cliente.LOGRADOURO.Split(',');
                endereco = end[0];
                numero = end[1];
            }
            else
            {
                endereco = cliente.LOGRADOURO;
            }


            return new Sacado
            {
                CPFCNPJ = bol.CGC,
                Nome = bol.NOMCLI,
                Observacoes = "",
                Endereco = new Endereco
                {
                    LogradouroEndereco = endereco,
                    LogradouroNumero = numero,
                    Bairro = cliente.BAIRRO,
                    Cidade = cliente.CIDADE,
                    UF = cliente.UF,
                    CEP = cliente.CEP.Replace("-", "")
                }
            };
        }
        public int InseriArquivo(string CaminhoArquivo, string nmArquivo, int nroSeq, string dataArquivo)
        {
            try
            {
                PAGNET_ARQUIVO arq = new PAGNET_ARQUIVO();
                arq.CAMINHOARQUIVO = CaminhoArquivo;
                arq.DTARQUIVO = Convert.ToDateTime(dataArquivo);
                arq.NMARQUIVO = nmArquivo;
                arq.NROSEQARQUIVO = nroSeq;
                arq.STATUS = "AGUARDANDO_DOWNLOAD";
                arq.TIPARQUIVO = "PAG";

                var CodArquivo = _arquivo.IncluiArquivo(arq);

                return CodArquivo;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int RetornaNroSeqArquivo()
        {
            try
            {
                int ultimoArquivo = _arquivo.GetMaxNroSequencial();

                ultimoArquivo += 1;

                if (ultimoArquivo < 11)
                {
                    ultimoArquivo = 11;
                }

                return ultimoArquivo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
