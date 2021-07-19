using PagNet.Application.Helpers;
using PagNet.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PagNet.Application.Models
{
    public class IndicaroresVM
    {
        public bool acessoAdmin { get; set; }
        public bool cartaoPos { get; set; }
        public bool cartaoPre { get; set; }
        public int codCredenciado { get; set; }
        public string codBanco { get; set; }

        [Display(Name = "Empresa")]
        public string codEmpresa { get; set; }
        public string nmEmpresa { get; set; }

        [Display(Name = "Código do Banco")]
        [Ajuda("Informe o Código do banco dos credenciados que deseja realizar o pagamento")]
        [Required(ErrorMessage = "Obrigatório informar o código do banco")]
        public string filtroCodBanco { get; set; }

        [Display(Name = "Nome do Banco")]
        public string FiltroNmBanco { get; set; }

        [Display(Name = "Código")]
        [Ajuda("Informe o CPF/CNPJ ou o código do credenciado")]
        public string filtroCredenciado { get; set; }

        [Display(Name = "Credenciado")]
        public string nmCredenciado { get; set; }

        [Display(Name = "Data Inicio")]
        [Ajuda("Filtro de início da data para consulta")]
        [InputMask("99/99/9999")]
        public string dtInicio { get; set; }

        [Display(Name = "Data Fim")]
        [Ajuda("Filtro de término da data para consulta")]
        [InputMask("99/99/9999")]
        public string dtFim { get; set; }
    }
    public class DadosGraficosVM
    {
        public Dictionary<string, decimal> DadosRegsMensais { get; set; }
        public Dictionary<string, decimal> PagamentosPeriodos { get; set; }
    }
    public class chartPagamentoPorPeriodoVm
    {
        public chartPagamentoPorPeriodoVm(PROC_PAGNET_IND_PAG_PREVISTO_DIA x)
        {
            dtPagameno = x.DATPGTO.ToShortDateString();
            vlPago = x.VLPAGAR;
            vlTaxa = x.VALTAXA;
        }
        public string dtPagameno { get; set; }
        public decimal vlPago { get; set; }
        public decimal vlTaxa { get; set; }
    }
    public class chartRecebimentoPorPeriodoVm
    {
        public chartRecebimentoPorPeriodoVm(PROC_PAGNET_INDI_RECEB_PREVISTA_DIA x)
        {
            dtReceita = x.DATPGTO.ToShortDateString();
            vlRecebido = x.VLRECEBER;
        }
        public string dtReceita { get; set; }
        public decimal vlRecebido { get; set; }
    }
    public class chartReceitaDespesaAnoVm
    {
        public chartReceitaDespesaAnoVm(PROC_PAGNET_INDICADOR_ENTRADA_SAIDA_ANO x)
        {
            MESREF = x.MESREF;
            RECEITA = x.RECEITA;
            DESPESA = x.DESPESA;
        }
        public string MESREF { get; set; }
        public decimal RECEITA { get; set; }
        public decimal DESPESA { get; set; }
    }

    public class ChartGenericoVm
    {

        public string MesRef { get; set; }
        public string dtReferencia { get; set; }
        public decimal vlReceitas { get; set; }
        public decimal vlDespesas { get; set; }
        public decimal vlTaxas { get; set; }

        /// <summary>
        /// Carrega as informações relacionadas a proc PROC_PAGNET_IND_PAG_REALIZADO_DIA para o gráfico de pagamentos realizados no período
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        internal static IList<ChartGenericoVm> ToViewInd_Pag_Realizado_Dia<T>(ICollection<T> collection) where T : PROC_PAGNET_IND_PAG_REALIZADO_DIA
        {
            return collection.Select(x => ToViewInd_Pag_Realizado_Dia(x)).ToList();
        }

        internal static ChartGenericoVm ToViewInd_Pag_Realizado_Dia(PROC_PAGNET_IND_PAG_REALIZADO_DIA x)
        {
            return new ChartGenericoVm
            {
                dtReferencia = x.DATPGTO.ToShortDateString(),
                vlDespesas = x.VLPAGAR,
                vlTaxas = x.VALTAXA

            };
        }
        /// <summary>
        /// Carrega as informações relacionadas a proc PROC_PAGNET_IND_PAG_ANO para o gráfico de pagamentos realizados no ano, separado mes a mes
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        internal static IList<ChartGenericoVm> ToViewInd_Pag_Ano<T>(ICollection<T> collection) where T : PROC_PAGNET_IND_PAG_ANO
        {
            return collection.Select(x => ToViewInd_Pag_Ano(x)).ToList();
        }

        internal static ChartGenericoVm ToViewInd_Pag_Ano(PROC_PAGNET_IND_PAG_ANO x)
        {
            return new ChartGenericoVm
            {
                MesRef = x.MESREF,
                vlDespesas = x.VLPAGO,
                vlTaxas = x.VLTAXA

            };
        }
    }

}
