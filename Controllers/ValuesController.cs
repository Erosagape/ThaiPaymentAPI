using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ThaiPaymentAPI.Controllers
{
    public class ValuesController : ApiController
    {
        public static string[] arr={ "value1", "value2" };
        // GET api/values
        public IEnumerable<string> Get()
        {
            return arr;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value=" + arr[id-1];
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
            arr = value.Split(',');
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
            arr[id] = value;
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
            arr = arr.Where(e => e != arr[id]).ToArray();
        }
    }
}
