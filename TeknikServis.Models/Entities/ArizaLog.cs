using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Models.Abstracts;

namespace TeknikServis.Models.Entities
{
    public class ArizaLog:RepositoryBase<Guid>
    {
        public ArizaLog()
        {
            Id = Guid.NewGuid();
        }

        public DateTime? ArizaOlusturmaTarihi { get; set; }
        public string MusteriAdi { get; set; }
        public string MusteriId { get; set; }
        public bool ArizaOnaylandiMi { get; set; }
        public DateTime? ArizaBaslangicTarihi { get; set; }
        public string TeknisyenAdi { get; set; }
        public DateTime? ArizaBitisTarihi { get; set; }

    }
}
