using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Models
{
    public class CustomerRateDatabaseHelper
    {
        private readonly DatabaseContext database = new DatabaseContext();
        #region CustomerRate
        public Entities.CustomerRate CreateCustomerRate(Entities.CustomerRate customerRate)
        {
            database.CustomerRateDbSet.Add(customerRate);
            database.SaveChanges();
            return customerRate;
        }


        public Entities.CustomerRate GetcustomerRate(int ProductID, int? custID)
        {
            Entities.CustomerRate customerRate = database.CustomerRateDbSet.Where(s => s.ProductId == ProductID && s.CustomerId == custID).FirstOrDefault();
            return customerRate;
        }

        public Entities.CustomerRate EditCustomerRate(Entities.CustomerRate customerRate)
        {
            Entities.CustomerRate model = GetcustomerRate(customerRate.ProductId, customerRate.CustomerId);
         
           
            model.Rate = customerRate.Rate;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }
        #endregion
    }
}
