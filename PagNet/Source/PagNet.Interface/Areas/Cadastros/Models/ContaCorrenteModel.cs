using PagNet.Api.Service.Interface.Model;
using PagNet.Api.Service.Model;
using PagNet.Application.Helpers;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PagNet.Interface.Areas.Cadastros.Models
{
    public class DadosHomologarContaCorrenteModel : IAPIDadosHomologarContaCorrenteModel
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

        [Display(Name = "Código")]
        [Ajuda("Informe o CPF/CNPJ ou o código do Fornecedor/Centralizadora")]
        public string filtroFavorecido { get; set; }

        [Display(Name = "Favorecido")]
        public string nomeFavorecido { get; set; }

        [Display(Name = "Código")]
        [Ajuda("Informe o CPF/CNPJ ou o código do Cliente")]
        public string filtroCliente { get; set; }

        [Display(Name = "Cliente")]
        public string nomeCliente { get; set; }
    }
    public class ContaCorrenteModel : IAPIContaCorrenteModel
    {
        public bool UsuarioTelenet { get; set; }
        public int codContaCorrente { get; set; }
        public bool bBoleto { get; set; }
        public bool bPagamento { get; set; }
        public bool bTransmitirArqAuto { get; set; }
        public bool acessoAdmin { get; set; }

        [Display(Name = "Nome da Conta Corrente")]
        [Required(ErrorMessage = "Informe o Nome da Conta Corrente")]
        [Ajuda("Nome utilizado para identificar a conta corrente no sistema.")]
        [StringLength(80)]
        public string nmContaCorrente { get; set; }

        [Display(Name = "Razão Social da Empresa")]
        [Required(ErrorMessage = "Obrigatório informar a Razão Social da Empresa.")]
        [Ajuda("Razão Social da empresa dona da conta.")]
        [StringLength(150)]
        public string nmEmpresa { get; set; }

        [Display(Name = "CNPJ")]
        [Ajuda("CNPJ da empresa")]
        [Required(ErrorMessage = "Informe o CNPJ da Empresa")]
        [StringLength(18)]
        [InputMask("99.999.999/9999-99")]
        public string CpfCnpj { get; set; }

        [Display(Name = "Empresa")]
        public string codEmpresa { get; set; }
        public string nmEmpresaPagNet { get; set; }

        [Display(Name = "Banco")]
        [Required(ErrorMessage = "Obrigatório informar o banco")]
        [Ajuda("Selecione o banco desta conta corrente")]
        public string CodBanco { get; set; }
        public string nmBanco { get; set; }

        [Display(Name = "Operação")]
        [StringLength(3)]
        public string codOperacaoCC { get; set; }

        [Display(Name = "Nº Conta Corrente")]
        [Required(ErrorMessage = "Informe o Nº Conta Corrente")]
        [StringLength(10)]
        public string nroContaCorrente { get; set; }

        [Display(Name = "Dígito")]
        [Required(ErrorMessage = "Informe o Dígito da Conta Corrente")]
        [Ajuda("Dígito da conta corrente")]
        [StringLength(1)]
        public string DigitoCC { get; set; }

        [Display(Name = "Nº da Agência")]
        [Required(ErrorMessage = "Informe o Nº da Agência")]
        [StringLength(6)]
        public string Agencia { get; set; }

        [Display(Name = "Dígito")]
        [Required(ErrorMessage = "Informe o Dígito da Agência")]
        [Ajuda("Dígito da agência")]
        [StringLength(1)]
        public string DigitoAgencia { get; set; }

        [Display(Name = "Nº Cedente")]
        [StringLength(20)]
        public string codigoCedente { get; set; }

        [Display(Name = "DV Cedente")]
        [StringLength(2)]
        public string digitoCodigoCedente { get; set; }

        [Display(Name = "Código de Transmissão")]
        [Ajuda("Informação concedida pelo banco.")]
        [StringLength(40)]
        public string CodTransmissao { get; set; }
        
        [Display(Name = "Nº da Carteira para o Arquivo Remessa")]
        [RequiredIf("bBoleto", true, "Informe o Nº da Carteira Remessa")]
        [Ajuda("Informação concedida pelo banco.")]
        public string CarteiraRemessa { get; set; }

        [Display(Name = "Variacao da Carteira")]
        [Ajuda("Deixar em branco caso não possua.")]
        [StringLength(5)]
        public string VariacaoCarteira { get; set; }

        [Display(Name = "Convênio para Pagamento")]
        [Ajuda("Informação concedida pelo banco (Banco Itaú não possui esta informação).")]
        [StringLength(20)]
        public string CodConvenioPag { get; set; }

        [Display(Name = "Parâmetro de Transmissão")]
        [Ajuda("Informação concedida pelo banco Caixa Econômica Feredal.")]
        [StringLength(20)]
        public string ParametroTransmissaoPag { get; set; }

        [Display(Name = "Valor TED")]
        [Ajuda("Este valor será abatido no valor total da transferência caso a forma de pagamento utilizada seja TED")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string valTED { get; set; }

        [Display(Name = "Valor Mínimo para Pagamento via CC")]
        [Ajuda("Somatória de todo valor a ser pago via CRÉDITO EM CONTA para o favorecido no dia deverá ser igual ou superior ao valor informado neste campo.")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string ValMinPGTO { get; set; }

        [Display(Name = "Valor Mínimo para Pagamento via TED")]
        [Ajuda("Somatória de todo valor a ser pago via TED para o favorecido no dia deverá ser igual ou superior ao valor informado neste campo.")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string ValMinTED { get; set; }

        [Display(Name = "Saldo em Conta")]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(15)]
        public string SaldoConta { get; set; }

        public string formaTransmissaoPG { get; set; }
        public int codigoTransmissaoArquivoPG { get; set; }
        public string tipoArquivoPG { get; set; }

        [Display(Name = "Login")]
        [StringLength(70)]
        public string loginTransmissaoPG { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [StringLength(30)]
        public string senhaTransmissaoPG { get; set; }

        [Display(Name = "Caminho do Aquivo de Remessa")]
        [StringLength(150)]
        public string caminhoRemessaPG { get; set; }

        [Display(Name = "Caminho do Aquivo de Retorno")]
        [StringLength(150)]
        public string caminhoRetornoPG { get; set; }

        public string caminhoAuxiliarPG { get; set; }

        public string formaTransmissaoBol { get; set; }
        public int codigoTransmissaoArquivoBol { get; set; }
        public string tipoArquivoBol { get; set; }

        [Display(Name = "Login")]
        [StringLength(70)]
        public string loginTransmissaoBol { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [StringLength(30)]
        public string senhaTransmissaoBol { get; set; }

        [Display(Name = "Caminho do Aquivo de Remessa")]
        [StringLength(150)]
        public string caminhoRemessaBol { get; set; }

        [Display(Name = "Caminho do Aquivo de Retorno")]
        [StringLength(150)]
        public string caminhoRetornoBol { get; set; }
        public string caminhoAuxiliarBol { get; set; }

        [Display(Name = "Será Cobrado Juros?")]
        public bool teraJuros { get; set; }

        [Display(Name = "Valor Juros")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(10)]
        public string ValorJuros { get; set; }

        [Display(Name = "Percentual do Juros")]
        [Ajuda("Percentual do juros a ser cobrado")]
        [InputMask("##0,00", IsReverso = true)]
        [InputAttrAux(Final = "%")]
        [StringLength(5)]
        public string PercJuros { get; set; }

        [Display(Name = "Será Cobrado Multa?")]
        public bool teraMulta { get; set; }

        [Display(Name = "Valor Multa")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(10)]
        public string valorMulta { get; set; }

        [Display(Name = "Percentual da Multa")]
        [Ajuda("Percentual da multa a ser cobrado")]
        [InputMask("##0,00", IsReverso = true)]
        [InputAttrAux(Final = "%")]
        [StringLength(5)]
        public string PercMulta { get; set; }

        [Display(Name = "Primeira Instrução Cobrança")]
        [RequiredIf("bBoleto", true, "Obrigatório informar a Primeira Instrução Cobrança")]
        [Ajuda("Instrução para o caixa")]
        public string codigoPrimeiraInscricaoCobraca { get; set; }
        public string NomePrimeiraInscricaoCobraca { get; set; }

        [Display(Name = "Segunda Instrução Cobrança")]
        [RequiredIf("bBoleto", true, "Obrigatório informar a Segunda Instrução Cobrança")]
        [Ajuda("Instrução para o caixa")]
        public string codigoSegundaInscricaoCobraca { get; set; }
        public string NomeSegundaInscricaoCobraca { get; set; }

        [Display(Name = "Valor da Taxa para Emissão de Boleto")]
        [Ajuda("Este valor será acrescentado no valor do boleto.")]
        [InputMask("#.##0,00", IsReverso = true)]
        [InputAttrAux(Inicio = "R$")]
        [StringLength(13)]
        public string TaxaEmissaoBoleto { get; set; }

        public bool cnab240pgto { get; set; }
        public bool cnab240boleto { get; set; }
        public string qtPosicaoArqPGTO { get; set; }
        public string qtPosicaoArqBoleto { get; set; }
        public bool AgruparFaturamentosDia { get; set; }

        internal static ContaCorrenteModel ToView(IAPIContaCorrenteModel item)
        {
            return new ContaCorrenteModel
            {
                bBoleto = item.bBoleto,
                bPagamento = item.bPagamento,
                codContaCorrente = item.codContaCorrente,
                nmContaCorrente = item.nmContaCorrente,
                nmEmpresa = item.nmEmpresa,
                CpfCnpj = item.CpfCnpj,
                codEmpresa = item.codEmpresa,
                nmEmpresaPagNet = item.nmEmpresaPagNet,
                CodBanco = item.CodBanco,
                nmBanco = item.nmBanco,
                codOperacaoCC = item.codOperacaoCC,
                nroContaCorrente = item.nroContaCorrente,
                DigitoCC = item.DigitoCC,
                Agencia = item.Agencia,
                DigitoAgencia = item.DigitoAgencia,
                CodTransmissao = item.CodTransmissao,
                CarteiraRemessa = item.CarteiraRemessa,
                VariacaoCarteira = item.VariacaoCarteira,
                CodConvenioPag = item.CodConvenioPag,
                ParametroTransmissaoPag = item.ParametroTransmissaoPag,
                valTED = item.valTED,
                ValMinPGTO = item.ValMinPGTO,
                ValMinTED = item.ValMinTED,
                SaldoConta = item.SaldoConta,

                codigoTransmissaoArquivoPG = item.codigoTransmissaoArquivoPG,
                tipoArquivoPG = item.tipoArquivoPG,
                formaTransmissaoPG = item.formaTransmissaoPG,
                loginTransmissaoPG = item.loginTransmissaoPG,
                senhaTransmissaoPG = item.senhaTransmissaoPG,
                caminhoRemessaPG = item.caminhoRemessaPG,
                caminhoRetornoPG = item.caminhoRetornoPG,
                caminhoAuxiliarPG = item.caminhoAuxiliarPG,

                codigoTransmissaoArquivoBol = item.codigoTransmissaoArquivoBol,
                tipoArquivoBol = item.tipoArquivoBol,
                formaTransmissaoBol = item.formaTransmissaoBol,
                loginTransmissaoBol = item.loginTransmissaoBol,
                senhaTransmissaoBol = item.senhaTransmissaoBol,
                caminhoRemessaBol = item.caminhoRemessaBol,
                caminhoRetornoBol = item.caminhoRetornoBol,
                caminhoAuxiliarBol = item.caminhoAuxiliarBol,


                teraJuros = item.teraJuros,
                ValorJuros = item.ValorJuros,
                PercJuros = item.PercJuros,
                teraMulta = item.teraMulta,
                valorMulta = item.valorMulta,
                PercMulta = item.PercMulta,
                codigoPrimeiraInscricaoCobraca = item.codigoPrimeiraInscricaoCobraca,
                NomePrimeiraInscricaoCobraca = item.NomePrimeiraInscricaoCobraca,
                codigoSegundaInscricaoCobraca = item.codigoSegundaInscricaoCobraca,
                NomeSegundaInscricaoCobraca = item.NomeSegundaInscricaoCobraca,
                TaxaEmissaoBoleto = item.TaxaEmissaoBoleto,
                AgruparFaturamentosDia = item.AgruparFaturamentosDia,
                qtPosicaoArqPGTO = item.qtPosicaoArqPGTO,
                qtPosicaoArqBoleto = item.qtPosicaoArqBoleto,
                cnab240pgto = (item.qtPosicaoArqPGTO == "240"),
                cnab240boleto = (item.qtPosicaoArqBoleto == "240"),
                codigoCedente = item.codigoCedente,
                digitoCodigoCedente = item.digitoCodigoCedente

            };
        }


    }
    public class BancoModel : IAPIBancoModel
    {
        public string CODBANCO { get; set; }
        public string NMBANCO { get; set; }
    }
    public class ConsultaContaCorrenteModel : IAPIConsultaContaCorrenteModel
    {
        public int codContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }
        public string nroContaCorrente { get; set; }
        public string Agencia { get; set; }


        internal static IList<ConsultaContaCorrenteModel> ToListView<T>(ICollection<T> collection) where T : IAPIConsultaContaCorrenteModel
        {
            return collection.Select(x => ToListView(x)).ToList();
        }

        internal static ConsultaContaCorrenteModel ToListView(IAPIConsultaContaCorrenteModel _cad)
        {
            return new ConsultaContaCorrenteModel
            {
                codContaCorrente = _cad.codContaCorrente,
                nmContaCorrente = _cad.nmContaCorrente,
                nroContaCorrente = _cad.nroContaCorrente,
                Agencia = _cad.Agencia
            };
        }
    }
}
