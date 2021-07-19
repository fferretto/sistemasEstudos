using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Api.Service.Model
{
    public interface IFiltroAntecipacaoPGTOModel
    {
        int? codigoEmpresa { get; set; }
        int? codigoFavorecido { get; set; }
        DateTime DatRealPGTO { get; set; }
        DateTime NovaDataPGTO { get; set; }
        int codigoTitulo { get; set; }
    }
}
