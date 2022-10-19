using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BizBook.Entities;
using BizBook.Models;

namespace BizBook
{

    public partial class Customer : UserControl
    {
        private readonly CustomerDatabaseHelper customerDatabaseHelper = new CustomerDatabaseHelper();
        private readonly PaymentsDatabaseHelper paymentsDatabaseHelper = new PaymentsDatabaseHelper();
        public List<Payments> CashInList = new List<Payments>();
        public List<Customers> custList = new List<Customers>();
        public List<Models.RunningBalanceCustomersViewModel> RunningBalancesList = new List<RunningBalanceCustomersViewModel>();
        private ObservableCollection<CashInOutModel> cashinmodel = new ObservableCollection<CashInOutModel>();
        public List<Models.DummyModels.CustomerSupplierPaymentModel> dummyPayments = new List<Models.DummyModels.CustomerSupplierPaymentModel>(); 
        public List<Models.DummyModels.CustomerRunningBalanceViewModel> dummyList = new List<Models.DummyModels.CustomerRunningBalanceViewModel>();
        IFormatProvider usFormatProvider = new System.Globalization.CultureInfo("en-US");

        public Customer()
        {
            InitializeComponent();
            LoadCustomers();
        }

        private void BtnNewCustomer_Click(object sender, RoutedEventArgs e)
        {
            New_Customer n = new New_Customer();
            n.ShowDialog();
        }

        private void BtnNewCashIn_Click(object sender, RoutedEventArgs e)
        {
         
        }

        private void LoadAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                dummyPayments.Clear();
                CashInList.Clear();
                CashInList = paymentsDatabaseHelper.GetPayments("Customer").ToList();
                CashInList.Sort((x, y) => x.Date.CompareTo(y.Date));
                foreach (var item in CashInList) 
                {
                    float Credit = (float)item.Credit;
                    float Debit = (float)item.Debit;
                    Models.DummyModels.CustomerSupplierPaymentModel model = new Models.DummyModels.CustomerSupplierPaymentModel 
                    {
                        Id = item.Id,
                        CustomerName = item.CustomerName,
                        CustomerID = item.CustomerID,
                        SupplierID = 0,//item.SupplierID,
                        Credit = Credit.ToString("#,##0.00", usFormatProvider),
                        Debit = Debit.ToString("#,##0.00", usFormatProvider),
                        Date = item.Date,
                        EntryDate = item.EntryDate,
                        Type = item.Type,
                        Description = item.Description,
                        PurchaseId = "", //item.PurchaseId,
                        SupplierClaimsID = "", //item.SupplierClaimsID,
                        CustomerClaimsID = "", //item.CustomerClaimsID,
                        SaleId = "", //item.SaleId

                    };
                    dummyPayments.Add(model);
                }
                CashInListView.ItemsSource = dummyPayments;
                float TotalCashIn = (float)CashInList.Sum(s => s.Credit);
                float TotalCashOut = (float)CashInList.Sum(s => s.Debit);
                LblTotalCashIn.Content = TotalCashIn.ToString("#,##0.00", usFormatProvider);
                LblTotalCashOut.Content = TotalCashOut.ToString("#,##0.00", usFormatProvider);

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no Cash-In details to show.");
                infobox.ShowDialog();
            }

        }

        private void CustomerCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                custList = customerDatabaseHelper.GetCustomers().ToList();
                CustomerCombo.ItemsSource = custList;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no Customers to show.");
                infobox.ShowDialog();
            }
        }

        private void CustomerCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                RunningBalancesList.Clear();
                dummyList.Clear();
                lblTotalRunningBalance.Content = "0";
                lblTotalSales.Content = "0";
                lblTotalCashins.Content = "0";

                var customer = CustomerCombo.SelectedItem as Entities.Customers;
                RunningBalancesList = customerDatabaseHelper.GetSpecificRunningBalance(customer);
                RunningBalancesList.Sort((x, y) => x.Date.CompareTo(y.Date));
                foreach (var item in RunningBalancesList)
                {
                    Models.DummyModels.CustomerRunningBalanceViewModel model = new Models.DummyModels.CustomerRunningBalanceViewModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Contact = item.Contact,
                        Address = item.Address,
                        Date = item.Date,
                        RunningBalance = item.RunningBalance.ToString("#,##0.00", usFormatProvider),
                        CreditBalance = item.CreditBalance.ToString("#,##0.00", usFormatProvider),
                        DebitBalance = item.DebitBalance.ToString("#,##0.00", usFormatProvider),
                        Sales = item.Sales.ToString("#,##0.00", usFormatProvider),
                        Credit = item.Credit.ToString("#,##0.00", usFormatProvider),
                        CreditLimit = item.CreditLimit.ToString("#,##0.00", usFormatProvider),
                        CreditStatus = item.CreditStatus,
                    };
                    dummyList.Add(model);
                }
                CustomerListView.ItemsSource = dummyList;
               
                CustomerListView.Items.Refresh();
     
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,Please select customer first.");
                infobox.ShowDialog();
            }

        }

        private void BtnLoadAll_Click(object sender, RoutedEventArgs e)
        {
            LoadCustomers();
        }

        public void LoadCustomers()
        {
            try
            {
                dummyList.Clear();
                RunningBalancesList.Clear();
                RunningBalancesList = customerDatabaseHelper.GetRunningBalance();
                RunningBalancesList.Sort((x, y) => x.Date.CompareTo(y.Date));
                foreach (var item in RunningBalancesList) 
                {
                    Models.DummyModels.CustomerRunningBalanceViewModel model = new Models.DummyModels.CustomerRunningBalanceViewModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Contact = item.Contact,
                        Address = item.Address,
                        Date = item.Date,
                        RunningBalance = item.RunningBalance.ToString("#,##0.00", usFormatProvider),
                        CreditBalance = item.CreditBalance.ToString("#,##0.00", usFormatProvider),
                        DebitBalance = item.DebitBalance.ToString("#,##0.00", usFormatProvider),
                        Sales = item.Sales.ToString("#,##0.00", usFormatProvider),
                        Credit = item.Credit.ToString("#,##0.00", usFormatProvider),
                        CreditLimit = item.CreditLimit.ToString("#,##0.00", usFormatProvider),
                        CreditStatus = item.CreditStatus,
                    };
                    dummyList.Add(model);
                }
                CustomerListView.ItemsSource = dummyList;
                CustomerListView.Items.Refresh();
                lblTotalSales.Content = RunningBalancesList.Sum(s=> s.Sales).ToString("#,##0.00", usFormatProvider);
                lblTotalCashins.Content = RunningBalancesList.Sum(s => s.Credit).ToString("#,##0.00", usFormatProvider);
                lblTotalRunningBalance.Content = RunningBalancesList.Sum(s => s.RunningBalance).ToString("#,##0.00", usFormatProvider);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                InfoBox infobox = new InfoBox("Something went wrong,there are no Customers to show.");
                infobox.ShowDialog();
            }
        }

        private void BtnLoadAllCashIn_Click(object sender, RoutedEventArgs e)
        {
            GetCashIns();
        }

        private void BtnGridUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
               var model = CustomerListView.SelectedItem as Models.DummyModels.CustomerRunningBalanceViewModel;
                Entities.Customers customer = customerDatabaseHelper.GetCustomer(model.Id);
                New_Customer SS = new New_Customer(customer, true);
                SS.ShowDialog();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,please select customer.");
                infobox.ShowDialog();
            }
        }

        private void CashInTab_Initialized(object sender, EventArgs e)
        {
            
        }
        private void GetCashIns()
        {
            try
            {
                CashInList.Clear();
                dummyList.Clear();
                dummyPayments.Clear();
                CashInListView.ItemsSource = null;
                Models.PaymentsDatabaseHelper helper = new PaymentsDatabaseHelper();
                CashInList = helper.GetPayments("Customer").ToList(); 
                CashInList.Sort((x, y) => x.Date.CompareTo(y.Date));
                foreach (var item in CashInList)
                {
                    float Credit = (float)item.Credit;
                    float Debit = (float)item.Debit;
                    Models.DummyModels.CustomerSupplierPaymentModel model = new Models.DummyModels.CustomerSupplierPaymentModel
                    {
                        Id = item.Id,
                        CustomerName = item.CustomerName,
                        CustomerID = item.CustomerID,
                        SupplierID = item.SupplierID,
                        Customer = item.customers.Name,
                        Credit = Credit.ToString("#,##0.00", usFormatProvider),
                        Debit = Debit.ToString("#,##0.00", usFormatProvider),
                        Date = item.Date,
                        EntryDate = item.EntryDate,
                        Type = item.Type,
                        Description = item.Description,
                        PurchaseId = item.PurchaseId,
                        SupplierClaimsID = item.SupplierClaimsID,
                        CustomerClaimsID = item.CustomerClaimsID,
                        SaleId = item.SaleId

                    };
                    dummyPayments.Add(model);
                }
                CashInListView.ItemsSource = dummyPayments;
                CashInListView.Items.Refresh();
                float TotalCashOut = (float)CashInList.Sum(s => s.Debit);
                float TotalCashIn = (float)CashInList.Sum(s => s.Credit);
                LblTotalCashOut.Content = TotalCashOut.ToString("#,##0.00", usFormatProvider);
                LblTotalCashIn.Content = TotalCashIn.ToString("#,##0.00", usFormatProvider);

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,Please Select a Csutomer to load Cash-In details.");
                infobox.ShowDialog();
            }
        }

        private void CustomerCashCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                custList.Clear();
                custList = customerDatabaseHelper.GetCustomers().ToList();
                CustomerCashCombo.ItemsSource = custList;
                CustomerCashCombo.Items.Refresh();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no Customers to show.");
                infobox.ShowDialog();
            }
        }

        private void SearchCashInCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CashInList.Clear();
                dummyList.Clear();
                dummyPayments.Clear();
                int id = Convert.ToInt32(CustomerCashCombo.SelectedValue.ToString());
                CashInList = paymentsDatabaseHelper.GetCustomerCashIn(id).ToList();
                CashInList.Sort((x, y) => x.Date.CompareTo(y.Date));
                foreach (var item in CashInList)
                {
                    float Credit = (float)item.Credit;
                    float Debit = (float)item.Debit;
                    Models.DummyModels.CustomerSupplierPaymentModel model = new Models.DummyModels.CustomerSupplierPaymentModel
                    {
                        Id = item.Id,
                        CustomerName = item.CustomerName,
                        CustomerID = item.CustomerID,
                        SupplierID = item.SupplierID,
                        Customer = item.customers.Name,

                        Credit = Credit.ToString("#,##0.00", usFormatProvider),
                        Debit = Debit.ToString("#,##0.00", usFormatProvider),
                        Date = item.Date,
                        EntryDate = item.EntryDate,
                        Type = item.Type,
                        Description = item.Description,
                        PurchaseId = item.PurchaseId,
                        SupplierClaimsID = item.SupplierClaimsID,
                        CustomerClaimsID = item.CustomerClaimsID,
                        SaleId = item.SaleId

                    };
                    dummyPayments.Add(model);
                }
                CashInListView.ItemsSource = dummyPayments;
                CashInListView.Items.Refresh();
                float TotalCashOut = (float)CashInList.Sum(s => s.Debit);
                float TotalCashIn = (float)CashInList.Sum(s => s.Credit);
                LblTotalCashOut.Content = TotalCashOut.ToString("#,##0.00", usFormatProvider);
                LblTotalCashIn.Content = TotalCashIn.ToString("#,##0.00", usFormatProvider);

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,Please Select a Csutomer to load Cash-In details.");
                infobox.ShowDialog();
            }
        }

        private void SearchCashDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CashInList.Clear();
                dummyList.Clear();
                dummyPayments.Clear();
                DateTime date = CashDatePicker.SelectedDate.Value.Date;
                CashInList = paymentsDatabaseHelper.GetDateCustomerPayments(date, "Customer");
                CashInList.Sort((x, y) => x.Date.CompareTo(y.Date));
                foreach (var item in CashInList)
                {
                    float Credit = (float)item.Credit;
                    float Debit = (float)item.Debit;
                    Models.DummyModels.CustomerSupplierPaymentModel model = new Models.DummyModels.CustomerSupplierPaymentModel
                    {
                        Id = item.Id,
                        CustomerName = item.CustomerName,
                        CustomerID = item.CustomerID,
                        SupplierID = item.SupplierID,
                        Customer = item.customers.Name,

                        Credit = Credit.ToString("#,##0.00", usFormatProvider),
                        Debit = Debit.ToString("#,##0.00", usFormatProvider),
                        Date = item.Date,
                        EntryDate = item.EntryDate,
                        Type = item.Type,
                        Description = item.Description,
                        PurchaseId = item.PurchaseId,
                        SupplierClaimsID = item.SupplierClaimsID,
                        CustomerClaimsID = item.CustomerClaimsID,
                        SaleId = item.SaleId

                    };
                    dummyPayments.Add(model);
                }
                CashInListView.ItemsSource = dummyPayments;
                CashInListView.Items.Refresh();
                float TotalCashOut = (float)CashInList.Sum(s => s.Debit);
                float TotalCashIn = (float)CashInList.Sum(s => s.Credit);
                LblTotalCashOut.Content = TotalCashOut.ToString("#,##0.00", usFormatProvider);
                LblTotalCashIn.Content = TotalCashIn.ToString("#,##0.00", usFormatProvider);

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,Please Select a date to load Cash-In details.");
                infobox.ShowDialog();
            }
        }

        private void BtnGridDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var  model = CashInListView.SelectedItem as Models.DummyModels.CustomerSupplierPaymentModel;
                var payments = paymentsDatabaseHelper.GetPayment(model.Id);
                Confirmation confirmation = new Confirmation(payments.Id);
                confirmation.ShowDialog();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there is no data to delete.");
                infobox.ShowDialog();
            }
        }

        private void SearchDateBetween_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CashInList.Clear();
                dummyPayments.Clear();
                DateTime dateFrom = DateFrom.SelectedDate.Value.Date;
                DateTime dateTo = DateTo.SelectedDate.Value.Date;
                CashInList = customerDatabaseHelper.CashInBetweenDates(dateFrom, dateTo);
                CashInList.Sort((x, y) => x.Date.CompareTo(y.Date));
                foreach (var item in CashInList)
                {
                    float Credit = (float)item.Credit;
                    float Debit = (float)item.Debit;
                    Models.DummyModels.CustomerSupplierPaymentModel model = new Models.DummyModels.CustomerSupplierPaymentModel
                    {
                        Id = item.Id,
                        CustomerName = item.CustomerName,
                        CustomerID = item.CustomerID,
                        SupplierID = item.SupplierID,
                        Customer = item.customers.Name,

                        Credit = Credit.ToString("#,##0.00", usFormatProvider),
                        Debit = Debit.ToString("#,##0.00", usFormatProvider),
                        Date = item.Date,
                        EntryDate = item.EntryDate,
                        Type = item.Type,
                        Description = item.Description,
                        PurchaseId = item.PurchaseId,
                        SupplierClaimsID = item.SupplierClaimsID,
                        CustomerClaimsID = item.CustomerClaimsID,
                        SaleId = item.SaleId

                    };
                    dummyPayments.Add(model);
                }
                CashInListView.ItemsSource = dummyPayments;
                CashInListView.Items.Refresh();
                float TotalCashOut = (float)CashInList.Sum(s => s.Debit);
                float TotalCashIn = (float)CashInList.Sum(s => s.Credit);
                LblTotalCashOut.Content = TotalCashOut.ToString("#,##0.00", usFormatProvider);
                LblTotalCashIn.Content = TotalCashIn.ToString("#,##0.00", usFormatProvider);

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There are no selected date range to show report.");
                infobox.ShowDialog();
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                float TotalCashIn = 0;
                float v1 = 0;
                foreach (var cashIn in CashInList)
                {
                    v1 = Convert.ToInt32(cashIn.Credit);
                    TotalCashIn = v1 + TotalCashIn;
                    CashInOutModel model = new CashInOutModel()
                    {
                        Date = cashIn.Date,
                        description = cashIn.Description,
                        cash = cashIn.Credit

                    };
                    cashinmodel.Add(model);
                }
                int id = Convert.ToInt32(CustomerCashCombo.SelectedValue.ToString());
                var customer = customerDatabaseHelper.GetCustomer(id);
                CashINViewReport cashINViewReport = new CashINViewReport(customer, cashinmodel, TotalCashIn);
                cashINViewReport.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //InfoBox infobox = new InfoBox("Something went wrong,there are no data to generate report.");
                //infobox.ShowDialog();
            }
        }

        private void btnNewCashIN_Click_1(object sender, RoutedEventArgs e)
        {
            New_CashINAdvance new_CashINAdvance = new New_CashINAdvance();
            new_CashINAdvance.ShowDialog();
        }

        private void btnPrintCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string balance = lblTotalRunningBalance.Content.ToString();
                List<Reporting.CustomersRunningBalance> _RunningBalanceList = new List<Reporting.CustomersRunningBalance>();
                foreach (var item in RunningBalancesList) 
                {
                    Reporting.CustomersRunningBalance model = new Reporting.CustomersRunningBalance() 
                    {
                        Name = item.Name,
                        Contact = item.Contact,
                        Address = item.Address,
                        Date = item.Date,
                        Balance = item.RunningBalance.ToString()
                    };
                    _RunningBalanceList.Add(model);
                }
                PrintCustomerRunningBalance printscreen = new PrintCustomerRunningBalance(_RunningBalanceList, balance);
                printscreen.ShowDialog();
            }
            catch (Exception)
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no data to generate report.");
                infobox.ShowDialog();
            }
        }

        private void btnNewCashOut_Click(object sender, RoutedEventArgs e)
        {
            New_Cash_OUT new_Cash_OUT = new New_Cash_OUT();
            new_Cash_OUT.ShowDialog();
        }

        private void SearchSpecificDateBetween_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CashInList.Clear();
                dummyPayments.Clear();
                DateTime dateFrom = DateFrom.SelectedDate.Value.Date;
                DateTime dateTo = DateTo.SelectedDate.Value.Date;
                var customer = CustomerCashCombo.SelectedItem as Entities.Customers;
                CashInList = customerDatabaseHelper.SpecificCashInBetweenDates(customer.Id,dateFrom, dateTo);
                CashInList.Sort((x, y) => x.Date.CompareTo(y.Date));
                foreach (var item in CashInList)
                {
                    float Credit = (float)item.Credit;
                    float Debit = (float)item.Debit;
                    Models.DummyModels.CustomerSupplierPaymentModel model = new Models.DummyModels.CustomerSupplierPaymentModel
                    {
                        Id = item.Id,
                        CustomerName = item.CustomerName,
                        CustomerID = item.CustomerID,
                        SupplierID = item.SupplierID,
                        Customer = item.customers.Name,

                        Credit = Credit.ToString("#,##0.00", usFormatProvider),
                        Debit = Debit.ToString("#,##0.00", usFormatProvider),
                        Date = item.Date,
                        EntryDate = item.EntryDate,
                        Type = item.Type,
                        Description = item.Description,
                        PurchaseId = item.PurchaseId,
                        SupplierClaimsID = item.SupplierClaimsID,
                        CustomerClaimsID = item.CustomerClaimsID,
                        SaleId = item.SaleId

                    };
                    dummyPayments.Add(model);
                }
                CashInListView.ItemsSource = dummyPayments;
                CashInListView.Items.Refresh();
                float TotalCashOut = (float)CashInList.Sum(s => s.Debit);
                float TotalCashIn = (float)CashInList.Sum(s => s.Credit);
                LblTotalCashOut.Content = TotalCashOut.ToString("#,##0.00", usFormatProvider);
                LblTotalCashIn.Content = TotalCashIn.ToString("#,##0.00", usFormatProvider);

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There are no selected date range to show report.");
                infobox.ShowDialog();
            }
        }

        private void btnGridDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var model = CashInListView.SelectedItem as Models.DummyModels.CustomerSupplierPaymentModel;
                var payments = paymentsDatabaseHelper.GetPayment(model.Id);
                if (payments.Credit != 0) 
                {
                    New_CashINAdvance new_CashINAdvance = new New_CashINAdvance(payments,true);
                    new_CashINAdvance.ShowDialog();
                }
                else if (payments.Debit != 0) 
                {
                    New_Cash_OUT new_Cash_OUT = new New_Cash_OUT(payments,false);
                    new_Cash_OUT.ShowDialog();
                }
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there is no data to show.");
                infobox.ShowDialog();
            }
        }

        private void btnSearchbyID_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CashInList.Clear();
                dummyList.Clear();
                dummyPayments.Clear();
                CashInListView.ItemsSource = null;
                int id = Convert.ToInt32(txtPaymentID.Text);
                CashInList = paymentsDatabaseHelper.GetSpecificPayments(id).ToList();
                if (CashInList != null)
                {
                    CashInList.Sort((x, y) => x.Date.CompareTo(y.Date));
                    foreach (var item in CashInList)
                    {
                        float Credit = (float)item.Credit;
                        float Debit = (float)item.Debit;
                        Models.DummyModels.CustomerSupplierPaymentModel model = new Models.DummyModels.CustomerSupplierPaymentModel
                        {
                            Id = item.Id,
                            CustomerName = item.CustomerName,
                            CustomerID = item.CustomerID,
                            SupplierID = item.SupplierID,
                            Customer = item.customers.Name,

                            Credit = Credit.ToString("#,##0.00", usFormatProvider),
                            Debit = Debit.ToString("#,##0.00", usFormatProvider),
                            Date = item.Date,
                            EntryDate = item.EntryDate,
                            Type = item.Type,
                            Description = item.Description,
                            PurchaseId = item.PurchaseId,
                            SupplierClaimsID = item.SupplierClaimsID,
                            CustomerClaimsID = item.CustomerClaimsID,
                            SaleId = item.SaleId

                        };
                        dummyPayments.Add(model);
                    }
                    CashInListView.ItemsSource = dummyPayments;
                    CashInListView.Items.Refresh();
                    float TotalCashOut = (float)CashInList.Sum(s => s.Debit);
                    float TotalCashIn = (float)CashInList.Sum(s => s.Credit);
                    LblTotalCashOut.Content = TotalCashOut.ToString("#,##0.00", usFormatProvider);
                    LblTotalCashIn.Content = TotalCashIn.ToString("#,##0.00", usFormatProvider);
                }
                InfoBox infobox = new InfoBox("Your ID is irrelevant.");
                infobox.ShowDialog();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,Please Select a Csutomer to load Cash-In details.");
                infobox.ShowDialog();
            }
        }
    }
}
