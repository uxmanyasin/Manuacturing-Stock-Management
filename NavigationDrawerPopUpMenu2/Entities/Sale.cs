using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BizBook.Entities
{
     public class Sale
    {
        [Key]
      //  [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string SId { get; set; }

        [Required]
        public float TotalPrice { get; set; }

        
        public float? Cash { get; set; }

        public int TotalProducts { get; set; }

        public int TotalQuantity { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime SaleDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime EntryDate { get; set; }

        [Required]
        public bool Status { get; set; }

        public string CustomerName { get; set; }
        [Required]
        public string GatePass { get; set; }
        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customers customers { get; set; }
    }
}
