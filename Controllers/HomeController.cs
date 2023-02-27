using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
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
    }
}
