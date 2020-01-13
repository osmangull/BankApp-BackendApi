using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankaBackend.Models;
using Microsoft.AspNetCore.Mvc;

namespace BankaBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private M4ABankContext db = new M4ABankContext();

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        [HttpGet("alluser")]
        public ActionResult<IEnumerable<string>> alluser()
        {
            try
            {
                var accounts = db.Customer.ToList();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
