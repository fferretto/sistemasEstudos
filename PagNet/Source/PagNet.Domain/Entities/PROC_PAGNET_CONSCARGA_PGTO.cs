using System;

namespace PagNet.Domain.Entities
{
    public class PROC_PAGNET_CONSCARGA_PGTO
    {
        public int CODCLI { get; set; }
        public string CNPJ { get; set; }
        public string NMCLIENTE { get; set; }
        public int NUMCARGA { get; set; }
        public DateTime DTAUTORIZ { get; set; }
        public DateTime? DTCARGA { get; set; }
        public int QUANT { get; set; }
        public decimal VALCARGA { get; set; }
        public decimal VAL2AVIA { get; set; }
        public decimal VALTAXA { get; set; }
        public decimal VALTAXACARREG { get; set; }
        public decimal SALDOCONTAUTIL { get; set; }
        public decimal TOTAL { get; set; }
        public string DESCRICAOOCORRENCIARETBOL { get; set; }
    }
}
           