using PagNet.Application.Helpers;
using PagNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace PagNet.Application.Models
{
    public class ConsultaClienteVM
    {
        public ConsultaClienteVM(PAGNET_CADCLIENTE x)
        {
            codCli = x.CODCLIENTE;
            nmCliente = x.NMCLIENTE.Trim();
            CNPJ = Geral.FormataCPFCnPj(x.CPFCNPJ.Trim());
        }

        public int codCli { get; set; }

        [Display(Name = "Cliente")]
        public string nmCliente { get; set; }

        [Display(Name = "CPF/CNPJ")]
        public string CNPJ { get; set; }
    }
    public class BorderoReceitaVm
    {
        public BorderoReceitaVm()
        {
            ListaFechamento = new List<ListaFechClienteVM>();
        }

        [Display(Name = "Fechamentos de Clientes")]
        public IList<ListaFechClienteVM> ListaFechamento { get; set; }

        [Display(Name = "Nome do Borderô")]
        [Ajuda("Este nome irá facilitar na identificação na hora de gerar o arquivo de remessa.")]
        [StringLength(80)]
        public string nmBordero { get; set; }
        
        [Required(ErrorMessage = "Obrigatório informar a conta corrente")]
        [Display(Name = "Conta Corrente")]
        [Ajuda("Conta que será debitado o valor")]
        public string codContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }

        [Display(Name = "Primeira Instrução Cobrança")]
        [Ajuda("Instrução para o caixa")]
        public string codPrimeiraInstrucaoCobranca { get; set; }
        public string nmPrimeiraInstrucaoCobranca { get; set; }

        [Display(Name = "Segunda Instrução Cobrança")]
        [Ajuda("Instrução para o caixa")]
        public string codSegundaInstrucaoCobranca { get; set; }
        public string nmSegundaInstrucaoCobranca { get; set; }
        [Display(Name = "Valor da Multa")]
        [Ajuda("Valor da multa a ser pago")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(13)]
        public string vlMulta { get; set; }

        [Display(Name = "Valor da Multa")]
        [Ajuda("Porcentagem da multa a ser pago")]
        [InputMask("##0,00%", IsReverso = true)]
        [InputAttrAux(Final = "%")]
        [StringLength(5)]
        public string PercMulta { get; set; }

        [Display(Name = "Valor de Mora")]
        [Ajuda("Porcentagem de juros a ser pago por dia de atraso")]
        [InputMask("##0,00%", IsReverso = true)]
        [InputAttrAux(Final = "%")]
        [StringLength(5)]
        public string PercJuros { get; set; }

        [Display(Name = "Valor de Mora")]
        [Ajuda("Valor de juros a ser pago por dia de atraso")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(13)]
        public string vlJuros { get; set; }

        [Display(Name = "Data de Vencimento")]
        [Ajuda("Data limite para pagamento")]
        [InputMask("99/99/9999")]
        public string dtVencimento { get; set; }

        public string dtInicioGrid { get; set; }
        public string dtFimGrid { get; set; }
        public string MensagemArquivoRemessa { get; set; }
        public string MensagemInstrucoesCaixa { get; set; }
        public string CaminhoArquivoRemessa { get; set; }
        public string CaminhoArquivoBoleto { get; set; }
        public int codUsuario { get; set; }
        public int codArquivo { get; set; }
        public string codigoEmpresa { get; set; }
        public int codOpe { get; set; }
        public bool ClientePre { get; set; }
        public bool CobraMulta { get; set; }
        public bool CobraJuros { get; set; }

        //internal static BorderoReceitaVm ToViewPosPago(List<PAGNET_CADCLIENTE> Cliente, FechamentoClienteVms md)
        //{
        //    return new BorderoReceitaVm
        //    {
        //        dtVencimento = DateTime.Now.ToShortDateString(),
        //        dtFimGrid = md.dtFim,
        //        dtInicioGrid = md.dtInicio,
        //        codigoEmpresa = md.CodEmpresa,
        //        ListaFechamento = ListaFechClienteVM.ToViewPos(Cliente)

        //    };
        //}
        internal static BorderoReceitaVm ToViewCarga(List<PROC_PAGNET_CONSCARGA_PGTO> CARGAC, DateTime dtIni, DateTime dtFinal)
        {
            return new BorderoReceitaVm
            {
                dtVencimento = DateTime.Now.ToShortDateString(),
                dtFimGrid = dtFinal.ToShortDateString(),
                dtInicioGrid = dtIni.ToShortDateString(),
                ListaFechamento = ListaFechClienteVM.ToViewCarga(CARGAC)

            };
        }        
    }

    public class ListaFechClienteVM
    {

        internal static IList<ListaFechClienteVM> ToViewPos<T>(ICollection<T> collection) where T : PAGNET_CADCLIENTE
        {
            return collection.Select(x => ToViewPos(x)).ToList();
        }
        internal static IList<ListaFechClienteVM> ToViewCarga<T>(ICollection<T> collection) where T : PROC_PAGNET_CONSCARGA_PGTO
        {
            return collection.Select(x => ToViewCarga(x)).ToList();
        }

        internal static ListaFechClienteVM ToViewPos(PAGNET_CADCLIENTE x)
        {
            return new ListaFechClienteVM
            {
                chkFechCred = true,
                //NUMFECCLI = x.NUMFECCLI,
                //CODCLI = x.CODCLI,
                //CGC = Geral.FormataCPFCnPj(x.VCLIENTE.CGC),
                //NOMCLI = x.CODCLI + "-" + Geral.FormataTexto(x.VCLIENTE.NOMCLI, 34).Trim(),
                //DATPGTO = x.DATPGTO.ToShortDateString(),
                //ANUIDADE = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (x.ANUIDADE ?? 0)),
                //VAL2VIA = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (x.VAL2VIA ?? 0)),
                //COMPRAS = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (x.COMPRAS ?? 0)),
                //TOTAL = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.TOTAL)
            };
        }

        internal static ListaFechClienteVM ToViewCarga(PROC_PAGNET_CONSCARGA_PGTO x)
        {
            return new ListaFechClienteVM
            {
                chkFechCred = true,
                NUMFECCLI = x.NUMCARGA,
                CODCLI = x.CODCLI,
                CGC = Geral.FormataCPFCnPj(x.CNPJ),
                NOMCLI = x.CODCLI + "-" + Geral.FormataTexto(x.NMCLIENTE, 34).Trim(),
                DATPGTO = x.DTAUTORIZ.ToShortDateString(),
                VAL2VIA = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (x.VAL2AVIA)),
                VALTAXA = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (x.VALTAXA)),
                VALTAXACARREG = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (x.VALTAXACARREG)),
                VALCARGA = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (x.VALCARGA)),
                SALDOCONTAUTIL = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (x.SALDOCONTAUTIL)),
                TOTAL = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (x.TOTAL)),
                DESCRICAOOCORRENCIARETBOL = x.DESCRICAOOCORRENCIARETBOL.Trim()
            };
        }

        public bool chkFechCred { get; set; }

        public string VALCARGA { get; set; }
        public string VALTAXA { get; set; }
        public string VALTAXACARREG { get; set; }
        public string SALDOCONTAUTIL { get; set; }
        public string DESCRICAOOCORRENCIARETBOL { get; set; }

        public int NUMFECCLI { get; set; }
        public int CODCLI { get; set; }

        [Display(Name = "CNPJ")]
        public string CGC { get; set; }

        [Display(Name = "Cliente")]
        public string NOMCLI { get; set; }

        [Display(Name = "Data de Pagamento")]
        public string DATPGTO { get; set; }

        [Display(Name = "Anuidade")]
        public string ANUIDADE { get; set; }

        [Display(Name = "Valor 2ª via")]
        public string VAL2VIA { get; set; }

        [Display(Name = "Valor Gasto")]
        public string COMPRAS { get; set; }

        [Display(Name = "Total")]
        public string TOTAL { get; set; }

        public string LogradouroEndereco { get; set; }
        public string LogradouroNumero { get; set; }
        public string LogradouroComplemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string CEP { get; set; }

        public string nmBoleto { get; set; }
        public int Cont { get; set; }
        public string numControle { get; set; }

    }
}
