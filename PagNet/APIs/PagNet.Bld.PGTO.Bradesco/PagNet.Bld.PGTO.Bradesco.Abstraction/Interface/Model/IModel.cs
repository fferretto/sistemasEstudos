using PagNet.Bld.PGTO.Bradesco.Abstraction.Model;
using System.Collections.Generic;

namespace PagNet.Bld.PGTO.Bradesco.Abstraction.Interface.Model
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
        int codigoEmpresa { get; set; }
        List<ListaBorderosPGTOVM> ListaBorderosPGTO { get; set; }
        mdCedente cedente { get; set; }
        int codigoArquivo { get; set; }
        string CaminhoArquivo { get; set; }

    }
    public class ListaBorderosPGTOVM
    {
        public int codigoBordero { get; set; }
    }
}
