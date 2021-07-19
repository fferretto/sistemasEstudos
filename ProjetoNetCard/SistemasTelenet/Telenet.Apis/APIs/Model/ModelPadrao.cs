namespace Telenet.Apis.APIs.Model
{
    public class APIRetornoDDLModel
    {
        public string Valor { get; set; }
        public string Descricao { get; set; }
        public string Title { get; set; }
    }
    public class APIRetornoModel
    {
        public bool Sucesso { get; set; }
        public string msgResultado { get; set; }
    }
}