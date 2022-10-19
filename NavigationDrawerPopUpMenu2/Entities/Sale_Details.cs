using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BizBook.Entities
{
    public class Sale_Details
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product product { get; set; }

        [Required]
        public float Quantity { get; set; }

        [Required]
        public float Rate { get; set; }

        [Required]
        public float Total { get; set; }

        public string SaleId { get; set; }
        [ForeignKey("SaleId")]
        public virtual Sale sale { get; set; }
        [Required]
        [Column(TypeName = "date")]
        public DateTime SaleDate { get; set; }
    }
}
