using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BizBook.Entities
{
     public class Purchase_Details
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

        public string PurchaseId { get; set; }
        [ForeignKey("PurchaseId")]
        public virtual Purchase purchase { get; set; }

        public int RawId { get; set; }
        [ForeignKey("RawId")]
        public virtual RawMaterial rawmaterial  { get; set; }
    }
}
