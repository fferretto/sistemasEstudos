using System;

namespace PagNet.Domain.Entities
{
    public class PROC_PAGNET_CONS_TITULOS_VENCIDOS
    {
        public int CODTITULO { get; set; }
        public int CODFAVORECIDO { get; set; }
        public string NMFAVORECIDO { get; set; }
        public string CPFCNPJ { get; set; }
        public DateTime DATREALPGTO { get; set; }
        public decimal VALTOTAL { get; set; }
        public string BANCO { get; set; }
        public string AGENCIA { get; set; }
        public string CONTACORRENTE { get; set; }
        public string TIPOTITULO { get; set; }
    }
}