
using PagNet.Bld.PGTO.CobrancaBancaria.Abstraction.Interface.Model;
using PagNet.Bld.PGTO.CobrancaBancaria.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.PGTO.CobrancaBancaria.Web.Setup.Models
{
    public class FiltroEmissaoBoletoModel : IBorderoBolVM
    {
        public List<ListaBorderoBolVM> ListaBordero { get; set; }
        public string CaminhoArquivo { get; set; }
        public int codigoEmpresa { get; set; }
        public int codContaCorrente { get; set; }
    }
    public class FiltroCobranca : IFiltroCobranca
    {
        public string caminhoArquivo { get; set; }
        public int codigoFatura { get; set; }
        public int codigoEmissaoBoleto { get; set; }
    }
}
