namespace NetCard.Common.Models
{
    public class TrocaCpf
    {
        public bool Sucesso { get; set; }
        public string Mensagem { get; set; }
        public string CpfTemp { get; set; }
        public string CpfNovo { get; set; }
        public string CrtTemp { get; set; }
        public string CrtNovo { get; set; }
        public string StatusCrtNovo { get; set; }
        public string NomeUsuNovo { get; set; }
    }
}
