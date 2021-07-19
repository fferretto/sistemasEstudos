using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.PGTO.ABCBrasil.Abstraction.Interface.Model
{
    public interface IRetornoArquivoBancoVM
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

    public interface IFiltroTransmissaoBancoVM
    {
        int codigoContaCorrente { get; set; }
        List<ListaBorderosPGTOVM> ListaBorderosPGTO { get; set; }

    }
    public class ListaBorderosPGTOVM
    {
        public int codigoBordero { get; set; }        
    }
}
