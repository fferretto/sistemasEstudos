using PagNet.Api.Service.Interface.Model;
using System.Collections.Generic;

namespace PagNet.Api.Service.Model
{
    public class APITransmissaoArquivoModal : IAPITransmissaoArquivoModal
    {
        public int codigoContaCorrente { get; set; }
        public int codigoEmpresa { get; set; }
        public List<APIBorderosPGTOVM> ListaBorderosPGTO { get; set; }
    }
    public class ResultadoTransmissaoArquivo
    {
        public bool Resultado { get; set; }
        public string msgResultado { get; set; }
        public string FormaTransmissao { get; set; }
        public string nomeArquivo { get; set; }
        public string CaminhoCompletoArquivo { get; set; }
    }
    public class APIRetornoArquivoBancoVM : IAPIRetornoArquivoBancoVM
    {
        public string SeuNumero { get; set; }
        public string codigoRetorno { get; set; }
        public string mensagemRetorno { get; set; }
        public string Resumo { get; set; }
        public string vlTotalArquivo { get; set; }
        public int qtRegistroArquivo { get; set; }
        public int qtRegistroOK { get; set; }
        public int qtRegistroFalha { get; set; }
        public string vlTotal { get; set; }
        public int qtRegistros { get; set; }
        public string RAZSOC { get; set; }
        public string Status { get; set; }
        public string CNPJ { get; set; }
        public string dataPGTO { get; set; }
        public string ValorLiquido { get; set; }
        public string MsgRetBanco { get; set; }
    }
    public class APIFiltroRetorno
    {
        public string caminhoArquivo { get; set; }
    }
}
