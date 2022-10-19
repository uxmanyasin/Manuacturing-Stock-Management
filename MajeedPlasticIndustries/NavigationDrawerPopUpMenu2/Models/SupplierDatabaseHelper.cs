using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizBook.Entities;

namespace BizBook.Models
{
    public class SupplierDatabaseHelper
    {
        private readonly DatabaseContext database = new DatabaseContext();

        public List<Suppliers> GetSuppliers()
        {
            var listOfSuppliers = database.SupplierDbSet.ToList();
            return listOfSuppliers;
        }

        public List<Suppliers> GetDateSuppliers(string date)
        {
            var listOfSuppliers = database.SupplierDbSet.Where(s => s.Date == date).ToList();
            return listOfSuppliers;
        }
        public Suppliers GetSupplier(int id)
        {
            Suppliers suppliers = database.SupplierDbSet.Where(s => s.Id == id).FirstOrDefault();
            return suppliers;
        }
        public Suppliers CreateSupplier(Suppliers suppliers,DateTime balanceDate)
        {
            database.SupplierDbSet.Add(suppliers);
            Payments _payments = new Payments();
            _payments.SupplierID = suppliers.Id;
            _payments.Credit = suppliers.CreditBalance;
            _payments.Date = balanceDate;
            _payments.EntryDate = DateTime.Now.Date;
            _payments.Type = "Supplier";
            _payments.Description = "Open Balance";
            _payments.Debit = suppliers.DebitBalance;
            database.PaymentsDbSet.Add(_payments);
            database.SaveChanges();
            return suppliers;
        }

        public Suppliers EditSupplier(Suppliers suppliers,DateTime date)
        {
            Suppliers model = GetSupplier(suppliers.Id);
            model.Name = suppliers.Name;
            model.Contact = suppliers.Contact;
            model.Address = suppliers.Address;
            model.Date = date.ToString();
            model.DebitBalance = suppliers.DebitBalance;
            model.CreditBalance = suppliers.CreditBalance;
            Entities.Payments paymentsModel = GetOpenBalancePayment(suppliers.Id);
            paymentsModel.Credit = suppliers.CreditBalance;
            paymentsModel.Debit = suppliers.DebitBalance;
            paymentsModel.Date = date;
            database.Entry(model).State = EntityState.Modified;
            database.Entry(paymentsModel).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public void DeleteSupplier(Suppliers suppliers)
        {
            database.SupplierDbSet.Remove(suppliers);
            database.SaveChanges();
        }

        public List<Entities.Payments> CashInBetweenDates(DateTime dateFrom, DateTime dateTo)
        {
            var model = database.PaymentsDbSet.Where(s => s.Date >= dateFrom && s.Date <= dateTo && s.PurchaseId == null && s.Type == "Supplier").ToList();
            return model;
        }
        public List<Entities.Payments> SpecificCashInBetweenDates(int id,DateTime dateFrom, DateTime dateTo)
        {
            var model = database.PaymentsDbSet.Where(s => s.SupplierID == id && s.Date >= dateFrom && s.Date <= dateTo && s.PurchaseId == null && s.Type == "Supplier").ToList();
            return model;
        }

        public List<Models.RunningBalanceCustomersViewModel> GetRunningBalance()
        {
            List<Models.RunningBalanceCustomersViewModel> runningBalanceCustomersViewModel = new List<RunningBalanceCustomersViewModel>();
            List<Entities.Suppliers> suppliersList = database.SupplierDbSet.ToList();
            foreach (var supplier in suppliersList)
            {
                List<Entities.Payments> Debitpayments = database.PaymentsDbSet.Where(s => s.SupplierID == supplier.Id ).ToList();
                float Debit = (float)Debitpayments.Sum(s => s.Debit);
                float Credit = (float)Debitpayments.Sum(s => s.Credit);
                float runningBalance = (Debit - Credit);
                Models.RunningBalanceCustomersViewModel viewModel = new RunningBalanceCustomersViewModel
                {
                    Id = supplier.Id,
                    Name = supplier.Name,
                    Contact = supplier.Contact,
                    Address = supplier.Address,
                    Date = supplier.Date,
                    CreditBalance = supplier.CreditBalance,
                    DebitBalance = supplier.DebitBalance,
                    CreditStatus = 0,
                    CreditLimit = 0,
                    Sales = Debit,
                    Credit = Credit,
                    RunningBalance = runningBalance
                };
                runningBalanceCustomersViewModel.Add(viewModel);
            }

            return runningBalanceCustomersViewModel;
        }

        public List<Models.RunningBalanceCustomersViewModel> GetSpecificRunningBalance(Entities.Suppliers supplier)
        {
            List<Models.RunningBalanceCustomersViewModel> runningBalanceCustomersViewModel = new List<RunningBalanceCustomersViewModel>();
            {
                List<Entities.Payments> Purchasepayments = database.PaymentsDbSet.Where(s => s.SupplierID == supplier.Id && s.PurchaseId != null).ToList();
                List<Entities.Payments> Debitpayments = database.PaymentsDbSet.Where(s => s.SupplierID == supplier.Id && s.PurchaseId == null).ToList();
                var pModel = Debitpayments.Where(item => item.SupplierID == supplier.Id && item.Description == "Open Balance").FirstOrDefault();

                float Debit = (float)Debitpayments.Sum(s => s.Debit);
                float Credit = (float)Debitpayments.Sum(s => s.Credit);
                float runningBalance = (Debit - Credit);
                Models.RunningBalanceCustomersViewModel viewModel = new RunningBalanceCustomersViewModel
                {
                    Id = supplier.Id,
                    Name = supplier.Name,
                    Contact = supplier.Contact,
                    Address = supplier.Address,
                    Date = pModel.Date.ToString(),
                    CreditStatus = 0,
                    CreditLimit = 0,
                    CreditBalance = supplier.CreditBalance,
                    DebitBalance = supplier.DebitBalance,
                    Sales = Debit,
                    Credit = Credit,
                    RunningBalance = runningBalance
                };
                runningBalanceCustomersViewModel.Add(viewModel);




                return runningBalanceCustomersViewModel;
            }
        }
        public Payments GetOpenBalancePayment(int id)
        {
            Payments _payments = database.PaymentsDbSet.Where(s => s.SupplierID == id && s.Description == "Open Balance").FirstOrDefault();
            return _payments;
        }
    }
}
