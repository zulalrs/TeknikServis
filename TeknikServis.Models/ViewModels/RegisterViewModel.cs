using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServis.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "Lütfen 20 karakteri geçmeyiniz")]
        [Display(Name = "Ad")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Lütfen sadece Harf kullanın")]
        public string Name { get; set; }

        [StringLength(20,ErrorMessage ="Lütfen 20 karakteri geçmeyiniz")]
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Lütfen sadece Harf kullanın")]
        [Display(Name = "Soyad")]
        public string Surname { get; set; }

        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required]
        [StringLength(600, ErrorMessage = "E-Mail 60 karakterden fazla olamaz!")]
        [EmailAddress(ErrorMessage = "Geçersiz E-Mail adresi")]
        public string Email { get; set; }

        [StringLength(200, ErrorMessage ="Lütfen 200 karakteri geçmeyiniz")]
        public string Adress { get; set; }

        [RegularExpression("^[0-9]*$", ErrorMessage = "Telefon numarasi sadece sayilardan olusmalidir")]
        [StringLength(11, ErrorMessage = "Telefon numarasi 11 haneden fazla olamaz")]
        public string Telephone { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Şifreniz en az 6 karakter olmalıdır!")]
        [Display(Name = "Şifre")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre Tekrar")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]

        public string ConfirmPassword { get; set; }
    }
}
