using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TeknikServis.Models.Entities;
using TeknikServis.Models.Enums;

namespace TeknikServis.Models.ViewModels
{
    public class ArizaViewModel
    {
        public UserProfileViewModel UserProfileViewModel { get; set; }

        public int Id { get; set; }
        [Required]
        [StringLength(200, ErrorMessage ="Adresiniz 200 karakterden fazla olamaz")]
        [Display(Name = "Adres")]
        public string Adres { get; set; }

        public DateTime ArizaOlusturmaTarihi { get; set; }
        public string ArizaOlusturmaTarihiS { get; set; }
        [Required]
        [StringLength(200, ErrorMessage = "Açıklamanız 200 karakterden fazla olamaz")]
        [Display(Name ="Açıklama")]
        public string Aciklama { get; set; }
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Arıza Başlangıç Tarihi")]
        public DateTime ArizaBaslangicTarihi { get; set; }
        [Display(Name = "Arıza Başlangıç Tarihi")]
        public string ArizaBaslangicTarihiS { get; set; }
        [Column(TypeName = "smalldatetime")]
        [Display(Name = "Arıza Bitiş Tarihi")]
        public DateTime ArizaBitisTarihi { get; set; }
        [Display(Name = "Arıza Bitiş Tarihi")]
        public string ArizaBitisTarihiS { get; set; }
        [Display(Name = "Arıza Onayı")]
        public bool ArizaOnaylandiMi { get; set; }
        public string MusteriId { get; set; }
        [Display(Name = "Müşteri Adı")]
        public string MusteriAdi { get; set; } 
        [Display(Name = "Marka Adı")]
        public string MarkaAdi { get; set; }
        [Display(Name = "Model Adı")]
        public string ModelAdi { get; set; }
        public string TeknisyenId { get; set; }
        [Display(Name = "Teknisyen Adı")]
        public string TeknisyenAdi { get; set; }
        [Display(Name = "Teknisyen Durumu")]
        public TeknisyenDurumu TeknisyenDurumu { get; set; }
        [Display(Name = "Arıza Sonucu")]
        public bool ArizaYapildiMi { get; set; }
        [Display(Name = "Garanti Durumu")]
        public bool GarantiliVarMi { get; set; }
        [Display(Name = "Ücret")]
        public int Ucret { get; set; }
        public int? AnketId { get; set; }
        public int ArizaLogId { get; set; }
        [Display(Name = "Arıza Bildirimleri")]
        public List<ArizaLog> ArizaLoglar { get; set; }
        [Display(Name = "Arıza Fotoğrafları")]
        public List<string> ArizaFotograflari { get; set; }
        [Display(Name = "Arıza Fotoğrafları")]
        public List<HttpPostedFileBase> PostedFile { get; set; }
    }
}
