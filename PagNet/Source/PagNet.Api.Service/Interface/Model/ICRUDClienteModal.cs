using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Api.Service.Interface.Model
{
    public interface IAPIClienteModel
    {
        int codigoCliente { get; set; }
        bool AgruparFaturamentos { get; set; }
        bool cobrancaDiferenciada { get; set; }
        string regraPropria { get; set; }
        string nomeCliente { get; set; }
        string email { get; set; }
        string cpfCnpj { get; set; }

        string codigoEmpresa { get; set; }
        string nomeEmpresa { get; set; }

        string cep { get; set; }
        string logradouro { get; set; }
        string numeroLogradouro { get; set; }
        string complemento { get; set; }
        string bairro { get; set; }
        string cidade { get; set; }
        string UF { get; set; }

        bool cobraMulta { get; set; }
        string valorMultaDiaAtraso { get; set; }
        string percentualMulta { get; set; }

        bool cobraJuros { get; set; }
        string percentualJuros { get; set; }
        string valorJurosDiaAtraso { get; set; }

        string codigoPrimeiraIntrucaoCobranca { get; set; }
        string nomePrimeiraIntrucaoCobranca { get; set; }

        string codigoSegundaIntrucaoCobranca { get; set; }
        string nomeSegundaIntrucaoCobranca { get; set; }

        string codigoFormaFaturamento { get; set; }
        string nomeFormaFaturamento { get; set; }

        string CodigoTipoPessoa { get; set; }
        string NomeTipoPessoa { get; set; }

        string taxaEmissaoBoleto { get; set; }

        string DescJustOutros { get; set; }
    }
    public interface IAPIFiltroClienteModel
    {
        int codigoEmpresa { get; set; }
        int codigoCliente { get; set; }
        string CpfCnpj { get; set; }
        string TipoCliente { get; set; }
        string JustificativaAcao { get; set; }
    }
}
