using System;

namespace PagNet.BLD.ProjetoPadrao.Domain.Entities
{
    public class PROC_PAGNET_CONS_BORDERO
    {
        public int CODBORDERO { get; set; }
        public decimal VLBORDERO { get; set; }
        public DateTime DTBORDERO { get; set; }
        public int CODEMPRESA { get; set; }
        public int CODUSUARIO { get; set; }
        public string STATUS { get; set; }
        public string TITULOVENCIDO { get; set; }
    }
}