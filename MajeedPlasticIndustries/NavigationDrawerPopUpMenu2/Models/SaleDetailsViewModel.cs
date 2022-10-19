using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Models
{
    public class SaleDetailsViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public float Price { get; set; }
        public float Quantity { get; set; }
        public float Total { get; set; }
    }
}
