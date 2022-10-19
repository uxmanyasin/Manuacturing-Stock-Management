using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizBook.Entities;
using BizBook.Models;
namespace BizBook.Models
{
    public class PaymentsDatabaseHelper
    {
        private readonly DatabaseContext database = new DatabaseContext();



        public List<Payments> GetCompanyLedger()
        {
            var listOfPayments = database.PaymentsDbSet.ToList();
            return listOfPayments;
        }

        public List<Payments> GetRangeCompanyLedger(DateTime dateFrom, DateTime dateTo)
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.Date >= dateFrom && s.Date <= dateTo).ToList();
            return listOfPayments;
        }
       
        public List<Payments> GetPayments(string PType)
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.Type == PType && s.SaleId == null && s.CustomerClaimsID == null ).ToList();
            return listOfPayments;
        }

        public List<Payments> GetSupplierPayments(string PType)
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.Type == PType && s.PurchaseId == null && s.Description != "Claim Cash" && s.Description != "Requested Claim").ToList();
            return listOfPayments;
        }

        public List<Payments> GetSupplierSinglePayment(int id)
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.Id == id && s.PurchaseId == null && s.Description != "Claim Cash" && s.Description != "Requested Claim").ToList();
            return listOfPayments;
        }

        public List<Payments> GetDateSupplierPayments(DateTime date,string PType)
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.Type == PType && s.Date == date && s.PurchaseId == null && s.Description != "Claim Cash" && s.Description != "Requested Claim").ToList();
            return listOfPayments;
        }
        public List<Payments> GetDateCustomerPayments(DateTime date, string PType)
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.Type == PType && s.Date == date && s.SaleId == null ).ToList();
            return listOfPayments;
        }
        public List<Payments> GetSupplierPayments(int SID)
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.SupplierID == SID ).ToList();
            return listOfPayments;
        }

        public List<Payments> GetSupplierCashOut(int SID)
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.SupplierID == SID && s.PurchaseId == null && s.Description != "Claim Cash" && s.Description != "Requested Claim").ToList();
            return listOfPayments;
        }
        public List<Payments> GetCustomerCashIn(int SID)
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.CustomerID == SID && s.SaleId == null ).ToList();
            return listOfPayments;
        }

        public List<Payments> GetSpecificPayments(int id)
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.Id == id ).ToList();
            return listOfPayments;
        }
        public List<Payments> GetExpensePayments()
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.Type == "Expense").ToList();
            return listOfPayments;
        }

        public List<Payments> GetSpecificExpensePayments(string desc)
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.Type == "Expense" && s.Description == desc).ToList();
            return listOfPayments;
        }

        public List<Payments> GetCustomerPayments(int CID)
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.CustomerID == CID).ToList();
            return listOfPayments;
        }
        public Payments GetCustomerOpenBalance(int CID)
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.CustomerID == CID && s.Description == "Open Balance").FirstOrDefault();
            return listOfPayments;
        }

        public Payments GetPayment(int id)
        {
            Payments customers = database.PaymentsDbSet.Where(s => s.Id == id).FirstOrDefault();
            return customers;
        }

        public Payments GetSalePayment(string id)
        {
            Payments customers = database.PaymentsDbSet.Where(s => s.SaleId == id).FirstOrDefault();
            return customers;
        }

        public Payments GetCustomerClaimPayment(string id)
        {
            Payments customers = database.PaymentsDbSet.Where(s => s.CustomerClaimsID == id).FirstOrDefault();
            return customers;
        }

        public Payments GetSupplierClaimPayment(string id)
        {
            Payments customers = database.PaymentsDbSet.Where(s => s.SupplierClaimsID == id).FirstOrDefault();
            return customers;
        }
        public Payments GetPurchasePayment(string id)
        {
            Payments customers = database.PaymentsDbSet.Where(s => s.PurchaseId == id).FirstOrDefault();
            return customers;
        }

        public Payments CreatePayment(Payments payment)
        {
            database.PaymentsDbSet.Add(payment);
            database.SaveChanges();
            return payment;
        }

        public Payments EditPayment(Payments payment)
        {
            Payments model = GetPayment(payment.Id);
            model.SupplierID = payment.SupplierID;
            model.Debit = payment.Debit;
            model.CustomerID = payment.CustomerID;
            model.Credit = payment.Credit;
            model.Date = payment.Date;
            model.EntryDate = payment.EntryDate;
            model.Description = payment.Description;
            
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Payments EditSaleAmountPayment(Entities.Sale Sale)
        {
            Payments model = GetSalePayment(Sale.SId);
            model.Debit = Sale.TotalPrice;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Payments EditSaleDate(Entities.Sale Sale)
        {
            Payments model = GetSalePayment(Sale.SId);
            model.Date = Sale.SaleDate;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }
        public Payments EditCustomerClaimDate(Entities.CustomerClaims Claim)
        {
            Payments model = GetCustomerClaimPayment(Claim.CId);
            model.Date = Claim.ClaimDate;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }
        public Payments EditSupplierClaimDate(Entities.SupplierClaims Claim)
        {
            Payments model = GetSupplierClaimPayment(Claim.CId);
            model.Date = Claim.ClaimDate;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        } 
        public Payments EditCustomerClaimPayment(string claim, float amount)
        {
            Payments model = GetCustomerClaimPayment(claim);
            model.Credit = amount;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }
        public Payments EditSupplierClaimPayment(string claim, float amount)
        {
            Payments model = GetSupplierClaimPayment(claim);
            model.Debit = amount;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }
        public Payments EditPurchaseAmountPayment(string pId, float amount)
        {
            Payments model = GetPurchasePayment(pId);
            model.Credit = amount;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Payments EditPurchasePaymentDate(Entities.Purchase purchase)
        {
            Payments model = GetPurchasePayment(purchase.PId);
            model.Date = purchase.PurchaseDate;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public void DeletePayment(Payments payment)
        {
            database.PaymentsDbSet.Remove(payment);
            database.SaveChanges();
        }

        public List<Payments> GetDateExpensePayments(DateTime date)
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.Type == "Expense" && s.Date == date ).ToList();
            return listOfPayments;
        }


        public List<Payments> GetDateBetweenSupplierPayments(int SID,DateTime date1, DateTime date2)
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.SupplierID == SID && s.Date >= date1 && s.Date <= date2).ToList();
            return listOfPayments;
        }

        public List<Payments> GetDateNOTBetweenSupplierPayments(int SID, DateTime date1, DateTime date2)
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.SupplierID == SID && !(s.Date > date2) && !(s.Date >= date1 && s.Date <= date2)).ToList();
            return listOfPayments;
        }

        public List<Payments> GetDateBetweenCustomerPayments(int CID , DateTime date1 , DateTime date2)
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.CustomerID == CID && s.Date >= date1 && s.Date <= date2).ToList();
            return listOfPayments;
        }

        public List<Payments> GetNOTDateBetweenCustomerPayments(int CID, DateTime date1, DateTime date2)
        {
            var listOfPayments = database.PaymentsDbSet.Where(s => s.CustomerID == CID && !(s.Date >= date1 && s.Date <= date2) && !(s.Date > date2)).ToList();
            return listOfPayments;
        }

    }
}
