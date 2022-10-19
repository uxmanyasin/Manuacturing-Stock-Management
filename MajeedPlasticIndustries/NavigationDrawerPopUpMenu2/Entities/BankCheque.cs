using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BizBook.Entities
{
    public class BankCheque
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public float Amount { get; set; }

        public string CashDate { get; set; }

        public string Description { get; set; }

        public string ChequeNumber { get; set; }
        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customers customers { get; set; }
        public int Status { get; set; }
    }
}
