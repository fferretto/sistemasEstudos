using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Api.Service.Interface.Model
{
    public interface IAPITransmissaoArquivoModal
    {
        int codigoContaCorrente { get; set; }
        int codigoEmpresa { get; set; }
        List<APIBorderosPGTOVM> ListaBorderosPGTO { get; set; }
    }
    public class APIBorderosPGTOVM
    {
        public int codigoBordero { get; set; }
    }
    public interface IAPIRetornoArquivoBancoVM
    {
        string SeuNumero { get; set; }
        string codigoRetorno { get; set; }
        string mensagemRetorno { get; set; }
        string Resumo { get; set; }
        string vlTotalArquivo { get; set; }
        int qtRegistroArquivo { get; set; }
        int qtRegistroOK { get; set; }
        int qtRegistroFalha { get; set; }
        string vlTotal { get; set; }
        int qtRegistros { get; set; }
        string RAZSOC { get; set; }
        string Status { get; set; }
        string CNPJ { get; set; }
        string dataPGTO { get; set; }
        string ValorLiquido { get; set; }
        string MsgRetBanco { get; set; }

    }
}