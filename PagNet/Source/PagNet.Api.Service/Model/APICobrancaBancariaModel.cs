using PagNet.Api.Service.Interface.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Api.Service.Model
{
    public class APIListaBorderoBolVM
    {
        public int codigoBordero { get; set; }

    }

    public class APIBorderoBolVM : IAPIBorderoBolVM
    {
        public APIBorderoBolVM()
        {
            ListaBordero = new List<APIListaBorderoBolVM>();
        }
        public List<APIListaBorderoBolVM> ListaBordero { get; set; }

        public string CaminhoArquivo { get; set; }
        public int codigoEmpresa { get; set; }
        public int codContaCorrente { get; set; }
    }
    public class APISolicitacaoBoletoVM : IAPISolicitacaoBoletoVM
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
    public class APIFiltroCobranca : IAPIFiltroCobranca
    {
        public string caminhoArquivo { get; set; }
        public int codigoFatura { get; set; }
        public int codigoEmissaoBoleto { get; set; }
    }

}
