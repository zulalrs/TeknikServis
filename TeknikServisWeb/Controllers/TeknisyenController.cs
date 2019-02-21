using Microsoft.AspNet.Identity;
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
            var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
            
            var data = new List<UserMarkaModelViewModel>();
            var ariza = new ArizaRepository().GetAll(x => x.TeknisyenId==id)
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
        public ActionResult GetArizaDetay(int id)
        {
            var ariza = new ArizaRepository().GetAll().FirstOrDefault(x => x.Id == id);

            var data = new UserMarkaModelViewModel()
            {
                Id = ariza.Id,
                MusteriAdi = ariza.Musteri.Name + " " + ariza.Musteri.Surname,
                Adres = ariza.Adres,
                Aciklama = ariza.Aciklama,
                GarantiliVarMi = ariza.GarantiliVarMi,
                MarkaAdi = ariza.Model.Marka.MarkaAdi,
                ModelAdi = ariza.Model.ModelAdi,
                ArizaFotograflari = ariza.Fotograflar.Select(y => y.Yol).ToList(),
                ArizaOlusturmaTarihi = ariza.ArizaOlusturmaTarihi,
                ArizaYapildiMi = ariza.ArizaYapildiMi,
               //TeknisyenDurumu = ariza.Teknisyen.TeknisyenDurumu,

            };

            return View(data);

        }
    }
}