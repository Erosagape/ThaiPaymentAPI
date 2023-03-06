using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThaiPaymentAPI.Models
{
    public class ErrorResponse
    {
        public bool success { get; set; }
        public string error { get; set; }
        public string data { get; set; }
    }
}