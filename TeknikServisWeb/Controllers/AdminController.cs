using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TeknikServis.BLL.Helpers;
using TeknikServis.BLL.Repository;
using TeknikServis.BLL.Services;
using TeknikServis.Models.Entities;
using TeknikServis.Models.Models;
using TeknikServis.Models.ViewModels;
using static TeknikServis.BLL.Identity.MembershipTools;

namespace TeknikServisWeb.Controllers
{

    public class AdminController : Controller
    {
        // GET: Admin
        [Authorize(Roles = "GenelYonetici")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "GenelYonetici")]
        public ActionResult GetUsers()
        {
            return View(NewUserStore().Users.ToList());
        }

        [HttpPost]
        [Authorize(Roles = "GenelYonetici")]
        public async Task<JsonResult> SendCode(string id)
        {
            try
            {
                var userStore = NewUserStore();
                var user = await userStore.FindByIdAsync(id);
                if (user == null)
                {
                    return Json(new ResponseData()
                    {
                        message = "Kullanıcı bulunamadı",
                        success = false
                    });
                }
                if (user.EmailConfirmed)
                {
                    return Json(new ResponseData()
                    {
                        message = "Kullanıcı zaten e-postasını onaylamış",
                        success = false
                    });
                }

                user.ActivationCode = StringHelpers.GetCode();
                await userStore.UpdateAsync(user);
                userStore.Context.SaveChanges();
                string SiteUrl = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                                 (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                var emailService = new EmailService();
                var body = $"Merhaba <b>{user.Name} {user.Surname}</b><br>Hesabınızı aktif etmek için aşadıdaki linke tıklayınız<br> <a href='{SiteUrl}/account/activation?code={user.ActivationCode}' >Aktivasyon Linki </a> ";
                await emailService.SendAsync(new IdentityMessage()
                {
                    Body = body,
                    Subject = "Sitemize Hoşgeldiniz"
                }, user.Email);
                return Json(new ResponseData()
                {
                    message = "Kullanıcıya yeni aktivasyon maili gönderildi",
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
        [Authorize(Roles = "GenelYonetici")]
        public async Task<JsonResult> SendPassword(string id)
        {
            try
            {
                var userStore = NewUserStore();
                var user = await userStore.FindByIdAsync(id);
                if (user == null)
                {
                    return Json(new ResponseData()
                    {
                        message = "Kullanıcı bulunamadı",
                        success = false
                    });
                }

                var newPassword = StringHelpers.GetCode().Substring(0, 6);
                await userStore.SetPasswordHashAsync(user, NewUserManager().PasswordHasher.HashPassword(newPassword));
                await userStore.UpdateAsync(user);
                userStore.Context.SaveChanges();

                string SiteUrl = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                                 (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);
                var emailService = new EmailService();
                var body = $"Merhaba <b>{user.Name} {user.Surname}</b><br>Hesabınızın parolası sıfırlanmıştır<br> Yeni parolanız: <b>{newPassword}</b> <p>Yukarıdaki parolayı kullanarak sistemize giriş yapabilirsiniz.</p>";
                emailService.Send(new IdentityMessage() { Body = body, Subject = $"{user.UserName} Şifre Kurtarma" }, user.Email);

                return Json(new ResponseData()
                {
                    message = "Şifre sıfırlama maili gönderilmiştir",
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

        [HttpGet]
        [Authorize(Roles = "GenelYonetici")]
        public ActionResult EditUser(string id)
        {
            try
            {
                var user = NewUserManager().FindById(id);
                if (user == null)
                    return RedirectToAction("GetUsers");

                var roller = GetRoleList();
                foreach (var role in user.Roles)
                {
                    foreach (var selectListItem in roller)
                    {
                        if (selectListItem.Value == role.RoleId)
                            selectListItem.Selected = true;
                    }
                }

                ViewBag.RoleList = roller;


                var model = new UserProfileViewModel()
                {
                    AvatarPath = user.AvatarPath,
                    Name = user.Name,
                    Email = user.Email,
                    Surname = user.Surname,
                    Id = user.Id,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName
                };
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "GetUsers",
                    ControllerName = "Admin",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }

        [Authorize(Roles = "GenelYonetici")]
        public List<SelectListItem> GetRoleList()
        {
            var data = new List<SelectListItem>();
            NewRoleStore().Roles
                .ToList()
                .ForEach(x =>
                {
                    data.Add(new SelectListItem()
                    {
                        Text = $"{x.Name}",
                        Value = x.Id
                    });
                });
            return data;
        }

        [HttpPost]
        [Authorize(Roles = "GenelYonetici")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditUser(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var userManager = NewUserManager();
                var user = await userManager.FindByIdAsync(model.Id);

                user.Name = model.Name;
                user.Surname = model.Surname;
                user.PhoneNumber = model.PhoneNumber;
                user.Email = model.Email;

                if (model.PostedFile != null &&
                    model.PostedFile.ContentLength > 0)
                {
                    var file = model.PostedFile;
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
                    var oldPath = user.AvatarPath;
                    user.AvatarPath = "/Upload/" + fileName + extName;

                    System.IO.File.Delete(Server.MapPath(oldPath));
                }
                await userManager.UpdateAsync(user);
                TempData["Message"] = "Güncelleme işlemi başarılı";
                return RedirectToAction("EditUser", new { id = user.Id });
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "GetUsers",
                    ControllerName = "Admin",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [Authorize(Roles = "GenelYonetici")]
        [ValidateAntiForgeryToken]
        public ActionResult EditUserRoles(UpdateUserRoleViewModel model)
        {
            //var userId = Request.Form[1].ToString();
            //var rolIdler = Request.Form[2].ToString().Split(',');
            var userId = model.Id;
            var rolIdler = model.Roles;
            var roleManager = NewRoleManager();
            var seciliRoller = new string[rolIdler.Count];
            for (var i = 0; i < rolIdler.Count; i++)
            {
                var rid = rolIdler[i];
                seciliRoller[i] = roleManager.FindById(rid).Name;
            }

            var userManager = NewUserManager();
            var user = userManager.FindById(userId);

            foreach (var identityUserRole in user.Roles.ToList())
            {
                userManager.RemoveFromRole(userId, roleManager.FindById(identityUserRole.RoleId).Name);
            }

            for (int i = 0; i < seciliRoller.Length; i++)
            {
                userManager.AddToRole(userId, seciliRoller[i]);
            }

            return RedirectToAction("EditUser", new { id = userId });
        }

        [HttpGet]
        [Authorize(Roles = "GenelYonetici,Operator")]
        public ActionResult Raporlar()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Rapor1()
        {
            var anketRepo = new AnketRepository();
            var soru1 = anketRepo.GetAll().Select(x => x.Soru1).Sum() / anketRepo.GetAll().Select(x => x.Soru1).Count();
            var soru2 = anketRepo.GetAll().Select(x => x.Soru2).Sum() / anketRepo.GetAll().Select(x => x.Soru2).Count();
            var soru3 = anketRepo.GetAll().Select(x => x.Soru3).Sum() / anketRepo.GetAll().Select(x => x.Soru3).Count();
            var soru4 = anketRepo.GetAll().Select(x => x.Soru4).Sum() / anketRepo.GetAll().Select(x => x.Soru4).Count();
            var soru5 = anketRepo.GetAll().Select(x => x.Soru5).Sum() / anketRepo.GetAll().Select(x => x.Soru5).Count();


            var data = new List<ReportData>();
            data.Add(new ReportData()
            {
                Soru = "Firma Memnuniyeti",
                Deger = soru1

            });
            data.Add(new ReportData()
            {
                Soru = "Nezaket",
                Deger = soru2
            });
            data.Add(new ReportData()
            {
                Soru = "Teknik Bilgi ve Yeterlilik",
                Deger = soru3
            });
            data.Add(new ReportData()
            {
                Soru = "Hız",
                Deger = soru4
            });
            data.Add(new ReportData()
            {
                Soru = "Fiyat",
                Deger = soru5
            });
            return Json(new ResponseData()
            {
                message = $"{data.Count} adet kayıt bulundu",
                success = true,
                data = data
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Rapor2()
        {
            var user = NewUserManager().Users.ToList();
            var arizaRepo = new ArizaRepository();
            var sonArizalar = new List<ArizaViewModel>();
            foreach (var item in user)
            {
                if (NewUserManager().IsInRole(item.Id, "Teknisyen"))
                {
                    var ariza = arizaRepo.GetAll().FindLast(x => x.TeknisyenId == item.Id);
                    if (ariza != null)
                    {
                        sonArizalar.Add(new ArizaViewModel()
                        {
                            TeknisyenAdi = ariza.Teknisyen?.Name + " " + ariza.Teknisyen?.Surname,
                            ArizaBaslangicTarihiS = $"{ariza.ArizaBaslangicTarihi:O}",
                            ArizaBitisTarihiS = $"{ariza.ArizaBitisTarihi:O}"
                        });
                    }
                }
            }
            return Json(new ResponseData()
            {
                message = $"{sonArizalar.Count} adet kayıt bulundu",
                success = true,
                data = sonArizalar
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]

        public JsonResult Rapor3()
        {
            var arizaRepo = new ArizaRepository();
            var arizalar = arizaRepo.GetAll(x => x.ArizaYapildiMi == true);
            var toplamS = new TimeSpan();
            foreach (var ariza in arizalar)
            {
                toplamS += (TimeSpan)(ariza.ArizaBitisTarihi - ariza.ArizaOlusturmaTarihi);
            }
            return Json(new ResponseData()
            {
                message = $" adet kayıt bulundu",
                success = true,
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Rapor4()
        {
            var arizaRepo = new ArizaRepository().GetAll();
            var userRepo = NewUserManager().Users.ToList();
            var anketRepo = new AnketRepository().GetAll();

            var teknisyenSorgu = from ariza in arizaRepo
                                 join teknisyen in userRepo on ariza.TeknisyenId equals teknisyen.Id
                                 join anket in anketRepo on ariza.AnketId equals anket.Id
                                 group new
                                 {
                                     ariza,
                                     anket,
                                     teknisyen
                                 }
                                 by new
                                 {
                                     anket.Soru6,
                                     teknisyen.Name,
                                     teknisyen.Surname
                                 }
                               into gp
                                 select new
                                 {
                                     isim=gp.Key.Name+" "+gp.Key.Surname,
                                     toplam = gp.Average(x=>x.ariza.Anket.Soru6)
                                 };

            var data = teknisyenSorgu.ToList();
             
            return Json(new ResponseData()
            {
                message = $"{data.Count} adet kayıt bulundu",
                success = true,
                data=data
            }, JsonRequestBehavior.AllowGet);
        }
    }
}

