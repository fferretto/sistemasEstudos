using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetCard.Common.Models
{
    public class DetalhesBeneficio
    {
        

        public DetalhesBeneficio()
        {
            BeneficiosCliente = new List<Beneficios>();
            BeneficiosUsuario = new List<Beneficios>();
            Mensagens = new List<string>();
            Usuario = new Usuario();
        }

        public List<string> Mensagens = new List<string>();
        public bool ExibirMensagem { get; set; }

        
        public List<Beneficios> BeneficiosCliente { get; set; }
        public List<Beneficios> BeneficiosUsuario { get; set; }
        public Usuario Usuario { get; set; }

        public string NomeBeneficio{ get; set; }
        public string Codigo { get; set; }
        public bool Compulsorio { get; set; }
        public DateTime DataAssociacao { get; set; }
        public bool AssocOutroCliente { get; set; }


        public class Beneficios
        {
            public string Codigo { get; set; }
            public string Nome { get; set; }
            public string Compulsorio { get; set; }
            public string Vigencia { get; set; }
            public string DataAssociacao { get; set; }
            public string DataBeneficio { get; set; }
            public string ValorTitular { get; set; }
            public string AssociadoOutroCliente { get; set; }
        }
    }
}
