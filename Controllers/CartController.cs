using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ThaiPaymentAPI.Models;

namespace ThaiPaymentAPI.Controllers
{
    public class CartController : Controller
    {
        private void LoadTempData()
        {
            TempData["UserIP"] = Request.UserHostAddress;
            if (Request.UserHostAddress.Equals("::1"))
                TempData["UserIP"] = "127.0.0.1";
            TempData["UserAgent"] = Request.UserAgent;
            string user = "Guest";
            if (TempData["UserLogin"] != null)
            {
                user = TempData["UserLogin"].ToString();
            }
            else
            {
                TempData["UserLogin"] = user;
            };
            if (TempData["Message"] != null)
            {
                ViewBag.Message = (ErrorResponse)TempData["Message"];
            }
            else
            {
                ViewBag.Message = new ErrorResponse()
                {
                    success = true,
                    data = "",
                    error = "Ready"
                };
            }
            ViewBag.User = user;
            ViewBag.UserIP = TempData["UserIP"];
            ViewBag.UserAgent = TempData["UserAgent"];
        }
        private IEnumerable<ShoppingCart> GetCurrentCart(bool getall=false)
        {
            return new ShoppingCart().Gets((string)ViewBag.UserIP, (string)ViewBag.User, (string)ViewBag.UserAgent,getall);
        }
        // GET: Cart
        public ActionResult Index()
        {
            LoadTempData();
            var cart = GetCurrentCart();            
            return View(cart);
        }
        public ActionResult Add(string id)
        {
            LoadTempData();
            var cart = GetCurrentCart(true);
            if (id!= "")
            {
                var data = new OrderDetail().Gets().Where(e => e.order_id.Equals(id)).FirstOrDefault();
                int qty = 1;
                if (Request.QueryString["qty"] != null)
                {
                    qty = Convert.ToInt32(Request.QueryString["qty"]);
                }
                double rate = Convert.ToInt32(new Models.SystemConfig() { ConfigCode = "DEFAULT", ConfigKey = "RATE_" + data.currency }.GetValue("1")); 
                if (Request.QueryString["rate"] != null)
                {
                    rate = Convert.ToDouble(Request.QueryString["rate"]);
                }
                if (data.order_id.Equals(id))
                {
                    var prd = new ShoppingCart()
                    {
                        from_browser = ViewBag.UserAgent,
                        from_ip = ViewBag.UserIP,
                        from_user = ViewBag.User,
                        seq = (cart.ToList().Count>0? cart.Max(e=>e.seq) + 1:1),
                        order_id = data.order_id,
                        order_price = data.order_amount,
                        order_qty = qty,
                        currency_payment = data.currency,
                        currency_convert = "THB",
                        exchangerate = rate,
                        order_amount = data.order_amount*qty,
                        order_date = DateTime.Now,
                        order_status = 1,
                        order_total = data.order_amount*qty*rate,
                        payment_date= (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                        payment_no="",
                        receipt_date= (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                        receipt_no=""
                    };
                    var result = prd.Save(prd.from_ip,prd.from_user,prd.from_browser);
                    TempData["Message"] = result;
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Delete(int id)
        {
            LoadTempData();
            var cart = GetCurrentCart();
            var data = cart.Where(e => e.seq.Equals(id)).FirstOrDefault();
            if (data!=null)
            {
                TempData["Message"]=data.Delete(data.from_ip, data.from_user, data.from_browser, data.seq);                    
            }
            return RedirectToAction("Index");
        }
        public ActionResult Checkout()
        {
            LoadTempData();
            var cart = TempData["CurrentCart"];
            if (cart == null)
                cart = GetCurrentCart().Where(e => e.order_status.Equals(1)).ToList();
            var result = new ErrorResponse()
            {
                success = true,
                data = "",
                error = "Ready"
            };
            ViewBag.Message = result;
            if (TempData["Message"] != null)
            {
                ViewBag.Message = (ErrorResponse)TempData["Message"];
            }
            TempData["Message"] = ViewBag.Message;
            return View(cart);
        }

        [HttpPost]
        [ActionName("Index")]
        public ActionResult Edit(FormCollection form)
        {
            LoadTempData();
            var cart = GetCurrentCart();
            var seq = Convert.ToInt32(form["seq"]);
            var qty = Convert.ToInt32(form["qty"]);
            var status = form["status"];
            var data = cart.FirstOrDefault(e => e.seq.Equals(seq));
            if (data != null)
            {
                if (status != null)
                {
                    data.order_status = 1;
                } else
                {
                    data.order_status = 0;
                }
                data.order_qty = Convert.ToInt32(qty);
                data.order_amount = data.order_qty * data.order_price;
                data.order_total = data.order_amount * data.exchangerate;                
                ViewBag.Message=data.Save(data.from_ip,data.from_user, data.from_browser);
            }
            TempData["Message"] = ViewBag.Message;
            return RedirectToAction("Index");
        }
    }
}