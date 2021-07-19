using PagNet.Bld.PGTO.Itau.Abstraction.Interface.Model;
using System.Collections.Generic;

namespace PagNet.Bld.PGTO.Itau.Abstraction.Model
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
        public mdCedente cedente { get; set; }
        public int codigoArquivo { get; set; }
        public string CaminhoArquivo { get; set; }
    }
    public class ResultadoTransmissaoArquivo
    {
        public bool Resultado { get; set; }
        public string FormaTransmissao { get; set; }
        public string msgResultado { get; set; }
        public string nomeArquivo { get; set; }
        public string CaminhoCompletoArquivo { get; set; }
    }
    public class RetornoArquivoBancoVM : IRetornoArquivoBancoVM
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
    public class mdCedente
    {

        public string codContaCorrente { get; set; }
        public int CodBanco { get; set; }
        public string CpfCNPJ { get; set; }
        public string ContaCorrente { get; set; }
        public string DigitoContaCorrente { get; set; }
        public string ContaMoviemnto { get; set; }
        public string Agencia { get; set; }
        public string DigitoAgencia { get; set; }
        public string CodConvenioBol { get; set; }
        public string CodConvenioPag { get; set; }
        public string CodTransmissao240 { get; set; }
        public string CodTransmissao400 { get; set; }
        public string ParametroTransmissao { get; set; }
        public string CarteiraRemessa { get; set; }
        public string CarteiraBol { get; set; }
        public string nmOperadora { get; set; }

        public string msg1 { get; set; }
        public string msg2 { get; set; }
        public string msg3 { get; set; }
        public string msg4 { get; set; }
        public string msg5 { get; set; }

        public string LogradouroEndereco { get; set; }
        public int LogradouroNumero { get; set; }
        public string LogradouroComplemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }

        public int NumSeq { get; set; }
        public int codArquivo { get; set; }
        public int codMovimento { get; set; }
    }

}
