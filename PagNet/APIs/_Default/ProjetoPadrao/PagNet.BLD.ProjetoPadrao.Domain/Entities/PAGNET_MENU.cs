namespace PagNet.BLD.ProjetoPadrao.Domain.Entities
{
    public partial class PAGNET_MENU
    {
        public int codMenu { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int? codMenuPai { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public int Nivel { get; set; }
        public int Ordem { get; set; }
        public string Ativo { get; set; }
        public string favIcon { get; set; }
        //public string ExclusivoUsuNC { get; set; }

    }
}
