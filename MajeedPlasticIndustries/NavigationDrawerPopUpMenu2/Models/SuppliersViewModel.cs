using BizBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Models
{
    public class SuppliersViewModel
    {
        public Payments Payments { get; set; }
        public BizBook.Entities.Purchase Purchase { get; set; }
    }
}
