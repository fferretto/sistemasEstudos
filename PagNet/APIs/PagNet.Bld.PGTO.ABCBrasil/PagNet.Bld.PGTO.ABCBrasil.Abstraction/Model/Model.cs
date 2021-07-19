using PagNet.Bld.PGTO.ABCBrasil.Abstraction.Interface.Model;
using System.Collections.Generic;

namespace PagNet.Bld.PGTO.ABCBrasil.Abstraction.Model
{
    public class FiltroTransmissaoBancoVM : IFiltroTransmissaoBancoVM
    {
        public FiltroTransmissaoBancoVM()
        {
            ListaBorderosPGTO = new List<ListaBorderosPGTOVM>();
        }
        public int codigoContaCorrente { get; set; }
        public List<ListaBorderosPGTOVM> ListaBorderosPGTO { get; set; }
    }
    public class ResultadoTransmissaoArquivo
    {
        public bool Resultado { get; set; }
        public string msgResultado { get; set; }
        public string nomeArquivo { get; set; }
    }
    public class RetornoArquivoBancoVM : IRetornoArquivoBancoVM
    {
        public string SeuNumero { get; set; }
        public string codigoRetorno { get; set; }
        public string mensagemRetorno { get; set; }
        public string Resumo { get; set; }
        public string vlTotalArquivo { get; set; }
        public int qtRegistroArquivo { get; set; }
        public int qtRegistroOK { get; set; }
        public int qtRegistroFalha { get; set; }
        public string vlTotal { get; set; }
        public int qtRegistros { get; set; }
        public string RAZSOC { get; set; }
        public string Status { get; set; }
        public string CNPJ { get; set; }
        public string dataPGTO { get; set; }
        public string ValorLiquido { get; set; }
        public string MsgRetBanco { get; set; }
    }
}
