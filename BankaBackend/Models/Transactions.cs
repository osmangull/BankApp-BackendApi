using System;
using System.Collections.Generic;

namespace BankaBackend.Models
{
    public partial class Transactions
    {
        public int Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public decimal? Amount { get; set; }
        public string Explanation { get; set; }
        public DateTime? SaveDate { get; set; }
        public int? SaveUser { get; set; }
        public DateTime? EditDate { get; set; }
        public int? EditUser { get; set; }
        public string AType { get; set; }
    }
}
