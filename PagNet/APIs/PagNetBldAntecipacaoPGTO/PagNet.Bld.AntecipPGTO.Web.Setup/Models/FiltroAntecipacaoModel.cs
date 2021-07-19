using PagNet.Bld.AntecipPGTO.Abstraction.Interface.Model;
using System;

namespace PagNet.Bld.AntecipPGTO.Web.Setup.Models
{
    public class FiltroAntecipacaoModel : IFiltroAntecipacaoModel
    {
        public int? codigoEmpresa { get; set; }
        public int? codigoFavorecido { get; set; }
        public DateTime DatRealPGTO { get; set; }
        public DateTime NovaDataPGTO { get; set; }
        public int codigoTitulo { get; set; }
    }
}
