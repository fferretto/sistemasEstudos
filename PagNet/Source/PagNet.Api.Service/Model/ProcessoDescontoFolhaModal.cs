using PagNet.Api.Service.Interface.Model;
using System;
using System.Collections.Generic;

namespace PagNet.Api.Service.Model
{
    public class DadosUsuarioVM : IAPIDadosUsuarioVM
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
    public class APIDFUsuarioNaoDescontadoVM : IAPIUsuariosArquivoRetornoVm
    {
        public APIDFUsuarioNaoDescontadoVM()
        {
            ListaUsuarios = new List<APIListaUsuariosArquivoRetornoVm>();
        }
        public string msgRetorno { get; set; }
        public int codigoFatura { get; set; }
        public int CodigoCliente { get; set; }
        public int quantidadeTotalEsperado { get; set; }
        public int quantidadeTotal { get; set; }
        public int quantidadeOK { get; set; }
        public int quantidadeNaoOK { get; set; }
        public List<APIListaUsuariosArquivoRetornoVm> ListaUsuarios { get; set; }
    }
    public class APIFiltroDescontoFolhaVM : IAPIFiltroDescontoFolhaVM
    {
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
    }
    public class APIConfigParamLeituraArquivoVM : IAPIConfigParamLeituraArquivoVM
    {
        public bool IsCPF { get; set; }
        public int codigoCliente { get; set; }
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
        public string nomeCliente { get; set; }
    }

}
