using PagNet.Application.Helpers;
using PagNet.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;

namespace PagNet.Application.Models
{
    public class EmissaoBoletoVM
    {
        public int codigoEmissaoBoleto { get; set; }
        public bool acessoAdmin { get; set; }
        public int codigoUsuario { get; set; }
        public string nomeBoleto { get; set; }
        public int codBordero { get; set; }
        public string SeuNumero { get; set; }
        public string valorPrevistoRecebimento { get; set; }

        [Display(Name = "Código")]
        public string CodigoCliente { get; set; }

        [Display(Name = "Código")]
        public string CodigoClienteConsulta { get; set; }


        [Display(Name = "Data de Vencimento")]
        [Ajuda("Data de Vencimento do Boleto")]
        [Required(ErrorMessage = "Obrigatório informar a data de vencimento")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string dataVencimento { get; set; }

        [Display(Name = "Data da Solicitação")]
        [Ajuda("Data da Solicitação do Boleto")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string dataSolicitacao { get; set; }

        [Display(Name = "Data de Referência")]
        [Ajuda("Data de Referência do Boleto")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string dataReferencia { get; set; }

        //-----Aplica Desconto
        public bool ConcedeDesconto { get; set; }

        [Display(Name = "Valor do Desconto")]
        [Ajuda("Valor de desconto concedido.")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string valorDesconto { get; set; }

        [Display(Name = "Data do Segundo Desconto")]
        [Ajuda("Data do Segundo Desconto do Boleto")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string dataSegundoDesconto { get; set; }
        

        [Display(Name = "Valor do Segundo Desconto")]
        [Ajuda("Valor de desconto concedido caso realize o pagamento até a data do segundo desconto.")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string valorSegundoDesconto { get; set; }

        //--------FIM DO SEGUNDO DESCONTO

        [Display(Name = "Status")]
        public string Status { get; set; }


        [Display(Name = "Código/CNPJ do Cliente")]
        [Ajuda("Informe o Código ou CPF/CNPJ do cliente")]
        [Required(ErrorMessage = "Obrigatório informar o cliente")]
        [StringLength(20)]
        public string filtroCliente { get; set; }

        [Display(Name = "Nome do Cliente")]
        [Ajuda("Nome do Cliente que irá receber o boleto para pagamento")]
        [StringLength(100)]
        public string nomeCliente { get; set; }

        [Display(Name = "Nome do Cliente")]
        [Ajuda("Nome do Cliente que irá receber o boleto para pagamento")]
        [StringLength(100)]
        public string nomeClienteConsulta { get; set; }

        public string CobrancaDiferenciada { get; set; }
        public string TaxaEmissaoBoleto { get; set; }
        public string EmailCliente { get; set; }

        [Display(Name = "CPF/CNPJ")]
        public string CPFCNPJCliente { get; set; }

        [Display(Name = "Primeira Instrução de Cobrança")]
        public string PrimeiraInstrucaoCobranca { get; set; }

        [Display(Name = "Segunda Instrução de Cobrança")]
        public string SegundaInstrucaoCobranca { get; set; }
        public string CobraMulta { get; set; }

        [Display(Name = "Valor da Multa")]
        public string ValorMulta { get; set; }

        [Display(Name = "Porcentagem da Multa")]
        public string PercentualMulta { get; set; }

        public string CobraJuros { get; set; }

        [Display(Name = "Valor do Juros")]
        public string ValorJuros { get; set; }

        [Display(Name = "Porcentagem do Juros")]
        public string PercentualJuros { get; set; }


        [Display(Name = "Conta Corrente *")]
        public string codigoContaCorrente { get; set; }
        public string nomeContaCorrente { get; set; }

        [Display(Name = "Forma de Faturamento *")]
        [Ajuda("Informe a forma que será realizado o recebimento deste valor")]
        public string codigoFormaFaturamento { get; set; }
        [Display(Name = "Forma de Faturamento ")]
        public string nomeFormaFaturamento { get; set; }


        [Display(Name = "Plano de Contas")]
        public string CodigoPlanoContas { get; set; }
        public string NomePlanoContas { get; set; }

        [Display(Name = "Boleto Emitido Via")]
        [StringLength(60)]
        public string Origem { get; set; }

        [Display(Name = "Empresa")]
        public string codigoEmpresa { get; set; }

        [Display(Name = "Empresa")]
        public string nomeEmpresa { get; set; }

        [Display(Name = "Tipo de Alteração")]
        [Ajuda("Informe o tipo de alteração que está realizando para este boleto")]
        public string codigoOcorrencia { get; set; }
        public string nomeOcorrencia { get; set; }

        //----O número de controle e o Nosso Número - é aconselhavel que o próprio sistema preencha ele
        [Display(Name = "Número de Controle")]
        [Ajuda("Número de controle que o sistema utilizará para localizar o boleto")]
        public int numeroControleTab { get; set; }

        [Display(Name = "Nosso Número")]
        [StringLength(18)]
        public string NossoNumero { get; set; }
        
        [Display(Name = "Código de Rastreio")]
        [Ajuda("Código utilizado para localizar os pedidos de faturamento. Por padrão, os pedidos que vierem do NetCard virá com o código do cliente + número do lote/Carga. ")]
        [StringLength(10)]
        public string nroDocumento { get; set; }

        [Display(Name = "Valor do Faturamento")]
        [Required(ErrorMessage = "Obrigatório informar o valor do Faturamento")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string Valor { get; set; }
        
        [Display(Name = "Mensagem Destinada ao Banco")]
        [StringLength(100)]
        public string MensagemArquivoRemessa { get; set; }
        
        [Display(Name = "Mensagem Destinada ao Caixa")]
        [StringLength(100)]
        public string MensagemInstrucoesCaixa { get; set; }

        [Display(Name = "Justificativa")]
        [Ajuda("Informe o motivo da ação sobre este faturamento")]
        public string codJustificativa { get; set; }
        public string descJustificativa { get; set; }

        [Display(Name = "Especifique")]
        [Ajuda("Especifique o motivo da ação.")]
        [StringLength(200)]
        public string DescJustOutros { get; set; }

        [Display(Name = "Valor Total Recebido")]
        [Ajuda("Total do valor recebido")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string ValorRecebido { get; set; }

        [Display(Name = "Valor do Desconto Concedido")]
        [Ajuda("Valor total do Desconto Concedido")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string ValorDescontoConcedido { get; set; }

        [Display(Name = "Valor do Juros Aplicado")]
        [Ajuda("Juros Total Aplicado")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string JurosCobrado { get; set; }

        [Display(Name = "Valor da Multa Aplicada")]
        [Ajuda("Especifique o motivo da ação.")]
        [InputAttrAux(Inicio = "R$")]
        [InputMask("#.##0,00", IsReverso = true)]
        [StringLength(15)]
        public string MultaCobrada { get; set; }


        internal static EmissaoBoletoVM ToView(PAGNET_EMISSAOFATURAMENTO _bol)
        {
            return new EmissaoBoletoVM
            {
                codigoEmissaoBoleto = _bol.CODEMISSAOFATURAMENTO,
                CodigoCliente = _bol.CODCLIENTE.ToString(),
                CodigoClienteConsulta = _bol.CODCLIENTE.ToString(),
                codBordero = _bol.CODBORDERO ?? 0,
                dataVencimento = _bol.DATVENCIMENTO.ToShortDateString(),
                dataSolicitacao = _bol.DATSOLICITACAO.ToShortDateString(),
                ConcedeDesconto = (_bol.VLDESCONTO > 0),
                valorDesconto = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _bol.VLDESCONTO)).Replace("R$", "").Trim(),
                dataSegundoDesconto = (_bol.DATSEGUNDODESCONTO == null) ? "" : Convert.ToDateTime(_bol.DATSEGUNDODESCONTO).ToShortDateString(),
                valorSegundoDesconto = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _bol.VLSEGUNDODESCONTO)).Replace("R$", "").Trim(),
                Status = _bol.STATUS.Replace("_", " "),
                filtroCliente = _bol.CODCLIENTE.ToString(),
                nomeCliente = _bol.PAGNET_CADCLIENTE.NMCLIENTE,
                nomeClienteConsulta = _bol.PAGNET_CADCLIENTE.NMCLIENTE,
                Origem = _bol.ORIGEM,
                codigoFormaFaturamento = _bol.CODFORMAFATURAMENTO.ToString(),
                codigoEmpresa = _bol.CODEMPRESA.ToString(),
                nomeEmpresa = _bol.PAGNET_CADEMPRESA.NMFANTASIA,
                nroDocumento = Geral.RemoveEspaco(_bol.NRODOCUMENTO),
                Valor = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _bol.VALOR)).Replace("R$", "").Trim(),
                MensagemArquivoRemessa = Convert.ToString(_bol.MENSAGEMARQUIVOREMESSA),
                MensagemInstrucoesCaixa = Convert.ToString(_bol.MENSAGEMINSTRUCOESCAIXA),
                CobrancaDiferenciada= _bol.PAGNET_CADCLIENTE.COBRANCADIFERENCIADA,
                TaxaEmissaoBoleto = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _bol.PAGNET_CADCLIENTE.TAXAEMISSAOBOLETO)),
                EmailCliente = _bol.PAGNET_CADCLIENTE.EMAIL,
                CPFCNPJCliente = Geral.FormataCPFCnPj(_bol.PAGNET_CADCLIENTE.CPFCNPJ),
                PrimeiraInstrucaoCobranca = _bol.PAGNET_CADCLIENTE.CODPRIMEIRAINSTCOBRA.ToString(),
                SegundaInstrucaoCobranca = _bol.PAGNET_CADCLIENTE.CODSEGUNDAINSTCOBRA.ToString(),
                CobraMulta = _bol.PAGNET_CADCLIENTE.COBRAMULTA,
                ValorMulta = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal(_bol.PAGNET_CADCLIENTE.VLMULTADIAATRASO))).Replace("R$", "").Trim(),
                PercentualMulta = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal(_bol.PAGNET_CADCLIENTE.PERCMULTA))).Replace("R$", "").Trim(),
                CobraJuros = _bol.PAGNET_CADCLIENTE.COBRAJUROS,
                ValorJuros = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal(_bol.PAGNET_CADCLIENTE.VLJUROSDIAATRASO))).Replace("R$", "").Trim(),
                PercentualJuros = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal(_bol.PAGNET_CADCLIENTE.PERCJUROS))).Replace("R$", "").Trim(),
                ValorRecebido = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal(_bol.VLPGTO))).Replace("R$", "").Trim(),
                ValorDescontoConcedido = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal(_bol.VLDESCONTOCONCEDIDO))).Replace("R$", "").Trim(),
                JurosCobrado = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal(_bol.JUROSCOBRADO))).Replace("R$", "").Trim(),
                MultaCobrada = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", Convert.ToDecimal(_bol.MULTACOBRADA))).Replace("R$", "").Trim()
            };
        }


        internal static IList<EmissaoBoletoVM> ToList<T>(ICollection<T> collection) where T : PAGNET_EMISSAOBOLETO
        {
            return collection.Select(x => ToList(x)).ToList();
        }

        internal static EmissaoBoletoVM ToList(PAGNET_EMISSAOBOLETO _bol)
        {
            return new EmissaoBoletoVM
            {
                codigoEmissaoBoleto = _bol.codEmissaoBoleto,
                CodigoCliente = _bol.CodCliente.ToString(),
                dataVencimento = _bol.dtVencimento.ToShortDateString(),
                dataSolicitacao = _bol.dtSolicitacao.ToShortDateString(),
                dataReferencia = (_bol.dtReferencia == null) ? "" : Convert.ToDateTime(_bol.dtReferencia).ToShortDateString(),
                ConcedeDesconto = (_bol.vlDesconto > 0),
                valorDesconto = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _bol.vlDesconto)).Replace("R$", ""),
                dataSegundoDesconto = (_bol.dtSegundoDesconto == null) ? "" : Convert.ToDateTime(_bol.dtSegundoDesconto).ToShortDateString(),
                valorSegundoDesconto = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _bol.vlSegundoDesconto)).Replace("R$", ""),
                Status = _bol.Status.Replace("_", " "),
                filtroCliente = _bol.CodCliente.ToString(),
                nomeCliente = _bol.PAGNET_CADCLIENTE.NMCLIENTE,
                codigoEmpresa = _bol.codEmpresa.ToString(),
                nomeEmpresa = _bol.PAGNET_CADEMPRESA.NMFANTASIA,
                numeroControleTab = _bol.codEmissaoBoleto,
                NossoNumero = _bol.NossoNumero,
                SeuNumero = _bol.SeuNumero,
                Valor = (string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", _bol.Valor)).Replace("R$", ""),
                MensagemArquivoRemessa = _bol.MensagemArquivoRemessa,
                MensagemInstrucoesCaixa = _bol.MensagemInstrucoesCaixa,

            };
        }

    }
    public class FiltroConsultaFaturamentoVM
    {
        public bool acessoAdmin { get; set; }
        public bool UtilizaNetCard { get; set; }
        public bool ValidaCliente { get; set; }
        public int CodigoUsuario { get; set; }
        public int CodigoCliente { get; set; }


        [Display(Name = "Código")]
        [Ajuda("Poderá ser utilizado o códgio ou CNPJ do cliente ")]
        public string filtroCliente { get; set; }

        [Display(Name = "Cliente")]
        public string nomeCliente { get; set; }

        [Display(Name = "Empresa")]
        public string codigoEmpresa { get; set; }
        public string nomeEmpresa { get; set; }

        [Display(Name = "Data de Início")]
        [Ajuda("Data de Vencimento do Boleto")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string dtInicio { get; set; }

        [Display(Name = "Data de Térmno")]
        [Ajuda("Data de Vencimento do Boleto")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string dtFim { get; set; }


        [Display(Name = "Código do Borderô")]
        public string codBordero { get; set; }
        
        [Display(Name = "Conta Corrente *")]
        [Ajuda("Conta que irá gerar os boletos")]
        public string codigoContaCorrente { get; set; }
        public string nomeContaCorrente { get; set; }

        [Display(Name = "Apenas Boletos Entregues")]
        public string CodApenasBoletosEntreges { get; set; }
        public string ApenasBoletosEntreges { get; set; }

        [Display(Name = "Status")]
        public string codStatus { get; set; }
        public string nmStatus { get; set; }
        
        [Display(Name = "Fatura")]
        public string codigoFatura { get; set; }
        public string nomeFatura { get; set; }

    }
    public class BorderoBolVM
    {
        public BorderoBolVM()
        {
            ListaBordero = new List<ListaBorderoBolVM>();
        }
        [Display(Name = "Lista dos Borderôs Criados")]
        public IList<ListaBorderoBolVM> ListaBordero { get; set; }

        public int codUsuario { get; set; }
        public int codOpe { get; set; }
        public bool acessoAdmin { get; set; }
        public string CaminhoArquivo { get; set; }
        public string codigoEmpresa { get; set; }
        
        [Display(Name = "Borderôs Selecionados")]
        [Ajuda("Quantidade de borderô que serão incluídos neste arquivo")]
        public string qtBorderosSelecionados { get; set; }

        [Display(Name = "Valor Arquivo")]
        [Ajuda("Valor total a ser incluído neste arquivo")]
        [InputAttrAux(Inicio = "R$")]
        public string ValorTotalArquivo { get; set; }

        [Display(Name = "Data de Vencimento")]
        [Ajuda("Data de vencimento do boleto")]
        [InputMask("99/99/9999")]
        public string dtVencimento { get; set; }


        [Display(Name = "Conta Corrente *")]
        [Ajuda("Conta Corrente que irá emitir o boleto")]
        public string codContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }


        internal static BorderoBolVM ToView(List<PAGNET_BORDERO_BOLETO> Bordero)
        {
            return new BorderoBolVM
            {
                ListaBordero = ListaBorderoBolVM.ToView(Bordero)

            };
        }
    }
    public class ListaBorderoBolVM
    {

        internal static IList<ListaBorderoBolVM> ToView<T>(ICollection<T> collection) where T : PAGNET_BORDERO_BOLETO
        {
            return collection.Select(x => ToView(x)).ToList();
        }

        internal static ListaBorderoBolVM ToView(PAGNET_BORDERO_BOLETO x)
        {
            return new ListaBorderoBolVM
            {
                chkBordero = true,
                CODBORDERO = x.CODBORDERO,
                STATUS = x.STATUS.Replace("_", " "),
                CODUSUARIO = x.CODUSUARIO,
                QUANTFATURAMENTO = x.QUANTFATURAS.ToString(),
                VLBORDERO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VLBORDERO),
                DTBORDERO = (x.DTBORDERO != null) ? Convert.ToDateTime(x.DTBORDERO).ToString() : "",
                CODEMPRESA = x.CODEMPRESA,
                POSSUIBOLETOVENCIDO = "N"

            };
        }

        public bool chkBordero { get; set; }
        public string POSSUIBOLETOVENCIDO { get; set; }

        [Display(Name = "Código do Borderô")]
        public int CODBORDERO { get; set; }

        [Display(Name = "Quant. Faturamentos")]
        public string QUANTFATURAMENTO { get; set; }

        [Display(Name = "Status")]
        public string STATUS { get; set; }

        [Display(Name = "Vencimento")]
        public string DTVENCIMENTO { get; set; }

        [Display(Name = "Código do Usuario")]
        public int CODUSUARIO { get; set; }
        
        [Display(Name = "Valor do Borderô")]
        public string VLBORDERO { get; set; }
        
        [Display(Name = "Data de Inclusão")]
        public string DTBORDERO { get; set; }

        [Display(Name = "Codigo da Empresa")]
        public int CODEMPRESA { get; set; }

    }
    public class ConstulaLogFaturaVM
    {
        public ConstulaLogFaturaVM(PAGNET_EMISSAOFATURAMENTO_LOG x)
        {
            codigoFatura = x.CODEMISSAOFATURAMENTO;
            codigoLog = x.CODEMISSAOFATURAMENTO_LOG;
            Status = x.STATUS.Replace("_", " ").Trim();
            dataAlteracao = x.DATINCLOG;
            Usuario = x.USUARIO_NETCARD.NMUSUARIO;
            Justificativa = x.DESCLOG;
          
        }


        [Display(Name = "Código")]
        public int codigoFatura { get; set; }

        [Display(Name = "Código log")]
        public int codigoLog { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Data Ação")]
        public DateTime dataAlteracao { get; set; }

        [Display(Name = "Usuário")]
        public string Usuario { get; set; }

        [Display(Name = "Justificativa")]
        public string Justificativa { get; set; }

    }

    public class ConstulaEmissao_BoletoVM
    {

        [Display(Name = "Código")]
        public int codEmissaoBoleto { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Nº Borderô")]
        public int codBordero { get; set; }

        [Display(Name = "Código")]
        public int CodCli { get; set; }

        [Display(Name = "Nome do Cliente")]
        public string nmCliente { get; set; }

        [Display(Name = "CNPJ")]
        public string cnpj { get; set; }

        [Display(Name = "Data Vencimento")]
        public string dtVencimento { get; set; }
        public DateTime dataVendimento { get; set; }

        [Display(Name = "Origem")]
        public string Origem  { get; set; }

        [Display(Name = "Data Emissao")]
        public string dtBordero { get; set; }

        [Display(Name = "Valor")]
        public string Valor { get; set; }

        [Display(Name = "Seu Número")]
        public string seuNumero { get; set; }

        internal static IList<ConstulaEmissao_BoletoVM> ToViewFaturamento<T>(ICollection<T> collection) where T : PAGNET_EMISSAOFATURAMENTO
        {
            return collection.Select(x => ToViewFaturamento(x)).ToList();
        }

        internal static ConstulaEmissao_BoletoVM ToViewFaturamento(PAGNET_EMISSAOFATURAMENTO x)
        {
            return new ConstulaEmissao_BoletoVM
            {
                codEmissaoBoleto = x.CODEMISSAOFATURAMENTO,
                Status = x.STATUS.Replace("_", " ").Trim(),
                codBordero = x.CODBORDERO ?? 0,
                CodCli = Convert.ToInt32(x.CODCLIENTE),
                Origem = x.ORIGEM,
                nmCliente = (x.CODCLIENTE != null) ? x.CODCLIENTE + " - " + Geral.FormataTexto(x.PAGNET_CADCLIENTE.NMCLIENTE, 35) : x.TIPOFATURAMENTO,
                cnpj = (x.CODCLIENTE != null) ? Geral.FormataCPFCnPj(x.PAGNET_CADCLIENTE.CPFCNPJ) : "",
                dtVencimento = x.DATVENCIMENTO.ToShortDateString(),
                dataVendimento = x.DATVENCIMENTO,
                Valor = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALOR),
                seuNumero = x.SEUNUMERO,
        };
        }

        internal static IList<ConstulaEmissao_BoletoVM> ToViewBoleto<T>(ICollection<T> collection) where T : PAGNET_EMISSAOBOLETO
        {
            return collection.Select(x => ToViewBoleto(x)).ToList();
        }

        internal static ConstulaEmissao_BoletoVM ToViewBoleto(PAGNET_EMISSAOBOLETO x)
        {
            return new ConstulaEmissao_BoletoVM
            {
                codEmissaoBoleto = x.codEmissaoBoleto,
                Status = x.Status.Replace("_", " ").Trim(),
                CodCli = x.CodCliente,
                nmCliente = x.CodCliente + " - " + Geral.FormataTexto(x.PAGNET_CADCLIENTE.NMCLIENTE, 35),
                cnpj = Geral.FormataCPFCnPj(x.PAGNET_CADCLIENTE.CPFCNPJ),
                dtVencimento = x.dtVencimento.ToShortDateString(),
                Valor = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.Valor),
                seuNumero = x.SeuNumero,
            };
        }
    }
    public class ListaEmissaoBoletoVM
    {
        public ListaEmissaoBoletoVM()
        {
            Sacado = new APIBoletoSacadoVM();
        }
        public APIBoletoSacadoVM Sacado { get; set; }


        public string CodigoOcorrencia { get; set; }
        public string DescricaoOcorrencia { get; set; }

        public string nmBoleto { get; set; }
        public decimal PercentualJurosDia { get; set; }
        public DateTime DataJuros { get; set; }
        public decimal PercentualMulta { get; set; }
        public DateTime DataMulta { get; set; }
        public string CodigoInstrucao1 { get; set; }
        public string ComplementoInstrucao1 { get; set; }
        public string CodigoInstrucao2 { get; set; }
        public string ComplementoInstrucao2 { get; set; }
        public string CodigoInstrucao3 { get; set; }
        public string ComplementoInstrucao3 { get; set; }
        public string MensagemInstrucoesCaixa { get; set; }
        public string MensagemArquivoRemessa { get; set; }
        public string NossoNumero { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime DataVencimento { get; set; }
        public string NumeroDocumento { get; set; }
        public string NumeroControleParticipante { get; set; }
        public decimal ValorTitulo { get; set; }
        public decimal ValorJurosDia { get; set; }
        public decimal ValorMulta { get; set; }
        public DateTime DataProcessamento { get; set; }



    }
    
    public class FiltroBoletosGeradosVM
    {
        public FiltroBoletosGeradosVM()
        {
            ListaBoletos = new List<ListaBoletosGeradosVM>();
        }

        [Display(Name = "Boletos Gerados e que ainda não foram liquidados")]
        public IList<ListaBoletosGeradosVM> ListaBoletos { get; set; }
        
        [Display(Name = "Selecione a Opção Desejada")]
        [Ajuda("Opções para realizar ajustes ou baixas nos boletos")]
        public string CodigoOcorrencia { get; set; }
        public string DescricaoOcorrencia { get; set; }


        internal static FiltroBoletosGeradosVM ToView(List<PAGNET_EMISSAOBOLETO> bol, string codOcorrencia, string DescOcorrencia)
        {
            return new FiltroBoletosGeradosVM
            {
                CodigoOcorrencia = codOcorrencia,
                DescricaoOcorrencia = DescOcorrencia,
                ListaBoletos = ListaBoletosGeradosVM.ToView(bol)
            };
        }
    }
    public class ListaBoletosGeradosVM
    {
        public bool chkFechCred { get; set; }

        [Display(Name = "Código")]
        public int codEmissaoBoleto { get; set; }

        [Display(Name = "status")]
        public string Status { get; set; }

        [Display(Name = "Código do Borderô")]
        public int codBordero { get; set; }

        [Display(Name = "Código")]
        public int CodCli { get; set; }

        [Display(Name = "Nome do Cliente")]
        public string nmCliente { get; set; }

        [Display(Name = "CNPJ")]
        public string cnpj { get; set; }

        [Display(Name = "Data Vencimento")]
        public string dtVencimento { get; set; }

        [Display(Name = "Valor")]
        public string Valor { get; set; }

        [Display(Name = "Lote")]
        public int numLote { get; set; }

        internal static IList<ListaBoletosGeradosVM> ToView<T>(ICollection<T> collection) where T : PAGNET_EMISSAOBOLETO
        {
            return collection.Select(x => ToView(x)).ToList();
        }

        internal static ListaBoletosGeradosVM ToView(PAGNET_EMISSAOBOLETO x)
        {
            return new ListaBoletosGeradosVM
            {
                chkFechCred = true,
                codEmissaoBoleto = x.codEmissaoBoleto,
                Status = x.Status,
                CodCli = x.CodCliente,
                cnpj = Geral.FormataCPFCnPj(x.PAGNET_CADCLIENTE.CPFCNPJ),
                nmCliente = Geral.FormataTexto(x.PAGNET_CADCLIENTE.NMCLIENTE, 39).Trim(),
                dtVencimento = x.dtVencimento.ToShortDateString(),
                Valor = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.Valor)
            };
        }
    }
    public class DadosBoletoVM
    {
        public DadosBoletoVM()
        {
            ListaBoletos = new List<SolicitacaoBoletoVM>();
        }

        [Display(Name = "Selecione os Itens para Incluir no Borderô")]
        public IList<SolicitacaoBoletoVM> ListaBoletos { get; set; }

        public int codUsuario { get; set; }
        public int codEmpresa { get; set; }
        public int codOpe { get; set; }

        [Display(Name = "Conta Corrente *")]
        [Ajuda("Conta que irá gerar os boletos")]
        public string codigoContaCorrente { get; set; }
        public string nomeContaCorrente { get; set; }


        [Display(Name = "Faturas Selecionadas")]
        [Ajuda("Quantidade de faturas que serão incluídos neste borderô")]
        public string qtFaturasSelecionados { get; set; }

        [Display(Name = "Valor Borderô")]
        [Ajuda("Valor total a ser incluído neste borderô")]
        [InputAttrAux(Inicio = "R$")]
        public string ValorBordero { get; set; }

        public string caminhoArquivo { get; set; }

        internal static DadosBoletoVM ToView(List<PROC_PAGNET_SOLICITACAOBOLETO> bol)
        {
            return new DadosBoletoVM
            {
                ListaBoletos = SolicitacaoBoletoVM.ToViewProc(bol)
            };
        }

       
    }
    public class SolicitacaoBoletoVM
    {
        public int codEmissaoBoleto { get; set; }
        public string MsgRetorno { get; set; }
        public bool chkBoleto { get; set; }

        public string nomeBoleto { get; set; }
        public string BoletoRecusado { get; set; }
        public string msgRecusa { get; set; }
        public string TAXAEMISSAOBOLETO { get; set; }
        public string PERCMULTA { get; set; }
        public string VLMULTADIAATRASO { get; set; }
        public string PERCJUROS { get; set; }
        public string VLJUROSDIAATRASO { get; set; }

        [Display(Name = "Código do Borderô")]
        public string codBordero { get; set; }

        [Display(Name = "Código do Cliente")]
        public string codigoCliente { get; set; }

        [Display(Name = "Cliente")]
        public string nomeCliente { get; set; }
        public string nomeCompletoCliente { get; set; }

        [Display(Name = "CNPJ")]
        public string cnpj { get; set; }
        
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }
        
        [Display(Name = "Data Vencimento")]
        public string dtVencimento { get; set; }

        [Display(Name = "Data Emissao")]
        public string dtEmissao { get; set; }

        [Display(Name = "Valor")]
        public string Valor { get; set; }

        [Display(Name = "Origem")]
        public string OrigemBoleto { get; set; }

        [Display(Name = "Origem")]
        public string TipoFaturamento { get; set; }
        

        [Display(Name = "Seu Número")]
        public string SeuNumero { get; set; }
        
        [Display(Name = "Código de Rastreio")]
        public string nroDocumento { get; set; }

        [Display(Name = "Ocorrencia Retorno")]
        public string DescricaoOcorrenciaRet { get; set; }

        [Display(Name = "Código Original")]
        public string CodFaturamentoPai { get; set; }

        [Display(Name = "Nº Parcela")]
        public string nParcela { get; set; }

        [Display(Name = "Total de Parcelas")]
        public string ntotalParcelas { get; set; }

        [Display(Name = "Valor Parcela")]
        public string ValParcela { get; set; }

        internal static IList<SolicitacaoBoletoVM> ToViewProc<T>(ICollection<T> collection) where T : PROC_PAGNET_SOLICITACAOBOLETO
        {
            return collection.Select(x => ToViewProc(x)).ToList();
        }

        internal static SolicitacaoBoletoVM ToViewProc(PROC_PAGNET_SOLICITACAOBOLETO x)
        {
            return new SolicitacaoBoletoVM
            {
                chkBoleto = true,
                codEmissaoBoleto = x.CODEMISSAOFATURAMENTO,
                //codBordero = Convert.ToString(x.CODBORDERO),
                Status = x.STATUS.Replace("_", " "),
                codigoCliente = x.CODCLIENTE.ToString(),
                nomeCliente = x.CODCLIENTE + " - " + Geral.FormataTexto(x.NMCLIENTE, 30).Trim(),
                nomeCompletoCliente = x.CODCLIENTE + " - " + x.NMCLIENTE.Trim(),
                cnpj = Geral.FormataCPFCnPj(x.CPFCNPJ),
                Email = Convert.ToString(x.EMAIL),
                dtVencimento = x.DATVENCIMENTO.ToShortDateString(),
                dtEmissao = Convert.ToDateTime(x.DATSOLICITACAO).ToShortDateString(),
                Valor = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALOR),
                nroDocumento = x.NRODOCUMENTO,
                OrigemBoleto = x.ORIGEM,
                TipoFaturamento = x.TIPOFATURAMENTO,
                CodFaturamentoPai = x.CODEMISSAOFATURAMENTOPAI.ToString(),
                nParcela = x.PARCELA.ToString(),
                ntotalParcelas = x.TOTALPARCELA.ToString(),
                ValParcela = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALORPARCELA),
            };
        }

        internal static IList<SolicitacaoBoletoVM> ToViewProcConsultaBoleto<T>(ICollection<T> collection) where T : PROC_PAGNET_CONSULTABOLETO
        {
            return collection.Select(x => ToViewProcConsultaBoleto(x)).ToList();
        }

        internal static SolicitacaoBoletoVM ToViewProcConsultaBoleto(PROC_PAGNET_CONSULTABOLETO x)
        {
            return new SolicitacaoBoletoVM
            {
                chkBoleto = true,
                codEmissaoBoleto = x.CODEMISSAOBOLETO,
                Status = x.STATUS.Replace("_", " "),
                codigoCliente = x.CODCLIENTE.ToString(),
                nomeCliente = x.CODCLIENTE + " - " + Geral.FormataTexto(x.NMCLIENTE, 39).Trim(),
                cnpj = Geral.FormataCPFCnPj(x.CPFCNPJ),
                Email = x.EMAIL.Trim(),
                dtVencimento = x.DATVENCIMENTO.ToShortDateString(),
                dtEmissao = Convert.ToDateTime(x.DATSOLICITACAO).ToShortDateString(),
                Valor = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALOR),
                SeuNumero = x.SEUNUMERO,
                OrigemBoleto = x.ORIGEM,
                DescricaoOcorrenciaRet = x.DESCRICAOOCORRENCIARETBOL,
                MsgRetorno = x.DESCRICAOOCORRENCIARETBOL,
                BoletoRecusado = x.BOLETORECUSADO,
                msgRecusa = x.MSGRECUSA,
                nomeBoleto = x.NMBOLETOGERADO,
                TAXAEMISSAOBOLETO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (decimal)x.TAXAEMISSAOBOLETO),
                PERCMULTA = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (decimal)x.PERCMULTA).Replace("R$", "") + "%",
                VLMULTADIAATRASO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (decimal)x.VLMULTADIAATRASO),
                PERCJUROS = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (decimal)x.PERCJUROS).Replace("R$", "") + "%",
                VLJUROSDIAATRASO = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", (decimal)x.VLJUROSDIAATRASO)

            };
        }
        

        internal static IList<SolicitacaoBoletoVM> ToViewFatura<T>(ICollection<T> collection) where T : PAGNET_EMISSAOFATURAMENTO
        {
            return collection.Select(x => ToViewFatura(x)).ToList();
        }

        internal static SolicitacaoBoletoVM ToViewFatura(PAGNET_EMISSAOFATURAMENTO x)
        {
            return new SolicitacaoBoletoVM
            {
                chkBoleto = true,
                codEmissaoBoleto = x.CODEMISSAOFATURAMENTO,
                Status = x.STATUS.Replace("_", " "),
                codigoCliente = (x.PAGNET_CADCLIENTE != null) ? x.CODCLIENTE.ToString() : "",
                nomeCliente =(x.PAGNET_CADCLIENTE != null) ? x.CODCLIENTE + " - " + Geral.FormataTexto(x.PAGNET_CADCLIENTE.NMCLIENTE, 39).Trim() : "",
                cnpj = (x.PAGNET_CADCLIENTE != null) ? Geral.FormataCPFCnPj(x.PAGNET_CADCLIENTE.CPFCNPJ): "",
                Email = (x.PAGNET_CADCLIENTE != null) ? Convert.ToString(x.PAGNET_CADCLIENTE.EMAIL) : "",
                dtVencimento = x.DATVENCIMENTO.ToShortDateString(),
                dtEmissao = Convert.ToDateTime(x.DATSOLICITACAO).ToShortDateString(),
                Valor = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", ((x.VALOR+ (x.MULTACOBRADA ?? 0) + (x.JUROSCOBRADO ?? 0)) - (x.VLDESCONTOCONCEDIDO ?? 0))),
                SeuNumero = x.SEUNUMERO,
                OrigemBoleto = x.ORIGEM,
                nroDocumento = x.NRODOCUMENTO,
                CodFaturamentoPai = x.CODEMISSAOFATURAMENTOPAI.ToString(),
                nParcela = x.PARCELA.ToString(),
                ntotalParcelas = x.TOTALPARCELA.ToString(),
                ValParcela = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.VALORPARCELA),

    };
        }
        internal static IList<SolicitacaoBoletoVM> ToView<T>(ICollection<T> collection) where T : PAGNET_EMISSAOBOLETO
        {
            return collection.Select(x => ToView(x)).ToList();
        }

        internal static SolicitacaoBoletoVM ToView(PAGNET_EMISSAOBOLETO x)
        {
            return new SolicitacaoBoletoVM
            {
                chkBoleto = true,
                codEmissaoBoleto = x.codEmissaoBoleto,
                nomeBoleto = x.nmBoletoGerado,
                //codBordero = Convert.ToString(x.CODBORDERO),
                Status = x.Status.Replace("_", " "),
                codigoCliente = x.CodCliente.ToString(),
                nomeCliente = x.CodCliente + " - " + Geral.FormataTexto(x.PAGNET_CADCLIENTE.NMCLIENTE, 39).Trim(),
                cnpj = Geral.FormataCPFCnPj(x.PAGNET_CADCLIENTE.CPFCNPJ),
                Email = Convert.ToString(x.PAGNET_CADCLIENTE.EMAIL),
                dtVencimento = x.dtVencimento.ToShortDateString(),
                dtEmissao = Convert.ToDateTime(x.dtSolicitacao).ToShortDateString(),
                Valor = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.Valor),
                SeuNumero = x.SeuNumero,
                DescricaoOcorrenciaRet = x.DescricaoOcorrenciaRetBol,
                MsgRetorno = x.DescricaoOcorrenciaRetBol,
            };
        }
    }
    public class ListaBoletosVM
    {

        internal static IList<ListaBoletosVM> ToView<T>(ICollection<T> collection) where T : PAGNET_EMISSAOBOLETO
        {
            return collection.Select(x => ToView(x)).ToList();
        }

        internal static ListaBoletosVM ToView(PAGNET_EMISSAOBOLETO x)
        {
            return new ListaBoletosVM
            {
                chkBoleto = true,
                codEmissaoBoleto = x.codEmissaoBoleto,
                Status = x.Status.Replace("_", " "),
                nomeBoleto = x.nmBoletoGerado,
                Cliente = x.CodCliente + " - " + Geral.FormataTexto(x.PAGNET_CADCLIENTE.NMCLIENTE, 39).Trim(),
                CNPJ = Geral.FormataCPFCnPj(x.PAGNET_CADCLIENTE.CPFCNPJ),
                EmailCliente = x.PAGNET_CADCLIENTE.EMAIL,
                Valor = string.Format(CultureInfo.GetCultureInfo("pt-BR"), "{0:C}", x.Valor),
                dtVencimento = x.dtVencimento.ToShortDateString(),
                BoletoEnviado = (x.BOLETOENVIADO == "S") ? "Sim" : "Não"

            };
        }

        public bool chkBoleto { get; set; }
        public int codEmissaoBoleto { get; set; }
        public string nomeBoleto { get; set; }
        public int codigoEmpresaBoleto { get; set; }


        [Display(Name = "Enviar cópia do e-mail")]
        public string copiaEmail { get; set; }

        [Display(Name = "Borderô")]
        public string codBordero { get; set; }

        [Display(Name = "Cliente")]
        public string Cliente { get; set; }

        [Display(Name = "CNPJ")]
        public string CNPJ { get; set; }

        [Display(Name = "Valor")]
        public string Valor { get; set; }

        [Display(Name = "Vencimento")]
        public string dtVencimento { get; set; }

        [Display(Name = "Email do Cliente")]
        public string EmailCliente { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "Enviado")]
        public string BoletoEnviado { get; set; }

    }
    public class ParcelamentoFaturaVm
    {
        public int Parcela_codFaturamento { get; set; }
        public int Parcela_codUsuario { get; set; }
        public int Parcela_codEmpresa { get; set; }

        public IList<ListaParcelasVm> ListaParcelas { get; set; }

        [Display(Name = "Cliente")]
        public string Parcela_Cliente { get; set; }

        [Display(Name = "Valor Original")]
        [InputAttrAux(Inicio = "R$")]
        public string Parcela_ValorOriginal { get; set; }

        [Display(Name = "Vencimento")]
        public string Parcela_dtVencimento { get; set; }

        [Display(Name = "Quantidade de Parcelas")]
        public int Parcela_qtParcelas { get; set; }

        [Display(Name = "Valor Restante")]
        [InputAttrAux(Inicio = "R$")]
        public string Parcela_ValorRestante { get; set; }
        
        [Display(Name = "Data Primeira Parcela")]
        [Ajuda("A partir da data da primeira parcela será calculado as outras parcelas")]
        [InputMask("99/99/9999")]
        [InputAttrAux(Final = "<i class='fa fa-calendar'></i>")]
        public string Parcela_dataPrimeiraParcela { get; set; }        

        [Display(Name = "Taxa")]
        [InputAttrAux(Final = "% / Mês")]
        [InputMask("#.##0,00", IsReverso = true)]
        [StringLength(5)]
        public string Parcela_TaxaMensal { get; set; }

        [Display(Name = "Justificativa")]
        [Ajuda("Informe o motivo da edição do título")]
        public string codJustificativaParcela { get; set; }
        public string descJustificativaParcela { get; set; }

        [Display(Name = "Especifique")]
        [Ajuda("Especifique o motivo da edição.")]
        [StringLength(200)]
        public string DescJustOutrosParcela { get; set; }

    }
    public class ListaParcelasVm
    {

        [Display(Name = "Total a Pagar")]
        public string TotalAPagar { get; set; }

        [Display(Name = "Total de Juros")]
        public string TotalJuros { get; set; }

        [Display(Name = "Valor da Parcela")]
        public string ValorParcela { get; set; }
        public string NroParcela { get; set; }
        public string SaldoAnterior { get; set; }
        public string SaldoDevedor { get; set; }
        public string Amortizacao { get; set; }
        public string Juros { get; set; }
        public string ValorPago { get; set; }

        public string VencimentoParcela { get; set; }
    }
    public class FiltroArquivoRetornoVM
    {
        public int codUsuario { get; set; }
        public bool acessoAdmin { get; set; }
        public string CodigoCliente { get; set; }

        [Display(Name = "Empresa")]
        public string codigoEmpresa { get; set; }
        public string nomeEmpresa { get; set; }

        [Display(Name = "Código")]
        [Ajuda("Poderá ser utilizado o códgio ou CNPJ do cliente ")]
        public string filtroCliente { get; set; }

        [Display(Name = "Cliente")]
        public string nomeCliente { get; set; }

        [Display(Name = "Fatura")]
        public string codigoFatura { get; set; }
        public string nomeFatura { get; set; }
    }
    public class ListaClienteUsuarioVm
    {
        [Display(Name = "Quantidade Não Selecinada")]
        public int qtNaoDefinidos { get; set; }
        [Display(Name = "Quantidade a Gerar Fatura")]
        public int qtGeraFatura { get; set; }
        [Display(Name = "Quantidade a Cancelar")]
        public int qtBaixaAutomatica { get; set; }

        [Display(Name = "Valor Não Selecinada")]
        public string vlNaoDefinido { get; set; }
        [Display(Name = "Valor a Gerar Fatura")]
        public string vlGeraFatura { get; set; }
        [Display(Name = "Valor a Cancelar")]
        public string vlBaixaAutomatica { get; set; }

        public int codigoFatura { get; set; }
        public bool chkGeraFaturaUsuario { get; set; }
        public bool chkProximaFatura { get; set; }
        public bool chkBaixarFatura { get; set; }
        public string Matricula { get; set; }
        public string CodigoCliente { get; set; }
        public string CPF { get; set; }
        public string NomeClienteUsuario { get; set; }
        public string Valor { get; set; }
    }
    public class DadosEnvioEmailMassModel
    {
        public DadosEnvioEmailMassModel()
        {
            ListaBoleto = new List<InformacaoFaturaModel>();
        }
        public IList<InformacaoFaturaModel> ListaBoleto { get; set; }
        public int codigoEmpresa { get; set; }
    }
    public class InformacaoFaturaModel
    {
        public int codEmissaoBoleto { get; set; }
        public int codStatus { get; set; }
    }
}
