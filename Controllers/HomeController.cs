using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using ThaiPaymentAPI.Models;
namespace ThaiPaymentAPI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public string TestConnection()
        {
            try
            {
                using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
                {
                    cn.Open();
                    cn.Close();
                }
                return "Connection Open";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public ActionResult TestCallBack()
        {
            return View();
        }
        public JsonResult TestActionLogSuccess()
        {
            var data = new ActionLog()
            {
                log_action = "SAV",
                log_message = DateTime.Now.ToString("yyyyMMddHHMMss"),
                log_source = "Home/TestActionLog",
                log_data = "ทดสอบ",
                log_error = false,
                log_stacktrace = ""
            };
            return Json(data.Save(),JsonRequestBehavior.AllowGet);
        }
        public JsonResult TestActionLogFail()
        {
            var data = new ActionLog()
            {
                log_action = "ERR",
                log_message = DateTime.Now.ToString("yyyyMMddHHMMss"),
                log_source = "Home/TestActionLog",
                log_data = "ทดสอบ",
                log_error = true,
                log_stacktrace = "From TestActionLog line 61"
            };
            return Json(data.Save(),JsonRequestBehavior.AllowGet);
        }
        public string TestSentMail()
        {
            return EMailHelper.TestSentMail();
        }
    }
}
