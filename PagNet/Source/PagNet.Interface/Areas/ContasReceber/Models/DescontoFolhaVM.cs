using PagNet.Api.Service.Interface.Model;
using PagNet.Interface.Helpers.HelperModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PagNet.Interface.Areas.ContasReceber.Models
{
    public class ModelDescontoFolhaVM
    {
        public bool acessoAdmin { get; set; }
        public bool VisualizarFaturasNaoDescontadas { get; set; }
        public int CodigoCliente { get; set; }

        [Display(Name = "Código")]
        [Ajuda("Poderá ser utilizado o códgio ou CNPJ do cliente ")]
        public string filtroCliente { get; set; }

        [Display(Name = "Cliente")]
        public string nomeCliente { get; set; }

        [Display(Name = "Empresa")]
        public string codigoEmpresa { get; set; }
        public string nomeEmpresa { get; set; }

        [Display(Name = "Fatura")]
        public string codigoFatura { get; set; }
        public string nomeFatura { get; set; }

    }
    public class FiltroDescontoFolhaModel : IAPIFiltroDescontoFolhaVM
    {
        public bool acessoAdmin { get; set; }
        public string maticulaUsuario { get; set; }
        public int codigoCliente { get; set; }
        public int numeroLote { get; set; }
        public string CPF { get; set; }
        public DateTime dataVencimento { get; set; }
        public bool renovarSaldo { get; set; }
        public string status { get; set; }
        public string CaminhoArquivo { get; set; }
        public int codFatura { get; set; }
        public int codigoConfigArquivo { get; set; }
        public bool ValidaArquivo { get; set; }
    }
    public class RetornoValidaDescontoFolhaVM
    {
        public RetornoValidaDescontoFolhaVM()
        {
            ListaUsuarios = new List<ListaUsuariosNaoDescontadosVM>();
        }
        public bool ArquivoValido { get; set; }
        public string msgRetornoValidacao { get; set; }
        public int codigoCliente { get; set; }
        public int codigoFatura { get; set; }
        [Display(Name = "Total Usarios a ser Descontados")]
        public int quantidadeTotalEsperado { get; set; }
        [Display(Name = "Total Usuarios no Arquivo")]
        public int quantidadeTotal { get; set; }
        [Display(Name = "Total Descontados")]
        public int quantidadeOK { get; set; }
        [Display(Name = "Total NÃO Descontados")]
        public int quantidadeNaoOK { get; set; }

        public IList<ListaUsuariosNaoDescontadosVM> ListaUsuarios { get; set; }

        
        internal static RetornoValidaDescontoFolhaVM ToView(IAPIUsuariosArquivoRetornoVm usu)
        {
            return new RetornoValidaDescontoFolhaVM
            {
                ArquivoValido = string.IsNullOrWhiteSpace(usu.msgRetorno),
                msgRetornoValidacao = usu.msgRetorno,
                quantidadeTotalEsperado = usu.quantidadeTotalEsperado,
                quantidadeTotal = usu.quantidadeTotal,
                quantidadeOK = usu.quantidadeOK,
                quantidadeNaoOK = usu.quantidadeNaoOK,
                ListaUsuarios = ListaUsuariosNaoDescontadosVM.ToListDF(usu.ListaUsuarios)
            };
        }
    }
    public class ListaUsuariosNaoDescontadosVM
    {
        public string msgRetorno { get; set; }
        public string Matricula { get; set; }
        public string CPF { get; set; }
        public string NomeClienteUsuario { get; set; }
        public string ValorRem { get; set; }
        public string ValorRet { get; set; }
        public decimal valorDecimal { get; set; }
        public string Acao { get; set; }

        internal static IList<ListaUsuariosNaoDescontadosVM> ToListDF<T>(ICollection<T> collection) where T : APIListaUsuariosArquivoRetornoVm
        {
            return collection.Select(x => ToListDF(x)).ToList();
        }

        internal static ListaUsuariosNaoDescontadosVM ToListDF(APIListaUsuariosArquivoRetornoVm x)
        {
            return new ListaUsuariosNaoDescontadosVM
            {
                msgRetorno = x.msgRetorno,
                Matricula = x.Matricula,
                CPF = Geral.FormataCPFCnPj(x.CPF),
                NomeClienteUsuario = x.NomeClienteUsuario,
                ValorRem = x.ValorRem,
                ValorRet = x.ValorRet,
                valorDecimal = x.valorDecimal,
                Acao = "PROXIMAFATURA"
            };
        }
    }
}
