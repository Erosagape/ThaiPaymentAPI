using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data.SqlClient;
using Newtonsoft.Json;
using ThaiPaymentAPI.Models;
using Dapper;
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
        public string Post([FromBody] object value)
        {
            //if (value.merchant_id == null)
            //{
            //    return "No data to process";
            //}
            return JsonConvert.SerializeObject(value);
            //var msg = value.Save();            
            //return msg.error;
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