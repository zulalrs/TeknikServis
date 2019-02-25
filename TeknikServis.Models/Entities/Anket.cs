using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Models.Abstracts;
using TeknikServis.Models.Enums;

namespace TeknikServis.Models.Entities
{
    [Table("Anketler")]
    public class Anket : RepositoryBase<int>
    {

        [DisplayName("Firma Memnuniyeti")]
        public double Soru1 { get; set; } = 0;
        [DisplayName("Nezakaket")]
        public double Soru2 { get; set; } = 0;
        [DisplayName("Teknik Bilgi ve Yeterlilik")]
        public double Soru3 { get; set; } = 0;
        [DisplayName("Hız")]
        public double Soru4 { get; set; } = 0;
        [DisplayName("Fiyat")]
        public double Soru5 { get; set; } = 0;
        [DisplayName("Görüş ve Öneriler")]
        [StringLength(500, ErrorMessage = "Max 200 karakter giriniz.")]
        public string Soru6 { get; set; }

        public virtual ICollection<Ariza> Arizalar { get; set; } = new HashSet<Ariza>();
    }
}
