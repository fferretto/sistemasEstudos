namespace NetCard.Common.Models
{
    public class Acao
    {
        public Acao(string texto, string mensagem, string id, string tag)
        {
            Texto = texto;
            Mensagem = mensagem;
            Id = id;
            Tag = tag;
        }
        public string Texto { get; set; }
        public string Mensagem { get; set; }
        public string Id { get; set; }
        public string Tag { get; set; }
    }
}
