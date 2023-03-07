using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThaiPaymentAPI.Models;
namespace ThaiPaymentAPI.Controllers
{
    public class ConfigController : Controller
    {
        // GET: Config
        public ActionResult Index()
        {
            ViewData["DataSource"] = new SystemConfig().Gets().ToList();
            if (TempData["Result"] != null)
            {
                ViewData["Result"] = (ErrorResponse)TempData["Result"];
            } else
            {
                ViewData["Result"] = new ErrorResponse()
                {
                    error = "OK",
                    data = "Ready",
                    success = true
                };
            }
            return View();
        }

        // GET: Config/Details/5
        public ActionResult Details(string code,string key)
        {
            var o = new SystemConfig().Gets().Where(e=>e.ConfigCode.Equals(code) && e.ConfigKey.Equals(key)).FirstOrDefault();
            return View(o);
        }

        // GET: Config/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Config/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here
                var o = new SystemConfig()
                {
                    ConfigCode = collection["ConfigCode"],
                    ConfigKey = collection["ConfigKey"],
                    ConfigValue = collection["ConfigValue"]
                };
                TempData["Result"]=o.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Config/Edit/5
        public ActionResult Edit(string code,string key)
        {
            var o = new SystemConfig().Gets().Where(e => e.ConfigCode.Equals(code) && e.ConfigKey.Equals(key)).FirstOrDefault();
            return View(o);
        }

        // POST: Config/Edit/5
        [HttpPost]
        public ActionResult Edit(string code,string key, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                var o= new SystemConfig().Gets().Where(e => e.ConfigCode.Equals(code) && e.ConfigKey.Equals(key)).FirstOrDefault();
                if (o == null)
                {
                    o = new SystemConfig()
                    {
                        ConfigCode = code,
                        ConfigKey = key,
                        ConfigValue = collection["ConfigValue"]
                    };
                } else 
                {
                    o.ConfigValue = collection["ConfigValue"];
                }
                TempData["Result"] = o.Save();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Config/Delete/5
        public ActionResult Delete(string code,string key)
        {
            var o = new SystemConfig().Gets().Where(e => e.ConfigCode.Equals(code) && e.ConfigKey.Equals(key)).FirstOrDefault();
            return View(o);
        }

        // POST: Config/Delete/5
        [HttpPost]
        public ActionResult Delete(string code,string key, FormCollection collection)
        {
            try
            {
                if (collection["confirm"].Equals(code))
                {
                    // TODO: Add delete logic here
                    var o = new SystemConfig().Gets().Where(e => e.ConfigCode.Equals(code) && e.ConfigKey.Equals(key)).FirstOrDefault();

                    var i = o.Delete();
                    TempData["Result"] = new ErrorResponse()
                    {
                        success = true,
                        error = "OK",
                        data = i + " rows Deleted"
                    };
                }
                else
                {
                    TempData["Result"] = new ErrorResponse()
                    {
                        success = true,
                        error = "ERR",
                        data = "Cannot Delete"
                    };
                }
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
