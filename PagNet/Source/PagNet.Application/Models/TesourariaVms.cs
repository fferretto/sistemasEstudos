using PagNet.Application.Helpers;
using PagNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace PagNet.Application.Models
{
    public class IncluiTransacaoVM
    {

        public int codigoUsuarioIncTransacao { get; set; }
        public bool acessoAdminIncTransacao { get; set; }
        public bool RepetirIncTransacao { get; set; }
        public string TipoIncTransacao { get; set; }

        [Display(Name = "Parcela Inicial")]
        [InputAttrAux(ValorMaximo = "99", ValorMinimo ="1", Type = "number")]
        public int ParcelaInicioIncTransacao { get; set; }

        [Display(Name = "Total de Parcelas")]
        [InputAttrAux(ValorMaximo = "99", ValorMinimo = "1", Type = "number")]
        public int ParcelaTerminoIncTransacao { get; set; }

        [Display(Name = "Totalizando")]
        [InputAttrAux(Inicio = "R$")]
        public string ValorTotalIncTransacao { get; set; }
        
        [Required(ErrorMessage = "Obrigatório informar a conta corrente")]
        [Display(Name = "Conta Corrente")]
        [Ajuda("Conta que será debitado o valor")]
        public string codContaCorrenteIncTransacao { get; set; }
        public string nmContaCorrenteIncTransacao { get; set; }

        [Display(Name = "Valor da Transação")]
        [Required(ErrorMessage = "obrigatório informar o valor da transação")]
        [Ajuda("Informe o valor da transação")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(10)]
        public string ValorIncTransacao { get; set; }
        
        [Display(Name = "Data da Transacao")]
        [Required(ErrorMessage = "Obrigatório informar a data da transação.")]
        [Ajuda("Data que será descontado o valor")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string dtIncTransacao { get; set; }

        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "Obrigatório informar a data da transação.")]
        public string codigoEmpresaIncTransacao { get; set; }
        public string nomeEmpresaIncTransacao { get; set; }

        [Display(Name = "Descrição da Transação")]
        [Required(ErrorMessage = "Obrigatório informar a descrição da transação.")]
        [StringLength(50)]
        public string DescricaoIncTransacao { get; set; }

    }
    public class EditarTransacaoVM
    {
        public int codigoTransacao { get; set; }
        public int codigoUsuario { get; set; }
        public bool acessoAdmin { get; set; }
        public bool RepetirTransacao { get; set; }
        public string TipoTransacao { get; set; }

        [Display(Name = "Parcela Inicial")]
        [InputAttrAux(ValorMaximo = "99", ValorMinimo = "1", Type = "number")]
        public int ParcelaInicio { get; set; }

        [Display(Name = "Total de Parcelas")]
        [InputAttrAux(ValorMaximo = "99", ValorMinimo = "1", Type = "number")]
        public int ParcelaTermino { get; set; }

        [Display(Name = "Totalizando")]
        [InputAttrAux(Inicio = "R$")]
        public string ValorTotal { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a conta corrente")]
        [Display(Name = "Conta Corrente")]
        [Ajuda("Conta que será debitado o valor")]
        public string codContaCorrenteTransacao { get; set; }
        public string nmContaCorrenteTransacao { get; set; }

        [Display(Name = "Valor da Transação")]
        [Required(ErrorMessage = "obrigatório informar o valor da transação")]
        [Ajuda("Informe o valor da transação")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(10)]
        public string ValorTransacao { get; set; }

        [Display(Name = "Data da Transacao")]
        [Required(ErrorMessage = "Obrigatório informar a data da transação.")]
        [Ajuda("Data que será descontado o valor")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string dtTransacao { get; set; }

        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "Obrigatório informar a data da transação.")]
        public string codigoEmpresaTransacao { get; set; }
        public string nomeEmpresaTransacao { get; set; }

        [Display(Name = "Descrição da Transação")]
        [Required(ErrorMessage = "Obrigatório informar a descrição da transação.")]
        [StringLength(50)]
        public string DescricaoTransacao { get; set; }

        internal static EditarTransacaoVM ToViewReceita(PAGNET_EMISSAOFATURAMENTO item)
        {
            return new EditarTransacaoVM
            {
                codigoTransacao = item.CODEMISSAOFATURAMENTO,
                RepetirTransacao = false,
                TipoTransacao = "ENTRADA",
                ParcelaInicio = item.PARCELA,
                ParcelaTermino = item.TOTALPARCELA,
                ValorTotal = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", item.VALOR).Replace("R$ ", ""),
                codContaCorrenteTransacao = item.CODCONTACORRENTE.ToString(),
                nmContaCorrenteTransacao = "",
                ValorTransacao = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", item.VALORPARCELA).Replace("R$ ", ""),
                dtTransacao = item.DATVENCIMENTO.ToShortDateString(),
                codigoEmpresaTransacao = item.CODEMPRESA.ToString(),
                nomeEmpresaTransacao = item.PAGNET_CADEMPRESA.NMFANTASIA,
                DescricaoTransacao = item.TIPOFATURAMENTO
            };
        }

        internal static EditarTransacaoVM ToViewDespesa(PAGNET_EMISSAO_TITULOS item)
        {
            return new EditarTransacaoVM
            {
                codigoTransacao = item.CODTITULO,
                RepetirTransacao = false,
                TipoTransacao = "SAÍDA",
                ParcelaInicio = 1,
                ParcelaTermino = 1,
                ValorTotal = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", item.VALTOTAL).Replace("R$ ", ""),
                codContaCorrenteTransacao = item.CODCONTACORRENTE.ToString(),
                nmContaCorrenteTransacao = "",
                ValorTransacao = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", item.VALTOTAL).Replace("R$ ", ""),
                dtTransacao = item.DATREALPGTO.ToShortDateString(),
                codigoEmpresaTransacao = item.CODEMPRESA.ToString(),
                nomeEmpresaTransacao = item.PAGNET_CADEMPRESA.NMFANTASIA,
                DescricaoTransacao = item.TIPOTITULO
            };
        }
    }

    public class FiltroTransacaoVM
    {
        public int codigoUsuario { get; set; }
        public bool acessoAdmin { get; set; }
        public bool TransacoesFuturas { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a conta corrente")]
        [Display(Name = "Conta Corrente")]
        [Ajuda("Conta que será debitado o valor")]
        public string codContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }

        [Display(Name = "Empresa ")]
        public string codigoEmpresa { get; set; }
        public string nomeEmpresa { get; set; }

        [Display(Name = "Mês Referência")]
        [Ajuda("Data de pagamento")]
        [InputMask("99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string MesRef { get; set; }

        public IList<ListaTransacoesAConsolidarVm> ListaTransacao { get; set; }

    }
    public class ListaTransacoesAConsolidarVm
    {
        public bool itemChecado { get; set; }
        public int codigo { get; set; }
        public string Descricao { get; set; }
        public string dataSolicitacao { get; set; }
        public string dataPGTO { get; set; }
        public string ValorTransacao { get; set; }
        public string TipoTransacao { get; set; }

    }

    public class FiltroExtratoBancarioVm
    {
        public bool acessoAdmin { get; set; }

        [Required(ErrorMessage = "Obrigatório informar a conta corrente")]
        [Display(Name = "Conta Corrente")]
        [Ajuda("Conta que será debitado o valor")]
        public string codContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }

        [Display(Name = "Empresa ")]
        public string codigoEmpresa { get; set; }
        public string nomeEmpresa { get; set; }
        
        [Display(Name = "Data Inicio")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string dtInicio { get; set; }

        [Display(Name = "Data Fim")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string dtFim { get; set; }

    }
    public class TesourariaExtratoBancarioVM
    {
        public TesourariaExtratoBancarioVM(PROC_PAGNET_EXTRATO_BANCARIO x)
        {
            if (x != null)
            {

                DescExtratoBancario = x.DESCRICAO;
                DataExtratoBancario = x.DATA.ToShortDateString();
                tipoTransacao = x.TIPOTRANSACAO;
                ValorExtratoBancarioReceita = (x.TIPOTRANSACAO == "ENTRADA") ? string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALOR).Replace("R$ ", "") : "";
                ValorExtratoBancarioDespesa = (x.TIPOTRANSACAO == "SAIDA") ? string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (-1 * x.VALOR)).Replace("R$ ", "") : "";
                TipoTransacaoExtratoBancario = x.TIPOTRANSACAO;
                ValorTotalEntrada = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.TOTALENTRADA).Replace("R$ ", "");
                ValorTotalSaida = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", -1 * x.TOTALSAIDA).Replace("R$ ", "");
                SaldoInicial = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.SALDOANTERIOR).Replace("R$ ", "");
                SaldoFinalPeriodo = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.SALDOFINAL).Replace("R$ ", "");
                SaldoAnterior = x.SALDOANTERIOR;
                ValorTransacao = (x.TIPOTRANSACAO == "SAIDA") ? -1 * x.VALOR : x.VALOR;

            }
        }

        public string DescExtratoBancario { get; set; }
        public string DataExtratoBancario { get; set; }
        public string ValorExtratoBancarioReceita { get; set; }
        public string ValorExtratoBancarioDespesa { get; set; }
        public string TipoTransacaoExtratoBancario { get; set; }

        public string ValorTotalEntrada { get; set; }
        public string ValorTotalSaida { get; set; }
        public string SaldoInicial { get; set; }
        public string Saldo { get; set; }
        public string SaldoFinalPeriodo { get; set; }
        public string tipoTransacao { get; set; }


        public decimal SaldoAnterior { get; set; }
        public decimal ValorTransacao { get; set; }

    }
    public class TesourariaInformacaoCCVM
    {

        [Display(Name = "Saldo Atual da Conta Corrente")]
        public string SaldoAtualContaCorrente { get; set; }

        [Display(Name = "Total Recebido no Período")]
        public string TotalReceitaPeriodo { get; set; }

        [Display(Name = "Total Pago no Período")]
        public string TotalDespesaPeriodo { get; set; }

        [Display(Name = "Saldo Anterior")]
        public string SaldoAnteriorContaCorrente { get; set; }
        

        internal static TesourariaInformacaoCCVM ToView(PROC_PAGNET_INFO_CONTA_CORRENTE x)
        {
            return new TesourariaInformacaoCCVM
            {
                SaldoAtualContaCorrente = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.SALDOCONTA),
                TotalReceitaPeriodo = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.TOTALRECEITA),
                TotalDespesaPeriodo = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.TOTALDESPESA),
                SaldoAnteriorContaCorrente = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.SALDOANTERIOR)
            };
        }
    }
    public class TesourariaMaioresDespesasVM
    {
        public TesourariaMaioresDespesasVM(PROC_PAGNET_MAIORES_DESPESAS x)
        {
            if (x != null)
            {
                NomeDespesa = (x.NOME.Length > 25) ? x.NOME.Substring(0, 25) : x.NOME;
                ValorDespesa = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (-1 * x.VALOR));
                OrigemDespesa = x.ORIGEM;
            }
        }
        
        public string NomeDespesa { get; set; }
        public string OrigemDespesa { get; set; }
        public string ValorDespesa { get; set; }
    }
    public class TesourariaMaioresReceitasVM
    {
        public TesourariaMaioresReceitasVM(PROC_PAGNET_MAIORES_RECEITAS x)
        {
            if (x != null)
            {
                NomeReceitas = (x.NOME.Length > 25) ? x.NOME.Substring(0,25) : x.NOME;
                ValorReceitas = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALOR);
                OrigemReceitas = x.ORIGEM;
            }
        }

        public string NomeReceitas { get; set; }
        public string OrigemReceitas { get; set; }
        public string ValorReceitas { get; set; }
    }
    public class FiltroConciliacaoBancariaVm
    {
        public bool acessoAdmin { get; set; }
        public int codigoUsuario { get; set; }
        public string caminhoArquivo { get; set; }

        [Display(Name = "Conta Corrente")]
        [Ajuda("Conta que será realizada a conciliação")]
        public string codContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }

        [Display(Name = "Empresa ")]
        public string codigoEmpresa { get; set; }
        public string nomeEmpresa { get; set; }

    }
    public class DadosArquivoOFXVM
    {
        public decimal ValorOFX { get; set; }
        public DateTime DataOFX { get; set; }
        public string DescricaoOFX { get; set; }
        public string TipoOFX { get; set; }
        public string CodigoTransacaoOFX { get; set; }
    }
    public class DadosArquivoConciliacaoVM
    {
        public decimal ValorConciliacao { get; set; }
        public DateTime DataConciliacao { get; set; }
        public string DescricaoConciliacao { get; set; }
        public string TipoConciliacao { get; set; }
        public string CodigoTransacaoConciliacao { get; set; }
        public bool TransacaoEncontrada { get; set; }
        public decimal ValorTransacao { get; set; }
    }
    public class ListaConciliacaoVM
    {
        public string DataConciliacao { get; set; }
        public string DescricaoConciliacao { get; set; }
        public string DescricaoAbreviadaConciliacao { get; set; }
        public string TipoConciliacao { get; set; }
        public string ValorTransacao { get; set; }
        public string TransacaoEncontrada { get; set; }
        public string CodigoTransacaoPN { get; set; }
        public string ValorPN { get; set; }
        public string StatusPN { get; set; }

        public string TotalCredito { get; set; }
        public string TotalDebito { get; set; }
        public string SaldoAnterior { get; set; }
        public string SaldoFinal { get; set; }
        public int TotalRegistroConciliados { get; set; }
        public int TotalRegistrosPendentes { get; set; }
        public int TotalRegistrosNaoEncontrados { get; set; }
        public int TotalRegistros { get; set; }
    }
    public class ItemConciliacaoVM
    {
        public int codigoUsuario { get; set; }
        public string codContaCorrente { get; set; }
        public string codigoEmpresa { get; set; }
        public string DataConciliacao { get; set; }
        public string DescricaoConciliacao { get; set; }
        public string TipoConciliacao { get; set; }
        public string ValorTransacao { get; set; }
        public string TransacaoEncontrada { get; set; }
        public string CodigoTransacaoPN { get; set; }
        public string ValorPN { get; set; }
        public string StatusPN { get; set; }
    }
}
