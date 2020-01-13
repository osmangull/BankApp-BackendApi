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
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private M4ABankContext db = new M4ABankContext();
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("AllAccount")]
        public async Task<IActionResult> AllAccount([FromBody] CustomerEntity customerEntity)
        {
            try
            {
                var accounts = db.Account.Where(s => s.CustomerId == customerEntity.CustomerTckn && s.Status==1).ToList();
                return Ok(accounts);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpGet("alluser")]
        public ActionResult<IEnumerable<string>> alluser()
        {
            try
            {
                var accounts = db.Customer.ToList();
                return Ok(accounts);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("AllAccountType")]
        public async Task<IActionResult> AllAccountType([FromBody] CustomerEntity customerEntity)
        {
            try
            {
                var accounts = db.Account.ToList();
                return Ok(accounts);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("TransactionData")]
        public async Task<IActionResult> TransactionData([FromBody] CustomerEntity customerEntity)
        {
            try
            {
                var accounts = db.Transactions.Where(s => s.SenderId == customerEntity.CustomerTckn).ToList();
                return Ok(accounts);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("NewAccount")]
        public async Task<IActionResult> NewAccount([FromBody] CustomerEntity customerEntity)
        {
            try
            {
                if (customerEntity.AddAccName == null || customerEntity.AType == null  || customerEntity.AType=="0"|| customerEntity.Balance == null)
                {
                    return Ok("Boşdeğer");
                }
                else
                {
                    var accounts = db.Account.OrderByDescending(s => s.AccountId).FirstOrDefault(s => s.CustomerId == customerEntity.CustomerTckn);
                    int lastAccNumber = int.Parse(accounts.AddAccNumber);
                    lastAccNumber++;

                    var account = new Account()
                    {
                        AccountNumber = customerEntity.CustomerTckn,
                        AddAccName = customerEntity.AddAccName,
                        AddAccNumber = lastAccNumber.ToString(),
                        Balance = customerEntity.Balance,
                        AType = customerEntity.AType,
                        CustomerId = customerEntity.CustomerTckn,
                        Status = 1,
                        EditDate = DateTime.Now,
                        EditUser = -1,
                        SaveDate = DateTime.Now,
                        SaveUser = -1
                    };
                    var hareketler = new Transactions()
                    {
                        Amount = customerEntity.Balance,
                        SenderId = customerEntity.CustomerTckn,
                        ReceiverId = "-",
                        Explanation = "Yeni Hesap Açılışı",
                        AType = "Yeni Hesap",
                        SaveDate = DateTime.Now,

                    };
                    db.Transactions.Add(hareketler);
                    db.Account.Add(account);
                    db.SaveChanges();
                    return Ok("başarılı");
                }
            }
            catch (Exception ex)
            {
                return Ok("Boşdeğer");
            }
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("AddMoney")]
        public async Task<IActionResult> AddMoney([FromBody] CustomerEntity customerEntity)
        {
            try
            {
                var accounts = db.Account.FirstOrDefault(s => s.CustomerId == customerEntity.CustomerTckn && s.AddAccNumber == customerEntity.AddAccNumber);
                if(accounts!=null)
                {
                    if (customerEntity.Balance < 0)
                    {
                        return Ok("eksipara");
                    }
                    else
                    {
                        accounts.Balance = accounts.Balance + customerEntity.Balance;
                        var hareketler = new Transactions()
                        {
                            Amount = customerEntity.Balance,
                            SenderId = customerEntity.CustomerTckn,
                            ReceiverId = customerEntity.CustomerTckn + " - " + customerEntity.AddAccNumber ,
                            Explanation = "Para Eklendi.",
                            AType = "Para Ekleme",
                            SaveDate = DateTime.Now,

                        };
                        db.Transactions.Add(hareketler);
                        db.SaveChanges();
                        return Ok("başarılı");
                    }
                }
                return Ok("hata");
            }
            catch (Exception)
            {
                return Ok("boş");
            }
        }
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("RemoveMoney")]
        public async Task<IActionResult> RemoveMoney([FromBody] CustomerEntity customerEntity)
        {
            try
            {
                var accounts = db.Account.FirstOrDefault(s => s.CustomerId == customerEntity.CustomerTckn && s.AddAccNumber == customerEntity.AddAccNumber);
                if (accounts != null)
                {
                    if (accounts.Balance >= customerEntity.Balance && customerEntity.Balance>0)
                    {
                        accounts.Balance = accounts.Balance - customerEntity.Balance;
                        var hareketler = new Transactions()
                        {
                            Amount = customerEntity.Balance,
                            SenderId = customerEntity.CustomerTckn,
                            ReceiverId = "-",
                            Explanation = "Para çekildi",
                            AType = "Para Çekme",
                            SaveDate = DateTime.Now,

                        };
                        db.Transactions.Add(hareketler);
                        db.SaveChanges();
                        return Ok("başarılı");
                    }
                    else
                    {
                        return Ok("yetersiz");
                    }
                }
                return Ok("hata");
            }
            catch (Exception)
            {
                return Ok("boş");
            }
        }
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("RemoveAccount")]
        public async Task<IActionResult> RemoveAccount([FromBody] CustomerEntity customerEntity)
        {
            try
            {
                
                var accounts = db.Account.FirstOrDefault(s => s.AccountId == customerEntity.AccountId);
                if (accounts != null)
                {
                    if (accounts.Balance == 0)
                    {
                        accounts.Status = 0;
                        var hareketler = new Transactions()
                        {
                            Amount = 0,
                            SenderId = customerEntity.CustomerTckn,
                            ReceiverId = "-",
                            Explanation = "Hesap Silindi",
                            AType = "Hesap Silme",
                            SaveDate = DateTime.Now,

                        };
                        db.Transactions.Add(hareketler);
                        db.SaveChanges();
                        return Ok("başarılı");
                    }
                    else
                        return Ok("paravar");
                }
                return Ok("hata");
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}