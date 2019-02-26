using AutoMapper;
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
using TeknikServis.Models.Models;
using TeknikServis.Models.ViewModels;
using static TeknikServis.BLL.Identity.MembershipTools;

namespace TeknikServisWeb.Controllers
{
    [Authorize(Roles = "Musteri")]
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
        public ActionResult ArizaBildirimi(ArizaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                var arizaRepo = new ArizaRepository();
                var ariza = new Ariza()
                {
                    MusteriId = HttpContext.User.Identity.GetUserId(),
                    Aciklama = model.Aciklama,
                    MarkaAdi = model.MarkaAdi,
                    ModelAdi = model.ModelAdi,
                    Adres = model.Adres,
                    ArizaOlusturmaTarihi = DateTime.Now,
                   
                };
                arizaRepo.Insert(ariza);
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
                var fotograflar= new FotografRepository().GetAll(x => x.ArizaId == ariza.Id).ToList();
                ariza.ArizaFoto = fotograflar.Select(x => x.Yol).ToList();
                arizaRepo.Update(ariza);               
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
            var data = new List<ArizaViewModel>();
            var ariza = new ArizaRepository().GetAll(x => x.MusteriId == id).ToList();
            foreach (var x in ariza)
            {

                if(x.TeknisyenId==null)
                {
                    data.Add(new ArizaViewModel()
                    {
                        Id = x.Id,
                        ArizaOlusturmaTarihiS = $"{x.ArizaOlusturmaTarihi:O}",
                        MusteriAdi = x.Musteri.Name + " " + x.Musteri.Surname,
                        ModelAdi = x.ModelAdi,
                        MarkaAdi = x.MarkaAdi,
                        Adres = x.Adres,
                        Aciklama = x.Aciklama,
                        ArizaOnaylandiMi = x.ArizaOnaylandiMi,
                        ArizaFotograflari = new FotografRepository().GetAll(z => z.ArizaId == x.Id).Select(y => y.Yol).ToList(),
                        GarantiliVarMi = x.GarantiliVarMi,
                        Ucret = x.Ucret,
                        ArizaYapildiMi = x.ArizaYapildiMi

                    });
                }
                else
                {
                    data.Add(new ArizaViewModel()
                    {
                        Id = x.Id,
                        ArizaOlusturmaTarihiS = $"{x.ArizaOlusturmaTarihi:O}",
                        MusteriAdi = x.Musteri.Name + " " + x.Musteri.Surname,
                        ModelAdi = x.ModelAdi,
                        MarkaAdi = x.MarkaAdi,
                        Adres = x.Adres,
                        Aciklama = x.Aciklama,
                        TeknisyenId = x.TeknisyenId,
                        TeknisyenAdi = NewUserManager().FindById(x.TeknisyenId).Name + " " + NewUserManager().FindById(x.TeknisyenId).Surname,
                        TeknisyenDurumu = NewUserManager().FindById(x.TeknisyenId).TeknisyenDurumu,
                        ArizaOnaylandiMi = x.ArizaOnaylandiMi,
                        ArizaFotograflari = new FotografRepository().GetAll(z => z.ArizaId == x.Id).Select(y => y.Yol).ToList(),
                        GarantiliVarMi = x.GarantiliVarMi,
                        Ucret = x.Ucret,
                        ArizaYapildiMi = x.ArizaYapildiMi

                    });
                }



            }
            return View(data);
        }
        
        [HttpGet]
        [Authorize]
        public ActionResult Anket(int code)
        {
            try
            {
                var anketRepo = new AnketRepository();
                var anket = anketRepo.GetById(code);
                if (anket == null)
                    return RedirectToAction("Index", "Home");
                var data = Mapper.Map<Anket, AnketViewModel>(anket);
                return View(data);
            }
            catch (Exception ex)
            {
                TempData["Message2"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Anket",
                    ControllerName = "Musteri",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Anket(AnketViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Hata Oluştu.");
                return RedirectToAction("Anket", "Musteri", model);
            }
            try
            {
                var anketRepo = new AnketRepository();
                var anket = anketRepo.GetById(model.AnketId);
                if (anket == null)
                    return RedirectToAction("Index", "Home");
                anket.Soru1 = model.Soru1;
                anket.Soru2 = model.Soru2;
                anket.Soru3 = model.Soru3;
                anket.Soru4 = model.Soru4;
                anket.Soru5 = model.Soru5;
                anket.Soru6 = model.Soru6;
                anketRepo.Update(anket);

                TempData["Message2"] = "Anket tamamlandı.";
                return RedirectToAction("Index", "Musteri");
            }
            catch (Exception ex)
            {
                TempData["Message2"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Anket",
                    ControllerName = "Musteri",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }
    }
}