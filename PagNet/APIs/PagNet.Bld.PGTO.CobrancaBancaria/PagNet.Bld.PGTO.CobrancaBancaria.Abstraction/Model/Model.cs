using Boleto2Net;
using PagNet.Bld.Domain.Entities;
using PagNet.Bld.PGTO.CobrancaBancaria.Abstraction.Interface.Model;
using PagNet.Bld.PGTO.CobrancaBancaria.Abstraction.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PagNet.Bld.PGTO.CobrancaBancaria.Abstraction.Model
{
    public class SolicitacaoBoletoVM : ISolicitacaoBoletoVM
    {
        public int codEmissaoBoleto { get; set; }
        public string MsgRetorno { get; set; }
        public bool chkBoleto { get; set; }
        public string nomeBoleto { get; set; }
        public string BoletoRecusado { get; set; }
        public string msgRecusa { get; set; }
        public string TAXAEMISSAOBOLETO { get; set; }
        public string PERCMULTA { get; set; }
        public string VLMULTADIAATRASO { get; set; }
        public string PERCJUROS { get; set; }
        public string VLJUROSDIAATRASO { get; set; }
        public string codBordero { get; set; }
        public string codigoCliente { get; set; }
        public string nomeCliente { get; set; }
        public string nomeCompletoCliente { get; set; }
        public string cnpj { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string dtVencimento { get; set; }
        public string dtEmissao { get; set; }
        public string Valor { get; set; }
        public string OrigemBoleto { get; set; }
        public string TipoFaturamento { get; set; }
        public string SeuNumero { get; set; }
        public string nroDocumento { get; set; }
        public string DescricaoOcorrenciaRet { get; set; }
        public string CodFaturamentoPai { get; set; }
        public string nParcela { get; set; }
        public string ntotalParcelas { get; set; }
        public string ValParcela { get; set; }

    }

    public class ListaBorderoBolVM
    {
        public int codigoBordero { get; set; }

    }

    public class APIBoletoVM : IAPIBoletoVM
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

        public static APIBoletoVM ToReturnProcessoBol(IAPIBoletoVM _bol)
        {
            return new APIBoletoVM
            {
                CaminhoArquivoRemessa = _bol.CaminhoArquivoRemessa,
                CaminhoArquivoRetorno = _bol.CaminhoArquivoRetorno,
                CaminhoArquivoBoleto = _bol.CaminhoArquivoBoleto,
                CodigoSequencialArquivo = _bol.CodigoSequencialArquivo,
                QuantidadePosicao = _bol.QuantidadePosicao,
                MsgRetorno = _bol.MsgRetorno,
                GeraArquivoRemessa = _bol.GeraArquivoRemessa,
                GeraBoleto = _bol.GeraBoleto,
                Cedente = _bol.Cedente,
                ListaBoletos = _bol.ListaBoletos
            };
        }

        public  static APIBoletoVM ToView(IBanco _banco, string pathRemessa, string pathBoleto, List<ListaEmissaoBoletoVM> listaEmissaoBoleto, string codBanco)
        {
            return new APIBoletoVM
            {
                CaminhoArquivoRemessa = pathRemessa,
                CaminhoArquivoBoleto = pathBoleto,
                Cedente = APIBoletoCedenteVM.ToView(_banco, codBanco),
                ListaBoletos = APIListaBoletoVM.ToView(listaEmissaoBoleto)
            };
        }
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

        public static APIBoletoCedenteVM ToView(IBanco _banco, string codBanco)
        {
            return new APIBoletoCedenteVM
            {
                Codigo = _banco.Cedente.Codigo,
                CodigoDV = _banco.Cedente.CodigoDV,
                CodigoFormatado = _banco.Cedente.CodigoFormatado,
                CodigoTransmissao = _banco.Cedente.CodigoTransmissao,
                CPFCNPJ = _banco.Cedente.CPFCNPJ,
                Nome = _banco.Cedente.Nome,
                Observacoes = _banco.Cedente.Observacoes,
                MostrarCNPJnoBoleto = _banco.Cedente.MostrarCNPJnoBoleto,

                Endereco = APIEnderecoVM.ToView(_banco.Cedente.Endereco),
                contaBancaria = APIContaBancariaVM.ToView(_banco.Cedente.ContaBancaria, codBanco)
            };
        }


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

        internal static List<APIListaBoletoVM> ToView<T>(ICollection<T> collection) where T : ListaEmissaoBoletoVM
        {
            return collection.Select(x => ToView(x)).ToList();
        }

        internal static APIListaBoletoVM ToView(ListaEmissaoBoletoVM bol)
        {
            return new APIListaBoletoVM
            {
                CodigoOcorrencia = bol.CodigoOcorrencia,
                DescricaoOcorrencia = bol.DescricaoOcorrencia,
                DataEmissao = DateTime.Now,
                DataProcessamento = DateTime.Now,
                DataVencimento = Convert.ToDateTime(bol.DataVencimento),
                ValorTitulo = bol.ValorTitulo,
                NossoNumero = bol.NossoNumero,
                NumeroDocumento = bol.NumeroDocumento,
                EspecieDocumento = TipoEspecieDocumento.DM,
                Aceite = "N",
                CodigoInstrucao1 = bol.CodigoInstrucao1,
                ComplementoInstrucao1 = bol.ComplementoInstrucao1.Trim(),
                CodigoInstrucao2 = bol.CodigoInstrucao2,
                ComplementoInstrucao2 = bol.ComplementoInstrucao2.Trim(),
                DataMulta = Convert.ToDateTime(bol.DataMulta),
                PercentualMulta = Convert.ToDecimal(bol.PercentualMulta),
                ValorMulta = Convert.ToDecimal(bol.ValorMulta),
                DataJuros = Convert.ToDateTime(bol.DataJuros),
                PercentualJurosDia = Convert.ToDecimal(bol.PercentualJurosDia),
                ValorJurosDia = Convert.ToDecimal(bol.ValorJurosDia),
                MensagemArquivoRemessa = bol.MensagemArquivoRemessa,
                NumeroControleParticipante = bol.NumeroControleParticipante,
                nmBoleto = bol.nmBoleto,

                Sacado = APIBoletoSacadoVM.ToView(bol),
                Avalista = APIBoletoSacadoVM.ToView(bol)
            };
        }


    }
    public class APIContaBancariaVM
    {
        public string CodigoBanco { get; set; }
        public string Conta { get; set; }
        public string DigitoAgencia { get; set; }
        public string Agencia { get; set; }
        public string DigitoConta { get; set; }
        public string CarteiraPadrao { get; set; }
        public string OperacaoConta { get; set; }
        public string VariacaoCarteiraPadrao { get; set; }
        public TipoCarteira TipoCarteiraPadrao { get; set; }
        public TipoFormaCadastramento TipoFormaCadastramento { get; set; }
        public TipoImpressaoBoleto TipoImpressaoBoleto { get; set; }

        internal static APIContaBancariaVM ToView(ContaBancaria _cb, string CodBanco)
        {
            return new APIContaBancariaVM
            {
                CodigoBanco = CodBanco,
                Conta = _cb.Conta,
                DigitoAgencia = _cb.DigitoAgencia,
                Agencia = _cb.Agencia,
                OperacaoConta = _cb.OperacaoConta,
                DigitoConta = _cb.DigitoConta,
                CarteiraPadrao = _cb.CarteiraPadrao,
                TipoCarteiraPadrao = _cb.TipoCarteiraPadrao,
                TipoFormaCadastramento = _cb.TipoFormaCadastramento,
                TipoImpressaoBoleto = _cb.TipoImpressaoBoleto,
                VariacaoCarteiraPadrao = _cb.VariacaoCarteiraPadrao
            };
        }

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
        internal static APIEnderecoVM ToViewSacado(APIEnderecoVM endereco)
        {
            return new APIEnderecoVM
            {
                LogradouroEndereco = endereco.LogradouroEndereco,
                LogradouroNumero = endereco.LogradouroNumero,
                LogradouroComplemento = endereco.LogradouroComplemento,
                Bairro = endereco.Bairro,
                Cidade = endereco.Cidade,
                UF = endereco.UF,
                CEP = endereco.CEP,
            };
        }
        internal static APIEnderecoVM ToViewEmissaoBoleto(PAGNET_CADCLIENTE cli)
        {
            return new APIEnderecoVM
            {
                LogradouroEndereco = cli.LOGRADOURO.Replace(".", "").Replace(",", ""),
                LogradouroNumero = cli.NROLOGRADOURO ?? "",
                LogradouroComplemento = (cli.COMPLEMENTO != null) ? cli.COMPLEMENTO.Replace(".", "").Replace(",", "") : "",
                Bairro = cli.BAIRRO,
                Cidade = cli.CIDADE,
                UF = cli.UF,
                CEP = cli.CEP,
            };
        }

        internal static APIEnderecoVM ToView(Endereco _endereco)
        {
            return new APIEnderecoVM
            {
                LogradouroEndereco = _endereco.LogradouroEndereco,
                LogradouroNumero = _endereco.LogradouroNumero,
                LogradouroComplemento = _endereco.LogradouroComplemento,
                Bairro = _endereco.Bairro,
                Cidade = _endereco.Cidade,
                UF = _endereco.UF,
                CEP = _endereco.CEP,
            };
        }

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
    public class APIBoletoSacadoVM : IAPIBoletoSacadoVM
    {
        public APIBoletoSacadoVM()
        {
            Endereco = new APIEnderecoVM();
        }
        public string Nome { get; set; }
        public string Observacoes { get; set; }
        public string CPFCNPJ { get; set; }
        public APIEnderecoVM Endereco { get; set; }

        public static APIBoletoSacadoVM ToView(ListaEmissaoBoletoVM bol)
        {
            return new APIBoletoSacadoVM
            {
                Nome = bol.Sacado.Nome,
                Observacoes = "",
                CPFCNPJ = Geral.RemoveCaracteres(bol.Sacado.CPFCNPJ),

                Endereco = bol.Sacado.Endereco
            };
        }

        public static APIBoletoSacadoVM ToViewEmissaoBoleto(PAGNET_EMISSAOBOLETO bol)
        {
            return new APIBoletoSacadoVM
            {
                Nome = bol.PAGNET_CADCLIENTE.NMCLIENTE,
                Observacoes = "",
                CPFCNPJ = Geral.RemoveCaracteres(bol.PAGNET_CADCLIENTE.CPFCNPJ),

                Endereco = APIEnderecoVM.ToViewEmissaoBoleto(bol.PAGNET_CADCLIENTE)
            };
        }

    }
    public class ListaEmissaoBoletoVM
    {
        public ListaEmissaoBoletoVM()
        {
            Sacado = new APIBoletoSacadoVM();
        }
        public APIBoletoSacadoVM Sacado { get; set; }


        public string CodigoOcorrencia { get; set; }
        public string DescricaoOcorrencia { get; set; }

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
        public string NossoNumero { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
        public string NumeroDocumento { get; set; }
        public string NumeroControleParticipante { get; set; }
        public decimal ValorTitulo { get; set; }
        public decimal ValorJurosDia { get; set; }
        public decimal ValorMulta { get; set; }
        public DateTime DataProcessamento { get; set; }
               
    }

    public class FiltroCobranca : IFiltroCobranca
    {
        public string caminhoArquivo { get; set; }
        public int codigoFatura { get; set; }
        public int codigoEmissaoBoleto { get; set; }
    }

    public class ResultadoTransmissaoArquivo
    {
        public bool Resultado { get; set; }
        public string FormaTransmissao { get; set; }
        public string msgResultado { get; set; }
        public string nomeArquivo { get; set; }
        public string CaminhoCompletoArquivo { get; set; }
    }

}
