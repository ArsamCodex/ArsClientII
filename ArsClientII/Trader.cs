using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsClientII
{
    public class Trader
    {
        public int TraderId { get; set; }
        public string? Pair { get; set; }
        public double? BuyPrice { get; set; }
        public double? SelPrice { get; set; }
        public DateTime? BougtDate { get; set; }
        public DateTime? SoldDate { get; set; }
        public bool IsTradeCompleted { get; set; }
        public double? WinPosition { get; set; }
        public double? LossPosition { get; set; }

    }
    
}
