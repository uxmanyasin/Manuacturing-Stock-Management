using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Models
{
    public class RunningBalanceCustomersViewModel
    {
        public int Id { get; set; }
       
        public string Name { get; set; }

        public string Contact { get; set; }

        public string Address { get; set; }

        public string Date { get; set; }

        public float RunningBalance { get; set; }

        public float CreditBalance { get; set; }

        public float DebitBalance { get; set; }

        public float Sales { get; set; }

        public float Credit { get; set; }

        public int CreditStatus { get; set; }

        public float CreditLimit { get; set; }
    }
}
