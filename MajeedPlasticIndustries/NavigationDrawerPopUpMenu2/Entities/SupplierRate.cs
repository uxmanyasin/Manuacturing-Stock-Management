using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BizBook.Entities
{
    public class SupplierRate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public float Rate { get; set; }

        public int? SupplierId { get; set; }
        [ForeignKey("SupplierId")]
        public virtual Suppliers suppliers { get; set; }

        public int RawId { get; set; }
        [ForeignKey("RawId")]
        public virtual RawMaterial rawmaterial { get; set; }
    }
}
