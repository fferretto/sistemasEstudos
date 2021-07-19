using System.Collections.Generic;

namespace NetCard.Common.Models
{
    public class TotalComprasAberto
    {
        public decimal TotalValor { get; set; }
        public string TitNome { get; set; }
        public string Matricula { get; set; }
        public string Cpf { get; set; }
        public string Filial { get; set; }
        public string Setor { get; set; }
        public List<DetalheMovimento> Detalhe { get; set; } 
    }
}
