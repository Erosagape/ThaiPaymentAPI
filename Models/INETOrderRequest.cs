using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace ThaiPaymentAPI.Models
{
    public class INETOrderRequest
    {
        public string key { get; set; }
        public string orderId { get; set; }
        public string orderDesc { get; set; }
        public string amount { get; set; }
        public string apUrl { get; set; }
        public string ud1 { get; set; }
        public string ud2 { get; set; }
        public string ud3 { get; set; }
        public string ud4 { get; set; }
        public string ud5 { get; set; }
        public string lang { get; set; }
        public string bankNo { get; set; }
        public string currCode { get; set; }
        public string payType { get; set; }
        public string inst { get; set; }
        public string term { get; set; }
        public string supplierId { get; set; }
        public string productId { get; set; }
        public ErrorResponse Save()
        {
            return new ActionLog()
            {
                log_action="REQ",
                log_message=this.orderDesc,
                log_data=JsonConvert.SerializeObject(this),
                log_source="INETOrderRequest.Save()",
                log_error=false,
                log_stacktrace=this.orderId
            }.Save();
        }
    }
}