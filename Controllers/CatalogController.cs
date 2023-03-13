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
            ViewBag.GroupData = new Models.OrderGroup().Gets().Where(e => e.isactive == true).ToList();
            ViewBag.DataSource = new Models.OrderDetail().Gets().Where(e=>e.isactive.Equals(true)).ToList();
            ViewData["ExchangeRate"] = Convert.ToInt32(new Models.SystemConfig() { ConfigCode = "DEFAULT", ConfigKey= "RATE_USD" }.GetValue("1"));
            ViewData["Message"] = "Ready";
            return View();
        }
    }
}