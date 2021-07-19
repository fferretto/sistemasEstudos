using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCard.Common.Models
{
    public class DetalheTotalMovimento
    {

        [Display(Name = "CPF Titular")]
        public string Cpf { get; set; }

        public DetalheTotalMovimento()
        {
            Parciais = new List<DetalheParcialMovimento>();
        }

        public DetalheTotalMovimento(decimal premios, decimal receitas, decimal taxas, decimal compras,
                                      decimal ativacoes, decimal transacoes, decimal total, decimal desconto,
                                      List<DetalheParcialMovimento> parciais)
        {
            Premios = premios;
            Receitas = receitas;
            Taxas = taxas;
            Compras = compras;
            Ativacoes = ativacoes;
            Transacoes = transacoes;
            Total = total;
            Desconto = desconto;
            Parciais = parciais;
        }

        public decimal Limite { get; set; }
        public decimal Premios { get; set; }
        public decimal Receitas { get; set; }
        public short PercentSub { get; set; }
        public decimal Subsidio { get; set; }
        public decimal Taxas { get; set; }
        public decimal Compras { get; set; }
        public decimal Ativacoes { get; set; }
        public decimal Transacoes { get; set; }
        public decimal Total { get; set; }
        public decimal Desconto { get; set; }
        public decimal Utilizado { get; set; }
        public string MsgAnuidade { get; set; }
        public void add(DetalheParcialMovimento parcial)
        {
            Parciais.Add(parcial);
        }

        public List<DetalheParcialMovimento> Parciais { get; set; }
    }
}
