using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCard.Common.Models
{
    public class TransferenciaSaldo
    {
        public int TipoTransferencia { get; set; }
        public string NomeCliente { get; set; }
        public string SaldoCliente { get; set; }
        public bool CartaoVoucher { get; set; }
    }
}
