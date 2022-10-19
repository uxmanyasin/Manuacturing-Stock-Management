using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Reporting
{
   public class SalePrintDetailsModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Price { get; set; }
        public float Quantity { get; set; }
        public string Total { get; set; }
    }
}
