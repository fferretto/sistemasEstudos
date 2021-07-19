using System;

namespace PagNet.Domain.Entities
{
    public class PROC_PAGNET_CONSULTA_TITULOS
    {
        public int CODTITULO { get; set; }
        public int CODFAVORECIDO { get; set; }
        public string NMFAVORECIDO { get; set; }
        public string STATUS { get; set; }
        public string CPFCNPJ { get; set; }
        public DateTime DATEMISSAO { get; set; }
        public DateTime DATPGTO { get; set; }
        public DateTime DATREALPGTO { get; set; }
        public string BANCO { get; set; }
        public string AGENCIA { get; set; }
        public string CONTACORRENTE { get; set; }
        public decimal VALBRUTO { get; set; }
        public decimal VALTAXA { get; set; }
        public decimal VALLIQ { get; set; }
        public decimal VALTOTAL { get; set; }
        public string TIPCARTAO { get; set; }
        public string TIPOTITULO { get; set; }
        public string MSGRETORNO { get; set; }
    }
}