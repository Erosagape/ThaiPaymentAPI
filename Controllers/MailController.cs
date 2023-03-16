using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThaiPaymentAPI.Models;
namespace ThaiPaymentAPI.Controllers
{
    public class MailController : Controller
    {
        // GET: Mail
        public ActionResult Index()
        {
            ViewData["Message"] = "Ready";
            return View();
        }
        [HttpPost]
        [ActionName("Index")]
        public ActionResult PostIndex()
        {
            ViewData["Message"] = Models.EMailHelper.TestSentMail();
            return View("Index");
        }
        public ActionResult Config()
        {
            var msg = EMailHelper.GetNewEMail();
            ViewData["Message"] = "Ready";
            if (TempData["Message"] != null)
            {
                ViewData["Message"] = TempData["Message"];
            }
            return View(msg);
        }
        [HttpPost]
        [ActionName("Config")]
        public ActionResult PostConfig(FormCollection form)
        {
            var smtpHost = new SystemConfig() { ConfigCode = "CONFIG_EMAIL", ConfigKey = "SMTP_HOST", ConfigValue = form["smtpHost"] }.Save().error;
            var smtpPort = new SystemConfig() { ConfigCode = "CONFIG_EMAIL", ConfigKey = "SMTP_PORT", ConfigValue = form["smtpPort"] }.Save().error;
            var smtpEmail = new SystemConfig() { ConfigCode = "CONFIG_EMAIL", ConfigKey = "EMAIL_USER", ConfigValue = form["emailAddress"] }.Save().error;
            var smtpPass = new SystemConfig() { ConfigCode = "CONFIG_EMAIL", ConfigKey = "EMAIL_PASSWORD", ConfigValue = form["emailPassword"] }.Save().error;
            var smtpSSL = new SystemConfig() { ConfigCode = "CONFIG_EMAIL", ConfigKey = "SMTP_USESSL", ConfigValue = (form["useSSL"] == null ? "N" : "Y") }.Save().error;

            TempData["Message"] = "Save Complete";
            return RedirectToAction("Config");
        }
        public ActionResult Test()
        {
            ViewData["Message"] = "Ready";
            if (TempData["Message"] != null)
            {
                ViewData["Message"] = TempData["Message"];
            }
            return View();
        }
        [HttpPost]
        [ActionName("Test")]
        public ActionResult TestPost(FormCollection form)
        {
            var msg = EMailHelper.GetNewEMail();
            msg.fromEmail = form["fromEmail"];
            msg.fromPerson = form["fromPerson"];
            msg.toEmail = form["toEmail"];
            msg.toPerson = form["toPerson"];
            msg.body = form["mailbody"];
            msg.subject = form["subject"];
            
            var res=EMailHelper.SentEMail(msg);

            TempData["Message"] = (res.success? res.data:res.error);
            return RedirectToAction("Test");
        }
        public ActionResult Message()
        {
            ViewBag.Message = new ErrorResponse(true, "Ready", "");
            ViewBag.EmailTo = new SystemConfig() { ConfigCode = "CONFIG_EMAIL", ConfigKey = "EMAIL_ADMIN" }.GetValue();
            if (TempData["Message"] != null)
                ViewBag.Message = (ErrorResponse)TempData["Message"];
            return View();
        }
        [HttpPost]
        [ActionName("Message")]
        public ActionResult PostMessage(FormCollection form)
        {
            var msg = EMailHelper.GetNewEMail();
            msg.fromEmail = "admin@aih-consultant.com";
            msg.fromPerson = form["msg_from"];
            msg.toEmail = form["msg_to"];
            msg.toPerson = form["msg_to"];
            msg.body = form["msg_body"] + "<br>" + "Contact: " + form["msg_contact"];
            msg.subject = form["msg_subject"];

            var res = EMailHelper.SentEMail(msg);

            TempData["Message"] = res;
            return RedirectToAction("Message");
        }
    }
}