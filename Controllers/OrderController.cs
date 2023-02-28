﻿using System;
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
                return View("TestResult", obj);
            }            
        }
    }
}