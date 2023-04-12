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
        public static string PRDSecretID => "AE52C93E03535A0698FB509B2BE655CEEDCD98868C1E8EF473578DCE0F9E4A27";
        public static string PRDEndpointPaymentToken => "https://pgw.2c2p.com/payment/4.1/paymentToken";
    }
}