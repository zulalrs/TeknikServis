using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Identity;
using TeknikServis.BLL.Repository;
using TeknikServis.BLL.Services;
using TeknikServis.Models.Enums;
using TeknikServis.Models.Models;
using TeknikServis.Models.ViewModels;
using static TeknikServis.BLL.Identity.MembershipTools;

namespace TeknikServisWeb.Controllers
{
    public class OperatorController : Controller
    {
        // GET: Operator
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Arizalar()
        {
            var teknisyenler = Teknisyenler();

            ViewBag.TeknisyenList = teknisyenler;

            var data = new List<ArizaViewModel>();
            //x => x.ArizaOnaylandiMi == false
            var arizaRepo = new ArizaRepository();
            var ariza = arizaRepo.GetAll()
                .ToList();
            foreach (var x in ariza)
            {

                data.Add(new ArizaViewModel()
                {
                    Id = x.Id,
                    //ArizaFoto = x.ArizaFoto,
                    MusteriAdi = x.Musteri?.Name + " " + x.Musteri?.Surname,
                    Adres = x.Adres,
                    TeknisyenId = x.TeknisyenId ?? null,
                    //ArizaBaslangicTarihi = x.ArizaBaslangicTarihi,                  
                    ArizaOnaylandiMi = x.ArizaOnaylandiMi
                });
            }
            return View(data);
        }

        public List<SelectListItem> Teknisyenler()
        {
            var data = new List<SelectListItem>();
            var users = NewUserStore().Users.ToList();

            data.Add(new SelectListItem()
            {
                Text = $"Teknisyen Seçiniz",
                Value = "0"
            });
            foreach (var user in users)
            {
                if (NewUserManager().IsInRole(user.Id, "Teknisyen") && user.TeknisyenDurumu == TeknisyenDurumu.Bosta)
                {
                    // var teknisyen = new ArizaRepository().GetAll().FirstOrDefault(x => x.TeknisyenId == user.Id);
                    //if (teknisyen == null)
                    //{

                    //}
                    data.Add(new SelectListItem()
                    {
                        Text = $"{user.Name} {user.Surname}",
                        Value = user.Id
                    });

                }

            }

            return data;
        }

        public async Task<JsonResult> Guncelle(ArizaViewModel data)
        {
            try
            {

                if (data == null)
                {
                    return Json(new ResponseData()
                    {
                        message = "Arıza kaydı bulunamadı",
                        success = false
                    });
                }
                var arizaRepo = new ArizaRepository();
                var ariza = arizaRepo.GetById(data.Id);
                ariza.TeknisyenId = data.TeknisyenId;
                ariza.ArizaOnaylandiMi = data.ArizaOnaylandiMi;

                if (data.ArizaOnaylandiMi)
                {
                    if (data.TeknisyenId == null || data.TeknisyenId == "0")
                    {
                        return Json(new ResponseData()
                        {
                            message = "Lütfen teknisyen seçiniz",
                            success = false
                        });
                    }
                    arizaRepo.Update(ariza);
                    ariza.Teknisyen.TeknisyenDurumu = TeknisyenDurumu.Beklemede;
                    arizaRepo.Update(ariza);
                    string SiteUrl = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                                    (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    var user = await NewUserStore().FindByIdAsync(ariza.TeknisyenId);
                    //user.TeknisyenDurumu = TeknisyenDurumu.Beklemede;
                    //await NewUserStore().UpdateAsync(user);
                    var emailService = new EmailService();
                    var body = $"Merhaba <b>{user.Name} {user.Surname}</b><br>İşi kabul ediyorsanız lütfen linke tıklayınız<br> <a href='{SiteUrl}/operator/activation?code={user.ActivationCode}' >Onay Linki </a> ";
                    emailService.Send(new IdentityMessage() { Body = body, Subject = "İş Kabul" }, user.Email);

                    var userM = NewUserManager().FindById(ariza.MusteriId);

                    var emailServiceM = new EmailService();
                    var bodyM = $"Merhaba <b>{userM.Name} {userM.Surname}</b><br>Arızanız onaylanmıştır. Sizinle en kısa sürede iletişime geçilecektir.<br> ";
                    emailServiceM.Send(new IdentityMessage() { Body = bodyM, Subject = "Arıza Onay" }, userM.Email);
                    return Json(new ResponseData()
                    {
                        message = "Güncelleme başarılı",
                        success = true
                    });
                }
                else
                {
                    return Json(new ResponseData()
                    {
                        message = "Ariza onaylanmadı",
                        success = false
                    });
                }

            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Bir hata oluştu {ex.Message}",
                    success = false
                });
            }
        }

        [HttpPost]
        // [ValidateAntiForgeryToken]
        public JsonResult GetArizaDetay(int id)
        {
            try
            {
                var ariza = new ArizaRepository().GetAll().FirstOrDefault(x => x.Id == id);
                if (ariza == null)
                {
                    return Json(new ResponseData()
                    {
                        message = "Arıza kaydı bulunamadı",
                        success = false
                    });
                }
                var data = new ArizaViewModel()
                {
                    Id = ariza.Id,
                    MusteriAdi = ariza.Musteri.Name + " " + ariza.Musteri.Surname,
                    ModelAdi = ariza.ModelAdi,
                    MarkaAdi = ariza.MarkaAdi,
                    TeknisyenId = ariza.TeknisyenId ?? null,
                    TeknisyenDurumu = ariza.Teknisyen?.TeknisyenDurumu ?? TeknisyenDurumu.Beklemede,
                    TeknisyenAdi = ariza.Teknisyen?.Name + " " + ariza.Teknisyen?.Surname,
                    Adres = ariza.Adres,
                    Aciklama = ariza.Aciklama,
                    ArizaOlusturmaTarihiS = $"{ariza.ArizaOlusturmaTarihi:O}",
                    ArizaFotograflari = ariza.Fotograflar.Select(y => y.Yol).ToList(),
                    GarantiliVarMi = ariza.GarantiliVarMi,
                    Ucret = ariza.Ucret,
                    ArizaYapildiMi = ariza.ArizaYapildiMi
                };
                return Json(new ResponseData()
                {
                    message = "Güncelleme başarılı",
                    success = true,
                    data=data
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseData()
                {
                    message = $"Bir hata oluştu {ex.Message}",
                    success = false
                });
            }
        }
    }
}