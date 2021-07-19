namespace NetCard.Common.Models
{
    public class Filtro
    {
        public Filtro(string _nome, string _serie)
        {
            Nome = _nome;
            Serie = _serie;
        }

        public Filtro()
        {

        }

        public string Nome { get; set; }
        public string Serie { get; set; }
    }

    
}
