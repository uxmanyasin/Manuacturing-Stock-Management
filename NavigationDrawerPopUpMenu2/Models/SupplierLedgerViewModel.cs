using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BizBook.Models
{
    public class SupplierLedgerViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public float Credit { get; set; }
        public float Debit { get; set; }
        public float Balance { get; set; }
        public string Ref { get; set; }

        public string Products { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

    }
}
