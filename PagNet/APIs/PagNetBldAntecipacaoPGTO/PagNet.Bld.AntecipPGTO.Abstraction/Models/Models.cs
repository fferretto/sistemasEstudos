using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.AntecipPGTO.Abstraction.Models
{
    public class RegraNegocioPagamentoVm
    {
        public int CODREGRA { get; set; }
        public bool acessoAdmin { get; set; }
        public int CodUsuario { get; set; }
        public bool COBRATAXAANTECIPACAO { get; set; }
        public string PORCENTAGEMTAXAANTECIPACAO { get; set; }
        public string VLTAXAANTECIPACAO { get; set; }
        public string TIPOFORMACOMPENSACAO { get; set; }
        public string ATIVO { get; set; }
        public string codJustificativa { get; set; }
        public string descJustificativa { get; set; }
        public string DescJustOutros { get; set; }

    }
    public class ListaAntecipacaoPGTOVm
    {

        public bool chkTituloAntecipacao { get; set; }
        public int Codigo { get; set; }
        public int CODFAVORECIDO { get; set; }
        public string NMFAVORECIDO { get; set; }
        public string CNPJ { get; set; }
        public string DTEMISSAO { get; set; }
        public string DTREALPGTO { get; set; }
        public string CODBANCO { get; set; }
        public string AGENCIA { get; set; }
        public string CONTACORRENTE { get; set; }
        public string VALATUAL { get; set; }
        public string VALTAXA { get; set; }
        public string ValorTaxaProRata { get; set; }
        public string VALTOTAL { get; set; }
        public string TIPOCARTAO { get; set; }
        public int CODEMPRESA { get; set; }       

    }

}
