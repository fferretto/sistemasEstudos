using System;
using System.Collections.Generic;
using System.Text;
using PagNet.Bld.Cobranca.Bordero.Abstraction.Interface.Model;
using PagNet.Bld.Cobranca.Bordero.Abstraction.Model;

namespace PagNet.Bld.Cobranca.Bordero.Web.Setup.Models
{
    public class FiltroBorderoModel : IFiltroBorderoModel
    {
        public int codigoContaCorrente { get; set; }
        public int codigoCliente { get; set; }
        public int codigoBordero { get; set; }
        public int codigoEmpresa { get; set; }
        public string DataInicial { get; set; }
        public string DataFinal { get; set; }
        public string status { get; set; }
    }
    public class DadosBoletoModel : IDadosBoletoModel
    {
        public IList<SolicitacaoBoletoModel> ListaBoletos { get; set; }
        public int codigoEmpresa { get; set; }
        public int codigoContaCorrente { get; set; }
        public int qtFaturasSelecionados { get; set; }
        public decimal ValorBordero { get; set; }
    }
}
