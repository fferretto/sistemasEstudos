using PagNet.Bld.PGTO.ArquivoRemessa.Abrstraction.Interface.Model;
using System.Collections.Generic;

namespace PagNet.Bld.PGTO.ArquivoRemessa.Web.Setup.Models
{
    public class FiltroEmissaoArquivoRemessaModel : IFiltroTransmissaoBancoVM
    {
        public int codigoContaCorrente { get; set; }
        public int codigoEmpresa { get; set; }
        public string DadosContaCorrente { get; set; }
        public List<ListaBorderosPGTOVM> ListaBorderosPGTO { get; set; }
    }
    public class APIFiltroRetorno
    {
        public string caminhoArquivo { get; set; }
    }
}
