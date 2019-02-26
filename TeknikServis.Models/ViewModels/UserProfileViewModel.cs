using System.ComponentModel.DataAnnotations;
using System.Web;

namespace TeknikServis.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Üye adı 20 karakterden fazla olamaz!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Lütfen sadece Harf kullanın")]
        [Display(Name = "Ad")]
        public string Name { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Üye Soyadı 20 karakterden fazla olamaz!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Lütfen sadece Harf kullanın")]
        [Display(Name = "Soyad")]
        public string Surname { get; set; }

        [Required]
        [StringLength(20, ErrorMessage = "Üye Soyadı 20 karakterden fazla olamaz!")]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Lütfen sadece Harf kullanın")]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }

        [Required]
        [StringLength(60, ErrorMessage = "E-Mail 60 karakterden fazla olamaz!")]
        [EmailAddress(ErrorMessage = "Geçersiz E-Mail adresi")]
        public string Email { get; set; }

        [Display(Name = "Telefon No")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Telefon sadece sayılardan oluşmalıdır")]
        [StringLength(11, ErrorMessage = "telefon 11 karakterden fazla olamaz!")]
        public string PhoneNumber { get; set; }

        public string AvatarPath { get; set; }
        [Display(Name = "Arıza Fotoğrafları")]
        public HttpPostedFileBase PostedFile { get; set; }
    }
}
