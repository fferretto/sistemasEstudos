using PagNet.Bld.PGTO.CEF.Abstraction.Interface.Model;
using PagNet.Bld.PGTO.CEF.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.PGTO.CEF.Web.Setup.Models
{
    public class FiltroTransmissaoBancoVM : IFiltroTransmissaoBancoVM
    {
        public int codigoContaCorrente { get; set; }
        public int codigoEmpresa { get; set; }
        public string DadosContaCorrente { get; set; }
        public List<ListaBorderosPGTOVM> ListaBorderosPGTO { get; set; }
        public mdCedente cedente { get; set; }
        public int codigoArquivo { get; set; }
        public string CaminhoArquivo { get; set; }
    }
    public class APIFiltroRetorno
    {
        public string caminhoArquivo { get; set; }
    }
}
