using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ThaiPaymentAPI.Models;
namespace ThaiPaymentAPI.Controllers
{
    public class LogController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<ActionLog> Get()
        {
            return new ActionLog().Gets();
        }

        // GET api/<controller>/5
        public ActionLog Get(int id)
        {
            return new ActionLog().Gets().Where(e=>e.log_id.Equals(id)).FirstOrDefault();
        }

        // POST api/<controller>
        public ErrorResponse Post([FromBody] ActionLog value)
        {
            return value.Save();
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