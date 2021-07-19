using NetCard.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCard.Common.Models
{
    public class CartaoFidelidade
    {
        public string CodCrt { get; set; }
        public string CodCrtMask { get { return CodCrt != null ? Utils.MascaraCartao(CodCrt, 17) : string.Empty; } }
        public string Rotulo { get; set; }
        public string LabelCartao { get { return CodCrt != null ? Rotulo + " - " + Utils.MascaraCartao(CodCrt, 17) : string.Empty; } }
    }
}
