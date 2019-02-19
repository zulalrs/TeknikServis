using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TeknikServis.Models.Entities;
using TeknikServis.Models.Enums;

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
        public string ActivationCode { get; set; }
        public string AvatarPath { get; set; }
        public bool TeknisyenBosMu { get; set; } = true;
        
        public TeknisyenDurumu TeknisyenDurumu { get; set; }


        public virtual ICollection<Ariza> Arizalar { get; set; } = new HashSet<Ariza>();

    }
}
