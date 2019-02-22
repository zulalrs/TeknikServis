using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Repository;
using TeknikServis.Models.Enums;
using TeknikServis.Models.Models;
using TeknikServis.Models.ViewModels;
using static TeknikServis.BLL.Identity.MembershipTools;

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

            var data = new List<ArizaViewModel>();
            var ariza = new ArizaRepository().GetAll(x => x.TeknisyenId == id)
                .ToList();
            foreach (var x in ariza)
            {
                data.Add(new ArizaViewModel()
                {
                    Id = x.Id,
                    MusteriAdi = x.Musteri.Name + " " + x.Musteri.Surname,
                    Adres = x.Adres,
                    Aciklama = x.Aciklama,
                    ArizaOlusturmaTarihi=x.ArizaOlusturmaTarihi
                });
            }
            return View(data);
        }

        [HttpGet]
        public ActionResult GetArizaDetay(int id=0)
        {
            if (id == 0)
                return View();

            var ariza = new ArizaRepository().GetAll().FirstOrDefault(x => x.Id == id);

            var data = new ArizaViewModel()
            {
                Id = ariza.Id,
                MusteriAdi = ariza.Musteri.Name + " " + ariza.Musteri.Surname,
                Adres = ariza.Adres,
                Aciklama = ariza.Aciklama,
                GarantiliVarMi = ariza.GarantiliVarMi,
                MarkaAdi = ariza.MarkaAdi,
                ModelAdi = ariza.ModelAdi,
                ArizaFotograflari = ariza.Fotograflar.Select(y => y.Yol).ToList(),
                ArizaOlusturmaTarihi = ariza.ArizaOlusturmaTarihi,
                ArizaYapildiMi = ariza.ArizaYapildiMi,
                //TeknisyenDurumu = ariza.Teknisyen.TeknisyenDurumu,

            };

            return View(data);
        }


        [HttpPost]
        public async Task<JsonResult> IsOnay(int id)
        {
            try
            {
                var ariza = new ArizaRepository().GetAll().FirstOrDefault(x => x.Id == id);
                var userStore = NewUserStore();
                var user = await userStore.FindByIdAsync(ariza.TeknisyenId);
                if (user == null)
                {
                    return Json(new ResponseData()
                    {
                        message = "Kullanıcı bulunamadı",
                        success = false
                    });
                }
                if (user.TeknisyenDurumu == TeknisyenDurumu.Atandı)
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
                    ariza.Teknisyen.TeknisyenDurumu = TeknisyenDurumu.Atandı;
                    new ArizaRepository().Update(ariza);
                }
                return Json(new ResponseData()
                {
                    message = "İş onayı başarılı",
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
        public async Task<JsonResult> Yolda(int id)
        {
            try
            {
                var ariza = new ArizaRepository().GetAll().FirstOrDefault(x => x.Id == id);
                var userStore = NewUserStore();
                var user = await userStore.FindByIdAsync(ariza.TeknisyenId);
                if (user == null)
                {
                    return Json(new ResponseData()
                    {
                        message = "Kullanıcı bulunamadı",
                        success = false
                    });
                }
                ariza.Teknisyen.TeknisyenDurumu = TeknisyenDurumu.Yolda;
                new ArizaRepository().Update(ariza);

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
                var ariza = new ArizaRepository().GetAll().FirstOrDefault(x => x.Id == id);
                var userStore = NewUserStore();
                var user = await userStore.FindByIdAsync(ariza.TeknisyenId);
                if (user == null)
                {
                    return Json(new ResponseData()
                    {
                        message = "Kullanıcı bulunamadı",
                        success = false
                    });
                }

                ariza.Teknisyen.TeknisyenDurumu = TeknisyenDurumu.Ulasti;
                new ArizaRepository().Update(ariza);
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
                var ariza = new ArizaRepository().GetAll().FirstOrDefault(x => x.Id == id);
                var userStore = NewUserStore();
                var user = await userStore.FindByIdAsync(ariza.TeknisyenId);
                if (user == null)
                {
                    return Json(new ResponseData()
                    {
                        message = "Kullanıcı bulunamadı",
                        success = false
                    });
                }

                ariza.ArizaBitisTarihi = DateTime.Now;
                ariza.Teknisyen.TeknisyenDurumu = TeknisyenDurumu.Bosta;
                new ArizaRepository().Update(ariza);
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
    }
}