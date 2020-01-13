using System;
using System.Collections.Generic;

namespace BankaBackend.Models
{
    public partial class Credit
    {
        public int CreditId { get; set; }
        public int? AccountId { get; set; }
        public decimal? CreditBalance { get; set; }
        public int EditUser { get; set; }
        public DateTime SaveDate { get; set; }
        public int SaveUser { get; set; }
        public DateTime EditDate { get; set; }
    }
}
