using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BizBook.Entities
{
    public class SupplierClaimDetails
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public float Quantity { get; set; }

        [Required]
        public float Total { get; set; }
        [Required]
        public float Rate { get; set; }

        public string SupplierClaimId { get; set; }
        [ForeignKey("SupplierClaimId")]
        public virtual SupplierClaims SupplierClaims { get; set; }

        public int RawId { get; set; }
        [ForeignKey("RawId")]
        public virtual Entities.RawMaterial RawMaterial { get; set; }
    }
}
