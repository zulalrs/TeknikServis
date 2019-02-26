using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Models.Abstracts;

namespace TeknikServis.Models.Entities
{
    public class ArizaLog: BaseEntity<int>
    {
        public DateTime Zaman { get; set; }
        public string Aciklama { get; set; }
        public int ArizaId { get; set; }
        [ForeignKey("ArizaId")]
        public virtual Ariza Ariza { get; set; }
    }
}
