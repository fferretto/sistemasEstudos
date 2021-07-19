using PagNet.Bld.PGTO.ArquivoRemessa.Abrstraction.Interface.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.PGTO.ArquivoRemessa.Abrstraction.Model
{
    public class FiltroTransmissaoBancoVM : IFiltroTransmissaoBancoVM
    {
        public FiltroTransmissaoBancoVM()
        {
            ListaBorderosPGTO = new List<ListaBorderosPGTOVM>();
        }
        public int codigoContaCorrente { get; set; }
        public List<ListaBorderosPGTOVM> ListaBorderosPGTO { get; set; }
        public int codigoEmpresa { get; set; }
    }
    public class ResultadoGeracaoArquivo
    {
        public bool Resultado { get; set; }
        public string FormaTransmissao { get; set; }
        public string msgResultado { get; set; }
        public string nomeArquivo { get; set; }
        public string CaminhoCompletoArquivo { get; set; }
    }
    public class RetornoModel
    {
        public bool Sucesso { get; set; }
        public string msgResultado { get; set; }
    }

    public class RetornoLeituraArquivoModel : IRetornoArquivoBancoVM
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
        public bool TituloRecusado { get; set; }
    }
}
