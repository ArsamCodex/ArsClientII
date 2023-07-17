using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsClientII
{
    public class CoinAnalysis
    {
        public int CoinAnalysisID { get; set; }
        public string? CoinName { get; set; }

        public double? Price { get; set; }
        public DateTime? Date { get; set; }

        public double? MovingAverage100 { get; set; }
        public double? MovingAverage21 { get; set; }
        public double? MovingAverage200 { get; set; }
    }
}
