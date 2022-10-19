using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Models
{
    public class CompanyLedgerViewModel
    {
        public string Date { get; set; }
        public string Customer { get; set; }
        public string Supplier { get; set; }
        public string Description { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
    }
}
