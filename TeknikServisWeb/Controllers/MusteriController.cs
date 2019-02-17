using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeknikServis.Models.ViewModels;

namespace TeknikServisWeb.Controllers
{
    public class MusteriController : Controller
    {
        // GET: Musteri
        public ActionResult ArizaBildirimi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ArizaBildirimi(UserMarkaModelViewModel model)
        {

            return View();
        }
    }
}