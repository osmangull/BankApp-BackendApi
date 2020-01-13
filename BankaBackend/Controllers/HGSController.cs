using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankaBackend.Models;
using BankaBackend.Models.Entities;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankaBackend.Controllers
{
    [EnableCors("MyPolicy")]
    [Route("api/[controller]")]
    public class HGSController : ControllerBase
    {
        private M4ABankContext db = new M4ABankContext();
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("hgsode")]
        public async Task<IActionResult> hgsode([FromBody] CustomerEntity customerEntity)
        {
            try
            {
                var hareketler = new Transactions()
                {
                    Amount = customerEntity.HgsBakiye,
                    SenderId = customerEntity.CustomerTckn,
                    ReceiverId = "-",
                    Explanation = "HGS ödemesi yapıldı.",
                    AType = "HGS",
                    SaveDate = DateTime.Now,

                };
                db.Transactions.Add(hareketler);
                db.SaveChanges();
                return Ok("başarılı");
               
            }
            catch (Exception)
            {
                return Ok("hata");
            }
            
        }
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("hgsvarmi")]
        public async Task<IActionResult> hgsvarmi([FromBody] CustomerEntity customerEntity)
        {
            try
            {
                var hgs = db.Hgs.FirstOrDefault(s => s.CustomerId == customerEntity.CustomerTckn);
                if(hgs!=null)
                {
                    return Ok("var");
                }
                else
                return Ok("yok");
            }
            catch (Exception)
            {
                return Ok("hata");
            }

        }
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("hgsbakiye")]
        public async Task<IActionResult> hgsbakiye([FromBody] CustomerEntity customerEntity)
        {
            try
            {
                var hgs = db.Hgs.FirstOrDefault(s => s.CustomerId == customerEntity.CustomerTckn);
                if (hgs != null)
                {
                    return Ok(hgs);
                }
                else
                    return Ok("yok");
            }
            catch (Exception)
            {
                return Ok("hata");
            }

        }
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("hgsac")]
        public async Task<IActionResult> hgsac([FromBody] CustomerEntity customerEntity)
        {
            try
            {
                var hgs = new Hgs()
                {
                    CustomerId = customerEntity.CustomerTckn,
                    HgsBalance = 0,
                    SaveUser = customerEntity.CustomerTckn,
                    SaveDate = DateTime.Now,
                    EditDate = DateTime.Now,
                    EditUser = -1
                };
                db.Hgs.Add(hgs);
                db.SaveChanges();
                return Ok("var");
            }
            catch (Exception ex)
            {
                return Ok("hata");
            }

        }

    }
}