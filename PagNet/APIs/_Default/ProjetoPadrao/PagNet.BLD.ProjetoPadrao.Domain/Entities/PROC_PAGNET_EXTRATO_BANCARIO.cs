using System;

namespace PagNet.BLD.ProjetoPadrao.Domain.Entities
{
    public class PROC_PAGNET_EXTRATO_BANCARIO
    {
        public string DESCRICAO { get; set; }
        public DateTime DATA { get; set; }
        public decimal VALOR { get; set; }
        public string TIPOTRANSACAO { get; set; }
        public decimal SALDOANTERIOR { get; set; }
        public decimal TOTALENTRADA { get; set; }
        public decimal TOTALSAIDA { get; set; }
        public decimal SALDOFINAL { get; set; }        
    }
}
