using System;
using System.ComponentModel.DataAnnotations;

namespace NetCard.Common.Models
{
    public class AdesaoCB
    {
        public string Inicio { get; set; }
        public string Fim { get; set; }

        [Display(Name = "Declaro que li o regulamento e aceito participar.")]
        public bool Aceite { get; set; }
    }
}

