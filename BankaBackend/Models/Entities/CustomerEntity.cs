using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankaBackend.Models.Entities
{
    public class CustomerEntity
    {
        public int CustomerId { get; set; }
        public string CustomerTckn { get; set; }
        public string NameSurname { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string PhoneNumber { get; set; }
        public string AType { get; set; }
        public DateTime SaveDate { get; set; }
        public int SaveUser { get; set; }
        public DateTime EditDate { get; set; }
        public int EditUser { get; set; }
        public decimal Balance { get; set; }
        public string AddAccName { get; set; }
        public string AddAccNumber { get; set; }
        public int AccountId { get; set; }
        public decimal HgsBakiye { get; set; }
    }
}
