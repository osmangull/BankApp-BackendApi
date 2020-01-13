using System;
using System.Collections.Generic;

namespace BankaBackend.Models
{
    public partial class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerTckn { get; set; }
        public string NameSurname { get; set; }
        public string Password { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime SaveDate { get; set; }
        public int SaveUser { get; set; }
        public DateTime EditDate { get; set; }
        public int EditUser { get; set; }
        public decimal? Balance { get; set; }
    }
}
