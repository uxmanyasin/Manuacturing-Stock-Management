using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Entities
{
    public class Customers
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    
        public string Contact { get; set; }
     
        public string Address { get; set; }
     
        public string Date { get; set; }

        public float DebitBalance { get; set; }

        public float CreditBalance { get; set; }

        public int CreditStatus { get; set; }

        public float CreditLimit { get; set; }


    }
}
