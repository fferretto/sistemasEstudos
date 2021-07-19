using PagNet.Application.Interface;
using PagNet.Application.Models;
using PagNet.Domain.Interface.Services.Procedures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PagNet.Application.Application
{
    public class IndicadoresApp : IIndicadoresApp
    {
        private readonly IProceduresService _proc;
        public IndicadoresApp(IProceduresService proc)
        {
            _proc = proc;
        }

        public List<chartPagamentoPorPeriodoVm> GetChartPagPrevistosPorPeriodos(IndicaroresVM model)
        {
            try
            {
                List<chartPagamentoPorPeriodoVm> dados = new List<chartPagamentoPorPeriodoVm>();

                //DateTime dtInicio = Convert.ToDateTime(model.dtInicio);
                //DateTime dtFim = Convert.ToDateTime(model.dtFim);
                //int codSubRede = Convert.ToInt32(model.codEmpresa);

                //dados = _fechCred.PagamentoPorPeriodo(dtInicio, dtFim, codSubRede)
                //    .Select(x => new chartPagamentoPorPeriodoVm(x)).ToList();

                return dados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<chartRecebimentoPorPeriodoVm> GetChartRecebPrevistosPorPeriodos(IndicaroresVM model)
        {
            try
            {
                List<chartRecebimentoPorPeriodoVm> dados;

                DateTime dtInicio = Convert.ToDateTime(model.dtInicio);
                DateTime dtFim = Convert.ToDateTime(model.dtFim);
                int codSubRede = Convert.ToInt32(model.codEmpresa);

                dados = _proc.ConsultaIndicadorReceitaDia(dtInicio, dtFim, codSubRede)
                    .Select(x => new chartRecebimentoPorPeriodoVm(x)).ToList();

                return dados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<chartReceitaDespesaAnoVm> GetChartRecDespPrevistosAno(IndicaroresVM model)
        {
            try
            {
                List<chartReceitaDespesaAnoVm> dados;

                int codSubRede = Convert.ToInt32(model.codEmpresa);

                dados = _proc.ConsultaIndicadorEntradaSaidaAno(codSubRede)
                    .Select(x => new chartReceitaDespesaAnoVm(x)).ToList();

                return dados;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Método utilizado para retornar as informações para o gráfico de pagamentos realizados no período.
        /// </summary>
        /// <proc name="PROC_PAGNET_IND_PAG_REALIZADO_DIA"></proc>
        /// <returns></returns>
        public List<ChartGenericoVm> GetChartPagRealizadoDia(IndicaroresVM model)
        {
            try
            {

                DateTime dtInicio = Convert.ToDateTime(model.dtInicio);
                DateTime dtFim = Convert.ToDateTime(model.dtFim);
                int codSubRede = Convert.ToInt32(model.codEmpresa);

                if (!string.IsNullOrWhiteSpace(model.codBanco))
                {
                    while (model.codBanco.Length < 4)
                    {
                        model.codBanco = "0" + model.codBanco;
                    }
                }
                else
                {
                    model.codBanco = model.codBanco ?? "";
                }

                var dados = _proc.ConsultaPagRealizadoDia(dtInicio, dtFim, codSubRede, model.codBanco, model.codCredenciado);
                var ListaRetorno = ChartGenericoVm.ToViewInd_Pag_Realizado_Dia(dados).ToList();
                    

                return ListaRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Método utilizado para retornar as informações para o gráfico de pagamentos realizados nos ultimos 12 meses.
        /// </summary>
        /// <proc name="PROC_PAGNET_IND_PAG_ANO"></proc>
        /// <returns></returns>
        public List<ChartGenericoVm> GetChartPagRealizadoAno(IndicaroresVM model)
        {
            try
            {

                int codSubRede = Convert.ToInt32(model.codEmpresa);

                if (!string.IsNullOrWhiteSpace(model.codBanco))
                {
                    while (model.codBanco.Length < 4)
                    {
                        model.codBanco = "0" + model.codBanco;
                    }
                }
                else
                {
                    model.codBanco = model.codBanco ?? "";
                }

                var dados = _proc.ConsultaPagRealizadoAno(codSubRede, model.codBanco, model.codCredenciado);
                var ListaRetorno = ChartGenericoVm.ToViewInd_Pag_Ano(dados).ToList();
                
                return ListaRetorno;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

