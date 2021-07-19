using System.Collections.Generic;

namespace PagNet.BLD.ContaCorrente.ModelAuxiliar
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
    public class APIBorderosPGTOVM
    {
        public int codigoBordero { get; set; }
    }
}
