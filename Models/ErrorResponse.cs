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
        public ErrorResponse()
        {
            error = "OK";
        }
        public ErrorResponse(bool sc,string er,string dt)
        {
            success = sc;
            error = er;
            data = dt;
        }
    }
}