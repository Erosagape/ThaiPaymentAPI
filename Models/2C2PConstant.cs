using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThaiPaymentAPI.Models
{
    public static class _2C2PConstant
    {
        public static string UATMerchantID => "JT04";
        public static string UATSecretID => "CD229682D3297390B9F66FF4020B758F4A5E625AF4992E5D75D311D6458B38E2";
        public static string UATEndpointPaymentToken => "https://sandbox-pgw.2c2p.com/payment/4.1/paymentToken";
        public static string PRDMerchantID => "764764000012617";
        public static string PRDSecretID => "7A4BD6C717B46F76E1EA1BB1DC656417ACFD3D73FEA995EFE826BB37CAE0DB66";
        public static string PRDEndpointPaymentToken => "https://pgw.2c2p.com/payment/4.1/paymentToken";
    }
}