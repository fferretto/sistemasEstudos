using PagNet.Api.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Api.Service.Interface.Model
{
    public interface IAPIDadosBoletoModel
    {
        IList<APIFaturasBorderoModel> ListaBoletos { get; set; }

        int codigoEmpresa { get; set; }
        int codigoContaCorrente { get; set; }
        int qtFaturasSelecionados { get; set; }
        decimal ValorBordero { get; set; }
    }
    public interface IAPIFiltroBorderoModel
    {
        string codigoContaCorrente { get; set; }
        int codigoCliente { get; set; }
        int codigoBordero { get; set; }

        string codigoEmpresa { get; set; }
        string DataInicial { get; set; }
        string DataFinal { get; set; }
        string status { get; set; }
    }
}
