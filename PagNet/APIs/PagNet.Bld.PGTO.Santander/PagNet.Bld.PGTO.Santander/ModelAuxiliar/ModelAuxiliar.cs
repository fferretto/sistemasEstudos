using PagNet.Bld.Domain.Entities;
using PagNet.Bld.PGTO.Santander.Util;
using System;
using System.Globalization;

namespace PagNet.Bld.PGTO.Santander.ModelAuxiliar
{
    public class ListaEmissaoTitulosVM
    {
        public ListaEmissaoTitulosVM(PAGNET_EMISSAO_TITULOS x)
        {
            CODIGOTITULO = x.CODTITULO;
            TIPOTITULO = x.TIPOTITULO;
            STATUS = x.STATUS.Replace("_", " ");
            CODBORDERO = Convert.ToInt32(x.CODBORDERO);
            CODFAVORECIDO = Convert.ToInt32(x.CODFAVORECIDO);
            NMFAVORECIDO = (x.PAGNET_CADFAVORECIDO == null) ? "" : Convert.ToInt32(x.CODFAVORECIDO) + " - " + Helper.FormataTexto(x.PAGNET_CADFAVORECIDO.NMFAVORECIDO, 35);
            CNPJ = (x.PAGNET_CADFAVORECIDO == null) ? "" : x.PAGNET_CADFAVORECIDO.CPFCNPJ;
            CODBANCO = (x.PAGNET_CADFAVORECIDO == null) ? "" : x.PAGNET_CADFAVORECIDO.BANCO;
            AGENCIA = (x.PAGNET_CADFAVORECIDO == null) ? "" : x.PAGNET_CADFAVORECIDO.AGENCIA;
            CONTACORRENTE = (x.PAGNET_CADFAVORECIDO == null) ? "" : x.PAGNET_CADFAVORECIDO.CONTACORRENTE;
            DATPGTO = x.DATREALPGTO;
            VALLIQ = x.VALLIQ;
            VALTOTAL = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALTOTAL);
            CODEMPRESA = x.CODEMPRESA;
            PREPAGO = x.SISTEMA == 1;
            LINHADIGITAVEL = x.LINHADIGITAVEL;
        }
        public int CODIGOTITULO { get; set; }
        public string STATUS { get; set; }
        public int CODBORDERO { get; set; }
        public int CODCEN { get; set; }
        public string CNPJ { get; set; }
        public DateTime DATPGTO { get; set; }
        public string CODBANCO { get; set; }
        public string AGENCIA { get; set; }
        public string CONTACORRENTE { get; set; }
        public decimal VALLIQ { get; set; }
        public string VALTOTAL { get; set; }
        public int CODEMPRESA { get; set; }
        public bool PREPAGO { get; set; }
        public int CODFAVORECIDO { get; set; }
        public string NMFAVORECIDO { get; set; }
        public string TIPOTITULO { get; set; }
        public string LINHADIGITAVEL { get; set; }
    }
    public class TransacoesPagamento
    {

        public string SeuNumero { get; set; } //ok
        public string nmFavorecido { get; set; } //ok
        public string CpfCnpj { get; set; }
        public int tipInscricaoFavorecido { get; set; }
        public string codigoBarras { get; set; }//ok
        public string NossoNumero { get; set; } //ok

        public int codTransacaoPagamento { get; set; } //ok
        public int codSubContaCorrente { get; set; }
        public int TipoServico { get; set; } //ok
        public int codFormaLancamento { get; set; } //ok

        public DateTime dtPagamento { get; set; } //ok
        public DateTime dtVencimento { get; set; } //ok

        public decimal Valor { get; set; } //ok
        public decimal vlDesconto { get; set; }
        public decimal vlJurosMulta { get; set; }
        public decimal vlTotalPagar { get; set; }
        internal static TransacoesPagamento ToEntity(PAGNET_TITULOS_PAGOS Titulo)
        {
            return new TransacoesPagamento
            {
                codTransacaoPagamento = Titulo.CODTITULOPAGO,
                TipoServico = Titulo.TIPOSERVICO,
                codFormaLancamento = Titulo.CODFORMALANCAMENTO,
                nmFavorecido = Titulo.PAGNET_CADFAVORECIDO.NMFAVORECIDO,
                CpfCnpj = Helper.RemoveCaracteres(Titulo.PAGNET_CADFAVORECIDO.CPFCNPJ),
                tipInscricaoFavorecido = (Titulo.PAGNET_CADFAVORECIDO.CPFCNPJ.Length <= 11) ? 1 : 2,
                SeuNumero = Titulo.SEUNUMERO,
                NossoNumero = Titulo.NOSSONUMERO,
                codigoBarras = Titulo.LINHADIGITAVEL,
                Valor = Titulo.VALOR,

                dtPagamento = Titulo.DTPAGAMENTO,
                dtVencimento = Titulo.DTVENCIMENTO,
            };
        }


    }
    public class TransacaoEmissaoBoleto
    {
        public string numControle { get; set; }
        public string TipOperacao { get; set; }
        public string TipArquivo { get; set; }
        public string Status { get; set; }
        public string NossoNumero { get; set; }
        public string GeraMulta { get; set; }
        public string SeuNumero { get; set; }
        public string CpfCnpjPag { get; set; }
        public string nmPag { get; set; }
        public string EndPag { get; set; }
        public string EndNumeroPag { get; set; }
        public string EndComplemento { get; set; }
        public string BairroPag { get; set; }
        public string CEPPag { get; set; }
        public string ComplementoCEPPag { get; set; }
        public string CidadePag { get; set; }
        public string UFPag { get; set; }
        public string Observacao { get; set; }
        public string codCarteira { get; set; }


        public int codTransacaoEmissaoBoleto { get; set; }
        public int codContaCorrente { get; set; }
        public int codOcorrencia { get; set; }
        public int codEspecieDoc { get; set; }
        public int codPrimeiraInstCobra { get; set; }
        public int codSegundaInstCobra { get; set; }
        public int TipInscricaoPag { get; set; }
        public int NumDiasProtesto { get; set; }
        public int numSeq { get; set; }
        public int codArquivo { get; set; }


        public DateTime dtLimiteDesconto { get; set; }
        public DateTime dtCobrancaJuros { get; set; }
        public DateTime dtEmissaoTitulo { get; set; }
        public DateTime dtCobrancaMulta { get; set; }
        public DateTime dtSegundoDesconto { get; set; }
        public DateTime dtVencimento { get; set; }

        public decimal Valor { get; set; }
        public decimal vlMultaDiaAtraso { get; set; }
        public decimal PercJuros { get; set; }
        public decimal vlJurosDiaAtraso { get; set; }
        public decimal PercMulta { get; set; }
        public decimal vlDesconto { get; set; }
        public decimal vlSegundoDesconto { get; set; }

    }
    public class TransacaoTransferencia
    {
        public int CodTransacaoTransferencia { get; set; }
        public int TipTransferencia { get; set; }
        public int codBancoFavorecido { get; set; }
        public int TipoServico { get; set; }
        public int codFormaLancamento { get; set; }
        public int tipInscricaoFavorecido { get; set; }
        public int codCamaraCentralizadora { get; set; }

        public string AgenciaFavorecido { get; set; }
        public string DigitoAgenciaFavorecido { get; set; }
        public string ContaCorrenteFavorecido { get; set; }
        public string DigitoContaFavorecido { get; set; }
        public string DigitoVeriricadorAgenciaConta { get; set; }
        public string CpfCnpjFavorecido { get; set; }
        public string nmFavorecido { get; set; }
        public string SeuNumero { get; set; }
        public string NossoNumero { get; set; }
        public string Mensagem { get; set; }
        public string FinalidadeDoc { get; set; }
        public string FinalidadeTed { get; set; }
        public string CodFinalidadeComplementar { get; set; }
        public string EmissaoAvisoFavorecido { get; set; }
        public string OcorrenciaRetorno { get; set; }
        public string CpfCnpj { get; set; }

        public DateTime dtPagamento { get; set; }
        public DateTime dtRealPagamento { get; set; }
        public DateTime dtVencimento { get; set; }


        public decimal Valor { get; set; }

        public int numSeq { get; set; }

        internal static TransacaoTransferencia ToEntity(PAGNET_TITULOS_PAGOS Titulo)
        {
            return new TransacaoTransferencia
            {
                CodTransacaoTransferencia = Titulo.CODTITULOPAGO,
                codBancoFavorecido = Convert.ToInt32(Titulo.PAGNET_CADFAVORECIDO.BANCO),
                TipoServico = Titulo.TIPOSERVICO,
                codFormaLancamento = Titulo.CODFORMALANCAMENTO,
                tipInscricaoFavorecido = (Titulo.PAGNET_CADFAVORECIDO.CPFCNPJ.Length <= 11) ? 1 : 2,
                CpfCnpjFavorecido = Titulo.PAGNET_CADFAVORECIDO.CPFCNPJ,

                AgenciaFavorecido = Titulo.PAGNET_CADFAVORECIDO.AGENCIA,
                DigitoAgenciaFavorecido = Titulo.PAGNET_CADFAVORECIDO.DVAGENCIA,
                ContaCorrenteFavorecido = Titulo.PAGNET_CADFAVORECIDO.CONTACORRENTE,
                DigitoContaFavorecido = Titulo.PAGNET_CADFAVORECIDO.DVCONTACORRENTE,
                DigitoVeriricadorAgenciaConta = "0",
                nmFavorecido = Titulo.PAGNET_CADFAVORECIDO.NMFAVORECIDO,
                SeuNumero = Titulo.SEUNUMERO,
                NossoNumero = Titulo.NOSSONUMERO,

                Valor = Titulo.VALOR,

                dtPagamento = Titulo.DTPAGAMENTO,
                dtRealPagamento = Titulo.DTREALPAGAMENTO,
                dtVencimento = Titulo.DTVENCIMENTO,
                FinalidadeDoc = "01",
                FinalidadeTed = "00010",
                CodFinalidadeComplementar = "CC",
            };
        }

    }
    public class AutenticacaoVM
    {
        public string grant_type { get; set; }
        public string scope { get; set; }
        public string username { get; set; }
        public string password { get; set; }

    }
}
