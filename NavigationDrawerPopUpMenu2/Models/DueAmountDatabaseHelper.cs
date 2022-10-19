using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BizBook.Entities;

namespace BizBook.Models
{
    public class DueAmountDatabaseHelper
    {
        private readonly DatabaseContext database = new DatabaseContext();
     

        public List<DueAmount> GetDues()
        {
            List<DueAmount> u = database.DueAmountDbSet.Where(s => s.Status == true).ToList();
            return u;
        }

        public List<DueAmount> GetCustomerDues(int Id)
        {
            List<DueAmount> duesList = database.DueAmountDbSet.Where(s => s.CustomerId == Id && s.Status == true).ToList();
            return duesList;
        }
        public DueAmount GetSingleDue(int Id)
        {
            DueAmount dues = database.DueAmountDbSet.Where(s => s.Id == Id).FirstOrDefault();
            return dues;
        }

        public List<Entities.DueAmount> GetDatedDues(DateTime date)
        {
            var listOfSales = database.DueAmountDbSet.Where(s => s.DueDate == date && s.Status == true).ToList();
            return listOfSales;
        }

        public List<Entities.DueAmount> DuesBetweenDates(DateTime dateFrom, DateTime dateTo)
        {
            var model = database.DueAmountDbSet.Where(s => s.DueDate >= dateFrom && s.DueDate <= dateTo && s.Status == true).ToList();
            return model;
        }
        public Entities.DueAmount StatusChange(Entities.DueAmount product)
        {
            Entities.DueAmount model = GetSingleDue(product.Id);
            model.Status = false; 
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Entities.DueAmount PartialPayment(Entities.DueAmount dueamount)
        {
            Entities.DueAmount model = GetSingleDue(dueamount.Id);
            model.PendingAmount = dueamount.PendingAmount;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public DueAmount GetSaleDue(string Id)
        {
            DueAmount dues = database.DueAmountDbSet.Where(s => s.SaleId == Id).FirstOrDefault();
            return dues;
        }

        public Entities.DueAmount UpdateDueAmount(string _SaleID,int _amount)
        {
            Entities.DueAmount model = GetSaleDue(_SaleID);
            model.PendingAmount = _amount;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }
    }
}
