using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Reporting
{
    public class SupplierLedgerModel
    {
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public string Quantity { get; set; }
        public string Rate { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public string Balance { get; set; }
        public string BillNo { get; set; }
    }
}
