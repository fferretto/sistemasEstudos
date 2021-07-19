using PagNet.Bld.Cobranca.Bordero.Abstraction.Interface.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.Cobranca.Bordero.Abstraction.Model
{
    public class DadosBoletoModel : IDadosBoletoModel
    {
        public DadosBoletoModel()
        {
            ListaBoletos = new List<SolicitacaoBoletoModel>();
        }
        public IList<SolicitacaoBoletoModel> ListaBoletos { get; set; }
        public int codigoEmpresa { get; set; }
        public int codigoContaCorrente { get; set; }
        public int qtFaturasSelecionados { get; set; }
        public decimal ValorBordero { get; set; }
    }
    public class SolicitacaoBoletoModel
    {
        public int codigoFatura { get; set; }

        public int codigoCliente { get; set; }
        public string nomeCliente { get; set; }
        public string cnpj { get; set; }

        public DateTime dataVencimento { get; set; }
        public decimal Valor { get; set; }
        public int QuantidadeFatura { get; set; }
    }

    public class RetornoModel
    {
        public bool Sucesso { get; set; }
        public string msgResultado { get; set; }
    }
    public class DadosBorderoModel
    {
        public int codigoBordero { get; set; }
        public string quantidadeFatura { get; set; }
        public string Status { get; set; }
        public decimal ValorBordero { get; set; }
        public DateTime dataEmissao { get; set; }

    }

}
