using Boleto2Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APIGeracaoBoleto.Models
{
    public class APIBoletoVM
    {

        public APIBoletoVM()
        {
            Cedente = new APIBoletoCedenteVM();
            ListaBoletos = new List<APIListaBoletoVM>();
        }


        public APIBoletoCedenteVM Cedente { get; set; }
        public List<APIListaBoletoVM> ListaBoletos { get; set; }

        public string CaminhoArquivoRemessa { get; set; }
        public string CaminhoArquivoRetorno { get; set; }
        public string CaminhoArquivoBoleto { get; set; }
        public int CodigoSequencialArquivo { get; set; }
        public int QuantidadePosicao { get; set; }

        public string MsgRetorno { get; set; }


        public bool GeraArquivoRemessa { get; set; }
        public bool GeraBoleto { get; set; }

    }
    public class APIBoletoCedenteVM
    {
        public APIBoletoCedenteVM()
        {
            contaBancaria = new APIContaBancariaVM();
            Endereco = new APIEnderecoVM();
        }
        public string Codigo { get; set; }
        public string CodigoDV { get; set; }
        public string CodigoFormatado { get; set; }
        public string CodigoTransmissao { get; set; }
        public string CPFCNPJ { get; set; }
        public string Nome { get; set; }
        public string Observacoes { get; set; }
        public bool MostrarCNPJnoBoleto { get; set; }

        public APIContaBancariaVM contaBancaria { get; set; }
        public APIEnderecoVM Endereco { get; set; }

    }
    public class APIListaBoletoVM
    {
        public APIListaBoletoVM()
        {
            Sacado = new APIBoletoSacadoVM();
            Avalista = new APIBoletoSacadoVM();
        }
        public APIBoletoSacadoVM Sacado { get; set; }
        public APIBoletoSacadoVM Avalista { get; set; }

        public string CodigoOcorrencia { get; set; }
        public string DescricaoOcorrencia { get; set; }
        public string CodOcorrenciaAuxiliar { get; set; }

        public string nmBoleto { get; set; }
        public decimal PercentualJurosDia { get; set; }
        public DateTime DataJuros { get; set; }
        public decimal PercentualMulta { get; set; }
        public DateTime DataMulta { get; set; }
        public string CodigoInstrucao1 { get; set; }
        public string ComplementoInstrucao1 { get; set; }
        public string CodigoInstrucao2 { get; set; }
        public string ComplementoInstrucao2 { get; set; }
        public string CodigoInstrucao3 { get; set; }
        public string ComplementoInstrucao3 { get; set; }
        public string MensagemInstrucoesCaixa { get; set; }
        public string MensagemArquivoRemessa { get; set; }
        public TipoEspecieDocumento EspecieDocumento { get; set; }
        public string NossoNumero { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
        public string NumeroDocumento { get; set; }
        public string NumeroControleParticipante { get; set; }
        public string Aceite { get; set; }
        public decimal ValorTitulo { get; set; }
        public decimal ValorJurosDia { get; set; }
        public decimal ValorMulta { get; set; }
        public DateTime DataProcessamento { get; set; }


    }
    public class APIBoletoSacadoVM
    {
        public APIBoletoSacadoVM()
        {
            Endereco = new APIEnderecoVM();
        }
        public string Nome { get; set; }
        public string Observacoes { get; set; }
        public string CPFCNPJ { get; set; }
        public APIEnderecoVM Endereco { get; set; }

    }
    public class APIContaBancariaVM
    {
        public string CodigoBanco { get; set; }
        public string Conta { get; set; }
        public string CarteiraPadrao { get; set; }
        public string OperacaoConta { get; set; }
        public string VariacaoCarteiraPadrao { get; set; }
        public string DigitoAgencia { get; set; }
        public string Agencia { get; set; }
        public string DigitoConta { get; set; }
        public TipoCarteira TipoCarteiraPadrao { get; set; }
        public TipoFormaCadastramento TipoFormaCadastramento { get; set; }
        public TipoImpressaoBoleto TipoImpressaoBoleto { get; set; }


    }
    public class APIEnderecoVM
    {
        public string LogradouroEndereco { get; set; }
        public string LogradouroNumero { get; set; }
        public string LogradouroComplemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }


    }
    public class APIDadosRetArquivo
    {
        public string caminhoArquivo { get; set; }
    }
    public class APIDetalhamentoFaturaReembolsoVm
    {
        public APIDetalhamentoFaturaReembolsoVm()
        {
            Detalhamento = new List<APIDetalhamentoValoresCobradosFaturaVm>();
        }

        public IList<APIDetalhamentoValoresCobradosFaturaVm> Detalhamento { get; set; }

        public string caminhoArquivo { get; set; }
        public string nomeArquivoPDF { get; set; }

        public string nroDocumento { get; set; }
        public string datEmissao { get; set; }
        public string datVencimento { get; set; }
        public string vlTotal { get; set; }

        //Dados Credor
        public string Credor { get; set; }
        public string CNPJCredor { get; set; }
        public string CEPCredor { get; set; }
        public string EnderecoCredor { get; set; }
        public string nroCredor { get; set; }
        public string ComplementoCredor { get; set; }
        public string BairroCredor { get; set; }
        public string CidadeCredor { get; set; }
        public string EstadoCredor { get; set; }
        public string TelefoneCredor { get; set; }

        //Dados Devedor
        public string Devedor { get; set; }
        public string CNPJDevedor { get; set; }
        public string CEPDevedor { get; set; }
        public string EnderecoDevedor { get; set; }
        public string nroDevedor { get; set; }
        public string ComplementoDevedor { get; set; }
        public string BairroDevedor { get; set; }
        public string CidadeDevedor { get; set; }
        public string EstadoDevedor { get; set; }
        public string TelefoneDevedor { get; set; }

        public string urlRelatorio { get; set; }
    }
    public class APIDetalhamentoValoresCobradosFaturaVm
    {
        public string Descricao { get; set; }
        public string Valor { get; set; }

    }
}

