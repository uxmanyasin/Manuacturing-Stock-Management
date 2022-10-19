using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Models.DummyModels
{
    public class CustomerRunningBalanceViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Contact { get; set; }

        public string Address { get; set; }

        public string Date { get; set; }

        public string RunningBalance { get; set; }

        public string CreditBalance { get; set; }

        public string DebitBalance { get; set; }


        public string Sales { get; set; }

        public string Credit { get; set; }

        public int CreditStatus { get; set; }

        public string CreditLimit { get; set; }
    }
}
