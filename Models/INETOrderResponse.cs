using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
    }
}