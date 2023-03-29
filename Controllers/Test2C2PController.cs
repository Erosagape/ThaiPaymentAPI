using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Web.Mvc;
using Jose;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;
using JWT.Exceptions;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using ThaiPaymentAPI.Models;
namespace ThaiPaymentAPI.Controllers
{
    public class Test2C2PController : Controller
    {
        // GET: Test2C2P
        public ActionResult Index()
        {
            ViewBag.Result = "Ready";
            return View();
        }
        public ActionResult Index2()
        {
            ViewBag.Result = "Ready";
            return View();
        }
        public ActionResult PostPaymentPRD(FormCollection data)
        {
            var payload = new Dictionary<string, object>()
            {
                {"merchantID", _2C2PConstant.PRDMerchantID},
                {"invoiceNo", data["invoiceNo"]},
                {"description", data["description"]},
                {"amount", data["amount"]},
                {"currencyCode", data["currencyCode"]}
            };
            string secretkey = _2C2PConstant.PRDSecretID;
            var algorithm = new HMACSHA256Algorithm();
            var serializer = new JsonNetSerializer();
            var urlEncoder = new JwtBase64UrlEncoder();
            var Encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var jwtToken = Encoder.Encode(payload, secretkey);
            var requestData = new Dictionary<string, string>{
                { "payload", jwtToken}
            };
            var requestStr = JsonConvert.SerializeObject(requestData);
            var client = new WebClient();
            client.Headers.Add("Content-Type", "application/*+json");
            client.Headers.Add("Accept", "text/plain");
            var responseText = client.UploadString(_2C2PConstant.PRDEndpointPaymentToken, "POST", requestStr);
            var responseObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
            var payloadString = "";
            try
            {
                var responseToken = responseObj["payload"];
                var Provider = new UtcDateTimeProvider();
                var validator = new JwtValidator(serializer, Provider);
                var Decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
                payloadString = Decoder.Decode(responseToken, secretkey, true);
                var payloadObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(payloadString);
                var paymentUrl = payloadObj["webPaymentUrl"];
                return Redirect(paymentUrl);
            }
            catch (Exception e)
            {
                if (responseObj.Count > 0)
                {
                    ViewBag.Result = JsonConvert.SerializeObject(responseObj);
                } else
                {
                    ViewBag.Result = e.Message;
                }
            }
            return View("Index2");
        }
        public ActionResult PostPaymentUAT(FormCollection data)
        {
            var payload = new Dictionary<string, object>()
            {
                {"merchantID", _2C2PConstant.UATMerchantID},
                {"invoiceNo", data["invoiceNo"]},
                {"description", data["description"]},
                {"amount", data["amount"]},
                {"currencyCode", data["currencyCode"]}
            };
            string secretkey = _2C2PConstant.UATSecretID;
            var algorithm = new HMACSHA256Algorithm();
            var serializer = new JsonNetSerializer();
            var urlEncoder = new JwtBase64UrlEncoder();
            var Encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            var jwtToken = Encoder.Encode(payload, secretkey);
            var requestData = new Dictionary<string, string>{
                { "payload", jwtToken}
            };
            var requestStr = JsonConvert.SerializeObject(requestData);
            var client = new WebClient();
            client.Headers.Add("Content-Type", "application/*+json");
            client.Headers.Add("Accept", "text/plain");
            var responseText = client.UploadString(_2C2PConstant.UATEndpointPaymentToken, "POST", requestStr);
            var responseObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseText);
            var payloadString = "";
            try                
            {
                var responseToken = responseObj["payload"];
                var Provider = new UtcDateTimeProvider();
                var validator = new JwtValidator(serializer, Provider);
                var Decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);
                payloadString = Decoder.Decode(responseToken, secretkey, true);
                var payloadObj = JsonConvert.DeserializeObject<Dictionary<string, string>>(payloadString);
                var paymentUrl = payloadObj["webPaymentUrl"];
                return Redirect(paymentUrl);
            }
            catch (Exception e)
            {
                if (responseObj.Count > 0)
                {
                    ViewBag.Result = JsonConvert.SerializeObject(responseObj);
                }
                else
                {
                    ViewBag.Result = e.Message;
                }
            }
            return View("Index");
        }
        [HttpPost]
        public ActionResult PostResponse(string paymentResponse)
        {
            ViewBag.Result = paymentResponse;
            return View("Result");
        }
        [HttpPost]
        [ActionName("Result")]
        public ActionResult PostResult(FormCollection data)
        {
            var result = "Ready";
            if (data.Count > 0)
            {
                result = "";
                for(int i=0;i<data.Count;i++)
                {
                    result += "\n"+ data.Keys[i].ToString() +" : " + data[i].ToString();
                }
            }
            ViewBag.Result = result;
            return View();
        }
        public ActionResult Result()
        {
            var result = Request.QueryString["paymentResponse"];
            if (result == null)
                result = "Ready";
            ViewBag.Result = result;
            return View();
        }
    }
}