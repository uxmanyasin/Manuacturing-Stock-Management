using BizBook.Entities;
using BizBook.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;


namespace BizBook
{
    
    public partial class Reports : UserControl
    {
        private DatabaseContext db = new DatabaseContext();
        private readonly CustomerDatabaseHelper customerDatabaseHelper = new CustomerDatabaseHelper();
        private readonly SupplierDatabaseHelper supplierDatabaseHelper = new SupplierDatabaseHelper();
        private readonly PaymentsDatabaseHelper paymentsDatabaseHelper = new PaymentsDatabaseHelper();
        private readonly PurchaseDatabaseHelper purchaseDatabaseHelper = new PurchaseDatabaseHelper();
        private readonly CustomerClaimsDatabaseHelper customerClaimsDatabaseHelper = new CustomerClaimsDatabaseHelper();
        private readonly SupplierClaimsDatabaseHelper supplierClaimsDatabaseHelper = new SupplierClaimsDatabaseHelper();
       
        private readonly SaleDatabaseHelper saleDatabaseHelper = new SaleDatabaseHelper();
        public List<Suppliers> suppliersList = new List<Suppliers>();
        public List<Customers> customerList = new List<Customers>();
        private List<SupplierLedgerViewModel> supplierledgerlist = new List<SupplierLedgerViewModel>();
        private List<SupplierLedgerViewModel> customerledgerlist = new List<SupplierLedgerViewModel>();
        public ObservableCollection<SupplierLedgerViewModel> _customerledger = new ObservableCollection<SupplierLedgerViewModel>();
        public ObservableCollection<SupplierLedgerViewModel> _supplierledger = new ObservableCollection<SupplierLedgerViewModel>();
        public ObservableCollection<CustomerLedgerModel> _customerLedgersModels = new ObservableCollection<CustomerLedgerModel>();
        public ObservableCollection<SupplierLedgerModel> _supplierLedgersModels = new ObservableCollection<SupplierLedgerModel>();
        public ObservableCollection<Models.DummyModels.Ledger_Customer_Supplier> ledger_Customer_Suppliers = new ObservableCollection<Models.DummyModels.Ledger_Customer_Supplier>();
        IFormatProvider usFormatProvider = new System.Globalization.CultureInfo("en-US");

        public Reports()
        {
            InitializeComponent();
            suppliersList = supplierDatabaseHelper.GetSuppliers().ToList();
            SupplierLedgerCombo.ItemsSource = suppliersList;
            customerList = customerDatabaseHelper.GetCustomers().ToList();
            CustomerLedgerCombo.ItemsSource = customerList;
        }

        #region SupplierLedger
        private void BtnCustomerLedgerSearch_Click(object sender, RoutedEventArgs e)
        {
           
        }
        #endregion


        #region CustomerLedger
        private void BtnCustomerSearch_Click(object sender, RoutedEventArgs e)
        {

           
        } 
        #endregion

        private void GetCustomerLedger() 
        {
            try
            {
                ledger_Customer_Suppliers.Clear();
                lblCustomerPreviousBalance.Content = "0";
                _customerLedgersModels.Clear();
                int id = Convert.ToInt32(CustomerLedgerCombo.SelectedValue.ToString());
                Models.SaleDatabaseHelper saleDatabaseHelper = new SaleDatabaseHelper();
                var listOfSale = saleDatabaseHelper.GetCustomerSales(id);
                listOfSale.Sort((x, y) => x.SaleDate.CompareTo(y.SaleDate));
                Models.PaymentsDatabaseHelper paymentsDatabaseHelper = new PaymentsDatabaseHelper();
                var ListOfPayments =  paymentsDatabaseHelper.GetCustomerPayments(id);
                ListOfPayments.Sort((x, y) => x.Date.CompareTo(y.Date));

                var customer = customerDatabaseHelper.GetCustomer(id);
                float balance = 0;
              
                float totalDebit = 0;
                float totalBalance = 0;
                float totalcredit = 0;

                foreach (var item in ListOfPayments)
                {

                    totalDebit += (float)item.Debit;
                    totalcredit += (float)item.Credit;

                    if (item.SaleId != null)
                    {
                        List<Entities.Sale_Details> _sale_Details = new List<Sale_Details>();
                        
                        _sale_Details = saleDatabaseHelper.GetSaleDetails(item.SaleId);
                        foreach (var items in _sale_Details)
                        {
                            totalBalance = items.Total + totalBalance;
                            string NetBalance = "";
                            if (totalBalance > 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Dr"; }
                            else if (totalBalance < 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Cr"; }
                            else { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider); }
                            _customerLedgersModels.Add(new CustomerLedgerModel()
                            {
                                ID = 00,
                                Credit = 0,
                                Description =  items.product.Name + " - (" + items.sale.GatePass + ")",
                                Date = items.SaleDate,
                                Quantity = items.Quantity,
                                Rate = items.Rate,
                                Debit = items.Total,
                                BillNo = items.SaleId,
                                Balance = NetBalance

                            });
                            //   totalBalance = balance;
                        }
                    }
                    else if (item.CustomerClaimsID != null && item.SaleId == null)
                    {
                        List<Entities.CustomerClaimDetails> customerClaimDetailsList = new List<CustomerClaimDetails>();
                        Models.CustomerClaimsDatabaseHelper customerClaimsDatabaseHelper = new CustomerClaimsDatabaseHelper();

                        customerClaimDetailsList = customerClaimsDatabaseHelper.GetClaimDetails(item.CustomerClaimsID);
                        foreach (var items in customerClaimDetailsList)
                        {
                            totalBalance = totalBalance - items.Total;
                            string NetBalance = "";
                            if (totalBalance > 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Dr"; }
                            else if (totalBalance < 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Cr"; }
                            else { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider); }
                            _customerLedgersModels.Add(new CustomerLedgerModel()
                            {
                                ID = 00,
                                Debit = 0,
                                Description =  items.product.Name + "( Return)" + "- (" + items.CustomerClaims.GatePass + ")",
                                Date = item.Date,
                                Quantity = items.Quantity,
                                Rate = items.Rate,
                                Credit = items.Total,
                                BillNo = "None",
                                Balance = NetBalance

                            });

                            // totalBalance = balance;
                        }
                    }

                    else if (item.SaleId == null && item.CustomerClaimsID == null)
                    {
                        if (item.Credit != 0) { totalBalance = (float)(totalBalance - item.Credit); }
                        else if (item.Debit != 0) { totalBalance = (float)(totalBalance + item.Debit); }
                        string NetBalance = "";
                        if (totalBalance > 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Dr"; }
                        else if (totalBalance < 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Cr"; }
                        else { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider); }
                        _customerLedgersModels.Add(new CustomerLedgerModel()
                        {
                            ID = item.Id,
                            Date = item.Date,
                            Description =  item.Description + " - ( " + item.Id + ")",
                            Quantity = 0,
                            Rate = 0,
                            Debit = (float)item.Debit,
                            Credit = (float)item.Credit,
                            Balance = NetBalance,
                            BillNo = "None"

                        });

                        // totalBalance = balance;

                    }

                    //   openbalance = 0;
                }
                foreach (var item in _customerLedgersModels) 
                {
                    Models.DummyModels.Ledger_Customer_Supplier model = new Models.DummyModels.Ledger_Customer_Supplier
                    {
                        ID = item.ID,
                        Date = item.Date,
                        Description = item.Description,
                        Quantity = item.Quantity,
                        Rate = item.Rate.ToString("#,##0.00", usFormatProvider),
                        Debit = item.Debit.ToString("#,##0.00", usFormatProvider),
                        Credit = item.Credit.ToString("#,##0.00", usFormatProvider),
                        Balance = item.Balance,
                        BillNo = item.BillNo,
                    };
                    ledger_Customer_Suppliers.Add(model);
                }
                CustomerLedgerListView.ItemsSource = ledger_Customer_Suppliers;
                CustomerLedgerListView.Items.Refresh();
               
                if (totalBalance < 0)
                {
                    lblTotalBalances.Content = totalBalance.ToString("#,##0.00", usFormatProvider) + " Cr";
                }
                else if (totalBalance > 0)
                {
                    lblTotalBalances.Content = totalBalance.ToString("#,##0.00", usFormatProvider) + " Dr";
                }
                else
                {
                    lblTotalBalances.Content = totalBalance.ToString("#,##0.00", usFormatProvider);
                }
                float TotalDebit = _customerLedgersModels.Sum(s => s.Debit);
                float TotalCredit = _customerLedgersModels.Sum(s => s.Credit);
                lblTotalSale.Content = TotalDebit.ToString("#,##0.00", usFormatProvider);
                lblTotalCashIn.Content = TotalCredit.ToString("#,##0.00", usFormatProvider);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                InfoBox infobox = new InfoBox("Something went wrong,there is no data to generate supplier ledger.");
                infobox.ShowDialog();
            }
        } 

        private void GetDateBetweenCustomerLedger()
        {
            try
            {
                ledger_Customer_Suppliers.Clear();
                float PreviousBalance = GetPreviousCustomerOpenBalance();
                DateTime date1 = CustomerFromDatePicker.SelectedDate.Value.Date;
                DateTime date2 = CustomerToDatePicker.SelectedDate.Value.Date;
                _customerLedgersModels.Clear();
                int id = Convert.ToInt32(CustomerLedgerCombo.SelectedValue.ToString());
                var listOfSale = saleDatabaseHelper.GetDateBetweenCustomerSales(id, date1, date2);
                listOfSale.Sort((x, y) => x.SaleDate.CompareTo(y.SaleDate));


                var ListOfPayments = paymentsDatabaseHelper.GetDateBetweenCustomerPayments(id, date1, date2);
                ListOfPayments.Sort((x, y) => x.Date.CompareTo(y.Date));


                var customer = customerDatabaseHelper.GetCustomer(id);
                float balance = 0;
                //float openbalance = Convert.ToInt32(customer.openbalance);
                //float openbalance1 = openbalance;
                float totalDebit = 0;
                float totalBalance = PreviousBalance;
                float totalcredit = 0;
                if (PreviousBalance > 0)
                {
                    _customerLedgersModels.Add(new CustomerLedgerModel()
                    {
                        ID = 00,
                        Date = date1,
                        Description = "Previous Balance",
                        Quantity = 0,
                        Rate = 0,
                        Debit = PreviousBalance,
                        Credit = 0,
                        Balance = PreviousBalance.ToString("#,##0.00", usFormatProvider) + " Dr",
                        BillNo = "None"

                    });
                }
                else
                {
                    _customerLedgersModels.Add(new CustomerLedgerModel()
                    {
                        ID = 00,
                        Date = date1,
                        Description = "Previous Balance",
                        Quantity = 0,
                        Rate = 0,
                        Debit = 0,
                        Credit = PreviousBalance,
                        Balance = PreviousBalance.ToString("#,##0.00", usFormatProvider) + " Cr",
                        BillNo = "None"
                    });
                }
                foreach (var item in ListOfPayments)
                {
                    totalDebit += (float)item.Debit;
                    totalcredit += (float)item.Credit;
                    if (item.SaleId != null)
                    {
                        List<Entities.Sale_Details> _sale_Details = new List<Sale_Details>();
                        _sale_Details = saleDatabaseHelper.GetSaleDetails(item.SaleId);
                        foreach (var items in _sale_Details)
                        {
                            totalBalance = items.Total + totalBalance;
                            string NetBalance = "";
                            if (totalBalance > 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Dr"; }
                            else if (totalBalance < 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Cr"; }
                            else { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider); }
                            _customerLedgersModels.Add(new CustomerLedgerModel()
                            {
                                ID = 00,
                                Credit = 0,
                                Description =  items.product.Name + " - (" + items.sale.GatePass + ")",
                                Date = items.SaleDate,
                                Quantity = items.Quantity,
                                Rate = items.Rate,
                                Debit = items.Total,
                                BillNo = items.SaleId,
                                Balance = NetBalance
                            });
                        }
                    }
                    else if (item.CustomerClaimsID != null && item.SaleId == null)
                    {
                        List<Entities.CustomerClaimDetails> customerClaimDetailsList = new List<CustomerClaimDetails>();
                        customerClaimDetailsList = customerClaimsDatabaseHelper.GetClaimDetails(item.CustomerClaimsID);
                        foreach (var items in customerClaimDetailsList)
                        {
                            totalBalance = totalBalance - items.Total;
                            string NetBalance = "";
                            if (totalBalance > 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Dr"; }
                            else if (totalBalance < 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Cr"; }
                            else { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider); }
                            _customerLedgersModels.Add(new CustomerLedgerModel()
                            {
                                ID = 00,
                                Debit = 0,
                                Description =  items.product.Name + "( Return)" + " - (" + items.CustomerClaims.GatePass + ")",
                                Date = item.Date,
                                Quantity = items.Quantity,
                                Rate = items.Rate,
                                Credit = items.Total,
                                BillNo = "None",
                                Balance = NetBalance

                            });
                        }
                    }

                    else if (item.SaleId == null && item.CustomerClaimsID == null)
                    {
                        if (item.Credit != 0) { totalBalance = (float)(totalBalance - item.Credit); }
                        else if (item.Debit != 0) { totalBalance = (float)(totalBalance + item.Debit); }
                        string NetBalance = "";
                        if (totalBalance > 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Dr"; }
                        else if (totalBalance < 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Cr"; }
                        else { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider); }
                        _customerLedgersModels.Add(new CustomerLedgerModel()
                        {
                            ID = item.Id,
                            Date = item.Date,
                            Description =  item.Description + " - (" + item.Id + ")",
                            Quantity = 0,
                            Rate = 0,
                            Debit = (float)item.Debit,
                            Credit = (float)item.Credit,
                            Balance = NetBalance,
                            BillNo = "None"
                        });
                    }
                    //openbalance = 0;
                }
                foreach (var item in _customerLedgersModels)
                {
                    Models.DummyModels.Ledger_Customer_Supplier model = new Models.DummyModels.Ledger_Customer_Supplier
                    {
                        ID = item.ID,
                        Date = item.Date,
                        Description = item.Description,
                        Quantity = item.Quantity,
                        Rate = item.Rate.ToString("#,##0.00", usFormatProvider),
                        Debit = item.Debit.ToString("#,##0.00", usFormatProvider),
                        Credit = item.Credit.ToString("#,##0.00", usFormatProvider),
                        Balance = item.Balance,
                        BillNo = item.BillNo,
                    };
                    ledger_Customer_Suppliers.Add(model);
                }
                CustomerLedgerListView.ItemsSource = ledger_Customer_Suppliers;
                CustomerLedgerListView.Items.Refresh();
                float TotalDebit = _customerLedgersModels.Sum(s => s.Debit);
                float TotalCredit = _customerLedgersModels.Sum(s => s.Credit);
                lblTotalSale.Content = TotalDebit.ToString("#,##0.00", usFormatProvider);
                lblTotalBalances.Content = totalBalance.ToString("#,##0.00", usFormatProvider);
                lblTotalCashIn.Content = TotalCredit.ToString("#,##0.00", usFormatProvider);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                InfoBox infobox = new InfoBox("Something went wrong,there is no data to generate supplier ledger.");
                infobox.ShowDialog();
            }   
        }

        private float GetPreviousCustomerOpenBalance()
        {
            float PreviousBalance = 0;
            try
            {
                DateTime date1 = CustomerFromDatePicker.SelectedDate.Value.Date;
                DateTime date2 = CustomerToDatePicker.SelectedDate.Value.Date;
                var customer = CustomerLedgerCombo.SelectedItem as Entities.Customers;
                var listofPaymnets = paymentsDatabaseHelper.GetNOTDateBetweenCustomerPayments(customer.Id, date1, date2);
                listofPaymnets.Sort((x, y) => x.Date.CompareTo(y.Date));
                float? TotalDebit = listofPaymnets.Sum(s => s.Debit);
                float? TotalCredit = listofPaymnets.Sum(s => s.Credit);
                PreviousBalance = (float)(TotalDebit - TotalCredit);
                lblCustomerPreviousBalance.Content = PreviousBalance.ToString();
                return PreviousBalance;
            }
            catch (Exception)
            {
                throw;
            }   
        }
        private float GetPreviousSupplierOpenBalance()
        {
            float? PreviousBalance = 0;
            try
            {
                DateTime date1 = SupplierFromDatePicker.SelectedDate.Value.Date;
                DateTime date2 = SupplierToDatePicker.SelectedDate.Value.Date;
                var supplier = SupplierLedgerCombo.SelectedItem as Entities.Suppliers;
                var listofPaymnets = paymentsDatabaseHelper.GetDateNOTBetweenSupplierPayments(supplier.Id, date1, date2);
                listofPaymnets.Sort((x, y) => x.Date.CompareTo(y.Date));
                float? TotalDebit = listofPaymnets.Sum(s => s.Debit);
                float? TotalCredit = listofPaymnets.Sum(s => s.Credit);
                PreviousBalance = TotalCredit - TotalDebit;
                lblSuppierPreviousBalance.Content = PreviousBalance.ToString();
                return (float)PreviousBalance;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
            return (float)PreviousBalance;
        }
        private void GetSupplierLedger()
        {
            try
            {
                ledger_Customer_Suppliers.Clear();
                lblSuppierPreviousBalance.Content = "0";
                _supplierLedgersModels.Clear();
                int id = Convert.ToInt32(SupplierLedgerCombo.SelectedValue.ToString());
                Models.PurchaseDatabaseHelper purchaseDatabaseHelper = new PurchaseDatabaseHelper();
                var listOfPurchase = purchaseDatabaseHelper.GetSupplierPurchase(id);
                listOfPurchase.Sort((x, y) => x.PurchaseDate.CompareTo(y.PurchaseDate));

                float totalPurchase = listOfPurchase.Sum(s => s.TotalPrice);
                Models.PaymentsDatabaseHelper paymentsDatabaseHelper = new PaymentsDatabaseHelper();
                var ListOfPayments = paymentsDatabaseHelper.GetSupplierPayments(id);
                ListOfPayments.Sort((x, y) => x.Date.CompareTo(y.Date));

                var supplier = supplierDatabaseHelper.GetSupplier(id);
                //float openbalance = supplier.openbalance;
                //float openbalace1 = openbalance;
                float balance = 0;
                float totalDebit = 0;
                float totalBalance = 0;
                float totalCredit = 0;

                foreach (var item in ListOfPayments)
                {
                    totalDebit += (float)item.Credit;
                    totalCredit += (float)item.Debit;

                    if (item.PurchaseId != null)
                    {
                        List<Entities.Purchase_Details> _Details = new List<Entities.Purchase_Details>();
                        _Details = purchaseDatabaseHelper.GetPurchaseDetails(item.PurchaseId);
                        foreach (var items in _Details)
                        {
                            totalBalance = items.Total + totalBalance;
                            string NetBalance = "";
                            if (totalBalance > 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Cr"; }
                            else if (totalBalance < 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Dr"; }
                            else { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider); }
                            _supplierLedgersModels.Add(new SupplierLedgerModel()
                            {
                                ID = 00,
                                Credit = items.Total,
                                Description =  items.rawmaterial.Name + " - (" + items.purchase.GatePass + ")",
                                Date = item.Date,
                                Quantity = items.Quantity,
                                Rate = items.Rate,
                                Debit = 0,
                                BillNo = item.PurchaseId,
                                Balance = NetBalance

                            });

                        }
                    }
                    else if (item.SupplierClaimsID != null)
                    {
                        List<Entities.SupplierClaimDetails> _Details = new List<Entities.SupplierClaimDetails>();
                        Models.SupplierClaimsDatabaseHelper supplierClaimsDatabaseHelper = new SupplierClaimsDatabaseHelper();
                        _Details = supplierClaimsDatabaseHelper.GetClaimDetails(item.SupplierClaimsID);
                        foreach (var items in _Details)
                        {
                            totalBalance = totalBalance - items.Total;
                            string NetBalance = "";
                            if (totalBalance > 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Cr"; }
                            else if (totalBalance < 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Dr"; }
                            else { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider); }
                            _supplierLedgersModels.Add(new SupplierLedgerModel()
                            {
                                ID = 00,
                                Credit = 0,
                                Description =  items.RawMaterial.Name + " (Return)"  + " - (" + items.SupplierClaims.GatePass + ")",
                                Date = item.Date,
                                Quantity = items.Quantity,
                                Rate = items.Rate,
                                Debit = items.Total,
                                BillNo = "None",
                                Balance = NetBalance
                            });
                        }
                    }
                    else if (item.PurchaseId == null && item.SupplierClaimsID == null)
                    {
                        if (item.Debit != 0) { totalBalance = totalBalance - (float)(item.Debit); }
                        else if (item.Credit != 0) { totalBalance = totalBalance + (float)(item.Credit); }
                        string NetBalance = "";
                        if (totalBalance > 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Cr"; }
                        else if (totalBalance < 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Dr"; }
                        else { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider); }
                        _supplierLedgersModels.Add(new SupplierLedgerModel()
                        {
                            ID = item.Id,
                            Date = item.Date,
                            Description =  item.Description  + " - (" + item.Id + ")",
                            Quantity = 0,
                            Rate = 0,
                            Debit = (float)item.Debit,
                            Credit = (float)item.Credit,
                            Balance = NetBalance,
                            BillNo = "None"
                        });

                    }
                    //openbalance = 0; 
                }
                foreach (var item in _supplierLedgersModels)
                {
                    Models.DummyModels.Ledger_Customer_Supplier model = new Models.DummyModels.Ledger_Customer_Supplier
                    {
                        ID = item.ID,
                        Date = item.Date,
                        Description = item.Description,
                        Quantity = item.Quantity,
                        Rate = item.Rate.ToString("#,##0.00", usFormatProvider),
                        Debit = item.Debit.ToString("#,##0.00", usFormatProvider),
                        Credit = item.Credit.ToString("#,##0.00", usFormatProvider),
                        Balance = item.Balance,
                        BillNo = item.BillNo,
                    };
                    ledger_Customer_Suppliers.Add(model);
                }
                SupplierLedgerListView.ItemsSource = ledger_Customer_Suppliers;
                SupplierLedgerListView.Items.Refresh();
                float TotalPurchases = _supplierLedgersModels.Sum(s => s.Credit);
                lblTotalPurchase.Content = TotalPurchases.ToString("#,##0.00", usFormatProvider);
                if (totalBalance < 0)
                {
                    lblTotalBalance.Content = totalBalance.ToString("#,##0.00", usFormatProvider) + " Dr";
                }
                else if (totalBalance > 0)
                {
                    lblTotalBalance.Content = totalBalance.ToString("#,##0.00", usFormatProvider) + " Cr";
                }
                else
                {
                    lblTotalBalance.Content = totalBalance.ToString("#,##0.00", usFormatProvider);
                }
                float TotalCashOut = _supplierLedgersModels.Sum(s => s.Debit);
                lblTotalCashOut.Content = TotalCashOut.ToString("#,##0.00", usFormatProvider);

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there is no data to generate supplier ledger.");
                infobox.ShowDialog();
            }
        } 
        private void GetDateBetweenSupplierLedger()
        {
            try
            {
                ledger_Customer_Suppliers.Clear();
                float PreviousBalance = GetPreviousSupplierOpenBalance();
                DateTime date1 = SupplierFromDatePicker.SelectedDate.Value.Date;
                DateTime date2 = SupplierToDatePicker.SelectedDate.Value.Date;
                _supplierLedgersModels.Clear();
                int id = Convert.ToInt32(SupplierLedgerCombo.SelectedValue.ToString());
                var listOfPurchase = purchaseDatabaseHelper.GetDateBetweenSupplierPurchase(id, date1, date2);
                listOfPurchase.Sort((x, y) => x.PurchaseDate.CompareTo(y.PurchaseDate));

                float totalPurchase = listOfPurchase.Sum(s => s.TotalPrice);
                var ListOfPayments = paymentsDatabaseHelper.GetDateBetweenSupplierPayments(id, date1, date2);
                ListOfPayments.Sort((x, y) => x.Date.CompareTo(y.Date));

                var supplier = supplierDatabaseHelper.GetSupplier(id);
                //float openbalance = supplier.openbalance;
                //float openbalace1 = openbalance;
                float balance = 0;
                float totalDebit = 0;
                float totalBalance = PreviousBalance;
                float totalCredit = 0;
                if (PreviousBalance > 0)
                {
                    _supplierLedgersModels.Add(new SupplierLedgerModel()
                    {

                        ID = 00,
                        Date = date1,
                        Description = "Previous Balance",
                        Quantity = 0,
                        Rate = 0,
                        Debit = 0,
                        Credit = PreviousBalance,
                        Balance = PreviousBalance.ToString("#,##0.00", usFormatProvider) + " Cr",
                        BillNo = "None"
                    });
                }
                else
                {
                    _supplierLedgersModels.Add(new SupplierLedgerModel()
                    {
                        ID = 00,
                        Date = date1,
                        Description = "Previous Balance",
                        Quantity = 0,
                        Rate = 0,
                        Debit = PreviousBalance,
                        Credit = 0,
                        Balance = PreviousBalance.ToString("#,##0.00", usFormatProvider) + " Dr",
                        BillNo = "None"
                    });
                }
                foreach (var item in ListOfPayments)
                {
                    totalDebit += (float)item.Credit;
                    totalCredit += (float)item.Debit;

                    if (item.PurchaseId != null)
                    {
                        List<Entities.Purchase_Details> _Details = new List<Entities.Purchase_Details>();
                        _Details = purchaseDatabaseHelper.GetPurchaseDetails(item.PurchaseId);
                        foreach (var items in _Details)
                        {
                            totalBalance = items.Total + totalBalance;
                            string NetBalance = "";
                            if (totalBalance > 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Cr"; }
                            else if (totalBalance < 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Dr"; }
                            else { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider); }
                            _supplierLedgersModels.Add(new SupplierLedgerModel()
                            {
                                ID = 00,
                                Credit = items.Total,
                                Description =  items.rawmaterial.Name + " - (" + items.purchase.GatePass + ")",
                                Date = item.Date,
                                Quantity = items.Quantity,
                                Rate = items.Rate,
                                Debit = 0,
                                BillNo = item.PurchaseId,
                                Balance = NetBalance
                            });
                        }
                    }
                    else if (item.SupplierClaimsID != null)
                    {
                        List<Entities.SupplierClaimDetails> _Details = new List<Entities.SupplierClaimDetails>();
                        _Details = supplierClaimsDatabaseHelper.GetClaimDetails(item.SupplierClaimsID);
                        foreach (var items in _Details)
                        {
                            totalBalance = totalBalance - items.Total;
                            string NetBalance = "";
                            if (totalBalance > 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Cr"; }
                            else if (totalBalance < 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Dr"; }
                            else { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider); }
                            _supplierLedgersModels.Add(new SupplierLedgerModel()
                            {
                                ID = 00,
                                Credit = 0,
                                Description =  items.RawMaterial.Name + " (Return)" + " - ( " + items.SupplierClaims.GatePass + ")",
                                Date = item.Date,
                                Quantity = items.Quantity,
                                Rate = items.Rate,
                                Debit = items.Total,
                                BillNo = "None",
                                Balance = NetBalance


                            });

                        }
                    }
                    else if (item.PurchaseId == null && item.SupplierClaimsID == null)
                    {
                        if (item.Debit != 0) { totalBalance = totalBalance - (float)(item.Debit); }
                        else if (item.Credit != 0) { totalBalance = totalBalance + (float)(item.Credit); }
                        string NetBalance = "";
                        if (totalBalance > 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Cr"; }
                        else if (totalBalance < 0) { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider) + " Dr"; }
                        else { NetBalance = totalBalance.ToString("#,##0.00", usFormatProvider); }
                        _supplierLedgersModels.Add(new SupplierLedgerModel()
                        {
                            ID = item.Id,
                            Date = item.Date,
                            Description =  item.Description + " - (" + item.Id +")" ,
                            Quantity = 0,
                            Rate = 0,
                            Debit = (float)item.Debit,
                            Credit = (float)item.Credit,
                            Balance = NetBalance,
                            BillNo = "None"
                        });

                    }
                }
                foreach (var item in _supplierLedgersModels)
                {
                    Models.DummyModels.Ledger_Customer_Supplier model = new Models.DummyModels.Ledger_Customer_Supplier
                    {
                        ID = item.ID,
                        Date = item.Date,
                        Description = item.Description,
                        Quantity = item.Quantity,
                        Rate = item.Rate.ToString("#,##0.00", usFormatProvider),
                        Debit = item.Debit.ToString("#,##0.00", usFormatProvider),
                        Credit = item.Credit.ToString("#,##0.00", usFormatProvider),
                        Balance = item.Balance,
                        BillNo = item.BillNo,
                    };
                    ledger_Customer_Suppliers.Add(model);
                }
                SupplierLedgerListView.ItemsSource = ledger_Customer_Suppliers;
                SupplierLedgerListView.Items.Refresh();
                float TotalCredit = _supplierLedgersModels.Sum(s => s.Credit);
                float TotalDebit = _supplierLedgersModels.Sum(s => s.Debit);
                lblTotalPurchase.Content = TotalCredit.ToString("#,##0.00", usFormatProvider);
                lblTotalBalance.Content = totalBalance.ToString("#,##0.00", usFormatProvider);
                lblTotalCashOut.Content = TotalDebit.ToString("#,##0.00", usFormatProvider);

            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there is no data to generate supplier ledger.");
                infobox.ShowDialog();
            }
        }

        private void BtnCustomerPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string TotalPurchase = lblTotalSale.Content.ToString();
                string TotalCashIn = lblTotalCashIn.Content.ToString();
                string TotalBalance = lblTotalBalances.Content.ToString();
                string StartDate = "00-00-0000";
                if (CustomerFromDatePicker.SelectedDate.HasValue == true)
                {
                    StartDate = CustomerFromDatePicker.SelectedDate.Value.Date.ToString();
                }
                string EndDate = "00-00-0000";
                if (CustomerToDatePicker.SelectedDate.HasValue == true)
                {
                    EndDate = CustomerToDatePicker.SelectedDate.Value.Date.ToString();
                }

                string PreviousBalance = lblCustomerPreviousBalance.Content.ToString();
                int id = Convert.ToInt32(CustomerLedgerCombo.SelectedValue.ToString());
                var customer = customerDatabaseHelper.GetCustomer(id);
                ObservableCollection<Reporting.CustomerLedgerModel> customerLedger = new ObservableCollection<Reporting.CustomerLedgerModel>();
                foreach (var item in _customerLedgersModels) 
                {
                    Reporting.CustomerLedgerModel model = new Reporting.CustomerLedgerModel 
                    {
                        Date = item.Date,
                        Description = item.Description,
                        Quantity = item.Quantity.ToString(),
                        Rate = item.Rate.ToString("#,##0.00", usFormatProvider),
                        Debit = item.Debit.ToString("#,##0.00", usFormatProvider),
                        Credit = item.Credit.ToString("#,##0.00", usFormatProvider),
                        Balance = item.Balance.ToString(),
                        BillNo = item.BillNo
                    };
                    customerLedger.Add(model);
                }
                CustomerLedgerReport customerLedgerReport = new CustomerLedgerReport(customer, customerLedger, TotalPurchase, TotalCashIn, TotalBalance, PreviousBalance ,StartDate , EndDate);
                customerLedgerReport.ShowDialog();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no data to generate report.");
                infobox.ShowDialog();
            }
        }

        private void BtnSupplierPrint_Click(object sender, RoutedEventArgs e)
        {
            try {

                string TotalPurchase = lblTotalPurchase.Content.ToString();
                string TotalCashIn = lblTotalCashOut.Content.ToString();
                string TotalBalance = lblTotalBalance.Content.ToString();
                string StartDate = "00-00-0000";
                if (SupplierFromDatePicker.SelectedDate.HasValue == true) 
                {
                    StartDate = SupplierFromDatePicker.SelectedDate.Value.Date.ToString();
                }
                string EndDate = "00-00-0000";
                if (SupplierToDatePicker.SelectedDate.HasValue == true)
                {
                    EndDate = SupplierToDatePicker.SelectedDate.Value.Date.ToString();
                }
                string PreviousBalance = lblSuppierPreviousBalance.Content.ToString();
                int id = Convert.ToInt32(SupplierLedgerCombo.SelectedValue.ToString());
                var supplier = supplierDatabaseHelper.GetSupplier(id);
                ObservableCollection<Reporting.SupplierLedgerModel> _ListLedger = new ObservableCollection<Reporting.SupplierLedgerModel>();
                foreach (var item in _supplierLedgersModels) 
                {
                    Reporting.SupplierLedgerModel model = new Reporting.SupplierLedgerModel 
                    {
                        Date = item.Date,
                        Description = item.Description,
                        Quantity = item.Quantity.ToString(),
                        Rate = item.Rate.ToString("#,##0.00", usFormatProvider),
                        Debit = item.Debit.ToString("#,##0.00", usFormatProvider),
                        Credit = item.Credit.ToString("#,##0.00", usFormatProvider),
                        Balance = item.Balance,
                        BillNo = item.BillNo
                    };
                    _ListLedger.Add(model);
                }
                SupplierLedgerReport supplierLedgerReport = new SupplierLedgerReport(supplier, _ListLedger, TotalPurchase, TotalCashIn, lblTotalBalance.Content.ToString(), PreviousBalance , StartDate ,EndDate);
                supplierLedgerReport.ShowDialog();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no data to generate report.");
                infobox.ShowDialog();
            }
        }

        //private void BtnCompanyLedger_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        var ListOfPayments = paymentsDatabaseHelper.GetCompanyLedger();
        //        CompanyLedgerListView.ItemsSource = ListOfPayments;
        //        int? totalCredit = ListOfPayments.Sum(s => s.Credit);
        //        int? totalDebit = ListOfPayments.Sum(s => s.Debit);
        //        lblCompanyTotalCredit.Content = totalCredit.ToString();
        //        lblCompanyTotalDebit.Content = totalDebit.ToString();
        //        lblCompanyBalance.Content = (totalCredit - totalDebit).ToString();
        //        CompanyLedgerListView.Items.Refresh();
        //    }
        //    catch (Exception)
        //    {
        //        InfoBox infobox = new InfoBox("Something went wrong,there are no data to generate report.");
        //        infobox.ShowDialog();
        //    }
        //}

        private void BtnCompanySearch_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dateFrom = CompanyFromDatePicker.SelectedDate.Value.Date;
                DateTime dateTo = CompanyToDatePicker.SelectedDate.Value.Date;
                var ListOfPayments = paymentsDatabaseHelper.GetRangeCompanyLedger(dateFrom, dateTo);
                ListOfPayments.Sort((x, y) => x.Date.CompareTo(y.Date));
                List<Models.CompanyLedgerViewModel> companyLedger = new List<CompanyLedgerViewModel>();
                foreach (var item in ListOfPayments)
                {
                    float _credit = (float)item.Credit;
                    float _debit = (float)item.Debit;
                    string _customer = "None";
                    string _supplier = "None";
                    if (item.CustomerID != null)
                    {
                        _customer = item.customers.Name;
                    }
                    if (item.SupplierID != null)
                    {
                        _supplier = item.Suppliers.Name;
                    }
                    Models.CompanyLedgerViewModel model = new CompanyLedgerViewModel
                    {
                        Date = item.Date.ToString(),
                        Customer = _customer,
                        Supplier = _supplier,
                        Description = item.Description,
                        Credit = _credit.ToString("#,##0.00", usFormatProvider),
                        Debit = _debit.ToString("#,##0.00", usFormatProvider)
                    };
                    companyLedger.Add(model);
                }
                CompanyLedgerListView.ItemsSource = companyLedger;
                float totalCredit = (float)ListOfPayments.Sum(s => s.Credit);
                float totalDebit = (float)ListOfPayments.Sum(s => s.Debit);
                lblCompanyTotalCredit.Content = totalCredit.ToString("#,##0.00", usFormatProvider);
                lblCompanyTotalDebit.Content = totalDebit.ToString("#,##0.00", usFormatProvider);
                lblCompanyBalance.Content = (totalCredit - totalDebit).ToString("#,##0.00", usFormatProvider);
                CompanyLedgerListView.Items.Refresh();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no data to generate report.");
                infobox.ShowDialog();
            }
        }

        private void CompanyLedger() 
        {
            try
            {
                var ListOfPayments = paymentsDatabaseHelper.GetCompanyLedger();
                CompanyLedgerListView.ItemsSource = ListOfPayments;
                float? totalCredit = ListOfPayments.Sum(s => s.Credit);
                float? totalDebit = ListOfPayments.Sum(s => s.Debit);
                lblCompanyTotalCredit.Content = totalCredit.ToString();
                lblCompanyTotalDebit.Content = totalDebit.ToString();
                lblCompanyBalance.Content = (totalCredit - totalDebit).ToString();
                CompanyLedgerListView.Items.Refresh();
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("Something went wrong,there are no data to generate report.");
                infobox.ShowDialog();
            }
        }

        private void CompanyLedger_Initialized(object sender, EventArgs e)
        {
           
        }

        private void btnGridCheckSales_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                var payment = CustomerLedgerListView.SelectedItem as Models.DummyModels.Ledger_Customer_Supplier;
                if (payment.BillNo != "None" && payment.ID == 00)
                {
                    Entities.Sale sale = new Entities.Sale();
                    sale = saleDatabaseHelper.GetSale(payment.BillNo);
                    ViewSaleDetails viewSaleDetails = new ViewSaleDetails(sale, true);
                    viewSaleDetails.ShowDialog();

                }

                else if (payment.BillNo == "None" && payment.ID != 00)
                {
                    var model = paymentsDatabaseHelper.GetPayment(payment.ID);
                    if (model.Credit == 0) 
                    {
                        New_Cash_OUT nc = new New_Cash_OUT(model, true);
                        nc.ShowDialog();
                    }

                    if (model.Debit == 0)
                    {
                        New_CashINAdvance nc = new New_CashINAdvance(model, true);
                        nc.ShowDialog();
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnGridCheckPurchase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var supplierLedgerView = SupplierLedgerListView.SelectedItem as Models.DummyModels.Ledger_Customer_Supplier;
                if (supplierLedgerView.BillNo != "None")
                {
                    Entities.Purchase purchase = new Entities.Purchase();
                    purchase = purchaseDatabaseHelper.GetPurchase(supplierLedgerView.BillNo);
                    ViewPurchaseDetails viewPurchaseDetails = new ViewPurchaseDetails(purchase);
                    viewPurchaseDetails.ShowDialog();
                }
                else if (supplierLedgerView.BillNo == "None" && supplierLedgerView.ID != 00)
                {
                    var model = paymentsDatabaseHelper.GetPayment(supplierLedgerView.ID);
                    if (model.Credit == 0)
                    {
                        New_Cash_OUT nc = new New_Cash_OUT(model, false);
                        nc.ShowDialog();
                    }

                    if (model.Debit == 0)
                    {
                        New_CashINAdvance nc = new New_CashINAdvance(model, false);
                        nc.ShowDialog();
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void CustomerLedgerCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetCustomerLedger();
        }

        private void BtnCustomerSearchDateBetween_Click(object sender, RoutedEventArgs e)
        {
            GetDateBetweenCustomerLedger();
        }

        private void SupplierLedgerCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GetSupplierLedger();
        }

        private void BtnSupplierDateBetweenLedger_Click(object sender, RoutedEventArgs e)
        {
            GetDateBetweenSupplierLedger();
        }

        private void SupplierLedgerCombo_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                var supplierList = supplierDatabaseHelper.GetSuppliers().ToList();
                supplierList.Sort((x, y) => string.Compare(x.Name, y.Name));
                SupplierLedgerCombo.ItemsSource = supplierList;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("No Supplier Selected.");
                infobox.ShowDialog();
            }
        }

        private void CustomerLedgerCombo_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                customerList.Clear();
                customerList = customerDatabaseHelper.GetCustomers();
                customerList.Sort((x, y) => string.Compare(x.Name, y.Name));
                CustomerLedgerCombo.ItemsSource = customerList;
            }
            catch (Exception)
            {
                InfoBox infobox = new InfoBox("No Customer Selected.");
                infobox.ShowDialog();
            }
        }

        private void btnSuuplierLedgerExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.CSVHelper cSVHelper = new CSVHelper();
                var person = SupplierLedgerCombo.SelectedItem as Entities.Suppliers;
                cSVHelper.CreateSupplierLedgerCSV(person.Name, _supplierLedgersModels);
                InfoBox infobox = new InfoBox(person.Name + "'s Ledger exported to Excel successfully.");
                infobox.ShowDialog();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void btnCustomerLedgerExport_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                Models.CSVHelper cSVHelper = new CSVHelper();
                var person = CustomerLedgerCombo.SelectedItem as Entities.Customers;

                cSVHelper.CreateCustomerLedgerCSV(person.Name, _customerLedgersModels);
                InfoBox infobox = new InfoBox(person.Name + "'s Ledger exported to Excel successfully.");
                infobox.ShowDialog();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        private void btnGridDeletePayment_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void btnGridDeleteCustomerPayment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var payment = CustomerLedgerListView.SelectedItem as Models.DummyModels.Ledger_Customer_Supplier;
                if (payment.ID != 00) 
                {
                    var model = paymentsDatabaseHelper.GetPayment(payment.ID);
                    Confirmation confirmation = new Confirmation(model.Id);
                    confirmation.ShowDialog();
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnGridSupplierPaymentDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var payment = SupplierLedgerListView.SelectedItem as Models.DummyModels.Ledger_Customer_Supplier;
                if (payment.ID != 00)
                {
                    var model = paymentsDatabaseHelper.GetPayment(payment.ID);
                    Confirmation confirmation = new Confirmation(model.Id);
                    confirmation.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnSupplierRefresh_Click(object sender, RoutedEventArgs e)
        {
            GetSupplierLedger();
        }

        private void btnCustomerRefresh_Click(object sender, RoutedEventArgs e)
        {
            GetCustomerLedger();
        }
    }
}
