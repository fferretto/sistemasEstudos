using PagNet.Bld.PGTO.Favorecido.Abstraction.Interface.Model;

namespace PagNet.Bld.PGTO.Favorecido.Abstraction.Model
{
    public class FavorecidosModel : IFavorecidosModel
    {
        public int codigoFavorecido { get; set; }
        public bool regraDiferenciada { get; set; }
        public bool contaPagamentoPadrao { get; set; }
        public int codigoEmpresa { get; set; }
        public string nomeEmpresa { get; set; }
        public string cobrancaDiferenciada { get; set; }
        public string nomeFavorecido { get; set; }
        public string codigoCentralizadora { get; set; }
        public string CPFCNPJ { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public string DvAgencia { get; set; }
        public string Operacao { get; set; }
        public string contaCorrente { get; set; }
        public string DvContaCorrente { get; set; }
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string nroLogradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string cidade { get; set; }
        public string UF { get; set; }
        public string ValorTED { get; set; }
        public string ValorMinimoTed { get; set; }
        public string ValorMinimoCC { get; set; }
        public string codigoContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }
    }
    public class FavorecidosLogModel
    {
        public int codigoFavorecido { get; set; }
        public bool regraDiferenciada { get; set; }
        public bool contaPagamentoPadrao { get; set; }
        public int codigoEmpresa { get; set; }
        public string nomeEmpresa { get; set; }
        public string cobrancaDiferenciada { get; set; }
        public string nomeFavorecido { get; set; }
        public string codigoCentralizadora { get; set; }
        public string CPFCNPJ { get; set; }
        public string Banco { get; set; }
        public string Agencia { get; set; }
        public string DvAgencia { get; set; }
        public string Operacao { get; set; }
        public string contaCorrente { get; set; }
        public string DvContaCorrente { get; set; }
        public string CEP { get; set; }
        public string Logradouro { get; set; }
        public string nroLogradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string cidade { get; set; }
        public string UF { get; set; }
        public string ValorTED { get; set; }
        public string ValorMinimoTed { get; set; }
        public string ValorMinimoCC { get; set; }
        public string codigoContaCorrente { get; set; }
        public string nmContaCorrente { get; set; }
    }

    public class RetornoModel
    {
        public bool Sucesso { get; set; }
        public string msgResultado { get; set; }
    }

    public class DadosLogModal
    {
        public string dataLog { get; set; }
        public string descLog { get; set; }
        public string nomeUsu { get; set; }
    }
}
