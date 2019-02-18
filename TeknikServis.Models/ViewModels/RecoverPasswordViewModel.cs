using System.ComponentModel.DataAnnotations;

namespace TeknikServis.Models.ViewModels
{
    public class RecoverPasswordViewModel
    {
        [StringLength(60, ErrorMessage = "E-Mail 60 karakterden fazla olamaz!")]
        [EmailAddress(ErrorMessage = "Geçersiz E-Mail adresi")]
        public string Email { get; set; }
    }
}