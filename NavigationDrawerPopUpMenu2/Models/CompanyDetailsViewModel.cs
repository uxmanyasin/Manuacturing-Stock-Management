using BizBook.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Models
{
    public class CompanyDetailsViewModel
    {
        public Company Company { get; set; }
        public List<Users> Users { get; set; }
    }
}
