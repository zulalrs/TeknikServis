using static TeknikServis.BLL.Identity.MembershipTools;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Web;
using System.Collections.Generic;
using System.Linq;

namespace TeknikServisWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var id = HttpContext.GetOwinContext().Authentication.User.Identity.GetUserId();
            if (id == null)
                return RedirectToAction("Index", "Account");
            else
            {
                var user = NewUserManager().GetRoles(id);
                if (user == null)
                    return RedirectToAction("Index");

                var roller = GetRoleList();
                foreach (var role in user)
                {
                    if (role == "GenelYonetici")
                        return RedirectToAction("Index", "Admin");
                }
                return View();
            }


        }
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
        public ActionResult Error()
        {
            return View();
        }

    }
}