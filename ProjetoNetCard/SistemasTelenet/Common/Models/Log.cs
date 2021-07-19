using System.Collections.Generic;

namespace NetCard.Common.Models
{
    public class Log
    {
        public Log() { ListaLog = new List<Log>(); }
        public Log(string id, string tipo, string descricao)
        {
            ListaLog = new List<Log>();
            Id = id;
            Tipo = tipo;
            DescricaoLog = descricao;            
        }
        public string Id { get; set; }
        public string Tipo { get; set; }
        public string DescricaoLog { get; set; }
        private List<Log> ListaLog { get; set; }
        public void AddLog(Log log) { ListaLog.Add(log); }
        public List<Log> getArquivoLog() { return ListaLog; }
        public void removeRecordOBS()
        {
            if (ListaLog == null) return;
            var count = ListaLog.Count;
            while (count > 0)
            {
                ListaLog.Remove(ListaLog.Find(x => x.Tipo == "OBS"));
                count--;
            }
        }
    }
}
