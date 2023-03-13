using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThaiPaymentAPI.Models
{
    public class ShoppingCart
    {
        public string from_ip { get; set; }
        public string from_user { get; set; }
        public List<Cart> order_list { get; set; }
    }
    public class Cart
    {
        public string order_id { get; set; }
        public int order_qty { get; set; }
        public double order_price { get; set; }
        public double order_amount { get; set; }
        public string currency_payment { get; set; }
        public double exchangerate { get; set; }
        public string currency_convert { get; set; }
        public double order_total { get; set; }
    }
}