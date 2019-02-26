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
    [Table("Fotograflar")]
    public class Fotograf: BaseEntity<int>
    {
       
        [Required]
        public string Yol { get; set; }
        public int ArizaId { get; set; }
        [ForeignKey("ArizaId")]
        public virtual Ariza Ariza { get; set; }
    }
}
