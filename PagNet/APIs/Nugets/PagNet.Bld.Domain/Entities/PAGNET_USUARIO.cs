namespace PagNet.Bld.Domain.Entities
{
    public partial class PAGNET_USUARIO_CONCENTRADOR
    {
        public int CODUSUARIO { get; set; }
        public string NMUSUARIO { get; set; }
        public string LOGIN { get; set; }
        public string SENHA { get; set; }
        public string CPF { get; set; }
        public int CODEMPRESA { get; set; }
        public string EMAIL { get; set; }
        public short CODOPE { get; set; }
        public string ADMINISTRADOR { get; set; }
        public string VISIVEL { get; set; }
        public string ATIVO { get; set; }
        public virtual OPERADORA OPERADORAS { get; set; }

    }
}
