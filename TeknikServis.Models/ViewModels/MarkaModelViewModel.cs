using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServis.Models.ViewModels
{
    public class MarkaModelViewModel
    {
        [Required]
        [StringLength(50, ErrorMessage = "Marka adı 50 karakterden fazla olamaz")]
        public string Marka { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Model adı 50 karakterden fazla olamaz")]
        public string Model { get; set; }

    }
}
