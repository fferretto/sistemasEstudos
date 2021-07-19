using Newtonsoft.Json;
using PagNet.Application.Helpers;
using PagNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;

namespace PagNet.Application.Models
{
    public class PagamentosVm
    {
        public int codTransacaoPagamento { get; set; }
        public int CodUsuario { get; set; }
        public decimal vlMulta { get; set; }
        public decimal vlJuros { get; set; }
        public decimal vlDesconto { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a conta corrente")]
        [Display(Name = "Conta Corrente")]
        [Ajuda("Conta que será debitado o valor")]
        public string codContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }

        [Display(Name = "Sub Conta Corrente")]
        [Ajuda("Utilizada apenas para organização de pagamentos")]
        public string codSubContaCorrente { get; set; }
        public string nmSubContaCorrente { get; set; }

        [Display(Name = "Data do Pagamento")]
        [Ajuda("Data que será realizado o pagamento")]
        public DateTime dtPagamento { get; set; }

        [Display(Name = "Data de Vencimento")]
        [Required(ErrorMessage = "Obrigatório informar a data de vencimento")]
        [Ajuda("Data de vencimento do boleto")]
        public DateTime dtVencimento { get; set; }

        [Display(Name = "Valor do Boleto")]
        [Required(ErrorMessage = "obrigatório informar o valor do boleto")]
        [Ajuda("Informe o valor total do boleto")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(10)]
        public string Valor { get; set; }

        [Display(Name = "Número de Controle")]
        [Required(ErrorMessage = "Obrigatório informar o número de controle")]
        [Ajuda("Número utilizado pela empresa para identificar os pagamentos")]
        public string SeuNumero { get; set; }

        [Display(Name = "Nome/Empresa Favorecida")]
        [Required(ErrorMessage = "obrigatório informar o Nome / Empresa Favorecida")]
        [Ajuda("Nome da empresa para a qual está efetuando o pagamento")]
        public string nmFavorecido { get; set; }

        [Display(Name = "Código de Barras")]
        [Ajuda("Linha digitável do código de barras")]
        public string codigoBarras { get; set; }

        [Display(Name = "Código de Barras")]
        [Ajuda("Linha digitável do código de barras")]
        public string codigoBarras1 { get; set; }
        
        [Display(Name = " ")]
        public string codigoBarras2 { get; set; }

        [Display(Name = " ")]
        public string codigoBarras3 { get; set; }

        [Display(Name = " ")]
        [MaxLength(1)]
        public string codigoBarras4 { get; set; }

        [Display(Name = " ")]
        public string codigoBarras5 { get; set; }
    }
    public class JustificativaVm
    {
        [Display(Name = "Justificativa")]
        [Ajuda("Informe o motivo")]
        public string codJustificativa { get; set; }
        public string descJustificativa { get; set; }


        [Display(Name = "Conta Corrente *")]
        public string codContaCorrenteBaixaManual { get; set; }
        public string nmContaCorrenteBaixaManual { get; set; }

        [Display(Name = "Especifique")]
        [Ajuda("Especifique o motivo da ação.")]
        [StringLength(200)]
        public string DescJustOutros { get; set; }

        [Display(Name = "Títulos Selecionados")]
        [Ajuda("Quantidade de títulos selecionados para realizar esta ação")]
        public string qtTitulosSelBaixaManual { get; set; }

        [Display(Name = "Valor Total")]
        [Ajuda("Valor total dos títulos selecionados")]
        [InputAttrAux(Inicio = "R$")]
        public string ValorTotalBaixaManual { get; set; }
    }
    public class FiltroTitulosPagamentoVM
    {
        public FiltroTitulosPagamentoVM()
        {
        }

        public int codFavorecido { get; set; }
        public string codBanco { get; set; }
        public string CaminhoArquivoDownload { get; set; }
        public bool acessoAdmin { get; set; }
        public bool cartaoPos { get; set; }
        public bool cartaoPre { get; set; }

        [Display(Name = "Código do Banco")]
        [Ajuda("Informe o Código do banco dos credenciados que deseja realizar o pagamento")]
        public string filtroCodBanco { get; set; }

        [Display(Name = "Nome do Banco")]
        public string FiltroNmBanco { get; set; }

        [Display(Name = "Código")]
        [Ajuda("Informe o CPF/CNPJ ou o código da Centralizadora/Fornecedor")]
        public string filtro { get; set; }

        [Display(Name = "Favorecido")]
        public string nmFavorecido { get; set; }

        [Display(Name = "Data Inicio")]
        [Ajuda("Data de pagamento")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string dtInicio { get; set; }

        [Display(Name = "Data Fim")]
        [Ajuda("Data de pagamento")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string dtFim { get; set; }

        [Display(Name = "Empresa")]
        public string codEmpresa { get; set; }
        public string nmEmpresa { get; set; }

        [Display(Name = "Forma de Pagamento *")]
        public string CodFormaPagamento { get; set; }
        public string nmFormaPagamento { get; set; }

        [Display(Name = "Conta Corrente *")]
        public string codContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }


        internal static FiltroTitulosPagamentoVM ToViewCredenciado(PAGNET_CADFAVORECIDO Fav, string _filtro)
        {
            return new FiltroTitulosPagamentoVM
            {
                codFavorecido = Fav.CODFAVORECIDO,
                nmFavorecido = Fav.NMFAVORECIDO,
                filtro = _filtro
            };
        }
        internal static FiltroTitulosPagamentoVM ToViewBanco(PAGNET_BANCO _Banco, string _filtro)
        {
            return new FiltroTitulosPagamentoVM
            {
                filtroCodBanco = _Banco.CODBANCO,
                codBanco = _Banco.CODBANCO,
                FiltroNmBanco = _Banco.NMBANCO,
                filtro = _filtro
            };
        }
    }
    public class FiltroConsultaBorderoPagVM
    {
        public string codBanco { get; set; }
        public bool acessoAdmin { get; set; }

        [Display(Name = "Código do Borderô")]
        public string codBordero { get; set; }

        [Display(Name = "Código do Banco")]
        [Ajuda("Informe o Código do banco dos credenciados que deseja realizar o pagamento")]
        public string filtroCodBanco { get; set; }

        [Display(Name = "Nome do Banco")]
        public string FiltroNmBanco { get; set; }

        [Display(Name = "Empresa *")]
        [Ajuda("Filtrar os borderôs por Empresa")]
        public string codEmpresa { get; set; }
        public string nmEmpresa { get; set; }

        [Display(Name = "Forma de Pagamento")]
        [Ajuda("Filtrar todos os borderôs com a forma de pagamento listada a baixo")]
        public string CodFormaPagamento { get; set; }
        public string nmFormaPagamento { get; set; }

        [Display(Name = "Status")]
        public string codStatus { get; set; }
        public string nmStatus { get; set; }

        [Display(Name = "Conta Corrente *")]
        public string codContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }


        internal static FiltroConsultaBorderoPagVM ToViewBanco(PAGNET_BANCO _Banco, string _filtro)
        {
            return new FiltroConsultaBorderoPagVM
            {
                filtroCodBanco = _Banco.CODBANCO,
                codBanco = _Banco.CODBANCO,
                FiltroNmBanco = _Banco.NMBANCO
            };
        }
    }
    public class GridTitulosVM
    {
        public GridTitulosVM()
        {
            ListaFechamento = new List<ListaTitulosVM>();
        }
        public string Justificativa { get; set; }
        public string JustificativaOutros { get; set; }

        [Display(Name = "Lista de Títulos Pendente de Pagamento")]
        public IList<ListaTitulosVM> ListaFechamento { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a conta corrente")]
        [Display(Name = "Conta Corrente")]
        [Ajuda("Conta que será debitado o valor")]
        public string codContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }

        [Display(Name = "Forma de Pagamento")]
        public int FormaPagamento { get; set; }
        public int codFormaPagamento { get; set; }

        [Display(Name = "Nova Data de Pagamento")]
        [Ajuda("Data que será realizado o pagamento.")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string dtTransferencia { get; set; }

        public string dtInicioGrid { get; set; }
        public string dtFimGrid { get; set; }
        public bool cartaoPre { get; set; }
        public bool cartaoPos { get; set; }
        public string CaminhoArquivo { get; set; }
        public int codUsuario { get; set; }
        public int codOpe { get; set; }
        public string codSubRede { get; set; }


        internal static GridTitulosVM ToView(List<PROC_PAGNET_CONS_TITULOS_BORDERO> Titulos, DateTime dtIni, DateTime dtFinal)
        {
            return new GridTitulosVM    
            {
                codFormaPagamento = 1,
                CaminhoArquivo = "",
                dtTransferencia = DateTime.Now.ToShortDateString(),
                dtFimGrid = dtFinal.ToShortDateString(),
                dtInicioGrid = dtIni.ToShortDateString(),
                ListaFechamento = ListaTitulosVM.ToView(Titulos)

            };
        }
        internal static GridTitulosVM ToViewTitulosVencidos(List<PROC_PAGNET_CONS_TITULOS_VENCIDOS> Titulos)
        {
            return new GridTitulosVM
            {
                dtTransferencia = DateTime.Now.ToShortDateString(),
                ListaFechamento = ListaTitulosVM.ToViewTitVencidos(Titulos)

            };
        }

    }
    public class LogTituloVM
    {
        public LogTituloVM(PAGNET_EMISSAO_TITULOS_LOG x)
        {
            CODIGOTITULO = x.CODTITULO;
            CODTITULO_LOG = x.CODTITULO_LOG;
            STATUS = x.STATUS.Replace("_"," ");
            CODBORDERO = Convert.ToString(x.CODBORDERO);
            DATEMISSAO = x.DATEMISSAO.ToShortDateString();
            DATPGTO = x.DATPGTO.ToShortDateString();
            DATREALPGTO = x.DATREALPGTO.ToShortDateString();
            CODBANCO = x.PAGNET_CADFAVORECIDO.BANCO;
            AGENCIA = x.PAGNET_CADFAVORECIDO.AGENCIA + "-" + x.PAGNET_CADFAVORECIDO.DVAGENCIA;
            CONTACORRENTE = x.PAGNET_CADFAVORECIDO.CONTACORRENTE + "-" + x.PAGNET_CADFAVORECIDO.DVCONTACORRENTE;
            CODEMPRESA = x.CODEMPRESA.ToString();
            SISTEMA = (x.SISTEMA == 1) ? "Pré Pago": "Pós Pago";
            CODUSUARIO = x.CODUSUARIO.ToString();
            NMUSUARIO = x.USUARIO_NETCARD.NMUSUARIO.Trim();
            DATINCLOG = x.DATINCLOG;
            DESCLOG = x.DESCLOG.Trim();
            //VALTAXAS = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal(x.VALTAXAS));
            VALBRUTO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal(x.VALBRUTO));
            VALLIQ = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal(x.VALLIQ));
            VALTOTAL = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal(x.VALTOTAL));
        }


        [Display(Name = "Código do Título")]
        public int CODIGOTITULO { get; set; }

        [Display(Name = "Código do Título")]
        public int CODTITULO_LOG { get; set; }

        [Display(Name = "Status")]
        public string STATUS { get; set; }

        [Display(Name = "Cod. Borderô")]
        public string CODBORDERO { get; set; }

        [Display(Name = "Centralizadora")]
        public string CODFAVORECIDO { get; set; }

        [Display(Name = "Data Emissao")]
        public string DATEMISSAO { get; set; }

        [Display(Name = "Data PGTO")]
        public string DATPGTO { get; set; }

        [Display(Name = "Data Real PGTO")]
        public string DATREALPGTO { get; set; }

        [Display(Name = "Banco")]
        public string CODBANCO { get; set; }

        [Display(Name = "Agência")]
        public string AGENCIA { get; set; }

        [Display(Name = "Conta")]
        public string CONTACORRENTE { get; set; }

        [Display(Name = "Valor PGTO")]
        public string VALLIQ { get; set; }

        [Display(Name = "Cod. Empresa")]
        public string CODEMPRESA { get; set; }

        [Display(Name = "Tipo Cartao")]
        public string SISTEMA { get; set; }

        [Display(Name = "Cod. Usuario")]
        public string CODUSUARIO { get; set; }

        [Display(Name = "Usuário")]
        public string NMUSUARIO { get; set; }

        [Display(Name = "Data Ação")]
        public DateTime DATINCLOG { get; set; }

        [Display(Name = "Descrição")]
        public string DESCLOG { get; set; }


        [Display(Name = "Taxas")]
        public string VALTAXAS { get; set; }

        [Display(Name = "Valor Bruto")]
        public string VALBRUTO { get; set; }

        [Display(Name = "Total a Pagar")]
        public string VALTOTAL { get; set; }

    }
    public class ListaTitulosVM
    {

        internal static IList<ListaTitulosVM> ToView<T>(ICollection<T> collection) where T : PROC_PAGNET_CONS_TITULOS_BORDERO
        {
            return collection.Select(x => ToView(x)).ToList();
        }

        internal static ListaTitulosVM ToView(PROC_PAGNET_CONS_TITULOS_BORDERO x)
        {
            return new ListaTitulosVM
            {
                chkFechCred = true,
                CODFAVORECIDO = x.CODFAVORECIDO,
                RAZSOC = Geral.FormataTexto(x.NMFAVORECIDO, 30),
                CNPJ = Geral.FormataCPFCnPj(x.CPFCNPJ),
                DATPGTO = x.DATREALPGTO.ToString("dd/MM/yyyy"),
                QTTITULOS = x.QTTITULOS.ToString(),
                VALLIQ = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALORPREVISTOPAGAMENTO),
                BANCO = x.BANCO,
                AGENCIA = x.AGENCIA,
                CONTACORRENTE = x.CONTACORRENTE,
                TIPPAGNET = x.TIPOTITULO,
                DESFORPAG = string.IsNullOrWhiteSpace(x.DESFORPAG) ? "Vazio" : x.DESFORPAG,
                LINHADIGITAVEL = x.LINHADIGITAVEL

            };
        }


        internal static IList<ListaTitulosVM> ToViewTitVencidos<T>(ICollection<T> collection) where T : PROC_PAGNET_CONS_TITULOS_VENCIDOS
        {
            return collection.Select(x => ToViewTitVencidos(x)).ToList();
        }

        internal static ListaTitulosVM ToViewTitVencidos(PROC_PAGNET_CONS_TITULOS_VENCIDOS x)
        {
            return new ListaTitulosVM
            {
                chkFechCred = true,
                CODTITULO = x.CODTITULO,
                CODFAVORECIDO = x.CODFAVORECIDO,
                RAZSOC = Geral.FormataTexto(x.NMFAVORECIDO, 30),
                CNPJ = Geral.FormataCPFCnPj(x.CPFCNPJ),
                DATPGTO = x.DATREALPGTO.ToString("dd/MM/yyyy"),
                VALLIQ = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALTOTAL),
                BANCO = x.BANCO,
                AGENCIA = x.AGENCIA,
                CONTACORRENTE = x.CONTACORRENTE

            };
        }
        public bool chkFechCred { get; set; }
        public string MSGRETBANCO { get; set; }
        public int CODTITULO { get; set; }

        [Display(Name = "Código do Favorecido")]
        public int CODFAVORECIDO { get; set; }

        [Display(Name = "Nome do Favorecido")]
        public string RAZSOC { get; set; }

        [Display(Name = "CNPJ")]
        public string CNPJ { get; set; }

        [Display(Name = "Data de Vencimento")]
        public string DATPGTO { get; set; }

        [Display(Name = "Quant. Títulos")]
        public string QTTITULOS { get; set; }        

        [Display(Name = "Valor a Pagar")]
        public string VALLIQ { get; set; }

        [Display(Name = "Quant.")]
        public string QTETRA { get; set; }

        [Display(Name = "Banco")]
        public string BANCO { get; set; }

        [Display(Name = "Agencia")]
        public string AGENCIA { get; set; }

        [Display(Name = "Conta Corrente")]
        public string CONTACORRENTE { get; set; }
        
        [Display(Name = "CÓDIGO de Pagamento")]
        public string TIPPAGNET { get; set; }

        [Display(Name = "Forma de Pagamento")]
        public string DESFORPAG { get; set; }

        [Display(Name = "Linha Digiável")]
        public string LINHADIGITAVEL { get; set; }



    }
    public class ListaEmissaoTitulosVM
    {
        public ListaEmissaoTitulosVM(PAGNET_EMISSAO_TITULOS x)
        {
            CODIGOTITULO = x.CODTITULO;
            TIPOTITULO = x.TIPOTITULO;
            STATUS = x.STATUS.Replace("_", " ");
            CODBORDERO = Convert.ToInt32(x.CODBORDERO);
            CODFAVORECIDO = Convert.ToInt32(x.CODFAVORECIDO);
            NMFAVORECIDO = (x.PAGNET_CADFAVORECIDO == null ) ? "" : Convert.ToInt32(x.CODFAVORECIDO) + " - " + Geral.FormataTexto(x.PAGNET_CADFAVORECIDO.NMFAVORECIDO, 35);
            CNPJ = (x.PAGNET_CADFAVORECIDO == null) ? "" : x.PAGNET_CADFAVORECIDO.CPFCNPJ;
            CODBANCO = (x.PAGNET_CONTACORRENTE == null) ? "" : x.PAGNET_CONTACORRENTE.CODBANCO;
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
    public class DadosTituloVm
    {

        public int CODIGOTITULO { get; set; }
        public int CODEMPRESA { get; set; }
        public int SISTEMA { get; set; }
        public int CODUSUARIO { get; set; }
        public int CODOPERADORA { get; set; }

        public string CODBANCO_ORI { get; set; }
        public string CODPGTO_ORI { get; set; }

        [Display(Name = "Status do Título")]
        public string STATUS { get; set; }

        [Display(Name = "Razão Social")]
        public string NMFAVORECIDO { get; set; }

        [Display(Name = "CPF/CNPJ")]
        public string CNPJ { get; set; }

        [Display(Name = "Código")]
        public int CODBORDERO { get; set; }

        [Display(Name = "Código")]
        public int CODFAVORECIDO { get; set; }

        [Display(Name = "Data de Emissão do Título")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string DATEMISSAO { get; set; }

        [Display(Name = "Data de Pagamento")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string DATPGTO { get; set; }

        [Display(Name = "Data Real de Pagamento")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string DATREALPGTO { get; set; }

        [Display(Name = "Banco")]
        [StringLength(4)]
        public string CODBANCO { get; set; }

        [Display(Name = "Agencia")]
        [StringLength(5)]
        public string AGENCIA { get; set; }

        [Display(Name = "DV Agência")]
        [StringLength(1)]
        public string DVAGENCIA { get; set; }

        [Display(Name = "Conta Corrente")]
        [StringLength(9)]
        public string CONTACORRENTE { get; set; }

        [Display(Name = "DV da Conta")]
        [StringLength(1)]
        public string DVCONTACORRENTE { get; set; }

        [Display(Name = "Valor a Pagar")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string VALLIQ { get; set; }

        [Display(Name = "Valor Bruto NetCard")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string VALBRUTO { get; set; }

        [Display(Name = "Valor das Taxa")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string VALTAXAS { get; set; }

        [Display(Name = "Aplicar Acréscimo")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string VALACRESCIMO { get; set; }
        
        [Display(Name = "Aplicar Desconto")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string VALDESCONTO { get; set; }

        [Display(Name = "Justificativa")]
        [Ajuda("Informe o motivo da edição do título")]
        public string codJustificativa { get; set; }
        public string descJustificativa { get; set; }
        
        [Display(Name = "Especifique")]
        [Ajuda("Especifique o motivo da edição.")]
        [StringLength(200)]
        public string DescJustOutros { get; set; }

        [Display(Name = "Plano de Contas")]
        public string CodigoPlanoContas { get; set; }
        public string NomePlanoContas { get; set; }


        internal static DadosTituloVm ToView(PAGNET_EMISSAO_TITULOS x)
        {
            return new DadosTituloVm
            {
                CODIGOTITULO = x.CODTITULO,
                NMFAVORECIDO = x.PAGNET_CADFAVORECIDO.NMFAVORECIDO.Trim(),
                STATUS = x.STATUS,
                CODEMPRESA = x.CODEMPRESA,
                SISTEMA = x.SISTEMA,
                CODBORDERO = Convert.ToInt32(x.CODBORDERO),
                CODFAVORECIDO = Convert.ToInt32(x.CODFAVORECIDO),
                DATEMISSAO = x.DATEMISSAO.ToShortDateString(),
                DATPGTO = x.DATPGTO.ToShortDateString(),
                DATREALPGTO = x.DATREALPGTO.ToShortDateString(),
                CODBANCO = x.PAGNET_CADFAVORECIDO.BANCO,
                AGENCIA = x.PAGNET_CADFAVORECIDO.AGENCIA,
                DVAGENCIA = x.PAGNET_CADFAVORECIDO.DVAGENCIA,
                CONTACORRENTE = x.PAGNET_CADFAVORECIDO.CONTACORRENTE,
                DVCONTACORRENTE = x.PAGNET_CADFAVORECIDO.DVCONTACORRENTE,
                VALLIQ = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALTOTAL)).Replace("R$", ""),
                VALBRUTO = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALBRUTO)).Replace("R$", ""),
                VALTAXAS = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (x.VALBRUTO-x.VALLIQ))).Replace("R$", ""),
                CNPJ = Geral.FormataCPFCnPj(x.PAGNET_CADFAVORECIDO.CPFCNPJ.Trim()),
                CodigoPlanoContas = (x.PAGNET_CADPLANOCONTAS !=  null) ? x.CODPLANOCONTAS.ToString(): "2",
                NomePlanoContas = (x.PAGNET_CADPLANOCONTAS != null) ? x.PAGNET_CADPLANOCONTAS.CLASSIFICACAO + " "  + x.PAGNET_CADPLANOCONTAS.DESCRICAO : "2 Despesas",
                CODBANCO_ORI = x.PAGNET_CADFAVORECIDO.BANCO,
            };
        }

    }
    public class AjustarValorTitulo
    {
        public int codigoTituloAjusteValor { get; set; }
        public int codigoEmpresaAjusteValor { get; set; }
        public bool Desconto { get; set; }
        public int codigoUsuarioAjusteValor { get; set; }

        [Display(Name = "Razão Social")]
        public string nomeFavorecido { get; set; }

        [Display(Name = "CPF/CNPJ")]
        public string cpfCnpj { get; set; }

        [Display(Name = "Código")]
        public int codigoFavorecido { get; set; }

        [Display(Name = "Valor Atual a Pagar")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string ValorAtual { get; set; }

        [Display(Name = "Valor")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string valorConcedido { get; set; }

        [Display(Name = "Novo Valor")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string NovoValor { get; set; }

        [Display(Name = "Descriçao")]
        [Required(ErrorMessage = "Obrigatório descrever a ação a ser realizada.")]
        [StringLength(100)]
        public string Descricao { get; set; }
    }
    public class ConsultaArquivosRemessaVM
    {
        public ConsultaArquivosRemessaVM(PAGNET_ARQUIVO x, int codigoEmpresa)
        {
            CodArquivo = x.CODARQUIVO;
            nmArquivo = x.NMARQUIVO.Trim();
            CodBanco = x.CODBANCO.Trim();
            Status = x.STATUS.Trim().Replace("_", " ");
            dtArquivo = x.DTARQUIVO.ToShortDateString();
            CaminhoArquivo = Path.GetFileNameWithoutExtension(x.NMARQUIVO.Trim());
            vlTotal = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VLTOTAL);
            qtRegistros = x.QTREGISTRO;
            codigoEmpresaArquivo = codigoEmpresa;
        }


        public int CodArquivo { get; set; }
        public int codigoEmpresaArquivo { get; set; }
        public string CaminhoArquivo { get; set; }

        [Display(Name = "Nome Arquivo")]
        public string nmArquivo { get; set; }

        [Display(Name = "Banco")]
        public string CodBanco { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Data de Emissão")]
        public string dtArquivo { get; set; }

        [Display(Name = "Valor Total")]
        public string vlTotal { get; set; }

        [Display(Name = "Quant. Títulos")]
        public int qtRegistros { get; set; }
    }
    public class BaixaPagamentoVM
    {
        public string SeuNumero { get; set; }
        public string CodRetorno { get; set; }
        public string MsgRetorno { get; set; }
        public string Resumo { get; set; }


        [Display(Name = "Valor Total do Arquivo")]
        public string vlTotalArquivo { get; set; }

        [Display(Name = "Quantidade de Registros no Arquivo")]
        public int qtRegistroArquivo { get; set; }

        [Display(Name = "Total liquidados")]
        public int qtRegistroOK { get; set; }

        [Display(Name = "Total Recusados")]
        public int qtRegistroFalha { get; set; }

        [Display(Name = "Valor Total")]
        public string vlTotal { get; set; }

        [Display(Name = "Quantidade Pagamentos")]
        public int qtRegistros { get; set; }

        [Display(Name = "Nome do Favorecido")]
        public string RAZSOC { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "CNPJ")]
        public string CNPJ { get; set; }

        [Display(Name = "Data PGTO")]
        public string DATPGTO { get; set; }

        [Display(Name = "Valor Pago")]
        public string VALLIQ { get; set; }

        [Display(Name = "Lote")]
        public string NUMFECCRE { get; set; }

        [Display(Name = "Mensagem de Retorno")]
        public string MsgRetBanco { get; set; }

    }
    public class FiltroDownloadArquivoVm
    {

        public bool acessoAdmin { get; set; }

        [Display(Name = "Empresa")]
        public string codEmpresa { get; set; }
        public string nmEmpresa { get; set; }


        [Display(Name = "Data Inicio")]
        [Ajuda("Filtro de início da data de transferência")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string dtInicio { get; set; }

        [Display(Name = "Data Fim")]
        [Ajuda("Filtro de término da data de transferência")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string dtFim { get; set; }


    }
    public class ConsultaFechamentoCredVm
    {
        public int codFavorecido { get; set; }
        public bool acessoAdmin { get; set; }

        [Display(Name = "Código do Título")]
        [Ajuda("Ao pesquisar pelo código do título, o sistema descartará os outros filtros preenchidos, com exceção da empresa.")]
        public string CodigoTitulo { get; set; }

        [Display(Name = "Empresa")]
        public string codEmpresa { get; set; }
        public string nmEmpresa { get; set; }

        [Display(Name = "Status")]
        public string codStatus { get; set; }
        public string nmStatus { get; set; }

        [Display(Name = "Código")]
        [Ajuda("Informe o CPF/CNPJ ou o código do Fornecedor/Centralizadora")]
        public string filtro { get; set; }

        [Display(Name = "Favorecido")]
        public string nmFavorecido { get; set; }

        [Display(Name = "Data Inicio")]
        [Ajuda("Filtro de início da data de pagamento")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string dtInicio { get; set; }

        [Display(Name = "Data Fim")]
        [Ajuda("Filtro de término da data de pagamento")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string dtFim { get; set; }

        internal static ConsultaFechamentoCredVm ToView(string _filtro, string nmFav, int codFav, DateTime dtini, DateTime dtfim)
        {
            return new ConsultaFechamentoCredVm
            {
                codFavorecido = codFav,
                nmFavorecido = nmFav,
                dtInicio = dtini.ToShortDateString(),
                dtFim = dtfim.ToShortDateString(),
                filtro = _filtro
            };
        }
    }
    public class ListaTitulosPGTOVM
    {
        public ListaTitulosPGTOVM(PAGNET_EMISSAO_TITULOS x)
        {
            if (x != null)
            {
                CODTITULO = x.CODTITULO;
                CODFAVORECIDO = x.CODFAVORECIDO.ToString();
                NMFAVORECIDO = x.PAGNET_CADFAVORECIDO.NMFAVORECIDO;
                CNPJ = Geral.FormataCPFCnPj(x.PAGNET_CADFAVORECIDO.CPFCNPJ);
                DATEMISSAO = x.DATEMISSAO.ToShortDateString();
                DATREALPGTO = x.DATREALPGTO.ToShortDateString();
                DATPGTO = x.DATPGTO.ToShortDateString();
                //VALTAXAS = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALTAXAS);
                VALBRUTO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALBRUTO);
                VALLIQ = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALLIQ);
                VALTOTAL = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALTOTAL);
                TIPOCARTAO = (x.SISTEMA  == 0) ? "Pós Pago" : "Pré Pago";
            }
        }

        [Display(Name = "Cod. Título")]
        public int CODTITULO { get; set; }

        [Display(Name = "Cod. Favorecido")]
        public string CODFAVORECIDO { get; set; }

        [Display(Name = "Favorecido")]
        public string NMFAVORECIDO { get; set; }

        [Display(Name = "CNPJ")]
        public string CNPJ { get; set; }

        [Display(Name = "Data de Emissao")]
        public string DATEMISSAO { get; set; }

        [Display(Name = "Data Real de Pagamento")]
        public string DATREALPGTO { get; set; }

        [Display(Name = "Data de Pagamento")]
        public string DATPGTO { get; set; }

        [Display(Name = "vl. Bruto")]
        public string VALBRUTO { get; set; }

        [Display(Name = "vl. Liquido")]
        public string VALLIQ { get; set; }

        [Display(Name = "vl. Desconto")]
        public string VALDESCONTO { get; set; }

        [Display(Name = "vl. Acrescimo")]
        public string VALACRESCIMO { get; set; }
        
        [Display(Name = "Tipo Cartão")]
        public string TIPOCARTAO { get; set; }

        [Display(Name = "Linha Digitável")]
        public string LINHADIGITAVEL { get; set; }

        [Display(Name = "vl. Taxa")]
        public string VALTAXAS { get; set; }

        [Display(Name = "vl. Total")]
        public string VALTOTAL { get; set; }


    }
    public class ListaTitulosPagVM
    {
        public ListaTitulosPagVM(PROC_PAGNET_CONSULTA_TITULOS x)
        {
            if (x != null)
            {
                CODTITULO = x.CODTITULO;
                CODFAVORECIDO = x.CODFAVORECIDO.ToString();
                NMFAVORECIDO = x.CODFAVORECIDO.ToString() + " - " + Geral.FormataTexto(x.NMFAVORECIDO, 28).Trim();
                CNPJ = Geral.FormataCPFCnPj(x.CPFCNPJ);
                DATPGTO = x.DATPGTO.ToShortDateString();
                DATEMISSAO = x.DATEMISSAO.ToShortDateString();
                DATREALPGTO = x.DATREALPGTO.ToShortDateString();
                VALORTAXA = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Math.Abs(x.VALTAXA));
                VALORBRUTO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALBRUTO);
                VALLIQ = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALLIQ);
                ValorTotalPagar = x.VALTOTAL.ToString();
                VALTOTAL = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALTOTAL);
                STATUS = x.STATUS.Replace("_", " ");
                TIPCARTAO = x.TIPCARTAO.Replace("_", " ");
                BANCO = (x.BANCO);
                AGENCIA = x.AGENCIA;
                CONTA = x.CONTACORRENTE;
                TITVENCIDO = Geral.ValidaTituloPendenteVencido(x.DATREALPGTO, x.STATUS);
                MSGRETBANCO = x.MSGRETORNO.Trim();
            }
        }


        [Display(Name = "Código")]
        public int CODTITULO { get; set; }
        public string TITVENCIDO { get; set; }
        public string MSGRETBANCO { get; set; }


        [Display(Name = "Total a Pagar")]
        [InputAttrAux(Inicio = "R$")]
        public string ValorTotalPagar { get; set; }

        [Display(Name = "Total Bruto")]
        [InputAttrAux(Inicio = "R$")]
        public string ValorTotalBruto { get; set; }

        [Display(Name = "Total Taxas")]
        [InputAttrAux(Inicio = "R$")]
        public string ValorTotalTaxas { get; set; }

        [Display(Name = "Tipo Cartão")]
        public string TIPCARTAO { get; set; }

        [Display(Name = "Código do Favorecido")]
        public string CODFAVORECIDO { get; set; }

        [Display(Name = "Favorecido")]
        public string NMFAVORECIDO { get; set; }
        
        [Display(Name = "CNPJ")]
        public string CNPJ { get; set; }

        [Display(Name = "Data Pgto")]
        public string DATPGTO { get; set; }

        [Display(Name = "Data Emissao")]
        public string DATEMISSAO { get; set; }

        [Display(Name = "Data Real de PGTO")]
        public string DATREALPGTO { get; set; }

        [Display(Name = "Valor Bruto")]
        public string VALORBRUTO { get; set; }

        [Display(Name = "Valor Taxas")]
        public string VALORTAXA { get; set; }

        [Display(Name = "Valor Líquido")]
        public string VALLIQ { get; set; }

        [Display(Name = "Valor a Pagar")]
        public string VALTOTAL { get; set; }        
        
        [Display(Name = "Status")]
        public string STATUS { get; set; }


        [Display(Name = "Banco")]
        public string BANCO { get; set; }

        [Display(Name = "Agencia")]
        public string AGENCIA { get; set; }

        [Display(Name = "Conta")]
        public string CONTA { get; set; }


    }
    public class FiltroBorderoPagVM
    {
        public FiltroBorderoPagVM()
        {
            ListaFechamento = new List<ListaTitulosVM>();
        }

        [Display(Name = "Lista de Títulos")]
        public IList<ListaTitulosVM> ListaFechamento { get; set; }

        public int codUsuario { get; set; }
        public int codOpe { get; set; }
        public string codEmpresa { get; set; }
        public int codigoContaCorrente { get; set; }
        public string codigoFormaPGTO { get; set; }
        public string codigoBanco { get; set; }

        public string codJustificativa { get; set; }
        public string descJustificativa { get; set; }
        public string DescJustOutros { get; set; }


        [Display(Name = "Títulos Selecionados")]
        [Ajuda("Quantidade de títulos que serão incluídos neste borderô")]
        public string qtTitulosSelecionados { get; set; }

        [Display(Name = "Valor Borderô")]
        [Ajuda("Valor total a ser incluído neste borderô")]
        [InputAttrAux(Inicio = "R$")]
        public string ValorBordero { get; set; }



        internal static FiltroBorderoPagVM ToView(List<PROC_PAGNET_CONS_TITULOS_BORDERO> Credenciado, DateTime dtIni, DateTime dtFinal)
        {
            return new FiltroBorderoPagVM
            {
                ListaFechamento = ListaTitulosVM.ToView(Credenciado)

            };
        }
    }
    public class BorderoPagVM
    {
        public BorderoPagVM()
        {
            ListaBordero = new List<ListaBorderoPagVM>();
        }

        public string codBanco { get; set; }
        public int codUsuario { get; set; }
        public int codOpe { get; set; }
        public int codEmpresa { get; set; }
        public bool acessoAdmin { get; set; }
        public string CaminhoArquivo { get; set; }
        public string codigoEmpresa{ get; set; }
        public string codigoFormaPGTO { get; set; }
        public string codigoBanco { get; set; }



        [Display(Name = "Lista dos Borderôs Criados")]
        public IList<ListaBorderoPagVM> ListaBordero { get; set; }

        [Display(Name = "Conta Corrente *")]
        public string codContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }

        [Display(Name = "Valor Borderô")]
        [Ajuda("Valor total a ser incluído neste borderô")]
        [InputAttrAux(Inicio = "R$")]
        public string ValorArquivo { get; set; }


        internal static BorderoPagVM ToView(List<PROC_PAGNET_CONS_BORDERO> Bordero)
        {
            return new BorderoPagVM
            {
                ListaBordero = ListaBorderoPagVM.ToView(Bordero)

            };
        }
    }
    public class ListaBorderoPagVM
    {
        internal static IList<ListaBorderoPagVM> ToView<T>(ICollection<T> collection) where T : PROC_PAGNET_CONS_BORDERO
        {
            return collection.Select(x => ToView(x)).ToList();
        }

        internal static ListaBorderoPagVM ToView(PROC_PAGNET_CONS_BORDERO x)
        {
            return new ListaBorderoPagVM
            {
                chkFechCred = true,
                CODBORDERO = x.CODBORDERO,
                STATUS = x.STATUS.Replace("_", " "),
                CODUSUARIO = x.CODUSUARIO,
                VLBORDERO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VLBORDERO),
                DTBORDERO = x.DTBORDERO,
                CODEMPRESA = x.CODEMPRESA,
                TITVENCIDO = x.TITULOVENCIDO

            };
        }
        public string TITVENCIDO { get; set; }

        public bool chkFechCred { get; set; }
        [Display(Name = "Código do Borderô")]
        public int CODBORDERO { get; set; }

        [Display(Name = "Status")]
        public string STATUS { get; set; }

        [Display(Name = "Forma Pagamento")]
        public string FORMAPGTO { get; set; }

        [Display(Name = "Código do Usuario")]
        public int CODUSUARIO { get; set; }

        [Display(Name = "Código do Banco")]
        public string CODBANCO { get; set; }

        [Display(Name = "Valor do Borderô")]
        public string VLBORDERO { get; set; }

        [Display(Name = "Data de emissão do Borderô")]
        public DateTime DTBORDERO { get; set; }

        [Display(Name = "Codigo da Empresa")]
        public int CODEMPRESA { get; set; }
    }
    public class ConstulaPagNetFechCredVM
    {
        [Display(Name = "Código")]
        public int Codigo { get; set; }

        [Display(Name = "Status")]
        public string STATUS { get; set; }

        [Display(Name = "Borderô Nº")]
        public int CODBORDERO { get; set; }

        [Display(Name = "Código")]
        public int CODFAVORECIDO { get; set; }

        [Display(Name = "Favorecido")]
        public string NMFAVORECIDO { get; set; }

        [Display(Name = "CNPJ")]
        public string CNPJ { get; set; }

        [Display(Name = "Data Emissao")]
        public string dtBordero { get; set; }

        [Display(Name = "Data Real PGTO")]
        public string dtAgendamento { get; set; }

        [Display(Name = "Banco")]
        public string CODBANCO { get; set; }

        [Display(Name = "Agência")]
        public string AGENCIA { get; set; }

        [Display(Name = "Conta")]
        public string CONTACORRENTE { get; set; }

        [Display(Name = "Valor")]
        public string VALLIQ { get; set; }

        [Display(Name = "Codigo da Empresa")]
        public int CODEMPRESA { get; set; }

        public bool TITVENCIDO { get; set; }

        internal static IList<ConstulaPagNetFechCredVM> ToViewListaTitulosBordero<T>(ICollection<T> collection) where T : PAGNET_EMISSAO_TITULOS
        {
            return collection.Select(x => ToViewListaTitulosBordero(x)).ToList();
        }

        internal static ConstulaPagNetFechCredVM ToViewListaTitulosBordero(PAGNET_EMISSAO_TITULOS x)
        {
            return new ConstulaPagNetFechCredVM
            {
                Codigo = x.CODTITULO,
                STATUS = x.STATUS.Replace("_", " ").Trim(),
                CODBORDERO = Convert.ToInt32(x.CODBORDERO),
                CODFAVORECIDO = x.CODFAVORECIDO ?? 0,
                NMFAVORECIDO = (x.CODFAVORECIDO != null) ? x.CODFAVORECIDO + "-" + Geral.FormataTexto(x.PAGNET_CADFAVORECIDO.NMFAVORECIDO, 35) : "BOLETO BANCARIO",
                CNPJ = (x.CODFAVORECIDO != null) ? Geral.FormataCPFCnPj(x.PAGNET_CADFAVORECIDO.CPFCNPJ) : "",
                dtBordero = x.PAGNET_BORDERO_PAGAMENTO.DTBORDERO.ToShortDateString(),
                dtAgendamento = x.DATREALPGTO.ToShortDateString(),
                CODBANCO = (x.CODFAVORECIDO != null) ? x.PAGNET_CADFAVORECIDO.BANCO : "",
                AGENCIA = (x.CODFAVORECIDO != null) ? x.PAGNET_CADFAVORECIDO.AGENCIA + "-" + x.PAGNET_CADFAVORECIDO.DVAGENCIA : "",
                CONTACORRENTE = (x.CODFAVORECIDO != null) ? x.PAGNET_CADFAVORECIDO.CONTACORRENTE + "-" +x.PAGNET_CADFAVORECIDO.DVCONTACORRENTE : "",
                VALLIQ = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (x.VALTOTAL)),
                TITVENCIDO = (x.DATREALPGTO < DateTime.Today)

            };
        }


        internal static IList<ConstulaPagNetFechCredVM> ToViewPagNetPagamento<T>(ICollection<T> collection, string dtEmissao) where T : PAGNET_TITULOS_PAGOS
        {
            return collection.Select(x => ToViewPagNetPagamento(x, dtEmissao)).ToList();
        }

        internal static ConstulaPagNetFechCredVM ToViewPagNetPagamento(PAGNET_TITULOS_PAGOS x, string dtEmissao)
        {
            return new ConstulaPagNetFechCredVM
            {

                Codigo = x.CODTITULOPAGO,
                STATUS = x.STATUS.Replace("_", " ").Trim(),
                CODBORDERO = x.CODBORDERO,
                CODFAVORECIDO = x.CODFAVORECIDO ?? 0,
                NMFAVORECIDO = x.PAGNET_CADFAVORECIDO.NMFAVORECIDO,
                CNPJ = "",
                dtBordero = dtEmissao,
                dtAgendamento = x.DTREALPAGAMENTO.ToShortDateString(),
                CODBANCO = x.PAGNET_CADFAVORECIDO.BANCO.ToString(),
                AGENCIA = x.PAGNET_CADFAVORECIDO.AGENCIA.ToString() + "-" + x.PAGNET_CADFAVORECIDO.DVAGENCIA.ToString(),
                CONTACORRENTE = x.PAGNET_CADFAVORECIDO.CONTACORRENTE.ToString() + "-" + x.PAGNET_CADFAVORECIDO.DVCONTACORRENTE.ToString(),
                VALLIQ = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALOR),
                CODEMPRESA = x.CODEMPRESA

            };
        }

    }
    public class EmissaoTituloAvulsto
    {
        public bool acessoAdmin { get; set; }
        public int codUsuario { get; set; }               
        public int CODTITULO { get; set; }
        public int CODTITULOPAI { get; set; }
        public bool PossuiNetCard { get; set; }

        [Display(Name = "Empresa")]
        public string CODEMPRESA { get; set; }
        public string NMEMPRESA { get; set; }

        [Display(Name = "Plano de Contas")]
        public string CodigoPlanoContas { get; set; }
        public string NomePlanoContas { get; set; }

        [Display(Name = "Favorecido")]
        public string NMFORNECEDOR { get; set; }

        [Display(Name = "Filtro")]
        [Ajuda("Informar o código do Favorecido/Centralizadora ou o CNPJ")]
        public string CODFORNECEDOR { get; set; }

        [Display(Name = "")]
        public string LINHADIGITAVEL { get; set; }

        [Display(Name = "")]
        public string LINHADIGITAVELCOBRANCA1 { get; set; }
        [Display(Name = "")]
        public string LINHADIGITAVELCOBRANCA2 { get; set; }
        [Display(Name = "")]
        public string LINHADIGITAVELCOBRANCA3 { get; set; }
        [Display(Name = "")]
        public string LINHADIGITAVELCOBRANCA4 { get; set; }
        [Display(Name = "")]
        public string LINHADIGITAVELCOBRANCA5 { get; set; }
        [Display(Name = "")]
        public string LINHADIGITAVELCOBRANCA6 { get; set; }
        [Display(Name = "")]
        public string LINHADIGITAVELCOBRANCA7 { get; set; }
        [Display(Name = "")]
        public string LINHADIGITAVELCOBRANCA8 { get; set; }

        [Display(Name = "Banco")]
        public string BANCO { get; set; }

        [Display(Name = "Vencimento")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string DATVENCIMENTOBOLETO { get; set; }
        
        [Display(Name = "Valor a Pagar")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string VALORBOLETO { get; set; }

        [Display(Name = "Valor a Pagar")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string VALOR { get; set; }

        [Display(Name = "Data Real de Pagamento")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string DATREALPGTOBOLETO { get; set; }

        [Display(Name = "Data Real de Pagamento")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string DATREALPGTO { get; set; }

        [Display(Name = "Tipo de Título")]
        public string TIPOTITULO { get; set; }
        public string NMTIPOTITULO { get; set; }

        [Display(Name = "Tipo de Boleto")]
        public string TIPOBOLETO { get; set; }

    }

    public class FiltroAntecipacaoPGTOVm
    {
        public bool acessoAdmin { get; set; }
        public int codUsuario { get; set; }

        [Display(Name = "Empresa")]
        public string codigoEmpresa { get; set; }
        public string nomeEmpresa { get; set; }

        [Display(Name = "Filtro")]
        [Ajuda("Informar o código do Favorecido/Centralizadora ou o CNPJ")]
        public string codigoFavorecido { get; set; }

        [Display(Name = "Favorecido *")]
        public string nomeFavorecido { get; set; }

        [Display(Name = "Nova Data de Pagamento *")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string dtAntecipacao { get; set; }

        [Display(Name = "Título em Aberto *")]
        public string codigoTitulo { get; set; }
        public string DescricaoTitulo { get; set; }
    }
    public class AntecipacaoPGTOVm
    {
        public AntecipacaoPGTOVm()
        {
            ListaTitulos = new List<ListaAntecipacaoPGTOVm>();
        }
        public int codUsuario { get; set; }
        public int CodigoTituloAntecipacao { get; set; }
        public int codigoEmpresa { get; set; }
        public string ValorAtualizado { get; set; }
        public string ValorOriginal { get; set; }

        [Display(Name = "Lista de Títulos")]
        public IList<ListaAntecipacaoPGTOVm> ListaTitulos { get; set; }

        [Display(Name = "Nova Data de PGTO")]
        [Ajuda("Nova data que será realizado o pagamento")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string NovaDataPGTO { get; set; }

        [Display(Name = "Taxa de Antecipação")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string TaxaAntecipacao { get; set; } 

        [Display(Name = "Justificativa")]
        [Ajuda("Informe o motivo")]
        public string codJustificativa { get; set; }
        public string descJustificativa { get; set; }

        [Display(Name = "Especifique")]
        [Ajuda("Especifique o motivo da ação.")]
        [StringLength(200)]
        public string DescJustOutros { get; set; }

        internal static AntecipacaoPGTOVm ToView(List<PAGNET_EMISSAO_TITULOS> Lista, string taxaCobrada)
        {
            return new AntecipacaoPGTOVm
            {
                TaxaAntecipacao = taxaCobrada,
                ListaTitulos = ListaAntecipacaoPGTOVm.ToViewListaTitulos(Lista, taxaCobrada)

            };
        }

    }
    public class ListaAntecipacaoPGTOVm
    {

        [Display(Name = "Código")]
        public int Codigo { get; set; }
        
        [Display(Name = "Código")]
        public int CODFAVORECIDO { get; set; }

        [Display(Name = "Favorecido")]
        public string NMFAVORECIDO { get; set; }

        [Display(Name = "CNPJ")]
        public string CNPJ { get; set; }

        [Display(Name = "Data Emissao")]
        public string DTEMISSAO { get; set; }

        [Display(Name = "Data Real PGTO")]
        public string DTREALPGTO { get; set; }

        [Display(Name = "Banco")]
        public string CODBANCO { get; set; }

        [Display(Name = "Agência")]
        public string AGENCIA { get; set; }

        [Display(Name = "Conta")]
        public string CONTACORRENTE { get; set; }

        [Display(Name = "Valor")]
        public string VALATUAL { get; set; }

        [Display(Name = "Taxa Cobrada")]
        public string VALTAXA { get; set; }

        [Display(Name = "Valor Taxa")]
        public string ValorTaxaProRata { get; set; }

        [Display(Name = "Valor Total")]
        public string VALTOTAL { get; set; }

        [Display(Name = "Tipo de Cartão")]
        public string TIPOCARTAO { get; set; }

        [Display(Name = "Codigo da Empresa")]
        public int CODEMPRESA { get; set; }


        internal static IList<ListaAntecipacaoPGTOVm> ToViewListaTitulos<T>(ICollection<T> collection, string ValTaxa) where T : PAGNET_EMISSAO_TITULOS
        {
            return collection.Select(x => ToViewListaTitulos(x, ValTaxa)).ToList();
        }

        internal static ListaAntecipacaoPGTOVm ToViewListaTitulos(PAGNET_EMISSAO_TITULOS x, string ValTaxa)
        {
            return new ListaAntecipacaoPGTOVm
            {
                Codigo = x.CODTITULO,
                CODEMPRESA = x.CODEMPRESA,
                CODFAVORECIDO = x.CODFAVORECIDO ?? 0,
                NMFAVORECIDO = Convert.ToString(x.CODFAVORECIDO) + "-" + Geral.FormataTexto(x.PAGNET_CADFAVORECIDO.NMFAVORECIDO, 35),
                CNPJ = Geral.FormataCPFCnPj(x.PAGNET_CADFAVORECIDO.CPFCNPJ),
                DTEMISSAO = x.DATEMISSAO.ToShortDateString(),
                DTREALPGTO = x.DATREALPGTO.ToShortDateString(),
                CODBANCO = x.PAGNET_CADFAVORECIDO.BANCO,
                AGENCIA = x.PAGNET_CADFAVORECIDO.AGENCIA + "-" + x.PAGNET_CADFAVORECIDO.DVAGENCIA,
                CONTACORRENTE = x.PAGNET_CADFAVORECIDO.CONTACORRENTE + "-" + x.PAGNET_CADFAVORECIDO.DVCONTACORRENTE,
                TIPOCARTAO = (x.SISTEMA == 0) ? "Pós Pag" : "Pré Pago",
                VALATUAL = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (x.VALTOTAL)),
                VALTAXA = ValTaxa,
                VALTOTAL = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", 0)
            };
        }

    }
    public class TaxasCobradasPGTOVm
    {

        [Display(Name = "Código da Taxa")]
        public int CodigoTaxa { get; set; }

        [Display(Name = "Código do Título")]
        public int CodigoTitulo { get; set; }

        [Display(Name = "Descrição")]
        public string Descrição { get; set; }

        [Display(Name = "Valor")]
        public string Valor { get; set; }

        [Display(Name = "Data Inclusão")]
        public string DataInclusao { get; set; }

        [Display(Name = "Usuário")]
        public string nmUsuario { get; set; }

        [Display(Name = "Valor Total")]
        public string ValorTotal { get; set; }
        public decimal ValorTotal_aux { get; set; }


        internal static IList<TaxasCobradasPGTOVm> ToListView<T>(ICollection<T> collection) where T : PAGNET_TAXAS_TITULOS
        {
            return collection.Select(x => ToListView(x)).ToList();
        }

        internal static TaxasCobradasPGTOVm ToListView(PAGNET_TAXAS_TITULOS x)
        {
            return new TaxasCobradasPGTOVm
            {
                CodigoTaxa = x.CODTAXATITULO,
                CodigoTitulo = x.CODTITULO,
                Descrição = x.DESCRICAO,
                ValorTotal_aux = x.VALOR,
                Valor = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Math.Abs(x.VALOR)),
                DataInclusao = x.DTINCLUSAO.ToShortDateString(),
                nmUsuario = x.USUARIO_NETCARD.NMUSUARIO
            };
        }

    }
    public class DadosExtratoBancarioVm
    {
        public string dataSolicitacao { get; set; }
        public string dataEfetivacao { get; set; }
        public string tipoLancamento { get; set; }
        public string valorLancamento { get; set; }
        public string categoriaLancamento { get; set; }
        public string codgioLancamentoBanco { get; set; }
        public string descricaoBanco { get; set; }
    }
    public class ListaTitulosInclusaoMassa
    {
        public int codEmpresa { get; set; }
        public int codUsuario { get; set; }
        public string codFavorecidoNC { get; set; }
        public string CPFUsuarioNC { get; set; }
        public string NomeUsuarioNC { get; set; }
        public string DataPGTO { get; set; }
        public string vlLiquido { get; set; }
        public string StatusProcessamento { get; set; }
        public string LogProcessamento { get; set; }

    }
}
