using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizBook.Models
{
    public class SupplierRateDatabaseHelper
    {
        private readonly DatabaseContext database = new DatabaseContext();
        public Entities.SupplierRate CreateSupplierRate(Entities.SupplierRate customerRate)
        {
            database.SupplierRateDbSet.Add(customerRate);
            database.SaveChanges();
            return customerRate;
        }


        public Entities.SupplierRate GetSupplierRate(int ProductID, int? custID)
        {
            Entities.SupplierRate customerRate = database.SupplierRateDbSet.Where(s => s.RawId == ProductID && s.SupplierId == custID).FirstOrDefault();
            return customerRate;
        }

        public Entities.SupplierRate EditSupplierRate(Entities.SupplierRate customerRate)
        {
            Entities.SupplierRate model = GetSupplierRate(customerRate.RawId, customerRate.SupplierId);


            model.Rate = customerRate.Rate;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }
    }
}
