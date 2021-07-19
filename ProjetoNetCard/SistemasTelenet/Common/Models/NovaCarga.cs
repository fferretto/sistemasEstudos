using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCard.Common.Models
{
    public class NovaCarga
    {
        public string Acao { get; set; }

        public List<string> LogRetorno { get; set; }

        public string DataProgramacao { get; set; }

        public string PersisteCpf { get; set; }

        public bool Processando { get; set; }

        public bool CargaCsv { get; set; }

        public bool ArquivoCsvValido { get; set; }

        public bool SubmissaoArquivo { get; set; }

        public NovaCarga()
        {
            LogRetorno = new List<string>();
        }
        
    }
}
