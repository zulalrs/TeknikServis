using System;
using System.Collections.Generic;
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
        public double GenelMemnuniyet { get; set; } = 0;
        public double Hız { get; set; } = 0;
        public double Fiyat { get; set; } = 0;
        public double Teknisyen { get; set; } = 0;
        public double CozumOdaklilik { get; set; } = 0;
        [StringLength(500)]
        public string Gorus { get; set; }

        public virtual ICollection<Ariza> Arizalar { get; set; } = new HashSet<Ariza>();
    }
}
