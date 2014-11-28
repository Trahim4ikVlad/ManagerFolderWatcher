using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLayer
{
    public class FileContents
    {
        public string ManagerName { get; set; }
        public IList<SaleInfo> SalesInfo { get; set; } 
    }
}
