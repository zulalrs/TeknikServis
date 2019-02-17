using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Models.Abstracts;

namespace TeknikServis.Models.Entities
{
    [Table("Modeller")]
    public class Model:BaseEntity<int>
    {
        public string ModelAdi { get; set; }
        public int MarkaId { get; set; }

        [ForeignKey("MarkaId")]
        public virtual Marka Marka { get; set; }

        public virtual ICollection<Ariza> Arizalar { get; set; } = new HashSet<Ariza>();
    }
}
