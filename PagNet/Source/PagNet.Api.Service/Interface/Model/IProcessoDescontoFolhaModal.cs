using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Api.Service.Interface.Model
{
    public interface IAPIDadosUsuarioVM
    {
        string cpfUsuario { get; set; }
        string nomeUsuario { get; set; }
        int codigoUsuario { get; set; }
        int codigoCliente { get; set; }
        string CEP { get; set; }
        string logradouroUsuario { get; set; }
        string numeroLogradouro { get; set; }
        string complementoLogradouro { get; set; }
        string bairroLogradouro { get; set; }
        string cidadeLogradouro { get; set; }
        string UF { get; set; }
    }

    public interface IAPIUsuariosArquivoRetornoVm
    {
        string msgRetorno { get; set; }
        int codigoFatura { get; set; }
        int CodigoCliente { get; set; }
        int quantidadeTotalEsperado { get; set; }
        int quantidadeTotal { get; set; }
        int quantidadeOK { get; set; }
        int quantidadeNaoOK { get; set; }

        List<APIListaUsuariosArquivoRetornoVm> ListaUsuarios { get; set; }
    }
    public class APIListaUsuariosArquivoRetornoVm
    {
        public string msgRetorno { get; set; }
        public string Matricula { get; set; }
        public string CPF { get; set; }
        public string NomeClienteUsuario { get; set; }
        public string ValorRem { get; set; }
        public string ValorRet { get; set; }
        public decimal valorDecimal { get; set; }
        public string Acao { get; set; }
    }
    public interface IAPIFiltroDescontoFolhaVM
    {
        string maticulaUsuario { get; set; }
        int codigoCliente { get; set; }
        int numeroLote { get; set; }
        string CPF { get; set; }
        DateTime dataVencimento { get; set; }
        bool renovarSaldo { get; set; }
        string status { get; set; }
        string CaminhoArquivo { get; set; }
        int codFatura { get; set; }
        int codigoConfigArquivo { get; set; }
    }
    public interface IAPIConfigParamLeituraArquivoVM
    {
        bool IsCPF { get; set; }
        int codigoCliente { get; set; }
        string nomeCliente { get; set; }
        string extensaoArquivoRET { get; set; }
        string linhaInicial { get; set; }
        int posicaoInicialCPF { get; set; }
        int posicaoFinalCPF { get; set; }
        int posicaoInicialMatricula { get; set; }
        int posicaoFinalMatricula { get; set; }
        int posicaoInicialValor { get; set; }
        int posicaoFinalValor { get; set; }
        int codigoArquivoDescontoFolha { get; set; }
        int codigoFormaVerificacao { get; set; }
        int codigoParamUsuario { get; set; }
        int codigoParamValor { get; set; }
    }

}
