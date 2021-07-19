using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using System.Collections.Generic;

namespace PagNet.Interface.Areas.ContasReceber.Models
{
    public class FiltroTransmissaoCB : IAPIBorderoBolVM
    {
        public FiltroTransmissaoCB()
        {
            ListaBordero = new List<APIListaBorderoBolVM>();
        }
        public List<APIListaBorderoBolVM> ListaBordero { get; set; }
        public string CaminhoArquivo { get; set; }
        public int codigoEmpresa { get; set; }
        public int codContaCorrente { get; set; }
    }
}
