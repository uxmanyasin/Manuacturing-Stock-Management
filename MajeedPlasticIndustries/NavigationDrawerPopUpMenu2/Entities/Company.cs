using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Entities
{
    public class Company
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        [MaxLength(150)]
        [Required]
        public string Name { get; set; }

        [Required]
        [MaxLength(200)]
        public string OwnerName { get; set; }

       [Required]
        public string Telephone { get; set; }
        [Required]
        public string Address { get; set; }

        public bool Status { get; set; }
    }
}
