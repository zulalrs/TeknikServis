using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Repository;
using TeknikServis.Models.ViewModels;

namespace TeknikServisWeb.Controllers
{
    public class TeknisyenController : Controller
    {
        // GET: Teknisyen
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult GetAriza()
        {
            var data = new List<UserMarkaModelViewModel>();
            var ariza = new ArizaRepository().GetAll(x => x.ArizaOnaylandiMi == false)
                .ToList();
            foreach (var x in ariza)
            {
                data.Add(new UserMarkaModelViewModel()
                {
                    Id = x.Id,
                    MusteriAdi = x.Musteri.Name + " " + x.Musteri.Surname,
                    Adres = x.Adres,
                    Aciklama = x.Aciklama,
                    //EklemeTarihi = x.EklemeTarihi
                });
            }
            return View(data);
        }



        //[HttpPost]
        //public ActionResult GetAriza(UserMarkaModelViewModel model)
        //{
        //    return View(model);
        //}


        [HttpGet]
        public ActionResult GetArizaDetay()
        {
            var data = new List<UserMarkaModelViewModel>();
            var ariza = new ArizaRepository().GetAll(x => x.ArizaOnaylandiMi == true)
                .ToList();
            foreach (var x in ariza)
            {
                data.Add(new UserMarkaModelViewModel()
                {
                    Id = x.Id,
                    MusteriAdi = x.Musteri.Name + " " + x.Musteri.Surname,
                    Adres = x.Adres,
                    Aciklama = x.Aciklama,
                    ArizaFoto = x.ArizaFoto,
                    GarantiliVarMi = x.GarantiliVarMi,
                    MarkaAdi = x.Model.Marka.MarkaAdi,
                    ModelAdi = x.Model.ModelAdi
                });
            }
            return View(data);

        }
    }
}