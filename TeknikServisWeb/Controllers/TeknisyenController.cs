using AutoMapper;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Repository;
using TeknikServis.BLL.Services;
using TeknikServis.Models.Entities;
using TeknikServis.Models.Enums;
using TeknikServis.Models.Models;
using TeknikServis.Models.ViewModels;
using static TeknikServis.BLL.Identity.MembershipTools;

namespace TeknikServisWeb.Controllers
{
    [Authorize(Roles = "Teknisyen")]
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
            var ariza = new ArizaRepository().GetAll().FirstOrDefault(x => x.TeknisyenId == id && x.ArizaYapildiMi == false);
            if (ariza != null)
            {
               
                var data = Mapper.Map<ArizaViewModel>(ariza);
                data.ArizaFotograflari = ariza.Fotograflar.Select(y => y.Yol).ToList();
                data.MusteriAdi = ariza.Musteri.Name + " " + ariza.Musteri.Surname;
                data.TeknisyenDurumu = ariza.Teknisyen.TeknisyenDurumu;
                //var data = new ArizaViewModel()
                //{
                //    Id = ariza.Id,
                //    MusteriAdi = ariza.Musteri.Name + " " + ariza.Musteri.Surname,
                //    Adres = ariza.Adres,
                //    Aciklama = ariza.Aciklama,
                //    ArizaOlusturmaTarihi = ariza.ArizaOlusturmaTarihi
                //};

                return View(data);
            }
            else
            {
                TempData["Message"] = "Atanmış Bir Arızanız Bulunmamaktadır";
                return View();
            }
        }

        [HttpGet]
        public ActionResult GetArizaDetay(int id = 0)
        {
            if (id == 0)
                return View();

            var ariza = new ArizaRepository().GetAll().FirstOrDefault(x => x.Id == id);
            var data = Mapper.Map<ArizaViewModel>(ariza);
            data.ArizaFotograflari = ariza.Fotograflar.Select(y => y.Yol).ToList();
            data.MusteriAdi = ariza.Musteri.Name + " " + ariza.Musteri.Surname;
            return View(data);
        }

        [HttpPost]
        public async Task<JsonResult> IsOnay(int id)
        {
            try
            {
                var arizaRepo = new ArizaRepository();
                var ariza = arizaRepo.GetAll().FirstOrDefault(x => x.Id == id);
                var userStore = NewUserStore();
                var teknisyen = await userStore.FindByIdAsync(ariza.TeknisyenId);
                if (teknisyen == null)
                {
                    return Json(new ResponseData()
                    {
                        message = "Kullanıcı bulunamadı",
                        success = false
                    });
                }
                if (teknisyen.TeknisyenDurumu == TeknisyenDurumu.Atandı)
                {
                    return Json(new ResponseData()
                    {
                        message = "Üzerinize kayıtlı bir iş var",
                        success = false
                    });
                }
                else
                {
                    ariza.ArizaBaslangicTarihi = DateTime.Now;
                    teknisyen.TeknisyenDurumu = TeknisyenDurumu.Atandı;
                    await userStore.UpdateAsync(teknisyen);
                    userStore.Context.SaveChanges();
                    ariza.Teknisyen.TeknisyenDurumu = teknisyen.TeknisyenDurumu;
                    var arizaLogRepo = new ArizaLogRepository();
                    var log = new ArizaLog()
                    {
                        ArizaId = ariza.Id,
                        Aciklama = "Teknisyen atanmıştır",
                        Zaman = DateTime.Now
                    };
                    arizaLogRepo.Insert(log);
                    //ariza.ArizaLoglar.Add(log);
                    arizaRepo.Update(ariza);
                }
                return Json(new ResponseData()
                {
                    message = "İş onayı başarılı",
                    success = true,
                    
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Bir hata oluştu: {ex.Message}",
                    success = false
                });
            }
        }

        [HttpPost]
        public async Task<JsonResult> Yolda(int id)
        {
            try
            {
                var arizaRepo = new ArizaRepository();
                var ariza = arizaRepo.GetAll().FirstOrDefault(x => x.Id == id);
                var userStore = NewUserStore();
                var teknisyen = await userStore.FindByIdAsync(ariza.TeknisyenId);
                if (teknisyen == null)
                {
                    return Json(new ResponseData()
                    {
                        message = "Kullanıcı bulunamadı",
                        success = false
                    });
                }
                teknisyen.TeknisyenDurumu = TeknisyenDurumu.Yolda;
                await userStore.UpdateAsync(teknisyen);
                userStore.Context.SaveChanges();
                ariza.Teknisyen.TeknisyenDurumu = teknisyen.TeknisyenDurumu;
                var arizaLogRepo = new ArizaLogRepository();
                var log = new ArizaLog()
                {
                    ArizaId = ariza.Id,
                    Aciklama = "Teknisyen yola çıktı.",
                    Zaman = DateTime.Now
                };
                arizaLogRepo.Insert(log);
                //ariza.ArizaLoglar.Add(log);
                arizaRepo.Update(ariza);

                return Json(new ResponseData()
                {
                    message = "İşlem başarılı",
                    success = true
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Bir hata oluştu: {ex.Message}",
                    success = false
                });
            }
        }

        [HttpPost]
        public async Task<JsonResult> Ulasti(int id)
        {
            try
            {
                var arizaRepo = new ArizaRepository();
                var ariza = arizaRepo.GetAll().FirstOrDefault(x => x.Id == id);
                var userStore = NewUserStore();
                var teknisyen = await userStore.FindByIdAsync(ariza.TeknisyenId);
                if (teknisyen == null)
                {
                    return Json(new ResponseData()
                    {
                        message = "Kullanıcı bulunamadı",
                        success = false
                    });
                }

                teknisyen.TeknisyenDurumu = TeknisyenDurumu.Ulasti;
                await userStore.UpdateAsync(teknisyen);
                userStore.Context.SaveChanges();
                ariza.Teknisyen.TeknisyenDurumu = teknisyen.TeknisyenDurumu;
                arizaRepo.Update(ariza);
                return Json(new ResponseData()
                {
                    message = "İşlem başarılı",
                    success = true
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Bir hata oluştu: {ex.Message}",
                    success = false
                });
            }
        }

        [HttpPost]
        public async Task<JsonResult> IslemTamam(int id)
        {
            try
            {
                var arizaRepo = new ArizaRepository();
                var ariza = arizaRepo.GetAll().FirstOrDefault(x => x.Id == id);
                var userStore = NewUserStore();
                var teknisyen = await userStore.FindByIdAsync(ariza.TeknisyenId);
                var musteri = await userStore.FindByIdAsync(ariza.MusteriId);
                if (teknisyen == null)
                {
                    return Json(new ResponseData()
                    {
                        message = "Kullanıcı bulunamadı",
                        success = false
                    });
                }

                ariza.ArizaBitisTarihi = DateTime.Now;
                teknisyen.TeknisyenDurumu = TeknisyenDurumu.Bosta;
                await userStore.UpdateAsync(teknisyen);
                userStore.Context.SaveChanges();
                ariza.ArizaYapildiMi = true;
              
                var arizaLogRepo = new ArizaLogRepository();
                var log = new ArizaLog()
                {
                    ArizaId = ariza.Id,
                    Aciklama = "Arıza çözümlenmiştir.",
                    Zaman = ariza.ArizaBitisTarihi.Value
                };
                arizaLogRepo.Insert(log);
                //ariza.ArizaLoglar.Add(log);
                ariza.Teknisyen.TeknisyenDurumu = teknisyen.TeknisyenDurumu;
                arizaRepo.Update(ariza);

                var anket = new Anket();
                var anketRepo = new AnketRepository();
                anketRepo.Insert(anket);
                ariza.AnketId = anket.Id;
                arizaRepo.Update(ariza);
                

                string SiteUrl = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                                  (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                var emailService = new EmailService();
                var body = $"Merhaba <b>{musteri.Name} {musteri.Surname}</b><br><br> <a href='{SiteUrl}/musteri/anket?code={ariza.AnketId}' >Anket Linki </a> ";
                await emailService.SendAsync(new IdentityMessage() { Body = body, Subject = "Memnuniyet Anketi" }, musteri.Email);

                var data = new ArizaViewModel
                {
                    ArizaYapildiMi = ariza.ArizaYapildiMi,
                    Id = ariza.Id,
                    ArizaBitisTarihi = ariza.ArizaBitisTarihi ?? DateTime.Now,
                    TeknisyenDurumu = ariza.Teknisyen.TeknisyenDurumu,
                    Aciklama = ariza.Aciklama,
                    Adres = ariza.Adres,
                    MarkaAdi = ariza.MarkaAdi,
                    ModelAdi = ariza.ModelAdi,
                    MusteriId = ariza.MusteriId,
                    MusteriAdi = ariza.Musteri.Name + " " + ariza.Musteri.Surname,
                    ArizaOlusturmaTarihi = ariza.ArizaOlusturmaTarihi,
                    ArizaOnaylandiMi = ariza.ArizaOnaylandiMi,
                    TeknisyenId = ariza.TeknisyenId,

                };


                return Json(new ResponseData()
                {
                    message = "İşlem başarılı",
                    success = true,
                    data = data
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Bir hata oluştu: {ex.Message}",
                    success = false
                });
            }
        }

        [HttpPost]
        public ActionResult GetArizaDetay(ArizaViewModel model)
        {
            try
            {
                var arizaRepo = new ArizaRepository();
                var ariza = arizaRepo.GetAll().FirstOrDefault(x => x.Id == model.Id);
                if (ariza == null)
                {
                    return View(model);
                }
                ariza.GarantiliVarMi = model.GarantiliVarMi;
                ariza.Ucret = model.Ucret;
                arizaRepo.Update(ariza);

                TempData["Message"] = "Güncelleme başarılı";
                return RedirectToAction("GetArizaDetay", "Teknisyen");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Guncelle",
                    ControllerName = "Teknisyen",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpGet]
        public ActionResult GetEskiArizalar()
        {
            var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
            var ariza = new ArizaRepository().GetAll(x => x.TeknisyenId == id && x.ArizaYapildiMi == true).ToList();
            if (ariza != null)
            {
                var data = new List<ArizaViewModel>();
                foreach (var item in ariza)
                {
                    data.Add(new ArizaViewModel()
                    {
                        Id = item.Id,
                        MusteriAdi = item.Musteri.Name + " " + item.Musteri.Surname,
                        Adres = item.Adres,
                        Aciklama = item.Aciklama,
                        ArizaOlusturmaTarihi = item.ArizaOlusturmaTarihi,
                        ArizaBaslangicTarihi = item.ArizaBaslangicTarihi ?? DateTime.Now,
                        ArizaBitisTarihi = item.ArizaBitisTarihi ?? DateTime.Now,
                        ArizaFotograflari = new FotografRepository().GetAll(x => x.ArizaId == item.Id).Select(y => y.Yol).ToList(),
                        GarantiliVarMi = item.GarantiliVarMi,
                        Ucret = item.Ucret,
                        MarkaAdi = item.MarkaAdi,
                        ModelAdi = item.ModelAdi,
                    });
                }
                return View(data);
            }
            else
            {
                TempData["Message"] = "Bitirilmiş Bir Arızanız Bulunmamaktadır";
                return View();
            }

        }
    }
}