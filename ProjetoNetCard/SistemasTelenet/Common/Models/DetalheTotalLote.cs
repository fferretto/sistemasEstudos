using System.Collections.Generic;

namespace NetCard.Common.Models
{
    public class DetalheTotalLote
    {
        public DetalheTotalLote()
        {
            Parciais = new List<DetalheParcialLote>();
        }

        public DetalheTotalLote(decimal totalvalinf, int totalquantinf,
            decimal totalvalafe, int totalquantafe, decimal totalvalorliq, List<DetalheParcialLote> parciais)
        {
            TotalValInf = totalvalinf;
            TotalQuantInf = totalquantinf;
            TotalValAfe = totalvalafe;
            TotalQuantAfe = totalquantafe;
            TotalValorLiq = totalvalorliq;
            Parciais = parciais;
        }

        public decimal TotalValInf { get; set; }
        public int TotalQuantInf { get; set; }
        public decimal TotalValAfe { get; set; }
        public int TotalQuantAfe { get; set; }
        public decimal TotalValorLiq { get; set; }

        public void add(DetalheParcialLote parcial)
        {
            Parciais.Add(parcial);
        }

        public List<DetalheParcialLote> Parciais { get; set; }
    }
}
