using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankaBackend.Models.Entities
{
    public class MoneyTransfer
    {
        public string CustomerId { get; set; }
        public string AvaibleAccNumber { get; set; }
        public decimal Money { get; set; }
        public string SentAccNumber { get; set; }
    }
}
