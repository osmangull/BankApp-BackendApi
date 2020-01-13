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
    public class CustomerController : ControllerBase
    {
        private M4ABankContext db = new M4ABankContext();
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("register")]
        public async Task<IActionResult> register([FromBody] CustomerEntity customerEntity)
        {
            try
            {
                var data = db.Customer.FirstOrDefault(s => s.CustomerTckn == customerEntity.CustomerTckn);
                if (data == null)
                {
                    var user = new Customer()
                    {
                        CustomerTckn = customerEntity.CustomerTckn,
                        NameSurname = customerEntity.NameSurname,
                        EditDate = DateTime.Now,
                        EditUser = -1,
                        Password = customerEntity.Password,
                        PhoneNumber = customerEntity.PhoneNumber,
                        SaveDate = DateTime.Now,
                        SaveUser = -1,
                        Balance = 0
                    };
                    var account = new Account()
                    {
                        AccountNumber = customerEntity.CustomerTckn,
                        AddAccName = "Vadesiz TL",
                        AddAccNumber = "1000",
                        AType = "TL",
                        Balance = 0,
                        CustomerId = customerEntity.CustomerTckn,
                        Status = 1,
                        EditDate = DateTime.Now,
                        EditUser = -1,
                        SaveDate = DateTime.Now,
                        SaveUser = -1

                    };
                    var hareketler = new Transactions()
                    {
                        Amount = 0,
                        SenderId = customerEntity.CustomerTckn,
                        ReceiverId = "-",
                        Explanation = "Yeni İlk Hesap Açılışı",
                        AType = "Yeni İlk Hesap",
                        SaveDate = DateTime.Now,

                    };
                    db.Transactions.Add(hareketler);
                    db.Customer.Add(user);
                    db.Account.Add(account);
                    db.SaveChanges();
                    return Ok("Kayıt Başarılı");
                }
                return Ok("ayni");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("setpassword")]
        public async Task<IActionResult> setpassword([FromBody] CustomerEntity customerEntity)
        {
            try
            {
                string tcNumber = customerEntity.CustomerTckn;
                string pass = customerEntity.OldPassword;
                var login = db.Customer.FirstOrDefault(s => s.CustomerTckn == tcNumber && s.Password == pass);
                if (login != null)
                {
                    login.Password = customerEntity.Password;
                    db.SaveChanges();
                    return Ok("başarılı");
                }
                else
                    return Ok("Hatalı");
            }
            catch (Exception EX)
            {
                return Ok("Sorgu Döndürülemedi");
            }

        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("setphone")]
        public async Task<IActionResult> setphone([FromBody] CustomerEntity customerEntity)
        {
            try
            {
                string tcNumber = customerEntity.CustomerTckn;
                string pass = customerEntity.Password;
                var login = db.Customer.FirstOrDefault(s => s.CustomerTckn == tcNumber && s.Password == pass);
                if (login != null)
                {
                    if (customerEntity.PhoneNumber.Length == 10)
                    {
                        login.PhoneNumber = customerEntity.PhoneNumber;
                        db.SaveChanges();
                        return Ok("başarılı");
                    }
                    return Ok("Hatalı");
                }
                else
                    return Ok("Hatalı");
            }
            catch (Exception EX)
            {
                return Ok("Sorgu Döndürülemedi");
            }

        }

        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] CustomerEntity customerEntity)
        {
            try
            {
                string tcNumber = customerEntity.CustomerTckn;
                string pass = customerEntity.Password;
                var login = db.Customer.FirstOrDefault(s => s.CustomerTckn == tcNumber && s.Password == pass);
                if (login != null)
                {
                    decimal dolar = 5;
                    decimal euro = 6;
                    decimal toplam = 0;
                    var accounts = db.Account.Where(s => s.CustomerId == customerEntity.CustomerTckn && s.Status == 1).ToList();
                    foreach (var item in accounts)
                    {
                        if (item.AType == "Dolar")
                        {
                            toplam = toplam + (item.Balance * dolar);
                            login.Balance = toplam;
                        }
                        else if (item.AType == "Euro")
                        {
                            toplam = toplam + (item.Balance * euro);
                            login.Balance= toplam;
                        }
                        else
                        {
                            toplam += item.Balance;
                            login.Balance = toplam;
                        }
                        db.SaveChanges();
                    }
                    return Ok(login);
                }
                else
                    return Ok("Hatalı");
            }
            catch (Exception EX)
            {
                return Ok("Sorgu Döndürülemedi");
            }
            
        }

        }
}