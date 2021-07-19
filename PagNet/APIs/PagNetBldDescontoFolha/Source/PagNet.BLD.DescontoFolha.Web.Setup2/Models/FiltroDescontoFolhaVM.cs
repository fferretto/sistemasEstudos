using PagNet.BLD.DescontoFolha.Abstraction.Interface.Model;
using System;
using System.Collections.Generic;

namespace PagNet.BLD.DescontoFolha.Web.Setup2.Models
{
    public class FiltroDescontoFolhaVM : IFiltroDescontoFolhaVM
    {
        public int codigoCliente { get; set; }
        public string maticulaUsuario { get; set; } = "";
        public int numeroLote { get; set; } 
        public string CPF { get; set; } = "";
        public DateTime dataVencimento { get; set; } = DateTime.Now;
        public bool renovarSaldo { get; set; } 
        public string status { get; set; } = "";
        public string CaminhoArquivo { get; set; } = "";
        public int codFatura { get; set; } 
        public int codigoConfigArquivo { get; set; }
        public int codigoEmpresa { get; set; }
    }
    public class DadosUsuarioVM : IDadosUsuarioVM
    {
        public string cpfUsuario { get; set; }
        public string nomeUsuario { get; set; }
        public int codigoUsuario { get; set; }
        public int codigoCliente { get; set; }
        public string CEP { get; set; }
        public string logradouroUsuario { get; set; }
        public string numeroLogradouro { get; set; }
        public string complementoLogradouro { get; set; }
        public string bairroLogradouro { get; set; }
        public string cidadeLogradouro { get; set; }
        public string UF { get; set; }
    }

    public class ConfigParamLeituraArquivoVM : IConfigParamLeituraArquivoVM
    {
        public bool IsCPF { get; set; }
        public int codigoCliente { get; set; }
        public string nomeCliente { get; set; }
        public string extensaoArquivoREM { get; set; }
        public string extensaoArquivoRET { get; set; }
        public string linhaInicial { get; set; }
        public int posicaoInicialCPF { get; set; }
        public int posicaoFinalCPF { get; set; }
        public int posicaoInicialMatricula { get; set; }
        public int posicaoFinalMatricula { get; set; }
        public int posicaoInicialValor { get; set; }
        public int posicaoFinalValor { get; set; }
        public int codigoArquivoDescontoFolha { get; set; }
        public int codigoFormaVerificacao { get; set; }
        public int codigoParamUsuario { get; set; }
        public int codigoParamValor { get; set; }
    }
    public class UsuariosArquivoRetornoVm : IUsuariosArquivoRetornoVm
    {
        public string msgRetorno { get; set; }
        public int codigoFatura { get; set; }
        public int CodigoCliente { get; set; }
        public int quantidadeTotalEsperado { get; set; }
        public int quantidadeTotal { get; set; }
        public int quantidadeOK { get; set; }
        public int quantidadeNaoOK { get; set; }
        public List<ListaUsuariosArquivoRetornoVm> ListaUsuarios { get; set; }
    }
}
