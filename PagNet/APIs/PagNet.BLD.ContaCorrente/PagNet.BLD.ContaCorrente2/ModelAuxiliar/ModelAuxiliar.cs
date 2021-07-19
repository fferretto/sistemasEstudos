using System.Collections.Generic;

namespace PagNet.BLD.ContaCorrente2.ModelAuxiliar
{
    public class FiltroTransmissaoBancoVM
    {
        public FiltroTransmissaoBancoVM()
        {
            ListaBorderosPGTO = new List<APIBorderosPGTOVM>();
        }
        public int codigoContaCorrente { get; set; }
        public int codigoEmpresa { get; set; }
        public List<APIBorderosPGTOVM> ListaBorderosPGTO { get; set; }
    }
    public class FiltroTransmissaoBoletoModel
    {
        public FiltroTransmissaoBoletoModel()
        {
            ListaBordero = new List<APIBorderosPGTOVM>();
        }
        public int codContaCorrente { get; set; }
        public string CaminhoArquivo { get; set; }
        public int codigoEmpresa { get; set; }
        public List<APIBorderosPGTOVM> ListaBordero { get; set; }
    }
    public class APIBorderosPGTOVM
    {
        public int codigoBordero { get; set; }
    }
    public class ResultadoGeracaoArquivo
    {
        public bool Resultado { get; set; }
        public string FormaTransmissao { get; set; }
        public string msgResultado { get; set; }
        public string nomeArquivo { get; set; }
        public string CaminhoCompletoArquivo { get; set; }
    }
}
