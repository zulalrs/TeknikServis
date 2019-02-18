using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Repository;
using TeknikServis.Models.Entities;
using TeknikServis.Models.ViewModels;
using static TeknikServis.BLL.Identity.MembershipTools;

namespace TeknikServisWeb.Controllers
{
    public class MusteriController : Controller
    {
        // GET: Musteri
        public ActionResult ArizaBildirimi()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArizaBildirimi(UserMarkaModelViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var marka = new Marka()
                {
                    MarkaAdi = model.MarkaModelViewModel.Marka
                };
                new MarkaRepository().Insert(marka);
                var markaId = marka.Id;
                var markaModel = new Model()
                {
                    ModelAdi = model.MarkaModelViewModel.Model,
                   MarkaId=markaId
                };
                new ModelRepository().Insert(markaModel);
                new MarkaRepository().Update(marka);
                var markaModelId = markaModel.Id;
                var ariza = new Ariza()
                {
                    MusteriId = HttpContext.User.Identity.GetUserId(),
                    Aciklama = model.Description,
                    MarkaId = markaId,
                    ModelId = markaModelId,
                    Adres = model.Adress,

                };
                new ArizaRepository().Insert(ariza);
                TempData["Message"] = "Kaydınız alınlıştır";
                return RedirectToAction("ArizaBildirimi","Musteri");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "ArizaBildirimi",
                    ControllerName = "Musteri",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }      
        }
    }
}