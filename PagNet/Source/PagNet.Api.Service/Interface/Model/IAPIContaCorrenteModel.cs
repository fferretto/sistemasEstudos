using PagNet.Api.Service.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Api.Service.Interface.Model
{
    public interface IAPIDadosHomologarContaCorrenteModel
    {
        int codigoEmpresa { get; set; }
        int codOPE { get; set; }
        int CodigoContaCorrente { get; set; }
        int codigoUsuario { get; set; }
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
    public interface IAPIContaCorrenteModel
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
    public interface IAPIRetornoDDLModel
    {
        string Valor { get; set; }
        string Descricao { get; set; }
        string Title { get; set; }
    }
    public interface IAPIBancoModel 
    {
        string CODBANCO { get; set; }
        string NMBANCO { get; set; }
    }
    public interface IAPIConsultaContaCorrenteModel
    {
        int codContaCorrente { get; set; }
        string nmContaCorrente { get; set; }
        string nroContaCorrente { get; set; }
        string Agencia { get; set; }
    }

    public interface IAPIResultadoTransmissaoArquivoModel 
    {
        bool Resultado { get; set; }
        string FormaTransmissao { get; set; }
        string msgResultado { get; set; }
        string nomeArquivo { get; set; }
        string CaminhoCompletoArquivo { get; set; }
    }
}
