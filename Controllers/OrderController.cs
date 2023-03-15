using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using ThaiPaymentAPI.Models;
namespace ThaiPaymentAPI.Controllers
{
    public class OrderController : Controller
    {
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Test()
        {
            return View();
        }
        public ActionResult TestProduction()
        {
            return View();
        }

        public ActionResult TestPayment()
        {
            var obj = (INETOrderResponse)TempData["payment"];
            return View(obj);
        }
        [HttpPost]
        [ActionName("Test")]
        public ActionResult PostTest()
        {
            var order = new INETOrderRequest()
            {
                key = INETConstants.INETMerchantKeyUAT,
                orderId = Request.Form["orderId"],
                orderDesc = Request.Form["orderDesc"],
                amount = Request.Form["amount"],
                apUrl = INETConstants.INETCallbackUrl,
                ud1 = "",
                ud2 = "",
                ud3 = "",
                ud4 = "",
                ud5 = "",
                lang = Request.Form["lang"],
                bankNo = "KTC",
                currCode = Request.Form["currCode"],
                payType = Request.Form["payType"],
                inst = "",
                term = "",
                supplierId = "",
                productId = ""
            };
            try
            {
                order.Save();
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                
                var req = WebRequest.Create(INETConstants.INETUrlOrderPlaceUAT);
                req.Method = "POST";
                
                var json = JsonConvert.SerializeObject(order);
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
                req.ContentType = "application/json";
                req.ContentLength = bytes.Length;

                var stream = req.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();

                var response = req.GetResponse();
                var res = response.GetResponseStream();
                var reader = new System.IO.StreamReader(res);
                var responseStr = reader.ReadToEnd();
                var obj = JsonConvert.DeserializeObject<INETOrderResponse>(responseStr);
                if (obj.status.Equals("success"))
                {
                    TempData["payment"] = obj;
                    return RedirectToAction("TestPayment");
                }
                obj.Save();
                return View("TestResult",obj);
            }
            catch(Exception e)
            {
                var obj = new INETOrderResponse()
                {
                    status = "500",
                    message = e.Message,
                    link = INETConstants.INETUrlOrderPlaceUAT,
                    token = e.StackTrace,
                    ref1 = "UAT:" + INETConstants.INETMerchantKeyUAT,
                    ref2 = "PRD:" + INETConstants.INETMerchantKeyPRD
                };
                obj.Save();
                return View("TestResult", obj);
            }            
        }
        [HttpPost]
        [ActionName("TestProduction")]
        public ActionResult PostTestProduction()
        {
            var order = new INETOrderRequest()
            {
                key = INETConstants.INETMerchantKeyPRD,
                orderId = Request.Form["orderId"],
                orderDesc = Request.Form["orderDesc"],
                amount = Request.Form["amount"],
                apUrl = INETConstants.INETCallbackUrl,
                ud1 = "",
                ud2 = "",
                ud3 = "",
                ud4 = "",
                ud5 = "",
                lang = Request.Form["lang"],
                bankNo = "KTC",
                currCode = Request.Form["currCode"],
                payType = Request.Form["payType"],
                inst = "",
                term = "",
                supplierId = "",
                productId = ""
            };
            try
            {
                order.Save();
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                var req = WebRequest.Create(INETConstants.INETUrlOrderPlacePRD);
                req.Method = "POST";

                var json = JsonConvert.SerializeObject(order);
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);
                req.ContentType = "application/json";
                req.ContentLength = bytes.Length;
                
                var stream = req.GetRequestStream();
                stream.Write(bytes, 0, bytes.Length);
                stream.Close();

                var response = req.GetResponse();
                var res = response.GetResponseStream();
                var reader = new System.IO.StreamReader(res);
                var responseStr = reader.ReadToEnd();
                var obj = JsonConvert.DeserializeObject<INETOrderResponse>(responseStr);
                if (obj.status.Equals("success"))
                {
                    TempData["payment"] = obj;
                    return RedirectToAction("TestPayment");
                }
                obj.Save();
                return View("TestResult", obj);
            }
            catch (Exception e)
            {
                var obj = new INETOrderResponse()
                {
                    status = "500",
                    message = e.Message,
                    link = INETConstants.INETUrlOrderPlacePRD,
                    token = e.StackTrace,
                    ref1 = "UAT:" + INETConstants.INETMerchantKeyUAT,
                    ref2 = "PRD:" + INETConstants.INETMerchantKeyPRD
                };
                obj.Save();
                return View("TestResult", obj);
            }
        }

        public ActionResult TestPostBack()
        {
            return View();
        }
        public ActionResult Confirmation()
        {
            var obj = new INETOrderPayment();
            ViewData["Result"] = new ErrorResponse()
            {
                data = "Ready",
                error = "0"
            };
            return View(obj);
        }
        [HttpPost]
        [ActionName("Confirmation")]
        public ActionResult PostConfirmation()
        {
            var obj = new INETOrderPayment()
            {
                orderId = Request.Form["orderId"],
                merchantId = Request.Form["merchantId"],
                bankNo = Request.Form["bankNo"],
                payType = Request.Form["payType"],
                orderIDRef = Request.Form["orderIdRef"],
                rcode = Request.Form["rcode"],
                rmsg = Request.Form["rmsg"],
                TxnAmount = Convert.ToDouble(Request.Form["TxnAmount"]),
                txnDate = Request.Form["txnDate"],
                txnTime = Request.Form["txnTime"],
                ORDER_DESC = Request.Form["ORDER_DESC"]
            };
            ViewData["Result"]=obj.Save();
            return View(obj);
        }
        public ActionResult Group()
        {
            var mdl = new OrderGroup().Gets().ToList();
            ViewBag.Message = new ErrorResponse()
            {
                success = true,
                data = "",
                error = "Ready"
            };
            if (TempData["Message"] != null)
            {
                ViewBag.Message = (ErrorResponse)TempData["Message"];
            }
            return View(mdl);
        }   
        public ActionResult GroupEdit()
        {
            var group_code = Request.QueryString["id"];
            if (group_code == null)
            {
                var mdl = new OrderGroup();
                return View(mdl);
            }
            else
            {
                var mdl = new OrderGroup().GetValue(group_code);
                return View(mdl);
            }
        }
        [HttpPost]
        [ActionName("GroupEdit")]
        public ActionResult PostGroupEdit(FormCollection form) 
        {
            var data = new OrderGroup()
            {
                order_group = form["order_group"],
                order_groupname = form["order_groupname"],
                order_groupdesc = form["order_groupdesc"],
                isactive = (form["isactive"]==null? false :true)
            };
            TempData["Message"]=data.Save();
            return RedirectToAction("Group");
        }
        public ActionResult List()
        {
            var group = Request.QueryString["group"];
            if (group != null)
            {
                ViewBag.DataSource = new OrderDetail().Gets().Where(e => e.order_group.Equals(group)).ToList();
            }
            else
            {
                ViewBag.DataSource = new OrderDetail().Gets().ToList();
            };
            if (TempData["Message"] != null)
            {
                ViewData["Message"] = (ErrorResponse)TempData["Message"];
            } else
            {
                ViewData["Message"] = new ErrorResponse()
                {
                    success = true,
                    data = "",
                    error = "Ready"
                };
            }
            return View();
        }
        public ActionResult Edit()
        {
            var id = Request.QueryString["id"];
            var mdl=new OrderDetail();
            if (id != null)
            {
                mdl = new OrderDetail().Gets().Where(e => e.order_id.Equals(id)).FirstOrDefault();
            }
            ViewBag.GroupData = new OrderGroup().Gets().ToList();
            var curr = new SystemConfig().Gets().Where(
                e => e.ConfigCode.Equals("DEFAULT")
                && e.ConfigKey.Equals("ORDER_CURRENCY")
                ).FirstOrDefault().ConfigValue;
            if (curr != "")
            {
                ViewBag.Currencys = curr.Split(',');
            }
            else
            {
                ViewBag.Currencys = new string[] { "USD", "THB" };
            }
            return View(mdl);
        }
        [HttpPost]
        [ActionName("Edit")]
        public ActionResult PostEdit(FormCollection form)
        {
            var file=Request.Files["order_pic"];
            string fname = "";
            if (file.FileName!="")
            {
                fname = "/images/" + file.FileName;
                file.SaveAs(Server.MapPath(fname));                
            }
            var data = new OrderDetail()
            {
                order_id=form["order_id"],
                order_name=form["order_name"],
                order_desc=form["order_desc"],
                order_group=form["order_group"],
                order_pic=fname,
                currency=form["currency"],
                order_actual=Convert.ToDouble(form["order_actual"]),
                order_target=Convert.ToDouble(form["order_target"]),
                order_amount=Convert.ToDouble(form["order_amount"]),
                isactive=(form["isactive"]==null?false:true)
            };
            TempData["Message"] = data.Save();
            return RedirectToAction("List");
        }
        public ActionResult Create()
        {
            ViewBag.GroupData = new OrderGroup().Gets().Where(e=>e.isactive.Equals(true)).ToList();
            var curr = new SystemConfig().Gets().Where(
                e => e.ConfigCode.Equals("DEFAULT")
                && e.ConfigKey.Equals("ORDER_CURRENCY")
                ).FirstOrDefault().ConfigValue.ToString();
            if (curr != "")
            {
                ViewBag.Currencys = curr.Split(',');
            }
            else
            {
                ViewBag.Currencys = new string[] { "USD", "THB" };
            }
            return View();
        }
        public ActionResult Payment(string id)
        {
            if (id!="")
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
                var user = "Guest";
                if (TempData["UserLogin"] != null)
                {
                    user = (string)TempData["UserLogin"];
                }
                var carts = new ShoppingCart().Gets(Request.UserHostAddress,user,Request.UserAgent,true);
                var cart = new List<ShoppingCart>();
                if (data.order_id.Equals(id))
                {
                    var prd = new ShoppingCart()
                    {
                        from_browser = Request.UserAgent,
                        from_ip = Request.UserHostAddress,
                        from_user = user,
                        seq = (carts.ToList().Count > 0 ? carts.Max(e => e.seq) + 1 : 1),
                        order_id = data.order_id,
                        order_price = data.order_amount,
                        order_qty = qty,
                        currency_payment = data.currency,
                        currency_convert = "THB",
                        exchangerate = rate,
                        order_amount = data.order_amount * qty,
                        order_date = DateTime.Now,
                        order_status = 1,
                        order_total = data.order_amount * qty * rate,
                        payment_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                        payment_no = "",
                        receipt_date = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue,
                        receipt_no = ""
                    };
                    var result = prd.Save(prd.from_ip, prd.from_user, prd.from_browser);
                    if (result.success)
                        cart.Add(prd);
                    TempData["CurrentCart"] = cart;
                    TempData["Message"] = result;
                }
            }
            return RedirectToAction("Checkout","Cart");
        }
    }
}