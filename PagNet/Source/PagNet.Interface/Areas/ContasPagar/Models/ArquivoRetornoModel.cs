using PagNet.Api.Service.Interface.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PagNet.Interface.Areas.ContasPagar.Models
{
    public class RetornoArquivoBancoVM : IAPIRetornoArquivoBancoVM
    {
        public string SeuNumero { get; set; }
        public string codigoRetorno { get; set; }
        public string mensagemRetorno { get; set; }
        public string Resumo { get; set; }

        [Display(Name = "Valor Total do Arquivo")]
        public string vlTotalArquivo { get; set; }

        [Display(Name = "Quantidade de Registros no Arquivo")]
        public int qtRegistroArquivo { get; set; }

        [Display(Name = "Total liquidados")]
        public int qtRegistroOK { get; set; }

        [Display(Name = "Total Recusados")]
        public int qtRegistroFalha { get; set; }

        [Display(Name = "Valor Total")]
        public string vlTotal { get; set; }

        [Display(Name = "Quantidade Pagamentos")]
        public int qtRegistros { get; set; }

        [Display(Name = "Nome do Favorecido")]
        public string RAZSOC { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "CNPJ")]
        public string CNPJ { get; set; }

        [Display(Name = "Data PGTO")]
        public string dataPGTO { get; set; }

        [Display(Name = "Valor Pago")]
        public string ValorLiquido { get; set; }
        
        [Display(Name = "Mensagem de Retorno")]
        public string MsgRetBanco { get; set; }

        internal static IList<RetornoArquivoBancoVM> ToListView<T>(ICollection<T> collection) where T : IAPIRetornoArquivoBancoVM
        {
            return collection.Select(x => ToListView(x)).ToList();
        }

        internal static RetornoArquivoBancoVM ToListView(IAPIRetornoArquivoBancoVM _arq)
        {
            return new RetornoArquivoBancoVM
            {
                SeuNumero = _arq.SeuNumero,
                codigoRetorno = _arq.codigoRetorno,
                mensagemRetorno = _arq.mensagemRetorno,
                Resumo = _arq.Resumo,
                vlTotalArquivo = _arq.vlTotalArquivo,
                qtRegistroArquivo = _arq.qtRegistroArquivo,
                qtRegistroOK = _arq.qtRegistroOK,
                qtRegistroFalha = _arq.qtRegistroFalha,
                vlTotal = _arq.vlTotal,
                qtRegistros = _arq.qtRegistros,
                RAZSOC = _arq.RAZSOC,
                Status = _arq.Status,
                CNPJ = _arq.CNPJ,
                dataPGTO = _arq.dataPGTO,
                ValorLiquido = _arq.ValorLiquido,
                MsgRetBanco = _arq.MsgRetBanco,
            };
        }
    }
}
