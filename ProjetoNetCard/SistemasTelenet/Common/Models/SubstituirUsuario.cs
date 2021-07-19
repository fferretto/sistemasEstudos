using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCard.Common.Models
{
    public class SubstituirUsuario
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public string CpfOrigem { get; set; }
        public string CpfDestino { get; set; }
        public string CartaoOrigem { get; set; }
        public string CartaoDestino { get; set; }
        public Decimal SaldoOrigem { get; set; }
        public Decimal SaldoDestino { get; set; }
        public string SaldoFinalOrigem { get; set; }
        public string SaldoFinalDestino { get; set; }
        public string NomeCliente { get; set; }
        public string CodigoCliente { get; set; }
    }
}
