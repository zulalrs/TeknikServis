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
        [StringLength(200)]
        public string Adress { get; set; }
        [Required]
        [StringLength(200)]
        public string Description { get; set; }

        public string ArizaFoto { get; set; }
    }
}
