using PagNet.BLD.ContaCorrente.Abstraction.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.BLD.ContaCorrente.Abstraction.Interface.Model
{
    public interface IDadosHomologarContaCorrenteVm
    {
        int codigoEmpresa { get; set; }
        int codOPE { get; set; }
        int CodigoContaCorrente { get; set; }
        int codigoFavorecido { get; set; }
        int codigoCliente { get; set; }
        string TipoArquivo { get; set; }
        string CaminhoArquivo { get; set; }
        bool ExisteArqRemessaBol { get; set; }
        string filtroFavorecido { get; set; }
        string nomeFavorecido { get; set; }
        string filtroCliente { get; set; }
        string nomeCliente { get; set; }
    }
    public interface IContaCorrenteVm
    {
        bool bBoleto { get; set; }
        bool bPagamento { get; set; }
        int codContaCorrente { get; set; }
        string nmContaCorrente { get; set; }
        string nmEmpresa { get; set; }
        string CpfCnpj { get; set; }
        string codEmpresa { get; set; }
        string nmEmpresaPagNet { get; set; }
        string CodBanco { get; set; }
        string nmBanco { get; set; }
        string codOperacaoCC { get; set; }
        string nroContaCorrente { get; set; }
        string DigitoCC { get; set; }
        string Agencia { get; set; }
        string DigitoAgencia { get; set; }
        string CodTransmissao { get; set; }
        string CarteiraRemessa { get; set; }
        string VariacaoCarteira { get; set; }
        string CodConvenioPag { get; set; }
        string ParametroTransmissaoPag { get; set; }
        string valTED { get; set; }
        string ValMinPGTO { get; set; }
        string ValMinTED { get; set; }
        string SaldoConta { get; set; }

        string formaTransmissaoPG { get; set; }
        int codigoTransmissaoArquivoPG { get; set; }
        string tipoArquivoPG { get; set; }
        string loginTransmissaoPG { get; set; }
        string senhaTransmissaoPG { get; set; }
        string caminhoRemessaPG { get; set; }
        string caminhoRetornoPG { get; set; }
        string caminhoAuxiliarPG { get; set; }
        string formaTransmissaoBol { get; set; }
        int codigoTransmissaoArquivoBol { get; set; }
        string tipoArquivoBol { get; set; }
        string loginTransmissaoBol { get; set; }
        string senhaTransmissaoBol { get; set; }
        string caminhoRemessaBol { get; set; }
        string caminhoRetornoBol { get; set; }
        string caminhoAuxiliarBol { get; set; }

        bool teraJuros { get; set; }
        string ValorJuros { get; set; }
        string PercJuros { get; set; }
        bool teraMulta { get; set; }
        string valorMulta { get; set; }
        string PercMulta { get; set; }
        string codigoPrimeiraInscricaoCobraca { get; set; }
        string NomePrimeiraInscricaoCobraca { get; set; }
        string codigoSegundaInscricaoCobraca { get; set; }
        string NomeSegundaInscricaoCobraca { get; set; }
        string TaxaEmissaoBoleto { get; set; }
        bool AgruparFaturamentosDia { get; set; }
        string qtPosicaoArqPGTO { get; set; }
        string qtPosicaoArqBoleto { get; set; }
        string codigoCedente { get; set; }
        string digitoCodigoCedente { get; set; }

    }

    public interface IAPIBorderoBolVM
    {
        List<APIListaBorderoBolVM> ListaBordero { get; set; }

        string CaminhoArquivo { get; set; }
        int codigoEmpresa { get; set; }
        int codContaCorrente { get; set; }

    }
    public class FormaTransmissaoArquivoVM
    {
        public int codigoTransmissaoArquivo { get; set; }
        public int codigoContaCorrente { get; set; }
        public string tipoArquivo { get; set; }
        public string formaTransmissao { get; set; }
        public string loginTransmissao { get; set; }
        public string senhaTransmissao { get; set; }
        public string caminhoRemessa { get; set; }
        public string caminhoRetorno { get; set; }
        public string caminhoAuxiliar { get; set; }
    }
}
