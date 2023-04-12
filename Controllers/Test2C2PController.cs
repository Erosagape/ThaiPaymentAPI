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
        [HttpPost]
        public ActionResult PostOrderPaymentUAT(FormCollection data)
        {
            var payload = new Dictionary<string, object>()
            {
                {"merchantID", _2C2PConstant.UATMerchantID},
                {"invoiceNo", data["orderId"]},
                {"description", data["orderDesc"]},
                {"amount", data["amount"]},
                {"currencyCode", data["currCode"]},
                {"frontendReturnUrl","https://www.aih-consultant.com/backend/test2c2p/result" },
                {"backendReturnUrl","https://www.aih-consultant.com/backend/test2c2p/PostResult" },
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
            return View("Result");
        }
        [HttpPost]
        public ActionResult PostOrderPaymentPRD(FormCollection data)
        {
            var payload = new Dictionary<string, object>()
            {
                {"merchantID", _2C2PConstant.PRDMerchantID},
                {"invoiceNo", data["orderId"]},
                {"description", data["orderDesc"]},
                {"amount", data["amount"]},
                {"currencyCode", data["currCode"]},
                {"frontendReturnUrl","https://www.aih-consultant.com/backend/test2c2p/result" },
                {"backendReturnUrl","https://www.aih-consultant.com/backend/test2c2p/PostResult" },
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
                }
                else
                {
                    ViewBag.Result = e.Message;
                }
            }
            return View("Result");
        }
        [HttpPost]
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
        [HttpPost]
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
            var log = new ActionLog()
            {
                log_action = "LOG",
                log_data = paymentResponse,
                log_error = false,
                log_message = "Post Back",
                log_source = "Test2C2P",
                log_stacktrace = "Test2c2p/PostResponse"
            };
            log.Save();
            TempData["Result"] = paymentResponse;
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
                    try
                    {
                        var base64EncodedBytes = System.Convert.FromBase64String(data[i].ToString());
                        var decodedData = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                        result += "\n" + data.Keys[i].ToString() + " : " + decodedData;
                    }
                    catch (Exception e)
                    {
                        result += "\n" + data.Keys[i].ToString() + " : " + data[i].ToString();
                    }
                }
            }
            var log = new ActionLog()
            {
                log_action = "LOG",
                log_data = result,
                log_error = false,
                log_message = "Post Back",
                log_source = "Test2C2P",
                log_stacktrace = "Test2C2P/PostResult"
            };
            log.Save();
            ViewBag.Result = result;
            return View();
        }
        public ActionResult Result()
        {
            var result = Request.QueryString["paymentResponse"];
            if (TempData["Result"] != null)
            {
                var data = TempData["Result"].ToString();
                var base64EncodedBytes = System.Convert.FromBase64String(data);
                result = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            }            
            if (result == null)
            {
                result = "Ready";
            } else
            {
                var log = new ActionLog()
                {
                    log_action = "LOG",
                    log_data = result,
                    log_error = false,
                    log_message = "Post Front",
                    log_source = "Test2C2P",
                    log_stacktrace = "Test2c2p/Result"
                };
                log.Save();
            }
            ViewBag.Result = result;
            return View();
        }        
        public ActionResult Decode()
        {
            ViewBag.Result = "Ready";
            return View();
        }
        [HttpPost]
        [ActionName("Decode")]
        public ActionResult PostDecode(FormCollection form)
        {
            string paymentResponse = form["responseText"];
            if (paymentResponse != null)
            {
                try
                {
                    var base64EncodedBytes = System.Convert.FromBase64String(paymentResponse);
                    ViewBag.Result= System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
                } catch (Exception e)
                {
                    ViewBag.Result = e.Message;
                }
            } else
            {
                ViewBag.Result = "No data to Convert";
            }
            return View();
        }
    }
}