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
        public static string UATEnvironmentUrl => "https://api.globalprimepay.com";
        public static string PRDMerchantID => "gbp13738";
        public static string UATMerchantID => "gbp10599";
        public static string PRDSecretKey => "4p6CA4qg7ebs3EgyGDqa6ZwnoB54e4m8";
        public static string PRDPublicKey => "XRhskUAmmPoaa7C89BwcRdwNVRoHahiW";
        public static string UATSecretKey => "erFK008OLMwAPD2VfOrZYWvurqQ85H6J";
        public static string UATPublicKey => "GqpOORx7Iw9LvkDBsNixSaAH7rqjwuc3";
        public static string AuthenticationUrl => "/unified/authToken/create";
        public static string UnifiedAPIUrl => "/unified/transaction";
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

    }
    public class GBPResponseToken
    {
        public string resultCode { get; set; }
        public string resultMessage { get; set; }
        public string authToken { get; set; }
        public int expireIn { get; set; }
    }
}