using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SCIMAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string apiUri = Url.HttpRouteUrl("DefaultApi", new { controller = "Users", });
            ViewBag.ApiUrl = new Uri(Request.Url, apiUri).AbsoluteUri.ToString();

            return View();
        }       
    }

    
}
