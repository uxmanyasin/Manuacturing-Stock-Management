using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Entities
{
    public class Payments
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

       
        public string CustomerName { get; set; }

        public int? CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customers customers { get; set; }

        public int? SupplierID { get; set; }
        [ForeignKey("SupplierID")]
        public virtual Suppliers Suppliers  { get; set; }
        public float? Credit { get; set; }

        public float? Debit { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime Date { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime EntryDate { get; set; }

        [Required]
        public string Type { get; set; }

        public string Description { get; set; }

        public string PurchaseId { get; set; }
        [ForeignKey("PurchaseId")]
        public virtual Purchase Purchase { get; set; }

        public string SupplierClaimsID { get; set; }
        [ForeignKey("SupplierClaimsID")]
        public virtual Entities.SupplierClaims SupplierClaims { get; set; }

        public string CustomerClaimsID { get; set; }
        [ForeignKey("CustomerClaimsID")]
        public virtual Entities.CustomerClaims CustomerClaims { get; set; }

        public string SaleId { get; set; }
        [ForeignKey("SaleId")]
        public virtual Sale Sale { get; set; }
    }
}
