using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCard.Common.Models
{
    public class LoteSubRede
    {
        public int CodSubRede { get; set; }
        public string Descricao {get; set;}
        public int Qtde {get; set;}
        public decimal Valor {get; set;}                
        public decimal Taxa {get; set;}
        public decimal ValLiq {get; set;}
        public int CodCre {get; set;}
        public int NumFech {get; set;}
    }
}
