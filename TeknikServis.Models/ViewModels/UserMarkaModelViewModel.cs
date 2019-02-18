using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServis.Models.ViewModels
{
    public class UserMarkaModelViewModel
    {
        public UserProfileViewModel UserProfileViewModel { get; set; }
        public MarkaModelViewModel MarkaModelViewModel { get; set; }

        [Required]
        [StringLength(200, ErrorMessage ="Adresiniz 200 karakterden fazla olamaz")]
        [Display(Name = "Adres")]
        public string Adress { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Açıklamanız 200 karakterden fazla olamaz")]
        [Display(Name ="Açıklama")]
        public string Description { get; set; }
    }
}
