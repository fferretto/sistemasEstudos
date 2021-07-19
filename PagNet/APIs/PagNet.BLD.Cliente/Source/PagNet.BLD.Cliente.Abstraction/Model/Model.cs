using PagNet.BLD.Cliente.Abstraction.Interface.Model;

namespace PagNet.BLD.Cliente.Abstraction.Model
{
    public class ClienteVm : IClienteVm
    {
        public int codigoCliente { get; set; }
        public bool AgruparFaturamentos { get; set; }
        public bool cobrancaDiferenciada { get; set; }
        public string regraPropria { get; set; }
        public string nomeCliente { get; set; }
        public string email { get; set; }
        public string cpfCnpj { get; set; }
        public string codigoEmpresa { get; set; }
        public string nomeEmpresa { get; set; }
        public string cep { get; set; }
        public string logradouro { get; set; }
        public string numeroLogradouro { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string UF { get; set; }
        public bool cobraMulta { get; set; }
        public string valorMultaDiaAtraso { get; set; }
        public string percentualMulta { get; set; }
        public bool cobraJuros { get; set; }
        public string percentualJuros { get; set; }
        public string valorJurosDiaAtraso { get; set; }
        public string codigoPrimeiraIntrucaoCobranca { get; set; }
        public string nomePrimeiraIntrucaoCobranca { get; set; }
        public string codigoSegundaIntrucaoCobranca { get; set; }
        public string nomeSegundaIntrucaoCobranca { get; set; }
        public string codigoFormaFaturamento { get; set; }
        public string nomeFormaFaturamento { get; set; }
        public string CodigoTipoPessoa { get; set; }
        public string NomeTipoPessoa { get; set; }
        public string taxaEmissaoBoleto { get; set; }
        public string DescJustOutros { get; set; }
    }
}
