using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizBook.Entities;
using BizBook.Models;
using System.Data.Entity;
namespace BizBook.Models
{
     public class BankChequeDatabaseHelper
    {

        private readonly DatabaseContext database = new DatabaseContext();

        public List<Entities.BankCheque> GetCustomerCheques(int id)
        {
            var listOfProducts = database.BankChequeDbSet.Where(s => s.CustomerID == id).ToList();
            return listOfProducts;
        }

        public List<Entities.BankCheque> GetDatedCheques(string date)
        {
            var listOfProducts = database.BankChequeDbSet.Where(s => s.CashDate == date).ToList();
            return listOfProducts;
        }
        public List<Entities.BankCheque> GetPendingCheques()
        {
            var listOfProducts = database.BankChequeDbSet.Where(s => s.Status == 0).ToList();
            return listOfProducts;
        }

        public List<Entities.BankCheque> GetComlpetedCheques()
        {
            var listOfProducts = database.BankChequeDbSet.Where(s => s.Status == 1).ToList();
            return listOfProducts;
        }

        public List<Entities.BankCheque> GetCheques()
        {
            var listOfProducts = database.BankChequeDbSet.ToList();
            return listOfProducts;
        }
        public Entities.BankCheque GetCheque(int id)
        {
            Entities.BankCheque Cheque = database.BankChequeDbSet.Where(s => s.Id == id).FirstOrDefault();
            return Cheque;
        }
        public Entities.BankCheque CreateCheque(Entities.BankCheque Cheque)
        {
            database.BankChequeDbSet.Add(Cheque);
            database.SaveChanges();
            return Cheque;
        }

        public Entities.BankCheque DeleteCheque(Entities.BankCheque Cheque)
        {
            database.BankChequeDbSet.Remove(Cheque);
            database.SaveChanges();
            return Cheque;
        }
        public Entities.BankCheque CashInCheque(Entities.BankCheque Cheque)
        {
            Entities.BankCheque model = GetCheque(Cheque.Id);
            Entities.Payments payments = new Payments
            {
                CustomerID = Cheque.CustomerID,
                Credit = Cheque.Amount,
                Date = Convert.ToDateTime(Cheque.CashDate),
                EntryDate = DateTime.Now.Date,
                Type = "Customer",
                Description = Cheque.Description,
                Debit = 0
            };
            model.Status = 1;
            database.Entry(model).State = EntityState.Modified;
            database.PaymentsDbSet.Add(payments);
            database.SaveChanges();
            return model;
        }

        public Entities.BankCheque EditCheque(Entities.BankCheque Cheque)
        {
            Entities.BankCheque model = GetCheque(Cheque.Id);
            model.CustomerID = Cheque.CustomerID;
            model.Amount = Cheque.Amount;
            model.CashDate = Cheque.CashDate;
            model.Description = Cheque.Description;
            model.ChequeNumber = Cheque.ChequeNumber;
            model.Status = 0;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

    }
}
