using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeknikServisWeb.Controllers
{
    public class PartialController : Controller
    {
        // GET: Partial
        public PartialViewResult HeaderPartial() 
        {
            return PartialView("Partial/_HeaderPartial");
        }
        public PartialViewResult SidebarPartial()
        {
            return PartialView("Partial/_SidebarPartial");
        }
        public PartialViewResult FooterPartial()
        {
            return PartialView("Partial/_FooterPartial");
        }
    }
}