using System.Collections.Generic;

namespace NetCard.Common.Models
{
    public class DetalheTotalTransacao
    {
        public DetalheTotalTransacao()
        {
            Parciais = new List<DetalheParcialTransacao>();
        }

        public DetalheTotalTransacao(decimal totalvalor, decimal totalvalorliq, List<DetalheParcialTransacao> parciais)
        {
            TotalValor = totalvalor;
            TotalValorLiq = totalvalorliq;
            Parciais = parciais;
        }

        public decimal TotalValor { get; set; }
        public decimal TotalValorLiq { get; set; }
        public void add(DetalheParcialTransacao parcial)
        {
            Parciais.Add(parcial);
        }

        public List<DetalheParcialTransacao> Parciais { get; set; }
    }
}