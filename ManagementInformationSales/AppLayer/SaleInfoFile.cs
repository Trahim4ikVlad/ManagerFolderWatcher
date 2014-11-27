using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer
{
    public class SaleInfoFile
    {
        public string ManagerName { get; set; }
        public DateTime DateSale { get; set; }
        public string ClientName { get; set; }
        public string ProductName { get; set; }
        public double CostSale { get; set; }
    }
}
