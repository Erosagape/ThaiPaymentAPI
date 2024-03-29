﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using Newtonsoft.Json;
using ThaiPaymentAPI.Models;
using Dapper;
using System.Collections.Specialized;
using System.Net.Http.Formatting;

namespace ThaiPaymentAPI.Controllers
{
    public class PaymentController : ApiController
    {
        // GET api/<controller>
        public List<INETPaymentResponse> Get()
        {
            List<INETPaymentResponse> lists = new List<INETPaymentResponse>();
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
            {
                cn.Open();
                string sql = "select a.*,b.event,b.merchant_id as merchant_idh,b.timestamp from INET_Payment_Response_Detail a inner join INET_Payment_Response b on a.id=b.id";
                using(SqlDataReader rd=new SqlCommand(sql, cn).ExecuteReader())
                {
                    while (rd.Read())
                    {
                        var hdr = new INETPaymentResponse();
                        hdr.event_text = rd.GetString(rd.GetOrdinal("event"));
                        hdr.merchant_id = rd.GetString(rd.GetOrdinal("merchant_idh"));
                        hdr.timestamp = rd.GetString(rd.GetOrdinal("timestamp"));
                        hdr.detail = new INETPaymentResponseDetail
                        {
                            response_code = rd.GetString(rd.GetOrdinal("response_code")),
                            response_message = rd.GetString(rd.GetOrdinal("response_message")),
                            merchant_id = rd.GetString(rd.GetOrdinal("merchant_id")),
                            payment_reference_id = rd.GetString(rd.GetOrdinal("payment_reference_id")),
                            order_id = rd.GetString(rd.GetOrdinal("order_id")),
                            order_description = rd.GetString(rd.GetOrdinal("order_description")),
                            payment_type = rd.GetString(rd.GetOrdinal("payment_type")),
                            payment_acquirer_bank = rd.GetString(rd.GetOrdinal("payment_acquirer_bank")),
                            receive_amount = rd.GetString(rd.GetOrdinal("receive_amount")),
                            transaction_date = rd.GetString(rd.GetOrdinal("transaction_date")),
                            transaction_time = rd.GetString(rd.GetOrdinal("transaction_time"))                            
                        };
                        lists.Add(hdr);
                    }
                    rd.Close();
                }
            }
            return lists;
        }

        // GET api/<controller>/5
        public INETPaymentResponse Get(string id)
        {
            using(SqlConnection cn=new SqlConnection(ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString))
            {
                string sql = @"select * from INET_Payment_Response_Detail where order_id=@id";
                var dtl = cn.QueryFirstOrDefault<INETPaymentResponseDetail>(sql, new { @id = id });
                sql = @"SELECT event,merchant_id,timestamp FROM INET_Payment_Response WHERE id in(select id from INET_Payment_Response_Detail where order_id=@id)";
                var hdr= cn.QueryFirstOrDefault<INETPaymentResponse>(sql, new { @id = id });
                if(dtl!=null)
                    hdr.detail = dtl;
                return hdr;
            }
        }
        // POST api/<controller>
        public string Post(FormDataCollection postData)
        {
            if (postData == null)
            {
                return "Not found form Data sent";
            }
            var obj = new INETOrderPayment()
            {
                orderId = postData["orderId"],
                merchantId = postData["merchantId"],
                bankNo = postData["bankNo"],
                payType = postData["payType"],
                orderIDRef = postData["orderIdRef"],
                rcode = postData["rcode"],
                rmsg = postData["rmsg"],
                TxnAmount = Convert.ToDouble(postData["TxnAmount"]),
                txnDate = postData["txnDate"],
                txnTime = postData["txnTime"],
                ORDER_DESC = postData["ORDER_DESC"]
            };
            var result = obj.Save();
            var log = new ActionLog()
            {
                log_action = "POST",
                log_source = "api/payment",
                log_data = JsonConvert.SerializeObject(obj),
                log_message = (result.success ? "Save Complete" : result.error),
                log_error = !result.success,
                log_stacktrace = result.data
            }.Save();
            return result.error;
            /*
            string str = "";
            foreach(var val in postData)
            {
                str += (str != "" ? "," : "");
                str += val.Key + "=" + val.Value + "";
            }
            
            ActionLog log = new ActionLog()
            {
                log_action = "RES",
                log_message = "POST",
                log_data = str,
                log_error = false,
                log_source = "api/payment",
                log_stacktrace = DateTime.Now.ToString("yyyyMMddHHMMss")
            };
            return log.Save().error;
            */

        }
        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}