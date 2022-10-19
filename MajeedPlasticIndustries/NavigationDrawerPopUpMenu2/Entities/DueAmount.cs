using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BizBook.Entities
{
    public class DueAmount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int PendingAmount { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime DueDate { get; set; }


        public bool Status { get; set; }

        public string SaleId { get; set; }
        [ForeignKey("SaleId")]
        public virtual Entities.Sale sales { get; set; }


        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Entities.Customers Customers { get; set; }
    }
}
