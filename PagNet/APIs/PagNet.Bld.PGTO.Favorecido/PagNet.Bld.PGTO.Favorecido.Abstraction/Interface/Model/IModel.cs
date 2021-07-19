namespace PagNet.Bld.PGTO.Favorecido.Abstraction.Interface.Model
{
    public interface IFavorecidosModel
    {
        int codigoFavorecido { get; set; }

        bool regraDiferenciada { get; set; }
        bool contaPagamentoPadrao { get; set; }
        int codigoEmpresa { get; set; }
        string nomeEmpresa { get; set; }
        string cobrancaDiferenciada { get; set; }
        string nomeFavorecido { get; set; }
        string codigoCentralizadora { get; set; }
        string CPFCNPJ { get; set; }
        string Banco { get; set; }
        string Agencia { get; set; }
        string DvAgencia { get; set; }
        string Operacao { get; set; }
        string contaCorrente { get; set; }
        string DvContaCorrente { get; set; }
        string CEP { get; set; }
        string Logradouro { get; set; }
        string nroLogradouro { get; set; }
        string Complemento { get; set; }
        string Bairro { get; set; }
        string cidade { get; set; }
        string UF { get; set; }
        string ValorTED { get; set; }
        string ValorMinimoTed { get; set; }

        string ValorMinimoCC { get; set; }
        
        string codigoContaCorrente { get; set; }
        string nmContaCorrente { get; set; }

    }

}
