using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TeknikServis.BLL.Helpers;
using TeknikServis.BLL.Repository;
using TeknikServis.Models.Entities;
using TeknikServis.Models.Enums;
using TeknikServis.Models.ViewModels;
using static TeknikServis.BLL.Identity.MembershipTools;

namespace TeknikServisWeb.Controllers
{
    public class MusteriController : Controller
    {
        // GET: Musteri
        public ActionResult Index()
        {
            return View();
        }
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
                    MarkaAdi = model.MarkaModelViewModel.MarkaViewModel.Marka
                };
                new MarkaRepository().Insert(marka);
                var markaId = marka.Id;
                var markaModel = new Model()
                {
                    ModelAdi = model.MarkaModelViewModel.ModelViewModel.Model,
                    MarkaId = markaId
                };
                new ModelRepository().Insert(markaModel);
                new MarkaRepository().Update(marka);
                var markaModelId = markaModel.Id;
                var ariza = new Ariza()
                {
                    MusteriId = HttpContext.User.Identity.GetUserId(),
                    Aciklama = model.Aciklama,
                    MarkaId = markaId,
                    ModelId = markaModelId,
                    Adres = model.Adres,

                };
                new ArizaRepository().Insert(ariza);
                if (model.PostedFile.Count > 0)
                {
                    model.PostedFile.ForEach(file =>
                    {
                        if (file != null && file.ContentLength > 0)
                        {

                            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                            string extName = Path.GetExtension(file.FileName);
                            fileName = StringHelpers.UrlFormatConverter(fileName);
                            fileName += StringHelpers.GetCode();
                            var klasoryolu = Server.MapPath("~/Upload/");
                            var dosyayolu = Server.MapPath("~/Upload/") + fileName + extName;

                            if (!Directory.Exists(klasoryolu))
                                Directory.CreateDirectory(klasoryolu);
                            file.SaveAs(dosyayolu);

                            WebImage img = new WebImage(dosyayolu);
                            img.Resize(250, 250, false);
                            img.Save(dosyayolu);

                            new FotografRepository().Insert(new Fotograf()
                            {
                                ArizaId = ariza.Id,
                                Yol = "/Upload/" + fileName + extName
                            });
                        }
                    });
                }
                new ArizaRepository().Update(ariza);
                TempData["Message"] = "Kaydınız alınlıştır";
                return RedirectToAction("ArizaBildirimi", "Musteri");
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

        [HttpGet]
        public ActionResult KayitliArizalar()
        {
            var id = HttpContext.User.Identity.GetUserId();
            var data = new List<UserMarkaModelViewModel>();
            var ariza = new ArizaRepository().GetAll(x => x.MusteriId == id).ToList();
            foreach (var x in ariza)
            {
                data.Add(new UserMarkaModelViewModel()
                {
                    Id = x.Id,
                    //ArizaBaslangicTarihi = x.ArizaBaslangicTarihi,
                    TeknisyenAdi = x.Teknisyen?.Name + " " + x.Teknisyen?.Surname,
                    TeknisyenDurumu = x.Teknisyen?.TeknisyenDurumu == null ? TeknisyenDurumu.Beklemede : x.Teknisyen.TeknisyenDurumu,
                    ArizaOnaylandiMi = x.ArizaOnaylandiMi,
                    Ucret = x.Ucret
                });
            }
            return View(data);
        }
    }
}