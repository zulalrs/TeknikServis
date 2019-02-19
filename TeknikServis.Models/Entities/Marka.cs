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
    [Table("Markalar")]
    public class Marka:RepositoryBase<int>
    {
        [StringLength(70)]
        public string MarkaAdi { get; set; }

        public virtual ICollection<Model> Modeller { get; set; } = new HashSet<Model>();
       
    }
}
