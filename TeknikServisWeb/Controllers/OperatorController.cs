using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Identity;
using TeknikServis.BLL.Repository;
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
            var teknisyenler = Teknisyenler();

            ViewBag.TeknisyenList = teknisyenler;

            var data = new List<UserMarkaModelViewModel>();
            var ariza = new ArizaRepository().GetAll(x=>x.ArizaOnaylandiMi==false)
                .ToList();
            foreach (var x in ariza)
            {
                data.Add(new UserMarkaModelViewModel()
                {
                    Id = x.Id,
                    ArizaFoto = x.ArizaFoto,
                    MusteriAdi = x.Musteri.Name + " " + x.Musteri.Surname,
                    Adres = x.Adres,
                    EklemeTarihi = x.EklemeTarihi,
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
                    if (data.TeknisyenId == null)
                    {
                        return Json(new ResponseData()
                        {
                            message = "Lütfen teknisyen seçiniz",
                            success = false
                        });
                    }
                    new ArizaRepository().Update(ariza);
                    //var teknisyen = NewUserManager().FindById(ariza.TeknisyenId);
                    //teknisyen.TeknisyenBosMu = false;
                    //NewUserManager().Update(teknisyen); ;
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

    }
}