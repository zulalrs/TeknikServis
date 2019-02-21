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

            var data = new List<UserMarkaModelViewModel>();
            var ariza = new ArizaRepository().GetAll(x => x.ArizaOnaylandiMi == false)
                .ToList();
            foreach (var x in ariza)
            {
                data.Add(new UserMarkaModelViewModel()
                {
                    Id = x.Id,
                    //ArizaFoto = x.ArizaFoto,
                    MusteriAdi = x.Musteri.Name + " " + x.Musteri.Surname,
                    Adres = x.Adres,
                    ArizaBaslangicTarihi = x.ArizaBaslangicTarihi,
                    TeknisyenId = x.TeknisyenId,
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
                if (NewUserManager().IsInRole(user.Id, "Teknisyen"))
                {
                    var teknisyen = new ArizaRepository().GetAll().FirstOrDefault(x => x.TeknisyenId == user.Id);

                    if (teknisyen == null)
                    {
                        data.Add(new SelectListItem()
                        {
                            Text = $"{user.Name} {user.Surname}",
                            Value = user.Id
                        });
                    }
                }

            }

            return data;
        }

        public JsonResult Guncelle(UserMarkaModelViewModel data)
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

                var ariza = new ArizaRepository().GetById(data.Id);
                ariza.TeknisyenId = data.TeknisyenId;
                ariza.ArizaOnaylandiMi = data.ArizaOnaylandiMi;
                if (data.ArizaOnaylandiMi)
                {
                    if (data.TeknisyenId == null || data.TeknisyenId=="0")
                    {
                        return Json(new ResponseData()
                        {
                            message = "Lütfen teknisyen seçiniz",
                            success = false
                        });
                    }
                    new ArizaRepository().Update(ariza);
                    string SiteUrl = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                                    (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    var user = NewUserManager().FindById(data.TeknisyenId);
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

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Activation(string code)
        {
            try
            {
                var userStore = NewUserStore();
                var user = userStore.Users.FirstOrDefault(x => x.ActivationCode == code);

                if (user != null)
                {
                    if (user.TeknisyenDurumu==TeknisyenDurumu.Atandı)
                    {
                        ViewBag.Message = $"<div class='alert alert-info alert-dismissible'><i class='icon fa fa-info'></i>Bu iş daha önce onaylanmıştır.<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>x</button></div>";
                    }
                    else
                    {
                        user.TeknisyenDurumu = TeknisyenDurumu.Atandı;

                        userStore.Context.SaveChanges();
                        ViewBag.Message = $"<div class='alert alert-success alert-dismissible'><i class='icon fa fa-check'></i>Onay işleminiz başarılı.<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>x</button></div>";
                    }
                }
                else
                {
                    ViewBag.Message = $"<div class='alert alert-danger alert-dismissible'><i class='icon fa fa-ban'></i>Onay başarısız.<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>x</button></div>";
                }
            }
            catch (Exception)
            {
                ViewBag.Message = "<div class='alert alert-danger alert-dismissible'><i class='icon fa fa-ban'></i>Onay işleminde bir hata oluştu.<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>x</button></div>";
            }

            return RedirectToAction("Activation", "Operator");
        }
    }
}