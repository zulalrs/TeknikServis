using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServis.Models.ViewModels
{
    public class AnketViewModel
    {
        public int AnketId { get; set; }
        public double GenelMemnuniyet { get; set; } = 0;
        public double Hız { get; set; } = 0;
        public double Fiyat { get; set; } = 0;
        public double Teknisyen { get; set; } = 0;
        public double CozumOdaklilik { get; set; } = 0;
        [StringLength(500)]
        public string Gorus { get; set; }
    }
}
