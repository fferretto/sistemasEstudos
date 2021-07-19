using PagNet.Bld.Cobranca.Bordero.Abstraction.Model;
using System.Collections.Generic;

namespace PagNet.Bld.Cobranca.Bordero.Abstraction.Interface.Model
{
    public interface IDadosBoletoModel
    {
        IList<SolicitacaoBoletoModel> ListaBoletos { get; set; }

        int codigoEmpresa { get; set; }
        int codigoContaCorrente { get; set; }
        int qtFaturasSelecionados { get; set; }
        decimal ValorBordero { get; set; }
    }
    public interface IFiltroBorderoModel
    {
        int codigoContaCorrente { get; set; }
        int codigoCliente { get; set; }
        int codigoBordero { get; set; }

        int codigoEmpresa { get; set; }
        string DataInicial { get; set; }
        string DataFinal { get; set; }
        string status { get; set; }
    }
}
