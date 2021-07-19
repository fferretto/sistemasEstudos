using System.Collections.Generic;

namespace NetCard.Common.Models
{
    public class DetalheParcialLote
    {
        public DetalheParcialLote()
        {

        }

        public DetalheParcialLote(string razsoc, int codcre)
        {
            RazSoc = razsoc;
            CodCre = codcre;
            SubTotalValInf = 0;
            SubTotalQuantInf = 0;
            SubTotalValAfe = 0;
            SubTotalQuantAfe = 0;
            SubTotalValorLiq = 0;
            Detalhes = new List<Lote>();
        }

        public DetalheParcialLote(string razsoc, int codcre, string cnpj)
        {
            RazSoc = razsoc;
            CodCre = codcre;
            Cnpj = cnpj;
            SubTotalValInf = 0;
            SubTotalQuantInf = 0;
            SubTotalValAfe = 0;
            SubTotalQuantAfe = 0;
            SubTotalValorLiq = 0;
            Detalhes = new List<Lote>();
        }

        public DetalheParcialLote(string razsoc, int codcre, decimal subtotalvalinf, int subtotalquantinf,
            decimal subtotalvalafe, int subtotalquantafe, decimal subtotalvalorliq, List<Lote> detalhes)
        {
            RazSoc = razsoc;
            CodCre = codcre;
            SubTotalValInf = subtotalvalinf;
            SubTotalQuantInf = subtotalquantinf;
            SubTotalValAfe = subtotalvalafe;
            SubTotalQuantAfe = subtotalquantafe;
            SubTotalValorLiq = subtotalvalorliq;
            Detalhes = detalhes;
        }

        public string RazSoc { get; set; }
        public int CodCre { get; set; }
        public string Cnpj { get; set; }
        public decimal SubTotalValInf { get; set; }
        public int SubTotalQuantInf { get; set; }
        public decimal SubTotalValAfe { get; set; }
        public int SubTotalQuantAfe { get; set; }
        public decimal SubTotalValorLiq { get; set; }

        public void add(Lote detalhe)
        {
            Detalhes.Add(detalhe);
        }

        public List<Lote> Detalhes { get; set; }
    }
}
