using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ThaiPaymentAPI.Controllers
{
    public class GBPrimeController : Controller
    {
        // GET: GBPrime
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TestAPIPayment()
        {
            string content = "Testing";
            content += "\nToken UAT : " + JsonConvert.SerializeObject(Models.GBPManager.GetTokenResultUAT());
            content += "\nToken PRD : " + JsonConvert.SerializeObject(Models.GBPManager.GetTokenResultPRD());
            return Content(content, "text/html");
        }
        public ActionResult Token()
        {
            ViewBag.GBPToken = Request.QueryString["gbToken"];
            bool rememberCard = (Request.QueryString["gbRememberCard"] == "true");
            return View();
        }
        [HttpPost]
        [ActionName("Index")]
        public ActionResult PostFormPayment(FormCollection form)
        {
            ViewBag.TotalAmount = form["amount"];
            ViewBag.PayType = form["paytype"];
            return View("Payment");
        }
    }

}