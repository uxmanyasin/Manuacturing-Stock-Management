using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BizBook.Entities
{
    public class Purchase
    {
        [Key]
        public string PId { get; set; }

        [Required]
        public int TotalProducts { get; set; }

        [Required]
        public int TotalQuantity { get; set; }

        [Required]
        public float TotalPrice { get; set; }

        [Required]
        public float Cash { get; set; }

        [Required]
        public float Freight { get; set; }

        [Required]
        public string GatePass { get; set; }

        [Required]
        public float PurchaseAmount { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime PurchaseDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime EntryDate { get; set; }

        [Required]
        public bool Status { get; set; }

        public int SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Suppliers suppliers { get; set; }
    }
}
