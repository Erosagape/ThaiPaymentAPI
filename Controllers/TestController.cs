using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using ThaiPaymentAPI.Models;
namespace ThaiPaymentAPI.Controllers
{
    public class TestController : ApiController
    {
        // GET api/<controller>
        public INETPaymentResponse _default = new INETPaymentResponse()
        {
            merchant_id = "990300000133",
            timestamp = "2023-01-01T12:30:15+07:00",
            event_text = "payment_status_change",
            detail = new INETPaymentResponseDetail()
            {
                response_code = "0",
                response_message = "Success",
                merchant_id = "990300000133",
                order_id = "202301010001",
                payment_reference_id = "{200806022341545",
                receive_amount = "10.48",
                payment_type = "Credit Card",
                payment_acquirer_bank = "KTC",
                transaction_date = "20230101",
                transaction_time = "190001",
                order_description = "TEST"
            }
        };
        public INETPaymentResponse Get()
        {
            return _default;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            _default.merchant_id = id.ToString();
            _default.detail.merchant_id = id.ToString();
            return JsonConvert.SerializeObject(_default);
        }

        // POST api/<controller>
        public void Post([FromBody] INETOrderPayment value)
        {
            value.Save();                
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] INETPaymentResponse value)
        {
            _default = value;
            _default.merchant_id = id.ToString();
            _default.detail.merchant_id = id.ToString();
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
            if (id.Equals(_default.merchant_id))
            {
                _default = new INETPaymentResponse();
            }
        }
    }
}