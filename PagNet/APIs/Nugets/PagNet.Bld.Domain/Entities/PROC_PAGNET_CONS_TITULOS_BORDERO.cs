using System;

namespace PagNet.Bld.Domain.Entities
{
    public class PROC_PAGNET_CONS_TITULOS_BORDERO
    {
        public int CODFAVORECIDO { get; set; }
        public string NMFAVORECIDO { get; set; }
        public string CPFCNPJ { get; set; }
        public DateTime DATREALPGTO { get; set; }
        public int QTTITULOS { get; set; }
        public decimal VALTOTAL { get; set; }
        public string BANCO { get; set; }
        public string AGENCIA { get; set; }
        public string CONTACORRENTE { get; set; }
        public string TIPOTITULO { get; set; }
        public string DESFORPAG { get; set; }
        public string LINHADIGITAVEL { get; set; }
        public decimal TAXATRANSFERENCIA { get; set; }
        public decimal VALORPREVISTOPAGAMENTO { get; set; }
        
    }
}