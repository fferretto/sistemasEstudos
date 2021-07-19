using PagNet.Api.Service.Helpers;
using PagNet.Api.Service.Interface.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Interface.Areas.ContasReceber.Models
{
    public class APISolicitacaoBoletoModel : IAPISolicitacaoBoletoVM
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

        internal static IList<APISolicitacaoBoletoModel> ToListView<T>(ICollection<T> collection) where T : IAPISolicitacaoBoletoVM
        {
            return collection.Select(x => ToListView(x)).ToList();
        }

        internal static APISolicitacaoBoletoModel ToListView(IAPISolicitacaoBoletoVM _cad)
        {
            return new APISolicitacaoBoletoModel
            {
                codEmissaoBoleto = _cad.codEmissaoBoleto,
                MsgRetorno = _cad.MsgRetorno,
                chkBoleto = _cad.chkBoleto,
                nomeBoleto = _cad.nomeBoleto,
                BoletoRecusado = _cad.BoletoRecusado,
                msgRecusa = _cad.msgRecusa,
                TAXAEMISSAOBOLETO = _cad.TAXAEMISSAOBOLETO,
                PERCMULTA = _cad.PERCMULTA,
                VLMULTADIAATRASO = _cad.VLMULTADIAATRASO,
                PERCJUROS = _cad.PERCJUROS,
                VLJUROSDIAATRASO = _cad.VLJUROSDIAATRASO,
                codBordero = _cad.codBordero,
                codigoCliente = _cad.codigoCliente,
                nomeCliente = _cad.nomeCliente,
                nomeCompletoCliente = _cad.nomeCompletoCliente,
                cnpj = _cad.cnpj,
                Email = _cad.Email,
                Status = _cad.Status,
                dtVencimento = _cad.dtVencimento,
                dtEmissao = _cad.dtEmissao,
                Valor = _cad.Valor,
                OrigemBoleto = _cad.OrigemBoleto,
                TipoFaturamento = _cad.TipoFaturamento,
                SeuNumero = _cad.SeuNumero,
                nroDocumento = _cad.nroDocumento,
                DescricaoOcorrenciaRet = _cad.DescricaoOcorrenciaRet,
                CodFaturamentoPai = _cad.CodFaturamentoPai,
                nParcela = _cad.nParcela,
                ntotalParcelas = _cad.ntotalParcelas,
                ValParcela = _cad.ValParcela
            };
        }
    }
    public class ClienteArquivoRetornoRecModel
    {
        public int CODCLIENTE { get; set; }
        public bool acessoAdmin { get; set; }
        public int codUsuario { get; set; }
        public bool COBRANCADIFERENCIADA { get; set; }
        public bool COBRAJUROS { get; set; }
        public bool COBRAMULTA { get; set; }
        public string REGRAPROPRIA { get; set; }
        public string NMCLIENTE { get; set; }
        public string EMAIL { get; set; }
        public string CPFCNPJ { get; set; }
        public string CODEMPRESA { get; set; }
        public string NMEMPRESA { get; set; }
        public string CEP { get; set; }
        public string LOGRADOURO { get; set; }
        public string NROLOGRADOURO { get; set; }
        public string COMPLEMENTO { get; set; }
        public string BAIRRO { get; set; }
        public string CIDADE { get; set; }
        public string UF { get; set; }
        public string VLMULTADIAATRASO { get; set; }
        public string PERCMULTA { get; set; }
        public string PERCJUROS { get; set; }
        public string VLJUROSDIAATRASO { get; set; }
        public string CODPRIMEIRAINSTCOBRA { get; set; }
        public string NMPRIMEIRAINSTCOBRA { get; set; }
        public string CODSEGUNDAINSTCOBRA { get; set; }
        public string NMSEGUNDAINSTCOBRA { get; set; }
        public string codigoFormaFaturamento { get; set; }
        public string nomeFormaFaturamento { get; set; }
        public string CodigoTipoPessoa { get; set; }
        public string NomeTipoPessoa { get; set; }
        public string TAXAEMISSAOBOLETO { get; set; }
        public string codJustificativa { get; set; }
        public string descJustificativa { get; set; }
        public string DescJustOutros { get; set; }
        public bool AgruparFaturamentos { get; set; }


        internal static ClienteArquivoRetornoRecModel ToView(IAPIClienteModel _cad)
        {
            return new ClienteArquivoRetornoRecModel
            {
                CODCLIENTE = _cad.codigoCliente,
                NMCLIENTE = _cad.nomeCliente,
                CPFCNPJ = Geral.FormataCPFCnPj(_cad.cpfCnpj),
                EMAIL = _cad.email,
                CODEMPRESA = _cad.codigoEmpresa,
                CEP = Geral.FormataCEP(_cad.cep),
                LOGRADOURO = _cad.logradouro,
                NROLOGRADOURO = _cad.numeroLogradouro,
                COMPLEMENTO = _cad.complemento,
                BAIRRO = _cad.bairro,
                CIDADE = _cad.cidade,
                UF = _cad.UF,
                codigoFormaFaturamento = _cad.codigoFormaFaturamento,
                AgruparFaturamentos = (_cad.AgruparFaturamentos),
                REGRAPROPRIA = _cad.regraPropria,
                COBRANCADIFERENCIADA = _cad.cobrancaDiferenciada,
                COBRAJUROS = (_cad.cobraJuros),
                VLJUROSDIAATRASO = _cad.valorJurosDiaAtraso,
                PERCJUROS = _cad.percentualJuros,
                COBRAMULTA = (_cad.cobraMulta),
                VLMULTADIAATRASO = _cad.valorMultaDiaAtraso,
                PERCMULTA = _cad.percentualMulta,
                CODPRIMEIRAINSTCOBRA = _cad.codigoPrimeiraIntrucaoCobranca,
                NMPRIMEIRAINSTCOBRA = _cad.nomePrimeiraIntrucaoCobranca,
                CODSEGUNDAINSTCOBRA = _cad.codigoSegundaIntrucaoCobranca,
                NMSEGUNDAINSTCOBRA = _cad.nomeSegundaIntrucaoCobranca,
                NMEMPRESA = _cad.nomeEmpresa,
                TAXAEMISSAOBOLETO = _cad.taxaEmissaoBoleto
            };
        }

        internal static IList<ClienteArquivoRetornoRecModel> ToListView<T>(ICollection<T> collection) where T : IAPIClienteModel
        {
            return collection.Select(x => ToListView(x)).ToList();
        }

        internal static ClienteArquivoRetornoRecModel ToListView(IAPIClienteModel _cad)
        {
            return new ClienteArquivoRetornoRecModel
            {
                CODCLIENTE = _cad.codigoCliente,
                NMCLIENTE = _cad.nomeCliente,
                CPFCNPJ = Geral.FormataCPFCnPj(_cad.cpfCnpj),
                EMAIL = _cad.email,
                CODEMPRESA = _cad.codigoEmpresa,
                CEP = Geral.FormataCEP(_cad.cep),
                LOGRADOURO = _cad.logradouro,
                NROLOGRADOURO = _cad.numeroLogradouro,
                COMPLEMENTO = _cad.complemento,
                BAIRRO = _cad.bairro,
                CIDADE = _cad.cidade,
                UF = _cad.UF,
                codigoFormaFaturamento = _cad.codigoFormaFaturamento,
                AgruparFaturamentos = (_cad.AgruparFaturamentos),
                REGRAPROPRIA = _cad.regraPropria,
                COBRANCADIFERENCIADA = _cad.cobrancaDiferenciada,
                COBRAJUROS = (_cad.cobraJuros),
                VLJUROSDIAATRASO = _cad.valorJurosDiaAtraso,
                PERCJUROS = _cad.percentualJuros,
                COBRAMULTA = (_cad.cobraMulta),
                VLMULTADIAATRASO = _cad.valorMultaDiaAtraso,
                PERCMULTA = _cad.percentualMulta,
                CODPRIMEIRAINSTCOBRA = _cad.codigoPrimeiraIntrucaoCobranca,
                NMPRIMEIRAINSTCOBRA = _cad.nomePrimeiraIntrucaoCobranca,
                CODSEGUNDAINSTCOBRA = _cad.codigoSegundaIntrucaoCobranca,
                NMSEGUNDAINSTCOBRA = _cad.nomeSegundaIntrucaoCobranca,
                NMEMPRESA = _cad.nomeEmpresa,
                TAXAEMISSAOBOLETO = _cad.taxaEmissaoBoleto
            };
        }
    }

}
