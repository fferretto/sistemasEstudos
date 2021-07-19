
using NetCard.Common.Util;
using System.Linq;
using System.Reflection;

namespace NetCard.Common.Models
{
    public class Permissao : IPermissao
    {
        public string FINCCART { get; set; }
        public string FBLOQCART  { get; set; }
        public string FDESBLOQCART { get; set; }
        public string FCANCCART { get; set; }
        public string FALTLIMITE { get; set; }
        public string FSEGVIACART { get; set; }
        public string FEXTMOV { get; set; } 
        public string FCONSREDE { get; set; }
        public string FLISTTRANSAB  { get; set; }
        public string FLISTCART { get; set; }
        public string FLISTINCCART { get; set; }
        public string FCARGA { get; set; }
        public string FTRANSFSALDO { get; set; }
        public string FTRANSFSALDOCLI { get; set; }

        public string FGERALCONSULTAS { get {
            if (this.FALTLIMITE == null) return Constantes.nao;
                return (
                            FCONSREDE == Constantes.nao && 
                            FEXTMOV == Constantes.nao && 
                            FLISTCART == Constantes.nao && 
                            FLISTINCCART == Constantes.nao) ? Constantes.nao : Constantes.sim;
            }
        }

        public bool TemPermissoes()
        {
            if (this.FALTLIMITE == null) return false;
            return GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Any(p => p.GetValue(this).ToString() == "S");
        }
    }
}
