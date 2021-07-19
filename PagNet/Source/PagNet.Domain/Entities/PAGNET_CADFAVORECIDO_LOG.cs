using System;


namespace PagNet.Domain.Entities
{
    public partial class PAGNET_CADFAVORECIDO_LOG
    {
        public int CODFAVORECIDO_LOG { get; set; }
        public int CODFAVORECIDO { get; set; }
        public string NMFAVORECIDO { get; set; }
        public string CPFCNPJ { get; set; }
        public int? CODCEN { get; set; }
        public string BANCO { get; set; }
        public string AGENCIA { get; set; }
        public string DVAGENCIA { get; set; }
        public string OPE { get; set; }
        public string CONTACORRENTE { get; set; }
        public string DVCONTACORRENTE { get; set; }
        public string CEP { get; set; }
        public string LOGRADOURO { get; set; }
        public string NROLOGRADOURO { get; set; }
        public string COMPLEMENTO { get; set; }
        public string BAIRRO { get; set; }
        public string CIDADE { get; set; }
        public string UF { get; set; }     
        public string ATIVO { get; set; }
        public int CODUSUARIO { get; set; }
        public DateTime DATINCLOG { get; set; }
        public string DESCLOG { get; set; }
    }
}
