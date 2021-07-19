using System;
using System.Collections.Generic;
using System.Text;

namespace PagNet.Bld.Domain.Entities
{
    public class PROC_PAGNET_CONS_FATURAS_BORDERO
    {
        public int CODFATURA { get; set; }
        public int CODCLIENTE { get; set; }
        public string NMCLIENTE { get; set; }
        public string CPFCNPJ { get; set; }
        public DateTime DATVENCIMENTO { get; set; }
        public int QTFATURAMENTO { get; set; }
        public decimal VALOR { get; set; }
    }
}