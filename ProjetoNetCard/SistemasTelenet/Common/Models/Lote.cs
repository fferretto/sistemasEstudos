using System;

namespace NetCard.Common.Models
{
    public class Lote
    {
        public Lote(int codigo, decimal valorafe, int quantafe, decimal taxa, decimal valtaxa, decimal valorliq, DateTime dinicio, DateTime dfim,
                           int nfechamento, string datafech, DateTime datapag)
        {
            Codigo = codigo;
            ValorAfe = valorafe;
            QuantidadeAfe = quantafe;
            Taxa = taxa;
            ValTaxa = valtaxa;
            Valor = String.Format("{0:0.00}", valorliq);
            DataInicio = dinicio.ToShortDateString();
            DataFim = dfim.ToShortDateString();
            NroFechamento = nfechamento;
            DataFech = datafech;
            DataPag = datapag.ToShortDateString();
        }

        public Lote(int codigo, decimal valorafe, int quantafe, decimal taxa, decimal valtaxa, decimal valorliq, DateTime dinicio, DateTime dfim,
                           int nfechamento, string datafech)
        {
            Codigo = codigo;
            ValorAfe = valorafe;
            QuantidadeAfe = quantafe;
            Taxa = taxa;
            ValTaxa = valtaxa;
            Valor = String.Format("{0:0.00}", valorliq);
            DataInicio = dinicio.ToShortDateString();
            DataFim = dfim.ToShortDateString();
            NroFechamento = nfechamento;
            DataFech = datafech;            
        }

        public int Codigo { get; set; }
        public decimal ValorAfe { get; set; }
        public int QuantidadeAfe { get; set; }
        public decimal Taxa { get; set; }
        public decimal ValTaxa { get; set; }
        public string Valor { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }
        public int NroFechamento { get; set; }
        public string DataFech { get; set; }
        public string DataPag { get; set; }
    }
}
