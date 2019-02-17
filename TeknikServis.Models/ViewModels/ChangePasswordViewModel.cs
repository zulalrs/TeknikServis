using System.ComponentModel.DataAnnotations;

namespace TeknikServis.Models.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Şifreniz en az 6 karakter olmalıdır!")]
        [Display(Name = "Eski Şifre")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Şifreniz en az 6 karakter olmalıdır!")]
        [Display(Name = "Yeni Şifre")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni Şifre Tekrar")]
        [Compare("NewPassword", ErrorMessage = "Şifreleriniz uyuşmuyor")]
        public string ConfirmNewPassword { get; set; }
    }
}
