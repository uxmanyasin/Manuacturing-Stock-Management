using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Models.DummyModels
{
    public class CustomerSupplierPaymentModel
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string Customer { get; set; }

        public string Supplier { get; set; }

        public int? CustomerID { get; set; }
       
        public int? SupplierID { get; set; }
      
        public string Credit { get; set; }

        public string Debit { get; set; }

       
        public DateTime Date { get; set; }

     
        public DateTime EntryDate { get; set; }

       
        public string Type { get; set; }

        public string Description { get; set; }

        public string PurchaseId { get; set; }
       
        public string SupplierClaimsID { get; set; }
      
        public string CustomerClaimsID { get; set; }
     
        public string SaleId { get; set; }
       
    }
}
