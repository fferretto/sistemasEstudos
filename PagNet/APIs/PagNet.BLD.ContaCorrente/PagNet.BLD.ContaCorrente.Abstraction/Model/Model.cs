using PagNet.BLD.ContaCorrente.Abstraction.Interface.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.BLD.ContaCorrente.Abstraction.Model
{
    public class DadosHomologarContaCorrenteVm : IDadosHomologarContaCorrenteVm
    {
        public int codigoEmpresa { get; set; }
        public int codOPE { get; set; }
        public int CodigoContaCorrente { get; set; }
        public int codigoUsuario { get; set; }
        public int codigoFavorecido { get; set; }
        public int codigoCliente { get; set; }
        public string TipoArquivo { get; set; }
        public string CaminhoArquivo { get; set; }
        public bool ExisteArqRemessaBol { get; set; }
        public string filtroFavorecido { get; set; }
        public string nomeFavorecido { get; set; }
        public string filtroCliente { get; set; }
        public string nomeCliente { get; set; }
    }
    public class ContaCorrenteVm : IContaCorrenteVm
    {
        public bool bBoleto { get; set; }
        public bool bPagamento { get; set; }
        public int codContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }
        public string nmEmpresa { get; set; }
        public string CpfCnpj { get; set; }
        public string codEmpresa { get; set; }
        public string nmEmpresaPagNet { get; set; }
        public string CodBanco { get; set; }
        public string nmBanco { get; set; }
        public string codOperacaoCC { get; set; }
        public string nroContaCorrente { get; set; }
        public string DigitoCC { get; set; }
        public string Agencia { get; set; }
        public string DigitoAgencia { get; set; }
        public string VariacaoCarteira { get; set; }
        public string CodTransmissao { get; set; }
        public string CarteiraRemessa { get; set; }
        public string CodConvenioPag { get; set; }
        public string ParametroTransmissaoPag { get; set; }
        public string valTED { get; set; }
        public string ValMinPGTO { get; set; }
        public string ValMinTED { get; set; }
        public string SaldoConta { get; set; }
        public string formaTransmissaoPG { get; set; }
        public int codigoTransmissaoArquivoPG { get; set; }
        public string tipoArquivoPG { get; set; }
        public string loginTransmissaoPG { get; set; }
        public string senhaTransmissaoPG { get; set; }
        public string caminhoRemessaPG { get; set; }
        public string caminhoRetornoPG { get; set; }
        public string caminhoAuxiliarPG { get; set; }
        public string formaTransmissaoBol { get; set; }
        public int codigoTransmissaoArquivoBol { get; set; }
        public string tipoArquivoBol { get; set; }
        public string loginTransmissaoBol { get; set; }
        public string senhaTransmissaoBol { get; set; }
        public string caminhoRemessaBol { get; set; }
        public string caminhoRetornoBol { get; set; }
        public string caminhoAuxiliarBol { get; set; }

        public bool teraJuros { get; set; }
        public string ValorJuros { get; set; }
        public string PercJuros { get; set; }
        public bool teraMulta { get; set; }
        public string valorMulta { get; set; }
        public string PercMulta { get; set; }
        public string codigoPrimeiraInscricaoCobraca { get; set; }
        public string NomePrimeiraInscricaoCobraca { get; set; }
        public string codigoSegundaInscricaoCobraca { get; set; }
        public string NomeSegundaInscricaoCobraca { get; set; }
        public string TaxaEmissaoBoleto { get; set; }
        public bool AgruparFaturamentosDia { get; set; }
        public string qtPosicaoArqPGTO { get; set; }
        public string qtPosicaoArqBoleto { get; set; }
        public string codigoCedente { get; set; }
        public string digitoCodigoCedente { get; set; }

    }
    public class BancoVM
    {
        public string CODBANCO { get; set; }
        public string NMBANCO { get; set; }
    }
    public class RetornoDDLVM
    {
        public string Valor { get; set; }
        public string Descricao { get; set; }
        public string Title { get; set; }
    }
    public class ConsultaContaCorrenteVM
    {
        public int codContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }
        public string nroContaCorrente { get; set; }
        public string Agencia { get; set; }
    }
    public class APIBorderoBolVM : IAPIBorderoBolVM
    {
        public APIBorderoBolVM()
        {
            ListaBordero = new List<APIListaBorderoBolVM>();
        }
        public List<APIListaBorderoBolVM> ListaBordero { get; set; }
        public string CaminhoArquivo { get; set; }
        public int codigoEmpresa { get; set; }
        public int codContaCorrente { get; set; }
    }
    public class APIListaBorderoBolVM
    {
        public int codigoBordero { get; set; }

    }

    public class APIFiltroEmissaoBoletoPDF
    {
        public string caminhoArquivo { get; set; }
        public int codigoFatura { get; set; }
        public int codigoEmissaoBoleto { get; set; }
    }

    public class ResultadoTransmissaoArquivo
    {
        public bool Resultado { get; set; }
        public string FormaTransmissao { get; set; }
        public string msgResultado { get; set; }
        public string nomeArquivo { get; set; }
        public string CaminhoCompletoArquivo { get; set; }
    }
}
