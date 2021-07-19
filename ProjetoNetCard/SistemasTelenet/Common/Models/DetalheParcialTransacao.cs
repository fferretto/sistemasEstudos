using System.Collections.Generic;

namespace NetCard.Common.Models
{
    public class DetalheParcialTransacao
    {
        public DetalheParcialTransacao()
        {

        }

        public DetalheParcialTransacao(string razsoc, int codcre)
        {
            RazSoc = razsoc;
            CodCre = codcre;
            SubTotalValor = 0;
            SubTotalValorLiq = 0;
            Detalhes = new List<Transacao>();
        }

        public DetalheParcialTransacao(string razsoc, int codcre, string cnpj)
        {
            RazSoc = razsoc;
            CodCre = codcre;
            Cnpj = cnpj;
            SubTotalValor = 0;
            SubTotalValorLiq = 0;
            Detalhes = new List<Transacao>();
        }

        public DetalheParcialTransacao(string razsoc, int codcre, decimal subtotalvalor, decimal subtotalvalorliq, List<Transacao> detalhes)
        {
            RazSoc = razsoc;
            CodCre = codcre;
            SubTotalValor = subtotalvalor;
            SubTotalValorLiq = subtotalvalorliq;
            Detalhes = detalhes;
        }

        public string RazSoc { get; set; }
        public int CodCre { get; set; }
        public string Cnpj { get; set; }
        public decimal SubTotalValor { get; set; }
        public decimal SubTotalValorLiq { get; set; }

        public void add(Transacao detalhe)
        {
            Detalhes.Add(detalhe);
        }

        public List<Transacao> Detalhes { get; set; }
    }
}