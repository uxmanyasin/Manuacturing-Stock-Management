using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Models.DummyModels
{
    public class PaymentsListModel
    {
       
        public int Id { get; set; }
        public float Debit { get; set; }
        public float  Credit { get; set; }
        public string Desc { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
    }
}
