using PagNet.BLD.DescontoFolha.Abstraction.Interface.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.BLD.DescontoFolha.Abstraction.Model
{
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
    public class ListaClienteUsuarioVm
    {
        public string msgRetorno { get; set; }
        public int codigoFatura { get; set; }
        public string Matricula { get; set; }
        public string CodigoCliente { get; set; }
        public string CPF { get; set; }
        public string NomeClienteUsuario { get; set; }
        public string ValorRem { get; set; }
        public string ValorRet { get; set; }
        public decimal valorDecimal { get; set; }
        public string Acao { get; set; }
    }
    public class ConfigParamLeituraArquivoVM
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
        public UsuariosArquivoRetornoVm()
        {
            ListaUsuarios = new List<ListaUsuariosArquivoRetornoVm>();
        }
        public string msgRetorno { get; set; }
        public int codigoFatura { get; set; }
        public int CodigoCliente { get; set; }
        public int quantidadeTotalEsperado { get; set; }
        public int quantidadeTotal { get; set; }
        public int quantidadeOK { get; set; }
        public int quantidadeNaoOK { get; set; }
        public List<Interface.Model.ListaUsuariosArquivoRetornoVm> ListaUsuarios { get; set; }
    }
    public class APIRetornoDDLModel : IAPIRetornoDDLModel
    {
        public string Valor { get; set; }
        public string Descricao { get; set; }
        public string Title { get; set; }
    }
}
