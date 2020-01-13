using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankaBackend.Models.Entities
{
    public class AccountEntity
    {
        public int AccountId { get; set; }
        public string CustomerId { get; set; }
        public string AccountNumber { get; set; }
        public int Status { get; set; }
        public string AddAccNumber { get; set; }
        public string AddAccName { get; set; }
        public decimal Balance { get; set; }
        public DateTime SaveDate { get; set; }
        public int SaveUser { get; set; }
        public DateTime EditDate { get; set; }
        public int EditUser { get; set; }
    }
}
