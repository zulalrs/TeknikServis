using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServis.Models.IdentityModels
{
    public class User:IdentityUser
    { 
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [StringLength(60)]
        [Required]
        public string Surname { get; set; }
        [StringLength(100)]
        public string Adress { get; set; }
        public string ActivationCode { get; set; }

    }
}
