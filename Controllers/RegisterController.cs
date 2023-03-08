using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThaiPaymentAPI.Models;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
namespace ThaiPaymentAPI.Controllers
{
    public class RegisterController : Controller
    {
        public ActionResult Index()
        {
            ViewData["Result"] = new ErrorResponse()
            {
                success = true,
                data = "No Data To Show",
                error = "Ready"
            };
            if (TempData["Message"] != null)
            {
                ViewData["Result"] = new ErrorResponse()
                {
                    success = false,
                    data = (string)TempData["Message"],
                    error = "ERROR"
                };
            }
            return View();
        }
        [HttpPost]
        [ActionName("Index")]
        public ActionResult PostIndex(FormCollection collection)
        {
            var email = collection["register_email"];
            var fullname = collection["register_name"];
            var username = collection["register_user"];
            var msg = "";
            using(SqlConnection cn=new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
            {
                var chk = cn.Execute("SELECT Username FROM dbo.SystemUser WHERE Username=@username", new { @username = username });
                if (chk > 0) msg += "Your user ID already used\n";
                chk = cn.Execute("SELECT Email FROM dbo.SystemUser WHERE Email=@email", new { @email = email });
                if (chk > 0) msg += "Your E-mail already registered as user\n";
                if (msg != "")
                {
                    TempData["Message"] = msg;
                    return RedirectToAction("Index");
                }
            }
            TempData["MailData"] =new MailBody() { 
                email_address=email,
                email_owner_full=fullname,
                email_owner_short=username
            };
            return RedirectToAction("MailSent");
        }
        // GET: Register
        public ActionResult MailSent()
        {
            if (TempData["MailData"] == null)
            {
                return RedirectToAction("Index");
            }
            ViewData["Result"] = new ErrorResponse()
            {
                success = true,
                data = "Mail sent,Please check your inbox! ",
                error = "Success"
            };
            ViewData["DataSource"] = (MailBody)TempData["MailData"];
            return View();
        }
        public ActionResult TestSignUp()
        {
            return View();
        }
    }
    public class MailBody
    {
        public string email_address { get; set; }
        public string email_owner_full { get; set; }
        public string email_owner_short { get; set; }
    }
}