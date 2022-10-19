using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BizBook.Entities
{
    public class CustomerClaimDetails
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

        public string CustomerClaimId { get; set; }
        [ForeignKey("CustomerClaimId")]
        public virtual CustomerClaims CustomerClaims { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product product { get; set; }
    }
}
