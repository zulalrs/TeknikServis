using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Helpers;
using TeknikServis.Models.IdentityModels;
using TeknikServis.Models.ViewModels;
using TeknikServis.BLL.Identity;
using static TeknikServis.BLL.Identity.MembershipTools;
using TeknikServis.BLL.Services;
using Microsoft.Owin.Security;

namespace TeknikServisWeb.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            if (HttpContext.GetOwinContext().Authentication.User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterLoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }
            try
            {
                var userStore = NewUserStore();
                var userManager = NewUserManager();

                var rm = model.RegisterViewModel;

                var user = await userManager.FindByNameAsync(rm.UserName);
                if (user != null)
                {
                    ModelState.AddModelError("UserName", "Bu kullanıcı adı daha önceden alınmıştır");
                    return View("Index", model);
                }

                var newUser = new User()
                {
                    UserName = rm.UserName,
                    Email = rm.Email,
                    Name = rm.Name,
                    Surname = rm.Surname,
                    Adress=rm.Adress,
                    PhoneNumber=rm.Telephone,
                    ActivationCode = StringHelpers.GetCode()
                };
                var result = await userManager.CreateAsync(newUser, rm.Password);
                if (result.Succeeded)
                {
                    if (userStore.Users.Count() == 1)
                    {
                        await userManager.AddToRoleAsync(newUser.Id, "GenelYonetici");
                    }
                    else
                    {
                        await userManager.AddToRoleAsync(newUser.Id, "Musteri");
                    }

                    string SiteUrl = Request.Url.Scheme + System.Uri.SchemeDelimiter + Request.Url.Host +
                                     (Request.Url.IsDefaultPort ? "" : ":" + Request.Url.Port);

                    var emailService = new EmailService();
                    var body = $"Merhaba <b>{newUser.Name} {newUser.Surname}</b><br>Hesabınızı aktif etmek için aşadıdaki linke tıklayınız<br> <a href='{SiteUrl}/account/activation?code={newUser.ActivationCode}' >Aktivasyon Linki </a> ";
                    await emailService.SendAsync(new IdentityMessage() { Body = body, Subject = "Sitemize Hoşgeldiniz" }, newUser.Email);
                }
                else
                {
                    var err = "";
                    foreach (var resultError in result.Errors)
                    {
                        err += resultError + " ";
                    }
                    ModelState.AddModelError("", err);
                    return View("Index", model);
                }

                TempData["Message"] = "Kaydınız alınlıştır. Lütfen giriş yapınız";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Index",
                    ControllerName = "Account",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View("Index", model);

                var userManager = NewUserManager();
                var user = await userManager.FindAsync(model.UserName, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
                    return View("Index", model);
                }
                var authManager = HttpContext.GetOwinContext().Authentication;
                var userIdentity =
                    await userManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                authManager.SignIn(new AuthenticationProperties()
                {
                    IsPersistent = model.RememberMe
                }, userIdentity);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata oluştu {ex.Message}",
                    ActionName = "Index",
                    ControllerName = "Account",
                    ErrorCode = 500
                };
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();
            return RedirectToAction("Index", "Account");
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
                    if (user.EmailConfirmed)
                    {
                        ViewBag.Message = $"<div class='alert alert-info alert-dismissible'><i class='icon fa fa-info'></i>Bu hesap daha önce aktive edilmiştir.<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>x</button></div>";
                    }
                    else
                    {
                        user.EmailConfirmed = true;

                        userStore.Context.SaveChanges();
                        ViewBag.Message = $"<div class='alert alert-success alert-dismissible'><i class='icon fa fa-check'></i>Aktivasyon işleminiz başarılı.<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>x</button></div>";
                    }
                }
                else
                {
                    ViewBag.Message = $"<div class='alert alert-danger alert-dismissible'><i class='icon fa fa-ban'></i>Aktivasyon başarısız<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>x</button></div>";
                }
            }
            catch (Exception)
            {
                ViewBag.Message = "<div class='alert alert-danger alert-dismissible'><i class='icon fa fa-ban'></i>Aktivasyon işleminde bir hata oluştu.<button type='button' class='close' data-dismiss='alert' aria-hidden='true'>x</button></div>";
            }

            return View();
        }
    }
}