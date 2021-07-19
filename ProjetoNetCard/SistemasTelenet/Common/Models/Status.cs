using System.Collections.Generic;

namespace NetCard.Common.Models
{
    public class Status
    {
        public string codSta { get; set; }
        public string desSta { get; set; }

        public Status(string _codSta, string _desSta)
        {
            codSta = _codSta;
            desSta = _desSta;
        }

        public List<Status> ListaStatus()
        {
            var listaStatus = new List<Status>();
            listaStatus.Add(new Status("00", "Ativo"));
            listaStatus.Add(new Status("01", "Bloqueado"));
            listaStatus.Add(new Status("02", "Cancelado"));
            listaStatus.Add(new Status("05", "Ativo sem renovação"));
            listaStatus.Add(new Status("06", "Suspenso"));
            listaStatus.Add(new Status("07", "Previ Saude"));
            return listaStatus;
        }

        public List<Status> ListaStatusCartao()
        {
            var listaStatus = new List<Status>();
            listaStatus.Add(new Status("00", "Ativo"));
            listaStatus.Add(new Status("01", "Bloqueado"));
            listaStatus.Add(new Status("02", "Cancelado"));
            return listaStatus;
        }
    }
}
