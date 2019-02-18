using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Identity;
using TeknikServis.BLL.Repository;
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
            var ariza = new ArizaRepository().GetAll()
                .ToList();
            foreach (var x in ariza)
            {
                data.Add(new UserMarkaModelViewModel()
                {
                    Id = x.Id,
                    ArizaFoto = x.ArizaFoto,
                    MusteriAdi = x.Musteri.Name + x.Musteri.Surname,
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
            var users = MembershipTools.NewUserStore().Users.ToList();
            data.Add(new SelectListItem()
            {
                Text = $"Teknisyen Seçiniz",
                Value = "0"
            });
            foreach (var user in users)
            {
                if (!user.TeknisyenBosMu)
                {
                    var roller = NewUserManager().GetRoles(user.Id);
                    foreach (var rol in roller)
                    {
                        if (rol == "Teknisyen")
                        {
                            data.Add(new SelectListItem()
                            {
                                Text = $"{user.Name} {user.Surname}",
                                Value = user.Id
                            });
                        }
                    }
                }
            }

            return data;
        }
    }
}