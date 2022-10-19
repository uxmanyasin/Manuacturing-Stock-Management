using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Models
{
    public class SupplierLedgerModel
    {
        public int ID { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public float Quantity { get; set; }
        public float Rate { get; set; }
        public float Debit { get; set; }
        public float Credit { get; set; }
        public string Balance { get; set; }
        public string BillNo { get; set; }
    }
}
