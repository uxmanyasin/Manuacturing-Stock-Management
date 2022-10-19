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
   
    public partial class Supplier : UserControl
    {
        private readonly PaymentsDatabaseHelper paymentsDatabaseHelper = new PaymentsDatabaseHelper();
        private readonly SupplierDatabaseHelper helper = new SupplierDatabaseHelper();
        public List<Suppliers> supplierlist = new List<Suppliers>();
        public List<Entities.Suppliers> FullSuppliersList = new List<Suppliers>();
        public List<Entities.Payments> paymentsList = new List<Payments>();
        public List<Models.RunningBalanceCustomersViewModel> RunningBalancesList = new List<RunningBalanceCustomersViewModel>();
        public ObservableCollection<CashInOutModel> cashoutmodel = new ObservableCollection<CashInOutModel>();
        public List<Models.DummyModels.CustomerRunningBalanceViewModel> DummyRunningBalanceList = new List<Models.DummyModels.CustomerRunningBalanceViewModel>();
        public List<Models.DummyModels.CustomerSupplierPaymentModel> dummyPayments = new List<Models.DummyModels.CustomerSupplierPaymentModel>();
        IFormatProvider usFormatProvider = new System.Globalization.CultureInfo("en-US");

        public Supplier()
        {
            InitializeComponent();
            LoadSuppliers();
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void BtnNewSupplier_Click(object sender, RoutedEventArgs e)
        {
            New_Supplier n = new New_Supplier();
            n.ShowDialog();
        }

        private void BtnNewCashOut_Click(object sender, RoutedEventArgs e)
        {
            New_Cash_OUT o = new New_Cash_OUT();
            o.ShowDialog();
        }
        private void BtnLoadSuppliers_Click(object sender, RoutedEventArgs e)
        {
            LoadSuppliers();
        }

        private void LoadSuppliers()
        {
            try
            {
                RunningBalancesList.Clear();
                DummyRunningBalanceList.Clear();
                Models.SupplierDatabaseHelper supplierDatabaseHelper = new SupplierDatabaseHelper();
                RunningBalancesList = supplierDatabaseHelper.GetRunningBalance();
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
                        CreditBalance = item.CreditBalance.ToString("#,##0.00", usFormatProvider),
                        DebitBalance = item.DebitBalance.ToString("#,##0.00", usFormatProvider),
                        Sales = item.Sales.ToString("#,##0.00", usFormatProvider),
                        Credit = item.Credit.ToString("#,##0.00", usFormatProvider),
                        RunningBalance = item.RunningBalance.ToString("#,##0.00", usFormatProvider),
                    };
                    DummyRunningBalanceList.Add(model);
                }
                SuppliersListView.ItemsSource = DummyRunningBalanceList;
                SuppliersListView.Items.Refresh();
                lblTotalPurchase.Content = RunningBalancesList.Sum(s => s.Sales).ToString("#,##0.00", usFormatProvider);
                lblTotalCashOuts.Content = RunningBalancesList.Sum(s => s.Credit).ToString("#,##0.00", usFormatProvider);
                lblTotalRunningBalance.Content = RunningBalancesList.Sum(s => s.RunningBalance).ToString("#,##0.00", usFormatProvider);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                InfoBox infobox = new InfoBox("Something went wrong,there are no Suppliers to show.");
                infobox.ShowDialog();
            }
        }
        private void SupplierCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                supplierlist = helper.GetSuppliers();
                supplierlist.Sort((x, y) => string.Compare(x.Name, y.Name));
                SupplierCombo.ItemsSource = supplierlist;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no suppliers to show.");
                infobox.ShowDialog();
            }
        }

        private void SupplierCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                lblTotalRunningBalance.Content = "0";
                lblTotalCashOuts.Content = "0";
                lblTotalPurchase.Content = "";
                RunningBalancesList.Clear();
                DummyRunningBalanceList.Clear();
                Entities.Suppliers supplier = SupplierCombo.SelectedItem as Entities.Suppliers;
                Models.SupplierDatabaseHelper supplierDatabaseHelper = new SupplierDatabaseHelper();
                RunningBalancesList = supplierDatabaseHelper.GetSpecificRunningBalance(supplier);
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
                        CreditBalance = item.CreditBalance.ToString("#,##0.00", usFormatProvider),
                        DebitBalance = item.DebitBalance.ToString("#,##0.00", usFormatProvider),
                        Sales = item.Sales.ToString("#,##0.00", usFormatProvider),
                        Credit = item.Credit.ToString("#,##0.00", usFormatProvider),
                        RunningBalance = item.RunningBalance.ToString("#,##0.00", usFormatProvider),
                    };
                    DummyRunningBalanceList.Add(model);
                }
                SuppliersListView.ItemsSource = DummyRunningBalanceList;
                SuppliersListView.Items.Refresh();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There is no selected supplier.");
                infobox.ShowDialog();
            }
        }

       

        private void LoadCashOuts_Click(object sender, RoutedEventArgs e)
        {
            LoadAllCashOuts();
        }

        private void BtnGridUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var model = SuppliersListView.SelectedItem as Models.DummyModels.CustomerRunningBalanceViewModel;
                Entities.Suppliers supplier = helper.GetSupplier(model.Id);
                New_Supplier SS = new New_Supplier(supplier, true);
                SS.ShowDialog();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("There is no selected supplier to update.");
                infobox.ShowDialog();
            }
        }
        private void CashOutTab_Initialized(object sender, EventArgs e)
        {
            
        }
        private void LoadAllCashOuts()
        {
            try
            {
                Models.PaymentsDatabaseHelper helper = new PaymentsDatabaseHelper();
                paymentsList.Clear();
                dummyPayments.Clear();
                CashOutListView.ItemsSource = null;
                paymentsList = helper.GetSupplierPayments("Supplier").ToList();

                paymentsList.Sort((x, y) => x.Date.CompareTo(y.Date));
                foreach (var item in paymentsList)
                {
                    float Credit = (float)item.Credit;
                    float Debit = (float)item.Debit;
                    Models.DummyModels.CustomerSupplierPaymentModel model = new Models.DummyModels.CustomerSupplierPaymentModel
                    {
                        Id = item.Id,
                        CustomerName = item.CustomerName,
                        CustomerID = item.CustomerID,
                        SupplierID = item.SupplierID,
                        Supplier = item.Suppliers.Name,

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
                CashOutListView.ItemsSource = dummyPayments;
                CashOutListView.Items.Refresh();
                float TotalCashOut = (float)paymentsList.Sum(s => s.Debit);
                float TotalCashIn = (float)paymentsList.Sum(s => s.Credit);
                LblTotalCashOut.Content = TotalCashOut.ToString("#,##0.00", usFormatProvider);
                LblTotalCashIn.Content = TotalCashIn.ToString("#,##0.00", usFormatProvider);
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no Cash-Outs to show.");
                infobox.ShowDialog();
            }
        }

        private void SupplierCashOutCombo_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                supplierlist.Clear();
                supplierlist = helper.GetSuppliers();
                supplierlist.Sort((x, y) => string.Compare(x.Name, y.Name));
                SupplierCashOutCombo.ItemsSource = supplierlist;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no suppliers to show.");
                infobox.ShowDialog();
            }
        }

        private void BtnCashOutSupplier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                paymentsList.Clear();
                dummyPayments.Clear();
                int id = Convert.ToInt32(SupplierCashOutCombo.SelectedValue.ToString());
                paymentsList = paymentsDatabaseHelper.GetSupplierCashOut(id);
                paymentsList.Sort((x, y) => x.Date.CompareTo(y.Date));
                foreach (var item in paymentsList)
                {
                    float Credit = (float)item.Credit;
                    float Debit = (float)item.Debit;
                    Models.DummyModels.CustomerSupplierPaymentModel model = new Models.DummyModels.CustomerSupplierPaymentModel
                    {
                        Id = item.Id,
                        CustomerName = item.CustomerName,
                        CustomerID = item.CustomerID,
                        SupplierID = item.SupplierID,
                        Supplier = item.Suppliers.Name,

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
                CashOutListView.ItemsSource = dummyPayments;
                CashOutListView.Items.Refresh();
                float TotalCashOut = (float)paymentsList.Sum(s => s.Debit);
                float TotalCashIn = (float)paymentsList.Sum(s => s.Credit);
                LblTotalCashOut.Content = TotalCashOut.ToString("#,##0.00", usFormatProvider);
                LblTotalCashIn.Content = TotalCashIn.ToString("#,##0.00", usFormatProvider);

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no Cash-Outs to show.");
                infobox.ShowDialog();
            }
        }

        private void SearchDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                paymentsList.Clear();
                dummyPayments.Clear();
                DateTime date = DatePicker.SelectedDate.Value.Date;
                paymentsList = paymentsDatabaseHelper.GetDateSupplierPayments(date, "Supplier");
                paymentsList.Sort((x, y) => x.Date.CompareTo(y.Date));

                foreach (var item in paymentsList)
                {
                    float Credit = (float)item.Credit;
                    float Debit = (float)item.Debit;
                    Models.DummyModels.CustomerSupplierPaymentModel model = new Models.DummyModels.CustomerSupplierPaymentModel
                    {
                        Id = item.Id,
                        CustomerName = item.CustomerName,
                        CustomerID = item.CustomerID,
                        SupplierID = item.SupplierID,
                        Supplier = item.Suppliers.Name,

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
                CashOutListView.ItemsSource = dummyPayments;
                CashOutListView.Items.Refresh();
                float TotalCashOut = (float)paymentsList.Sum(s => s.Debit);
                float TotalCashIn = (float)paymentsList.Sum(s => s.Credit);
                LblTotalCashOut.Content = TotalCashOut.ToString("#,##0.00", usFormatProvider);
                LblTotalCashIn.Content = TotalCashIn.ToString("#,##0.00", usFormatProvider);

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no Cash-Outs to show.");
                infobox.ShowDialog();
            }
        }

        private void BtnGridDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var model = CashOutListView.SelectedItem as Models.DummyModels.CustomerSupplierPaymentModel;
                var payments = paymentsDatabaseHelper.GetPayment(model.Id);
                Confirmation confirmation = new Confirmation(payments.Id);
                confirmation.ShowDialog();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,Please try again.");
                infobox.ShowDialog();
            }
        }

        private void SearchDateBetween_Click(object sender, RoutedEventArgs e)
        {
            try {
                paymentsList.Clear();
                dummyPayments.Clear();
                DateTime dateFrom = DateFrom.SelectedDate.Value.Date;
                DateTime dateTo = DateTo.SelectedDate.Value.Date;
                paymentsList = helper.CashInBetweenDates(dateFrom, dateTo);
                paymentsList.Sort((x, y) => x.Date.CompareTo(y.Date));
                foreach (var item in paymentsList)
                {
                    float Credit = (float)item.Credit;
                    float Debit = (float)item.Debit;
                    Models.DummyModels.CustomerSupplierPaymentModel model = new Models.DummyModels.CustomerSupplierPaymentModel
                    {
                        Id = item.Id,
                        CustomerName = item.CustomerName,
                        CustomerID = item.CustomerID,
                        SupplierID = item.SupplierID,
                        Supplier = item.Suppliers.Name,

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
                CashOutListView.ItemsSource = dummyPayments;
                CashOutListView.Items.Refresh();
                float TotalCashOut = (float)paymentsList.Sum(s => s.Debit);
                float TotalCashIn = (float)paymentsList.Sum(s => s.Credit);
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
                int TotalCashIn = 0;
                int v1 = 0;
                foreach (var cashout in paymentsList)
                {
                    v1 = Convert.ToInt32(cashout.Debit);
                    TotalCashIn = v1 + TotalCashIn;
                    CashInOutModel model = new CashInOutModel()
                    {
                        Date = cashout.Date,
                        description = cashout.Description,
                        cash = cashout.Credit

                    };
                    cashoutmodel.Add(model);
                }
                int id = Convert.ToInt32(SupplierCashOutCombo.SelectedValue.ToString());
                var supplier = helper.GetSupplier(id);
                CashOutReportView cashOutViewReport = new CashOutReportView(cashoutmodel);
                cashOutViewReport.ShowDialog();
            }
            catch (Exception )
            {

                InfoBox infobox = new InfoBox("Something went wrong,there are no data to generate report.");
                infobox.ShowDialog();
            }
        }

        private void btnPrintSupplier_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string balance = lblTotalRunningBalance.Content.ToString();
                List<Reporting.CustomersRunningBalance> balanceList = new List<Reporting.CustomersRunningBalance>();
                foreach (var item in RunningBalancesList) 
                {
                    Reporting.CustomersRunningBalance model = new Reporting.CustomersRunningBalance()
                    {
                        Name = item.Name,
                        Contact = item.Contact,
                        Address = item.Address,
                        Balance = item.RunningBalance.ToString(),
                        Date = item.Date
                    };
                    balanceList.Add(model);
                }
                PrintSupplierRunningBalance printscreen = new PrintSupplierRunningBalance(balanceList, balance);
                printscreen.ShowDialog();
            }
            catch (Exception)
            {
              
                InfoBox infobox = new InfoBox("Something went wrong,there are no data to generate report.");
                infobox.ShowDialog();
            }
        }

        private void btnNewCashIn_Click(object sender, RoutedEventArgs e)
        {
            New_CashINAdvance new_CashINAdvance = new New_CashINAdvance();
            new_CashINAdvance.ShowDialog();
        }

        private void SearchSpecificDateBetween_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                paymentsList.Clear();
                dummyPayments.Clear();
                DateTime dateFrom = DateFrom.SelectedDate.Value.Date;
                DateTime dateTo = DateTo.SelectedDate.Value.Date;
                var supplier = SupplierCashOutCombo.SelectedItem as Entities.Suppliers;
                paymentsList = helper.SpecificCashInBetweenDates(supplier.Id, dateFrom, dateTo);
                paymentsList.Sort((x, y) => x.Date.CompareTo(y.Date));
                foreach (var item in paymentsList)
                {
                    float Credit = (float)item.Credit;
                    float Debit = (float)item.Debit;
                    Models.DummyModels.CustomerSupplierPaymentModel model = new Models.DummyModels.CustomerSupplierPaymentModel
                    {
                        Id = item.Id,
                        CustomerName = item.CustomerName,
                        CustomerID = item.CustomerID,
                        SupplierID = item.SupplierID,
                        Supplier = item.Suppliers.Name,

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
                CashOutListView.ItemsSource = dummyPayments;
                CashOutListView.Items.Refresh();
                float TotalCashOut = (float)paymentsList.Sum(s => s.Debit);
                float TotalCashIn = (float)paymentsList.Sum(s => s.Credit);
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
                var model = CashOutListView.SelectedItem as Models.DummyModels.CustomerSupplierPaymentModel;
                var payments = paymentsDatabaseHelper.GetPayment(model.Id);
                if (payments.Credit != 0)
                {
                    New_CashINAdvance new_CashINAdvance = new New_CashINAdvance(payments,false);
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
                Models.PaymentsDatabaseHelper helper = new PaymentsDatabaseHelper();
                paymentsList.Clear();
                dummyPayments.Clear();
                int Pay_ID = Convert.ToInt32(txtPaymentID.Text);
                paymentsList = helper.GetSupplierSinglePayment(Pay_ID).ToList();

                paymentsList.Sort((x, y) => x.Date.CompareTo(y.Date));
                foreach (var item in paymentsList)
                {
                    float Credit = (float)item.Credit;
                    float Debit = (float)item.Debit;
                    Models.DummyModels.CustomerSupplierPaymentModel model = new Models.DummyModels.CustomerSupplierPaymentModel
                    {
                        Id = item.Id,
                        CustomerName = item.CustomerName,
                        CustomerID = item.CustomerID,
                        SupplierID = item.SupplierID,
                        Supplier = item.Suppliers.Name,

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
                CashOutListView.ItemsSource = dummyPayments;
                CashOutListView.Items.Refresh();
                float TotalCashOut = (float)paymentsList.Sum(s => s.Debit);
                float TotalCashIn = (float)paymentsList.Sum(s => s.Credit);
                LblTotalCashOut.Content = TotalCashOut.ToString("#,##0.00", usFormatProvider);
                LblTotalCashIn.Content = TotalCashIn.ToString("#,##0.00", usFormatProvider);
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no Cash-Outs to show.");
                infobox.ShowDialog();
            }
        }

        private void btnSearchbyDesc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.PaymentsDatabaseHelper helper = new PaymentsDatabaseHelper();
                paymentsList.Clear();
                dummyPayments.Clear();
                
                paymentsList = helper.GetDescribedPayments(txtSearchDesc.Text,"Supplier").ToList();

                paymentsList.Sort((x, y) => x.Date.CompareTo(y.Date));
                foreach (var item in paymentsList)
                {
                    float Credit = (float)item.Credit;
                    float Debit = (float)item.Debit;
                    Models.DummyModels.CustomerSupplierPaymentModel model = new Models.DummyModels.CustomerSupplierPaymentModel
                    {
                        Id = item.Id,
                        CustomerName = item.CustomerName,
                        CustomerID = item.CustomerID,
                        SupplierID = item.SupplierID,
                        Supplier = item.Suppliers.Name,

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
                CashOutListView.ItemsSource = dummyPayments;
                CashOutListView.Items.Refresh();
                float TotalCashOut = (float)paymentsList.Sum(s => s.Debit);
                float TotalCashIn = (float)paymentsList.Sum(s => s.Credit);
                LblTotalCashOut.Content = TotalCashOut.ToString("#,##0.00", usFormatProvider);
                LblTotalCashIn.Content = TotalCashIn.ToString("#,##0.00", usFormatProvider);
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no Cash-Outs to show.");
                infobox.ShowDialog();
            }
        }
    }
}
