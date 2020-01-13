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
    public class MoneyTransferController : ControllerBase
    {
        private M4ABankContext db = new M4ABankContext();
        [Consumes("application/json")]
        [Produces("application/json")]
        [HttpPost("Virman")]
        public async Task<IActionResult> Virman([FromBody] MoneyTransfer moneyTransfer)
        {
            try
            {
               if (moneyTransfer.Money <= 0)
                {
                    return Ok("eksipara");
                }
                else if( moneyTransfer.AvaibleAccNumber=="0"||moneyTransfer.SentAccNumber=="0")
                {
                    return Ok("hata");
                }
                else
                {
                    var accountsgonderen = db.Account.Where(s => s.CustomerId == moneyTransfer.CustomerId && s.AccountId == int.Parse(moneyTransfer.AvaibleAccNumber)).ToList();
                    var accountsalici = db.Account.Where(s => s.CustomerId == moneyTransfer.CustomerId && s.AccountId == int.Parse(moneyTransfer.SentAccNumber)).ToList();
                    if (accountsgonderen != null && accountsalici != null)
                    {

                        foreach (var accountsAlici in accountsgonderen)
                        {
                            foreach (var accountsAlici1 in accountsalici)
                            {
                                if (accountsAlici.AccountId == accountsAlici1.AccountId)
                                {
                                    return Ok("Aynı Hesap");
                                }

                                else
                                {
                                    if (accountsAlici.Balance >= moneyTransfer.Money)
                                    {
                                        if (accountsAlici.AType == "Dolar" && (accountsAlici1.AType == "TL" || accountsAlici1.AType == "Vadeli" || accountsAlici1.AType == "Vadesiz" || accountsAlici1.AType == "Birikim"))
                                        {
                                            accountsAlici.Balance = accountsAlici.Balance - moneyTransfer.Money;
                                            accountsAlici1.Balance = accountsAlici1.Balance + (moneyTransfer.Money * 5);
                                            db.SaveChanges();
                                        }
                                        else if (accountsAlici.AType == "Euro" && (accountsAlici1.AType == "TL" || accountsAlici1.AType == "Vadeli" || accountsAlici1.AType == "Vadesiz" || accountsAlici1.AType == "Birikim"))
                                        {
                                            accountsAlici.Balance = accountsAlici.Balance - moneyTransfer.Money;
                                            accountsAlici1.Balance = accountsAlici1.Balance + (moneyTransfer.Money * 6);
                                            db.SaveChanges();
                                        }
                                        else if (accountsAlici.AType == "Dolar" && accountsAlici1.AType == "Euro")
                                        {
                                            accountsAlici.Balance = accountsAlici.Balance - moneyTransfer.Money;
                                            accountsAlici1.Balance = accountsAlici1.Balance + (moneyTransfer.Money * (5 / 6));
                                            db.SaveChanges();
                                        }
                                        else if (accountsAlici.AType == "Euro" && accountsAlici1.AType == "Dolar")
                                        {
                                            accountsAlici.Balance = accountsAlici.Balance - moneyTransfer.Money;
                                            accountsAlici1.Balance = accountsAlici1.Balance + (moneyTransfer.Money * (6 / 5));
                                            db.SaveChanges();
                                        }
                                        else if (accountsAlici1.AType == "Euro" && (accountsAlici.AType == "TL" || accountsAlici.AType == "Vadeli" || accountsAlici.AType == "Vadesiz" || accountsAlici.AType == "Birikim"))
                                        {
                                            accountsAlici.Balance = accountsAlici.Balance - (moneyTransfer.Money);
                                            accountsAlici1.Balance = accountsAlici1.Balance + (moneyTransfer.Money / 6);
                                            db.SaveChanges();
                                        }
                                        else if (accountsAlici1.AType == "Dolar" && (accountsAlici.AType == "TL" || accountsAlici.AType == "Vadeli" || accountsAlici.AType == "Vadesiz" || accountsAlici.AType == "Birikim"))
                                        {
                                            accountsAlici.Balance = accountsAlici.Balance - (moneyTransfer.Money);
                                            accountsAlici1.Balance = accountsAlici1.Balance + (moneyTransfer.Money / 5);
                                            db.SaveChanges();
                                        }
                                        else
                                        {
                                            accountsAlici.Balance = accountsAlici.Balance - (moneyTransfer.Money);
                                            accountsAlici1.Balance = accountsAlici1.Balance + (moneyTransfer.Money);
                                            db.SaveChanges();
                                        }
                                    }
                                    else
                                    {
                                        return Ok("eksipara");
                                    }
                                }
                            }


                        }
                        var hareketler = new Transactions()
                        {
                            Amount = moneyTransfer.Money,
                            SenderId = moneyTransfer.CustomerId,
                            ReceiverId = moneyTransfer.CustomerId +"-"+ moneyTransfer.SentAccNumber,
                            Explanation = moneyTransfer.CustomerId+"-" +moneyTransfer.AvaibleAccNumber + "hesabınızdan virman işlemi gerçekleştirildi.",
                            AType = "Virman",
                            SaveDate = DateTime.Now,

                        };
                        db.Transactions.Add(hareketler);
                        db.SaveChanges();
                        return Ok("Başarılı");
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
        [HttpPost("Havale")]
        public async Task<IActionResult> Havale([FromBody] HavaleEntities havaleEntities)
        {
            try
            {
                if (havaleEntities.GonderenHesap == "0")
                {
                    return Ok("hata");
                }
                else
                {
                    var accountsGonderen = db.Account.FirstOrDefault(s => s.CustomerId == havaleEntities.GonderenId && s.AddAccNumber == havaleEntities.GonderenHesap);
                    var accountsAlici = db.Account.FirstOrDefault(s => s.CustomerId == havaleEntities.AliciId && s.Status == 1 && (s.AType == "TL" || s.AType == "Vadeli" || s.AType == "Vadesiz" || s.AType == "Birikim"));
                    if (accountsGonderen.Balance >= havaleEntities.Tutar && havaleEntities.Tutar <= 0)
                    {
                        return Ok("yetersiz");
                    }
                    else if(accountsAlici ==null)
                    {
                        return Ok("musterinohatasi");
                    }
                    else
                    {
                        if (accountsGonderen != null && accountsAlici != null)
                        {

                            if (accountsGonderen.AType == "Dolar")
                            {
                                accountsGonderen.Balance = accountsGonderen.Balance - havaleEntities.Tutar;
                                accountsAlici.Balance = accountsAlici.Balance + (havaleEntities.Tutar * 5);
                                
                                db.SaveChanges();
                                

                            }
                            else if (accountsGonderen.AType == "Euro")
                            {
                                accountsGonderen.Balance = accountsGonderen.Balance - havaleEntities.Tutar;
                                accountsAlici.Balance = accountsAlici.Balance + (havaleEntities.Tutar * 6);
                                db.SaveChanges();

                            }
                            else
                            {
                                if (accountsAlici.AType == "Dolar")
                                {
                                    accountsGonderen.Balance = accountsGonderen.Balance - havaleEntities.Tutar;
                                    accountsAlici.Balance = accountsAlici.Balance + (havaleEntities.Tutar / 5);
                                    db.SaveChanges();
                                    
                                }
                                else if (accountsAlici.AType == "Euro")
                                {
                                    accountsGonderen.Balance = accountsGonderen.Balance - havaleEntities.Tutar;
                                    accountsAlici.Balance = accountsAlici.Balance + (havaleEntities.Tutar / 6);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    accountsGonderen.Balance = accountsGonderen.Balance - havaleEntities.Tutar;
                                    accountsAlici.Balance = accountsAlici.Balance + (havaleEntities.Tutar);
                                    db.SaveChanges();
                                }

                            }
                            var hareketler = new Transactions()
                            {
                                Amount = havaleEntities.Tutar,
                                SenderId = havaleEntities.GonderenId,
                                ReceiverId = havaleEntities.AliciId,
                                Explanation = havaleEntities.Aciklama,
                                AType = "Giden Havale",
                                SaveDate = DateTime.Now,

                            };
                            db.Transactions.Add(hareketler);
                            var alicihareketler = new Transactions()
                            {
                                Amount = havaleEntities.Tutar,
                                SenderId = havaleEntities.AliciId,
                                ReceiverId = havaleEntities.GonderenId,
                                Explanation = havaleEntities.Aciklama,
                                AType = "Gelen Havale",
                                SaveDate = DateTime.Now,

                            };
                            db.Transactions.Add(alicihareketler);
                            db.SaveChanges();
                            return Ok("Başarılı");
                        }
                    }
                }
                return Ok("hata");
            }
            catch (Exception ex)
            {
                return Ok("boş");
            }
        }
    }
}