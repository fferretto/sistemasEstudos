using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCard.Common.Models
{
    public class DesbloqEmMassa
    {
        public string DataInclusaoInicial { get; set; }
        public string DataInclusaoFinal { get; set; }
        public string Cartoes { get; set; }
        public string Mensagem { get; set; }
        public string MensagemPesquisa { get; set; }

        public List<Cartao> ListaCartoes { get; set; }

        public DesbloqEmMassa()
        {
            ListaCartoes = new List<Cartao>();
        }


    }
}
