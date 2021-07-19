using PagNet.Bld.PGTO.CobrancaBancaria.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.PGTO.CobrancaBancaria.Abstraction.Interface.Model
{
    public interface ISolicitacaoBoletoVM
    {
        int codEmissaoBoleto { get; set; }
        string MsgRetorno { get; set; }
        bool chkBoleto { get; set; }
        string nomeBoleto { get; set; }
        string BoletoRecusado { get; set; }
        string msgRecusa { get; set; }
        string TAXAEMISSAOBOLETO { get; set; }
        string PERCMULTA { get; set; }
        string VLMULTADIAATRASO { get; set; }
        string PERCJUROS { get; set; }
        string VLJUROSDIAATRASO { get; set; }
        string codBordero { get; set; }
        string codigoCliente { get; set; }
        string nomeCliente { get; set; }
        string nomeCompletoCliente { get; set; }
        string cnpj { get; set; }
        string Email { get; set; }
        string Status { get; set; }
        string dtVencimento { get; set; }
        string dtEmissao { get; set; }
        string Valor { get; set; }
        string OrigemBoleto { get; set; }
        string TipoFaturamento { get; set; }
        string SeuNumero { get; set; }
        string nroDocumento { get; set; }
        string DescricaoOcorrenciaRet { get; set; }
        string CodFaturamentoPai { get; set; }
        string nParcela { get; set; }
        string ntotalParcelas { get; set; }
        string ValParcela { get; set; }
    }
    public interface IFiltroCobranca
    {
        string caminhoArquivo { get; set; }
        int codigoFatura { get; set; }
        int codigoEmissaoBoleto { get; set; }
    }
    public interface IBorderoBolVM
    {
        List<ListaBorderoBolVM> ListaBordero { get; set; }

        string CaminhoArquivo { get; set; }
        int codigoEmpresa { get; set; }
        int codContaCorrente { get; set; }

    }

    public interface IAPIBoletoVM
    {
        APIBoletoCedenteVM Cedente { get; set; }
        List<APIListaBoletoVM> ListaBoletos { get; set; }

        string CaminhoArquivoRemessa { get; set; }
        string CaminhoArquivoRetorno { get; set; }
        string CaminhoArquivoBoleto { get; set; }
        int CodigoSequencialArquivo { get; set; }
        int QuantidadePosicao { get; set; }

        string MsgRetorno { get; set; }


        bool GeraArquivoRemessa { get; set; }
        bool GeraBoleto { get; set; }

    }
    public interface IAPIBoletoSacadoVM
    {
        string Nome { get; set; }
        string Observacoes { get; set; }
        string CPFCNPJ { get; set; }
        APIEnderecoVM Endereco { get; set; }

    }

}
