using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using BizBook.Entities;

namespace BizBook.Models
{
    public class CustomerDatabaseHelper
    {
        private readonly DatabaseContext database = new DatabaseContext();

        public List<Customers> GetCustomers()
        {
            var listOfCompanies = database.CustomerDbSet.ToList();
            return listOfCompanies;
        }

        public List<Models.RunningBalanceCustomersViewModel> GetRunningBalance()
        {
            List<Models.RunningBalanceCustomersViewModel> runningBalanceCustomersViewModel = new List<RunningBalanceCustomersViewModel>();
            List<Entities.Customers> customersList = database.CustomerDbSet.ToList();
            foreach(var customer in customersList)
            {
                List<Entities.Payments> Creditpayments = database.PaymentsDbSet.Where(s => s.CustomerID == customer.Id).ToList();

                float Credit = (float)Creditpayments.Sum(s => s.Credit);
                float Debit = (float)Creditpayments.Sum(s => s.Debit);

                float runningBalance = Debit - Credit;
                Models.RunningBalanceCustomersViewModel viewModel = new RunningBalanceCustomersViewModel
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Contact = customer.Contact,
                    Address = customer.Address,
                    Date = customer.Date,
                    CreditStatus = customer.CreditStatus,
                    CreditLimit = customer.CreditLimit,
                    Sales = Debit,
                    Credit = Credit,
                    CreditBalance = customer.CreditBalance,
                    DebitBalance = customer.DebitBalance,
                    RunningBalance = runningBalance
                };
                runningBalanceCustomersViewModel.Add(viewModel);
            }



            return runningBalanceCustomersViewModel;
        }
        public List<Models.RunningBalanceCustomersViewModel> GetSpecificRunningBalance(Entities.Customers customer)
        {
            List<Models.RunningBalanceCustomersViewModel> runningBalanceCustomersViewModel = new List<RunningBalanceCustomersViewModel>();
                List<Entities.Payments> Creditpayments = database.PaymentsDbSet.Where(s => s.CustomerID == customer.Id).ToList();

            float Credit = (float)Creditpayments.Sum(s => s.Credit);
            float Debit = (float)Creditpayments.Sum(s => s.Debit);

            float runningBalance = Debit - Credit;
         
                Models.RunningBalanceCustomersViewModel viewModel = new RunningBalanceCustomersViewModel
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Contact = customer.Contact,
                    Address = customer.Address,
                    Date = customer.Date,
                    CreditStatus = customer.CreditStatus,
                    CreditLimit = customer.CreditLimit,
                    Sales = Debit,
                    Credit = Credit,
                    CreditBalance = customer.CreditBalance,
                    DebitBalance = customer.DebitBalance,
                    RunningBalance = runningBalance
                };
                runningBalanceCustomersViewModel.Add(viewModel);
           



            return runningBalanceCustomersViewModel;
        }


        public float CheckGetSpecificRunningBalance(Entities.Customers customer)
        {
            List<Entities.Payments> Salespayments = database.PaymentsDbSet.Where(s => s.CustomerID == customer.Id && s.SaleId != null).ToList();
            List<Entities.Payments> Creditpayments = database.PaymentsDbSet.Where(s => s.CustomerID == customer.Id && s.SaleId == null).ToList();

            float Sales = Salespayments.Sum(s => s.Sale.TotalPrice);
            int Cashin = (int)Creditpayments.Sum(s => s.Credit);
            float runningBalance = (float)(Sales - Cashin);
         
            return runningBalance;
        }

        public Customers GetCustomer(int id)
        {
            Customers customers = database.CustomerDbSet.Where(s => s.Id == id).FirstOrDefault();
            return customers;
        }

        public Customers CreateCustomer(Customers customers , DateTime balance_date )
        {
            database.CustomerDbSet.Add(customers);
            Payments _payments = new Payments();
            _payments.CustomerID = customers.Id;
            _payments.Credit = customers.CreditBalance;
            _payments.Date = balance_date;
            _payments.EntryDate = DateTime.Now.Date;
            _payments.Type = "Customer";
            _payments.Description = "Open Balance";
            _payments.Debit = customers.DebitBalance;
            database.PaymentsDbSet.Add(_payments);
            database.SaveChanges();
            return customers;
        }

        public Customers EditCustomer(Customers customers,DateTime date)
        {
            Customers model = GetCustomer(customers.Id);
            model.Name = customers.Name;
            model.Contact = customers.Contact;
            model.Address = customers.Address;
            model.Date = date.ToString();
            model.CreditBalance = customers.CreditBalance;
            model.DebitBalance = customers.DebitBalance;
            model.CreditLimit = customers.CreditLimit;
            model.CreditStatus = customers.CreditStatus;
            Entities.Payments paymentsModel = GetOpenBalancePayment(customers.Id);
            paymentsModel.Credit = customers.CreditBalance;
            paymentsModel.Debit = customers.DebitBalance;
            paymentsModel.Date = date;
            database.Entry(model).State = EntityState.Modified;
            database.Entry(paymentsModel).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public Customers EditCustomer(Customers customers)
        {
            Customers model = GetCustomer(customers.Id);
            model.Name = customers.Name;
            model.Contact = customers.Contact;
            model.Address = customers.Address;
            model.Date = customers.Date;
            model.CreditBalance = customers.CreditBalance;
            model.DebitBalance = customers.DebitBalance;
            model.CreditLimit = customers.CreditLimit;
            model.CreditStatus = customers.CreditStatus;
            database.Entry(model).State = EntityState.Modified;
            database.SaveChanges();
            return model;
        }

        public void DeleteCustomer(Customers customer)
        {
            database.CustomerDbSet.Remove(customer);
            database.SaveChanges();
        }
        public CompanyDetailsViewModel GetCompanyDetails(int companyId)
        {
            var Data = (from company in database.CompaniesDbSet
                       where company.Id == companyId
                       select new CompanyDetailsViewModel
                       {
                           Company = company,
                           Users = (from users in database.UsersDbSet where users.CompanyId == company.Id select users).ToList()
                       }).FirstOrDefault();
            return Data;
        }
        public List<Entities.Payments> CashInBetweenDates(DateTime dateFrom, DateTime dateTo)
        {
            var model = database.PaymentsDbSet.Where(s => s.Date >= dateFrom && s.Date <= dateTo && s.SaleId == null && s.Type == "Customer").ToList();
            return model;
        }

        public List<Entities.Payments> SpecificCashInBetweenDates(int id, DateTime dateFrom, DateTime dateTo)
        {
            var model = database.PaymentsDbSet.Where(s => s.CustomerID == id && s.Date >= dateFrom && s.Date <= dateTo && s.SaleId == null && s.Type == "Customer").ToList();
            return model;
        }

        public Payments GetOpenBalancePayment(int id)
        {
            Payments _payments = database.PaymentsDbSet.Where(s => s.CustomerID == id && s.Description == "Open Balance").FirstOrDefault();
            return _payments;
        }
    }
}
