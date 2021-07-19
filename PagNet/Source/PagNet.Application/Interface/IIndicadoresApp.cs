using PagNet.Application.Models;
using System.Collections.Generic;

namespace PagNet.Application.Interface
{
    public interface IIndicadoresApp
    {
        List<chartPagamentoPorPeriodoVm> GetChartPagPrevistosPorPeriodos(IndicaroresVM model);
        List<chartRecebimentoPorPeriodoVm> GetChartRecebPrevistosPorPeriodos(IndicaroresVM model);
        List<chartReceitaDespesaAnoVm> GetChartRecDespPrevistosAno(IndicaroresVM model);
        List<ChartGenericoVm> GetChartPagRealizadoDia(IndicaroresVM model);
        List<ChartGenericoVm> GetChartPagRealizadoAno(IndicaroresVM model);
    }
}
