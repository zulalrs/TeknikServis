using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Models.Enums;

namespace TeknikServis.Models.ViewModels
{
    public class UserMarkaModelViewModel
    {
        //public UserProfileViewModel UserProfileViewModel { get; set; }
        public MarkaModelViewModel MarkaModelViewModel { get; set; }

        public int Id { get; set; }
        [Required]
        [StringLength(200, ErrorMessage ="Adresiniz 200 karakterden fazla olamaz")]
        [Display(Name = "Adres")]
        public string Adres { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Açıklamanız 200 karakterden fazla olamaz")]
        [Display(Name ="Açıklama")]
        public string Aciklama { get; set; }
        public string ArizaFoto { get; set; }
        [Column(TypeName = "smalldatetime")]
        public DateTime EklemeTarihi { get; set; } = DateTime.Now;
        public bool ArizaOnaylandiMi { get; set; }
        public string MusteriId { get; set; }
        public string MusteriAdi { get; set; }
        public int MarkaId { get; set; }
        public string MarkaAdi { get; set; }
        public int ModelId { get; set; }
        public string ModelAdi { get; set; }
        public string TeknisyenId { get; set; }
        public string Teknisyen { get; set; }
        public TeknisyenDurumu TeknisyenDurumu { get; set; }
        public bool TeknisyenBosMu { get; set; }
        public bool ArizaYapildiMi { get; set; }
        public bool GarantiliVarMi { get; set; }
        public int Ucret { get; set; }
    }
}
