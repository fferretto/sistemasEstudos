using PagNet.Api.Service.Interface.Model;

namespace PagNet.Api.Service.Model
{
    public class APIRetornoDDLModel : IAPIRetornoDDLModel
    {
        public string Valor { get; set; }
        public string Descricao { get; set; }
        public string Title { get; set; }
    }
    public class RetornoModel
    {
        public bool Sucesso { get; set; }
        public string msgResultado { get; set; }
    }
}
