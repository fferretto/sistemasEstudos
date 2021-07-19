using PagNet.Api.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Api.Service.Interface.Model
{
    public interface IAPIBorderoBolVM
    {
        List<APIListaBorderoBolVM> ListaBordero { get; set; }

        string CaminhoArquivo { get; set; }
        int codigoEmpresa { get; set; }
        int codContaCorrente { get; set; }

    }
    public interface IAPIFiltroCobranca
    {
        string caminhoArquivo { get; set; }
        int codigoFatura { get; set; }
        int codigoEmissaoBoleto { get; set; }
    }
    public interface IAPISolicitacaoBoletoVM
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
}
