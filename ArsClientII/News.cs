using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArsClientII
{
    public class News

    {
        public int NewsID { get; set; }
        public string? MyNews { get; set; }
        public DateTime CurrentDate { get; set; } = DateTime.Now;
        
    }
}
