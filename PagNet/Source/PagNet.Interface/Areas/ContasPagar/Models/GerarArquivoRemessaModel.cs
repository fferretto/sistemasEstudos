using PagNet.Api.Service.Interface.Model;
using System.Collections.Generic;

namespace PagNet.Interface.Areas.ContasPagar.Models
{
    public class TransmissaoArquivoModal : IAPITransmissaoArquivoModal
    {
        public TransmissaoArquivoModal()
        {
            ListaBorderosPGTO = new List<APIBorderosPGTOVM>();
        }
        public int codigoContaCorrente { get; set; }
        public string DadosContaCorrente { get; set; }
        public int codigoEmpresa { get; set; }
        public List<APIBorderosPGTOVM> ListaBorderosPGTO { get; set; }
    }
}
