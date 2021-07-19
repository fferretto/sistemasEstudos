using PagNet.Api.Service.Interface.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Api.Service.Model
{
    public class APIDadosBoletoModel : IAPIDadosBoletoModel
    {
        public APIDadosBoletoModel()
        {
            ListaBoletos = new List<APIFaturasBorderoModel>();
        }
        public IList<APIFaturasBorderoModel> ListaBoletos { get; set; }
        public int codigoEmpresa { get; set; }
        public int codigoContaCorrente { get; set; }
        public int qtFaturasSelecionados { get; set; }
        public decimal ValorBordero { get; set; }
    }
    public class APIFaturasBorderoModel
    {
        public int codigoFatura { get; set; }

        public int codigoCliente { get; set; }
        public string nomeCliente { get; set; }
        public string cnpj { get; set; }

        public DateTime dataVencimento { get; set; }
        public decimal Valor { get; set; }
        public int QuantidadeFatura { get; set; }
    }
    public class APIDadosBorderoModel
    {
        public int codigoBordero { get; set; }
        public string quantidadeFatura { get; set; }
        public string Status { get; set; }
        public decimal ValorBordero { get; set; }
        public DateTime dataEmissao { get; set; }

    }

}
