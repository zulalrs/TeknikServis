using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BLL.Repository;

namespace TeknikServisWeb.Controllers
{
    public class BaseController : Controller
    {


        protected List<SelectListItem> GetMarka()
        {
            var markalar = new MarkaRepository()
                .GetAll()
                .OrderBy(x => x.MarkaAdi);

            var markaList = new List<SelectListItem>();
            foreach (var marka in markalar)
            {
                markaList.Add(new SelectListItem()
                {
                    Text = marka.MarkaAdi,
                    Value=marka.Id.ToString()
                });
            }
            return markaList;
        }
        protected List<SelectListItem> GetModel()
        {
            var modeller = new ModelRepository()
                .GetAll()
                .OrderBy(x => x.ModelAdi);

            var modelList = new List<SelectListItem>();

            foreach (var model in modeller)
            {
                modelList.Add(new SelectListItem()
                {
                    Text = model.ModelAdi,
                    Value = model.Id.ToString()
                });
            }
            return modelList;
        }

    }
}