using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
namespace ThaiPaymentAPI.Models
{
    public class INETOrderResponse
    {
        public string status { get; set; }
        public string message { get; set; }
        public string token { get; set; }
        public string link { get; set; }
        public string ref1 { get; set; }
        public string ref2 { get; set; }
        public ErrorResponse Save()
        {
            return new ActionLog()
            {
                log_action = "RES",
                log_message = this.message,
                log_error = !this.status.ToLower().Equals("success"),
                log_data = JsonConvert.SerializeObject(this),
                log_source = "INETOrderResponse.Save()",
                log_stacktrace = "REF1:" + this.ref1 + ",REF2:" + this.ref2
            }.Save();
        }
    }
}