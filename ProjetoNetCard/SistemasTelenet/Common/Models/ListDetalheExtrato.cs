using System.Collections.Generic;

namespace NetCard.Common.Models
{
    public class ListDetalheMovimento
    {
        public ListDetalheMovimento()
        {
        }

        public ListDetalheMovimento(List<DetalheMovimento> detalhes)
        {
            Detalhes = detalhes;

            foreach (DetalheMovimento d in Detalhes)
            {
                Total += d.Valor;
            }
        }

        public List<DetalheMovimento> Detalhes { get; set; }

        public decimal Total { get; set; }
    }
}