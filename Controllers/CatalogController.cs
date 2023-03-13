using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ThaiPaymentAPI.Controllers
{
    public class CatalogController : Controller
    {
        // GET: Catalog
        public ActionResult Index()
        {
            ViewData["ExchangeRate"] = Convert.ToInt32(new Models.SystemConfig() { ConfigCode = "DEFAULT", ConfigKey= "RATE_THB" }.GetValue("1"));
            ViewData["Message"] = "Ready";
            return View();
        }
    }
}