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
        [HttpPost]
        public ActionResult Checkout(FormCollection form)
        {
            TempData["OrderNo"] = form["orderNo"];
            TempData["OrderAmt"] = form["orderAmt"];
            TempData["OrderDetail"] = form["orderDetail"];
            TempData["CustomerName"] = form["customerName"];
            TempData["CustomerAddress"] = form["customerAddress"];
            TempData["CustomerEmail"] = form["customerEmail"];
            TempData["CustomerPhone"] = form["customerPhone"];
            return RedirectToAction("CardPayment");
        }
        public ActionResult CardPayment()
        {
            ViewBag.OrderNo = DateTime.Now.ToString("yyyyMMddHHMMss");            
            if (TempData["OrderNo"] != null)
            {
                ViewBag.OrderNo = TempData["OrderNo"];
                TempData["OrderNo"] = ViewBag.OrderNo;
            }
            ViewBag.OrderAmount = 0;
            if (TempData["OrderAmt"] != null)
            {
                ViewBag.OrderAmt = TempData["OrderAmt"];
                TempData["OrderAmt"] = ViewBag.OrderAmt;
            }
            ViewBag.OrderDetail = "";
            if (TempData["OrderDetail"] != null)
            {
                ViewBag.OrderDetail = TempData["OrderDetail"];
                TempData["OrderDetail"] = ViewBag.OrderDetail;
            }
            ViewBag.CustomerName = "";
            if (TempData["CustomerName"] != null)
            {
                ViewBag.CustomerName = TempData["CustomerName"];
                TempData["CustomerName"] = ViewBag.CustomerName;
            }
            ViewBag.CustomerAddress = "";
            if (TempData["CustomerAddress"] != null)
            {
                ViewBag.CustomerAddress = TempData["CustomerAddress"];
                TempData["CustomerAddress"] = ViewBag.CustomerAddress;
            }
            ViewBag.CustomerEmail = "";
            if (TempData["CustomerEmail"] != null)
            {
                ViewBag.CustomerEmail = TempData["CustomerEmail"];
                TempData["CustomerEmail"] = ViewBag.CustomerEmail;
            }
            ViewBag.CustomerPhone = "";
            if (TempData["CustomerPhone"] != null)
            {
                ViewBag.CustomerPhone = TempData["CustomerPhone"];
                TempData["CustomerPhone"] = ViewBag.CustomerPhone;
            }
            ViewBag.Result = "Ready";
            return View();
        }

        public ActionResult Payment()
        {
            return View();
        }
        public ActionResult TestTokenCardPayment()
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var endpoint = Models.GBPConstants.UATEnvironmentUrl + Models.GBPConstants.TokenCreateUrl;
                var req = WebRequest.Create(endpoint);
                var keyBytes =  System.Text.UTF8Encoding.UTF8.GetBytes(Models.GBPConstants.UATPublicKey);                
                var authorizeStr = String.Format("Basic {0}:", Convert.ToBase64String(keyBytes));
                req.Method = "POST";
                req.Headers.Add("Authorization", authorizeStr);

                var payload = new Models.GBPRequestCardToken()
                {
                    rememberCard = false,
                    card = new Models.GBPCardInfo
                    {
                        number = "5258860689905862",
                        expirationMonth = "02",
                        expirationYear = "25",
                        securityCode = "950",
                        name = "Test Card UAT"
                    }
                };
                var payloadString = JsonConvert.SerializeObject(payload);
                var payloadBytes = System.Text.Encoding.UTF8.GetBytes(payloadString);                
                req.ContentType = "application/json";
                req.ContentLength = payloadBytes.Length;

                var stream = req.GetRequestStream();
                stream.Write(payloadBytes, 0, payloadBytes.Length);
                stream.Close();

                var response = req.GetResponse();
                var res = response.GetResponseStream();
                var reader = new System.IO.StreamReader(res);
                var responseStr = reader.ReadToEnd();
                return Content(responseStr, "text/json");
            } catch (Exception e)
            {
                return Content(e.Message, "text/html");
            }
        }
        [HttpPost]
        [ActionName("CardPayment")]
        public ActionResult PostCardPayment(FormCollection form)
        {
            var paytype = form["payType"];
            var amount = Convert.ToDouble(form["orderAmt"]);
            var detail = form["orderDetail"];
            var custName = form["customerName"];
            var custEmail = form["customerEmail"];
            var custAddress = form["customerAddress"];
            var custTelephone = form["customerTelephone"];
            var cardNo = form["cardNumber"];
            var secureCode = form["secureCode"];
            var expireMonth = form["expireMonth"];
            var expireYear = form["expireYear"];
            var orderNo = form["orderNo"];
            var objToken = (paytype=="PRD"? Models.GBPManager.GetTokenResultPRD(): Models.GBPManager.GetTokenResultUAT());
            if (objToken.resultCode.Equals("00"))
            {
                var tokenStr = objToken.authToken;
                var order = new Models.GBPRequestPaymentTransaction()
                {
                    amount=amount,
                    detail=detail,
                    customerAddress=custAddress,
                    customerName=custName,
                    customerEmail=custEmail,
                    customerTelephone=custTelephone,                    
                    cardNumber=cardNo,
                    rememberCard=false,
                    securityCode=secureCode,
                    expirationMonth=expireMonth,
                    expirationYear=expireYear,
                    otp="Y",
                    paymentType="C",
                    referenceNo=orderNo,
                    responseUrl = "https://www.aih-consultant.com/backend/gbprime/result",
                    backgroundUrl = "https://www.aih-consultant.com/backend/gbprime/result"
                };
                var res = Models.GBPManager.Payment(order, tokenStr,paytype);
                if (res.resultCode.Equals("00"))
                {
                    return Redirect(res.redirectUrl);
                } else
                {
                    ViewBag.Result = res.resultMessage;
                }
            }
            else
            {
                ViewBag.Result =objToken.resultMessage;
            }
            ViewBag.Result = "Ready";
            return View();
        }
        public ActionResult TestUnifiedPayment()
        {
            string content = "Testing";
            var objTokenuat = Models.GBPManager.GetTokenResultUAT();
            var objTokenprd = Models.GBPManager.GetTokenResultPRD();
            content += "\nToken UAT = " + objTokenuat.resultMessage + " : " + objTokenuat.authToken;
            content += "\nToken PRD = "+ objTokenprd.resultMessage +" : " + objTokenprd.authToken;
            var testresponse = JsonConvert.DeserializeObject<Models.GBPResponseUnified>(Models.GBPManager.TestPayment(objTokenuat.authToken));
            content += "\nTest Payment UAT = " + testresponse.resultMessage;
            if (testresponse.resultCode.Equals("00"))
            {
                return Redirect(testresponse.redirectUrl);
            }
            return Content(content, "text/html");
        }
        public ActionResult Token()
        {
            //orderNo=20230417110435&orderAmount=500&transTypw=test&gbToken=d11d6282-3ad1-4567-ad79-6742208273c0&gbRememberCard=false
            ViewBag.GBPToken = Request.QueryString["gbToken"];
            ViewBag.TransType = Request.QueryString["transType"];
            ViewBag.OrderNo = Request.QueryString["orderNo"];
            ViewBag.OrderAmount = Convert.ToDouble(Request.QueryString["orderAmount"]);
            ViewBag.RememberCard = (Request.QueryString["gbRememberCard"] == "true");
            return View();
        }
        [HttpPost]
        [ActionName("Token")]
        public ActionResult PostToken(FormCollection form)
        {
            var token = form["token"];
            var transType = form["transType"];
            var orderNo = form["orderNo"];
            var orderAmt = Convert.ToDouble(form["orderAmount"]);
            var rememberCard = (form["rememberCard"] == "true");
            var obj = new Models.GBPRequestCardPayment()
            {
                amount = orderAmt,
                referenceNo = orderNo,
                card = new Dictionary<string, string>()
                {
                    {"token",token }
                },
                detail = "Test Products",
                otp = "Y",
                customerName = "TEST COMPANY",
                customerEmail = "all@tawantech.co.th",
                customerAddress = "TEST ADDRESS",
                customerTelephone = "+66823320506",
                backgroundUrl = "Result",
                responseUrl = "Result"
            };
            var endpoint = Models.GBPConstants.UATEnvironmentUrl + Models.GBPConstants.TokenChargeUrl;
            var requestStr = JsonConvert.SerializeObject(obj);
            var secretkeyBytes = System.Text.Encoding.ASCII.GetBytes(Models.GBPConstants.UATSecretKey);

            var client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            client.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(secretkeyBytes) + ":");
            var responseText = client.UploadString(endpoint, "POST", requestStr);
            ViewBag.Result= responseText;
            return View("Result");
        }
        [HttpPost]
        [ActionName("Index")]
        public ActionResult PostFormPayment(FormCollection form)
        {
            ViewBag.TotalAmount = form["amount"];
            ViewBag.PayType = form["paytype"];
            return View("Payment");
        }
        [HttpPost]
        public ActionResult PostResultForm(FormCollection form)
        {
            string response = "";
            foreach(var data in form)
            {
                response += "<br/>" + data.ToString();
            }
            ViewBag.Result = response;
            return View("Result");
        }
        [HttpPost]
        [ActionName("Result")]
        public ActionResult PostResult(Models.GBPResponseCallBack data)
        {
            string response = data.SaveLog().error;
            ViewBag.Result = response;
            return View("Result");
        }
        public ActionResult Result()
        {
            ViewBag.Result = "Ready";
            return View();
        }
    }
}