using System;
using System.Collections.Generic;

namespace BankaBackend.Models
{
    public partial class Hgs
    {
        public int HgsId { get; set; }
        public string CustomerId { get; set; }
        public decimal HgsBalance { get; set; }
        public DateTime SaveDate { get; set; }
        public string SaveUser { get; set; }
        public DateTime EditDate { get; set; }
        public int EditUser { get; set; }
    }
}
