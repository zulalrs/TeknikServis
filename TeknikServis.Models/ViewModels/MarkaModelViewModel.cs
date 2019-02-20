using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Models.Entities;

namespace TeknikServis.Models.ViewModels
{
    public class MarkaModelViewModel
    {
        public MarkaViewModel MarkaViewModel { get;set;}
        public ModelViewModel ModelViewModel { get; set; }
       
        
    }
    public class MarkaViewModel
    {
        public int MarkaId { get; set; }
    
        [StringLength(50, ErrorMessage = "Marka adı 50 karakterden fazla olamaz")]
        public string Marka { get; set; }
    }
    public class ModelViewModel
    {
        public int ModelId { get; set; }
      
        [StringLength(50, ErrorMessage = "Model adı 50 karakterden fazla olamaz")]
        public string Model { get; set; }
        public string MarkaId { get; set; }
    }
}
