using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankaBackend.Models.Entities
{
    public class HavaleEntities
    {
        public string GonderenId { get; set; }
        public string AliciId { get; set; }
        public string AliciAdSoyad { get; set; }
        public decimal Tutar { get; set; }
        public string GonderenHesap { get; set; }
        public string AliciHesap { get; set; }
        public string Aciklama { get; set; }
    }
}
