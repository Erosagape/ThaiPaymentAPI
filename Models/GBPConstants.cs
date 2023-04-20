using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ThaiPaymentAPI.Models
{
    public static class GBPConstants
    {
        public static string PRDEnvironmentUrl => "https://api.gbprimepay.com";               
        public static string PRDMerchantID => "gbp13738";
        public static string PRDSecretKey => "4p6CA4qg7ebs3EgyGDqa6ZwnoB54e4m8";
        public static string PRDPublicKey => "XRhskUAmmPoaa7C89BwcRdwNVRoHahiW";

        public static string UATEnvironmentUrl => "https://api.globalprimepay.com";
        public static string UATMerchantID => "gbp10599";
        public static string UATSecretKey => "erFK008OLMwAPD2VfOrZYWvurqQ85H6J";
        public static string UATPublicKey => "GqpOORx7Iw9LvkDBsNixSaAH7rqjwuc3";

        public static string AuthenticationUrl => "/unified/authToken/create";
        public static string UnifiedAPIUrl => "/unified/transaction";
        public static string TokenChargeUrl => "/v2/tokens/charge";
        public static string Call3DSecureUrl=>"/v2/tokens/3d_secured";
        public static string TokenCreateUrl => "/v2/tokens";
    }
    public class GBPManager
    {
        public static GBPResponseToken GetTokenResultPRD()
        {
            var client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            var requestData = new Dictionary<string, string>{
                { "secretKey", Models.GBPConstants.PRDSecretKey },
                { "publicKey", Models.GBPConstants.PRDPublicKey }
            };
            var endpoint = Models.GBPConstants.PRDEnvironmentUrl + Models.GBPConstants.AuthenticationUrl;
            var requestStr = JsonConvert.SerializeObject(requestData);
            var responseText = client.UploadString(endpoint, "POST", requestStr);
            return JsonConvert.DeserializeObject<Models.GBPResponseToken>(responseText);            
        }
        public static GBPResponseToken GetTokenResultUAT()
        {
            var client = new WebClient();
            client.Headers.Add("Content-Type", "application/json");
            var requestData = new Dictionary<string, string>{
                { "secretKey", Models.GBPConstants.UATSecretKey },
                { "publicKey", Models.GBPConstants.UATPublicKey }
            };
            var endpoint = Models.GBPConstants.UATEnvironmentUrl + Models.GBPConstants.AuthenticationUrl;
            var requestStr = JsonConvert.SerializeObject(requestData);
            var responseText = client.UploadString(endpoint, "POST", requestStr);
            return JsonConvert.DeserializeObject<Models.GBPResponseToken>(responseText);
        }
        public static GBPResponseUnified Payment(GBPRequestPaymentTransaction order,string token,string paytype)
        {
            var client = new WebClient();
            client.Headers.Add("content-type", "application/json");
            client.Headers.Add("Authorization", token);
            var requestData = new GBPRequestUnified
            {
                apiType = "PC",
                createPaymentTransactionRequest = order
            };
            var endpoint = Models.GBPConstants.PRDEnvironmentUrl + Models.GBPConstants.UnifiedAPIUrl;
            if (paytype == "UAT")
                endpoint = Models.GBPConstants.UATEnvironmentUrl + Models.GBPConstants.UnifiedAPIUrl;
            var requestStr = JsonConvert.SerializeObject(requestData);
            var responseText = client.UploadString(endpoint, "POST", requestStr);
            return JsonConvert.DeserializeObject<GBPResponseUnified>(responseText);
        }
        public static string TestPayment(string token)
        {
            var client = new WebClient();
            client.Headers.Add("content-type", "application/json");
            client.Headers.Add("Authorization", token);
            var payload = new GBPRequestPaymentTransaction()
            {
                referenceNo = DateTime.Now.ToString("yyyyMMddHHMMss"),
                paymentType = "C",
                amount = 500,
                cardNumber = "5258860689905862",
                expirationMonth = "02",
                expirationYear = "25",
                securityCode = "950",
                rememberCard = false,
                otp = "Y",
                detail = "TEST",
                responseUrl="https://www.aih-consultant.com/backend/gbprime/result",
                backgroundUrl = "https://www.aih-consultant.com/backend/gbprime/result"                
            };
            var requestData = new GBPRequestUnified {
                apiType ="PC",
                createPaymentTransactionRequest =payload
            };
            var endpoint = Models.GBPConstants.UATEnvironmentUrl + Models.GBPConstants.UnifiedAPIUrl;
            var requestStr = JsonConvert.SerializeObject(requestData);
            var responseText = client.UploadString(endpoint, "POST", requestStr);
            return responseText;
        }
    }
    public class GBPResponseCallBack
    {
        public double amount { get; set; }
        public string referenceNo { get; set; }
        public string gbpReferenceNo { get; set; }
        public string currencyCode { get; set; }
        public string resultCode { get; set; }
        public string paymentType { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string cardNo { get; set; }
        public string cardHolderName { get; set; }
        public string detail { get; set; }
        public string customerName { get; set; }
        public string customerEmail { get; set; }
        public string customerAddress { get; set; }
        public string customerTelephone { get; set; }
        public string merchantDefined1 { get; set; }
        public string merchantDefined2 { get; set; }
        public string merchantDefined3 { get; set; }
        public string merchantDefined4 { get; set; }
        public string merchantDefined5 { get; set; }
        public string retryFlag { get; set; }
        public ErrorResponse SaveLog()
        {
            return new ActionLog()
            {
                log_action="RES",
                log_datetime=DateTime.Now,
                log_error=false,
                log_message=(this.resultCode.Equals("00") ?"Success":"Failed"),
                log_data=Newtonsoft.Json.JsonConvert.SerializeObject(this),
                log_source="GBPResponseCallBack",
                log_stacktrace="",
            }.Save();
        }
    }
    public class GBPResponseUnified
    {
        public string apiType { get; set; }
        public string status { get; set; }
        public string resultCode { get; set; }
        public string resultMessage { get; set; }
        public string redirectUrl { get; set; }
        public string referenceNo { get; set; }
        public string token { get; set; }
        public string transactionStatus { get; set; }
        public string gbpReferenceNo { get; set; }
        public string paymentType { get; set; }
        public string detail { get; set; }
        public string customerName { get; set; }
        public string customerEmail { get; set; }
        public string customerAddress { get; set; }
        public string customerTelephone { get; set; }
        public string merchantDefined1 { get; set; }
        public string merchantDefined2 { get; set; }
        public string merchantDefined3 { get; set; }
        public string merchantDefined4 { get; set; }
        public string merchantDefined5 { get; set; }
    }
    public class GBPRequestUnified
    {
        public string apiType { get; set; }
        public GBPRequestPaymentTransaction createPaymentTransactionRequest { get; set; }
    }
    public class GBPRequestCardPayment
    {
        public double amount { get; set; }
        public string referenceNo { get; set; }
        public string detail { get; set; }
        public string customerName { get; set; }
        public string customerEmail { get; set; }
        public string customerAddress { get; set; }
        public string customerTelephone { get; set; }
        public Dictionary<string,string> card { get; set; }
        public string otp { get; set; }
        public string responseUrl { get; set; }
        public string backgroundUrl { get; set; }
    }
    public class GBPRequestPaymentTransaction
    {
        public string referenceNo { get; set; }
        public string paymentType { get; set; }
        public double amount { get; set; }
        public string cardNumber { get; set; }
        public string expirationMonth { get; set; }
        public string expirationYear { get; set; }
        public string securityCode { get; set; }
        public string cardToken { get; set; }
        public bool rememberCard { get; set; }
        public string otp { get; set; }
        public string responseUrl { get; set; }
        public string backgroundUrl { get; set; }
        public string detail { get; set; }
        public string customerName { get; set; }
        public string customerEmail { get; set; }
        public string customerAddress { get; set; }
        public string customerTelephone { get; set; }
        public string merchantDefined1 { get; set; }
        public string merchantDefined2 { get; set; }
        public string merchantDefined3 { get; set; }
        public string merchantDefined4 { get; set; }
        public string merchantDefined5 { get; set; }

    }
    public class GBPResponseToken
    {
        public string resultCode { get; set; }
        public string resultMessage { get; set; }
        public string authToken { get; set; }
        public int expireIn { get; set; }
    }
    public class GBPCardInfo
    {
        public string number { get; set; }
        public string expirationMonth { get; set; }
        public string expirationYear { get; set; }
        public string securityCode { get; set; }
        public string name { get; set; }
    }
    public class GBPRequestCardToken
    {
        public bool rememberCard { get; set; }
        public GBPCardInfo card { get; set; }
    }
}