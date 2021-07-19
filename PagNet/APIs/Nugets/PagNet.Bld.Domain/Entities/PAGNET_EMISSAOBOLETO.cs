using System;
using System.Collections.Generic;

namespace PagNet.Bld.Domain.Entities
{
    public partial class PAGNET_EMISSAOBOLETO
    {
        public PAGNET_EMISSAOBOLETO()
        {
            this.PAGNET_LOGEMAILENVIADO = new List<PAGNET_LOGEMAILENVIADO>();
            
        }
        public int codEmissaoBoleto { get; set; }
        public string Status { get; set; }
        public int CodCliente { get; set; }
        public int codEmpresa { get; set; }
        public int codContaCorrente { get; set; }
        public DateTime dtVencimento { get; set; }
        public DateTime? DATPGTO { get; set; }
        public decimal? VLPGTO { get; set; }
        public string NossoNumero { get; set; }
        public int? codOcorrencia { get; set; }
        public string SeuNumero { get; set; }
        public decimal Valor { get; set; }
        public DateTime dtSolicitacao { get; set; }
        public DateTime? dtReferencia { get; set; }
        public DateTime? dtSegundoDesconto { get; set; }
        public decimal? vlDesconto { get; set; }
        public decimal? vlSegundoDesconto { get; set; }        
        public string MensagemArquivoRemessa { get; set; }
        public string MensagemInstrucoesCaixa { get; set; }        
        public string numControle { get; set; }
        public string OcorrenciaRetBol { get; set; }
        public string nmBoletoGerado { get; set; }
        public string DescricaoOcorrenciaRetBol { get; set; }
        public int CODARQUIVO { get; set; }
        public string BOLETOENVIADO { get; set; }

        public virtual PAGNET_CADCLIENTE PAGNET_CADCLIENTE { get; set; }
        public virtual PAGNET_CADEMPRESA PAGNET_CADEMPRESA { get; set; }
        public virtual PAGNET_CONTACORRENTE PAGNET_CONTACORRENTE { get; set; }

        public virtual ICollection<PAGNET_LOGEMAILENVIADO> PAGNET_LOGEMAILENVIADO { get; set; }
    }
}
