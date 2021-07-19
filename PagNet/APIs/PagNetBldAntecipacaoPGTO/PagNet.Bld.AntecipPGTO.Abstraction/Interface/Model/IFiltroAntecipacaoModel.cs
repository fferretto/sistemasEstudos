using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.AntecipPGTO.Abstraction.Interface.Model
{
    public interface IFiltroAntecipacaoModel
    {
        int? codigoEmpresa { get; set; }
        int? codigoFavorecido { get; set; }
        DateTime DatRealPGTO { get; set; }
        DateTime NovaDataPGTO { get; set; }
        int codigoTitulo { get; set; }
    }
}
