using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
namespace ThaiPaymentAPI.Models
{
    public static class INETConstants
    {
        public static string INETMerchantKeyUAT = Properties.Settings.Default.INETMerchantKeyUAT;
        public static string INETMerchantKeyPRD = Properties.Settings.Default.INETMerchantKeyPRD;
        public static string INETUrlOrderPlaceUAT = Properties.Settings.Default.INETUrlOrderPlaceUAT;
        public static string INETUrlOrderPlacePRD = Properties.Settings.Default.INETUrlOrderPlacePRD;
        public static string INETCallbackUrl = Properties.Settings.Default.INETCallbackUrl;
        public static string[] Language=new string[] { "E","T" };
        public static Dictionary<string,int> Currency
        {
            get
            {
                var values= new Dictionary<string, int>();
                values.Add("THB", 764);
                return values;
            }
        }        
        public static Dictionary<string,string> PaymentType
        {
            get
            {
                var values = new Dictionary<string, string>();
                values.Add("CR", "Credit Card");
                values.Add("IN", "Credit Card (Installment)");
                values.Add("AC", "Internet Banking (Citizen)");
                values.Add("CO", "Internet Banking (Cooperate)");
                values.Add("QR", "QR Payment");
                return values;
            }
        }
        public static Dictionary<string,string[]> PaymentBanks
        {
            get
            {
                var values = new Dictionary<string, string[]>();
                values.Add("CR", new string[] { "KTB" });
                values.Add("IN", new string[] { "KTB" });
                values.Add("AC", new string[] { "KTB","BAY","BBL" });
                values.Add("CO", new string[] { "KTB", "BAY", "BBL" });
                values.Add("QR", new string[] { "BAY", "BBL" });
                return values;
            }
        }
        public static Dictionary<string,string> InstallmentType
        {
            get
            {
                var values = new Dictionary<string, string>();
                values.Add("01", "Zero rate");
                values.Add("02", "Fixed rate");
                return values;
            }
        }
    }
}