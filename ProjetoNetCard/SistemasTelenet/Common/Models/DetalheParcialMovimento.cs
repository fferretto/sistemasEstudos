using System.Collections.Generic;

namespace NetCard.Common.Models
{
    public class DetalheParcialMovimento
    {
        public DetalheParcialMovimento()
        {

        }

        public DetalheParcialMovimento(string titnome, string matricula, string cpf, string filial, string setor)
        {
            TitNome = titnome;
            Matricula = matricula;
            Cpf = cpf;
            Filial = filial;
            Setor = setor;
            SubTotal = 0;
            Desconto = 0;
            Detalhes = new List<DetalheMovimento>();
        }

        public DetalheParcialMovimento(string titnome, string matricula, string cpf, string filial, string setor,
                                    decimal subtotal, decimal desconto, List<DetalheMovimento> detalhes)
        {
            TitNome = titnome;
            Matricula = matricula;
            Cpf = cpf;
            Filial = filial;
            Setor = setor;
            SubTotal = subtotal;
            Desconto = desconto;
            Detalhes = detalhes;
        }

        public decimal Premios { get; set; }
        public decimal Receitas { get; set; }
        public short PercentSub { get; set; }
        public decimal Subsidio { get; set; }
        public decimal Taxas { get; set; }
        public decimal Compras { get; set; }
        public decimal Ativacoes { get; set; }
        public decimal Transacoes { get; set; }
        public decimal Total { get; set; }

        public string TitNome { get; set; }
        public string Matricula { get; set; }
        public string Cpf { get; set; }
        public string Filial { get; set; }
        public string Setor { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Desconto { get; set; }
        public void add(DetalheMovimento detalhe)
        {
            Detalhes.Add(detalhe);
        }

        public List<DetalheMovimento> Detalhes { get; set; }
        public List<DetalheParcialMovimento> Parciais { get; set; }
    }
}