using System.ComponentModel.DataAnnotations;

namespace NetCard.Common.Models
{
    public class TermoConsentimento
    {
        [Display(Name = "Declaro que li e aceito o termo de consentimento.")]
        public bool Aceite { get; set; }
    }
}
