using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BizBook.Entities
{
    public class SupplierClaims
    {
        [Key]
        public string CId { get; set; }

        [Required]
        public float TotalPrice { get; set; }

        [Required]
        public float Cash { get; set; }

        [Required]
        public string GatePass { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime ClaimDate { get; set; }

       
        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Entities.Suppliers Suppliers { get; set; }
    }
}
